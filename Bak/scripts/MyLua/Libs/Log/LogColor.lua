MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "LogColor";
GlobalNS[M.clsName] = M;

M.eLC_LOG = 0;
M.eLC_WARN = 1;
M.eLC_ERROR = 2;
M.eLC_Count = 3;

return M;