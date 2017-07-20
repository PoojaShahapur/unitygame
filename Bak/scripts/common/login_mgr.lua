local M = {}
--多个网关的玩家,只能登陆一次 
--有openid登陆过,就存下来openid对应的svrid
--若该玩家已登陆过,再次请求登录,发消息通知旧网关下线,等待旧网关下线成功后通知新网关
--local users = {}--所有玩家的列表,key:openid, value:svrid
local openid2svrid = {}--key:openid, value:该openid登录的svrid
local svrid2count = {}--每个服务器人数统计,key:svrid,value:人数
local svrid_syncok = {}--key:svrid,value:true代表服务器连接后,同步ok了该服务器的负载,
local log = require("log"):new("login_mgr")
local balance_mgr = require("balance_mgr")
local session_user_state_mgr = require("session_user_state_mgr")
local serpent = require('serpent')
local rr = require("remote_run.remote_runner")
local pb = require("protobuf")
local http_client = require("httpclient")
local config = require("config")
local time = require("time")
local mdb = require("database")
local timer_queue = c_timer_queue.CTimerQueue()
local sessionid = 1

local last_timestamp = os.time()
local login_fail_str = assert(pb.encode("svr.CanILoginResponse", {ok = 0}))
local login_success_str = assert(pb.encode("svr.CanILoginResponse", {ok = 1}))

function M.can_i_login(ctx, svrid, openid)
    log:info("check login,svrid=%d,%s", svrid, openid)
    -- 没有同步完该服务器的负载,不能登录
    if svrid_syncok[svrid] ~= true then
        log:info("login failed,svrid=%d,%s", svrid, openid)
	    c_rpc.reply_to(ctx, login_fail_str)
    end

    if openid2svrid[openid] == nil then
        M.add_svr_count(svrid)
        log:info("login success,svrid=%d,%s", svrid, openid)
	    c_rpc.reply_to(ctx, login_success_str)
        openid2svrid[openid] = svrid
    else
        -- 通知旧的网关把玩家踢了,踢成功了回复网关可以登录
        rr.run_mfa(openid2svrid[openid], "user_mgr", "erase_by_openid", {openid},
            function(ret)
                if ret then
                    openid2svrid[openid] = svrid
	                c_rpc.reply_to(ctx, login_success_str)
                end
            end)
    end
end

function M.add_svr_count(svrid)
    local old_count = svrid2count[svrid]
    if old_count == nil then
        old_count = 0
    end
    svrid2count[svrid] = old_count + 1
    log:info('svrid:%d,在线人数%d', svrid, svrid2count[svrid])
end

function M.logout(svrid, openid, acc, uid)
    log:info("logout,svrid=%d,%s", svrid, openid)
    svrid2count[svrid] = svrid2count[svrid] - 1
    if svrid2count[svrid] < 0 then
        log:error("%s,%s 下线时,svrid=%d 服务器在线人数为0",openid, acc,svrid)
        svrid2count[svrid] = 0
    end
    openid2svrid[openid] = nil
    session_user_state_mgr.offline(uid)
end

-- run_mfa 没法
function M.sync_login_data(svrid, datastr)
    local all_openids = assert(pb.decode("svr.AllOpenids", datastr))
    local num = 0
    log:info("服务器启动同步登录数据, start,svrid=%d", svrid)
    for k,v in pairs(all_openids.openids) do
        openid2svrid[v] = svrid
        num = num + 1
        log:info("sync login,%s", v)
    end
    log:info("服务器启动同步登录数据, end,svrid=%d", svrid)
    svrid2count[svrid] = num
    if all_openids.is_sync_complete == 1 then
        svrid_syncok[svrid] = true
    end
end

function M.check_uids_state(all_uids)
    local all_uid_state = {
    }

    for i = 1,#all_uids do
        local uid = all_uids[i]
        local uid_state = {
            uid = uid,
            state = session_user_state_mgr.get_user_state(uid)
        }
        table.insert(all_uid_state, uid_state)
    end

    --return pb.encode("svr.AllUidStateMsg", all_uid_state)
    return all_uid_state
end

-- 服务器断开连接
function M.server_disconnected(svrid)
    svrid2count[svrid] = 0
    local need_erase_openid = {}
    for k,v in pairs(openid2svrid) do
        if v == svrid then
            table.insert(need_erase_openid, k)
        end
    end
    for i = 1,#need_erase_openid do
        openid2svrid[need_erase_openid[i]] = nil
    end
    log:info("svrid=%d 断开链接,清除所有登录记录", svrid)
