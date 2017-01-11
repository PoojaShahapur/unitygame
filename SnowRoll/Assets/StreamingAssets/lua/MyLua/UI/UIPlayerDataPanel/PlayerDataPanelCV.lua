MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIPlayerDataPanel.PlayerDataPanelNS");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "PlayerDataPanelPath";
GlobalNS.PlayerDataPanelNS[M.clsName] = M;

function M.init()
end

M.init();

return M;