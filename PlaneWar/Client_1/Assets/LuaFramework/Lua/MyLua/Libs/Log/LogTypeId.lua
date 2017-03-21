local M = {};
GlobalNS["LogTypeId"] = M;

M.eLogCommon = 0;       -- 通用日志
M.eLogTest = 1;         -- 测试日志
M.eLogNoPriorityListCheck = 2; 	--优先级检查日志

return M;