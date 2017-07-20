MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

MLoader("MyLua.Libs.UI.ObjectItem.ItemViewBase");

local M = GlobalNS.Class(GlobalNS.ItemViewBase);
M.clsName = "ShopViewItem";
GlobalNS[M.clsName] = M;

function M:ctor(...)
	
end

function M:dtor()
	
end

function M:init()
    M.super.init(self);
	
	self:addItemClickHandle();
	
	self.mImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mImage:setIsDestroySelf(false);
	self.mImage:setIsNeedHideWhenLoadSprite(true);
	self.mImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ShopItemImage));
	
	self.mNameText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mNameText:setIsDestroySelf(false);
	self.mNameText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ShopItemName));
	
	self.mPriceText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mPriceText:setIsDestroySelf(false);
	self.mPriceText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ShopItemPriceNum));
	
	self.mPriceBtmText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mPriceBtmText:setIsDestroySelf(false);
	self.mPriceBtmText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ShopItemPriceBtmNum));
	
	self.mBuyBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBuyBtn:setIsDestroySelf(false);
	self.mBuyBtn:addEventHandle(self, self.onBuyBtnClick);
	self.mBuyBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ShopItemBuyBtn));

	self.mMoneyImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mMoneyImage:setIsDestroySelf(false);
	self.mMoneyImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ShopItemMoneyImage));
	
	self.mMoneyBtnImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mMoneyBtnImage:setIsDestroySelf(false);
	self.mMoneyBtnImage:setIsNeedHideWhenLoadSprite(true);
	self.mMoneyBtnImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ShopItemMoneyBtnImage));
	
	self.mAcquireActor = GlobalNS.new(GlobalNS.AuxComponent);
	self.mAcquireActor:setSelfGoByPath(self.mRootGo, GlobalNS.ObjectViewCV.ShopItemAcquireActor);
	
	self.mShopItemAcquireText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mShopItemAcquireText:setIsDestroySelf(false);
	self.mShopItemAcquireText:setSelfGoByPath(self.mRootGo, GlobalNS.ObjectViewCV.ShopItemAcquireText);
	
	if(self.mItemData:isEndAcquireMode()) then
		self.mAcquireActor:show();
		self.mBuyBtn:hide();
	else
		self.mAcquireActor:hide();
		self.mBuyBtn:show();
	end

	self:update();
end

function M:dispose()
	if(nil ~= self.mImage) then
		self.mImage:dispose();
		self.mImage = nil;
	end

	if(nil ~= self.mNameText) then
		self.mNameText:dispose();
		self.mNameText= nil;
	end
	
	if(nil ~= self.mPriceText) then
		self.mPriceText:dispose();
		self.mPriceText= nil;
	end
	
	if(nil ~= self.mPriceBtmText) then
		self.mPriceBtmText:dispose();
		self.mPriceBtmText= nil;
	end
	
	if(nil ~= self.mBuyBtn) then
		self.mBuyBtn:dispose();
		self.mBuyBtn = nil;
	end
	
	if(nil ~= self.mMoneyImage) then
		self.mMoneyImage:dispose();
		self.mMoneyImage = nil;
	end
	
	if(nil ~= self.mMoneyBtnImage) then
		self.mMoneyBtnImage:dispose();
		self.mMoneyBtnImage = nil;
	end
	
	if(nil ~= self.mAcquireActor) then
		self.mAcquireActor:dispose();
		self.mAcquireActor = nil;
	end
	
	if(nil ~= self.mShopItemAcquireText) then
		self.mShopItemAcquireText:dispose();
		self.mShopItemAcquireText = nil;
	end
	
	M.super.dispose(self);
end

function M:update()
	self:updateImage();
	self:updateName();
	self:updatePrice();
	self:updatePriceImage();
	
	if(self.mItemData:isEndAcquireMode()) then
		self.mShopItemAcquireText:setText(self.mItemData:getAcquireWayDesc());
	end
end

function M:updateImage()
	self.mImage:setSpritePath(self.mItemData:getAtlasPath(), self.mItemData:getSpriteName());
end

function M:updateName()
	self.mNameText:setText(self.mItemData:getName());
end

function M:updatePrice()
	self.mPriceText:setText(self.mItemData:getObjectWorthValue());
	self.mPriceBtmText:setText(self.mItemData:getPrice());
end

function M:updatePriceImage()
	if(self.mItemData:getPrice()) then
		self.mMoneyImage:setSpritePath(
			GlobalNS.UtilLogic.getMoneyAtlasPathByType(self.mItemData:getMoneyType()), 
			GlobalNS.UtilLogic.getMoneySpriteNameByType(self.mItemData:getMoneyType()));
		
		self.mMoneyBtnImage:setSpritePath(
			GlobalNS.UtilLogic.getMoneyAtlasPathByType(self.mItemData:getMoneyType()), 
			GlobalNS.UtilLogic.getMoneySpriteNameByType(self.mItemData:getMoneyType()));
	end
end

-- Item 点击
function M:onItemClick(dispObj)
	local form = nil;
	
	M.super.onItemClick(self, dispObj);
	
	form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIItemsCirclePanel);
	form:setOpenType(GlobalNS.CirclePanelOpenType.eOpenFromShop);
	form:setObjectItem(self.mItemData);
end

function M:onBuyBtnClick(dispObj)
	GlobalNS.CSSystem.reqByShopItem(self.mItemData:getShopId(), self.mItemData:getBaseId());
end

return M;