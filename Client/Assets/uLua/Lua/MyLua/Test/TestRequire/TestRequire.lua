--[[  
      @brief 测试 require 实现，实际 findload 是 nil 值 
]]--

function require (name)
    if not package.loaded[name] then
        local loader = findload(name)
        if loader == nil then
            error("unable to load module " .. name)
        end
        package.loaded[name] = true --避免递归加载时死循环。
        local res = loader(name)
        if res ~= nil then
            package.loaded[name] = res
        end
    end
    return package.loaded[name]
end

require "MyLua.TestRequire.TestRequireDep"

result = addSelf(10, 20)
print(result)




