require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "DelayHandleObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_delayObject = nil;
    self.m_delayParam = nil;
end

return M;