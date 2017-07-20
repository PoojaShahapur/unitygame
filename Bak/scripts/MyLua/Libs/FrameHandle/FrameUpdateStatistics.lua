MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.DataStruct.MList");
MLoader("MyLua.Libs.DelayHandle.DelayPriorityHandleMgrBase");

--[[
	@brief 帧调用，定时器调用统计信息
]]
local M = GlobalNS.Class(GlobalNS.DelayPriorityHandleMgrBase);
M.clsName = "FrameUpdateStatistics";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mTotalNum = 0;
	self.mIsEnableUpdate = false; 	-- 是否开启更新
	self.mNeedUpdateList = GlobalNS.new(GlobalNS.MList);
	
	local index = 0;
	while(index < GlobalNS.FrameUpdateStatisticsTypeId.eFUST_Count) do
		self.mNeedUpdateList:add(false);
		index = index + 1;
	end
end

function M:init()
	
end

function M:dispose()
	
end

function M:hasNeedUpdate()
	local ret = false;
	
	local index = 0;
	while(index < GlobalNS.FrameUpdateStatisticsTypeId.eFUST_Count) do
		if(self.mNeedUpdateList:get(index)) then
			ret = true;
			break;
		end
		
		index = index + 1;
	end
end

function M:setNeedUpdateByTypeId(typeId, value)
	if(typeId < GlobalNS.FrameUpdateStatisticsTypeId.eFUST_Count) then
		self.mNeedUpdateList:set(typeId, value);
		
		if(self:hasNeedUpdate()) then
			if(not self.mIsEnableUpdate) then
				self.mIsEnableUpdate = true;
				GlobalNS.CSSystem.setNeedUpdateFromExternal(true);
			end
		else
			if(self.mIsEnableUpdate) then
				self.mIsEnableUpdate = false;
				GlobalNS.CSSystem.setNeedUpdateFromExternal(false);
			end
		end
	end
end

return M;