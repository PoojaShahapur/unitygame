MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIItemsColourPanel.ItemsColourPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ItemsColourPanelPath";
GlobalNS.ItemsColourPanelNS[M.clsName] = M;

function M.init()
	M.CloseBtn = "Image";
	M.ClosePanel = "BG";
	
	M.LeftNameText = "item_pack/itemname_Image/item_name";
	M.LeftPriceText = "item_pack/itempricenum_Image/item_pricenum";
	M.LeftModelImage = "item_pack/item_Image";
	M.LeftRootGo = "item_pack";
	M.LeftMoneyImage = "item_pack/item_pricenum/item_priceimage";
	
	M.RightPanelContent = "ScrollRectPiFu/Viewport/Content";
	M.RightNameText = "item_button/Text";
	M.RightMoneyImage = "item_button/Image";
	M.RightModelImage = "item_plane";
	M.RightUseBtn = "item_button";
end

M.init();

return M;