local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UtilPath";
GlobalNS[M.clsName] = M;

function M.CombineTwo(aPath, bPath)
	aPath = string.gsub(aPath, "\\", "/");
	local retPath = string.format("%s/%s", aPath, bPath);
	retPath = string.gsub(retPath, "//", "/");
	return retPath;
end

return M;