-- 定义一个类表

-- 采用虚函数表的形式
--[[
local _class={}     -- 保存定义的所有的类

function class(super)
    local class_type = {}       -- 返回的类表
    class_type.ctor = false     -- 构造构造函数默认是没有的
    class_type.super = super    -- 设置父类表，可以通过 super 这个字段访问父类
    
    class_type.new = function(...)    -- 以这个表为元表，生成一个新的表
        local obj={}
        
        -- 生成表后，直接赋值元表为类的 vtbl 表，这样 ctor 中就可以使用类的 vtbl 中的数据了
        setmetatable(obj, { __index = _class[class_type] })     -- 设置新表的元表为父类表的 vtbl 虚函数表，注意不是类自己
        
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
        
        -- setmetatable(obj, { __index = _class[class_type] })     -- 设置新表的元表为父类表的 vtbl 虚函数表，注意不是类自己
        
        return obj
    end
    
    local vtbl = {}
    _class[class_type] = vtbl     -- 几率这个类的虚函数表

   -- 类也可以查找自己的数据成员和成员函数
   -- 设置当前类的元表为 vtbl 表，所有向 class() 返回的表中添加数据，都是添加到这个表的 vtbl 表中 
    setmetatable(class_type, {__newindex =
        function(t, k, v)
            vtbl[k]=v       -- 添加一个成员完全是重新生成
        end
    , __index = vtbl
    })
   
   -- 如果父类表存在，设置 vtbl 表的元表为父类表
    if super then
        setmetatable(vtbl, {__index =
            function(t, k)
                local ret = _class[super][k] -- 获取 super 类的 vtbl 虚函数表
                vtbl[k] = ret       -- 读取的时候从父类查找
                return ret
            end
        })
    end
   
    return class_type
end
]]

-- 直接采用表的形式
function class(super)
    local class_type = {}       -- 返回的类表
    class_type.ctor = false     -- 构造构造函数默认是没有的
    class_type.super = super    -- 设置父类表，可以通过 super 这个字段访问父类
    
    class_type.new = function(...)    -- 以这个表为元表，生成一个新的表
        local obj={}
        
        -- 生成表后，直接赋值元表为类的表，这样 ctor 中就可以使用类中的数据了
        setmetatable(obj, { __index = class_type })     -- 设置新表的元表为父类表
        
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
        
        return obj
    end
   
   -- 如果父类表存在，设置 vtbl 表的元表为父类表
    if super then
        setmetatable(vtbl, {__index =
            function(t, k)
                local ret = super[k] -- 获取 super 类的表
                class_type[k] = ret       -- 读取的时候从父类查找，保存到自己类表中，以后不用再次查找了
                return ret
            end
        })
    end
   
    return class_type
end