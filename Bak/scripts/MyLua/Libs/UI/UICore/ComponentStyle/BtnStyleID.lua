MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "BtnStyleID";
GlobalNS[M.clsName] = M;

function M.ctor()
    M.eBSID_None = 0;
    M.eTotal = 1;
end

M.ctor();

return M;