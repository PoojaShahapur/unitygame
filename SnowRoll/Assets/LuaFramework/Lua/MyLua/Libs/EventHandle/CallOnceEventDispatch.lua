--[[
    @brief 一次事件分发，分发一次就清理
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.EventHandle.EventDispatch");

local M = GlobalNS.Class(GlobalNS.EventDispatch);
M.clsName = "CallOnceEventDispatch";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:dispatchEvent(dispatchObject)
    M.super.dispatchEvent(self, dispatchObject);
	
    self:clearEventHandle();
end

return M;