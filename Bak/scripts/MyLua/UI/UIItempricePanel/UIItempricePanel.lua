MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIItempricePanel.ItempricePanelNS");
MLoader("MyLua.UI.UIItempricePanel.ItempricePanelData");
MLoader("MyLua.UI.UIItempricePanel.ItempricePanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIItempricePanel";
GlobalNS.ItempricePanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIItempricePanel;
	self.mData = GlobalNS.new(GlobalNS.ItempricePanelNS.ItempricePanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mModelImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mModelImage:setIsDestroySelf(false);
	
	self.mNameText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mNameText:setIsDestroySelf(false);
	
	self.mUsageDescText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mUsageDescText:setIsDestroySelf(false);
	
	self.mAcquireDescText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mAcquireDescText:setIsDestroySelf(false);
end

function M:onReady()
    M.super.onReady(self);
	self:attachCloseBtn("");
	
	self.mModelImage:setSelfGoByPath(self.mGuiWin, GlobalNS.ItempricePanelNS.ItempricePanelPath.ModelImage);
	self.mNameText:setSelfGoByPath(self.mGuiWin, GlobalNS.ItempricePanelNS.ItempricePanelPath.NameText);
	self.mUsageDescText:setSelfGoByPath(self.mGuiWin, GlobalNS.ItempricePanelNS.ItempricePanelPath.UsageDescText);
	self.mAcquireDescText:setSelfGoByPath(self.mGuiWin, GlobalNS.ItempricePanelNS.ItempricePanelPath.AcquireDescText);
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
	if(nil ~= self.mModelImage) then
		self.mModelImage:dispose();
		self.mModelImage = nil;
	end
	if(nil ~= self.mNameText) then
		self.mNameText:dispose();
		self.mNameText = nil;
	end
	if(nil ~= self.mUsageDescText) then
		self.mUsageDescText:dispose();
		self.mUsageDescText = nil;
	end
	if(nil ~= self.mAcquireDescText) then
		self.mAcquireDescText:dispose();
		self.mAcquireDescText = nil;
	end
	
    M.super.onExit(self);
end

function M:setObjectId(objId)
	self.mObjectId = objId;
	
	self:update();
end

function M:update()
	local name = GlobalNS.CSSystem.getObjectNameByBaseId(self.mObjectId);
	self.mNameText:setText(name);
	local usageDesc = GlobalNS.CSSystem.getObjectUsageDescByBaseId(self.mObjectId);
	self.mUsageDescText:setText(usageDesc);
	local acquireDesc = GlobalNS.CSSystem.getObjectAcquireDescByBaseId(self.mObjectId);
	self.mAcquireDescText:setText(acquireDesc);
	
	local objectImage = GlobalNS.CSSystem.getObjectImageByBaseId(self.mObjectId);
	local objectType = GlobalNS.CSSystem.getObjectTypeByBaseId(self.mObjectId);
	local spriteName = GlobalNS.UtilLogic.getSpriteName(objectImage);
	local atlasPath = GlobalNS.UtilLogic.getAtlasPath(objectType, objectImage);
	
	self.mModelImage:setSpritePath(atlasPath, spriteName);
end

return M;