MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.Home);
M.clsName = "HomeOther";
GlobalNS[M.clsName] = M;

function M:ctor()

end