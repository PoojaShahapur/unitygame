if true == g_debugMode then
    local initconnection = require("debugger")
    initconnection("127.0.0.1", "10000", "luaidekey")
end

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
