MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UITest.PackNS");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "PackPath";
GlobalNS.TestNS[M.clsName] = M;

function M.init()
	M.BtnTest = "BtnTest";
end

M.init();

return M;