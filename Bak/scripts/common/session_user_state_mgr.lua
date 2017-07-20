local M = {}
--[[
1. 给定一个uid,知道该uid是 在线，离线，战斗中(svrid,roomid)，匹配中,能统计在线人数.
2. 玩家想进入房间,根据模式能计算出最小负载服务器
3. 处理玩家进入房间请求
4. 定时向平台报告在线人数

进入房间的流程:
1. 把svrid,rpcid,acc,uid,nickname,mode,skinid,bullet,level 发给 session 请求进房间
2. session找到负载最小的svrid,将用户数据发过去,找个房间创建Player,创建成功后，session存下来 svrid2rooms[svrid] = {[2] = {uid}}
3. 通知玩家所在gateway,玩家去断线重连 svrid ,svrid上的room_mgr根据game_clt_str 找到房间,返回EnterRoomResponse

加入房间流程:
多了一步根据uid查找房间的流程,其他不变
--]]

-- 目前SessionUser中存放了玩家的状态,网关id,队伍id
-- key:uid; value:SessionUser
local session_users = {}
local online_count = 0
-- key:uid, value:{svrid=1,roomid=2, mode = 0}
local uid2roominfo = {}
-- key:svrid, value : {{roomid=1, all_players= {}, count = 0}}
local svrid2rooms = {}
local log = require("log"):new("session_user_mgr")
local rr = require("remote_run.remote_runner")
local serpent = require('serpent')
local config = require("config")
local new_login_mgr = require("new_login_mgr")
local SessionUser = require("session_user")
local Friendm = require("friend_mgr")

State_Offline = 0   --默认,离线状态
State_Online = 1    --在线
State_Fight = 2     --战斗
State_Match = 3     --匹配

RoomMode_Normal = 0     -- 普通模式
RoomMode_Purgatory = 1  -- 炼狱模式
RoomMode_Team = 2       -- 团队模式

function M.online(svrid, uid)
    if session_users[uid] ~= nil then
        log:error("玩家上线时候,session已有uid=%d,svrid=%d玩家数据", uid, svrid)
    end

    local u = SessionUser:new(svrid)
    u.friendm = Friendm:new(uid)
    u.friendm:online(session_users)
    session_users[uid] = u
    online_count = online_count + 1
    if uid2roominfo[uid] then
        rr.run_mfa(svrid, "user_mgr", "notify_player_reenter_game", {uid}) 
    end
end

function M.offline(openid, account, uid)
    log:info("%d,%s,openid=%s下线",  uid, account, openid)
    require("team_mgr").leave_team(uid)
    new_login_mgr.offline(openid)
    session_users[uid].friendm:offline(session_users)
    session_users[uid] = nil
    online_count = online_count - 1
end

-- 获取负载最小服务器id
-- 遍历每个服务器,获取人满的房间个数,最小的人满房间即为负载最小
-- 这里要区分模式,不能因为炼狱模式进了一个玩家,导致该服务器负载提高,玩家被分配到其他服务器
function M.get_min_stress_svrid(mode)
    local svrid2fullroom = {}
    local min_stress_svrid = 0
    local min_fullroom_count = math.maxinteger
    for svrid, rooms in pairs(svrid2rooms) do
        local this_server_fullroom = 0
        for _, room in pairs(rooms) do
            if mode == room.mode then
                if mode == RoomMode_Purgatory then
                    this_server_fullroom = this_server_fullroom + 1
                else   
                    if room.count >= config.room.hold_num then
                        this_server_fullroom = this_server_fullroom + 1
                    end
                end
            end
        end
        svrid2fullroom[svrid] = this_server_fullroom
        if this_server_fullroom < min_fullroom_count then
            min_stress_svrid = svrid
            min_fullroom_count = this_server_fullroom
        end
    end

    return min_stress_svrid
end

