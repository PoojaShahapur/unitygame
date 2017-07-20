MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIItemsColourPanel.ItemsColourPanelNS");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "LeftColourViewPanel";
GlobalNS.ItemsColourPanelNS[M.clsName] = M;

function M:ctor()
	self.mWinGo = nil;
	self.mSelfRootGo = nil;
	
	self.mItemData = nil;
	self.mIsTypeInUsing = false;
end

function M:dtor()
	
end

function M:init()
	self.mSelfRootActor = GlobalNS.new(GlobalNS.AuxComponent);
	self.mSelfRootActor:setIsDestroySelf(false);
	
	self.mImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mImage:setIsDestroySelf(false);
	
	self.mNameText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mNameText:setIsDestroySelf(false);
	
	self.mPriceText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mPriceText:setIsDestroySelf(false);
	
	self.mMoneyImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mMoneyImage:setIsDestroySelf(false);
	
	GCtx.mPlayerData.mSkinData:addCurSkinChangeHandle(self, self.onCurSkinChanged);
	
	--self:update();
end

function M:dispose()
	GCtx.mPlayerData.mSkinData:removeCurSkinChangeHandle(self, self.onCurSkinChanged);
	
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
	
	if(nil ~= self.mSelfRootActor) then
		self.mSelfRootActor:dispose();
		self.mSelfRootActor = nil;
	end
	
	self.mWinGo = nil;
end

function M:setWinGo(value)
	self.mWinGo = value;
end

function M:setTplItemData(value)
	self.mItemData = value;
	self:update();
end

function M:attachWidget()
	self.mSelfRootActor:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mWinGo, GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.LeftRootGo));
	
	self.mImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mWinGo, GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.LeftModelImage));
	self.mNameText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mWinGo, GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.LeftNameText));
	self.mPriceText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mWinGo, GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.LeftPriceText));
	
	self.mMoneyImage:setSelfGoByPath(self.mWinGo, GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.LeftMoneyImage);
end

-- 更新显示
function M:update()
	if(nil ~= self.mItemData) then
		self.mSelfRootActor:show();
		
		if(GCtx.mPlayerData.mSkinData:hasSkin()) then
			self.mIsTypeInUsing = true;
			local itemData = GCtx.mPlayerData.mSkinData:getCurSkinItemData();
			
			if(self.mItemData:getSkinId() == itemData:getSkinId()) then
				self.mItemData = itemData;
			else
				self.mIsTypeInUsing = false;
			end
		else
			self.mIsTypeInUsing = false;
		end
		
		self:updateImage();
		self:updateName();
		self:updatePrice();
		self:updatePriceImage();
	end
end

function M:updateImage()
	if(self.mIsTypeInUsing) then
		self.mImage:setSpritePath(self.mItemData:getAtlasPath(), self.mItemData:getSpriteName());
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

function M:onCurSkinChanged(dispObj)
	self:update();
end

function M:updatePriceImage()
	if(self.mItemData:getPrice()) then
		self.mMoneyImage:setSpritePath(
			GlobalNS.UtilLogic.getMoneyAtlasPathByType(self.mItemData:getMoneyType()), 
			GlobalNS.UtilLogic.getMoneySpriteNameByType(self.mItemData:getMoneyType()));
	end
end

return M;