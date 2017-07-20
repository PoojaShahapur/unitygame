MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ActivityRewardItemData";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mUniqueId = 0;
	self.mItemConfig = nil;
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
end

function M:getUniqueId()
	return self.mUniqueId;
end

function M:setUniqueId(value)
	self.mUniqueId = value;
end

function M:setItemConfig(value)
	self.mItemConfig = value;
end

function M:getActivityValue()
	return GlobalNS.UtilApi.tonumber(self.mItemConfig["ActivityValue"]);
end

function M:getRewardObjectId()
	return GlobalNS.UtilApi.tonumber(self.mItemConfig["ObjectId"]);
end

function M:isCurActivityValueGreatEqualConfigValue()
	return GCtx.mPlayerData.mDailyTaskData:getActivityValue() >= self:getActivityValue();
end

return M;