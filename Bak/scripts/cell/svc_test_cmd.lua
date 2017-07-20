local M = {}

local log = require("log"):new("svc_test_cmd")
local pb = require("protobuf")
local inspect = require("inspect")

-- Return TestCmdResponse.
local function testcmd(ctx, request)
	local cmd = request.cmd
	if cmd =="mongodb_test" then
		return require("test/mongodb_test").test(ctx, request)
	elseif cmd=="mongodb_test_b" then
		return require("test/mongodb_test").test_binary_data(ctx, request)
	elseif cmd =="mongo_rs" then
		require("test/mongodb_replica_set_test").test(ctx, request)
	elseif cmd =="gpreftools_heap" then
		c_util.gpreftools_heap_dump()
	elseif cmd =="gpreftools_cpu" then
		c_util.gpreftools_flush()
	elseif cmd == "redis_test" then
		require("test.redis_test").test()
	else
		return {cmd = cmd, result = "ERR", error_str = "Not supported."}
	end
	return {cmd = cmd, result = "OK"}
end  -- testcmd()

function M.TestCmd(ctx, content)
	log:info("test cmd")
	local req = assert(pb.decode("svr.TestCmdRequest", content))
	log:info("TestCmd inspect req: "..inspect(req))
	local resp = testcmd(ctx, req)
	assert("table" == type(resp))
	c_rpc.reply_to(ctx, pb.encode("svr.TestCmdResponse", resp))
end  -- TestCmd()

require("rpc_request_handler").register_service("svr.TestCmd", M)
return M
