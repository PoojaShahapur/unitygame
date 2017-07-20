--[[
Event handler.
Author: Jin Qing (http://blog.csdn.net/jq0123)
ע���¼������¼�����ʱ������Ӧ�Ĵ�������
C++����CEventToLua������Lua�¼���������
�¼�����˵����: doc/event.md
--]]

local M = {}

local log = require("log"):new(...)
local handler_map = {}
local handler_once_map = {}
local warned = {}

function M.handle(event_name, ...)

	local handler_once = handler_once_map[event_name]
	if handler_once then
		handler_once_map[event_name] = nil
		return handler_once(...)
	end

	local handler = handler_map[event_name]
	if handler then
		return handler(...)
	end

	if warned[event_name] then
		return
	end
	warned[event_name] = true
	log:warn("No handler for event: %q", event_name)
end  -- handle()

function M.register(event_name, handler)
	log:debug("Register: %s", event_name)
	handler_map[event_name] = handler
end  -- register()

function M.register_once(event_name, handler)
	log:debug("Register once: %s", event_name)
	handler_once_map[event_name] = handler
end  -- register_once()

return M
