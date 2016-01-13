require "MyLua.Libs.Core.GlobalNS"

local M = GlobalNS.StaticClass()
M.clsName = "Memory"
GlobalNS[M.clsName] = M

-- 内存分配
function M.new(cls, ...)
    local instance = {}
    instance.dataType = "Instance"
    instance.clsCode = cls

    setmetatable(instance, cls)

    do
        local create
        create = function(cls, ...)
            if cls.super then
                create(cls.super, ...)
            end
            if cls.ctor then
                cls.ctor(instance, ...)
            end
        end

        create(cls, ...)
    end

    return instance
end

-- 删除空间
function M.delete(ptr)
    
end

return M
