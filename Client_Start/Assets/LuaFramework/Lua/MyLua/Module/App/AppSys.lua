--导入公用文件
require "MyLua.Libs.FrameWork.GCtx"
--导入登陆模块
require "MyLua.Module.Login.LoginCommon"
--导入游戏模块
require "MyLua.Module.Game.GameCommon"

-- 定义 Application 应用程序表

local M = {};
M.clsName = "AppSys";
GlobalNS[M.clsName] = M;
local this = M;

function M.ctor()
	-- 加载登陆模块
	this.mLoginSys = GlobalNS.new(GlobalNS.LoginSys);
    this.mGameSys = GlobalNS.new(GlobalNS.GameSys);
end

function M.init()
	this.mLoginSys:init();
    this.mGameSys:init();
end

function M.run()
	
end

--[[
M.ctor();
M.init();
M.run();
]]

return M;