g_debugMode = false

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