require "MyLua.Libs.Core.Prequisites"
require "MyLua.Libs.Network.NetMgr"
require "MyLua.Libs.Network.ProtobufUtil"

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
    this.mCSSystem = GlobalNS.new(GlobalNS.CSSystem);
    this.m_processSys = GlobalNS.new(GlobalNS.ProcessSys);
    this.m_timerMgr = GlobalNS.new(GlobalNS.TimerMgr);
    this.mNetMgr = GlobalNS.NetMgr;     -- Net 使用原始的表
    this.mProtobufUtil = GlobalNS.ProtobufUtil;     -- PB 工具
end

function M.interInit()
    this.mNetMgr.init();
end

function M.postInit()
    
end

function M.init()
    this.preInit();
    this.interInit();
    this.postInit();
end

M.ctor();
M.init();

return M;