local M = {}

local pb = require("protobuf")
local user = require("user")
local serpent = require("serpent")

function M.login(account, password)
	local req = {account = account, password = password}
	local req_str = pb.encode("rpc.LoginRequest", req)
	assert("string" == type(req_str))
	user.rpc_request("rpc.Login", "Login", req_str,
		function(resp_str)
			local resp = pb.decode("rpc.LoginResponse", resp_str)
			print("Login response: " .. serpent.block(resp))
		end)
end  -- login()

function M.l()
	M.login("LuaTester", "password")
end

return M
