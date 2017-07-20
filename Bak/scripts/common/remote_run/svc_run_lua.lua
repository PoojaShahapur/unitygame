-- svc_run_lua.lua
-- svr.RunLua service
-- run_lua.proto
-- Run lua by other servers.
-- Only used between game servers.

local M = {}

local pb = require("protobuf")
local log = require("log"):new("svc_run_lua")
local new_login_mgr = require("new_login_mgr")
local balance_mgr = require("balance_mgr")
local serpent = require("serpent")

log:debug("loading service...")

-- Todo: Base服要禁止Clt请求

function M.Run(ctx, content)
	local req = assert(pb.decode("svr.RunLuaRequest", content))
	log:debug("Run: "..req.codes)  -- todo: from where?
	local result = assert(load(req.codes))()
	local res_type = type(result)
	local codes = ""
	if ("boolean" == res_type or
		"string" == res_type or
		"number" == res_type) then
		codes = ""..result
	end
	local resp = { codes = codes }
	local resp_str = pb.encode("svr.RunLuaResponse", resp)
	c_rpc.reply_to(ctx, resp_str)
end  -- Run()

-- Run module.function(...arguments...)
function M.RunMfa(ctx, content)
	local request = pb.decode("svr.RunLuaMfaRequest", content)
	--log:debug("RunMfa %s.%s", request.module_name, request.function_name)  -- todo: from where?
	local mod = require(request.module_name)
	local fun = mod[request.function_name]
	local ok, arguments = serpent.load(request.arguments_dump)
	assert(ok, "Illegal arguments.")
	assert("table" == type(arguments))
	local result_table = table.pack(fun(table.unpack(arguments)))
	local resp = { returned_dump = serpent.dump(result_table) }
	local resp_str = pb.encode("svr.RunLuaMfaResponse", resp)
	c_rpc.reply_to(ctx, resp_str)
end  -- Run()

function M.CanILogin(ctx, content)
    local req = assert(pb.decode("svr.LoginRequest", content))
    new_login_mgr.can_i_login(ctx, req.svrid, req.openid)
end

function M.ExitRoom(ctx, content)
    log:debug("session 收到退出房间消息")
    --[[
    local req = assert(pb.decode("svr.ExitRoomMsg", content))
    balance_mgr.leave_room(req.account, req.player_server_id, req.room_id, req.uid)
    --]]
end

require("rpc_request_handler").register_service("svr.RunLua", M)

return M
