require "MyLua.Libs.Core.Prequisites"
require "MyLua.Libs.Network.NetMgr"

-- 全局变量表，自己定义的所有的变量都放在 GCtx 表中，不放在 GlobalNS 表中
GCtx = {};
local M = GCtx;
local this = GCtx;

function M.ctor()
	
end

function M.dtor()
	
end

function M.preInit()
    this.m_config = GlobalNS.new(GlobalNS.Config);
    this.m_timerIdGentor = GlobalNS.new(GlobalNS.UniqueIdGentor);
    this.m_processSys = GlobalNS.new(GlobalNS.ProcessSys);
    this.m_timerMgr = GlobalNS.new(GlobalNS.TimerMgr);
    this.mNetMgr = GlobalNS.NetMgr;     -- Net 使用原始的表
    this.mLogSys = GlobalNS.new(GlobalNS.LogSys);
    this.m_netCmdNotify = GlobalNS.new(GlobalNS.NetCmdNotify);
    this.m_gameSys = GlobalNS.new(GlobalNS.GameSys);
end

function M.interInit()
    GlobalNS.CSSystem.init();
    this.mNetMgr:init();
    this.m_gameSys:init();
end

function M.postInit()
    -- 加载逻辑处理
    GlobalNS.ClassLoader.loadClass("MyLua.Libs.FrameWork.GlobalEventCmd");
end

function M.init()
    this.preInit();
    this.interInit();
    this.postInit();
end

M.ctor();
M.init();

return M;