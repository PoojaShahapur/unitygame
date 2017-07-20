MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "LogTypeId";
GlobalNS[M.clsName] = M;

M.eLogCommon = 0;       -- 通用日志
M.eLogTest = 1;         -- 测试日志
M.eLogNoPriorityListCheck = 2;  --优先级检查日志
M.eLogPack = 3;         -- 背包日志
M.eLogEventHandle = 4;  -- EventHandle 流程日志

M.eWarnCommon = 1000;       -- 通用日志

M.eErrorCommon = 2000;       -- 通用日志

return M;