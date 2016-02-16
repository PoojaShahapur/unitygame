-- 生成唯一 ID

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "UniqueIdGentor";
GlobalNS[M.clsName] = M;

function M:ctor()
   self.m_curIdx = 0;
   self.m_preIdx = 0;
end

function M:next()
    self.m_preIdx = self.m_curIdx;
    self.m_curIdx = self.m_curIdx + 1;
    return self.m_preIdx;
end

return M;