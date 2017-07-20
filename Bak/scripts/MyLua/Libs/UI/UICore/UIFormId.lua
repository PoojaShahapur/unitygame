MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UIFormId";
GlobalNS[M.clsName] = M;

function M.ctor()
	
end

function M.init()
    --10000
	this.eUITest = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    --10001
	this.eUIStartGame = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIFirstPlayTipPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIRelivePanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIServerHistoryRankListPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIFirstHardPlayTipPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    --10004 依次类推... ...
    this.eUIRankListPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUILevelChangePanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIPlayerDataPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIOptionPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUITopXRankPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIConsoleDlg = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
	this.eUIShop_SkinPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
	this.eUIPack = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUISettingsPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIMessagePanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUISignPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIDayAwardPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIOtherAwardPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIAccountPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIAccountAvatarPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIEditInfoPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIgiftPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIBugReportPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIEmoticonPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIShareSelfPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIItemsCirclePanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIShareMoney = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIItempiecePanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIItemLevelupPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIFriendSystemPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIMyskinPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUISkinitemsCireclePanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
	this.eUIShop = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIItemsColourPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIFindFriend = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIBulletSelectPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIConfirmAgain = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIItempricePanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUITeamBattlePanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUITeamRulePanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIHowGetGoodsPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIUseGiftsPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIReconnectionPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIDailytasksPanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    this.eUIPerformancePanel = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
    --[[替换占位符(勿删)--]]
	this.eUICount = GCtx.mUiMgr.mUniqueNumIdGen:genNewId();
end

--静态表直接构造就行了，不会使用 new 操作符
M.ctor();

return M;