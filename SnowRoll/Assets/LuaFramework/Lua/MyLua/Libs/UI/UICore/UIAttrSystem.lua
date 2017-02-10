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
	M[GlobalNS.UIFormId.eUIShop_SkinPanel] = {
            mWidgetPath = "UI/UIPack/UIPack.prefab",
            mLuaScriptPath = "MyLua.UI.UIPack.UIPack",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIShop_SkinPanel] = {
            mWidgetPath = "UI/UIShop_SkinPanel/UIShop_SkinPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIShop_SkinPanel.UIShop_SkinPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUISettingsPanel] = {
            mWidgetPath = "UI/UISettingsPanel/UISettingsPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UISettingsPanel.UISettingsPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIMessagePanel] = {
            mWidgetPath = "UI/UIMessagePanel/UIMessagePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIMessagePanel.UIMessagePanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUISignPanel] = {
            mWidgetPath = "UI/UISignPanel/UISignPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UISignPanel.UISignPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIDayAwardPanel] = {
            mWidgetPath = "UI/UISignPanel/UIDayAwardPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UISignPanel.UIDayAwardPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIOtherAwardPanel] = {
            mWidgetPath = "UI/UISignPanel/UIOtherAwardPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UISignPanel.UIOtherAwardPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIAccountPanel] = {
            mWidgetPath = "UI/UIAccountPanel/UIAccountPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIAccountPanel.UIAccountPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIAccountAvatarPanel] = {
            mWidgetPath = "UI/UIAccountPanel/UIAccountAvatarPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIAccountPanel.UIAccountAvatarPanel",
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