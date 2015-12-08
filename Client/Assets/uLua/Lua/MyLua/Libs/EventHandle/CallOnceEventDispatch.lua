--[[
    @brief 一次事件分发，分发一次就清理
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.EventHandle.EventDispatch"

local M = GlobalNS.Class(GlobalNS.EventDispatch)
M.clsName = "CallOnceEventDispatch"
GlobalNS[M.clsName] = M

function M:ctor()

end

function M:dispatchEvent(dispatchObject)
    self.super.dispatchEvent(self, dispatchObject);
    self:clearEventHandle();
end

return M