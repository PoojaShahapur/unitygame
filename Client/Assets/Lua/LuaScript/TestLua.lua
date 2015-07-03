--require('debugger')('192.168.122.64', '10000')

local initconnection = require("debugger")
initconnection("192.168.122.64", "10000", "luaidekey")

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

require("TestLua")