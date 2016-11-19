--堆栈实现

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.DataStruct.MList");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MStack";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_list = GlobalNS.new(GlobalNS.MList)
end

function M:dtor()

end

function M:setFuncObject(pThis, func)
    self.m_list:setFuncObject(pThis, func);
end

function M:push(value)
    self.m_list:insert(0, value)
end

function M:pop()
    local ret;
    ret = self.m_list:removeAtAndRet(0);
    return ret;
end

function M:front()
    local ret;
    ret = self.m_list:at(0);
    return ret;
end

function M:removeAllEqual(value)
    self.m_list:removeAllEqual(value);
end

return M;