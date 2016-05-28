require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "MacroDef";
GlobalNS[M.clsName] = M;

function M.ctor()
	M.UNIT_TEST = true;
end

M.ctor();

return M;