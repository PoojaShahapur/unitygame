MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UtilStr";
GlobalNS[M.clsName] = M;

function M.IsNullOrEmpty(str)
	return (nil == str or "" == str);
end

return M;