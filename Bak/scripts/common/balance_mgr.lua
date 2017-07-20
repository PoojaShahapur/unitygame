local M = {}

--local svrid_syncok = {}--key:svrid,value:true代表服务器连接后,同步ok了该服务器的负载,
local log = require("log"):new("balance_mgr")
local serpent = require('serpent')
local rr = require("remote_run.remote_runner")
local pb = require("protobuf")
local session_user_mgr = require("session_user_mgr")
local http_client = require("httpclient")

-- 负载均衡依靠table: svrid2fullroom,该列表表示了每个服务器满人房间的个数
-- 变量 min_stress_svrid 代表负载最小的服务器
-- 每次玩家请求进入游戏,都将玩家分配到 min_stress_svrid
-- 每次某服务器有个人满的房间,就通知 session,满人房间+1(若中途有人离去,仍算满人房间).更新下负载;
-- 每次房间解散时,判断当初如果通知过session人满+1,现在就通知session房间解散,人满-1
local svrid2fullroom = {}
local min_stress_svrid = 0

local svrid2purgatory = {}
local min_purgatory_svrid = 0
-- 选择加入战斗时候,从该 table 中找到svrid,roomid
local account2room = {}
-- key:svrid, value:table(key:roomid, value:登录该房间的玩家列表)
-- 这个列表是为了加快索引速度用的
local svrid2roomaccs = {}
local purgatory_count = 0
local purgatory_limit = 100
local table_insert = table.insert
local table_remove = table.remove

function M.get_min_stress_roomid(mode)
    if mode ==1 then
        log:info('purcount=%d,limit=%d', purgatory_count, purgatory_limit)
        if purgatory_count < purgatory_limit then
            return min_purgatory_svrid
        else
            return 0
        end
    else    
        return min_stress_svrid
    end
end

function M.get_allaccinfo()
    return svrid2roomaccs
end

function M.get_accountroom()
    return account2room
end

function M.get_purgatory()
    return purgatory_count
end

function M.add_to_acclist(acc, svrid, roomid)
    if svrid2roomaccs[svrid] == nil then
        svrid2roomaccs[svrid] = {}
    end
    
    if svrid2roomaccs[svrid][roomid] == nil then
        svrid2roomaccs[svrid][roomid] = {}
    end

    svrid2roomaccs[svrid][roomid][acc] = 1
end

function M.enter_room_ok(account, uid, svrid, roomid)
    local data  = account2room[account]
    if data ~= nil then
        log:error("%s 进入房间时,已有重复数据,svrid=%d,roomid=%d", account, data.svrid, data.roomid)
    end

    account2room[account] = {svrid = svrid, roomid = roomid}
    M.add_to_acclist(account, svrid, roomid)
    session_user_mgr.on_enter_room(uid)
end

function M.leave_room(acc, svrid, roomid, uid)
    log:info('svrid=%d,acc=%s leave roomid=%d,uid=%d', svrid, acc, roomid, uid)
    if account2room[acc] == nil then
        log:error("%s leave room,no data", acc)
    end
    account2room[acc] = nil

    if svrid2roomaccs[svrid] ~= nil and svrid2roomaccs[svrid][roomid] ~= nil then
        svrid2roomaccs[svrid][roomid][acc] = nil
        local room_people_count = 0
        for k,v in pairs(svrid2roomaccs[svrid][roomid]) do
            room_people_count = room_people_count + 1
        end
        if room_people_count == 0 then
            svrid2roomaccs[svrid][roomid] = nil
        end
    end

    session_user_mgr.on_leave_room(uid)
end

function M.get_room_by_account(account)
    local data  = account2room[account]
    if data == nil then
        return 0,0
    else
        return data.svrid, data.roomid
    end
end

function M.add_fullroom(svrid, is_purgatory)
    local tbl = svrid2fullroom
    if is_purgatory == 1 then
        tbl = svrid2purgatory
    end
    local old_full = tbl[svrid]
    if old_full == nil then
        old_full = 0
    end

    tbl[svrid] = old_full + 1
    if is_purgatory == 1 then
        purgatory_count = purgatory_count + 1
    end
    M.calc_min_stress()
    log:info("svrid=%d 增加满员房间为 %d,目前最小负载svrid=%d", svrid, old_full + 1, min_stress_svrid)
    return true
end

function M.remove_fullroom(svrid, roomid, is_purgatory)
    local tbl = svrid2fullroom
    if is_purgatory == 1 then
        tbl = svrid2purgatory
        purgatory_count = purgatory_count - 1
        if purgatory_count < 0 then
            purgatory_count = 0
        end
    end

    -- 减掉计数
    local old_full = tbl[svrid]
    if old_full == nil or old_full < 0 then
        log:error("svrid=%d 移除roomid=%d ,purgatory=%d时,找不到旧的记录", svrid, roomid, is_purgatory)
        return 
    else    
        tbl[svrid] = old_full - 1
        M.calc_min_stress()
    end
    log:info("%d 减少满员房间roomid= %d,目前最小负载svrid=%d", svrid, roomid, min_stress_svrid)
    -- 清空下列所有账号的房间信息
    if svrid2roomaccs[svrid][roomid] ~= nil then
        for k,v in pairs(svrid2roomaccs[svrid][roomid]) do
            account2room[v] = nil
        end
        -- 清空房间信息
        svrid2roomaccs[svrid][roomid] = nil
    end
end

function M.calc_min_stress()
    local minid = 0
    local min_fullroom = math.maxinteger
    for k,v in pairs(svrid2fullroom) do
        if min_fullroom > v then
            minid = k
            min_fullroom = v
        end
    end

    local min_purgatoryid = 0
    local min_purgatory = math.maxinteger
    for k,v in pairs(svrid2purgatory) do
        if min_purgatory > v then
            min_purgatory = v
            min_purgatoryid = k
        end
    end

    min_stress_svrid = minid
    min_purgatory_svrid = min_purgatoryid
end

-- 服务器断开连接,该服务器负载全部清除,并设置最小负载svrid
function M.server_disconnected(svrid)
    svrid2fullroom[svrid] = nil
    svrid2purgatory[svrid] = nil
    svrid2roomaccs[svrid] = nil
    if min_stress_svrid == svrid or min_purgatory_svrid == svrid then
        M.calc_min_stress()
    end
    log:info("svrid=%d 断开链接,清除所有 stress 记录", svrid)
end

function M.server_connected(svrid)
    if c_util.get_my_svr_id() == svrid then
        return
    end
    log:info("svrid=%d 链接,开始要 stress 数据", svrid)
    rr.run_mfa(svrid, "plane.room_mgr", "give_session_stress_data", {},
        function(full_room_count)
            svrid2fullroom[svrid] = full_room_count
            svrid2purgatory[svrid] = 0
            M.calc_min_stress()
            log:info("svrid=%d full room count=%d", svrid, full_room_count)
        end)
end

return M
