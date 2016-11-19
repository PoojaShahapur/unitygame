MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "GkEncode";
GlobalNS[M.clsName] = M;

M.UTF8 = 0;
--M.GB2312 = Encoding.GetEncoding(936);         -- GB2312 这个解码器在 mono 中是没有的，不能使用
M.GB2312 = 1;         -- GB2312
M.Unicode = 2;
M.Default = 3;

return M;