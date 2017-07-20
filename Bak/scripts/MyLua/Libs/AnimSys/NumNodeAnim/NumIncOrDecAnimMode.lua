MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "NumIncOrDecAnimMode";
GlobalNS[M.clsName] = M;

M.eInc = 0;       -- 增加模式
M.eDec = 1;       -- 减少模式
M.ePingPong = 2;  -- 循环播放

return M;