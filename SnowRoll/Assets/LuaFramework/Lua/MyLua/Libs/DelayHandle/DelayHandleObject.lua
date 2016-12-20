MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "DelayHandleObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mDelayObject = nil;
    self.mDelayParam = nil;
end

return M;