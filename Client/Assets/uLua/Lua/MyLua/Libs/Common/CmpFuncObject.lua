require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "CmpFuncObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_handle = nil;
    self.m_pThis = nil;
end

function M:dtor()

end

function M:setPThisAndHandle(pThis, handle)
	self.m_pThis = pThis;
	self.m_handle = handle;
end

function M:callOneParam(param)
    if(nil ~= self.m_pThis and nil ~= self.m_handle) then
        return self.m_handle(self.m_pThis, param);
    elseif nil ~= self.m_handle then
        return self.m_handle(param);
    end
end

function M:callTwoParam(oneParam, twoParam)
    if(nil ~= self.m_pThis and nil ~= self.m_handle) then
        return self.m_handle(self.m_pThis, oneParam, twoParam);
    elseif nil ~= self.m_handle then
        return self.m_handle(oneParam, twoParam);
    else
        return 0;
    end
end

return M;