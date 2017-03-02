MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "EEndian";
GlobalNS[M.clsName] = M;

M.eBIG_ENDIAN = 0;         -- 大端
M.eLITTLE_ENDIAN = 1;      -- 小端

return M;