MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIPlayerDataPanel.PlayerDataPanelNS");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "PlayerDataPanelData";
GlobalNS.PlayerDataPanelNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

return M;