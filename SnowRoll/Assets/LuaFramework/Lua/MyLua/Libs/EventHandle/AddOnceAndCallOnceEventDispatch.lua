MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.EventHandle.EventDispatch");

local M = GlobalNS.Class(GlobalNS.EventDispatch);
M.clsName = "AddOnceAndCallOnceEventDispatch";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:addEventHandle(pThis, handle)
    if (not self:isExistEventHandle(pThis, handle)) then
        M.super.addEventHandle(self, pThis, handle);
    end
end

function M:dispatchEvent(dispatchObject)
    M.super.dispatchEvent(self, dispatchObject);
	
    self:clearEventHandle();
end

return M;