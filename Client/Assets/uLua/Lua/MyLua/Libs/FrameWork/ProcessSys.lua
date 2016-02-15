require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ProcessSys";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:advance(delta)
    print("ProcessSys:advance");
    GCtx.m_timerMgr:Advance(delta);
end

return M;