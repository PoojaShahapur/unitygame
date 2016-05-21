require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.StaticClass();
M.clsName = "UIAttrSystem";
GlobalNS[M.clsName] = M;

function M.ctor()
    M[GlobalNS.UIFormID.eUITest] = {
            m_widgetPath = "UI/UILua/UITest.prefab",
            m_luaScriptPath = "MyLua/UI/UITest/UITest",
			m_luaScriptTableName = "GlobalNS.UILua",
        };
end

M.ctor();

return M;