function M.req_reconnect_room(uid)
    log:debug("req_reconnect_room, uid=%d", uid)
    local user = session_users[uid]
    if not user then
        return
    end
    local roominfo = uid2roominfo[uid]
    -- 找不到房间,说明本次比赛结束了,通知客户端将断线重连界面消失掉
    if not roominfo then
        rr.run_mfa(user.gateway_id, "user_mgr", "notify_no_game", {uid}) 
        return
    end

    if roominfo.mode == RoomMode_Normal then
        rr.run_mfa(user.gateway_id, "user_mgr", "reconnect_room", {uid, roominfo.svrid, roominfo.roomid}) 
    elseif roominfo.mode == RoomMode_Team then
        rr.run_mfa(user.gateway_id, "user_mgr", "connect_team_room", {uid, roominfo.svrid, roominfo.roomid}) 
    else
        log:error("uid=%d玩家请求断线重连时,找不到房间信息")
    end
end

-- 玩家请求进入房间
function M.enter_room(mode, account, uid, nickname, skinid, bulletid)
    local roominfo = uid2roominfo[uid]
    if roominfo then
        local user = session_users[uid]
        if user then
            rr.run_mfa(user.gateway_id, "user_mgr", "reconnect_room", {uid, roominfo.svrid, roominfo.roomid, 0}) 
        end
        return
    end

    local min_stress_svrid = M.get_min_stress_svrid(mode)
    log:debug("session enterroom, min_stress_svrid=%d", min_stress_svrid)
    rr.run_mfa(min_stress_svrid, "plane.room_mgr", "enter_room"
    , {0, 0, account, uid, nickname, mode, skinid, bulletid}, function (roomid)
        M.set_uid2roominfo(uid, min_stress_svrid, roomid, RoomMode_Normal)

        -- 通知玩家所在网关,去连房间服
        local user = session_users[uid]
        if user then
            rr.run_mfa(user.gateway_id, "user_mgr", "reconnect_room", {uid, min_stress_svrid, roomid, 0}) 
        end
    end)
end

function M.set_uid2roominfo(uid, svrid, roomid, mode)
        if svrid2rooms[svrid] == nil then
            svrid2rooms[svrid] = {}
        end
        if svrid2rooms[svrid][roomid] == nil then
            svrid2rooms[svrid][roomid] = {}
        end
        if svrid2rooms[svrid][roomid].all_players == nil then
            svrid2rooms[svrid][roomid].all_players = {}
            svrid2rooms[svrid][roomid].count = 0
        end
        table.insert(svrid2rooms[svrid][roomid].all_players, uid)
        svrid2rooms[svrid][roomid].count = svrid2rooms[svrid][roomid].count + 1
        uid2roominfo[uid] = {svrid = svrid, roomid = roomid, mode = mode}
end

function M.destroy_room(svrid, roomid)
    log:debug("房间时间到, svrid=%d,roomid=%d,roominfo=%s", svrid, roomid, serpent.block(svrid2rooms))
    for k,playeruid in pairs(svrid2rooms[svrid][roomid].all_players) do
        uid2roominfo[playeruid] = nil
        M.on_leave_room(playeruid)
    end

    svrid2rooms[svrid][roomid] = nil
end

-- 请求加入房间
function M.req_join_room(basesvrid, rpcid, dest_uid, account, uid, nickname, skinid, bulletid)
    log:debug("req_join_normal_room, uid=%d,destuid=%d", uid, dest_uid)
    local user = session_users[uid]
    if not user then
        return
    end

    -- 该玩家已经在战斗中了
    if uid2roominfo[uid] then
        return
    end

    local roominfo = uid2roominfo[dest_uid]
    log:debug("%s", serpent.block(roominfo))
    if not roominfo then
        return '该玩家已不在房间,无法加入战斗'
    end
    if roominfo.mode == RoomMode_Purgatory then
        return '该玩家正在炼狱模式,无法加入战斗'
    -- 普通模式,直接发请求到svrid,roomid
    elseif roominfo.mode == RoomMode_Normal then
        rr.run_mfa(roominfo.svrid, "plane.room_mgr", "req_join_normal_room"
            , {basesvrid, rpcid, uid, roominfo.roomid, account, nickname, skinid, bulletid}
            ,function (retcode, uid, svrid, roomid)
                if retcode == 0 then
                    M.set_uid2roominfo(uid, svrid, roomid, RoomMode_Normal)
                end
            end) 
    else
    -- 团战模式,先创建一个临时队伍,发往目标服务器
        require("team_mgr").temp_team_join_room(basesvrid, rpcid, dest_uid, account
            , uid, nickname, bulletid, roominfo.svrid, roominfo.roomid)
    end
