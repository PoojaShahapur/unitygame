require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.EventHandle.EventDispatch"

local M = GlobalNS.Class(GlobalNS.EventDispatch)
M.clsName = "ResEventDispatch"
GlobalNS[M.clsName] = M

function M:ctor()
    
end

function M:dtor()

end

function M:dispatchEvent(dispatchObject)
    self.super.dispatchEvent(self, dispatchObject)
    self:clearEventHandle();
end

return M