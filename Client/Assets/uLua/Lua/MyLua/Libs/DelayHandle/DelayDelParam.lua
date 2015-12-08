require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.DelayHandle.DelayHandleParamBase"

local M = GlobalNS.Class(GlobalNS.DelayHandleParamBase)
M.clsName = "DelayDelParam"
GlobalNS[M.clsName] = M

function M:ctor()
    
end

return M