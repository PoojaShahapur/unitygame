MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIItempiecePanel.ItempiecePanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ItempiecePanelPath";
GlobalNS.ItempiecePanelNS[M.clsName] = M;

function M.init()
	M.CloseBgPanel = "CloseBgPanel";
	M.ComposeBtn = "BG/Compose_Panel/Compose_Btnpanel";
	M.ObjectImage = "BG/Top_Panel/Item_Image";
	M.NameText = "BG/Top_Panel/Itemname_Text";
	M.DescText = "BG/Top_Panel/Itemexp_Text";
	M.AcquireDescText = "BG/Compose_Panel/Getway_Text/Itemexp_Text";
end

M.init();

return M;