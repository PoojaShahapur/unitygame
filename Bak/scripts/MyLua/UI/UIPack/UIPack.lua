MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIPack.PackNS");
MLoader("MyLua.UI.UIPack.PackViewData");
MLoader("MyLua.UI.UIPack.PackCV");
MLoader("MyLua.UI.UIPack.Panel.PackViewPanel");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIPack";
GlobalNS.PackNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIPack;
	self.mData = GlobalNS.new(GlobalNS.PackNS.PackViewData);
	
	self.mIsExit = false;	-- 背包隐藏
	self.mTabPanel = GlobalNS.new(GlobalNS.TabPageMgr);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
end

function M:onReady()
    if(MacroDef.ENABLE_LOG) then
        GCtx.mLogSys:log("UIPack::onReady, enter", GlobalNS.LogTypeId.eLogPack);
    end

    M.super.onReady(self);
	
	do
		self.mTplItem = GlobalNS.new(GlobalNS.AuxPrefabLoader);
		self.mTplItem:setIsNeedInsPrefab(false);
		self.mTplItem:syncLoad("UI/UIPack/PackItem.prefab");
		local tplGo = self.mTplItem:getPrefabTmpl();
	
		local page = nil;
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.ModelBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.ModelPanel), 
			GlobalNS.PackNS.PackViewPanel
			);
		page:setTag(GlobalNS.ObjectPanelType.eModel);
		page:setTableViewContentGoPath(GlobalNS.PackNS.PackPath.ModelPanelContent);
		page:setTplGo(tplGo);
		page:setWinGo(self.mGuiWin);
		page:setRedImage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.ModelRed));
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.BulletBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.BulletPanel), 
			GlobalNS.PackNS.PackViewPanel
			);
		page:setTag(GlobalNS.ObjectPanelType.eBullet);
		page:setTableViewContentGoPath(GlobalNS.PackNS.PackPath.BulletPanelContent);
		page:setTplGo(tplGo);
		page:setWinGo(self.mGuiWin);
		page:setRedImage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.BulletRed));
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.TrailBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.TrailPanel), 
			GlobalNS.PackNS.PackViewPanel
			);
		page:setTag(GlobalNS.ObjectPanelType.eTrail);
		page:setTableViewContentGoPath(GlobalNS.PackNS.PackPath.TrailPanelContent);
		page:setTplGo(tplGo);
		page:setWinGo(self.mGuiWin);
		page:setRedImage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.TrailRed));
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.MoneyBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.MoneyPanel), 
			GlobalNS.PackNS.PackViewPanel
			);
		page:setTag(GlobalNS.ObjectPanelType.eMoney);
		page:setTableViewContentGoPath(GlobalNS.PackNS.PackPath.MoneyPanelContent);
		page:setTplGo(tplGo);
		page:setWinGo(self.mGuiWin);
		page:setRedImage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.TrailRed));
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.GiftBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.GiftPanel), 
			GlobalNS.PackNS.PackViewPanel
			);
		page:setTag(GlobalNS.ObjectPanelType.eGift);
		page:setTableViewContentGoPath(GlobalNS.PackNS.PackPath.GiftPanelContent);
		--page:setTplGo(tplGo);
		page:setTplPath(GlobalNS.PackNS.PackPath.GiftPrefabPath);
		page:setWinGo(self.mGuiWin);
		page:setRedImage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.TrailRed));
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.PieceBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.PiecePanel), 
			GlobalNS.PackNS.PackViewPanel
			);
		page:setTag(GlobalNS.ObjectPanelType.ePiece);
		page:setTableViewContentGoPath(GlobalNS.PackNS.PackPath.PiecePanelContent);
		--page:setTplGo(tplGo);
		page:setTplPath(GlobalNS.PackNS.PackPath.PiecePrefabPath);
		page:setWinGo(self.mGuiWin);
		page:setRedImage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.PieceRed));
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.DazzleLightBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.DazzleLightPanel), 
			GlobalNS.PackNS.PackViewPanel
			);
		page:setTag(GlobalNS.ObjectPanelType.eDazzleLight);
		page:setTableViewContentGoPath(GlobalNS.PackNS.PackPath.DazzleLightPanelContent);
		page:setTplGo(tplGo);
		page:setWinGo(self.mGuiWin);
		page:setRedImage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.PackNS.PackPath.DazzleLightRed));
		page:getTableViewGO();
	end
	
	self.mTabPanel:openPage(GlobalNS.ObjectPanelType.eModel - 1);
	
	self:attachCloseBtn(GlobalNS.PackNS.PackPath.CloseBtn);
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
	
	if(nil ~= self.mTabPanel) then
		GlobalNS.delete(self.mTabPanel);
		self.mTabPanel = nil;
	end
	
	--GlobalNS.PackNS = nil; 	-- 释放功能模块，目前代码不卸载
	
    M.super.onExit(self);
end

function M:initBtn()
	local btn = nil;
	local index = 0;
	
	self.mBtnList = GlobalNS.new(GlobalNS.MList);
	
	while(index < GlobalNS.ObjectPanelType.eCount) do
		btn = GlobalNS.new(GlobalNS.AuxButton);
		self.mBtnList:add(btn);
		
		index = index + 1;
	end
	
	self.mBtnList:get(GlobalNS.ObjectPanelType.eModel - 1):addEventHandle(self, self.onModelBtnClick);
	self.mBtnList:get(GlobalNS.ObjectPanelType.eBullet - 1):addEventHandle(self, self.onButtonBtnClick);
	self.mBtnList:get(GlobalNS.ObjectPanelType.eTrail - 1):addEventHandle(self, self.onTrailBtnClick);
	self.mBtnList:get(GlobalNS.ObjectPanelType.eDazzleLight - 1):addEventHandle(self, self.onDazzleLightBtnClick);
	self.mBtnList:get(GlobalNS.ObjectPanelType.ePiece - 1):addEventHandle(self, self.onPieceBtnClick);
end

function M:addObjectByNativeItem(objectItem)
    if(self:isReady()) then
        local panelIndex = objectItem:getPanelTypeZeroIndex();
        if(panelIndex ~= GlobalNS.UtilMath.MaxNum) then
            self.mTabPanel:getPageByTag(panelIndex):addObjectByNativeItem(objectItem);
        end
    end
end

function M:removeObjectByNativeItem(objectItem)
    if(self:isReady()) then
        local panelIndex = objectItem:getPanelTypeZeroIndex();
        if(panelIndex ~= GlobalNS.UtilMath.MaxNum) then
            self.mTabPanel:getPageByTag(panelIndex):removeObjectByNativeItem(objectItem);
        end
    end
end

function M:updateOneObjectInfo(uid, newnum, newbind, newupgrade, panelIndex)
    if(self:isReady()) then
        if(panelIndex ~= GlobalNS.UtilMath.MaxNum) then
            self.mTabPanel:getPageByTag(panelIndex):updateOneObjectInfo(uid, newnum, newbind, newupgrade);
        end
    end
end

return M;