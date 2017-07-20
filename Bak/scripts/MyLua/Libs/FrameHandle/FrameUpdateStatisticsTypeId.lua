MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "FrameUpdateStatisticsTypeId";
GlobalNS[M.clsName] = M;

M.eFUST_TimerMgr = 0; 			-- 定时器
M.eFUST_NumNodeAnimSys = 1;		-- 动画系统

M.eFUST_Count = 2;				-- 总数量

return M;