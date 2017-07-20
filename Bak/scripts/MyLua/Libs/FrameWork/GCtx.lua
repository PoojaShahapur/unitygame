MLoader("MyLua.Libs.Core.Prequisites");
MLoader("MyLua.Libs.GameData.PreDataLoader");

-- 全局变量表，自己定义的所有的变量都放在 GCtx 表中，不放在 GlobalNS 表中
GCtx = {};
local M = GCtx;
local this = GCtx;

function M.ctor()
	
end

function M.dtor()
	
end

function M.preInit()
	--主要执行构造
    this.mConfig = GlobalNS.new(GlobalNS.Config);
    this.mTimerIdGentor = GlobalNS.new(GlobalNS.UniqueNumIdGen);
    this.mProcessSys = GlobalNS.new(GlobalNS.ProcessSys);
	
	this.mTickMgr = GlobalNS.new(GlobalNS.TickMgr);
    this.mTimerMgr = GlobalNS.new(GlobalNS.TimerMgr);
    this.mNetMgr = GlobalNS.NetMgr;     -- Net 使用原始的表
    this.mLogSys = GlobalNS.new(GlobalNS.LogSys);
	
    --this.mWidgetStyleMgr = GlobalNS.new(GlobalNS.WidgetStyleMgr);
	--this.mUiMgr = GlobalNS.new(GlobalNS.UIMgr);
	this.mTableSys = GlobalNS.new(GlobalNS.TableSys);
	
    this.mNetCmdNotify = GlobalNS.new(GlobalNS.NetCmdNotify);
	this.mNetCmdNotify_KBE = GlobalNS.new(GlobalNS.NetCmdDispHandle_KBE);

    this.mGameData = GlobalNS.new(GlobalNS.GameData);
    this.mGoodsData = GlobalNS.new(GlobalNS.GoodsData);
    this.mSignData = GlobalNS.new(GlobalNS.SignData);
	this.mPlayerData = GlobalNS.new(GlobalNS.PlayerData);
    this.mFlyPos = GlobalNS.new(GlobalNS.FlyPos);
    this.mSocialData = GlobalNS.new(GlobalNS.SocialData);
    this.mTeamData = GlobalNS.new(GlobalNS.TeamData);
	
	this.mFrameUpdateStatistics = GlobalNS.new(GlobalNS.FrameUpdateStatistics);
	this.mLangMgr = GlobalNS.new(GlobalNS.LangMgr);
	this.mBeginnerGuideSys = GlobalNS.new(GlobalNS.BeginnerGuideSys);
	this.mNumNodeAnimSys = GlobalNS.new(GlobalNS.NumNodeAnimSys);
end

function M.execInit()
	--主要执行 init 初始化
    --GlobalNS.CSSystem.init();
    this.mNetMgr:init();
	--GlobalNS.NoDestroyGo.init();
	this.mTableSys:init();
	--this.mUiMgr:init();
	
	this.mPlayerData:init();
    --this.mFlyPos:init();
	
	this.mFrameUpdateStatistics:init();
	this.mLangMgr:init();
	--this.mBeginnerGuideSys:init();
	--this.mNumNodeAnimSys:init();
end

function M.postInit()
	--主要处理真正的逻辑
    -- 加载逻辑处理
    GlobalNS.ClassLoader.loadClass("MyLua.Libs.FrameWork.GlobalEventCmd");
end

function M.init()
    this.preInit();
    this.execInit();
    this.postInit();
end

function M.preDispose()
	
end

function M.execDispose()
	this.mUiMgr:dispose();
	this.mNumNodeAnimSys:dispose();
end

function M.poseDispose()
	
end

function M.dispose()
	this.preDispose();
	this.execDispose();
	this.poseDispose();
end

M.ctor();
M.init();

return M;