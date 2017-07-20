MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "DataItemType";
GlobalNS[M.clsName] = M;

M.eObjectItem  = 0;    	-- 道具 Item
M.eShopItem = 1;    	-- 商店 Item
M.eSkinItem_PiFu  = 2;    	-- 皮肤 Item
M.eSkinItem_Bullet  = 3;    -- 子弹 Item
M.eSkinClientItem_PiFu = 4;  -- 皮肤客户端 Item
M.eSkinClientItem_Bullet = 5;  -- 皮肤子弹客户端 Item

return M;