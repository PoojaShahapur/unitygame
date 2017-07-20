local M = {}

local pb = require("protobuf")
local log = require("log"):new(...)

function M.rpc_request(service_name, method_name, request, callback)
	assert("string" == type(service_name))
	assert("string" == type(method_name))
	assert("string" == type(request))
	assert(not callback or "function" == type(callback))
	log:debug("rpc request: %s.%s", service_name, method_name)
	c_rpc.request(service_name, method_name, request, callback)
end  -- rpc_request

function M.svr_run(codes)
	local req = { codes = codes }
	local req_str = pb.encode("svr.RunLuaRequest", req)
	M.rpc_request("svr.RunLua", "Run", req_str,
		function(resp_str)
			local resp = pb.decode("svr.RunLuaResponse", resp_str)
			log:debug("Svr run lua codes response: " .. resp.codes)
		end)
end  -- svr_run_lua()

return M
