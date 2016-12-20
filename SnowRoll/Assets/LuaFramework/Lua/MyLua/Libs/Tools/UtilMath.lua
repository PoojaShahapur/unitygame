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

return M;