MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.UI.TabPageMgr.TabPage");

MLoader("MyLua.UI.UIShop.ShopNS");

local M = GlobalNS.Class(GlobalNS.TabPage);
M.clsName = "ShopPanelView";
GlobalNS.ShopNS[M.clsName] = M;

function M:ctor()
	self.mParentTag = 0;
	self.mShopPanelData = nil;
	self.mGridInfoList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mGridInfoList:setIsSpeedUpFind(true);
	self.mGridInfoList:setIsOpKeepSort(true);
	
	self.mWinGo = nil;
	self.mTplGo = nil;
	
	self.mTableViewContentPath = "";
	self.mTableViewContentGO = nil;
	
	self.mIsFirstClick = true;
end

function M:dtor()
	self:dispose();
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	self.mShopPanelData = nil;
	
	local index = 0;
	local listLen = self.mGridInfoList:count();
	local item = nil;
	
	while(index < listLen) do
		item = self.mGridInfoList:get(index);
		item:dispose();
	end
	
	self.mGridInfoList:clear();
	
	self.mWinGo = nil;
	self.mTplGo = nil;
	
	self.mTableViewContentPath = "";
	self.mTableViewContentGO = nil;
	
	M.super:dispose(self);
end

function M:setTableViewContentGoPath(value)
	self.mTableViewContentPath = value;
end

function M:setWinGo(value)
	self.mWinGo = value;
end

function M:setTplGo(value)
    self.mTplGo = value;
end

function M:setParentTag(parentTag)
	self.mParentTag = parentTag;
end

function M:getTableViewGO()
	self.mTableViewContentGO = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mWinGo, self.mTableViewContentPath);
	self.mShopPanelData = GCtx.mPlayerData.mShopData:getShopPanelByIndex(self.mParentTag, self:getTag());
end

function M:onTabBtnClk(dispObj)
	M.super.onTabBtnClk(self, dispObj);
	
	if(self.mIsFirstClick) then
		self.mIsFirstClick = false;
		
		if(not self.mShopPanelData:hasReqData()) then
			self.mShopPanelData:reqData();
		else
			self:addAllShopItem();
		end
	end
end

-- 添加所有商店数据
function M:addAllShopItem()
	local index = 0;
	local listLen = self.mShopPanelData:getItemCount();
	local dataItem = nil;
	
	while(index < listLen) do
		dataItem = self.mShopPanelData:getItemByIndex(index);
		self:addOneShopItem(dataItem);
		index = index + 1;
	end
end

function M:addOneShopItem(itemData)
    local itemView = GlobalNS.new(GlobalNS.ShopViewItem);
    self.mGridInfoList:add(itemData:getBaseId(), itemView);
	
    itemView:setParentGo(self.mTableViewContentGO);
    local go = GlobalNS.UtilApi.Instantiate(self.mTplGo);
    itemView:setRootGo(go);
    itemView:setItemData(itemData);
    itemView:init();
end

return M;