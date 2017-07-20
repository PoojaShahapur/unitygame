MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "DailyTaskTypeId";
GlobalNS[M.clsName] = M;

M.eLoginReward 				= 0;		-- 登陆奖励
M.eSignReward 				= 1;		-- 签到奖励
M.eShareReward 				= 2;		-- 分享快乐
M.eTVReward 				= 3;		-- 我要上电视
M.eFlyPlaneReward 			= 4;		-- 我要开飞机
M.eDespairReward 			= 5;		-- 我要体验绝望
M.eSoldierReward 			= 6;		-- 幽浮战士
M.eOpenBlackReward 			= 7;		-- 开黑走起
M.eGreatGodLoginReward 		= 8;		-- 大神降临
M.eWorldWarReward 			= 9;		-- 一战封神
M.eDailyTaskTotal 			= 10;		-- 每日任务总是

return M;