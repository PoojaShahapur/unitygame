MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "GuideTypeId";
GlobalNS[M.clsName] = M;

M.eGTNull = 0;                -- 无定义
M.eGTJoyStickClick = 1;       -- 摇杆点击
M.eGTShoot = 2;				  -- 设计按钮点击
M.eGTSplit = 3;				  -- 分裂按钮点击
M.eGTEnd = 4;                 -- 结束

return M;