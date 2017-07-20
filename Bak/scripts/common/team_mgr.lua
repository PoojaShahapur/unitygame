local M = {}
-- todo, 去掉member1,member2,3这些类
-- 队伍所有成员数据集合
local teamdata = {}
local uid2teamid = {}
local cur_teamid = 50
local log = require("log"):new("team_mgr")
local rr = require("remote_run.remote_runner")
local serpent = require("serpent")
local session_user_state_mgr = require("session_user_state_mgr")
local balance_mgr = require("balance_mgr")
local pb = require("protobuf")


function M.create_team(uid, account)
    cur_teamid = cur_teamid + 1
    local team  = {
        teamid = cur_teamid,
        leader_uid = uid,
        team_name = account .. '的队伍',
        total_num = 3,
        current_num = 0,
        voice_token = c_util.gen_objid(),
        member1 = nil,
        member2 = nil,
        member3 = nil
    }

    teamdata[cur_teamid] = team
    return team
end

--[[
 若teamid==0(玩家想创建队伍),且玩家已有队伍,则return玩家没队伍,create_team
若teamid~=0,但该队伍不存在,则return error;否则 join_team
--]]
function M.enter_team(teamid, uid, nickname, account, imgid, imgurl, sex)
    -- 如果玩家想创建队伍
    if teamid == 0 then
        if uid2teamid[uid] then
            log:error("%d,%s,%s请求创建队伍时候,已有一只队伍", uid, account, nickname)
            -- todo,回复pb能用的response格式
            return 0, teamdata[uid2teamid[uid]]
        else
            -- 如果有正在进行的比赛,请你负责到底
            local roominfo = session_user_state_mgr.get_user_roominfo(uid)
            if not roominfo then
                team = M.create_team(uid, account)
                local retcode,data = M.join_team(team, uid, nickname, account, imgid, imgurl, sex)
                return retcode, data
            else
                local user = session_user_state_mgr.get_user(uid)
                if user then
                    rr.run_mfa(user.gateway_id, "user_mgr", "notify_player_reenter_game", {uid}) 
                end
            end
        end
    else
        local team = teamdata[teamid]
        if not team then
            return 1, '您要加入的队伍已解散!'
        else
            local retcode, data = M.join_team(team, uid, nickname, account, imgid, imgurl, sex)
            return retcode, data
        end
    end
end

-- 加入队伍,team为队伍数据table
function M.join_team(team, uid, nickname, account, imgid, imgurl, sex)
    if team.member1 and team.member2 and team.member3 then
        return 1, '该队伍人员已满'
    end
    uid2teamid[uid] = team.teamid
    local new_member_data = {
        uid = uid,
        nickname = nickname,
        account = account,
        imgid = imgid,
        imgurl = imgurl,
        sex = sex
    }
    if not team.member1 then
        M.on_new_member(team, new_member_data, 1)
        team.member1 = new_member_data
    elseif not team.member2 then
        M.on_new_member(team, new_member_data, 2)
        team.member2 = new_member_data
    elseif not team.member3 then
        M.on_new_member(team, new_member_data, 3)
        team.member3 = new_member_data
    end
    team.current_num = team.current_num + 1
    session_user_state_mgr.on_enter_team(uid)
    return 0, M.enter_team_response(team)
end

function M.leave_team(uid)
    log:debug("team_mgr, leave team id=%d", uid)
    local teamid = uid2teamid[uid]
    if not teamid then
        return
    end

    local team = teamdata[teamid]
    if not team then
        return
    end

    if uid == team.leader_uid then
        M.on_abadon_team(team)
        teamdata[teamid] = nil
    else
        M.on_leave_team(team, uid)
        team.current_num = team.current_num - 1
        uid2teamid[uid] = nil
        session_user_state_mgr.on_leave_team(uid)
    end
end

-- 改变该uid的昵称,通知出该玩家外的其他玩家
function M.change_nickname(uid, nickname)
    local teamid = uid2teamid[uid]
    if not teamid then
        return
    end

    local team = teamdata[teamid]
    if not team then
        return
    end

    local pos = 0
    if team.member1 and team.member1.uid == uid then
        team.member1.nickname = nickname
        pos = 1
    end
    if team.member2 and team.member2.uid == uid then
        team.member2.nickname = nickname
        pos = 2
    end
    if team.member3 and team.member3.uid == uid then
        team.member3.nickname = nickname
        pos = 3
    end

    if pos ~= 0 then
        M.on_change_nickname(team, pos, uid, nickname)
    else
        log:error("uid=%d请求更改昵称找不到team", uid)
    end
