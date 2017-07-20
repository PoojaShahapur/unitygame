 MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIShop.ShopNS");
MLoader("MyLua.UI.UIShop.ShopViewData");
MLoader("MyLua.UI.UIShop.ShopCV");
MLoader("MyLua.UI.UIShop.Panel.ShopPanelView");
MLoader("MyLua.UI.UIShop.Panel.ShopTopPanelView");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIShop";
GlobalNS.ShopNS[M.clsName] = M;

function M:ctor()
	self.mData = GlobalNS.new(GlobalNS.ShopNS.ShopViewData);
	
	self.mPackBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mPackBtn:setIsDestroySelf(false);
	self.mPackBtn:addEventHandle(self, self.onPackBtnClick);
	
	self.mPiFuBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mPiFuBtn:setIsDestroySelf(false);
	self.mPiFuBtn:addEventHandle(self, self.onPiFuBtnClick);
	
	self.mTopPage = GlobalNS.new(GlobalNS.TabPageMgr);
	
	self.mTplItem = GlobalNS.new(GlobalNS.AuxPrefabLoader);
	self.mTplItem:setIsNeedInsPrefab(false);
	
	self.mMoneyText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mMoneyText:setIsDestroySelf(false);
	self.mTicketText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mTicketText:setIsDestroySelf(false);
	self.mPlasticText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mPlasticText:setIsDestroySelf(false);
	
	self.mPackRed = GlobalNS.new(GlobalNS.AuxImage);
	self.mPackRed:setIsDestroySelf(false);
	self.mSkinRed = GlobalNS.new(GlobalNS.AuxImage);
	self.mSkinRed:setIsDestroySelf(false);
	
	self.mMoneyBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mMoneyBtn:setIsDestroySelf(false);
	self.mMoneyBtn:addEventHandle(self, self.onMoneyBtnClick);
	self.mTicketBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mTicketBtn:setIsDestroySelf(false);
	self.mTicketBtn:addEventHandle(self, self.onTicketBtnClick);
	self.mPlasticBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mPlasticBtn:setIsDestroySelf(false);
	self.mPlasticBtn:addEventHandle(self, self.onPlasticBtnClick);
    self.mGetTicketBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mGetTicketBtn:setIsDestroySelf(false);
	self.mGetTicketBtn:addEventHandle(self, self.onGetTicketBtnClick);
	self.mGetPlasticBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mGetPlasticBtn:setIsDestroySelf(false);
	self.mGetPlasticBtn:addEventHandle(self, self.onGetPlasticBtnClick);
end

function M:dtor()
	
end

