MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TickProcessObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mTickObject = nil;
    self.mPriority = 0;
end

return M;