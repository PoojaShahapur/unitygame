local M = {}

local pb = require("protobuf")
local user = require("user")

function M.test()
	local req = { codes = "print(\"run lua test\"); return 123456" }
	local req_str = pb.encode("svr.RunLuaRequest", req)
	user.rpc_request("svr.RunLua", "Run", req_str,
		function(resp_str)
			local resp = pb.decode("svr.RunLuaResponse", resp_str)
			print("Run lua response codes: " .. resp.codes)
		end)
end  -- test()

return M
