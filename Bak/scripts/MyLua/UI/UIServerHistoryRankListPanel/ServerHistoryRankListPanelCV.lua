MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIServerHistoryRankListPanel.ServerHistoryRankListPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ServerHistoryRankListPanelPath";
GlobalNS.ServerHistoryRankListPanelNS[M.clsName] = M;

function M.init()
	M.BtnBackGame = "BackGame_BtnTouch";
end

M.init();

return M;