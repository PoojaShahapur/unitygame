MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIItemsCirclePanel.ItemsCirclePanelNS");
MLoader("MyLua.UI.UIItemsCirclePanel.Panel.CirclePanelBase");

local M = GlobalNS.Class(GlobalNS.ItemsCirclePanelNS.CirclePanelBase);
M.clsName = "CircleShopPanel";
GlobalNS.ItemsCirclePanelNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	M.super.dispose(self);
end

function M:setObjectItem(value)
	M.super.setObjectItem(self, value);
	
	self.mNameText:setText(self.mItemData:getName());
	self.mPriceText:setText(self.mItemData:getObjectWorthValue());
	self.mPriceBtmText:setText(self.mItemData:getPrice());
	self.mModelImage:setSpritePath(self.mItemData:getAtlasPath(), self.mItemData:getSpriteName());
	
	self.mActiveBtn:setText(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eObject, 4));
	self.mShuoMingText:setText(self.mItemData:getUsageDesc());
	
	if(self.mItemData:isEndAcquireMode()) then
		self.mItemsJiaGeActor:hide();
		self.mPriceBtmText:hide();
		self.mActiveBtn:hide();
		self.mItemsHowGetBtn:show();
	else
		self.mItemsJiaGeActor:show();
		self.mPriceBtmText:show();
		self.mActiveBtn:show();
		self.mItemsHowGetBtn:hide();
	end
	
	self:updatePriceImage();
end

function M:onActiveBtnClick(dispObj)
	--GlobalNS.CSSystem.reqByShopItem(self.mItemData:getShopId(), self.mItemData:getBaseId());
	--M.super.onActiveBtnClick(self, dispObj);
	
	--[[
	local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIConfirmAgain);
	form:addOkEventHandle(self, self.onOkHandle);
	]]
	
	if(self.mItemData:isMoneyEnough()) then
		self:onOkHandle(nil);
	else
		GCtx.mGameData:ShowRollMessage(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eObject, 6));
	end
end

function M:onOkHandle(dispObj)
	GlobalNS.CSSystem.reqByShopItem(self.mItemData:getShopId(), self.mItemData:getBaseId());
	M.super.onActiveBtnClick(self, dispObj);
end

return M;