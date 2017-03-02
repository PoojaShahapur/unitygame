--[[
    @brief 倒计时定时器
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.FrameHandle.TimerItemBase");

local M = GlobalNS.Class(GlobalNS.TimerItemBase);
M.clsName = "DaoJiShiTimer";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:setTotalTime(value)
    M.super.setTotalTime(self, value);
    self.mCurRunTime = value;
end

function M:getRunTime()
    return self.mTotalTime - self.mCurRunTime;
end

-- 如果要获取剩余的倒计时时间，使用 getLeftCallTime 
function M:getLeftRunTime()
    return self.mCurRunTime;
end

function M:OnTimer(delta)
    if self.mIsDisposed then
        return;
    end

    self.mCurRunTime = self.mCurRunTime - delta;
    if(self.mCurRunTime < 0) then
        self.mCurRunTime = 0;
    end
    self.mIntervalLeftTime = self.mIntervalLeftTime + delta;

    if self.mIsInfineLoop then
        self:checkAndDisp();
    else
        if self.mCurRunTime <= 0 then
            self:disposeAndDisp();
        else
            self:checkAndDisp();
        end
    end
end

function M:reset()
	M.super.reset(self);
	
    self.mCurRunTime = self.mTotalTime;
    self.mCurCallTime = 0;
    self.mIntervalLeftTime = 0;
    self.mIsDisposed = false;
end

return M;