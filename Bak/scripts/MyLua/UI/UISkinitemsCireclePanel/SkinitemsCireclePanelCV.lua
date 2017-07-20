MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UISkinitemsCireclePanel.SkinitemsCireclePanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "SkinitemsCireclePanelPath";
GlobalNS.SkinitemsCireclePanelNS[M.clsName] = M;

function M.init()
	--M.BtnRelive = "Relive_BtnTouch";
end

M.init();

return M;