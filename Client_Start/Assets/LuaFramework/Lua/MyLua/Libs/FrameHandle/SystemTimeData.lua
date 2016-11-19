MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "SystemTimeData";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_preTime = 0;              -- 上一次更新时的秒数
    self.m_curTime = 0;              -- 正在获取的时间
    self.m_deltaSec = 0;             -- 两帧之间的间隔
end

function M:getDeltaSec()
    return self.m_deltaSec;
end

function M:setDeltaSec(value)
    self.m_deltaSec = value;
end

function M:getCurTime()
    return self.m_curTime;
end

function M:setCurTime(value)
    self.m_curTime = value;
end

function M:nextFrame()
    self.m_preTime = self.m_curTime;
end

return M;