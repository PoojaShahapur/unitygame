MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");

MLoader("MyLua.Libs.UI.ObjectItem.ItemViewBase");

local M = GlobalNS.Class(GlobalNS.ItemViewBase);
M.clsName = "SkinViewItem";
GlobalNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
	
	self:addItemClickHandle();
	
	self.mImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mImage:setIsDestroySelf(false);
	self.mImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.SkinItemImage));
	
	self.mNameText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mNameText:setIsDestroySelf(false);
	self.mNameText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.SkinItemName));
	
	self.mPriceText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mPriceText:setIsDestroySelf(false);
	self.mPriceText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.SkinItemPriceNum));
	
	self.mMoneyImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mMoneyImage:setIsDestroySelf(false);
	self.mMoneyImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.SkinItemMoneyImage));
	
	self.mInUsingImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mInUsingImage:setIsDestroySelf(false);
	self.mInUsingImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.SkinInUsingImage));
	self.mInUsingImage:hide();
	
	self:update();
end

function M:dispose()
	if(nil ~= self.mImage) then
		self.mImage:dispose();
		self.mImage = nil;
	end

	if(nil ~= self.mNameText) then
		self.mNameText:dispose();
		self.mNameText = nil;
	end
	
	if(nil ~= self.mPriceText) then
		self.mPriceText:dispose();
		self.mPriceText = nil;
	end
	
	if(nil ~= self.mMoneyImage) then
		self.mMoneyImage:dispose();
		self.mMoneyImage = nil;
	end
	
	if(nil ~= self.mInUsingImage) then
		self.mInUsingImage:dispose();
		self.mInUsingImage = nil;
	end
	
	M.super.dispose(self);
end

function M:onItemClick(dispObj)
	M.super.onItemClick(self, dispObj);
	
	if(GlobalNS.DataItemType.eSkinItem_Bullet == self.mItemData:getItemType()) then
		local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIBulletSelectPanel);
		form:setItemData(self.mItemData);
	elseif(GlobalNS.DataItemType.eSkinItem_PiFu == self.mItemData:getItemType()) then
		local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIItemsColourPanel);
		form:setObjectItem(self.mItemData);
	end
	
	--[[
	local item = nil;
	local skinId = self.mItemData:getSkinId();
	item = GCtx.mPlayerData.mSkinData:getSkinItemBySkinId(skinId);
	
	while(nil ~= item) do
		GlobalNS.CSSystem.testRemovePiFu(item:getBaseId());
		item = GCtx.mPlayerData.mSkinData:getSkinItemBySkinId(skinId);
	end
	]]
	
	--[[
	GlobalNS.CSSystem.testRemovePiFu(self.mItemData:getBaseId());
	]]
end

function M:update()
	self:updateImage();
	self:updateName();
	self:updatePrice();
	self:updatePriceImage();
end

function M:updateImage()
	--self.mImage:setSpritePath(self.mItemData:getAtlasPath(), self.mItemData:getSpriteName());
	--如果是当前皮肤，就使用皮肤表中的颜色图像，如果不是，就使用道具表中的图像
	if(GCtx.mPlayerData.mSkinData:hasSkin()) then
		local curItemData = GCtx.mPlayerData.mSkinData:getCurSkinItemData();
		
		if(nil ~= curItemData and curItemData:getSkinId() == self.mItemData:getSkinId()) then
			self.mImage:setSpritePath(curItemData:getAtlasPath(), curItemData:getSpriteName());
		else
			self.mImage:setSpritePath(self.mItemData:getObjectAtlasPath(), self.mItemData:getObjectSpriteName());
		end
	else
		self.mImage:setSpritePath(self.mItemData:getObjectAtlasPath(), self.mItemData:getObjectSpriteName());
	end
end

function M:updateName()
	self.mNameText:setText(self.mItemData:getName());
end

function M:updatePrice()
	self.mPriceText:setText(self.mItemData:getPrice());
end

function M:updatePriceImage()
	if(self.mItemData:getPrice()) then
		self.mMoneyImage:setSpritePath(
			GlobalNS.UtilLogic.getMoneyAtlasPathByType(self.mItemData:getMoneyType()), 
			GlobalNS.UtilLogic.getMoneySpriteNameByType(self.mItemData:getMoneyType()));
	end
end

function M:setUsingState(isInUsing)
	self.mInUsingImage:setVisible(isInUsing);
	
	if(isInUsing) then
		self:moveToBottom();
	end
end

return M;