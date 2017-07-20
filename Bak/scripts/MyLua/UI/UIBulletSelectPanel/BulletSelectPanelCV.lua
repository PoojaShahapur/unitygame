MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIBulletSelectPanel.BulletSelectPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "BulletSelectPanelPath";
GlobalNS.BulletSelectPanelNS[M.clsName] = M;

function M.init()
	M.NameText = "item_pack/itemname_Image/item_name";
	M.SkinImage = "item_pack/item_Image/Image";
	M.PriceText = "item_pack/item_pricenum";
	M.PriceImage = "item_pack/item_pricenum/item_priceimage";
	M.UseBtn = "item_pack/Use_Btn";
end

M.init();

return M;