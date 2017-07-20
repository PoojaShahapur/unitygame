MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UISignPanel.SignPanelNS");
MLoader("MyLua.UI.UISignPanel.SignPanelData");
MLoader("MyLua.UI.UISignPanel.SignPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIOtherAwardPanel";
GlobalNS.SignPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIOtherAwardPanel;
	self.mData = GlobalNS.new(GlobalNS.SignPanelNS.SignPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);

    self.mGetBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mGetBtn:setIsDestroySelf(false);
	self.mGetBtn:addEventHandle(self, self.onGetBtnClk);
	
	self.mOneObjectImage = GlobalNS.new(GlobalNS.AuxObjectImage);
	self.mOneObjectImage:setIsDestroySelf(false);
	self.mOneObjectText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mOneObjectText:setIsDestroySelf(false);
	self.mTwoObjectImage = GlobalNS.new(GlobalNS.AuxObjectImage);
	self.mTwoObjectImage:setIsDestroySelf(false);
	self.mTwoObjectText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mTwoObjectText:setIsDestroySelf(false);
	
	self.mTitleText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mTitleText:setIsDestroySelf(false);
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachCloseBtn(GlobalNS.SignPanelNS.SignPanelPath.OtherAwardCloseBtn);
	
    self.mGetBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.OtherAwardGetBtn));
	self.mOneObjectImage:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.OtherAwardOneImage);
	self.mOneObjectText:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.OtherAwardOneText);
	self.mTwoObjectImage:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.OtherAwardTwoImage);
	self.mTwoObjectText:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.OtherAwardTwoText);
	self.mTitleText:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.OtherAwardTitleText);
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
	
	if(nil ~= self.mGetBtn) then
		self.mGetBtn:dispose();
		self.mGetBtn = nil;
	end
	if(nil ~= self.mOneObjectImage) then
		self.mOneObjectImage:dispose();
		self.mOneObjectImage = nil;
	end
	if(nil ~= self.mOneObjectText) then
		self.mOneObjectText:dispose();
		self.mOneObjectText = nil;
	end
	if(nil ~= self.mTwoObjectImage) then
		self.mTwoObjectImage:dispose();
		self.mTwoObjectImage = nil;
	end
	if(nil ~= self.mTwoObjectText) then
		self.mTwoObjectText:dispose();
		self.mTwoObjectText = nil;
	end
	if(nil ~= self.mTitleText) then
		self.mTitleText:dispose();
		self.mTitleText = nil;
	end
end

function M:onGetBtnClk()
	GCtx.mLogSys:log("领取奖励", GlobalNS.LogTypeId.eLogCommon);
end

function M:setRewardsId(value)
	self.mRewardsId = value;
	self:UpdateUIData(nil);
end

function M:UpdateUIData(args)
	local cumulaReward = LuaConfig.SignConfig["CumulaRewards"][self.mRewardsId];
	local objectId = GlobalNS.UtilApi.tonumber(cumulaReward["Reward"][1]["ObjectId"]);
	local num = GlobalNS.UtilApi.tonumber(cumulaReward["Reward"][1]["ObjectNum"]);
	local name = GlobalNS.UtilLogic.getObjectNameByBaseId(GlobalNS.UtilApi.tonumber(cumulaReward["Reward"][1]["ObjectId"]));
    
	self.mOneObjectImage:setObjectBaseId(objectId);
	self.mOneObjectText:setText(string.format("%d个%s", num, name));
	
	objectId = GlobalNS.UtilApi.tonumber(cumulaReward["Reward"][2]["ObjectId"]);
	num = GlobalNS.UtilApi.tonumber(cumulaReward["Reward"][2]["ObjectNum"]);
	name = GlobalNS.UtilLogic.getObjectNameByBaseId(GlobalNS.UtilApi.tonumber(cumulaReward["Reward"][2]["ObjectId"]));
    
	self.mTwoObjectImage:setObjectBaseId(objectId);
	self.mTwoObjectText:setText(string.format("%d个%s", num, name));
	
	self.mTitleText:setText(string.format("%d月签到%d天", GCtx.mSignData:getMonth(), GCtx.mSignData:getSignedNumOfDaysInMonth()));
end

return M;