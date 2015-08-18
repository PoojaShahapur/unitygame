local g_debugMode = true

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

function connectServer()
    if true == g_debugMode then
        local initconnection = require("debugger")
        initconnection("127.0.0.1", "10000", "luaidekey")
    end
end

regPath("D:/file/opensource/unity-game-git/unitygame/unitygame/Client/Assets/Prefabs/Resources")
regPath("D:/file/opensource/unity-game-git/unitygame/unitygame/Client/Assets/Prefabs/Resources/LuaScript/LuaLib")
regCPath("D:/file/opensource/unity-game-git/unitygame/unitygame/Client/Assets/Plugins/x86_64")

