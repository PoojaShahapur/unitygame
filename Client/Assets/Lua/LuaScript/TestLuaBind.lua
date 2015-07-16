function luaFunc(i)
    a = i + 100
    print(a)
    return a
end

local testGlobal = 25

luaFunc(36)

testTable = {}
testTable.tableData = 256

testTable.tableFunc = function (i)
	a = i + 300
	return a
end

-- SDK.Lib.TestStaticHandle.log("aaaaa")
GlobalLog = SDK.Lib.TestStaticHandle
GlobalLog.log("aaaaa")