MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "CallFuncObjectBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_handle = nil;
    self.m_pThis = nil;
    self.m_param = nil;
end

function M:dtor()

end

function M:setPThisAndHandle(pThis, handle, param)
	
end

function M:clear()
    self.m_pThis = nil;
    self.m_handle = nil;
    self.m_param = nil;
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

function M:call()
    
end

return M;