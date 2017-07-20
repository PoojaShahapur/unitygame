-- 运行另一服务器上的Lua代码
-- 也支持本服运行(本服RPC调用)

local M = {}

local pb = require("protobuf")
local log = require("log"):new("remote_runner")
local serpent = require("serpent")

-- on_result(result) 生成 rpc 回调函数 cb(resp_str)
local function get_cb(on_result)
	if (not on_result) then return nil end
	assert("function" == type(on_result))

	local cb = function(resp_str)
		assert("string" == type(resp_str))
		local resp = assert(pb.decode("svr.RunLuaResponse", resp_str))
		local codes = resp.codes
		log:debug("RunLuaResponse codes: %q", codes)
		local result = assert(load(codes))()
		on_result(result) -- 回调时执行
	end  -- cb

	return cb
end  -- get_cb()

function M.run(svr_id, codes, on_result)
	assert("number" == type(svr_id))
	assert("string" == type(codes))
	assert(nil == on_result or "function" == type(on_result))
	log:debug("Request Svr_%s to run: %s", svr_id, codes)
	local req = { codes = codes }
	local req_str = pb.encode("svr.RunLuaRequest", req)
	local cb = get_cb(on_result)
	c_rpc.request_svr(svr_id, "svr.RunLua", "Run", req_str, cb)
end  -- run()

-- on_result(result) 生成 rpc 回调函数 cb(response)
local function get_mfa_cb(on_result)
	if (not on_result) then return nil end
	assert("function" == type(on_result))

	local cb = function(resp_str)
		assert("string" == type(resp_str))
		local resp = assert(pb.decode("svr.RunLuaMfaResponse", resp_str))
		local ok, copy = serpent.load(resp.returned_dump)
		assert(ok, "Run mfa returns invalid value.")
		log:debug("RunLuaMfaResponse: %s", serpent.line(copy))
		on_result(table.unpack(copy)) -- 回调时执行
	end  -- cb

	return cb
end  -- get_mfa_cb()

-- Run module function with arguments on remote server.
-- 示例 rum_mfa(123, "event.dispatcher", "dispatch", {"EventName", 1,2,3}, nil)
-- Todo: arguments 应该是 table.pack() 产生的表，以允许 nil 元素
function M.run_mfa(svr_id, module_name, function_name, arguments, on_result)
	assert("number" == type(svr_id))
	assert("string" == type(module_name))
	assert("string" == type(function_name))
	assert("table" == type(arguments))
	assert(nil == on_result or "function" == type(on_result))
    local my_svr_id = c_util.get_my_svr_id()
    if my_svr_id == svr_id then
        local module = require(module_name)
        local func = module[function_name]
        -- 要调用func,否则结果不对
        local res = table.pack(func(table.unpack(arguments)))
        if on_result ~= nil then
            on_result(table.unpack(res))
        end
        return
    end
	--log:debug("Request to call Svr_%s %s.%s()", svr_id, module_name, function_name)
	local req = {
		module_name = module_name,
		function_name = function_name,
		arguments_dump = serpent.dump(arguments)
	}
	local req_str = pb.encode("svr.RunLuaMfaRequest", req)
	local cb = get_mfa_cb(on_result)
	c_rpc.request_svr(svr_id, "svr.RunLua", "RunMfa", req_str, cb)
end  -- run()

return M
