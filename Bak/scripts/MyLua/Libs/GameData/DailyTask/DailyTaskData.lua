MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Config.DailyTaskConfig");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "DailyTaskData";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mTaskList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mActivityList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mActivityValue = GlobalNS.new(GlobalNS.AuxIntProperty);	-- 活跃度值
	self.mActivityValue:setData(0);
end

function M:dtor()
	
end

function M:init()
	local dailyTaskConfig = LuaConfig.DailyTaskConfig["DailyTask"];
	local taskNum = GlobalNS.UtilApi.getTableLen(dailyTaskConfig);
	local index = 0;
	local taskItem = nil;
	
	while(index < taskNum) do
		taskItem = GlobalNS.new(GlobalNS.DailyTaskItemData);
		self.mTaskList:add(index, taskItem);
		taskItem:setTaskUniqueId(index);
		taskItem:setTaskItemConfig(dailyTaskConfig[index + 1]);
		taskItem:init();
		
		index = index + 1;
	end
	
	-- 活跃度奖励
	index = 0;
	local activityConfig = LuaConfig.DailyTaskConfig["ActivityReward"];
	local activityItem = nil;
	local activityCount = GlobalNS.UtilApi.getTableLen(activityConfig);
	
	while(index < activityCount) do
		activityItem = GlobalNS.new(GlobalNS.ActivityRewardItemData);
		self.mActivityList:add(index, activityItem);
		activityItem:setUniqueId(index);
		activityItem:setItemConfig(activityConfig[index + 1]);
		activityItem:init();
		
		index = index + 1;
	end
end

function M:dispose()
	local dailyTaskConfig = LuaConfig.DailyTaskConfig["DailyTask"];
	local taskNum = GlobalNS.UtilApi.getTableLen(dailyTaskConfig);
	local index = 0;
	local taskItem = nil;
	
	while(index < taskNum) do
		taskItem = self.mTaskList:get(index);
		taskItem:dispose();
		
		index = index + 1;
	end
	
	local activityConfig = LuaConfig.DailyTaskConfig["ActivityReward"];
	local activityCount = GlobalNS.UtilApi.getTableLen(activityConfig);
	index = 0;
	local activityItem = nil;
	
	while(index < activityCount) do
		activityItem = self.mActivityList:get(index);
		activityItem:dispose();
		
		index = index + 1;
	end
	
	if(nil ~= self.mActivityValue) then
		self.mActivityValue:dispose();
		self.mActivityValue = nil;
	end
end

function M:getTaskItemDataByTaskId(taskId)
	local item = nil;
	item = self.mTaskList:value(taskId);
	return item;
end

function M:getTaskCount()
	local ret = 0;
	ret = self.mTaskList:count();
	return ret;
end

function M:getActivityItemDataByActivityId(activityId)
	local item = nil;
	item = self.mActivityList:value(activityId);
	return item;
end

function M:getActivityCount()
	local ret = 0;
	ret = self.mActivityList:count();
	return ret;
end

function M:getActivityValue()
	return self.mActivityValue:getData();
end

function M:setActivityValue(value)
	self.mActivityValue:setData(value);
end

function M:addActivityChangeEventHandle(pThis, handle)
	self.mActivityValue:addEventHandle(pThis, handle);
end

function M:removeActivityChangeEventHandle(pThis, handle)
	self.mActivityValue:removeEventHandle(pThis, handle);
end

function M:getSliderValue()
	local total = self.mActivityList:get(GlobalNS.ActivityRewardTypeId.eActivityReward_4):getActivityValue();
	local ret = self.mActivityValue:getData() / total;
	return ret;
end

function M:getLightStarId()
	local miscTaskConfig = LuaConfig.DailyTaskConfig["Misc"];
	return miscTaskConfig["LightStar"];	
end

function M:getDarkStarId()
	local miscTaskConfig = LuaConfig.DailyTaskConfig["Misc"];
	return miscTaskConfig["DarkStar"];
end

return M;