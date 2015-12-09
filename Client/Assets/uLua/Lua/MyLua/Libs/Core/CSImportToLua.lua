-- 使用 CS 反射绑定 CS 到 lua

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

local M = GlobalNS.StaticClass()
M.clsName = "CSImportToLua"
GlobalNS[M.clsName] = M

function M:ctor()

end

M.UtilApi = luanet.import_type("SDK.Lib.UtilApi")        -- 参数一定不能空

M.ctor()

return M