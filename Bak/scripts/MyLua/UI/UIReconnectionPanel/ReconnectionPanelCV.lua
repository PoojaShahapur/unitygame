MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIReconnectionPanel.ReconnectionPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ReconnectionPanelPath";
GlobalNS.ReconnectionPanelNS[M.clsName] = M;

function M.init()
	--M.BtnRelive = "Relive_BtnTouch";
end

M.init();

return M;