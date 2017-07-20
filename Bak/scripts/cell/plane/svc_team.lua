local M = {}
local user_mgr = require("user_mgr")
local rr = require("remote_run.remote_runner")
local pb = require("protobuf")
local log = require("log"):new("svc_team")
local serpent = require('serpent')

function M.EnterTeam(ctx, content)
    local req = assert(pb.decode("plane.EnterTeamMsg", content))
    log:debug("enter team,%s", serpent.block(req))
    local user = user_mgr.get_user(ctx:get_rpc_clt_id())
    if user ~= nil then
        rr.run_mfa(1, "team_mgr", "enter_team"
        , {req.teamid, user.uid, req.nickname,user.account,user.header_imgid,user.header_imgurl, user.sex}
        , function (retcode, response)
            if retcode == 1 then
                user:popMessageBox(data);
            else
                log:debug("enter team respons")
                c_rpc.reply_to(ctx, pb.encode("plane.EnterTeamResponse", response))
            end
        end)
    end
end

function M.LeaveTeam(ctx)
    log:debug("leave team")
    local user = user_mgr.get_user(ctx:get_rpc_clt_id())
    if user ~= nil then
        rr.run_mfa(1, "team_mgr", "leave_team", {user.uid})
    end
end

function M.InviteJoinTeam(ctx, content)
    local req = assert(pb.decode("plane.UidListMsg", content))
    log:debug("invite join team,uids=%s", serpent.block(req))
    local user = user_mgr.get_user(ctx:get_rpc_clt_id())
    if user ~= nil then
        rr.run_mfa(1, "team_mgr", "invite_join_team", {user.uid, user.account, req.uids})
    end
end

function M.ChangeNickname(ctx, content)
    log:debug("change nickname")
    local req = assert(pb.decode("plane.NicknameMsg", content))
    -- todo, 检查昵称的合法性
    local user = user_mgr.get_user(ctx:get_rpc_clt_id())
    if user ~= nil then
        rr.run_mfa(1, "team_mgr", "change_nickname", {user.uid, req.nickname})
    end
end

function M.TeamEnterRoom(ctx)
    local user = user_mgr.get_user(ctx:get_rpc_clt_id())
    if user ~= nil then
        rr.run_mfa(1, "team_mgr", "team_enter_room", {user.uid})
    end
end

function M.ViewFriendList(ctx, content)
    local user = user_mgr.get_user(ctx:get_rpc_clt_id())
    if user ~= nil then
        local req = assert(pb.decode("plane.PageNumMsg", content))
        user.log:debug("请求第%d页好友数据", req.page)
        rr.run_mfa(1, "session_user_state_mgr", "get_page_x_friend_uids", {user.uid, req.page}
        ,function(ret, total)
            if ret then
                -- 获得uid了,取下这些uid的数据
                user:send_friend_data(ctx, ret, req.page, total)
            end
        end)
    end
end

require("rpc_request_handler").register_service("plane.Team", M)
return M
