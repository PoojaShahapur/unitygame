require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.UI.UICore.Form"

local M = GlobalNS.Class(GlobalNS.Form)
M.clsName = "UILua"
GlobalNS[M.clsName] = M

function M:ctor()
    
end

return M