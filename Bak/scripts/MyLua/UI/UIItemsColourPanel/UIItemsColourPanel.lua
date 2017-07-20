MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIItemsColourPanel.ItemsColourPanelNS");
MLoader("MyLua.UI.UIItemsColourPanel.ItemsColourPanelData");
MLoader("MyLua.UI.UIItemsColourPanel.ItemsColourPanelCV");
MLoader("MyLua.UI.UIItemsColourPanel.Panel.LeftColourViewPanel");
MLoader("MyLua.UI.UIItemsColourPanel.Panel.RightColourViewPanel");
MLoader("MyLua.UI.UIItemsColourPanel.Panel.ColoutViewItem");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIItemsColourPanel";
GlobalNS.ItemsColourPanelNS[M.clsName] = M;

function M:ctor()
	self.mData = GlobalNS.new(GlobalNS.ItemsColourPanelNS.ItemsColourPanelData);
	self.mItemData = nil;
	
	self.mLeftColourViewPanel = GlobalNS.new(GlobalNS.ItemsColourPanelNS.LeftColourViewPanel);
	self.mRightColourViewPanel = GlobalNS.new(GlobalNS.ItemsColourPanelNS.RightColourViewPanel);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mLeftColourViewPanel:init();
	self.mRightColourViewPanel:init();
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachCloseBtn(GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.CloseBtn);
	self:attachClosePanel(GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.ClosePanel);
	
	self:attachWidget();
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
	if(nil ~= self.mTplItem) then
		self.mTplItem:dispose();
		self.mTplItem = nil;
	end
	
	self.mLeftColourViewPanel:dispose();
	self.mLeftColourViewPanel = nil;
	self.mRightColourViewPanel:dispose();
	self.mRightColourViewPanel = nil;
	
    M.super.onExit(self);
end

function M:setObjectItem(itemDtat)
	self.mItemData = itemDtat;
	
	self.mRightColourViewPanel:setTplItemData(self.mItemData);
	self.mLeftColourViewPanel:setTplItemData(self.mItemData);
	
	self:attachWidget();
end

function M:attachWidget()
	if(nil ~= self.mRightColourViewPanel and self:isReady()) then
		self.mTplItem = GlobalNS.new(GlobalNS.AuxPrefabLoader);
		self.mTplItem:setIsNeedInsPrefab(false);
		self.mTplItem:syncLoad("UI/UIItemsColourPanel/ColorItem.prefab");
		local tplGo = self.mTplItem:getPrefabTmpl();
		
		self.mRightColourViewPanel:setTableViewContentGoPath(GlobalNS.ItemsColourPanelNS.ItemsColourPanelPath.RightPanelContent);
		self.mRightColourViewPanel:setTplGo(tplGo);
		self.mRightColourViewPanel:setWinGo(self.mGuiWin);
		self.mRightColourViewPanel:getTableViewGO();
		self.mRightColourViewPanel:addAllItem();
		
		self.mLeftColourViewPanel:setWinGo(self.mGuiWin);
		self.mLeftColourViewPanel:attachWidget();
	end
end

function M:update()
	
end

return M;