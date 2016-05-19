require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestTimer";
GlobalNS[M.clsName] = M;

function M:run()
    --self:testDefinite();
    --self:test_2();
    self:test_3();
end

-- 测试有限次循环
function M:testDefinite()
    local index = 0;
    local timer = nil;
    timer = GlobalNS.new(GlobalNS.TimerItemBase);
    timer.m_totalTime = 2;
    timer:setFuncObject(self, self.onTimerEnd);
    GCtx.m_timerMgr:addTimer(timer);
    
    timer = GlobalNS.new(GlobalNS.TimerItemBase);
    timer.m_totalTime = 1;
    timer:setFuncObject(self, self.onTimerEnd);
    GCtx.m_timerMgr:addTimer(timer);
    
    while(index < 100) do
        GCtx.m_processSys:advance(0.6);
        index = index + 1;
    end
end

function M:test_2()
    local index = 0;
    local timer = nil;
    timer = GlobalNS.new(GlobalNS.TimerItemBase);
    timer.m_totalTime = 2;
    timer.m_bInfineLoop = true;
    timer:setFuncObject(self, self.onTimerEnd);
    GCtx.m_timerMgr:addTimer(timer);
    
    while(index < 100) do
        GCtx.m_processSys:advance(0.6);
        index = index + 1;
    end
end

function M:test_3()
    local index = 0;
    local timer = nil;
    timer = GlobalNS.new(GlobalNS.TimerItemBase);
    timer.m_totalTime = 2;
    timer:setFuncObject(self, self.onTimerEnd);
    GCtx.m_timerMgr:addTimer(timer);
    
    while(index < 100) do
        GCtx.m_processSys:advance(0.6);
        index = index + 1;
        
        if(index == 2) then
            timer = GlobalNS.new(GlobalNS.TimerItemBase);
            timer.m_totalTime = 1;
            timer:setFuncObject(self, self.onTimerEnd);
            GCtx.m_timerMgr:addTimer(timer);
        end
    end
end

function M:onTimerEnd(timer)
    print("TestTimer:onTimerEnd called");
end

return M;