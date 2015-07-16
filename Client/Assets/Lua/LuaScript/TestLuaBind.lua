--[[
function luaFunc(i)
    a = i + 100
    print(a)
    return a
end
]]

-- 不允许这么使用
-- local testGlobal = 25

--luaFunc(36)

-- 测试表中函数
testTable = {}
testTable.tableData = 256

testTable.tableFunc = function (i)
	a = i + 300
	return a
end

-- 测试调用 CS 函数
-- SDK.Lib.TestStaticHandle.log("aaaaa")
GlobalLog = SDK.Lib.TestStaticHandle
GlobalLog.log("aaaaa")