end


function M.on_change_nickname(team, pos, uid, nickname)
    local uids = M.get_member_uids(team)
    for i = 1,#uids do
        if uids[i] ~= uid then
            local user = session_user_state_mgr.get_user(uids[i])
            rr.run_mfa(user.gateway_id, "user_mgr", "change_team_nickname", {uids[i], pos, nickname})
        end
    end
end

function M.get_member_uids(team)
    local uids = {}
    if team.member1 then
        table.insert(uids, team.member1.uid)
    end
    if team.member2 then
        table.insert(uids, team.member2.uid)
    end
    if team.member3 then
        table.insert(uids, team.member3.uid)
    end

    return uids
end

function M.invite_join_team(inviter_uid, inviter_account,uids)
    log:debug("%d,%s邀请%s", inviter_uid, inviter_account, serpent.block(uids))
    local teamid = uid2teamid[inviter_uid]
    if not teamid then
        log:error("%d,%s邀请%s进入团队,找不到teamid", inviter_uid, inviter_account)
        return
    end

    local team = teamdata[teamid]
    if not team then
        log:error("%d,%s邀请%s进入团队,找不到team", inviter_uid, inviter_account)
        return
    end

    for i = 1,#uids do
        local user = session_user_state_mgr.get_user(uids[i])
        if user and user.state == State_Online then
            rr.run_mfa(user.gateway_id, "user_mgr", "ask_agree_join", {uids[i], team.teamid, inviter_account})
        end
    end
end

function M.on_abadon_team(team, is_after_enter_room)
    local uids = M.get_member_uids(team)
    for i = 1,#uids do
        local user = session_user_state_mgr.get_user(uids[i])
        if not is_after_enter_room then
            rr.run_mfa(user.gateway_id, "user_mgr", "abadon_team", {uids[i]})
        end
        uid2teamid[uids[i]] = nil
        session_user_state_mgr.on_leave_team(uids[i])
        log:debug("队伍解散,teamid=%d,uid=%d", team.teamid, uids[i])
    end
end

function M.on_leave_team(team, uid)
    local pos = 0
    if team.member1 and team.member1.uid == uid then
        team.member1.nickname = nickname
        pos = 1
        team.member1 = nil
    end
    if team.member2 and team.member2.uid == uid then
        team.member2.nickname = nickname
        pos = 2
        team.member2 = nil
    end
    if team.member3 and team.member3.uid == uid then
        team.member3.nickname = nickname
        pos = 3
        team.member3 = nil
    end

    if team.member1 then
        local user = session_user_state_mgr.get_user(team.member1.uid)
        rr.run_mfa(user.gateway_id, "user_mgr", "notify_remove_team_member", {team.member1.uid, uid, pos})
    end
    if team.member2 then
        local user = session_user_state_mgr.get_user(team.member2.uid)
        rr.run_mfa(user.gateway_id, "user_mgr", "notify_remove_team_member", {team.member2.uid, uid, pos})
    end
    if team.member3 then
        local user = session_user_state_mgr.get_user(team.member3.uid)
        rr.run_mfa(user.gateway_id, "user_mgr", "notify_remove_team_member", {team.member3.uid, uid, pos})
    end
end

-- 有新成员加入,通知团队现有成员,pos为该成员所在格子
function M.on_new_member(team, new_member_data, pos)
    if team.member1 then
        local user = session_user_state_mgr.get_user(team.member1.uid)
        rr.run_mfa(user.gateway_id, "user_mgr", "notify_team_new_member", {team.member1.uid, pos, M.get_pb_table(new_member_data)})
    end
    if team.member2 then
        local user = session_user_state_mgr.get_user(team.member2.uid)
        rr.run_mfa(user.gateway_id, "user_mgr", "notify_team_new_member", {team.member2.uid, pos, M.get_pb_table(new_member_data)})
    end
    if team.member3 then
        local user = session_user_state_mgr.get_user(team.member3.uid)
        rr.run_mfa(user.gateway_id, "user_mgr", "notify_team_new_member", {team.member3.uid, pos, M.get_pb_table(new_member_data)})
    end
end


function M.get_pb_table(member1)
        local data = {
            account = member1.account,
            uid = member1.uid,
            header_imgid = member1.imgid,
            header_imgurl = member1.imgurl,
            sex = member1.sex,
            nickname = member1.nickname
        }
        return data
