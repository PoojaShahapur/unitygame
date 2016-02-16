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

function M:OnTimer(delta)
    if self.m_disposed then
        return;
    end

    self.m_curTime = self.m_curTime - delta;
    if(self.m_curTime < 0) then
        self.m_curTime = 0;
    end
    self.m_curLeftTimer = self.m_curLeftTimer + delta;

    if self.m_bInfineLoop then
        self:checkAndDisp();
    else
        if self.m_curTime <= 0 then
            self:disposeAndDisp();
        else
            self:checkAndDisp();
        end
    end
end

function M:reset()
    self.m_curTime = self.m_totalTime;
    self.m_curLeftTimer = 0;
    self.m_disposed = false;
end

return M;