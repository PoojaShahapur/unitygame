local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UtilMath";
GlobalNS[M.clsName] = M;

function M.floor(value)
	return math.floor(value);
end

return M;