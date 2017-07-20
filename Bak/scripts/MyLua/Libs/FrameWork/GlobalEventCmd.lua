MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.Test.TestMain");

if(MacroDef.UNIT_TEST) then
	MLoader("MyLua.Test.TestMain");
end

--[[
    处理 CS 到 Lua 的全局事件
]]
local M = GlobalNS.StaticClass();
M.clsName = "GlobalEventCmd";
GlobalNS[M.clsName] = M;

--退出 App
function M.quitApp()
	GCtx.dispose();
end

-- 接收消息
function M.onReceiveToLua(id, buffer)
    --GCtx.mLogSys:log("GlobalEventCmd::onReceiveToLua", GlobalNS.LogTypeId.eLogCommon);
    GCtx.mNetMgr:receiveCmd(id, buffer);
end

function M.onReceiveToLuaRpc(buffer, length)
    --GCtx.mLogSys:log("GlobalEventCmd::onReceiveToLuaRpc", GlobalNS.LogTypeId.eLogCommon);
    GCtx.mNetMgr:receiveCmdRpc(buffer, length);
end

-- 接收消息, KBE
function M.onReceiveToLua_KBE(msgName, param)
    --GCtx.mLogSys:log("GlobalEventCmd::onReceiveToLua_KBE", GlobalNS.LogTypeId.eLogCommon);
	GCtx.mNetCmdNotify_KBE:handleMsg(msgName, param);
end

-- 场景加载完成
function M.onSceneLoaded()
	if(MacroDef.UNIT_TEST) then
		pTestMain = GlobalNS.new(GlobalNS.TestMain);
		pTestMain:run();
	end
end

-- 主角加载完成
function M.onPlayerMainLoaded()
    --加载场景上的UI组件，主角加载完成后再加载UI，否则UI拿不到主角数据
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIRankListPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIRelivePanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIShop_SkinPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUISettingsPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIMessagePanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUISignPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIDayAwardPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIOtherAwardPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIAccountPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIAccountAvatarPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUITeamBattlePanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIReconnectionPanel);

    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIPlayerDataPanel);

    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:isLeftHandedOp() then
        GlobalNS.UIAttrSystem[GlobalNS.UIFormId.eUIOptionPanel].mWidgetPath = "UI/UIOptionPanel/UILeftOptionPanel.prefab";
    else
        GlobalNS.UIAttrSystem[GlobalNS.UIFormId.eUIOptionPanel].mWidgetPath = "UI/UIOptionPanel/UIOptionPanel.prefab";
    end
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIOptionPanel);
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUITopXRankPanel);
	
	if(GCtx.mBeginnerGuideSys:isEnableGuide()) then
		GCtx.mBeginnerGuideSys:onLevelLoaded();
	end
end

-- 帧循环
function M.onAdvance(delta, tickMode)
	--测试 tick
	--[[
	if(nil == M.mTickItemBase) then
		M.mTickItemBase = GlobalNS.new(GlobalNS.TickItemBase);
		M.mTickItemBase:addSelfTick(0);
	end
	]]
	
	GCtx.mProcessSys:advance(delta, tickMode);
end

function M.openForm(formId)
	GCtx.mUiMgr:loadAndShow(formId);
end

function M.exitForm(formId)
	GCtx.mUiMgr:exitForm(formId);
end

function M.requireFile(filePath)
	return MLoader(filePath);
end

--场景加载进度, progress: [0, 1]
function M.onSceneLoadProgress(progress)
	local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIStartGame);
	if(nil ~= form) then
		form:setProgress(progress);
	end
end

-- 添加一个道具
function M.addObjectByNativeItem(objectItem)
	GCtx.mPlayerData.mPackData:addObjectByNativeItem(objectItem);
	
	local packForm = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIPack);
	if(nil ~= packForm) then
	   packForm:addObjectByNativeItem(objectItem);
    end
end

function M.batchAddObjects(is_request)
	if(not is_request) then
		GCtx.mPlayerData.mPackData:getRedPointProperty():reset();
	end
end

function M.removeObjectByNativeItem(objectItem)
	GCtx.mPlayerData.mPackData:removeObjectByThisId(objectItem:getStrThisId());
	
	local packForm = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIPack);
    if(nil ~= packForm) then
       packForm:removeObjectByNativeItem(objectItem);
    end
end

--直接更新显示，数据已经更新
function M.updateOneObjectInfo(...)
	local args = {...};
	local uid = args[1];
	local newnum = args[2];
	local newbind = args[3];
	local newupgrade = args[4];
	local panelIndex = args[5];
	
	local packForm = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIPack);
	
    if(nil ~= packForm) then
       packForm:updateOneObjectInfo(uid, newnum, newbind, newupgrade, panelIndex);
    end
end

function M.addOneShopNativeItem(item)
	local shopDataItem = GCtx.mPlayerData.mShopData:addOneNativeShopItem(item);
	
	local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIShop);
	
    if(nil ~= form) then
       form:addOneShopItem(shopDataItem);
    end
end

