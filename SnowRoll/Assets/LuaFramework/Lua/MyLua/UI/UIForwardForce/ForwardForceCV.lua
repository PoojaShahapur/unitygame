MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIForwardForce.ForwardForceNS");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ForwardForcePath";
GlobalNS.ForwardForceNS[M.clsName] = M;

function M.init()
	--M.BtnRelive = "Relive_BtnTouch";
end

M.init();

return M;