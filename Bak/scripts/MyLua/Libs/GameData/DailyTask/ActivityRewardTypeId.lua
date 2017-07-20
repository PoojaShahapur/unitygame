MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "ActivityRewardTypeId";
GlobalNS[M.clsName] = M;

M.eActivityReward_0 				= 0;		-- 100
M.eActivityReward_1 				= 1;		-- 300
M.eActivityReward_2 				= 2;		-- 500
M.eActivityReward_3 				= 3;		-- 750
M.eActivityReward_4 				= 4;		-- 1000
M.eActivityReward_Total 			= 5;		-- Total

return M;