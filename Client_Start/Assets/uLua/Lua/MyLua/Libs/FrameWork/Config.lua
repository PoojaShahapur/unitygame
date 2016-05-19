require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "Config";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_allowCallCS = false;     -- 是否允许调用 CS
end

function M:isAllowCallCS()
    return self.m_allowCallCS;
end

return M;