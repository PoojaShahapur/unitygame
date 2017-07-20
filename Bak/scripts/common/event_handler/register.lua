-- Event handler register.

local M = {}

local log = require("log"):new(...)
local event_handler = require("event_handler.event_handler")

-- 如果处理器模块有event_name和handle()，就可按模块注册。
-- 不然就需要另外指定事件名和处理函数。
local function register_handler(mod)
	assert("table" == type(mod))
	assert("string" == type(mod.event_name))
	assert("function" == type(mod.handle))
	event_handler.register(mod.event_name, mod.handle)
end

local function register_module(module_name)
	assert("string" == type(module_name))
	register_handler(require("event_handler." .. module_name))
end

function M.register_all()
	log:debug("Register all event handler.")
	register_module("client_connected");
	register_module("client_disconnected");
	register_module("server_connected");
	register_module("server_disconnected");
end  -- register_all()

return M
