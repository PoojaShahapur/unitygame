MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIItempiecePanel.ItempiecePanelNS");
MLoader("MyLua.UI.UIItempiecePanel.ItempiecePanelData");
MLoader("MyLua.UI.UIItempiecePanel.ItempiecePanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIItempiecePanel";
GlobalNS.ItempiecePanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIItempiecePanel;
	self.mData = GlobalNS.new(GlobalNS.ItempiecePanelNS.ItempiecePanelData);
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
	
	self.mAcquireDescText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mAcquireDescText:setIsDestroySelf(false);
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
	self.mCompositeBtn:setSelfGoByPath(self.mGuiWin, GlobalNS.ItempiecePanelNS.ItempiecePanelPath.ComposeBtn);
	self.mObjectImage:setSelfGoByPath(self.mGuiWin, GlobalNS.ItempiecePanelNS.ItempiecePanelPath.ObjectImage);
	self.mNameText:setSelfGoByPath(self.mGuiWin, GlobalNS.ItempiecePanelNS.ItempiecePanelPath.NameText);
	self.mDescText:setSelfGoByPath(self.mGuiWin, GlobalNS.ItempiecePanelNS.ItempiecePanelPath.DescText);
	self.mAcquireDescText:setSelfGoByPath(self.mGuiWin, GlobalNS.ItempiecePanelNS.ItempiecePanelPath.AcquireDescText);
end

function M:addWidgetEventHandle()
	self:attachClosePanel(GlobalNS.ItempiecePanelNS.ItempiecePanelPath.CloseBgPanel);
end

function M:onComposeBtnClick(dispObj)
	
	
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
	self.mAcquireDescText:setText(self.mItemData:getAcquireDesc());
end

return M;