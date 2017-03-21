MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

-- 当需要管理的对象可能在遍历中间添加的时候，需要这个管理器
local M = GlobalNS.StaticClass();
M.clsName = "TickMode";
GlobalNS[M.clsName] = M;

M.eTM_Update = 0;
M.eTM_FixedUpdate = 1;
M.eTM_LateUpdate = 2;

return M;