function M.OnNotifyAllMoneyInfo()
	GCtx.mPlayerData.mHeroData:updateMoney();
	
	local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIShop);
	
	if(nil ~= form) then
		form:onNotifyAllMoneyInfo();
	end
end

function M.onOnlineSendAllSkins(...)
	local args = {...};
	local baseIndex = 0;
	GCtx.mPlayerData.mSkinData:setCurSkinIndex(args[baseIndex + 1]);
	baseIndex = baseIndex + 1;
	GCtx.mPlayerData.mSkinData:setCurBulletBaseId(args[baseIndex + 1]);
	baseIndex = baseIndex + 1;
	
	local listLen = GlobalNS.UtilApi.getTableLen(args) - 2;	-- 前两个是长度
	local index = 0;
	local item = nil;
	
	while(index < listLen) do
		item = GlobalNS.UtilApi.getTableElementByIndex(args, baseIndex);
		GCtx.mPlayerData.mSkinData:addObjectByNativeItem(item);
		
		baseIndex = baseIndex + 1;
		index = index + 1;
	end
	
	-- 清除红点
	GCtx.mPlayerData.mSkinData:clearAllRed();
end

function M.onAddOneSkin(args)
	local item = GCtx.mPlayerData.mSkinData:addObjectByNativeItem(args);
end

function M.onBatchAddSkins(...)
	local args = {...};
	
	local listLen = GlobalNS.UtilApi.getTableLen(args);
	local index = 0;
	local item = nil;
	local itemData = nil;
	
	while(index < listLen) do
		item = GlobalNS.UtilApi.getTableElementByIndex(args, index);
		itemData = GCtx.mPlayerData.mSkinData:addObjectByNativeItem(item);
		
		index = index + 1;
	end
end

function M.onRemoveOneSkin(args)
	local item = GCtx.mPlayerData.mSkinData:removeObjectByThisId(args);
	
	if(GCtx.mPlayerData.mSkinData:isUseSkinByBaseId(args)) then
		GCtx.mPlayerData.mSkinData:setCurSkinIndex(GlobalNS.UtilMath.MaxNum);
	end
end

function M.onBatchRemoveSkins(...)
	local args = {...};
	
	local listLen = GlobalNS.UtilApi.getTableLen(args);
	local index = 0;
	local item = nil;
	local itemData = nil;
	
	local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIMyskinPanel);
	
	while(index < listLen) do
		item = GlobalNS.UtilApi.getTableElementByIndex(args, index + 1);
		itemData = GCtx.mPlayerData.mSkinData:removeObjectByThisId(item);
		
		index = index + 1;
	end
end

function M.onNotifyUserSkinResult(...)
	local args = {...};
	local result = GlobalNS.UtilApi.getTableElementByIndex(args, 0);
	local skinIndex = GlobalNS.UtilApi.getTableElementByIndex(args, 1);
	
	-- 1代表使用成功,目前不会=0
	if(1 == result) then
		GCtx.mPlayerData.mSkinData:setCurSkinIndex(skinIndex);
	end
end

--子弹消息
function M.onAddOneBullet(...)
	local args = {...};
	local item = GlobalNS.UtilApi.getTableElementByIndex(args, 0);
	GCtx.mPlayerData.mSkinData:addObjectByNativeItem(item);
end

function M.onRemoveOneBullet(...)
	local args = {...};
	local baseId = GlobalNS.UtilApi.getTableElementByIndex(args, 0);
	GCtx.mPlayerData.mSkinData:removeObjectByThisId(baseId);
end

function M.onNotifyUseBulletResult(...)
	local args = {...};
	local result = GlobalNS.UtilApi.getTableElementByIndex(args, 0);
	local skinIndex = GlobalNS.UtilApi.getTableElementByIndex(args, 1);
	
	-- 1代表使用成功,目前不会=0
	if(1 == result) then
		GCtx.mPlayerData.mSkinData:setCurBulletBaseId(skinIndex);
	end
end

-- 切换账号
function M.OnSwitchAcc()
	GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIItemsColourPanel);
	GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIMyskinPanel);
	
	GCtx.mPlayerData.mSkinData:clear();
    GCtx.mPlayerData.mHeroData.mMyselfLevel = 0;
end

--开启新手引导
function M.enableGuide()
	GCtx.mBeginnerGuideSys:setIsEnableGuide(true);
end

function M.disableGuide()
	GCtx.mBeginnerGuideSys:setIsEnableGuide(false);
end

function M:ShowRollMessageWithTimeLen(strId)
	GCtx.mGameData:ShowRollMessageWithTimeLen(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eMessage, strId));
end

function M.nextGuide(guideId)
	GCtx.mBeginnerGuideSys:nextGuide(guideId, false);
end

function M.popupBeginnerGuideEnd()
	local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIConfirmAgain);
	form:hideCancelBtn();
	form:addOkEventHandle(GCtx.mBeginnerGuideSys, GCtx.mBeginnerGuideSys.onOkHandle);
	form:setDesc(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eMessage, 3));
end

function M.setHitScore(value)
	local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIPlayerDataPanel);
	
	if(nil ~= form) then
		form:refreshScore(value);
	end
end

return M;