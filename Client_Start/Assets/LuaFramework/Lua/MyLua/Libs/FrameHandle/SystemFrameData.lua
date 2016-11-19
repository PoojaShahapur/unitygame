MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "SystemFrameData";
GlobalNS[M.clsName] = M;

function M:ctor(delta)
    self.m_totalFrameCount = 0;      -- 当前帧数
    self.m_curFrameCount = 0;        -- 当前帧数
    self.m_curTime = 0;              -- 当前一秒内时间
    self.m_fps = 0;                  -- 帧率
end

function M:nextFrame(delta)
    self.m_totalFrameCount = self.m_totalFrameCount + 1;
    self.m_curFrameCount = self.m_curFrameCount + 1;
    self.m_curTime = self.m_curTime + delta;

    if self.m_curTime > 1 then
        self.m_fps = self.m_curFrameCount / self.m_curTime;
        self.m_curFrameCount = 0;
        self.m_curTime = 0;
    end
end

return M;