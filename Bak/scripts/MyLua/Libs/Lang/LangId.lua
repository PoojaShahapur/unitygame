MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "LangId";
GlobalNS[M.clsName] = M;

M.zh_CN = 0;

return M;