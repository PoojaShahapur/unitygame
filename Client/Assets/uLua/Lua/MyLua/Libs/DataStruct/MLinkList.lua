--连接列表实现

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MLinkList";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_header = nil;
    self.m_tail = nil;
end

function M:dtor()

end

function M:isEmpty()
    return self.m_header == nil and self.m_tail == nil;
end

return M;