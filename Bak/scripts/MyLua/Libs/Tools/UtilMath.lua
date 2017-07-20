local M = GlobalNS.StaticClass();
M.clsName = "UtilMath";
GlobalNS[M.clsName] = M;

M.MaxNum = 100000;
M.InvalidIndex = -1;

M.DAY = { [1] = 31, 
		  [2] = 28, 
		  [3] = 31, 
		  [4] = 30, 
		  [5] = 31, 
		  [6] = 30, 
		  [7] = 31, 
		  [8] = 31, 
		  [9] = 30, 
		  [10] = 31, 
		  [11] = 30, 
		  [12] = 31 
		  };

function M.floor(value)
	return math.floor(value);
end

function M.ceil(value)
    return math.ceil(value);
end

-- 取余
function M.mod(a, b)
	local ret = 0;
	ret = math.mod(a, b);
	return ret;
end

--取整
function M.integer(a)
	local ret = 0;
	ret = a - M.mod(a, 1);
	return ret;
end

--取小数
function M.fract(a)
	local ret = 0;
	ret = M.mod(a, 1);
	return ret;
end

--精确到小数点后几位
function M.integerWithFract(a, fractNum)
	local ret = 0;
	local modValue = 1 / (10 * fractNum);
	ret = a - M.mod(a, modValue);
	return ret;
end

function M.getMassByRadius(radius)
    return math.pow(radius, GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mRealMassFactor);
end

function M.getRadiusByMass(mass)
    return math.pow(mass, 1/GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mRealMassFactor);
end

function M.keepTwoDecimalPlaces(decimal)
    --保留到小数点后两位，不四舍五入。 eg 12.999999 -> 12.99
    local ret = decimal - decimal % 0.01;
    return ret;
    --[[
    decimal = decimal * 100;
    if decimal % 1 >= 0.5 then 
        decimal=math.ceil(decimal);
    else
        decimal=math.floor(decimal);
    end
    return  decimal * 0.01;
    ]]--
end

--客户端显示质量
function M.getShowMass(radius)
    local showmass = "1毫克";
    local k = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mMassFactor;

    local _yt = math.pow((radius / math.pow(1000 * 1000 * 1000 * 10000 * 10000, 1/k)), k); --亿t
    if _yt >= 1 then
        showmass = string.format("%0.1f亿吨", _yt);
        return showmass;
    end

    local _wt = math.pow((radius / math.pow(1000 * 1000 * 1000 * 10000, 1/k)), k); --万t
    if _wt >= 1 then
        showmass = string.format("%0.1f万吨", _wt);
        return showmass;
    end

    local _t = math.pow((radius / math.pow(1000 * 1000 * 1000, 1/k)), k); --t
    if _t >= 1 then
        showmass = string.format("%0.1f吨", _t);
        return showmass;
    end

    local _kg = math.pow((radius / math.pow(1000 * 1000, 1/k)), k); --kg
    if _kg >= 1 then
        showmass = string.format("%0.1f千克", _kg);
        return showmass;
    end

    local _g = math.pow((radius / math.pow(1000, 1/k)), k); --g
    if _g >= 1 then
        showmass = string.format("%0.1f克", _g);
        return showmass;
    end

    local _mg = math.pow((radius), k); --mg
    showmass = string.format("%0.1f毫克", _mg);

    return showmass;
end


-- 是否是闰年
function M.isLeapYear(year)
	local ret = false;

	if((0 == year % 4 and 0 ~= year % 100) or 
		(0 == year % 400)) then
		ret = true;
	end

	return ret;
end

-- 是否是平年
function M.isCommonYear(year)
	local ret = false;

	ret = not (M.isLeapYear(year));

	return ret;
end

-- 返回天数, month [1, 12]
function M.getNumOfDaysByYearAndMonth(year, month)
	local day = 30;

	if(2 == month) then
		if(M.isLeapYear(year)) then
			day = 29;
		else
			day = 28;
		end
	elseif(month <= 12) then
		day = M.DAY[month];
	end

	return day;
end

-- 获取当前月的天数
function M.getNumOfDaysCurrentMonth()
	local year = GlobalNS.UtilApi.getYear();
	local month = GlobalNS.UtilApi.getMonth();
	
	local ret = 30;
	ret = M.getNumOfDaysByYearAndMonth(year, month);
	return ret;
end

-- 提取符号, 小于 0 就是 -1， 大于 0 就是 1， 等于 0 就是 0
function M.sign(value)
	local ret = 0;
	
	if(value > 0) then
		ret = 1;
	elseif(value < 0) then
		ret = -1;
	else
		ret = 0;
	end
	
	return ret;
end

function M.abs(value)
	local ret = value;
	
	if(value < 0) then
		ret = -value;
	end
	
	return ret;
end

return M;