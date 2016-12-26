MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIRockerPanel.RockerPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "RockerPanelPath";
GlobalNS.RockerPanelNS[M.clsName] = M;

function M.init()
	M.BtnRocker = "Rocker_BtnTouch";
end

M.init();

return M;