--[[
Rpc response handler for test client.
Author: Jin Qing (http://blog.csdn.net/jq0123)
]]

local M = {}

local callbacks = {}
local log = require("log"):new(...)

local function erase_callback(resp_id)
	callbacks[resp_id] = nil
end

-- 返回回调函数
local function pop_callback(resp_id)
	local callback = callbacks[resp_id]
	if not callback then
		log:debug("No callback for response. resp_id=%q", resp_id)
		return nil
	end
	erase_callback(resp_id)
	return callback
end

-- 处理Rpc回调
-- resp_id 已经是lua内部ID
function M.handle(resp_id, resp_content)
	log:debug("handle(RespId=%u, resp(len=%u))",
		resp_id, #resp_content)
	local callback = pop_callback(resp_id)
	if callback then
		callback(resp_content)
	end
end  -- handle()

-- 注册回调。lua_rpc_id是Lua内部生成的ID,由C++负责与RpcId的映射。
function M.register_callback(lua_rpc_id, callback)
	assert("function" == type(callback))
	callbacks[lua_rpc_id] = callback
end  -- register_service

log:debug("Loaded.")
return M
