MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
@brief SkinPanel
]]

local M = GlobalNS.Class(GlobalNS.ObjectPanelBase);
M.clsName = "SkinPanel";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mSkinTypeList = GlobalNS.new(GlobalNS.MKeyIndexList);	-- 第一个颜色
	self.mSkinTypeList:setIsSpeedUpFind(true);
	self.mSkinTypeList:setIsOpKeepSort(true);
	
	self.mSkinType2ListDic = GlobalNS.new(GlobalNS.MDictionary); 	-- 激活的颜色
	self.mAllSkinType2ListDic = GlobalNS.new(GlobalNS.MDictionary); -- 所有的颜色
	
	self.mAddOneTypeItemDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
	self.mRemoveOneTypeItemDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
	self.mUpdateOneTypeItemDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	self.mAddOneTypeItemDispatch:clearEventHandle();
	self.mRemoveOneTypeItemDispatch:clearEventHandle();
	self.mUpdateOneTypeItemDispatch:clearEventHandle();
	
	M.super.dispose(self);
end

function M:clear()
	self.mSkinTypeList:clear();
	self.mSkinType2ListDic:clear();
	self.mAllSkinType2ListDic:clear();
	
	self.mAddOneTypeItemDispatch:clearEventHandle();
	self.mRemoveOneTypeItemDispatch:clearEventHandle();
	self.mUpdateOneTypeItemDispatch:clearEventHandle();
end

-- 皮肤一个颜色的列表
function M:getSkinTypeListCount()
	return self.mSkinTypeList:count();
end

function M:getSkinTypeItemByIndex(index)
	return self.mSkinTypeList:get(index);
end

-- 服务器所有颜色的列表
function M:getSkinTypeListByKey(key)
	return self.mSkinType2ListDic:value(key);
end

function M:getSkinTypeListCountByKey(key)
	local count = 0;
	
	if(self.mSkinType2ListDic:ContainsKey(key)) then
		count = self.mSkinType2ListDic:value(key):count();
	end
	
	return count;
end

function M:getSkinTypeListItemByIndex(key, index)
	local item = nil;
	
	if(self.mSkinType2ListDic:ContainsKey(key)) then
		item = self.mSkinType2ListDic:value(key):get(index);
	end
	
	return item;
end

-- 客户端本地所有颜色列表
function M:getAllSkinTypeListByKey(key)
	return self.mAllSkinType2ListDic:value(key);
end

function M:getAllSkinTypeListCountByKey(key)
	local count = 0;
	
	if(self.mAllSkinType2ListDic:ContainsKey(key)) then
		count = self.mAllSkinType2ListDic:value(key):count();
	end
	
	return count;
end

function M:getAllSkinTypeListItemByIndex(key, index)
	local item = nil;
	
	if(self.mAllSkinType2ListDic:ContainsKey(key)) then
		item = self.mAllSkinType2ListDic:value(key):get(index);
	end
	
	return item;
end

function M:addObjectById(baseId, thisId, item)
	--M.super.addObjectById(self, baseId, thisId, item);
	self.mObjectList:add(thisId, item);
	
	local list = nil;
	
	if(not self.mAllSkinType2ListDic:ContainsKey(item:getSkinId())) then
		list = GlobalNS.new(GlobalNS.MKeyIndexList);
		list:setIsSpeedUpFind(true);
		list:setIsOpKeepSort(false);
		self.mAllSkinType2ListDic:add(item:getSkinId(), list);
		
		local baseIdList = item:getBaseIdListBySkinId();
		
		local index = 0;
		local listLen = baseIdList:count();
		local skinClientItem = nil;
		
		while(index < listLen) do
			if(item:isPiFu()) then
				skinClientItem = GlobalNS.new(GlobalNS.SkinPiFuClientItem);
				skinClientItem:setBaseItem(baseIdList:get(index));
			elseif(item:isBullet()) then
				skinClientItem = GlobalNS.new(GlobalNS.SkinBulletClientItem);
				skinClientItem:setBaseItem(item);
			end

			list:add(skinClientItem:getBaseId(), skinClientItem);
			
			index = index + 1;
		end
	end
	
	list = nil;
	
	if(not self.mSkinTypeList:ContainsKey(item:getSkinId())) then
		self.mSkinTypeList:add(item:getSkinId(), item);
		
		self.mAddOneTypeItemDispatch:dispatchEvent(item);
	end
	
	if(not self.mSkinType2ListDic:ContainsKey(item:getSkinId())) then
		list = GlobalNS.new(GlobalNS.MKeyIndexList);
		list:setIsSpeedUpFind(true);
		list:setIsOpKeepSort(false);
		self.mSkinType2ListDic:add(item:getSkinId(), list);
	else
		list = self.mSkinType2ListDic:value(item:getSkinId());
	end
	
	list:add(item:getBaseId(), item);
	
	self.mAddOneItemDispatch:dispatchEvent(item);
	self.mIsAddItemProperty:setData(true);
end

function M:removeObjectByThisId(thisId)
	local item = self.mObjectList:value(thisId);
	
	--M.super.removeObjectByThisId(self, thisId);
	self.mObjectList:remove(thisId);
	
	-- 查找是否还存在这种类型的 Item 
	local index = 0;
	local listLen = self.mObjectList:count();
	local has = false;
	local findItem = nil;
	
	while(index < listLen) do
		findItem = self.mObjectList:get(index);
		
		if(findItem:getSkinId() == item:getSkinId()) then
			has = true;
			break;
		end
		
		index = index + 1;
	end
	
	if(not has) then
		if(self.mAllSkinType2ListDic:ContainsKey(item:getSkinId())) then
			self.mAllSkinType2ListDic:remove(item:getSkinId());
		end
		
		if(self.mSkinTypeList:ContainsKey(item:getSkinId())) then
			self.mSkinTypeList:remove(item:getSkinId());
			self.mRemoveOneTypeItemDispatch:dispatchEvent(item);
		end
	end
	
	local list = nil;
	
	if(self.mSkinType2ListDic:ContainsKey(item:getSkinId())) then
		list = self.mSkinType2ListDic:value(item:getSkinId());
	end
	
	if(nil ~= list) then
		list:remove(thisId);
		
		if(0 == list:count()) then
			self.mSkinType2ListDic:remove(item:getSkinId());
		end
	end
	
	self.mRemoveOneItemDispatch:dispatchEvent(item);
	
	if(item ~= nil) then
		item:dispose();
		item = nil;
	end
end

--通过 SkinId 和 Skin 表中的 Id 判断是否激活
function M:isActiveBySkinIdAndBaseId(skinId, baseId)
	local isActive = false;
	
	if(self.mSkinType2ListDic:ContainsKey(skinId)) then
		isActive = self.mSkinType2ListDic:value(skinId):ContainsKey(baseId);
	end
	
	return isActive;
end

return M;