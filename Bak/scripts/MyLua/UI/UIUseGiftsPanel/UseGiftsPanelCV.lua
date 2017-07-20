MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIUseGiftsPanel.UseGiftsPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "UseGiftsPanelPath";
GlobalNS.UseGiftsPanelNS[M.clsName] = M;

function M.init()
	M.CloseBgPanel = "CloseBgPanel";
	M.ComposeBtn = "BG/Top_Panel/Compose_Btnpanel";
	M.ObjectImage = "BG/Top_Panel/Item_Image";
	M.NameText = "BG/Top_Panel/Itemname_Text";
	M.DescText = "BG/Top_Panel/Itemexp_Text";
end

M.init();

return M;