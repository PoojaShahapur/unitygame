MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.DelayHandle.DelayHandleParamBase");

local M = GlobalNS.Class(GlobalNS.DelayHandleParamBase);
M.clsName = "DelayDelParam";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

return M;