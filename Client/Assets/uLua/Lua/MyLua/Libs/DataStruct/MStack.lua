--堆栈实现

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.DataStruct.MList"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MStack";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_data = GlobalNS.new(GlobalNS.MList)
end

function M:dtor()

end

function M:push(value)
    self.m_data:add(value)
end

function M:pop()
    local ret;
    ret = self.m_data:removeAtAndRet(1);
    return ret;
end

function M:front()
    local ret;
    ret = self.m_data:at(1);
    return ret;
end

return M;