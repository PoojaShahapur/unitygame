MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.UI.ObjectItem.ObjectViewItem");

MLoader("MyLua.UI.UIPack.PackNS");

local M = GlobalNS.Class(GlobalNS.TabPage);
M.clsName = "PackViewPanel";
GlobalNS.PackNS[M.clsName] = M;

function M:ctor()
	self.mRedImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mWinGo = nil;
	
	self.mTableViewContentPath = "";
	self.mTableViewContentGO = nil; 	-- TableView 根节点
	
	self.mTplGo = nil;	-- 模板 GO
	self.mTplPath = ""; 	-- 
	self.mAuxTplLoader = nil;
	
	self.mGridInfoList = GlobalNS.new(GlobalNS.MKeyIndexList); 	-- 格子信息列表
	self.mGridInfoList:setIsSpeedUpFind(true);
	self.mGridInfoList:setIsOpKeepSort(true);
	self.mPanelData = nil;     -- 对应 Panel 的数据
	
	self.mIsFirstAddData = true;
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	if(nil ~= self.mPanelData:getIsAddItemProperty()) then
		self.mPanelData:getIsAddItemProperty():removeEventHandle(self, self.onAddItem);
	end
	
	if(nil ~= self.mRedImage) then
		self.mRedImage:dispose();
		self.mRedImage = nil;
	end
	
	self.mWinGo = nil;
	
	self.mTableViewContentPath = "";
	self.mTableViewContentGO = nil; 	-- TableView 根节点

	self.mTplGo = nil;	-- 模板 GO
	if(nil ~= self.mAuxTplLoader) then
		self.mAuxTplLoader:dispose();
		self.mAuxTplLoader = nil;
	end
	
	local index = 0;
	local listLen = self.mGridInfoList:Count();
	
	while(index < listLen) do
		self.mGridInfoList:get(index):dispose();
		index = index + 1;
	end
	
	self.mGridInfoList:Clear();
	
	self.mPanelData = nil;     -- 对应 Panel 的数据
	
	M.super.dispose(self);
end

-- 通过标签返回道具类型
function M:getObjectTypeByTag()
	return self.mTag;
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

function M:setTplPath(value)
	if(self.mTplPath ~= value) then
		self.mTplPath = value;

		if(nil ~= self.mAuxTplLoader) then
			self.mAuxTplLoader:dispose();
			self.mAuxTplLoader = nil;
		end
		
		self.mAuxTplLoader = GlobalNS.new(GlobalNS.AuxPrefabLoader);
		self.mAuxTplLoader:setIsNeedInsPrefab(false);
		self.mAuxTplLoader:syncLoad(self.mTplPath);
		self.mTplGo = self.mAuxTplLoader:getPrefabTmpl();
	end
end

function M:getTableViewGO()
	self.mTableViewContentGO = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mWinGo, self.mTableViewContentPath);
	self.mPanelData = GCtx.mPlayerData.mPackData:getPanelDataByIndex(self.mTag - 1);
	
	self.mPanelData:getIsAddItemProperty():addEventHandle(self, self.onAddItem);
	if(self.mPanelData:getIsAddItemProperty():isValid()) then
		self.mRedImage:show();
	else
		self.mRedImage:hide();
	end
end

function M:setRedImage(go)
	self.mRedImage:setSelfGo(go);
end

function M:onTabBtnClk(dispObj)
    M.super.onTabBtnClk(self, dispObj);
	
	self.mPanelData:getIsAddItemProperty():reset();
	self.mRedImage:hide();
	
	self:addAllObjectItem();
end

-- 添加道具
function M:addAllObjectItem()
	if(self.mIsFirstAddData) then
		self.mIsFirstAddData = false;
		
		local index = 0;
		local itemData = nil;
		local listLen = self.mPanelData:getItemCount();
		local item = nil;
		local go = nil;
		
		local thisId = 0;
		
		while(index < listLen) do
			itemData = self.mPanelData:getItemByIndex(index);
			item = GlobalNS.new(GlobalNS.ObjectViewItem);
			
			thisId = itemData:getThisId();
			self.mGridInfoList:add(thisId, item);
			
			item:setParentGo(self.mTableViewContentGO);
			go = GlobalNS.UtilApi.Instantiate(self.mTplGo);
			item:setRootGo(go);
			item:setItemData(itemData);
			item:init();
			
			index = index + 1;
		end
	end
end

function M:addObjectByNativeItem(objectItem)
    local itemData = self.mPanelData:getObjectByThisId(objectItem:getStrThisId());
    local item = GlobalNS.new(GlobalNS.ObjectViewItem);

	local thisId = itemData:getThisId();
	self.mGridInfoList:add(thisId, item);
	
    item:setParentGo(self.mTableViewContentGO);
    local go = GlobalNS.UtilApi.Instantiate(self.mTplGo);
    item:setRootGo(go);
    item:setItemData(itemData);
    item:init();
end

function M:removeObjectByNativeItem(objectItem)
	local thisId = objectItem:getStrThisId();
	
	if(self.mGridInfoList:ContainsKey(thisId)) then
		local itemData = self.mGridInfoList:value(thisId);
		self.mGridInfoList:remove(thisId);
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

function M:onAddItem(dispObj)
	self.mRedImage:show();
end

return M;