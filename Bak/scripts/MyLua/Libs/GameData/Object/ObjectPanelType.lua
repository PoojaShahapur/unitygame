MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

-- Panel 区域
local M = GlobalNS.StaticClass();
M.clsName = "ObjectPanelType";
GlobalNS[M.clsName] = M;

M.eModel  = 1;    -- 机型(皮肤)
M.eBullet = 2;    -- 子弹
M.eTrail  = 3;    -- 轨迹
M.eMoney  = 4;    -- 货币
M.eGift  = 5;    -- 礼包
M.ePiece   = 6;    -- 碎片
M.eDazzleLight  = 7;-- 炫光
M.eCount  = 8;    -- 总数

return M;