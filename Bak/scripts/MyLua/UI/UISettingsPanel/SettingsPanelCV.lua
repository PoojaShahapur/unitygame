MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UISettingsPanel.SettingsPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "SettingsPanelPath";
GlobalNS.SettingsPanelNS[M.clsName] = M;

function M.init()
	M.CloseBtn = "Close_BtnTouch";
end

M.init();

return M;