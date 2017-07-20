MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "CallFuncObjectBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mHandle = nil;
    self.mThis = nil;
    self.mParam = nil;
end

function M:dtor()

end

function M:setPThisAndHandle(pThis, handle, param)
	
end

function M:clear()
    self.mThis = nil;
    self.mHandle = nil;
    self.mParam = nil;
end

function M:isValid()
    if(nil ~= self.mThis and nil ~= self.mHandle) then
        return true;
    elseif(nil ~= self.mHandle) then
        return true;
    else
        return false;
    end
end

function M:call()
    
end

return M;