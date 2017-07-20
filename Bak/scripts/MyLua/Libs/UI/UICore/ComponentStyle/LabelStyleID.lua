MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "LabelStyleID";
GlobalNS[M.clsName] = M;

function M.ctor()
    M.eLSID_None = 0;
    M.eTotal = 1;
end

M.ctor();

return M;