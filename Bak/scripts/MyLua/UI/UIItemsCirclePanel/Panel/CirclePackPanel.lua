MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIItemsCirclePanel.ItemsCirclePanelNS");
MLoader("MyLua.UI.UIItemsCirclePanel.Panel.CirclePanelBase");

local M = GlobalNS.Class(GlobalNS.ItemsCirclePanelNS.CirclePanelBase);
M.clsName = "CirclePackPanel";
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
	
	self.mActiveBtn:setText(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eObject, 3));
	self.mShuoMingText:setText(self.mItemData:getUsageDesc());
	
	self:updatePriceImage();
end

function M:onActiveBtnClick(dispObj)
	GlobalNS.CSSystem.reqUseObject(self.mItemData:getStrThisId());
	
	M.super.onActiveBtnClick(self, dispObj);
end

return M;