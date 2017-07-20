MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.ItemViewBase);
M.clsName = "ColoutViewItem";
GlobalNS.ItemsColourPanelNS[M.clsName] = M;

function M:ctor()
	self.mPath = "";		-- 资源目录，暂时没有用
	self.mRootGo = nil; 	-- 显示的预制
	self.mParentGo = nil;
	
	-- Item 点击处理函数
	self.mItemBtn = nil;
	self.mPriceText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mPriceText:setIsDestroySelf(false);
	self.mModelImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mModelImage:setIsDestroySelf(false);
	self.mMoneyImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mMoneyImage:setIsDestroySelf(false);

	self.mItemBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mItemBtn:setIsDestroySelf(false);
	self.mItemBtn:addEventHandle(self, self.onItemClick);
end

function M:dtor()

end

function M:init()
	self.mPriceText:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.RightNameText));
	self.mModelImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.RightModelImage));
	self.mMoneyImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.RightMoneyImage));
	self.mItemBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mRootGo, GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.RightUseBtn));

	self:update();
end

function M:dispose()
	if(nil ~= self.mItemBtn) then
		self.mItemBtn:removeEventHandle(self, self.onItemClick);
		self.mItemBtn:dispose();
		self.mItemBtn = nil;
	end
	
	if(nil ~= self.mPriceText) then
		self.mPriceText:dispose();
		self.mPriceText = nil;
	end
	
	if(nil ~= self.mModelImage) then
		self.mModelImage:dispose();
		self.mModelImage = nil;
	end
	
	if(nil ~= self.mMoneyImage) then
		self.mMoneyImage:dispose();
		self.mMoneyImage = nil;
	end
end

function M:onItemClick(dispObj)
	local panelType = self.mItemData:getPanelTypeZeroIndex();
	if(GCtx.mPlayerData.mSkinData:isActiveByPlaneIdAndSkinIdAndBaseId(
			panelType, 
			self.mItemData:getSkinId(), 
			self.mItemData:getBaseId())) then
		if(not GCtx.mPlayerData.mSkinData:isUseSkinByBaseId(self.mItemData:getBaseId())) then
			GlobalNS.CSSystem.reqUseSkin(self.mItemData:getBaseId());
		end
	else
		local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIConfirmAgain);
		form:addOkEventHandle(self, self.onOkHandle);
		form:setDesc(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eObject, 5));

		--self:onOkHandle(nil);
	end
	
	--[[
	local item = nil;
	local skinId = 1;
	item = GCtx.mPlayerData.mSkinData:getSkinItemBySkinId(skinId);
	
	while(nil ~= item) do
		GlobalNS.CSSystem.testRemovePiFu(item:getBaseId());
		item = GCtx.mPlayerData.mSkinData:getSkinItemBySkinId(skinId);
	end
	]]
end

function M:onOkHandle(dispObj)
	GlobalNS.CSSystem.reqActiveSkin(self.mItemData:getBaseId());
end

function M:update()
	self:updateImage();
	self:updatePrice();
end

function M:updateImage()
	self.mModelImage:setSpritePath(self.mItemData:getAtlasPath(), self.mItemData:getSpriteName());
end

function M:updatePrice()
	local panelType = self.mItemData:getPanelTypeZeroIndex();
	if(GCtx.mPlayerData.mSkinData:isActiveByPlaneIdAndSkinIdAndBaseId(
			panelType, 
			self.mItemData:getSkinId(), 
			self.mItemData:getBaseId())) then
		self.mMoneyImage:hide();
		
		if(GCtx.mPlayerData.mSkinData:isUseSkinByBaseId(self.mItemData:getBaseId())) then
			self.mPriceText:setText(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eObject, 2));
		else
			self.mPriceText:setText(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eObject, 1));
		end
	else
		self.mMoneyImage:show();
		
		self.mPriceText:setText(self.mItemData:getActivePrice());
	end
end

function M:setCanUse()
	self.mMoneyImage:hide();
	self.mPriceText:setText(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eObject, 1));
end

function M:setUsed()
	self.mMoneyImage:hide();
	self.mPriceText:setText(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eObject, 2));
end

return M;