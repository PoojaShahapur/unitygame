require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

require "MyLua.UI.UITest.TestNS"

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "TestPath";
GlobalNS.TestNS[M.clsName] = M;

function M.init()
	M.BtnTest = "BtnTest";
end

M.init();

return M;