MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.DataStruct.MList");

--堆栈实现
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MStack";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mList = GlobalNS.new(GlobalNS.MList)
end

function M:dtor()

end

function M:setFuncObject(pThis, func)
    self.mList:setFuncObject(pThis, func);
end

function M:push(value)
    self.mList:insert(0, value)
end

function M:pop()
    local ret;
    ret = self.mList:removeAtAndRet(0);
    return ret;
end

function M:front()
    local ret;
    ret = self.mList:at(0);
    return ret;
end

function M:removeAllEqual(value)
    self.mList:removeAllEqual(value);
end

return M;