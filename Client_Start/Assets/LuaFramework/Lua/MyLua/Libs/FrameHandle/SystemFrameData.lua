MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "SystemFrameData";
GlobalNS[M.clsName] = M;

function M:ctor(delta)
    self.mTotalFrameCount = 0;      -- 当前帧数
    self.mCurFrameCount = 0;        -- 当前帧数
    self.mCurTime = 0;              -- 当前一秒内时间
    self.mFps = 0;                  -- 帧率
end

function M:nextFrame(delta)
    self.mTotalFrameCount = self.mTotalFrameCount + 1;
    self.mCurFrameCount = self.mCurFrameCount + 1;
    self.mCurTime = self.mCurTime + delta;

    if self.mCurTime > 1 then
        self.mFps = self.mCurFrameCount / self.mCurTime;
        self.mCurFrameCount = 0;
        self.mCurTime = 0;
    end
end

return M;