MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

-- Panel 区域
local M = GlobalNS.StaticClass();
M.clsName = "ShopTopType";
GlobalNS[M.clsName] = M;

M.ePiFu = 0;      -- 皮肤
M.eNiuDan = 1;    -- 扭蛋
M.eHuiYuan = 2;   -- 会员

return M;