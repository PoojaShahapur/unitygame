--[[
    @brief 可分发的对象
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.EventHandle.EventDispatch"

local M = GlobalNS.Class(GlobalNS.GObject:new())
M.clsName = "IDispatchObject"
GlobalNS[M.clsName] = M

function M:ctor()

end

return M