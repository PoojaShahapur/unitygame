MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "PrioritySort";
GlobalNS[M.clsName] = M;

M.ePS_Less = 0;       -- 小于
M.ePS_Great = 1;      -- 大于

return M;