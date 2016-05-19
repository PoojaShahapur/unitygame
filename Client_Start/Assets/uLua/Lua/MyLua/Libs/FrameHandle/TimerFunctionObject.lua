require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TimerFunctionObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_handle = nil;
    self.m_pThis = nil;
    self.m_param = nil;
end

function M:dtor()

end

function M:setPThisAndHandle(pThis, handle)
    self.m_pThis = pThis;
    self.m_handle = handle;
end

function M:clear()
    self.m_handle = nil;
    self.m_pThis = nil;
end

function M:isValid()
    if(nil ~= self.m_pThis and nil ~= self.m_handle) then
        return true;
    elseif(nil ~= self.m_handle) then
        return true;
    else
        return false;
    end
end

function M:call(param)
    self.m_param = param;

    if(nil ~= self.m_pThis and nil ~= self.m_handle) then
        return self.m_handle(self.m_pThis, self.m_param);
    elseif nil ~= self.m_handle then
        return self.m_handle(self.m_param);
    else
        return 0
    end
end

return M;