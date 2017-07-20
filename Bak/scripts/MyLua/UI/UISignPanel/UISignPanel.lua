MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UISignPanel.SignPanelNS");
MLoader("MyLua.UI.UISignPanel.SignPanelData");
MLoader("MyLua.UI.UISignPanel.SignPanelCV");
MLoader("MyLua.UI.UISignPanel.ItemData");

MLoader("MyLua.Config.SignConfig");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UISignPanel";
GlobalNS.SignPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUISignPanel;
	self.mData = GlobalNS.new(GlobalNS.SignPanelNS.SignPanelData);
	self.mOffsetDay = 0;
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	GCtx.mSignData:calcDate(self.mOffsetDay); 	-- 计算当前月的天数
	
    --底部签到按钮
    self.mSignBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mSignBtn:setIsDestroySelf(false);
	self.mSignBtn:addEventHandle(self, self.onSignBtnClk);

    --前/后一天
    self.mBeforeBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBeforeBtn:setIsDestroySelf(false);
	self.mBeforeBtn:addEventHandle(self, self.onBeforeBtnClk);
    self.mNextBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mNextBtn:setIsDestroySelf(false);
	self.mNextBtn:addEventHandle(self, self.onNextBtnClk);

    --连续签到奖励
    self.m3DaysBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.m3DaysBtn:setIsDestroySelf(false);
	self.m3DaysBtn:addEventHandle(self, self.on3DaysBtnClk);
    self.m5DaysBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.m5DaysBtn:setIsDestroySelf(false);
	self.m5DaysBtn:addEventHandle(self, self.on5DaysBtnClk);
    self.m7DaysBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.m7DaysBtn:setIsDestroySelf(false);
	self.m7DaysBtn:addEventHandle(self, self.on7DaysBtnClk);
	
	self.m3DaysText = GlobalNS.new(GlobalNS.AuxLabel);
	self.m3DaysText:setIsDestroySelf(false);
	self.m5DaysText = GlobalNS.new(GlobalNS.AuxLabel);
	self.m5DaysText:setIsDestroySelf(false);
	self.m7DaysText = GlobalNS.new(GlobalNS.AuxLabel);
	self.m7DaysText:setIsDestroySelf(false);
	
	self.mDateText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mDateText:setIsDestroySelf(false);
	
	self.mTipsText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mTipsText:setIsDestroySelf(false);

    --item prefab
    self.mItem_prefab = GlobalNS.new(GlobalNS.AuxPrefabLoader);
	self.mItem_prefab:setIsNeedInsPrefab(false);
    self.isPrefabLoaded = false;
    --items gameobject数组
    self.items = GlobalNS.new(GlobalNS.MList);
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachCloseBtn(GlobalNS.SignPanelNS.SignPanelPath.CloseBtn);

	self.mSignBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.SignBtn));
    self.mDateText:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.DateText);
    self.mBeforeBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.BeforeBtn));
    self.mNextBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.NextBtn));

    self.Award3 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.Award3Image);
    self.m3DaysBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.N3DaysBtn));
    self.Award5 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.Award5Image);
    self.m5DaysBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.N5DaysBtn));
    self.Award7 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.Award7Image);
    self.m7DaysBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.N7DaysBtn));
	
	self.m3DaysText:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.N3DaysText);
	self.m5DaysText:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.N5DaysText);
	self.m7DaysText:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.N7DaysText);

	self.mTipsText:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.TipsText);

    self.scrollrect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(dayAward, "ScrollRect");
    self.Content = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.Content);
    --加载items
	self.mItem_prefab:syncLoad("UI/UISignPanel/DayItem.prefab", self, self.onPrefabLoaded, nil);
	
	--最多 31 天，创建出来
	local index = 0;
	local itemData = nil;
	
	while(index < 31) do
		itemData = GlobalNS.new(GlobalNS.SignPanelNS.ItemData);
		self.items:add(itemData);
		itemData:init(self.mItemprefab, self.Content, index);
		index = index + 1;
	end
	
	self:updateUIData();
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
    GCtx.mSignData.day = 0;
	
	if(nil ~= self.mItem_prefab) then
		self.mItem_prefab:dispose();
		self.mItem_prefab = nil;
	end
	if(nil ~= self.mSignBtn) then
		self.mSignBtn:dispose();
		self.mSignBtn = nil;
	end
	if(nil ~= self.mBeforeBtn) then
		self.mBeforeBtn:dispose();
		self.mBeforeBtn = nil;
	end
	if(nil ~= self.mNextBtn) then
		self.mNextBtn:dispose();
		self.mNextBtn = nil;
	end
	if(nil ~= self.m3DaysBtn) then
		self.m3DaysBtn:dispose();
		self.m3DaysBtn = nil;
	end
	if(nil ~= self.m5DaysBtn) then
		self.m5DaysBtn:dispose();
		self.m5DaysBtn = nil;
	end
	if(nil ~= self.m7DaysBtn) then
		self.m7DaysBtn:dispose();
		self.m7DaysBtn = nil;
	end
	if(nil ~= self.m3DaysText) then
		self.m3DaysText:dispose();
		self.m3DaysText = nil;
	end
	if(nil ~= self.m5DaysText) then
		self.m5DaysText:dispose();
		self.m5DaysText = nil;
	end
	if(nil ~= self.m7DaysText) then
		self.m7DaysText:dispose();
		self.m7DaysText = nil;
	end
	if(nil ~= self.mDateText) then
		self.mDateText:dispose();
		self.mDateText = nil;
	end
	if(nil ~= self.mTipsText) then
		self.mTipsText:dispose();
		self.mTipsText = nil;
	end
	
	local index = 0;
	local itemData = nil;
	
	while(index < 31) do
		itemData = self.items:get(index);
		itemData:dispose();
		index = index + 1;
	end
	
	self.items:clear();
