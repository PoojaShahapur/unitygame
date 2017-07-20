MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "LangTypeId";
GlobalNS[M.clsName] = M;

M.eObject = 1;	-- 道具
M.eMessage = 2;	-- 消息

return M;