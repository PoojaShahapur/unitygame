MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIItemsColourPanel.ItemsColourPanelNS");
MLoader("MyLua.UI.UIItemsColourPanel.Panel.ColoutViewItem");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "RightColourViewPanel";
GlobalNS.ItemsColourPanelNS[M.clsName] = M;

function M:ctor()
	self.mWinGo = nil;
	
	self.mTableViewContentPath = "";
	self.mTableViewContentGO = nil; 	-- TableView 根节点
	
	self.mTplGo = nil;	-- 模板 GO
	
	self.mGridInfoList = GlobalNS.new(GlobalNS.MKeyIndexList); 	-- 格子信息列表
	self.mGridInfoList:setIsSpeedUpFind(true);
	self.mGridInfoList:setIsOpKeepSort(true);
	self.mPanelData = nil;     -- 对应 Panel 的数据
	
	self.mTplItemData = nil;
	
	self.mIsFirstAddData = true;
end

function M:dtor()
	
end

function M:init()
	GCtx.mPlayerData.mSkinData:addCurSkinChangeHandle(self, self.onCurSkinChanged);
end

function M:dispose()
	GCtx.mPlayerData.mSkinData:removeCurSkinChangeHandle(self, self.onCurSkinChanged);
	
	self.mPanelData:getAddOneItemDispatch():removeEventHandle(self, self.onAddOneItemDispatch);
	self.mPanelData:getRemoveOneItemDispatch():removeEventHandle(self, self.onRemoveOneItemDispatch);
	self.mPanelData:getUpdateOneItemDispatch():removeEventHandle(self, self.onUpdateOneItemDispatch);
	
	self.mWinGo = nil;
	
	self.mTableViewContentPath = "";
	self.mTableViewContentGO = nil; 	-- TableView 根节点

	self.mTplGo = nil;	-- 模板 GO
	
	local index = 0;
	local listLen = self.mGridInfoList:Count();
	while(index < listLen) do
		self.mGridInfoList:get(index):dispose();
		index = index + 1;
	end
	
	self.mGridInfoList:Clear();
	self.mGridInfoList = nil;
	self.mPanelData = nil;     -- 对应 Panel 的数据
	self.mTplItemData = nil;
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

function M:setTplItemData(value)
	self.mTplItemData = value;
end

function M:getTableViewGO()
	self.mTableViewContentGO = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mWinGo, self.mTableViewContentPath);
	local panelType = self.mTplItemData:getPanelTypeZeroIndex();
	self.mPanelData = GCtx.mPlayerData.mSkinData:getPanelDataByIndex(panelType);
	
	self.mPanelData:getAddOneItemDispatch():addEventHandle(self, self.onAddOneItemDispatch);
	self.mPanelData:getRemoveOneItemDispatch():addEventHandle(self, self.onRemoveOneItemDispatch);
	self.mPanelData:getUpdateOneItemDispatch():addEventHandle(self, self.onUpdateOneItemDispatch);
end

-- 添加道具
function M:addAllItem()
	if(self.mIsFirstAddData) then
		self.mIsFirstAddData = false;
		
		local index = 0;
		local itemData = nil;
		local listLen = self.mPanelData:getAllSkinTypeListCountByKey(self.mTplItemData:getSkinId());
		local item = nil;
		local go = nil;
		
		local thisId = 0;
		
		while(index < listLen) do
			itemData = self.mPanelData:getAllSkinTypeListItemByIndex(self.mTplItemData:getSkinId(), index);
			self:addObjectByItem(itemData);
			
			index = index + 1;
		end
	end
end

function M:addObjectByItem(itemData)
    local item = GlobalNS.new(GlobalNS.ItemsColourPanelNS.ColoutViewItem);
    self.mGridInfoList:add(itemData:getBaseId(), item);

    item:setParentGo(self.mTableViewContentGO);
    local go = GlobalNS.UtilApi.Instantiate(self.mTplGo);
    item:setRootGo(go);
    item:setItemData(itemData);
    item:init();
end

function M:removeObjectByItem(objectItem)
	local baseId = objectItem:getBaseId();
	
	if(self.mGridInfoList:ContainsKey(baseId)) then
		local itemData = self.mGridInfoList:value(baseId);
		self.mGridInfoList:Remove(baseId);
		itemData:dispose();
		itemData = nil;
	end
end

function M:updateOneObjectInfo(uid, newnum, newbind, newupgrade)
	if(self.mGridInfoList:ContainsKey(uid)) then
		local itemData = self.mGridInfoList:value(uid);
		itemData:updateNum();
	end
end

function M:onAddOneItemDispatch(dispObj)
	if(self.mTplItemData:getSkinId() == dispObj:getSkinId()) then
		local item = self.mGridInfoList:value(dispObj:getBaseId());
		item:update();
	end
end

function M:onRemoveOneItemDispatch(dispObj)
	if(self.mTplItemData:getSkinId() == dispObj:getSkinId()) then
		local item = self.mGridInfoList:value(dispObj:getBaseId());
		self.mGridInfoList:remove(dispObj:getBaseId());
		item:dispose();
		item = nil;
	end
end

function M:onUpdateOneItemDispatch(dispObj)
	
end

function M:onCurSkinChanged(dispObj)
	local baseId = -1;
	local viewItem = nil;
	
	if(GCtx.mPlayerData.mSkinData:hasSkin()) then
		baseId = GCtx.mPlayerData.mSkinData:getCurSkinIndex();
		viewItem = self.mGridInfoList:value(baseId);
		
		if(nil ~= viewItem) then
			viewItem:setUsed();
		end
	end
	if(GCtx.mPlayerData.mSkinData:hasPreSkin()) then
		baseId = GCtx.mPlayerData.mSkinData:getPreSkinIndex();
		viewItem = self.mGridInfoList:value(baseId);
		
		if(nil ~= viewItem) then
			viewItem:setCanUse();
		end
	end
end

return M;