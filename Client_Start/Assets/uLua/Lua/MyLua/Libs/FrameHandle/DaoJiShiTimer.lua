--[[
    @brief 倒计时定时器
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.FrameHandle.TimerItemBase"

local M = GlobalNS.Class(GlobalNS.TimerItemBase);
M.clsName = "DaoJiShiTimer";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:setTotalTime(value)
    M.super.setTotalTime(self, value);
    self.m_curRunTime = value;
end

function M:getRunTime()
    return self.m_totalTime - self.m_curRunTime;
end

-- 如果要获取剩余的倒计时时间，使用 getLeftCallTime 
function M:getLeftRunTime()
    return self.m_curRunTime;
end

function M:OnTimer(delta)
    if self.m_disposed then
        return;
    end

    self.m_curRunTime = self.m_curRunTime - delta;
    if(self.m_curRunTime < 0) then
        self.m_curRunTime = 0;
    end
    self.m_intervalLeftTime = self.m_intervalLeftTime + delta;

    if self.m_bInfineLoop then
        self:checkAndDisp();
    else
        if self.m_curRunTime <= 0 then
            self:disposeAndDisp();
        else
            self:checkAndDisp();
        end
    end
end

function M:reset()
    self.m_curRunTime = self.m_totalTime;
    self.m_curCallTime = 0;
    self.m_intervalLeftTime = 0;
    self.m_disposed = false;
end

return M;