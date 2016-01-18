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
    self.m_curTime = 0;             -- 当前已经调用的定时器的时间
    self.m_bInfineLoop = false;     -- 是否是无限循环
    self.m_curLeftTimer = 0;        -- 当前定时器剩余的次数
    self.m_timerDisp = nil;         -- 定时器分发
    self.m_pThis = nil;
    self.m_disposed = false;        -- 是否已经被释放
end

function M:OnTimer(delta)
    if self.m_disposed then
        return;
    end

    self.m_curTime = self.m_curTime + delta;
    self.m_curLeftTimer = self.m_curLeftTimer + delta;

    if self.m_bInfineLoop then
        self:checkAndDisp();
    else
        if self.m_curTime >= self.m_totalTime then
            self:disposeAndDisp();
        else
            self:checkAndDisp();
        end
    end
end

function M:disposeAndDisp()
    self.m_disposed = true;
    if self.m_timerDisp ~= nil then
        self.m_timerDisp(self.m_pThis);
    end
end

function M:checkAndDisp()
    if self.m_curLeftTimer >= self.m_internal then
        self.m_curLeftTimer = self.m_curLeftTimer - self.m_internal;

        if (self.m_timerDisp ~= nil) then
            self.m_timerDisp(self.m_pThis);
        end
    end
end

function M:reset()
    self.m_curTime = 0;
    self.m_curLeftTimer = 0;
    self.m_disposed = false;
end

function M:setClientDispose()
end

function M:getClientDispose()
    return false;
end

return M;