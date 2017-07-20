
local M = {
	event_name = "OnS2sDisconnected",
}

local log = require("log"):new(...)
local new_login_mgr = require("new_login_mgr")
local session_user_state_mgr = require("session_user_state_mgr")

function M.on_disconnected(svrid)
	log:info("server (%u) disconnected.", svrid)
    new_login_mgr.server_disconnected(svrid)
    session_user_state_mgr.server_disconnected(svrid)
end  -- on_disconnected()

M.handle = M.on_disconnected
return M
