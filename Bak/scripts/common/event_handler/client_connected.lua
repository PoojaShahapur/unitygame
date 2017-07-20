--[[
Event handler for "ClientConnected".
--]]

local M = {
	event_name = "ClientConnected",
}

local log = require("log"):new(...)
local user_mgr = require("user_mgr")
local timer_queue = c_timer_queue.CTimerQueue()

local function check_connection(rpc_clt_id)
	if user_mgr.get_user(rpc_clt_id) then return end
	log:debug("Disconnect non-login client(%u).", rpc_clt_id)
	-- disconnect_game_client() 不会断开服务器，仅会断开游戏客户端连接
	c_util.disconnect_game_client(rpc_clt_id)
end  -- check_connection()

function M.on_connected(rpc_clt_id, clt_addr, clt_port)
	assert("number" == type(rpc_clt_id))
	log:debug("Client(%u) connected from %s:%u.", rpc_clt_id, clt_addr, clt_port)

	-- 5s后断开仍未登录的连接(恶意客户端)
	timer_queue:insert_single_from_now(5000,
		function() check_connection(rpc_clt_id) end)
end  -- on_connected()

M.handle = M.on_connected
return M
