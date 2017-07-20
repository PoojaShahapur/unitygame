MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "SkinData";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mSkinBaseIdProperty = GlobalNS.new(GlobalNS.AuxRecordIntProperty);
	self.mBulletBaseIdProperty = GlobalNS.new(GlobalNS.AuxRecordIntProperty);
	self.mRedPointProperty = GlobalNS.new(GlobalNS.AuxBoolProperty);
	
	self.mObjectList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mObjectList:setIsSpeedUpFind(true);
	self.mObjectList:setIsOpKeepSort(true);
	
	self.mPanelArray = GlobalNS.new(GlobalNS.MList);
	
	local index = 0;
	local objectPanel = nil;
	
	while(index < GlobalNS.ObjectPanelType.eCount) do
		objectPanel = GlobalNS.new(GlobalNS.SkinPanel);
		objectPanel:setPanelType(index + 1);
		self.mPanelArray:add(objectPanel);
		objectPanel:init();
		
		index = index + 1;
	end
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
end

function M:getRedPointProperty()
	return self.mRedPointProperty;
end

function M:clear()
	self.mObjectList:clear();
	self.mSkinBaseIdProperty:reset();
	self.mBulletBaseIdProperty:reset();
	
	local index = 0;
	local objectPanel = nil;
	
	while(index < GlobalNS.ObjectPanelType.eCount) do
		objectPanel = self.mPanelArray:get(index);
		objectPanel:clear();
		
		index = index + 1;
	end
end

-- 设置皮肤
function M:setCurSkinIndex(value)
	self.mSkinBaseIdProperty:setData(value);
end

function M:getPreSkinIndex()
	return self.mSkinBaseIdProperty:getPreData();
end

function M:getCurSkinIndex()
	return self.mSkinBaseIdProperty:getData();
end

function M:hasPreSkin()
	return self.mSkinBaseIdProperty:isPreValid();
end

--是否已经穿戴皮肤
function M:hasSkin()
	return self.mSkinBaseIdProperty:isValid();
end

function M:getPreSkinItemData()
	local item = nil;
	item = self.mObjectList:value(self.mSkinBaseIdProperty:getPreData());
	return item;
end

-- 当前皮肤数据
function M:getCurSkinItemData()
	local item = nil;
	item = self.mObjectList:value(self.mSkinBaseIdProperty:getData());
	return item;
end

function M:isPreUseSkinByBaseId(skinId)
	return skinId == self.mSkinBaseIdProperty:getPreData();
end

-- 判断当前皮肤 Id 是否在使用中
function M:isUseSkinByBaseId(skinId)
	return skinId == self.mSkinBaseIdProperty:getData();
end

function M:addCurSkinChangeHandle(pThis, handle)
	self.mSkinBaseIdProperty:addEventHandle(pThis, handle);
end

function M:removeCurSkinChangeHandle(pThis, handle)
	self.mSkinBaseIdProperty:removeEventHandle(pThis, handle);
end

function M:setCurBulletBaseId(value)
	self.mBulletBaseIdProperty:setData(value);
end

function M:getCurBulletBaseId()
	return self.mBulletBaseIdProperty:getData();
end

function M:getPreBulletBaseId()
	return self.mBulletBaseIdProperty:getPreData();
end

function M:getCurBulletBaseIdProperty()
	return self.mBulletBaseIdProperty;
end

function M:hasBullet()
	return self.mBulletBaseIdProperty:isValid();
end

function M:hasPreBullet()
	return self.mBulletBaseIdProperty:isPreValid();
end

function M:getCurBulletItemData()
	local item = nil;
	item = self.mObjectList:value(self.mBulletBaseIdProperty:getData());
	return item;
end

function M:getPreBulletItemData()
	local item = nil;
	item = self.mObjectList:value(self.mBulletBaseIdProperty:getPreData());
	return item;
end

function M:getObjectByThisId(thisId)
    return self.mObjectList:value(thisId);
end

function M:addObjectById(baseId, thisId)
	local item = nil;

	if (not self.mObjectList:ContainsKey(thisId)) then
		-- 大于 20000 就是子弹
		if(thisId < 20000) then
			item = GlobalNS.new(GlobalNS.SkinPiFuItem);
		else
			item = GlobalNS.new(GlobalNS.SkinBulletItem);
		end

		self.mObjectList:add(thisId, item);
	else
		item = self.mObjectList:value(thisId);
	end

	return item;
end

function M:removeObjectByThisId(thisId)
	local item = nil;
	
	if (self.mObjectList:ContainsKey(thisId)) then
		item = self.mObjectList:value(thisId);
		self.mObjectList:remove(thisId);
		
		local panelType = item:getPanelTypeZeroIndex();
		
		if(GlobalNS.UtilMath.MaxNum ~= panelType) then
			self.mPanelArray:get(panelType):removeObjectByThisId(item:getBaseId());
		end
	end
	
	return item;
end

-- Native ObjectItem
function M:addObjectByNativeItem(obj)
	local item = nil;
	
	if(nil ~= obj) then
		local item = self:addObjectById(obj:getBaseId(), obj:getBaseId());
		item:setNativeItem(obj);

		local panelType = item:getPanelTypeZeroIndex();
		-- test
		--panelType = 1;
		if(GlobalNS.UtilMath.MaxNum ~= panelType) then
			self.mPanelArray:get(panelType):addObjectById(obj:getBaseId(), obj:getBaseId(), item);
		end
		
		self.mRedPointProperty:setData(true);
	end
	
	return item;
end

function M:removeObjectByItem(obj)
	self.removeObjectByThisId(obj:getBaseId());
end

function M:getPanelDataByIndex(index)
	local objectPanel = self.mPanelArray:get(index);
	
	return objectPanel;
end

function M:isActiveByPlaneIdAndSkinIdAndBaseId(planeId, skinId, baseId)
	local isActive = false;
	local panel = self.mPanelArray:get(planeId);
	
	if(nil ~= panel) then
		isActive = panel:isActiveBySkinIdAndBaseId(skinId, baseId);
	end
	
	return isActive;
end

function M:getSkinItemBySkinId(skinId)
	local item = nil;
	local index = 0;
	local listLen = self.mObjectList:count();
	
	while(index < listLen) do
		if(self.mObjectList:get(index):getSkinId() == skinId) then
			item = self.mObjectList:get(index);
			break;
		end
		
		index = index + 1;
	end
	
	return item;
end

function M:clearAllRed()
	-- 清除商店中皮肤按钮上的红点信息
	self.mRedPointProperty:reset();
	-- 清除皮肤界面皮肤和子弹红点信息
	
	local index = 0;
	local listLen = self.mPanelArray:count();
	local panel = nil;
	
	while(index < listLen) do
		panel = self.mPanelArray:get(index);
		panel:getIsAddItemProperty():reset();
		
		index = index + 1;
	end
end

return M;