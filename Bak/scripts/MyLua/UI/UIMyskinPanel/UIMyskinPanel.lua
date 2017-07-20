MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");
MLoader("MyLua.Libs.UI.TabPageMgr.TabPageMgr");

MLoader("MyLua.UI.UIMyskinPanel.MyskinPanelNS");
MLoader("MyLua.UI.UIMyskinPanel.MyskinPanelData");
MLoader("MyLua.UI.UIMyskinPanel.MyskinPanelCV");
MLoader("MyLua.UI.UIMyskinPanel.Panel.SkinPanelView");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIMyskinPanel";
GlobalNS.MyskinPanelNS[M.clsName] = M;

function M:ctor()
	self.mData = GlobalNS.new(GlobalNS.MyskinPanelNS.MyskinPanelData);
	self.mTabPanel = GlobalNS.new(GlobalNS.TabPageMgr);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachCloseBtn(GlobalNS.MyskinPanelNS.MyskinPanelPath.CloseBtn);
	self:attachClosePanel(GlobalNS.MyskinPanelNS.MyskinPanelPath.ClosePanel);
	
	do
		self.mTplItem = GlobalNS.new(GlobalNS.AuxPrefabLoader);
		self.mTplItem:setIsNeedInsPrefab(false);
		self.mTplItem:syncLoad("UI/UIMyskinPanel/SkinPackItem.prefab");
		local tplGo = self.mTplItem:getPrefabTmpl();
		
		local page = nil;
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.ModelBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.ModelPanel), 
			GlobalNS.MyskinPanelNS.SkinPanelView);
		page:setTag(GlobalNS.ObjectPanelType.eModel);
		page:setRedGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.ModelRed));
		page:setTableViewContentGoPath(GlobalNS.MyskinPanelNS.MyskinPanelPath.ModelPanelContent);
		page:setTplGo(tplGo);
		page:setWinGo(self.mGuiWin);
		page:setRedImage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.ModelRed));
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.BulletBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.BulletPanel), 
			GlobalNS.MyskinPanelNS.SkinPanelView);
		page:setTag(GlobalNS.ObjectPanelType.eBullet);
		page:setRedGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.BulletRed));
		page:setTableViewContentGoPath(GlobalNS.MyskinPanelNS.MyskinPanelPath.BulletPanelContent);
		page:setTplGo(tplGo);
		page:setWinGo(self.mGuiWin);
		page:setRedImage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.BulletRed));
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.TrailBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.TrailPanel), 
			GlobalNS.MyskinPanelNS.SkinPanelView);
		page:setTag(GlobalNS.ObjectPanelType.eTrail);
		page:setRedGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.TrailRed));
		page:setTableViewContentGoPath(GlobalNS.MyskinPanelNS.MyskinPanelPath.TrailPanelContent);
		page:setTplGo(tplGo);
		page:setWinGo(self.mGuiWin);
		page:setRedImage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.TrailRed));
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.DazzleLightBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.DazzleLightPanel), 
			GlobalNS.MyskinPanelNS.SkinPanelView);
		page:setTag(GlobalNS.ObjectPanelType.eDazzleLight);
		page:setRedGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.DizzleLightRed));
		page:setTableViewContentGoPath(GlobalNS.MyskinPanelNS.MyskinPanelPath.DizzleLightPanelContent);
		page:setTplGo(tplGo);
		page:setWinGo(self.mGuiWin);
		page:setRedImage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.MyskinPanelNS.MyskinPanelPath.DazzleLightRed));
		page:getTableViewGO();
	end
	
	self.mTabPanel:openPage(GlobalNS.ObjectPanelType.eModel - 1);
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
	
	if(nil ~= self.mTabPanel) then
		GlobalNS.delete(self.mTabPanel);
		self.mTabPanel = nil;
	end
end

function M:onAddOneSkin(itemData)
	self.mTabPanel:getPageByTag(itemData:getPanelTypeZeroIndex()):addObjectByItem(itemData);
end

function M:onRemoveOneSkin(itemData)
	self.mTabPanel:getPageByTag(itemData:getPanelTypeZeroIndex()):removeObjectByItem(itemData);
end

return M;