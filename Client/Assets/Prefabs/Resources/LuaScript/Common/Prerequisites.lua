g_debugMode = true

function regPath(path_)
    local origPackagePath = package.path
    package.path = string.format("%s;%s/?.lua", origPackagePath, path_)
    
    return package.path 
end

function regCPath(path_)
    local origPackageCPath = package.cpath
    package.cpath = string.format("%s;%s/?.dll", origPackageCPath, path_)
    
    return package.cpath 
end

function connectServer(ip, port)
    if true == g_debugMode then
        local initconnection = require("debugger")
        initconnection("127.0.0.1", "10000", "luaidekey")
    end
end

-- regPath("E:/Self/Self/unity/unitygame/Client/Assets/Prefabs/Resources/LuaScript/LuaLib")
regCPath("E:/Self/Self/unity/unitygame/Client/Assets/Plugins/x86_64")

