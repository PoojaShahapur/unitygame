-- 定义一个类表
local _class={}     -- 保存定义的所有的类

function class(super)
    local class_type = {}       -- 返回的类表
    class_type.ctor = false     -- 构造构造函数默认是没有的
    class_type.super = super    -- 设置父类表
    
    class_type.new = function(...)    -- 以这个表为元表，生成一个新的表
        local obj={}
        do
            local create              -- 调用类的创建函数
            create = function(c, ...)
                if c.super then         -- 如果有父类，就递归调用父类的创建函数
                    create(c.super, ...)
                end
                if c.ctor then          -- 调用自己的构造函数
                    c.ctor(obj, ...)
                end
            end
     
            create(class_type, ...)
        end
        
        setmetatable(obj, { __index = _class[class_type] })     -- 设置新表的元表为父类表
        
        return obj
    end
    
    local vtbl = {}
    _class[class_type] = vtbl     -- 几率这个类的虚函数表
   
   -- 设置当前类的元表为 vtbl 表
    setmetatable(class_type, {__newindex =
        function(t, k, v)
            vtbl[k]=v       -- 添加一个成员完全是重新生成
        end
    })
   
   -- 如果父类表存在，设置 vtbl 表的元表为父类表
    if super then
        setmetatable(vtbl, {__index =
            function(t, k)
                local ret = _class[super][k]
                vtbl[k] = ret       -- 读取的时候从父类查找
                return ret
            end
        })
    end
   
    return class_type
end