end

function M.enter_team_response(team)
     local response = {
         teamname = team.team_name,
         total_num = team.total_num,
         leader_uid = team.leader_uid,
         members = {},
         voice_token = "abc",
    }
    if team.member1 then
        table.insert(response.members, M.get_pb_table(team.member1))
    end
    if team.member2 then
        table.insert(response.members, M.get_pb_table(team.member2))
    end
    if team.member3 then
        table.insert(response.members, M.get_pb_table(team.member3))
    end
    return response
end

function M.team_enter_room(uid)
    log:debug("团队请求进入房间,请求者id=%d", uid)
    local teamid = uid2teamid[uid]
    if not teamid then
        return
    end

    local team = teamdata[teamid]
    if not team then
        return
    end

    if uid ~= team.leader_uid then
        return 
    end

    local teamdata = {
        teamid = team.teamid,
        leaderid = team.leader_uid,
        total_num = team.total_num,
        current_num = team.current_num,
        voice_token = team.voice_token,
        members = {}
    }
    if team.member1 then
        local memberinfo = {
            uid = team.member1.uid,
            account = team.member1.account,
            nickname = team.member1.nickname,
            bulletid = 20001,
        }
        table.insert(teamdata.members, memberinfo)
    end
    if team.member2 then
        local memberinfo = {
            uid = team.member2.uid,
            account = team.member2.account,
            nickname = team.member2.nickname,
            bulletid = 20001,
        }
        table.insert(teamdata.members, memberinfo)
    end
    if team.member3 then
        local memberinfo = {
            uid = team.member3.uid,
            account = team.member3.account,
            nickname = team.member3.nickname,
            bulletid = 20001,
        }
        table.insert(teamdata.members, memberinfo)
    end

    local min_stress_svrid = session_user_state_mgr.get_min_stress_svrid(0)
    log:debug("团队模式进房间,token=%s,min_stress_svrid,svrid=%d", team.voice_token, min_stress_svrid)
    local teamdata_str = pb.encode("svr.TeamData", teamdata)
    rr.run_mfa(min_stress_svrid, "plane.room_mgr", "enter_team_room", {teamdata_str}, function(roomid, ret)
        if ret then
            local uids = M.get_member_uids(team)
            for i = 1,#uids do
                local user = session_user_state_mgr.get_user(uids[i])
                if user then
                    rr.run_mfa(user.gateway_id, "user_mgr", "connect_team_room", {uids[i], min_stress_svrid, roomid})
                end
                log:debug("团队模式,设置房间信息,%d,svrid=%d,roomid=%d", uids[i], min_stress_svrid, roomid)
                session_user_state_mgr.set_uid2roominfo(uids[i], min_stress_svrid, roomid, 2)
            end
            M.on_abadon_team(team, true)
            teamdata[teamid] = nil
        end
    end)
end

-- 加入团战房间时,为单个玩家创建一个队伍
function M.temp_team_join_room(basesvrid, rpcid, dest_uid, account, uid, nickname, bulletid, svrid, roomid)
    local team = M.create_team(uid, account)
    team.member1 = {
        uid = uid,
        nickname = nickname,
        account = account,
    }
    local teamdata = {
        teamid = team.teamid,
        leaderid = team.leader_uid,
        total_num = 0,
        current_num = 1,
        voice_token = team.voice_token,
        members = {}
    }
    local memberinfo = {
        uid = uid,
        account = account,
        nickname = nickname,
        bulletid = bulletid,
    }
    table.insert(teamdata.members, memberinfo)

    local teamdata_str = pb.encode("svr.TeamData", teamdata)
    rr.run_mfa(svrid, "plane.room_mgr", "join_team_room", {teamdata_str, roomid}, function(ret, failstr)
        if ret == 1 then
            rr.run_mfa(basesvrid, "user_mgr", "popMessageBox", 
                {rpcid, failstr}
            )
        else
            local user = session_user_state_mgr.get_user(uid)
            log:debug("%s", serpent.block(user))
            if user then
                rr.run_mfa(user.gateway_id, "user_mgr", "connect_team_room", {uid, svrid, roomid})
            end
            session_user_state_mgr.set_uid2roominfo(uid, svrid, roomid, 2)
            M.on_abadon_team(team, true)
            teamdata[team.teamid] = nil
        end
    end)
end

return M
