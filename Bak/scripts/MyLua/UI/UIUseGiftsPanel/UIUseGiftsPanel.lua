MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIUseGiftsPanel.UseGiftsPanelNS");
MLoader("MyLua.UI.UIUseGiftsPanel.UseGiftsPanelData");
MLoader("MyLua.UI.UIUseGiftsPanel.UseGiftsPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIUseGiftsPanel";
GlobalNS.UseGiftsPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIUseGiftsPanel;
	self.mData = GlobalNS.new(GlobalNS.UseGiftsPanelNS.UseGiftsPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mCompositeBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCompositeBtn:setIsDestroySelf(false);
	self.mCompositeBtn:addEventHandle(self, self.onComposeBtnClick);
	
	self.mObjectImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mObjectImage:setIsDestroySelf(false);
	
	self.mNameText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mNameText:setIsDestroySelf(false);
	
	self.mDescText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mDescText:setIsDestroySelf(false);
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachWidget();
	self:addWidgetEventHandle();
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
	self:dispose();
	
    M.super.onExit(self);
end

function M:dispose()
	if(nil ~= self.mCompositeBtn) then
		self.mCompositeBtn:dispose();
		self.mCompositeBtn = nil;
	end
	if(nil ~= self.mObjectImage) then
		self.mObjectImage:dispose();
		self.mObjectImage = nil;
	end
	if(nil ~= self.mNameText) then
		self.mNameText:dispose();
		self.mNameText = nil;
	end
	if(nil ~= self.mDescText) then
		self.mDescText:dispose();
		self.mDescText = nil;
	end
	if(nil ~= self.mAcquireDescText) then
		self.mAcquireDescText:dispose();
		self.mAcquireDescText = nil;
	end
end

function M:attachWidget()
	self.mCompositeBtn:setSelfGoByPath(self.mGuiWin, GlobalNS.UseGiftsPanelNS.UseGiftsPanelPath.ComposeBtn);
	self.mObjectImage:setSelfGoByPath(self.mGuiWin, GlobalNS.UseGiftsPanelNS.UseGiftsPanelPath.ObjectImage);
	self.mNameText:setSelfGoByPath(self.mGuiWin, GlobalNS.UseGiftsPanelNS.UseGiftsPanelPath.NameText);
	self.mDescText:setSelfGoByPath(self.mGuiWin, GlobalNS.UseGiftsPanelNS.UseGiftsPanelPath.DescText);
end

function M:addWidgetEventHandle()
	self:attachClosePanel(GlobalNS.UseGiftsPanelNS.UseGiftsPanelPath.CloseBgPanel);
end

function M:onComposeBtnClick(dispObj)
	local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIPerformancePanel);
	
	self:onCloseBtnClk(nil);
end

function M:setItemData(value)
	self.mItemData = value;
	
	self:updateUI();
end

function M:updateUI()
	self.mObjectImage:setSpritePath(self.mItemData:getAtlasPath(), self.mItemData:getSpriteName());
	self.mNameText:setText(self.mItemData:getName());
	self.mDescText:setText(self.mItemData:getUsageDesc());
end

return M;