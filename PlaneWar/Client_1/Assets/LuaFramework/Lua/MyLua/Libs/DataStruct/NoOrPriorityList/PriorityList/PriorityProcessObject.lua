MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

-- 优先级队列对象
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "PriorityProcessObject";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mPriorityObject = nil;
	self.mPriority = 0.0;
end

return M;