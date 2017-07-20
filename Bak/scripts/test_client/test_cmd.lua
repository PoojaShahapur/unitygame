--- 测试command
-- @module test_cmd

local M = {}

local pb = require("protobuf")
local user = require("user")
local serpent = require("serpent")

function M.test(cmd)
	local req = {cmd = cmd}
	local req_str = pb.encode("svr.TestCmdRequest", req)
	user.rpc_request("svr.TestCmd", "TestCmd", req_str,
		function(resp_str)
			local resp = pb.decode("svr.TestCmdResponse", resp_str)
			assert("table" == type(resp))
			print("Response: " .. serpent.block(resp))
		end)
end  -- TestCmd()

return M
