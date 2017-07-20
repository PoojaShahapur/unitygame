MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

-- Path 区域
local M = GlobalNS.StaticClass();
M.clsName = "ObjectViewCV";
GlobalNS[M.clsName] = M;

M.ObjectItemImage = "item_Image";     -- 图像
M.ObjectItemName = "item_name";       -- 名字
M.ObjectItemPriceNum = "item_pricenum";  -- 价格
M.ObjectItemNum = "Text";    -- 数量
M.ObjectItemMoneyImage = "item_priceimage";	-- 钱图标

M.ShopItemImage = "GoodsImage";     -- 图像
M.ShopItemName = "GoodsName";       -- 名字
M.ShopItemPriceNum = "Goods_Price";  -- 价格
M.ShopItemPriceBtmNum = "BuyItem_BtnTouch/Num";  -- 价格
M.ShopItemBuyBtn = "BuyItem_BtnTouch";	-- 购买按钮
M.ShopItemMoneyImage = "Goods_Image";	-- 钱图标
M.ShopItemMoneyBtnImage = "BuyItem_BtnTouch/Kind";	-- 钱图标
M.ShopItemAcquireActor = "OnlyImage";	-- 赛季结束获取奖励
M.ShopItemAcquireText = "OnlyImage/OnlyText";	-- 赛季结束获取奖励描述

M.SkinItemImage = "Image/item_Image";     -- 图像
M.SkinItemName = "Name_Image/item_name";       -- 名字
M.SkinItemPriceNum = "Price_Image/item_pricenum";  -- 价格
M.SkinItemNum = "Text";    -- 数量
M.SkinItemMoneyImage = "Price_Image/item_priceimage";	-- 钱图标
M.SkinInUsingImage = "Use_Image";

return M;