end

function M.server_connected(svrid)
    if c_util.get_my_svr_id() == svrid then
        return
    end
    log:info("svrid=%d 链接,开始要数据", svrid)
    svrid_syncok[svrid] = false
    rr.run_mfa(svrid, "user_mgr", "give_session_login_data", {},
        function(datastr)
            M.sync_login_data(svrid, datastr)
        end)
end

function M.init()
    if c_util.get_my_svr_id() ~= sessionid then
        return
    end
    -- 平台说最好1分钟1次,省得破坏数据
    timer_queue:insert_repeat_now(60*1000, report_gasdk_online_count)
    timer_queue:insert_repeat_now(5*1000, check_daily_zero)
end

---------
-- 全局函数

function check_daily_zero()
    local now = os.time()
    if time.is_two_time_in_different_day(last_timestamp, now) then
    --if true then
        -- 遍历排行榜发奖励
        local yesterday = os.date("%y%m%d", last_timestamp)
        --local yesterday = os.date("%y%m%d", now)
	    c_redis.command("zrevrange", "rankpur:" .. yesterday, {0, config.purgatory_rank_reward_num, }
            , function(reply)
                if 2 == reply.type and #reply.elements > 0 then
                    local all_uid_str = ''
                    local order_uids = {}
                    for i = 1, #reply.elements do
                        table.insert(order_uids, tonumber(reply.elements[i].str))
                    end
    
                    for i = 1,#order_uids do
                        local reward = config.purgatory_rankreward[i]
                        if reward > 0 then
                            -- 查下这个玩家的openid,根据openid找到玩家所在网关
                            -- 如果找到了,直接发奖励;如果找不到,扔数据库里,下次上线user去拿奖励
                        mdb:query_b(mdb.db, mdb.collection, {uid = order_uids[i]}, {_id = 1}, function(results)
                            if #results > 0 then
                                local info = mdb:get_table(results[1])
                                local openid = info['_id']
                                if openid2svrid[openid] ~= nil then
                                    rr.run_mfa(openid2svrid[openid], "user_mgr", "give_purgatory_reward", {order_uids[i], reward})
                                    log:info('通知svrid=%u给uid=%d发放炼狱奖励=%d', openid2svrid[openid], order_uids[i], reward)
                                else
                                    mdb:update_b(mdb.db, mdb.collection, '{"uid" : ' .. order_uids[i] .. '}', "$inc"
                                    , {purgatory_reward = reward }, function(result)
                                        log:info('uid=%d不在线,数据库中增加炼狱奖励=%d', order_uids[i], reward)
                                    end)
                                end
                            end
                        end)
                        end
                    end
                else
                    log:error("零点发奖励,zrevrange,return error type=%d,integer=%d,str=%s", reply.type,reply.integer, reply.str)
                end
            end)
    end

    last_timestamp = now
end


-- 向巨人sdk报告人数,注意该函数没带 M.
function report_gasdk_online_count()
    local total = 0
    for _,v in pairs(svrid2count) do
        total = total + v
    end
    log:info('report total online=%d', total)
    local login_key = "fbf9a68914fdf172737cd72b547d508c"
    local game_id = 5187
    local zone_id = 1
    local channel_id = 0 -- 渠道ID
    local sign = c_util.md5(""..game_id.."&"..total.."&"..zone_id.."&"..login_key)
    local url = string.format("http://stat.mztgame.com/game/online?game_id=%d&zone_id=%d&number=%d&channel_id=%d&sign=%s",game_id,zone_id,total,channel_id,sign)
    http_client:get(url, function (result)            
	        log:info("report_global_count, result:"..result)
        end)

        -- 输出负载情况
        for svrid, rooms in pairs(balance_mgr.get_allaccinfo()) do
            local room_count = 0
            for _,v in pairs(rooms) do
                room_count = room_count + 1
            end
            log:info('服务器负载统计,svrid=%d 房间个数=%d', svrid, room_count)
        end

        local player_count = 0
        for _,v in pairs(balance_mgr.get_accountroom()) do
            player_count = player_count + 1
        end
        log:info('当前房间内玩家一共%d个,炼狱玩家%d个', player_count, balance_mgr.get_purgatory())
end

return M
