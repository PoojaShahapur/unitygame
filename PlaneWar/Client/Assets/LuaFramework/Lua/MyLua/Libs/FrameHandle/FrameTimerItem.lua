--[[
    @brief 定时器，这个是不断增长的
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.DelayHandle.IDelayHandleItem");

local M = GlobalNS.Class(GlobalNS.IDelayHandleItem);
M.clsName = "FrameTimerItem";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mInternal = 1;
    self.mTotalFrameCount = 1;
    self.mCurFrame = 0;
    self.mIsInfineLoop = false;
    self.mCurLeftFrame = 0;
    self.mTimerDisp = nil;
    self.mThis = nil
    self.mIsDisposed = false;
end

function M:OnFrameTimer()
    if self.mIsDisposed then
        return;
    end

    self.mCurFrame = self.mCurFrame + 1;
    self.mCurLeftFrame = self.mCurLeftFrame + 1;

    if self.mIsInfineLoop then
        if self.mCurLeftFrame == self.mInternal then
            self.mCurLeftFrame = 0;

            if self.mTimerDisp ~= nil then
                self:mTimerDisp(self);
            end
        end
    else
        if self.mCurFrame == self.mTotalFrameCount then
            self.mIsDisposed = true;
            if self.mTimerDisp ~= nil then
                self.mTimerDisp(self.mThis);
            end
        else
            if self.mCurLeftFrame == self.mInternal then
                self.mCurLeftFrame = 0;
                if self.mTimerDisp ~= nil then
                    self.mTimerDisp(self.mThis);
                end
            end
        end
    end
end

function M:reset()
    self.mCurFrame = 0;
    self.mCurLeftFrame = 0;
    self.mIsDisposed = false;
end

function M:setClientDispose()

end

function M:getClientDispose()
    return false;
end

return M;