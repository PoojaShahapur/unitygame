local M = {
    event_name = "OnS2sConnected",
}

local log = require("log"):new(...)
local new_login_mgr = require("new_login_mgr")
local session_user_state_mgr = require("session_user_state_mgr")

function M.on_svrconnected(svrid)
    log:info("server (%u) connected.", svrid)
    new_login_mgr.server_connected(svrid)
    session_user_state_mgr.server_connected(svrid)
end  -- on_disconnected()

M.handle = M.on_svrconnected
return M

