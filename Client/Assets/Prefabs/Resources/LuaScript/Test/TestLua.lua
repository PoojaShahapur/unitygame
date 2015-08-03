-- require "LuaScript/Common/Prerequisites"  -- 只要包含这一行就不能调试，在连接服务器之前一定不能有 require 指令，但是可以有代码
-- connectServer()

-- local aaa = 5

-- if true == g_debugMode then
    local initconnection = require("debugger")
    initconnection("192.168.122.64", "10000", "luaidekey")
-- end

--[[

mime   = require("mime")

function nullRet()

    if mime == nil then
        return 0
    else 
        if mime.encode == nil then
            return 1
        end
    end
    return 2
end
]]


function luaFunc(i)
    a = i + 100
    print(a)
    return a
end

function addVarArg(...)
    print(3)
    local arg = {...}
    local n = table.getn(arg)
    --print(n)
    print(2)
	  return arg[1] + arg[2]
end

--addVarArg(10, 20)

--require("TestLua")

aaa = luaFunc(10)
print(aaa)