end

function M.on_enter_room(uid)
    session_users[uid].state = State_Fight
end

-- 如果是直接关闭客户端下线,就找不到session_user,因为offline先被调用..
-- 直连服直接发offline(),远程服务器后面再发退出房间消息
function M.on_leave_room(uid)
    local session_user = session_users[uid]
    if session_user then
        session_user.state = State_Online
    end
end

function M.on_enter_team(uid)
    session_users[uid].state = State_Match
end

function M.on_leave_team(uid)
    session_users[uid].state = State_Online
end

function M.get_user(uid)
    if session_users[uid] == nil then
        return nil
    end

    return session_users[uid]
end

function M.get_user_state(uid)
    if session_users[uid] == nil then
        return State_Offline
    end

    return session_users[uid].state
end

function M.get_user_roominfo(uid)
    return uid2roominfo[uid]
end

-- 服务器断开连接,该服务器负载全部清除,并设置最小负载svrid
-- 清除是为了玩家在其他网关进程能登录游戏
function M.server_disconnected(svrid)
    if c_util.get_my_svr_id() == svrid then
        return
    end
    local rooms = svrid2rooms[svrid]
    if not rooms then
        return
    end
    for _,room in pairs(rooms) do
        for k, uid in pairs(room.all_players) do
            uid2roominfo[uid] = nil
            session_users[uid] = nil
        end
    end

    log:info("svrid=%d 断开链接,清除所有 stress 记录", svrid)
end

-- 服务器启动时候,重新计算负载
function M.server_connected(svrid)
    if c_util.get_my_svr_id() == svrid then
        return
    end
    log:info("svrid=%d 链接,开始要 stress 数据%s", svrid, serpent.block(svrid2rooms))
    svrid2rooms[svrid] = {}
end

function M.get_page_x_friend_uids(uid, page)
    local u = session_users[uid]
    if not u then
        log:error("uid=%d 的玩家不在线竟然请求好友数据", uid)
        return nil
    end

    local uids = u.friendm:get_page_x_uids(page)
    if #uids <= 0 then
        return nil
    end

    local ret = {}
    -- 为啥不用字典?保证顺序,在线好友排在前面
    --log:debug("page_x,%s", serpent.block(session_users))
    for i = 1,#uids do
        local one_friend_info = {
            uid = uids[i],
            state = 0,
        }
        local tmp = session_users[uids[i]]
        if tmp then
            one_friend_info.state = tmp.state
        end
        table.insert(ret, one_friend_info)
    end

    return ret, u.friendm.total_num
end

-- add_friend_uid 为新添加好友的uid
function M.on_add_friend(uid, add_friend_uid)
    log:debug("session收到添加好友请求,%d添加%d为好友", uid, add_friend_uid)
    local user = session_users[uid]
    if not user then
        return
    end

    local is_online = 0
    local friend_user = session_users[add_friend_uid]
    if friend_user then
        is_online = 1
    end
    user.friendm:on_add_friend(add_friend_uid, is_online)
    log:debug("%d,friendm=%s", uid, serpent.block(friendm))
end

function M.on_remove_friend(uid, remove_friend_uid)
    log:debug("session收到移除好友请求,%d移除%d为好友", uid, remove_friend_uid)
    local user = session_users[uid]
    if not user then
        return
    end

    user.friendm:on_remove_friend(remove_friend_uid)
end

return M
