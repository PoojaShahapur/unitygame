local M = {}

local id2user = {}
local account2user = {}
local openid2user = {}
local uid2user = {}
local log = require("log"):new("user_mgr")
local pb = require("protobuf")
local count = 0
local empty_msg_str = pb.encode("rpc.EmptyMsg", { })
local rr = require("remote_run.remote_runner")
local router = require("rpc.base_rpc_router")

-- return false is already exist
function M.insert(user)
	assert(user)
	local rpc_clt_id = user:get_rpc_clt_id()
	assert(rpc_clt_id)
	if id2user[rpc_clt_id] then
		return false
	end
	local account = user.account
	assert(account)
	if account2user[account] then
		return false
	end

    local openid = user.openid
	assert(openid)
    if openid2user[openid] then
        return false
    end

    assert(user.uid > 0)  
    if uid2user[user.uid] then
        return false
    end

	id2user[rpc_clt_id] = user
	account2user[account] = user
    openid2user[openid] = user
    uid2user[user.uid] = user
	count = count + 1
	log:debug("Inserted. rpc_clt_id=%u, acc=%s, now_count=%d, uid=%d", rpc_clt_id, account, count, user.uid)
	return true
end  -- insert()

function M.erase(rpc_clt_id)
	assert("number" == type(rpc_clt_id))
	if not rpc_clt_id then return end
	local user = id2user[rpc_clt_id]
	if not user then return end

    user:offline()
	user.timer_queue:erase_all();  -- 主动清定时器，因为user对象可能会延时删除
    local user_account = user.account
	id2user[rpc_clt_id] = nil
	account2user[user.account] = nil
    openid2user[user.openid] = nil
    uid2user[user.uid] = nil
	count = count - 1
	log:info("Erased. rpc_clt_id=%u, acc=%s,now_count=%d", rpc_clt_id, user_account,count)
end  -- erase()

function M.erase_by_openid(openid)
    local user = openid2user[openid]
    -- 找不到该openid的用户,说明玩家不在我这边了,跟session说可以登录啦
    if not user then return true end
    user:popMessageBox("您的账号在别处登录,您已被迫下线!")
    c_util.disconnect_game_client(user:get_rpc_clt_id())
    return true
end

function M.replace(newuser)
    assert(nil ~= newuser)
    local rpc_clt_id = newuser:get_rpc_clt_id()
    M.erase(rpc_clt_id)
    id2user[rpc_clt_id] = newuser
    account2user[newuser.account] = newuser
    openid2user[newuser.openid] = newuser
    uid2user[newuser.uid] = newuser
    count = count + 1
	log:debug("Replaced. rpc_clt_id=%u, to new acc=%s", rpc_clt_id, newuser.account)
end

--[[
function M.erase(account)
	if not account then return end
	local user = account2user[account]
	if not user then return end
	id2user[account] = nil
	account2user[user:get_rpc_clt_id()] = nil
	count = count - 1
	log:info("Erased. rpc_clt_id=%u, now_count=%d", rpc_clt_id, count)
end  -- erase()
]]

function M.get_user_by_account(account)
    assert("string" == type(account))
    if not account then return nil end
    return account2user[account]
end

function M.get_user(rpc_clt_id)
	assert("number" == type(rpc_clt_id))
	if not rpc_clt_id then return nil end
	return id2user[rpc_clt_id]
end

function  M.get_user_account(rpc_clt_id)
    assert("number" == type(rpc_clt_id))
    if not rpc_clt_id then return nil end
    local user = id2user[rpc_clt_id]
    if not user then return end
    return user.account
end

function M.get_rpc_clt_id_by_openid(openid)
	if not openid then return end
	local user = openid2user[openid]
	if not user then return end
	return user:get_rpc_clt_id()
end

function M.get_user_by_uid(uid)
	if not uid then return end
    return uid2user[uid]
end

function M.get_rpc_clt_id(account)
	if not account then return end
	local user = account2user[account]
	if not user then return end
	return user:get_rpc_clt_id()
end

-- 玩家退出房间了,可以操作与钱相关的变量了
function M.set_user_can_op_money(rpc_clt_id)
    local user = id2user[rpc_clt_id]
    if user then
        log:debug("set_user_can_op_money %u", rpc_clt_id)
        user.can_op_money = 1
    end
end

function M.get_count()
	return count
end

-- 将我这里记录的登录信息发送给session
function M.give_session_login_data()
    local all_openids = {
        is_sync_complete = 1,
        openids = {},
    }
    local allopenid = all_openids.openids
    for k,v in pairs(id2user) do
        table.insert(allopenid, v.openid)
    end

    local ret_str = assert(pb.encode("svr.AllOpenids", all_openids))
    return ret_str
end

function M.give_reward(rpc_clt_id, money, sugar, need_add_champion, destroynum,killnum,score,highest_combo, unlock_pur, roommode, newlevel, is_mvp)
	local user = M.get_user(rpc_clt_id)
	if not user then
	    log:info("给%d加 %d,%d fail,can not find userid=%d", rpc_clt_id, money, sugar, rpc_clt_id)
        return false
    end

    user:add_money_after_game(money, sugar, need_add_champion, destroynum,killnum,score,highest_combo, unlock_pur, roommode, newlevel, is_mvp)
