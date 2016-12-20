MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.CallFuncObjectBase);
M.clsName = "CallFuncObjectVarParam";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mThis = nil;
    self.mHandle = nil;
    self.mParam = nil;
end

function M:dtor()

end

function M:setPThisAndHandle(pThis, handle, ...)
	self.mThis = pThis;
	self.mHandle = handle;
	self.mParam = {...};
end

function M:call()
    if(nil ~= self.mThis and nil ~= self.mHandle) then
        return self.mHandle(self.mThis, unpack(self.mParam));
    elseif nil ~= self.mHandle then
        return self.mHandle(unpack(self.mParam));
    else
        return 0
    end
end

return M;