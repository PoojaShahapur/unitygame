MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MonthSignInfo";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mSignedDailyRewardsList = GlobalNS.new(GlobalNS.MList);		-- 一个月已经签到的天
	self.mSignedDailyRewardsList:setIsSpeedUpFind(true);
	self.mSignedDailyRewardsList:setIsOpKeepSort(true);
	
	self.mSignedCumulaRewardsList = GlobalNS.new(GlobalNS.MList);		-- 已经领取的累积签到 Id 列表
	self.mSignedCumulaRewardsList:setIsSpeedUpFind(true);
	self.mSignedCumulaRewardsList:setIsOpKeepSort(true);
end

function M:dtor()
	
end

function M:clear()
	self.mSignedCumulaRewardsList:clear();
	self.mSignedDailyRewardsList:clear();
end

-- 添加每日签到
function M:addDailySignin(day)
	if(not self.mSignedDailyRewardsList:ContainsKey(day)) then
		self.mSignedDailyRewardsList:add(day);
	end
end

-- 添加领取累积奖励, 这个 id 就是配置文件中的索引
function M:addReceiveCumulaReward(id)
	if(not self.mSignedCumulaRewardsList:ContainsKey(id)) then
		self.mSignedCumulaRewardsList:add(id);
	end
end

function M:getContinueDays()
	local ret = 0;
	
	local index = 0;
	local listLen = self.mSignedDailyRewardsList:count();
	
	local preDay = -1;
	local curDay = -1;
	local maxContinueDays = 0;
	local curContinueDays = 0;
	
	while(index < listLen) do
		preDay = curDay;
		curDay = self.mSignedDailyRewardsList:get(index);
		
		if(1 == curDay - preDay) then
			curContinueDays = curContinueDays + 1;
		else
			-- 至少应该是 1
			if(maxContinueDays < 1) then
				maxContinueDays = 1;
			end
			
			if(maxContinueDays < curContinueDays) then
				maxContinueDays = curContinueDays;
			end
			
			curContinueDays = 0;
		end 
			
		
		index = index + 1;
	end
	
	return ret;
end

function M:isCumulaRewardsDrawById(id)
	local ret = false;
	
	local index = 0;
	local listLen = self.mSignedCumulaRewardsList:count();
	
	while(index < listLen) do
		if(id == self.mSignedCumulaRewardsList:get(index)) then
			ret = true;
			break;
		end

		index = index + 1;
	end
	
	return ret;
end

function M:getSignedNumOfDaysInMonth()
	local ret = self.mSignedDailyRewardsList:count();
	return ret;
end

return M;