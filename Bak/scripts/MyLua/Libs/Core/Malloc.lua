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

-- 删除占用的空间
local delete = function (pThis)
	if(nil ~= pThis) then
		if(nil ~= pThis.dtor) then
			pThis:dtor();       -- 调用析构函数
		elseif(nil ~= pThis.dispose) then
			pThis:dispose();       -- 调用析构函数
		end
	end
end

GlobalNS["delete"] = delete;

return GlobalNS;