MLoader("MyLua.Libs.Core.Prequisites");
MLoader("MyLua.Libs.Network.NetMgr");

-- 全局变量表，自己定义的所有的变量都放在 GCtx 表中，不放在 GlobalNS 表中
GCtx = {};
local M = GCtx;
local this = GCtx;

function M.ctor()
	
end

function M.dtor()
	
end

function M.preInit()
    this.mConfig = GlobalNS.new(GlobalNS.Config);
    this.mTimerIdGentor = GlobalNS.new(GlobalNS.UniqueIdGentor);
    this.mProcessSys = GlobalNS.new(GlobalNS.ProcessSys);
    this.mTimerMgr = GlobalNS.new(GlobalNS.TimerMgr);
    this.mNetMgr = GlobalNS.NetMgr;     -- Net 使用原始的表
    this.mLogSys = GlobalNS.new(GlobalNS.LogSys);
    this.mWidgetStyleMgr = GlobalNS.new(GlobalNS.WidgetStyleMgr);
	this.mUiMgr = GlobalNS.new(GlobalNS.UIMgr);
	this.mTableSys = GlobalNS.new(GlobalNS.TableSys);
	
    this.mNetCmdNotify = GlobalNS.new(GlobalNS.NetCmdNotify);
end

function M.interInit()
    GlobalNS.CSSystem.init();
    this.mNetMgr:init();
	GlobalNS.NoDestroyGo.init();
	this.mUiMgr:init();
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