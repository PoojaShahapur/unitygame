-- test_cats_and_dogs.lua

local M = {}

local pb = require("protobuf")
local user = require("user")

local function login()
	local req = {account = "account", password = "password"}
	local req_str = pb.encode("rpc.LoginRequest", req)
	user.rpc_request("rpc.Login", "Login", req_str)
end  -- login()

function M.test()
	login()

	user.rpc_request("rpc.CatsAndDogs", "EnterMatch",
		pb.encode("rpc.EmptyMsg", {}),
		function(s) print("Entered!") end)
end  -- test()

return M
