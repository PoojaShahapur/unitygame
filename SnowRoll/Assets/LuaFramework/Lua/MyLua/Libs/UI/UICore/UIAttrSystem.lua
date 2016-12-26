MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

--[[
local M = GlobalNS.StaticClass();
M.clsName = "PreFormModeWhenOpen";
GlobalNS[M.clsName] = M;

function M.ctor()
	M.eNONE = 0;
	M.eHIDE = 1;
	M.eCLOSE = 2;
end

M.ctor();

-------------------------------------
M = GlobalNS.StaticClass();
M.clsName = "PreFormModeWhenClose";
GlobalNS[M.clsName] = M;

function M.ctor()
	M.eNONE = 0;
	M.eSHOW = 1;
end

M.ctor();
]]

-------------------------------------
local M = GlobalNS.StaticClass();
M.clsName = "UIAttrSystem";
GlobalNS[M.clsName] = M;

function M.ctor()
    
end

--如果一个文件中重复定义了多个 M ，如果不是在 ctor 调用
function M.init()
	M[GlobalNS.UIFormID.eUITest] = {
            m_widgetPath = "UI/UITest/UITest.prefab",
            m_luaScriptPath = "MyLua.UI.UITest.UITest",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
			m_preFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			m_preFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormID.eUIStartGame] = {
            m_widgetPath = "UI/UIStartGame/UIStartGame.prefab",
            m_luaScriptPath = "MyLua.UI.UIStartGame.UIStartGame",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
			m_preFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			m_preFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormID.eUIRankListPanel] = {
            m_widgetPath = "UI/UIRankListPanel/UIRankListPanel.prefab",
            m_luaScriptPath = "MyLua.UI.UIRankListPanel.UIRankListPanel",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
			m_preFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			m_preFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormID.eUIRelivePanel] = {
            m_widgetPath = "UI/UIRelivePanel/UIRelivePanel.prefab",
            m_luaScriptPath = "MyLua.UI.UIRelivePanel.UIRelivePanel",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
			m_preFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			m_preFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormID.eUIPlayerDataPanel] = {
            m_widgetPath = "UI/UIPlayerDataPanel/UIPlayerDataPanel.prefab",
            m_luaScriptPath = "MyLua.UI.UIPlayerDataPanel.UIPlayerDataPanel",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
			m_preFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			m_preFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormID.eUIForwardForce] = {
            m_widgetPath = "UI/UIForwardForce/UIForwardForce.prefab",
            m_luaScriptPath = "MyLua.UI.UIForwardForce.UIForwardForce",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
			m_preFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			m_preFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormID.eUIOptionPanel] = {
            m_widgetPath = "UI/UIOptionPanel/UIOptionPanel.prefab",
            m_luaScriptPath = "MyLua.UI.UIOptionPanel.UIOptionPanel",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
			m_preFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			m_preFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormID.eUITopXRankPanel] = {
            m_widgetPath = "UI/UITopXRankPanel/UITopXRankPanel.prefab",
            m_luaScriptPath = "MyLua.UI.UITopXRankPanel.UITopXRankPanel",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
			m_preFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			m_preFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormID.eUIConsoleDlg] = {
            m_widgetPath = "UI/UIConsoleDlg/UIConsoleDlg.prefab",
            m_luaScriptPath = "MyLua.UI.UIConsoleDlg.UIConsoleDlg",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
			m_preFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			m_preFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormID.eUIRockerPanel] = {
            m_widgetPath = "UI/UIRockerPanel/UIRockerPanel.prefab",
            m_luaScriptPath = "MyLua.UI.UIRockerPanel.UIRockerPanel",
			m_luaScriptTableName = "GlobalNS.UILua",
			m_canvasId = GlobalNS.UICanvasID.eUIFirstCanvas,
			m_layerId = GlobalNS.UILayerID.eUISecondLayer,
			m_preFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			m_preFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	--[[替换占位符(勿删)--]]
end

M.ctor();

return M;