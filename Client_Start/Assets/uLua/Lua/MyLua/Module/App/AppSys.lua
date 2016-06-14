--加载宏定义
require "MyLua.Libs.FrameWork.MacroDef"
--启动调试服务器连接
if(MacroDef.UNITY_EDITOR) then
    require("mobdebug").start()
end
--导入公用文件
require "MyLua.Libs.FrameWork.GCtx"
--导入登陆模块
require "MyLua.Module.Login.LoginCommon"
--导入游戏模块
require "MyLua.Module.Game.GameCommon"

-- 定义全局表
AppSys = {};
local M = GCtx;
local this = GCtx;

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

M.ctor();
M.init();
M.run();

return M;