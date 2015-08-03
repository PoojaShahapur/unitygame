-- require "LuaScript/Common/Prerequisites"
-- connectServer()

local initconnection = require("debugger")
initconnection("192.168.122.64", "10000", "luaidekey")

-- 不允许定义全局函数
--[[
function luaFunc(i)
    a = i + 100
    print(a)
    return a
end
]]

-- 不允许定义全局变量
-- local testGlobal = 25

--luaFunc(36)

-- 定义表
testTable = {}
-- 定义表中变量
testTable.tableData = 256
-- 定义表中函数
testTable.tableFunc = function (i)
	a = i + 300
	return a
end

testTable.tableFunc(20)

-- 测试调用 CS 函数，全路径和非全路径都可以
-- SDK.Lib.TestStaticHandle.log("aaaaa")
-- GlobalLog = SDK.Lib.TestStaticHandle
-- GlobalLog.log("aaaaa")