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
end

function M:init()
	
end

function M:dispose()
	
end

--增加数量
function M:incNum()
	self.mTotalNum = self.mTotalNum + 1;
	
	--[[
	-- 如果第一次添加，需要通知，以后就不用再通知了
	if(1 == self.mTotalNum) then
		GlobalNS.CSSystem.setNeedUpdateFromExternal(true);
	end
	]]
	
	self:checkUpdate();
end

--减少数量
function M:decNum()
	self.mTotalNum = self.mTotalNum - 1;
	
	if(self.mTotalNum < 0) then
		self.mTotalNum = 0;
	end
	
	--[[
	-- 说明错误增加和减少不对称
	if(self.mTotalNum < 0) then
		--GlobalNS.UtilApi.error("FrameUpdateStatistics decNum less zero");
		self.mTotalNum = 0;
	end
	
	if(0 == self.mTotalNum) then
		GlobalNS.CSSystem.setNeedUpdateFromExternal(false);
	end
	]]
	
	self:checkUpdate();
end

function M:checkUpdate()
	local updateNum = 0;
	updateNum = GCtx.mTimerMgr:getCount();
	
	if(0 == updateNum) then
		if(self.mIsEnableUpdate) then
			self.mIsEnableUpdate = false;
			GlobalNS.CSSystem.setNeedUpdateFromExternal(false);
		end
	else
		if(not self.mIsEnableUpdate) then
			self.mIsEnableUpdate = true;
			GlobalNS.CSSystem.setNeedUpdateFromExternal(true);
		end
	end
end

return M;