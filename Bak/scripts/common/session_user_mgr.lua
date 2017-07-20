local M = {}
-- 目前SessionUser中存放了玩家的状态,网关id,队伍id
-- key:uid; value:SessionUser
local session_users = {}
local log = require("log"):new("session_user_mgr")

State_Offline = 0   --默认,离线状态
State_Online = 1    --在线
State_Fight = 2     --战斗
State_Match = 3     --匹配

function M.new_session_user(svrid)
    return {
        state = State_Online,
        gateway_id = svrid,
        teamid = 0,
    }
end

function M.online(svrid, uid)
    if session_users[uid] ~= nil then
        log:error("玩家上线时候,session已有uid=%d,svrid=%d玩家数据", uid, svrid)
    end

    session_users[uid] = M.new_session_user(svrid)
end

function M.offline(uid)
    require("team_mgr").leave_team(uid)
    log:error("uid=%d offline", uid)
    session_users[uid] = nil
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

return M
