MLoader("MyLua.Libs.Core.GlobalNS");

-- 内存分配
local new = function (cls, ...)
	if(nil == cls) then
		error("Class is nil");
	end
	
    local instance = {};
    instance.dataType = "Instance";
    instance.clsCode = cls;

    setmetatable(instance, cls);

    do
        local create;
        create = function(cls, ...)
            if cls.super then
                create(cls.super, ...);
            end
            if cls.ctor then
                cls.ctor(instance, ...);
            end
        end

        create(cls, ...);
    end

    return instance;
end

GlobalNS["new"] = new;

-- 删除空间
local delete = function (pThis)
	if(nil ~= pThis) then
		pThis:dtor();       -- 调用析构函数
	end
end

GlobalNS["delete"] = delete;

return GlobalNS;