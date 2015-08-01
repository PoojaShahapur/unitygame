g_debugMode = true

function regPath(path_)
    local m_package_path = package.path
    package.path = string.format("%s;%s/?.lua", m_package_path, path_)
    
    return package.path 
end

function regCPath(path_)
    local m_package_cpath = package.cpath
    package.cpath = string.format("%s;%s/?.dll", m_package_cpath, path_)
    
    return package.cpath 
end