MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

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
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIStartGame] = {
            mWidgetPath = "UI/UIStartGame/UIStartGame.prefab",
            mLuaScriptPath = "MyLua.UI.UIStartGame.UIStartGame",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIRankListPanel] = {
            mWidgetPath = "UI/UIRankListPanel/UIRankListPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIRankListPanel.UIRankListPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUILevelChangePanel] = {
            mWidgetPath = "UI/UIRankListPanel/UILevelChangePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIRankListPanel.UILevelChangePanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIRelivePanel] = {
            mWidgetPath = "UI/UIRelivePanel/UIRelivePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIRelivePanel.UIRelivePanel",
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
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIPack] = {
            mWidgetPath = "UI/UIPack/UIPack.prefab",
            mLuaScriptPath = "MyLua.UI.UIPack.UIPack",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIShop_SkinPanel] = {
            mWidgetPath = "UI/UIShop_SkinPanel/UIShop_SkinPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIShop_SkinPanel.UIShop_SkinPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUISettingsPanel] = {
            mWidgetPath = "UI/UISettingsPanel/UISettingsPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UISettingsPanel.UISettingsPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIMessagePanel] = {
            mWidgetPath = "UI/UIMessagePanel/UIMessagePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIMessagePanel.UIMessagePanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUISignPanel] = {
            mWidgetPath = "UI/UISignPanel/UISignPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UISignPanel.UISignPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIDayAwardPanel] = {
            mWidgetPath = "UI/UISignPanel/UIDayAwardPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UISignPanel.UIDayAwardPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIOtherAwardPanel] = {
            mWidgetPath = "UI/UISignPanel/UIOtherAwardPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UISignPanel.UIOtherAwardPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIAccountPanel] = {
            mWidgetPath = "UI/UIAccountPanel/UIAccountPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIAccountPanel.UIAccountPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIAccountAvatarPanel] = {
            mWidgetPath = "UI/UIAccountPanel/UIAccountAvatarPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIAccountPanel.UIAccountAvatarPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIEditInfoPanel] = {
            mWidgetPath = "UI/UIAccountPanel/UIEditInfoPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIAccountPanel.UIEditInfoPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIgiftPanel] = {
            mWidgetPath = "UI/UIAccountPanel/UIgiftPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIAccountPanel.UIgiftPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIBugReportPanel] = {
            mWidgetPath = "UI/UIBugReportPanel/UIBugReportPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIBugReportPanel.UIBugReportPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIEmoticonPanel] = {
            mWidgetPath = "UI/UIEmoticonPanel/UIEmoticonPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIEmoticonPanel.UIEmoticonPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIShareSelfPanel] = {
            mWidgetPath = "UI/UIShareSelfPanel/UIShareSelfPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIShareSelfPanel.UIShareSelfPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIFirstPlayTipPanel] = {
            mWidgetPath = "UI/UIFirstPlayTipPanel/UIFirstPlayTipPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIFirstPlayTipPanel.UIFirstPlayTipPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUIFirstHardPlayTipPanel] = {
            mWidgetPath = "UI/UIFirstPlayTipPanel/UIFirstHardPlayTipPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIFirstPlayTipPanel.UIFirstHardPlayTipPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIServerHistoryRankListPanel] = {
            mWidgetPath = "UI/UIServerHistoryRankListPanel/UIServerHistoryRankListPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIServerHistoryRankListPanel.UIServerHistoryRankListPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIItemsCirclePanel] = {
            mWidgetPath = "UI/UIItemsCirclePanel/UIItemsCirclePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIItemsCirclePanel.UIItemsCirclePanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIShareMoney] = {
            mWidgetPath = "UI/UIShareMoney/UIShareMoney.prefab",
            mLuaScriptPath = "MyLua.UI.UIShareMoney.UIShareMoney",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIItempiecePanel] = {
            mWidgetPath = "UI/UIItempiecePanel/UIItempiecePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIItempiecePanel.UIItempiecePanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIItemLevelupPanel] = {
            mWidgetPath = "UI/UIItemLevelupPanel/UIItemLevelupPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIItemLevelupPanel.UIItemLevelupPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIFriendSystemPanel] = {
            mWidgetPath = "UI/UIFriendSystemPanel/UIFriendSystemPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIFriendSystemPanel.UIFriendSystemPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIMyskinPanel] = {
            mWidgetPath = "UI/UIMyskinPanel/UIMyskinPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIMyskinPanel.UIMyskinPanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUISkinitemsCireclePanel] = {
            mWidgetPath = "UI/UISkinitemsCireclePanel/UISkinitemsCireclePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UISkinitemsCireclePanel.UISkinitemsCireclePanel",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIShop] = {
            mWidgetPath = "UI/UIShop/UIShop.prefab",
            mLuaScriptPath = "MyLua.UI.UIShop.UIShop",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIItemsColourPanel] = {
            mWidgetPath = "UI/UIItemsColourPanel/UIItemsColourPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIItemsColourPanel.UIItemsColourPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIFindFriend] = {
            mWidgetPath = "UI/UIFindFriend/UIFindFriend.prefab",
            mLuaScriptPath = "MyLua.UI.UIFindFriend.UIFindFriend",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIBulletSelectPanel] = {
            mWidgetPath = "UI/UIBulletSelectPanel/UIBulletSelectPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIBulletSelectPanel.UIBulletSelectPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIConfirmAgain] = {
            mWidgetPath = "UI/UIConfirmAgain/UIConfirmAgain.prefab",
            mLuaScriptPath = "MyLua.UI.UIConfirmAgain.UIConfirmAgain",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIItempricePanel] = {
            mWidgetPath = "UI/UIItempricePanel/UIItempricePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIItempricePanel.UIItempricePanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUITeamBattlePanel] = {
            mWidgetPath = "UI/UITeamBattlePanel/UITeamBattlePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UITeamBattlePanel.UITeamBattlePanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
    M[GlobalNS.UIFormId.eUITeamRulePanel] = {
            mWidgetPath = "UI/UITeamBattlePanel/UITeamRulePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UITeamBattlePanel.UITeamRulePanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIHowGetGoodsPanel] = {
            mWidgetPath = "UI/UIHowGetGoodsPanel/UIHowGetGoodsPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIHowGetGoodsPanel.UIHowGetGoodsPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIUseGiftsPanel] = {
            mWidgetPath = "UI/UIUseGiftsPanel/UIUseGiftsPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIUseGiftsPanel.UIUseGiftsPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIReconnectionPanel] = {
            mWidgetPath = "UI/UIReconnectionPanel/UIReconnectionPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIReconnectionPanel.UIReconnectionPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIDailytasksPanel] = {
            mWidgetPath = "UI/UIDailytasksPanel/UIDailytasksPanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIDailytasksPanel.UIDailytasksPanel",
			mLuaScriptTableName = "GlobalNS.UILua",
			mCanvasId = GlobalNS.UICanvasId.eUIFirstCanvas,
			mLayerId = GlobalNS.UILayerID.eUISecondLayer,
			mPreFormModeWhenOpen = GlobalNS.PreFormModeWhenOpen.eNONE,
			mPreFormModeWhenClose = GlobalNS.PreFormModeWhenClose.eNONE,
        };
	M[GlobalNS.UIFormId.eUIPerformancePanel] = {
            mWidgetPath = "UI/UIUseGiftsPanel/UIPerformancePanel.prefab",
            mLuaScriptPath = "MyLua.UI.UIPerformancePanel.UIPerformancePanel",
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