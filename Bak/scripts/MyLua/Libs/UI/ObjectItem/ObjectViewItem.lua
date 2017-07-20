MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxImage");
MLoader("MyLua.Libs.UI.ObjectItem.ObjectViewCV");

--[[
@brief 道具 Item
]]

local M = GlobalNS.Class(GlobalNS.ItemViewBase);
M.clsName = "ObjectViewItem";
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
	self.mImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ObjectItemImage));
	
	self.mNameText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mNameText:setIsDestroySelf(false);
	self.mNameText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ObjectItemName));
	
	self.mPriceText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mPriceText:setIsDestroySelf(false);
	self.mPriceText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ObjectItemPriceNum));
	
	self.mNumText = GlobalNS.new(GlobalNS.AuxLabel);
    self.mNumText:setIsDestroySelf(false);
    self.mNumText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ObjectItemNum));
	
	self.mMoneyImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mMoneyImage:setIsDestroySelf(false);
	self.mMoneyImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ObjectViewCV.ObjectItemMoneyImage));
	
	self:update();
end

function M:dispose()
    M.super.dispose(self);
	
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
	
	if(nil ~= self.mNumText) then
		self.mNumText:dispose();
		self.mNumText = nil;
	end
	
	if(nil ~= self.mMoneyImage) then
		self.mMoneyImage:dispose();
		self.mMoneyImage = nil;
	end
end

function M:update()
	self:updateImage();
	self:updateName();
	self:updatePrice();
	self:updateNum();
	self:updatePriceImage();
end

function M:updateImage()
	self.mImage:setSpritePath(self.mItemData:getAtlasPath(), self.mItemData:getSpriteName());
end

function M:updateName()
	self.mNameText:setText(self.mItemData:getName());
end

function M:updatePrice()
	self.mPriceText:setText(self.mItemData:getPrice());
end

function M:updateNum()
	self.mNumText:setText(self.mItemData:getNum());
end

function M:updatePriceImage()
	if(self.mItemData:getPrice()) then
		self.mMoneyImage:setSpritePath(
			GlobalNS.UtilLogic.getMoneyAtlasPathByType(self.mItemData:getMoneyType()), 
			GlobalNS.UtilLogic.getMoneySpriteNameByType(self.mItemData:getMoneyType()));
	end
end

-- Item 点击
function M:onItemClick(dispObj)
	M.super.onItemClick(self, dispObj);
	
	local form = nil;
	
	if(self.mItemData:isPiece()) then
		form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIItempiecePanel);
		
		if(nil ~= form) then
			form:setItemData(self.mItemData);
		end
	elseif(self.mItemData:isGift()) then
		form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIUseGiftsPanel);
		
		if(nil ~= form) then
			form:setItemData(self.mItemData);
		end
	else
		form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIItemsCirclePanel);
		form:setOpenType(GlobalNS.CirclePanelOpenType.eOpenFromPack);
		form:setObjectItem(self.mItemData);
	end
	
	--[[
	GlobalNS.CSSystem.testRemovePiFu(10000);
	]]
	
	--[[
	GlobalNS.CSSystem.testRemovePiFuStr(self.mItemData:getThisId());
	]]
end

return M;