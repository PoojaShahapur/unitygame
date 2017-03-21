MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--连接列表实现
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MLinkListNode";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mData = nil;
    self.mPrev = nil
    self.mNext = nil;
end

function M:dtor()

end

function M:setData(value)
    self.mData = value;
end

function M:getData()
    return self.mData
end

function M:setPrev(value)
    self.mPrev = value
end

function M:getPrev()
    return self.mPrev
end

function M:setNext(value)
    self.mNext = value;
end

function M:getNext()
    return self.mNext
end

return M;