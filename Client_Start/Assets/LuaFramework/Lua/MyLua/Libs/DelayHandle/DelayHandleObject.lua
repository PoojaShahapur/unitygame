MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "DelayHandleObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_delayObject = nil;
    self.m_delayParam = nil;
end

return M;