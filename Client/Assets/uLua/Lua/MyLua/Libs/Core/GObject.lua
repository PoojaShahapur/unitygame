-- 所有的类的基类

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"

local M = GlobalNS.Class()
M.clsName = "GObject"
GlobalNS[M.clsName] = M

--[[
function M.__newindex(tbl, key, value)
    
end
]]

return M