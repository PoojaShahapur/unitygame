--[[
    @brief 定时器，这个是不断增长的
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.DelayHandle.IDelayHandleItem"

local M = GlobalNS.Class(GlobalNS.IDelayHandleItem);
M.clsName = "TimerItemBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_internal = 1;            -- 定时器间隔
    self.m_totalTime = 1;           -- 总共定时器时间
    self.m_curRunTime = 0;          -- 当前定时器运行的时间
    self.m_curCallTime = 0;         -- 当前定时器已经调用的时间
    self.m_bInfineLoop = false;     -- 是否是无限循环
    self.m_intervalLeftTime = 0;    -- 定时器调用间隔剩余时间
    self.m_timerDisp = GlobalNS.new(GlobalNS.TimerFunctionObject);         -- 定时器分发
    self.m_disposed = false;        -- 是否已经被释放
    self.m_bContinuous = false;     -- 是否是连续的定时器
end

function M:setFuncObject(pThis, func)
    self.m_timerDisp:setPThisAndHandle(pThis, func);
end

function M:setTotalTime(value)
    self.m_totalTime = value;
end

function M:getRunTime()
    return self.m_curRunTime;
end

function M:getCallTime()
    return self.m_curCallTime;
end

-- 在调用回调函数之前处理
function M:onPreCallBack()
    
end

function M:OnTimer(delta)
    if self.m_disposed then
        return;
    end

    self.m_curRunTime = self.m_curRunTime + delta;
    if(self.m_curRunTime > self.m_totalTime) then
        self.m_curRunTime = self.m_totalTime;
    end
    self.m_intervalLeftTime = self.m_intervalLeftTime + delta;

    if self.m_bInfineLoop then
        self:checkAndDisp();
    else
        if self.m_curRunTime >= self.m_totalTime then
            self:disposeAndDisp();
        else
            self:checkAndDisp();
        end
    end
end

function M:disposeAndDisp()
    self.m_disposed = true;
    self.m_curCallTime = self.m_totalTime;
    self:onPreCallBack();
    
    if (self.m_timerDisp:isValid()) then
        self.m_timerDisp:call(self);
    end
end

function M:checkAndDisp()
    if(self.m_bContinuous) then
        self:continueCheckAndDisp();
    else
        self:discontinueCheckAndDisp();
    end
end

-- 连续的定时器
function M:continueCheckAndDisp()
    while (self.m_intervalLeftTime >= self.m_internal) do
        self.m_curCallTime = self.m_curCallTime + self.m_internal;
        self.m_intervalLeftTime = self.m_intervalLeftTime - self.m_internal;
        self:onPreCallBack();

        if (self.m_timerDisp:isValid()) then
            self.m_timerDisp:call(self);
        end
    end
end

-- 不连续的定时器
function M:discontinueCheckAndDisp()
    if (self.m_intervalLeftTime >= self.m_internal) then
        self.m_curCallTime = self.m_curCallTime + ((math.floor(self.m_intervalLeftTime / self.m_internal)) * self.m_internal);
        self.m_intervalLeftTime = self.m_intervalLeftTime % self.m_internal;   -- 只保留余数
        self:onPreCallBack();

        if (self.m_timerDisp:isValid()) then
            self.m_timerDisp:call(self);
        end
    end
end

function M:reset()
    self.m_curRunTime = 0;
    self.m_curCallTime = 0;
    self.m_intervalLeftTime = 0;
    self.m_disposed = false;
end

function M:setClientDispose()
    
end

function M:getClientDispose()
    return false;
end

return M;