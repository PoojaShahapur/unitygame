-- 静态类，不能继承

MLoader("MyLua.Libs.Core.GlobalNS");

local StaticClass = function (...)
    local classType = {};        -- 返回的类表
    classType.dataType = "Class";   -- 表的类型
    return classType;
end

GlobalNS["StaticClass"] = StaticClass;

return StaticClass;