require "MyLua/Libs/Core/Prequisites"

-- 全局变量表，自己定义的所有的变量都放在 GCtx 表中，不放在 GlobalNS 表中
GCtx = {};
local M = GCtx;
local this = GCtx;

function M.ctor()
	
end

function M.dtor()
	
end

function M.preInit()
    --this.m_csSystem = GlobalNS.new(GlobalNS.CSSystem);
    this.m_processSys = GlobalNS.new(GlobalNS.ProcessSys);
    this.m_timerMgr = GlobalNS.new(GlobalNS.TimerMgr);
end

function M.interInit()
    
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