end

function M.update_pur_score(rpc_clt_id, score)
	local user = M.get_user(rpc_clt_id)
	if not user then
	    log:info("给%d更新炼狱排行榜得分失败 score=%d", rpc_clt_id, score)
        return false
    end

    user:set_pur_highest_score(score)
end

function M.popMessageBox(rpc_clt_id, content)
	local user = M.get_user(rpc_clt_id)
	if not user then
	    log:info("给%d弹框 %sfail", rpc_clt_id, content)
        return false
    end

    user:popMessageBox(content)
end

function M.notify_team_new_member(uid, pos, data)
    log:info('notify_team_new_member, id=%d,pos=%d', uid, pos)
    local user = M.get_user_by_uid(uid)
    if user then
        local add_team = {
            pos = pos,
            member = data
        }
        user:rpc_request("plane.TeamPush", "NotifyAddTeamMember", pb.encode("plane.AddTeamMemberMsg", add_team))
    end
end

function M.notify_remove_team_member(uid, dest_uid, pos)
    log:info('notify_remove_team_member, id=%d,pos=%d', uid, pos)
    local user = M.get_user_by_uid(uid)
    if user then
        user.log:debug("退出")
        local remove = {pos = pos, uid = dest_uid}
        user:rpc_request("plane.TeamPush", "NotifyRemoveTeamMember", pb.encode("plane.RemoveTeamMemberMsg", remove))
    end
end

function M.abadon_team(uid)
    log:info('abadon_team, id=%d', uid)
    local user = M.get_user_by_uid(uid)
    if user then
        --user:popMessageBox("队伍已解散")
        user:rpc_request("plane.TeamPush", "NotifyAbadonTeam", empty_msg_str)
    end
end

function M.change_team_nickname(uid, pos, nickname)
    local user = M.get_user_by_uid(uid)
    if user then
        user.log:debug("pos=%d修改昵称=%s", pos, nickname)
        local msg = {pos = pos, nickname = nickname}
        user:rpc_request("plane.TeamPush", "NotifyMemberNicknameChanged", pb.encode("plane.NewNickNameMsg", msg))
    end
end

function M.ask_agree_join(uid, teamid, inviter_account)
    local user = M.get_user_by_uid(uid)
    if user then
        local msg = {teamid = teamid, inviter_acc = inviter_account}
        user.log:info('%s ask_agree_join,teamid=%d', inviter_account, teamid)
        user:rpc_request("plane.TeamPush", "AskAgreeJoin", pb.encode("plane.InviteMsg", msg))
    end
end

function M.connect_team_room(uid, svrid, roomid)
    local user = M.get_user_by_uid(uid)
    if user then
        user.log:info("房间创建好了,连接svrid=%d,roomid=%d", svrid, roomid)
        rr.run_mfa(svrid, "plane.room_mgr", "reconnect_team_room"
        , {user.game_clt_id.base_svr_id, user.game_clt_id.base_rpc_clt_id, roomid, uid}, function (resp)
        user.log:info("Team收到房间回复,通知客户端,")
        --user:rpc_request("plane.TeamPush", "NotifyEnterRoomResponse", resp)
            rr.run_mfa(1, "session_user_state_mgr", "on_enter_room", {user.uid})
            router.set_svc_dst_svr_id(user.game_clt_id, "plane.Plane", svrid, function()
                user:rpc_request("plane.TeamPush", "NotifyEnterRoomResponse", resp)
            end)
        end)
    end
end

function M.notify_player_reenter_game(uid)
    local user = M.get_user_by_uid(uid)
    if user then
        user:rpc_request("plane.TeamPush", "NotifyPlayerInGame", empty_msg_str)
    end
end

function M.notify_no_game(uid)
    local user = M.get_user_by_uid(uid)
    if user then
        user:rpc_request("plane.TeamPush", "NotifyPlayerNoGame", empty_msg_str)
    end
end

function M.reconnect_room(uid, svrid, roomid)
    local user = M.get_user_by_uid(uid)
    if user then
       user.log:info("普通模式房间创建好了,连接svrid=%d,roomid=%d", svrid, roomid)
       rr.run_mfa(svrid, "plane.room_mgr", "reconnect_room"
       , {user.game_clt_id.base_svr_id, user.game_clt_id.base_rpc_clt_id, roomid, uid}, function (resp)
            user.log:info("收到房间回复,通知客户端,")
            rr.run_mfa(1, "session_user_state_mgr", "on_enter_room", {user.uid})
            router.set_svc_dst_svr_id(user.game_clt_id, "plane.Plane", svrid, function()
                user:rpc_request("plane.TeamPush", "NotifyEnterRoomResponse", resp)
            end)
        end)
    end
end

function M.give_purgatory_reward(uid, reward)
    log:info('purgatory uid=%d', uid)
    local user = M.get_user_by_uid(uid)
    if user then
        user:zero_clock_give_purgatory_reward(reward)
    end
end

-- Called by remote.
function M.get_account_and_nick(rpc_clt_id)
	local user = M.get_user(rpc_clt_id)
	if not user then return false end
	return true, user.account, user.skinm.default_skinid, user.skinm.default_bullet
end  -- get_account_and_nick()

return M
