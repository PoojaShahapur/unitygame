MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIItempricePanel.ItempricePanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ItempricePanelPath";
GlobalNS.ItempricePanelNS[M.clsName] = M;

function M.init()
	M.ModelImage = "BG/Top_Panel/Item_Image";
	M.NameText = "BG/Top_Panel/Itemname_Text";
	M.UsageDescText = "BG/Top_Panel/Itemexp_Text";
	M.AcquireDescText = "BG/Compose_Panel/Getway_Text/Itemexp_Text";
	
	M.CloseBtn = "BG";
end

M.init();

return M;