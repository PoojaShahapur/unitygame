local M = {}

local log = require("log"):new("mongo_test")
local pb = require("protobuf")

local mytest = "testrs"
local host = {'192.168.124.254','192.168.124.254','192.168.124.254'}
local port = {20001,20002,20003}
local user = ''
local pswd = ''
local auth = "test"
local options = ""


local mdb = require("mongodb"):new(mytest,host,port,auth,user,pswd,options)


function M.test(ctx, request)
	log:info("rs_test begin...")
	
	mdb:insert("test", "coll", { roleid=1001, rolename="cat001" })
	mdb:insert("test", "coll", { roleid=1002, rolename="cat002" })
	mdb:insert("test", "coll", { roleid=1003, rolename="cat333" })
	mdb:insert("test", "coll", { roleid=1004, rolename="cat004" })
	mdb:remove("test", "coll", { roleid=1002 })
	mdb:update("test", "coll", { roleid=1003 }, {rolename="dog003"})
	
	return {cmd = request.cmd, result = "OK"}
end

return M
