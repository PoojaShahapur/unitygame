local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UtilMath";
GlobalNS[M.clsName] = M;

function M.floor(value)
	return math.floor(value);
end

function M.ceil(value)
    return math.ceil(value);
end

function M.getMassByRadius(radius)
    return math.pow(radius, 3);
end

function M.getRadiusByMass(mass)
    return math.pow(mass, 1/3);
end

--客户端显示质量
function M.getShowMass(radius)
    local showmass = "1毫克";
    local k = 3.0;
    local _wt = k * math.pow((radius * 0.0001), 3.0) / 10; --万吨
    if _wt > 1 then
        showmass = string.format("%0.1f万吨", _wt);
        return showmass;
    end

    local _t = k * math.pow((radius * 0.001), 3.0); --吨
    if _t > 1 then
        showmass = string.format("%0.1f吨", _t);
        return showmass;
    end

    local _kg = k * math.pow((radius * 0.01), 3.0); --kg
    if _kg > 1 then
        showmass = string.format("%0.1f千克", _kg);
        return showmass;
    end

    local _g = k * math.pow((radius * 0.1), 3.0); --g
    if _g > 1 then
        showmass = string.format("%0.1f克", _g);
        return showmass;
    end

    local _mg = k * math.pow((radius), 3.0); --mg
    showmass = string.format("%0.1f毫克", _mg);

    return showmass;
end

return M;