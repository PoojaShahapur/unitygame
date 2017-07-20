MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

--[[
 @brief Tick 的优先级
 @brief TP TickPriority 缩写
]]
local M = GlobalNS.StaticClass();
M.clsName = "TickPriority";
GlobalNS[M.clsName] = M;

M.eTPSNodeNumAnimSys = 0;

return M;