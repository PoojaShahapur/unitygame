MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "DailyTaskItemData";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mTaskUniqueId = 0;
	self.mTaskItemConfig = nil;
	
	self.mAlreadyFinishedCount = GlobalNS.new(GlobalNS.AuxIntProperty); 	-- 已经完成次数
	self.mAlreadyFinishedCount:setData(0);
	
	self.mIsReceiveTask = GlobalNS.new(GlobalNS.AuxBoolProperty);
	self.mIsReceiveTask:setData(false);
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
end

function M:addFinishCountChangeHandle(pThis, handle)
	self.mAlreadyFinishedCount:addEventHandle(pThis, handle);
end

function M:removeFinishCountChangeHandle(pThis, handle)
	self.mAlreadyFinishedCount:removeEventHandle(pThis, handle);
end

function M:addIsReceiveTaskChangeHandle(pThis, handle)
	self.mIsReceiveTask:addEventHandle(pThis, handle);
end

function M:removeIsReceiveTaskChangeHandle(pThis, handle)
	self.mIsReceiveTask:removeEventHandle(pThis, handle);
end

function M:setTaskUniqueId(value)
	self.mTaskUniqueId = value;
end

function M:setTaskItemConfig(value)
	self.mTaskItemConfig = value;
end

function M:getTaskName()
	return self.mTaskItemConfig["TaskName"];
end

function M:getAddActivityValue()
	return self.mTaskItemConfig["AddActivityValue"];
end

function M:getTaskDesc()
	return self.mTaskItemConfig["TaskDesc"];
end

function M:getNeedFinishCount()
	return self.mTaskItemConfig["FinishCount"];
end

function M:getTaskRewardCount()
	return GlobalNS.UtilApi.getTableLen(self.mTaskItemConfig["TaskReward"]);
end

--索引从 0 开始
function M:getTaskRewardObjectIdById(id)
	return self.mTaskItemConfig["TaskReward"][id + 1]["ObjectId"];
end

function M:getTaskRewardObjectNumById(id)
	return self.mTaskItemConfig["TaskReward"][id + 1]["ObjectNum"];
end

function M:isFinishedTask()
	return self:getFinishCount() == self:getNeedFinishCount();
end

function M:getFinishCount()
	return self.mAlreadyFinishedCount:getData();
end

function M:setFinishCount(value)
	self.mAlreadyFinishedCount:setData(value);
end

function M:getIsReceiveTask()
	return self.mIsReceiveTask:getData();
end

function M:setIsReceiveTask(value)
	self.mIsReceiveTask:setData(value);
end

function M:getTaskImageId()
	return self.mTaskItemConfig["TaskImage"];
end

return M;