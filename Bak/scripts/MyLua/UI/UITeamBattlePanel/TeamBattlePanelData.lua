MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UITeamBattlePanel.TeamBattlePanelNS");

--数据区
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TeamBattlePanelData";
GlobalNS.TeamBattlePanelNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

return M;