MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.DelayHandle.DelayHandleParamBase");

local M = GlobalNS.Class(GlobalNS.DelayHandleParamBase);
M.clsName = "DelayAddParam";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_priority = 0;
end

return M;