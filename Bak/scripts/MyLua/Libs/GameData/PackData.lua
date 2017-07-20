MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.GameData.Object.ObjectType");
MLoader("MyLua.Libs.GameData.Object.ObjectItem");
MLoader("MyLua.Libs.GameData.Object.ObjectPanelType");
MLoader("MyLua.Libs.GameData.Object.ObjectPanel");

--[[
@brief PackData
]]

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "PackData";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mObjectList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mObjectList:setIsSpeedUpFind(true);
	self.mObjectList:setIsOpKeepSort(true);
	
	self.mPanelArray = GlobalNS.new(GlobalNS.MList);
	
	local index = 0;
	local objectPanel = nil;
	
	while(index < GlobalNS.ObjectPanelType.eCount) do
		objectPanel = GlobalNS.new(GlobalNS.ObjectPanel);
		objectPanel:setPanelType(index + 1);
		self.mPanelArray:add(objectPanel);
		objectPanel:init();
		
		index = index + 1;
	end
	
	self.mRedPointProperty = GlobalNS.new(GlobalNS.AuxBoolProperty);
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
end

function M:clear()
	self.mObjectList:clear();
end

function M:getRedPointProperty()
	return self.mRedPointProperty;
end

function M:getObjectByThisId(thisId)
    return self.mObjectList:value(thisId);
end

function M:addObjectById(baseId, thisId)
	local item = nil;

	if (not self.mObjectList:ContainsKey(thisId)) then
		item = GlobalNS.new(GlobalNS.ObjectItem);

		self.mObjectList:add(thisId, item);
	else
		item = self.mObjectList:value(thisId);
	end
	
	self.mRedPointProperty:setData(true);

	return item;
end

function M:removeObjectByThisId(thisId)
	if (self.mObjectList:ContainsKey(thisId)) then
		local objItem = self.mObjectList:value(thisId);
		local panelType = objItem:getPanelTypeZeroIndex();
		
		self.mObjectList:remove(thisId);
		
		if(GlobalNS.UtilMath.MaxNum ~= panelType) then
			self.mPanelArray:get(panelType):removeObjectByThisId(objItem:getStrThisId());
		end
	end
end

-- Native ObjectItem
function M:addObjectByNativeItem(obj)
	local item = self:addObjectById(obj:getBaseId(), obj:getStrThisId());
	item:setNativeItem(obj);
	
	local panelType = obj:getPanelTypeZeroIndex();
	-- test
	--panelType = 1;
	if(GlobalNS.UtilMath.MaxNum ~= panelType) then
		self.mPanelArray:get(panelType):addObjectById(obj:getBaseId(), obj:getStrThisId(), item);
	end
end

function M:removeObjectByItem(obj)
	self.removeObjectByThisId(obj:getThisId());
end

function M:getPanelDataByIndex(index)
	local objectPanel = self.mPanelArray:get(index);
	
	return objectPanel;
end

return M;