MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.UI.TabPageMgr.TabPage");

MLoader("MyLua.UI.UIShop.ShopNS");

local M = GlobalNS.Class(GlobalNS.TabPage);
M.clsName = "ShopTopPanelView";
GlobalNS.ShopNS[M.clsName] = M;

function M:ctor()
	self.mTabPanel = GlobalNS.new(GlobalNS.TabPageMgr);
	self.mGuiWin = nil;
	self.mTplGo = nil;
	self.mIsClicked = false; 	-- 是否点击过
	
	self.mRedImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mRedImage:setIsDestroySelf(false);
	self.mRedImage:hide();
	self.mUnderlineImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mUnderlineImage:setIsDestroySelf(false);
	self.mUnderlineImage:hide();
	
	self.mTopPanelData = nil;
end

function M:dtor()
	self:dispose();
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	self.mGuiWin = nil;
	
	if(nil ~= self.mRedImage) then
		self.mRedImage:dispose();
		self.mRedImage = nil;
	end
	
	if(nil ~= self.mUnderlineImage) then
		self.mUnderlineImage:dispose();
		self.mUnderlineImage = nil;
	end
	
	self.mTopPanelData:getRedProperty():removeEventHandle(self, self.onRedChange);
	
	M.super:dispose(self);
end

function M:setWinGo(value)
	self.mWinGo = value;
end

function M:setTplGo(value)
    self.mTplGo = value;
end

function M:setRedImageByPath(parentGo, path)
	self.mRedImage:setSelfGoByPath(parentGo, path);
end

function M:setUnderlineByPath(parentGo, path)
	self.mUnderlineImage:setSelfGoByPath(parentGo, path);
end

function M:onLostFocus()
	M.super.onLostFocus(self);
	self.mUnderlineImage:hide();
end

function M:addSubPage(guiWin)
	local page = nil;
	self.mGuiWin = guiWin;
	
	self.mTopPanelData = GCtx.mPlayerData.mShopData:getTopPanelByIndex(self.mTag);
	self.mTopPanelData:getRedProperty():addEventHandle(self, self.onRedChange);
	self:onRedChange(self.mTopPanelData:getRedProperty());
	
	if(GlobalNS.ShopNS.ShopPanelTag.eTopModel == self.mTag) then
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.ModelBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.ModelPanel), 
			GlobalNS.ShopNS.ShopPanelView
		);
		page:setTag(GlobalNS.ShopNS.ShopPanelTag.eModel);
		page:setParentTag(self:getTag());
		page:setWinGo(self.mGuiWin);
		page:setTplGo(self.mTplGo);
		page:setTableViewContentGoPath(GlobalNS.ShopNS.ShopPath.ModelPanelContent);
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.BulletBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.BulletPanel), 
			GlobalNS.ShopNS.ShopPanelView
		);
		page:setTag(GlobalNS.ShopNS.ShopPanelTag.eBullet);
		page:setParentTag(self:getTag());
		page:setWinGo(self.mGuiWin);
		page:setTplGo(self.mTplGo);
		page:setTableViewContentGoPath(GlobalNS.ShopNS.ShopPath.BulletPanelContent);
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.GhostBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.GhostPanel), 
			GlobalNS.ShopNS.ShopPanelView
		);
		page:setTag(GlobalNS.ShopNS.ShopPanelTag.eGhost);
		page:setParentTag(self:getTag());
		page:setWinGo(self.mGuiWin);
		page:setTplGo(self.mTplGo);
		page:setTableViewContentGoPath(GlobalNS.ShopNS.ShopPath.GhostPanelContent);
		page:getTableViewGO();
		
		page = self.mTabPanel:addTabPage(
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.LightBtn), 
			GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.ShopNS.ShopPath.LightPanel), 
			GlobalNS.ShopNS.ShopPanelView
		);
		page:setTag(GlobalNS.ShopNS.ShopPanelTag.eLight);
		page:setParentTag(self:getTag());
		page:setWinGo(self.mGuiWin);
		page:setTplGo(self.mTplGo);
		page:setTableViewContentGoPath(GlobalNS.ShopNS.ShopPath.LightPanelContent);
		page:getTableViewGO();
	elseif(GlobalNS.ShopNS.ShopPanelTag.eTopNiuDan == self.mTag) then
		
	elseif(GlobalNS.ShopNS.ShopPanelTag.eTopHuiYuan == self.mTag) then
		
	end
end

function M:onGetFocus()
	M.super.onGetFocus(self);
	
	self.mUnderlineImage:show();
	
	if(not self.mIsClicked) then
		self.mIsClicked = true;
		
		if(GlobalNS.ShopNS.ShopPanelTag.eTopModel == self.mTag) then
			self.mTabPanel:openPage(GlobalNS.ShopNS.ShopPanelTag.eModel);
		end
	end
end

function M:getPageByTag(tag)
	return self.mTabPanel:getPageByTag(tag);
end

function M:onRedChange(dispObj)
	local property = dispObj;
	
	if(property:isValid()) then
		if(self:getTabPageMgr():getCurPageTag() == self.mTag) then
			self.mRedImage:hide();
		else
			self.mRedImage:show();
		end
	else
		self.mRedImage:hide();
	end
	
	property:reset();
end

return M;