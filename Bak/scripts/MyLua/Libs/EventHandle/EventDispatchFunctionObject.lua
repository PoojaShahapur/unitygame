MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @brief EventDispatchFunctionObject
]]

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "EventDispatchFunctionObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mIsClientDispose = false;       -- 是否释放了资源
    self.mHandle = nil;
    self.mThis = nil;
end

function M:dtor()

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

function M:isEqual(pThis, handle)
    return pThis == self.mThis and handle == self.mHandle;
end

function M:setFuncObject(pThis, func)
    self.mThis = pThis;
    self.mHandle = func;
end

function M:call(dispObj)
    if(nil ~= self.mThis and nil ~= self.mHandle) then
        -- self.mThis:self.mHandle(dispObj);     -- 这么写好像不行
        self.mHandle(self.mThis, dispObj);
    elseif(nil ~= self.mHandle) then
        self.mHandle(dispObj);
    else
        GlobalNS.UtilApi.error("EventDispatchFunctionObject is InValid");        -- 抛出一个异常
    end
end

function M:setClientDispose()
    self.mIsClientDispose = true;
end

function M:getClientDispose()
    return self.mIsClientDispose;
end

return M;