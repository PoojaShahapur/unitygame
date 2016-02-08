require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.CallFuncObjectBase);
M.clsName = "CallFuncObjectVarParam";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_handle = nil;
    self.m_pThis = nil;
    self.m_param = nil;
end

function M:dtor()

end

function M:setPThisAndHandle(pThis, handle, ...)
	self.m_pThis = pThis;
	self.m_handle = handle;
	self.m_param = {...};
end

function M:call()
    if(nil ~= self.m_pThis and nil ~= self.m_handle) then
        return self.m_handle(self.m_pThis, unpack(self.m_param));
    elseif nil ~= self.m_handle then
        return self.m_handle(unpack(self.m_param));
    else
        return 0
    end
end

return M;