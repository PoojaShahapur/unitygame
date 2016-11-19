MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "DelayHandleParamBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

return M;