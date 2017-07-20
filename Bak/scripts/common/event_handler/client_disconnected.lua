--[[
Event handler for "ClientDisconnected".
--]]

local M = {
	event_name = "ClientDisconnected",
}

local log = require("log"):new(...)
local user_mgr = require("user_mgr")

function M.on_disconnected(rpc_clt_id)
	assert("number" == type(rpc_clt_id))
	log:info("Client(%u) disconnected.", rpc_clt_id)
	-- for manager user erase
	user_mgr.erase(rpc_clt_id)

	require("plane.on_disconnected").on_disconnected(rpc_clt_id)
end  -- on_disconnected()

M.handle = M.on_disconnected
return M
