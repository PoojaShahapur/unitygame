require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.StaticClass()
M.clsName = "UIFormID"
GlobalNS[M.clsName] = M

-- 定义 Form ID
M.eUILua = 100

-- 定义属性
local M = GlobalNS.StaticClass()
M.clsName = "UIAttrSystem"
GlobalNS[M.clsName] = M

M[UIFormID.eUILua] = {
        m_widgetPath = "UI/UIFormLua/UIFormLua.prefab",
        m_luaScriptPath = "UI/UIFormLua/UIFormLua.lua",
        m_luaScriptTableName = "UIFormLua"
    }

return M