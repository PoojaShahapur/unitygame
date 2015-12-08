require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

local M = GlobalNS.StaticClass()
local this = M
M.clsName = "UtilApi"
GlobalNS[M.clsName] = M

function M.ctor()

end

M.ctor()        -- 构造

return M