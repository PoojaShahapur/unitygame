require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.StaticClass()
M.clsName = "UIFormID"
GlobalNS[M.clsName] = M

-- 定义 Form ID
function M.ctor()
    M.eUILua = 100
end

M.ctor()    -- 构造函数调用

--[[******************************************]]
-- 定义属性
local M = GlobalNS.StaticClass()
M.clsName = "UIAttrSystem"
GlobalNS[M.clsName] = M

function M:ctor()
    M[GlobalNS.UIFormID.eUILua] = {
            m_widgetPath = "UI/UILua/UILua.prefab",
            m_luaScriptPath = "MyLua/UI/UILua/UILua",
            m_luaScriptTableName = "GlobalNS.UILua"
        }
end

M.ctor()

return M