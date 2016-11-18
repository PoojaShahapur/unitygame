--连接列表实现

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MLinkListNode";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_data = nil;
    self.m_prev = nil
    self.m_next = nil;
end

function M:dtor()

end

function M:setData(value)
    self.m_data = value;
end

function M:getData()
    return self.m_data
end

function M:setPrev(value)
    self.m_prev = value
end

function M:getPrev()
    return self.m_prev
end

function M:setNext(value)
    self.m_next = value;
end

function M:getNext()
    return self.m_next
end

return M;