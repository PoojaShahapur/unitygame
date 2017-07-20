MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

-- 非优先级或者优先级对象接口
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "INoOrPriorityObject";
GlobalNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

return M;