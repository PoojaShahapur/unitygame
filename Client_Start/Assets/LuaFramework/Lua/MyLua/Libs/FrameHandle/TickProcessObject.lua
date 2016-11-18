require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TickProcessObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_tickObject = nil;
    self.m_priority = 0;
end

return M;