MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIItemsCirclePanel.ItemsCirclePanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ItemsCirclePanelPath";
GlobalNS.ItemsCirclePanelNS[M.clsName] = M;

function M.init()
	M.ItemsName = "BG/ItemsCircle/Itemsname/Text";
	M.ItemsPrice = "BG/ItemsCircle/Itemsprice/Text_jiazhi";
	M.ItemsPriceBtm = "BG/ItemsCircle/items_jiaqian";
	M.ItemsImage = "BG/ItemsCircle/ItemsImage";
	M.ItemsActiveBtn = "BG/ItemsCircle/Items_Btn";
	M.ItemsTopPriceImage = "BG/ItemsCircle/Itemsprice/money";
	M.ItemsBtmPriceImage = "BG/ItemsCircle/items_jiaqian/Image";
	M.ItemsShuoMing = "BG/ItemsCircle/Items_shuoming";
	M.ItemsHowGetBtn = "BG/ItemsCircle/Howget_Btn";
	M.ItemsJiaGeImage = "BG/ItemsCircle/Items_jiage";	-- 价格背景
	
	M.CloseBtn = "BG";
end

M.init();

return M;