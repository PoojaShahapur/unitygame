--导入公用文件
MLoader("MyLua.Libs.FrameWork.GCtx");
--导入登陆模块
MLoader("MyLua.Module.Login.LoginCommon");
--导入游戏模块
MLoader("MyLua.Module.Game.GameCommon");

-- 定义 Application 应用程序表

local M = {};
M.clsName = "AppModule";
GlobalNS[M.clsName] = M;
local this = M;

function M.ctor()
	-- 加载登陆模块
	this.mLoginModule = GlobalNS.new(GlobalNS.LoginModule);
    this.mGameModule = GlobalNS.new(GlobalNS.GameModule);
end

function M.init()
	this.mLoginModule:init();
    this.mGameModule:init();
end

function M.run()
	
end

--[[
M.ctor();
M.init();
M.run();
]]

return M;