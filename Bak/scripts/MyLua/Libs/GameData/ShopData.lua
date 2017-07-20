MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ShopData";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mObjectList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mObjectList:setIsSpeedUpFind(true);
	self.mObjectList:setIsOpKeepSort(true);
	
	self.mShopTopPanelList = GlobalNS.new(GlobalNS.MList);
	
	local shopTopPanel = GlobalNS.new(GlobalNS.ShopTopPanel);
	shopTopPanel:setShopTopType(GlobalNS.ShopTopType.ePiFu);
	self.mShopTopPanelList:add(shopTopPanel);
	
	shopTopPanel = GlobalNS.new(GlobalNS.ShopTopPanel);
	shopTopPanel:setShopTopType(GlobalNS.ShopTopType.eNiuDan);
	self.mShopTopPanelList:add(shopTopPanel);
	
	shopTopPanel = GlobalNS.new(GlobalNS.ShopTopPanel);
	shopTopPanel:setShopTopType(GlobalNS.ShopTopType.eHuiYuan);
	self.mShopTopPanelList:add(shopTopPanel);
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
end

function M:getTopPanelByIndex(index)
	return self.mShopTopPanelList:get(index);
end

function M:addOneNativeShopItem(item)
	local shopDataItem = GlobalNS.new(GlobalNS.ShopDataItem);
	self.mObjectList:add(item:getComposeId(), shopDataItem);
	
	shopDataItem:setNativeItem(item);
	local panelType = shopDataItem:getTopTypeZeroIndex();
	self.mShopTopPanelList:get(panelType):addItem(shopDataItem);
	
	return shopDataItem;
end

function M:getShopPanelByIndex(topIndex, index)
	local shopPanel = nil;
	shopPanel = self.mShopTopPanelList:get(topIndex):getShopPanelByIndex(index);
	return shopPanel;
end

return M;