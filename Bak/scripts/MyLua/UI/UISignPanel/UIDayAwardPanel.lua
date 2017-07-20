MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UISignPanel.SignPanelNS");
MLoader("MyLua.UI.UISignPanel.SignPanelData");
MLoader("MyLua.UI.UISignPanel.SignPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIDayAwardPanel";
GlobalNS.SignPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIDayAwardPanel;
	self.mData = GlobalNS.new(GlobalNS.SignPanelNS.SignPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mAwardNameText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mAwardNameText:setIsDestroySelf(false);
	
	self.mObjectImage = GlobalNS.new(GlobalNS.AuxObjectImage);
	self.mObjectImage:setIsDestroySelf(false);
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachCloseBtn(GlobalNS.SignPanelNS.SignPanelPath.DayAwardCloseBtn);
    self.mAwardNameText:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.DayAwardText);
	self.mObjectImage:setSelfGoByPath(self.mGuiWin, GlobalNS.SignPanelNS.SignPanelPath.DayAwardImage);

    self:UpdateUIData(nil);
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
	
	if(nil ~= self.mObjectImage) then
		self.mObjectImage:dispose();
		self.mObjectImage = nil;
	end
	if(nil ~= self.mAwardNameText) then
		self.mAwardNameText:dispose();
		self.mAwardNameText = nil;
	end
end

function M:onCloseBtnClk()
	M.super.onCloseBtnClk(self);
end

function M:UpdateUIData(args)
	local dayReward = LuaConfig.SignConfig["DailyRewards"][GCtx.mSignData:getMonth()][GCtx.mSignData:getDay()];
	local name = GlobalNS.UtilLogic.getObjectNameByBaseId(GlobalNS.UtilApi.tonumber(dayReward["ObjectId"]));
	local num = GlobalNS.UtilApi.tonumber(dayReward["ObjectNum"]);
    self.mAwardNameText:setText(name .. " x" .. num);
	self.mObjectImage:setObjectBaseId(GlobalNS.UtilApi.tonumber(dayReward["ObjectId"]));
end

return M;