MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIBulletSelectPanel.BulletSelectPanelNS");
MLoader("MyLua.UI.UIBulletSelectPanel.BulletSelectPanelData");
MLoader("MyLua.UI.UIBulletSelectPanel.BulletSelectPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIBulletSelectPanel";
GlobalNS.BulletSelectPanelNS[M.clsName] = M;

function M:ctor()
	self.mData = GlobalNS.new(GlobalNS.BulletSelectPanelNS.BulletSelectPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mNameText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mNameText:setIsDestroySelf(false);
	
	self.mPriceText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mPriceText:setIsDestroySelf(false);	
	
	self.mPriceImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mPriceImage:setIsDestroySelf(false);
	
	self.mSkinImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mSkinImage:setIsDestroySelf(false);
	
	self.mUseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mUseBtn:setIsDestroySelf(false);
	self.mUseBtn:addEventHandle(self, self.onBtnClick);
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachCloseBtn("");
	
	self.mNameText:setSelfGoByPath(self.mGuiWin, GlobalNS.BulletSelectPanelNS.BulletSelectPanelPath.NameText);
	self.mPriceText:setSelfGoByPath(self.mGuiWin, GlobalNS.BulletSelectPanelNS.BulletSelectPanelPath.PriceText);
	self.mSkinImage:setSelfGoByPath(self.mGuiWin, GlobalNS.BulletSelectPanelNS.BulletSelectPanelPath.SkinImage);
	self.mPriceImage:setSelfGoByPath(self.mGuiWin, GlobalNS.BulletSelectPanelNS.BulletSelectPanelPath.PriceImage);
	self.mUseBtn:setSelfGoByPath(self.mGuiWin, GlobalNS.BulletSelectPanelNS.BulletSelectPanelPath.UseBtn);
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
	if(nil ~= self.mNameText) then
		self.mNameText:dispose();
		self.mNameText = nil;
	end
	if(nil ~= self.mPriceText) then
		self.mPriceText:dispose();
		self.mPriceText = nil;
	end
	if(nil ~= self.mPriceImage) then
		self.mPriceImage:dispose();
		self.mPriceImage = nil;
	end
	if(nil ~= self.mSkinImage) then
		self.mSkinImage:dispose();
		self.mSkinImage = nil;
	end
	if(nil ~= self.mUseBtn) then
		self.mUseBtn:removeEventHandle(self, self.onBtnClick);
		self.mUseBtn:dispose();
		self.mUseBtn = nil;
	end
	
    M.super.onExit(self);
end

function M:setItemData(value)
	self.mItemData = value;
	
	self:update();
end

function M:update()
	self.mNameText:setText(self.mItemData:getName());
	self.mPriceText:setText(self.mItemData:getPrice());
	self.mSkinImage:setSpritePath(self.mItemData:getObjectAtlasPath(), self.mItemData:getObjectSpriteName());
	
	if(self.mItemData:getPrice()) then
		self.mPriceImage:setSpritePath(
			GlobalNS.UtilLogic.getMoneyAtlasPathByType(self.mItemData:getMoneyType()), 
			GlobalNS.UtilLogic.getMoneySpriteNameByType(self.mItemData:getMoneyType()));
	end
end

function M:onBtnClick(dispObj)
	GlobalNS.CSSystem.reqUseBullet(self.mItemData:getBaseId());
	self:exit();
end

return M;