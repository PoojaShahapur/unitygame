package.path = string.format("%s;%s/?.lua", package.path, "D:/File/OpenSource/Unity/Tools/LuaLibs/src")
-- package.path = string.format("%s;%s/?.lua", package.path, "E:/Self/Self/Tools/Tools/LuaLibs/src")
require "MyLua.Libs.Core.Prequisites"

local function main()
    -- testArray()
    -- testLen()
    -- testFuncEnv()
    testDispatcher()
end

function testArray()
    local array = GlobalNS.MList:new()
    local metatable = array.metatable   -- 在 lua 中是不能直接这样取值的
    array:add(1)
    array:add(2)
    array:add(3)
    
    array:remove(2)
end

function testLen()
    local tbs = {}
    tbs = {[2] = 1}
    tbs["aaa"] = "bbb"
    local len = #tbs
    len = table.getn(tbs)
    print(len)
end

function testFuncEnv()
    require "TestEnv.TestEnv"
    local aaa = 411
end

function testDispatcher()
    local callOnceEventDispatch = GlobalNS.CallOnceEventDispatch:new()
    callOnceEventDispatch:addEventHandle(eventCall)
    callOnceEventDispatch:dispatchEvent(nil)
end

function eventCall(dispObj)

end

main()