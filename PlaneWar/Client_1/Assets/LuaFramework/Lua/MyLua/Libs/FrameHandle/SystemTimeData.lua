MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "SystemTimeData";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mPreTime = 0;              -- 上一次更新时的秒数
    self.mCurTime = 0;              -- 正在获取的时间
    self.mDeltaSec = 0;             -- 两帧之间的间隔
end

function M:getDeltaSec()
    return self.mDeltaSec;
end

function M:setDeltaSec(value)
    self.mDeltaSec = value;
end

function M:getCurTime()
    return self.mCurTime;
end

function M:setCurTime(value)
    self.mCurTime = value;
end

function M:nextFrame()
    self.mPreTime = self.mCurTime;
end

return M;