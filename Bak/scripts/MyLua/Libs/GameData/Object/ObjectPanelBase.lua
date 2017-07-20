MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
@brief ObjectPanelBase
]]

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ObjectPanelBase";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mPanelType = 0;
	self.mObjectList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mObjectList:setIsSpeedUpFind(true);
	self.mObjectList:setIsOpKeepSort(true);
	
	self.mAddOneItemDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
	self.mRemoveOneItemDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
	self.mUpdateOneItemDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
	
	self.mIsAddItemProperty = GlobalNS.new(GlobalNS.AuxBoolProperty);
end

function M:dtor()
	
end

function M:init()
	self.mIsAddItemProperty:init();
end

function M:dispose()
	self.mAddOneItemDispatch:clearEventHandle();
	self.mAddOneItemDispatch = nil;
	self.mRemoveOneItemDispatch:clearEventHandle();
	self.mRemoveOneItemDispatch = nil;
	self.mUpdateOneItemDispatch:clearEventHandle();
	self.mUpdateOneItemDispatch = nil;
	self.mIsAddItemDispatch:clearEventHandle();
	self.mIsAddItemDispatch = nil;
	
	self.mIsAddItemProperty:dispose();
	self.mIsAddItemProperty = nil;
end

function M:clear()
	self.mObjectList:clear();
	self.mAddOneItemDispatch:clearEventHandle();
	self.mRemoveOneItemDispatch:clearEventHandle();
	self.mUpdateOneItemDispatch:clearEventHandle();
	self.mIsAddItemProperty:reset();
end

function M:getIsAddItemProperty()
	return self.mIsAddItemProperty;
end

function M:getAddOneItemDispatch()
	return self.mAddOneItemDispatch;
end

function M:getRemoveOneItemDispatch()
	return self.mRemoveOneItemDispatch;
end

function M:getUpdateOneItemDispatch()
	return self.mUpdateOneItemDispatch;
end

function M:getIsAddItemDispatch()
	return self.mIsAddItemDispatch;
end

function M:setPanelType(value)
	self.mPanelType = value;
end

function M:getItemCount()
    return self.mObjectList:count();
end

function M:getItemByIndex(index)
    local ret = nil;
    ret = self.mObjectList:get(index);
    return ret;
end

function M:addObjectById(baseId, thisId, item)
	self.mObjectList:add(thisId, item);
	
	self.mAddOneItemDispatch:dispatchEvent(item);
	self.mIsAddItemProperty:setData(true);
end

function M:removeObjectByThisId(thisId)
	local removeItem = self.mObjectList:value(thisId);
	self.mObjectList:remove(thisId);
	
	self.mRemoveOneItemDispatch:dispatchEvent(removeItem);
end

function M:getObjectByThisId(thisId)
	return self.mObjectList:value(thisId);
end

return M;