end

function M:onSignBtnClk()
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIDayAwardPanel);
end

function M:onBeforeBtnClk()
	--GCtx.mLogSys:log("Before", GlobalNS.LogTypeId.eLogCommon);
	self.mOffsetDay = self.mOffsetDay - 1;
	GCtx.mSignData:calcDate(self.mOffsetDay);
	self:updateUIData();
end

function M:onNextBtnClk()
	--GCtx.mLogSys:log("next", GlobalNS.LogTypeId.eLogCommon);
	self.mOffsetDay = self.mOffsetDay + 1;
	GCtx.mSignData:calcDate(self.mOffsetDay);
	self:updateUIData();
end

function M:on3DaysBtnClk()
	local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIOtherAwardPanel);
	
	if(nil ~= form) then
		form:setRewardsId(1);
	end
end

function M:on5DaysBtnClk()
	local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIOtherAwardPanel);
	
	if(nil ~= form) then
		form:setRewardsId(2);
	end
end

function M:on7DaysBtnClk()
	local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIOtherAwardPanel);
	
	if(nil ~= form) then
		form:setRewardsId(3);
	end
end

function M:onPrefabLoaded(dispObj)
    --获取item prefab对象
    self.mItemprefab = self.mItem_prefab:getPrefabTmpl();
    self.isPrefabLoaded = true;
end

function M:updateUIData()
	local index = 0;
	local month = GCtx.mSignData:getMonth();
	local monthConfig = LuaConfig.SignConfig["DailyRewards"][month];
	local dayConfig = nil;
	local rewardItem = nil;
	
	while(index < GCtx.mSignData.daysCount) do
		rewardItem = self.items:get(index);
		dayConfig = monthConfig[index + 1];
		rewardItem:setDayConfig(dayConfig);
		rewardItem:show();
		index = index + 1;
	end
	
	while(index < 31) do
		rewardItem = self.items:get(index);
		rewardItem:hide();
		
		index = index + 1;
	end
	
	self.m3DaysText:setText(LuaConfig.SignConfig["CumulaRewards"][1]["RewardDesc"]);
	self.m5DaysText:setText(LuaConfig.SignConfig["CumulaRewards"][2]["RewardDesc"]);
	self.m7DaysText:setText(LuaConfig.SignConfig["CumulaRewards"][3]["RewardDesc"]);
	
	self.mDateText:setText(string.format("%d月%d日", GCtx.mSignData:getMonth(), GCtx.mSignData:getDay()));
    self.mTipsText:setText(string.format("已连续签到:%d天", GCtx.mSignData:getContinueDays()));
	
	self.m3DaysBtn:toggleEnable(not GCtx.mSignData:isCumulaRewardsDrawById(1));
	self.m5DaysBtn:toggleEnable(not GCtx.mSignData:isCumulaRewardsDrawById(2));
	self.m7DaysBtn:toggleEnable(not GCtx.mSignData:isCumulaRewardsDrawById(3));
	
	self.mSignBtn:toggleEnable(not GCtx.mSignData:isSignedToday());
end

return M;