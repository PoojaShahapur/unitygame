MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIItemsCirclePanel.ItemsCirclePanelNS");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "CirclePanelBase";
GlobalNS.ItemsCirclePanelNS[M.clsName] = M;

function M:ctor()
	self.mItemData = nil;
	self.mGuiWin = nil;
	
	self.mNameText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mNameText:setIsDestroySelf(false);
	
	self.mPriceText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mPriceText:setIsDestroySelf(false);
	
	self.mPriceBtmText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mPriceBtmText:setIsDestroySelf(false);
	
	self.mModelImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mModelImage:setIsDestroySelf(false);
	
	self.mActiveBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mActiveBtn:setIsDestroySelf(false);
	self.mActiveBtn:addEventHandle(self, self.onActiveBtnClick);
	
	self.mMoneyImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mMoneyImage:setIsDestroySelf(false);
	
	self.mMoneyBtnImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mMoneyBtnImage:setIsDestroySelf(false);
	
	self.mShuoMingText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mShuoMingText:setIsDestroySelf(false);	
	
	self.mItemsHowGetBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mItemsHowGetBtn:setIsDestroySelf(false);
	self.mItemsHowGetBtn:addEventHandle(self, self.onHowGetBtnClick);
	
	self.mItemsJiaGeActor = GlobalNS.new(GlobalNS.AuxComponent);
	self.mItemsJiaGeActor:setIsDestroySelf(false);
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	if(nil ~= self.mNameText) then
        self.mNameText:dispose();
        self.mNameText = nil;
    end
    
    if(nil ~= self.mPriceText) then
        self.mPriceText:dispose();
        self.mPriceText = nil;
    end
    
    if(nil ~= self.mModelImage) then
        self.mModelImage:dispose();
        self.mModelImage = nil;
    end
    
    if(nil ~= self.mActiveBtn) then
        self.mActiveBtn:dispose();
        self.mActiveBtn = nil;
    end
	
	if(nil ~= self.mPriceBtmText) then
		self.mPriceBtmText:dispose();
		self.mPriceBtmText = nil;
	end
	
	if(nil ~= self.mMoneyImage) then
		self.mMoneyImage:dispose();
		self.mMoneyImage = nil;
	end
	
	if(nil ~= self.mMoneyBtnImage) then
		self.mMoneyBtnImage:dispose();
		self.mMoneyBtnImage = nil;
	end
	if(nil ~= self.mShuoMingText) then
		self.mShuoMingText:dispose();
		self.mShuoMingText = nil;
	end
	
	if(nil ~= self.mItemsHowGetBtn) then
		self.mItemsHowGetBtn:dispose();
		self.mItemsHowGetBtn = nil;
	end
	if(nil ~= self.mItemsJiaGeActor) then
		self.mItemsJiaGeActor:dispose();
		self.mItemsJiaGeActor = nil;
	end
end

function M:attachWidget()
	self.mNameText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelPath.ItemsName));
	self.mPriceText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelPath.ItemsPrice));
	self.mPriceBtmText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelPath.ItemsPriceBtm));
	self.mModelImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelPath.ItemsImage));
	self.mActiveBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelPath.ItemsActiveBtn));
	self.mItemsHowGetBtn:setSelfGoByPath(self.mGuiWin, GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelPath.ItemsHowGetBtn);
	
	self.mMoneyImage:setSelfGoByPath(self.mGuiWin, GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelPath.ItemsTopPriceImage);
	self.mMoneyBtnImage:setSelfGoByPath(self.mGuiWin, GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelPath.ItemsBtmPriceImage);
	self.mShuoMingText:setSelfGoByPath(self.mGuiWin, GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelPath.ItemsShuoMing);
	self.mItemsJiaGeActor:setSelfGoByPath(self.mGuiWin, GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelPath.ItemsJiaGeImage);
end

function M:setGuiWin(value)
	self.mGuiWin = value;
end

function M:onActiveBtnClick(dispObj)
	local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIItemsCirclePanel);
	form:exit();
end

function M:onHowGetBtnClick(dispObj)
	if(self.mItemData:isEndAcquireMode()) then
		form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIHowGetGoodsPanel);
		form:setBaseShopItem(self.mItemData);
	end
	
	--[[
	local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIItemsCirclePanel);
	form:exit();
	]]
end

function M:setObjectItem(value)
	self.mItemData = value;
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

return M;