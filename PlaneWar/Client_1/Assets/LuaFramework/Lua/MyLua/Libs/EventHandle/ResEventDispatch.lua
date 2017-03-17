MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.EventHandle.EventDispatch");

local M = GlobalNS.Class(GlobalNS.EventDispatch);
M.clsName = "ResEventDispatch";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:dtor()

end

function M:dispatchEvent(dispatchObject)
    M.super.dispatchEvent(self, dispatchObject);
	
    self:clearEventHandle();
end

return M;