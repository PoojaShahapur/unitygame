MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.UI.ObjectItem.ObjectViewItem");

MLoader("MyLua.UI.UIPack.PackNS");

local M = GlobalNS.Class(GlobalNS.TabPage);
M.clsName = "SkinPanelView";
GlobalNS.MyskinPanelNS[M.clsName] = M;

function M:ctor()
	self.mRedImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mWinGo = nil;
	
	self.mTableViewContentPath = "";
	self.mTableViewContentGO = nil; 	-- TableView 根节点
	
	self.mTplGo = nil;	-- 模板 GO
	
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
	if(nil ~= self.mRedImage) then
		GlobalNS.delete(self.mRedImage);
		self.mRedImage = nil;
	end
	
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
	
	if(nil ~= self.mPanelData) then
		self.mPanelData.mAddOneTypeItemDispatch:removeEventHandle(self, self.onAddOneItem);
		self.mPanelData.mRemoveOneTypeItemDispatch:removeEventHandle(self, self.onRemoveOneItem);
		self.mPanelData.mUpdateOneTypeItemDispatch:removeEventHandle(self, self.onUpdateOneItem);		
		self.mPanelData:getIsAddItemProperty():removeEventHandle(self, self.onAddItem);
		
		if(self.mTag == GlobalNS.ObjectPanelType.eModel) then
			GCtx.mPlayerData.mSkinData:removeCurSkinChangeHandle(self, self.onSkinUsingChange);
		elseif(self.mTag == GlobalNS.ObjectPanelType.eBullet) then
			GCtx.mPlayerData.mSkinData:getCurBulletBaseIdProperty():removeEventHandle(self, self.onBulletUsingChange);
		end
		
		self.mPanelData = nil;     -- 对应 Panel 的数据
	end
	
	M.super.dispose(self);
end

function M:setRedGo(go_)
	self.mRedImage:setSelfGo(go_);
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

function M:getTableViewGO()
	self.mTableViewContentGO = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mWinGo, self.mTableViewContentPath);
	self.mPanelData = GCtx.mPlayerData.mSkinData:getPanelDataByIndex(self.mTag - 1);
	
	self.mPanelData.mAddOneTypeItemDispatch:addEventHandle(self, self.onAddOneItem);
	self.mPanelData.mRemoveOneTypeItemDispatch:addEventHandle(self, self.onRemoveOneItem);
	self.mPanelData.mUpdateOneTypeItemDispatch:addEventHandle(self, self.onUpdateOneItem);
	
	self.mPanelData:getIsAddItemProperty():addEventHandle(self, self.onAddItem);
	if(self.mPanelData:getIsAddItemProperty():isValid()) then
		self.mRedImage:show();
	else
		self.mRedImage:hide();
	end
	
	if(self.mTag == GlobalNS.ObjectPanelType.eModel) then
		GCtx.mPlayerData.mSkinData:addCurSkinChangeHandle(self, self.onSkinUsingChange);
	elseif(self.mTag == GlobalNS.ObjectPanelType.eBullet) then
		GCtx.mPlayerData.mSkinData:getCurBulletBaseIdProperty():addEventHandle(self, self.onBulletUsingChange);
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
		local listLen = self.mPanelData:getSkinTypeListCount();
		
		while(index < listLen) do
			itemData = self.mPanelData:getSkinTypeItemByIndex(index);
			self:addObjectByItem(itemData);
			
			index = index + 1;
		end
		
		self:onBulletUsingChange(nil);
		self:onSkinUsingChange(nil);
	end
end

function M:addObjectByItem(itemData)
	local item = nil;
	local skinId = 0;
	local go = nil;
	
	item = GlobalNS.new(GlobalNS.SkinViewItem);
	skinId = itemData:getSkinId();
	self.mGridInfoList:add(skinId, item);
	
	item:setParentGo(self.mTableViewContentGO);
	go = GlobalNS.UtilApi.Instantiate(self.mTplGo);
	item:setRootGo(go);
	item:setItemData(itemData);
	item:init();
end

function M:removeObjectByItem(itemData)
	local skinId = itemData:getSkinId();
	local itemView = self.mGridInfoList:value(skinId);
	self.mGridInfoList:remove(baseId);
	itemView:setIsDestroySelf(true);	-- 需要卸载自己
	itemView:dispose();
	itemView = nil;
end

function M:updateOneObjectInfo(uid, newnum, newbind, newupgrade)
	if(self.mGridDic:ContainsKey(uid)) then
		local itemData = self.mGridDic:value(uid);
		itemData:updateNum();
	end
end

function M:onAddOneItem(dispObj)
	self:addObjectByItem(dispObj);
end

function M:onRemoveOneItem(dispObj)
	self:removeObjectByItem(dispObj);
end

function M:onUpdateOneItem(dispObj)
	
end

function M:onAddItem(dispObj)
	self.mRedImage:show();
end

function M:onBulletUsingChange(dispObj)
	if(GCtx.mPlayerData.mSkinData:hasPreBullet()) then
		local preBulletItemData = GCtx.mPlayerData.mSkinData:getPreBulletItemData();
		local preBulletViewItem = self.mGridInfoList:value(preBulletItemData:getSkinId());
		
		if(nil ~= preBulletViewItem) then
			preBulletViewItem:setUsingState(false);
		end
	end

	if(GCtx.mPlayerData.mSkinData:hasBullet()) then
		local curBulletItemData = GCtx.mPlayerData.mSkinData:getCurBulletItemData();
		local curBulletViewItem = self.mGridInfoList:value(curBulletItemData:getSkinId());
		
		if(nil ~= curBulletViewItem) then
			curBulletViewItem:setUsingState(true);
		end
	end
end

function M:onSkinUsingChange(dispObj)
	if(GCtx.mPlayerData.mSkinData:hasPreSkin()) then
		local preSkinItemData = GCtx.mPlayerData.mSkinData:getPreSkinItemData();
		local preSkinViewItem = self.mGridInfoList:value(preSkinItemData:getSkinId());

		if(nil ~= preSkinViewItem) then
			preSkinViewItem:setUsingState(false);
			preSkinViewItem:updateImage();
		end
	end
	if(GCtx.mPlayerData.mSkinData:hasSkin()) then
		local curSkinItemData = GCtx.mPlayerData.mSkinData:getCurSkinItemData();
		local curSkinViewItem = self.mGridInfoList:value(curSkinItemData:getSkinId());
		
		if(nil ~= curSkinViewItem) then
			curSkinViewItem:setUsingState(true);
			curSkinViewItem:updateImage();
		end
	end
end

return M;