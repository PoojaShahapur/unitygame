MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

-- ObjectType 区域
local M = GlobalNS.StaticClass();
M.clsName = "ObjectType";
GlobalNS[M.clsName] = M;

M.eModel  = 1;    -- 机型(皮肤)
M.eBullet = 2;    -- 子弹
M.eTrail  = 3;    -- 轨迹
M.eMoney  = 4;    -- 货币
M.eGift   = 5;    -- 礼包
M.eOther  = 6;    -- 普通道具
M.eDazzleLight = 7; -- 炫光
M.ePiece = 8; 	 	-- 碎片

M.eCount  = 8;     -- 总数

return M;