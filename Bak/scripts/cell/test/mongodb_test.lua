--- mongodb test.
-- @module test.mongodb_test

local M = {}

local log = require("log"):new("mongo_test")
local pb = require("protobuf")
local serpent = require("serpent")

local mytest = "luatest"
local host = {'222.73.63.209'}
local port = {27017}
local user = 'test'
local pswd = '123@123'
local auth = "giantserver_test" --该账号所属数据库
local options = ""	--连接选项，若有需要设置，请参见官方文档，了解有哪些选项可以设置。格式为"key1:value1&key2:value2"

-- 获取数据库实例
--    该行语句会创建数据库连接并保持。如果该实例名的连接已经创建，则只是简单的返回其引用
--    请不用担心多次调用 同样参数的 这行语句会不会多次创建多个数据库连接等底层的问题
--    如果是对复制集进行操作，则host、port定义类似如下：
--			local host = {“xxx.xxx.xxx.xx1”,“xxx.xxx.xxx.xx2”,“xxx.xxx.xxx.xx3”}
--          local port = {yyy1,yyy2,yyy3}
--			需要将复制集所有ip、port填写进去
--    如果是对单mongod实例，或是对分片集群的mongos进行操作，则host、port定义类似如下：
--			local host = {“xxx.xxx.xxx.xx1”}
--          local port = {yyy1}

local mdb = require("mongodb"):new(mytest,host,port,auth,user,pswd,options)


-- 使用 mdb:insert 、mdb:remove、mdb:update、mdb:query 操作数据
-- 操作的数据可以是 json字符串、也可以是lua的table对象。看个人喜好，推荐使用lua的table对象

function M.test(ctx, request)
	mdb:update("giantserver_test", "table1", { roleid=1003 }, {rolename="dog003"})
	mdb:query("giantserver_test", "t_planeaccounts", '{}', '', function (datas)
		for i = 1, #datas,1 do
			log:info("row" .. i .. ": " .. datas[i])
		end
	end)
	log:info("mongo_test begin...")

	--mdb:remove("giantserver_test", "table1", '{}')

	-- 插入数据
	mdb:insert("giantserver_test", "table1", { roleid=1001, rolename="cat001" })
	mdb:insert("giantserver_test", "table1", { roleid=1002, rolename="cat002" })
	mdb:insert("giantserver_test", "table1", { roleid=1003, rolename="cat333" })
	mdb:insert("giantserver_test", "table1", { roleid=1004, rolename="cat004" })

	-- 删除数据
	mdb:remove("giantserver_test", "table1", { roleid=1002 })

	-- 更新数据
	mdb:update("giantserver_test", "table1", { roleid=1003 }, {rolename="dog003"})

	-- 查询所有记录
	--    查询的第4个参数，可以指定只返回某些列，如该参数可以填写成这样 {roleid=1},则rolename字段内容不会被查询到
	--    查询的第5个参数，指定一个回调函数，其参数是一个下标从1开始的lua table，里面的元素为json字符串
	log:info("begin query all data ...")
	mdb:query("giantserver_test", "table1", '{}', '', function (datas)
		for i = 1, #datas,1 do
			log:info("row" .. i .. ": " .. datas[i])
		end
	end)

	return {cmd = request.cmd, result = "OK"}
end

-- 下面是mongodb 操作2进制数据的例子
function M.test_binary_data(ctx, request)
	log:info("mongo_test_binary_data begin...")

	-- 插入数据
    mdb:remove("giantserver_test", "table2", {})
	mdb:insert_b("giantserver_test", "table2", { roleid=1001, rolename="cat001" })
	-- 更新数据
	mdb:update_b("giantserver_test", "table2", { roleid=1001 }, nil, {num=0, desc='update_b not working!!!'})

	-- 查询数据
	log:info("begin query all data ...")
	mdb:query_b("giantserver_test", "table2", '{}', '', function (datas)
		for i = 1, #datas,1 do
			local _doc = mdb:get_table(datas[i])
			log:info("row" .. i .. ": ")
			print(serpent.block(_doc))
		end
	end)

	--return {cmd = request.cmd, result = "OK"}
end


return M
