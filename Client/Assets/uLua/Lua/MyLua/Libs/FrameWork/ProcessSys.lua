require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.FrameHandle.TimerItemBase"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ProcessSys";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:advance(delta)
    GCtx.m_timerMgr.Advance(delta);
end

return M;