--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "SignData";
GlobalNS[M.clsName] = M;

function M:ctor(...)
	self.mYear = 0;		--计算的年
	self.mMonth = 0;	--计算的月
	self.mDay = 0;		--计算的天
	self.mIsSignedToday = false;		-- 今天是否签到
	self.mMonthSignInfoKey = "";
	self.mMonthSignInfoDic = GlobalNS.new(GlobalNS.MDictionary);
end

function M:dtor()
    
end

function M:clear()
	
end

-- 计算当前月的天数, deltaMonth 与当前偏差的月， 向前就是负值，向后就是正值
function M:calcDate(deltaDay)
	self.mYear = 0;
	self.mMonth = 0;
	self.mDay = 0;
	
	self.mYear, self.mMonth, self.mDay = GlobalNS.UtilLogic.getDateByOffsetDay(deltaDay);
	
	self.mMonthSignInfoKey = "" .. self.mYear .. "-" .. self.mMonth;
	self.daysCount = GlobalNS.UtilMath.getNumOfDaysByYearAndMonth(self.mYear, self.mMonth);
end

function M:getYear()
	return self.mYear;
end

function M:getMonth()
	return self.mMonth;
end

function M:getDay()
	return self.mDay;
end

function M:setBtnState(index)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUISignPanel);
    if nil ~= form and form.mIsReady then
        form:enableBtn(index);
    end
end

function M:isSignedToday()
	return self.mIsSignedToday;
end

function M:setIsSignedToday(value)
	self.mIsSignedToday = value;
end

-- 获取连续签到天数
function M:getContinueDays()
	local ret = 0;
	local monthInfo = self.mMonthSignInfoDic:value(self.mMonthSignInfoKey);
	
	if(nil ~= monthInfo) then
		ret = monthInfo:getContinueDays();
	end
	
	return ret;
end

function M:isCumulaRewardsDrawById(id)
	local ret = false;
	local monthInfo = self.mMonthSignInfoDic:value(self.mMonthSignInfoKey);
	
	if(nil ~= monthInfo) then
		ret = monthInfo:isCumulaRewardsDrawById(id);
	end
	
	return ret;
end

function M:getSignedNumOfDaysInMonth()
	local ret = 0;
	local monthInfo = self.mMonthSignInfoDic:value(self.mMonthSignInfoKey);
	
	if(nil ~= monthInfo) then
		ret = monthInfo:getSignedNumOfDaysInMonth();
	end
	
	return ret;
end

-- 设置每日签到列表和添加领取累积奖励
function M:setDailySigninAndReceiveCumulaReward(signinCount, signinList, receivecount, cumularewardlist)
	local year = GlobalNS.UtilApi.getYear();
	local month = GlobalNS.UtilApi.getMonth();
	self.mMonthSignInfoKey = "" .. year .. "-" .. month;
	local monthSignIn = nil;
	
	if(self.mMonthSignInfoDic:ContainsKey(self.mMonthSignInfoKey)) then
		monthSignIn = self.mMonthSignInfoDic:value(self.mMonthSignInfoKey);
	else
		monthSignIn = GlobalNS.new(GlobalNS.MonthSignIn);
		self.mMonthSignInfoDic:add(self.mMonthSignInfoKey, monthSignIn);
	end
	
	monthSignIn:clear();
	
	local index = 0;
	local listLen = signinCount;
	
	while(index < listLen) do
		monthSignIn:addDailySignin(signinList[index]);
		index = index + 1;
	end
	
	index = 0;
	listLen = receivecount;
	
	while(index < listLen) do
		monthSignIn:addReceiveCumulaReward(cumularewardlist[index]);
		index = index + 1;
	end
end

return M;
--endregion
