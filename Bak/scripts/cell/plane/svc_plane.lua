local M = { }

local log = require("log"):new("plane.svc")
local room_mgr = require("plane.room_mgr")
local user_mgr = require("user_mgr")
local balancer = require("plane.balancer")
local rr = require("remote_run.remote_runner")
local router = require("rpc.base_rpc_router")

local pb = require("protobuf")
local empty_msg_str = pb.encode("rpc.EmptyMsg", { })
local sessionid = 1

log:debug("loading service...")

function GetCutNickName(str)
    local lenInByte = #str;
    local count = 0;
    local i = 1;
    while i <= lenInByte do
        local curByte = string.byte(str, i);
        local byteCount = 1;
        if curByte > 0 and curByte < 128 then
            byteCount = 1;
        elseif curByte>=128 and curByte<224 then
            byteCount = 2;
        elseif curByte>=224 and curByte<240 then
            byteCount = 3;
        elseif curByte>=240 and curByte<=247 then
            byteCount = 4;
        else
            break;
        end

        i = i + byteCount;
        count = count + 1;
		if count >= 8 then
			return string.sub(str, 1, i)
		end
    end
    return str;
end

function M.ReconnectEnterRoom(ctx, content)
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user == nil then
        return
    end
    rr.run_mfa(sessionid, "session_user_state_mgr", "req_reconnect_room", {user.uid})
end

function M.EnterRoom(ctx, content)
    -- 1. 问session,哪个服负载(正在进行游戏的满人房间数最小)
    -- 2. 远程调用最小服务器的 enter_room
    -- 3. 收到结果后,设置路由,并设置不能操作金钱
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user == nil then
        return
    end
    local req = assert(pb.decode("plane.EnterRoomMsg", content))
    if string.find(req.nickname,' ') or string.find(req.nickname,'%%') then
        user:popMessageBox("昵称中含有非法字符!")
        return
    end
    if string.len(req.nickname) > 24 then
        user:popMessageBox("昵称过长,请删掉部分字符再试!")
        return
    end
    if req.mode == 1 and user.can_in_pur ~= 1 then
        user:popMessageBox("普通模式获得第一名才解锁炼狱模式哦！")
        return
    end
    rr.run_mfa(sessionid, "session_user_state_mgr", "enter_room" 
        , {req.mode, user.account, user.uid, req.nickname, user.skinm.default_skinid, user.skinm.default_bullet})
    --[[
    rr.run_mfa(sessionid, "balance_mgr", "get_min_stress_roomid", {req.mode},
        function (svrid)
                local game_clt_id = ctx:get_game_clt_id()
                local base_svr_id = game_clt_id.base_svr_id
                local base_rpc_clt_id = game_clt_id.base_rpc_clt_id
                local user = user_mgr.get_user(base_rpc_clt_id)
                if user ~= nil then
                    if svrid ~= 0 then
                        rr.run_mfa(svrid, "plane.room_mgr", "enter_room",
                            -- game_clt_id不是table, 不能直接传
                            {base_svr_id, base_rpc_clt_id, user.account, user.uid, req.nickname, req.mode,
                            user.skinm.default_skinid, user.skinm.default_bullet, user.level},
                            function(roomid, result)
                                user.can_op_money = 0
                                -- 通知session我进入房间了
                                rr.run_mfa(sessionid, "balance_mgr", "enter_room_ok", {user.account, user.uid, svrid, roomid})
                                router.set_svc_dst_svr_id(game_clt_id, "plane.Plane", svrid, function()
                                    c_rpc.reply_to(ctx, result)
                                end)
                            end)
                    else
                        user:popMessageBox("炼狱模式过于火爆,房间已满,请稍后再试！")
                    end
                end
        end
    )
    --]]
end  -- EnterMatch()

function M.JoinRoom(ctx, content)
    -- 问下 session,那哥们在哪个服务器
    local req = assert(pb.decode("plane.JoinRoomMsg", content))
    local clt_id = ctx:get_rpc_clt_id()
    local user = user_mgr.get_user(clt_id)
    if user ~= nil then
        --[[
        --user都在网关上,是能找到的;这里直接去问session在哪个服务器,哪个房间
        local dest_user = user_mgr.get_user_by_account(req.acc)
        if dest_user ~= nil then--和我在一个服务器
            room_mgr.join_room(ctx, c_util.get_my_svr_id(), clt_id, req.acc, user.account
                , req.nickname, user.skinid, user.bulletid)
        else--在另外的cell,去问下session,到底在哪个服务器
        ]]
    rr.run_mfa(sessionid, "session_user_state_mgr", "req_join_room" 
        , {c_util.get_my_svr_id(), clt_id, req.uid, user.account, user.uid
        , req.nickname, user.skinm.default_skinid, user.skinm.default_bullet}, function (failstr)
            if failstr then
                user:popMessageBox(failstr)
            end
        end)
        --[[
            rr.run_mfa(sessionid, "balance_mgr", "get_room_by_account", {req.acc},function (svrid, roomid)
                if svrid ~= 0 then
                    rr.run_mfa(svrid, "plane.room_mgr", "join_room", 
                        {roomid, c_util.get_my_svr_id(), clt_id,req.acc
                        , user.account, user.uid, req.nickname, user.skinm.default_skinid, user.skinm.default_bullet, user.level},
                        function (roomid, result)
                            if roomid ~= 0 then
                                user.can_op_money = 0
                                -- 通知session我进入房间了
                                rr.run_mfa(sessionid, "balance_mgr", "enter_room_ok", {user.account, svrid, roomid})
                                router.set_svc_dst_svr_id(ctx:get_game_clt_id(), "plane.Plane", svrid, function()
                                    local resp_str = pb.encode("plane.EnterRoomResponse", result)
                                    c_rpc.reply_to(ctx, resp_str)
                                end)
                            end
                        end
                    )
                else
                    user:popMessageBox("该玩家已不在房间！")
                end
            end)
            ]]
    else
        log:debug('JoinRoom,找不到client id=%d的玩家', clt_id)
    end
end
function M.RunGMCmd(ctx, content)
    local req = assert(pb.decode("plane.GMCmd", content))
    local user = user_mgr.get_user(ctx:get_rpc_clt_id())
    log:debug('gm cmd=' .. req.cmd)
    if user ~= nil then
        log:debug('user do gm cmd=' .. req.cmd)
        user:do_gm_cmd(ctx, req.cmd)
    end
end

function M.BackHall(ctx, content)
    local game_clt_id = ctx:get_game_clt_id()
        c_rpc.request_clt(game_clt_id, "plane.PlanePush", "BackHallOK",'')
        router.reset_svc_dst_svr_id(game_clt_id, "plane.Plane")
        -- 设置可以用钱了
        rr.run_mfa(game_clt_id.base_svr_id,
            "user_mgr", "set_user_can_op_money",
            { game_clt_id.base_rpc_clt_id })
    room_mgr.erase_player(game_clt_id)
end

require("rpc_request_handler").register_service("plane.Plane", M)
return M
