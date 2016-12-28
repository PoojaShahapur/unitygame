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
	M[GlobalNS.UIFormId.eUITest] = {
            mWidgetPath = "UI/UITest/UITest.prefab",
            mLuaScriptPath = "MyLua.UI.UITest.UITest",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIStartGame] = {
            mWidgetPath = "UI/UIStartGame/UIStartGame.prefab",
            mLuaScriptPath = "MyLua.UI.UIStartGame.UIStartGame",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIRankListPanel] = {
            mWidgetPath = "UI/UIRankListPanel/UIRankListPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIRankListPanel.UIRankListPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIRelivePanel] = {
            mWidgetPath = "UI/UIRelivePanel/UIRelivePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIRelivePanel.UIRelivePanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIPlayerDataPanel] = {
            mWidgetPath = "UI/UIPlayerDataPanel/UIPlayerDataPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIPlayerDataPanel.UIPlayerDataPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIForwardForce] = {
            mWidgetPath = "UI/UIForwardForce/UIForwardForce.prefab",
            mLuaScriptPath = "MyLua.UI.UIForwardForce.UIForwardForce",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIOptionPanel] = {
            mWidgetPath = "UI/UIOptionPanel/UIOptionPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIOptionPanel.UIOptionPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUITopXRankPanel] = {
            mWidgetPath = "UI/UITopXRankPanel/UITopXRankPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UITopXRankPanel.UITopXRankPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIConsoleDlg] = {
            mWidgetPath = "UI/UIConsoleDlg/UIConsoleDlg.prefab",
            mLuaScriptPath = "MyLua.UI.UIConsoleDlg.UIConsoleDlg",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIRockerPanel] = {
            mWidgetPath = "UI/UIRockerPanel/UIRockerPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIRockerPanel.UIRockerPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIPack] = {
            mWidgetPath = "UI/UIRockerPanel/UIPack.prefab",
            mLuaScriptPath = "MyLua.UI.UIPack.UIPack",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	--[[替换占位符(勿删)--]]
end

M.ctor();

return M;