MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TimerFunctionObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mHandle = nil;
    self.mThis = nil;
    self.mParam = nil;
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

function M:call(param)
    self.mParam = param;

    if(nil ~= self.mThis and nil ~= self.mHandle) then
        return self.mHandle(self.mThis, self.mParam);
    elseif nil ~= self.mHandle then
        return self.mHandle(self.mParam);
    else
        return 0
    end
end

return M;