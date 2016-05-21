require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.StaticClass();
M.clsName = "UIAttrSystem";
GlobalNS[M.clsName] = M;

function M.ctor()
    M[GlobalNS.UIFormID.eUITest] = {
            m_widgetPath = "UI/UITest/UITest.prefab",
            m_luaScriptPath = "MyLua.UI.UITest.UITest",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
        };
end

M.ctor();

return M;