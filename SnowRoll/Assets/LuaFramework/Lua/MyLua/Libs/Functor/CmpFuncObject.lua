MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "CmpFuncObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mThis = nil;
    self.mHandle = nil;
end

function M:dtor()

end

function M:setPThisAndHandle(pThis, handle)
	self.mThis = pThis;
	self.mHandle = handle;
end

function M:clear()
    self.mHandle = nil;
    self.mThis = nil;
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

function M:callOneParam(param)
    if(nil ~= self.mThis and nil ~= self.mHandle) then
        return self.mHandle(self.mThis, param);
    elseif (nil ~= self.mHandle) then
        return self.mHandle(param);
    else
        return 0;
    end
end

function M:callTwoParam(oneParam, twoParam)
    if(nil ~= self.mThis and nil ~= self.mHandle) then
        return self.mHandle(self.mThis, oneParam, twoParam);
    elseif (nil ~= self.mHandle) then
        return self.mHandle(oneParam, twoParam);
    else
        return 0;
    end
end

return M;