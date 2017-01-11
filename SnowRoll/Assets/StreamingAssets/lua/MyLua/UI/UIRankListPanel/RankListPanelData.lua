MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIRankListPanel.RankListPanelNS");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "RankListPanelData";
GlobalNS.RankListPanelNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

return M;