function M:onInit()
	GCtx.mPlayerData.mPackData:getRedPointProperty():addEventHandle(self, self.onRedPointPack);
	GCtx.mPlayerData.mSkinData:getRedPointProperty():addEventHandle(self, self.onRedPointSkin);
	
    M.super.onInit(self);
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachCloseBtn(GlobalNS.ShopNS.ShopPath.CloseBtn);
	self.mPackBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.PackBtn));
	self.mPiFuBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.PiFuBtn));

	self.mMoneyBtn:setSelfGoByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.MoneyBtn);
	self.mTicketBtn:setSelfGoByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.TicketBtn);
	self.mPlasticBtn:setSelfGoByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.PlasticBtn);
    self.mGetTicketBtn:setSelfGoByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.GetTicketBtn);
	self.mGetPlasticBtn:setSelfGoByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.GetPlasticBtn);

	do
		self.mTplItem:syncLoad("UI/UIShop/ShopItem.prefab");
		local tplGo = self.mTplItem:getPrefabTmpl();
		
		local page = nil;
		
		page = self.mTopPage:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.ModelTopBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.ModelTopPanel), 
			GlobalNS.ShopNS.ShopTopPanelView
			);
		page:setTag(GlobalNS.ShopNS.ShopPanelTag.eTopModel);
		page:setTplGo(tplGo);
		page:setRedImageByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.TopSkinRed);
		page:setUnderlineByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.TopSkinUnderline);
		page:addSubPage(self.mGuiWin);
		
		page = self.mTopPage:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.NiuDanTopBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.NiuDanTopPanel), 
			GlobalNS.ShopNS.ShopTopPanelView
			);
		page:setTag(GlobalNS.ShopNS.ShopPanelTag.eTopNiuDan);
		page:setTplGo(tplGo);
		page:setRedImageByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.TopNiuDanRed);
		page:setUnderlineByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.TopNiuDanUnderline);
		page:addSubPage(self.mGuiWin);
		
		page = self.mTopPage:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.HuiYuanTopBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.HuiYuanTopPanel), 
			GlobalNS.ShopNS.ShopTopPanelView
			);
		page:setTag(GlobalNS.ShopNS.ShopPanelTag.eTopHuiYuan);
		page:setTplGo(tplGo);
		page:setRedImageByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.TopHuiYuanRed);
		page:setUnderlineByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.TopHuiYuanUnderline);
		page:addSubPage(self.mGuiWin);
		
		self.mTopPage:openPage(GlobalNS.ShopNS.ShopPanelTag.eTopModel);
	end
	
	do
		self.mMoneyText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.MoneyText));
		self.mTicketText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.TicketText));
		self.mPlasticText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.PlasticText));
		
		self:onNotifyAllMoneyInfo();
	end
	
	self.mPackRed:setSelfGoByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.PackRed);
	self.mSkinRed:setSelfGoByPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.SkinRed);
	
	self:onRedPointPack(GCtx.mPlayerData.mPackData:getRedPointProperty());
	self:onRedPointSkin(GCtx.mPlayerData.mSkinData:getRedPointProperty());
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
	GCtx.mPlayerData.mPackData:getRedPointProperty():removeEventHandle(self, self.onRedPointPack);
	GCtx.mPlayerData.mSkinData:getRedPointProperty():removeEventHandle(self, self.onRedPointSkin);
	
	if(nil ~= self.mPackBtn) then
		self.mPackBtn:dispose();
		self.mPackBtn = nil;
	end
	
	if(nil ~= self.mPiFuBtn) then
		self.mPiFuBtn:dispose();
		self.mPiFuBtn = nil;
	end
	
	if(nil ~= self.mTplItem) then
		GlobalNS.delete(self.mTplItem);
		self.mTplItem = nil;
	end
	
	if(nil ~= self.mTabPanel) then
		GlobalNS.delete(self.mTabPanel);
		self.mTabPanel = nil;
	end
	
	if(nil ~= self.mPackRed) then
		self.mPackRed:dispose();
		self.mPackRed = nil;
	end
	
	if(nil ~= self.mSkinRed) then
		self.mSkinRed:dispose();
		self.mSkinRed = nil;
	end
	
	if(nil ~= self.mMoneyBtn) then
		self.mMoneyBtn:dispose();
		self.mMoneyBtn = nil;
	end
	
	if(nil ~= self.mTicketBtn) then
		self.mTicketBtn:dispose();
		self.mTicketBtn = nil;
	end
	
	if(nil ~= self.mPlacticBtn) then
		self.mPlacticBtn:dispose();
		self.mPlacticBtn = nil;
	end

    if(nil ~= self.mGetTicketBtn) then
		self.mGetTicketBtn:dispose();
		self.mGetTicketBtn = nil;
	end
	
	if(nil ~= self.mGetPlacticBtn) then
		self.mGetPlacticBtn:dispose();
		self.mGetPlacticBtn = nil;
	end

	M.super.onExit(self);
end

function M:onPackBtnClick(dispObj)
	self.mPackRed:hide();
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIPack);
end

function M:onPiFuBtnClick(dispObj)
	self.mSkinRed:hide();
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIMyskinPanel);
end

function M:addOneShopItem(shopDataItem)
	local topPanel = self.mTopPage:getPageByTag(shopDataItem:getTopTypeZeroIndex());
	local subPanel = topPanel:getPageByTag(shopDataItem:getSubTypeZeroIndex());
	subPanel:addOneShopItem(shopDataItem);
end

function M:onNotifyAllMoneyInfo()
	self.mMoneyText:setText(GCtx.mPlayerData.mHeroData:getMoney());
	self.mTicketText:setText(GCtx.mPlayerData.mHeroData:getTicket());
	self.mPlasticText:setText(GCtx.mPlayerData.mHeroData:getPlastic());
end

function M:onRedPointPack(dispObj)
	local property = dispObj;
	
	if(property:isValid()) then
		if(not GCtx.mUiMgr:isFormVisible(GlobalNS.UIFormId.eUIPack)) then
			self.mPackRed:show();
		else
			self.mPackRed:hide();
		end
	else
		self.mPackRed:hide();
	end
	
	property:reset();
end

function M:onRedPointSkin(dispObj)
	local property = dispObj;
	
	if(property:isValid()) then
		if(not GCtx.mUiMgr:isFormVisible(GlobalNS.UIFormId.eUIMyskinPanel)) then
			self.mSkinRed:show();
		else
			self.mSkinRed:hide();
		end
	else
		self.mSkinRed:hide();
	end
	
	property:reset();
end

function M:onMoneyBtnClick(dispObj)
	local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIItempricePanel);
	form:setObjectId(1);
end

function M:onTicketBtnClick(dispObj)
	local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIItempricePanel);
	form:setObjectId(2);
end

function M:onPlasticBtnClick(dispObj)
	local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIItempricePanel);
	form:setObjectId(3);
end

function M:onGetTicketBtnClick(dispObj)
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIShareMoney);
end

function M:onGetPlasticBtnClick(dispObj)
	GCtx.mGameData:ShowRollMessage("此功能尚未开放");
end

return M;