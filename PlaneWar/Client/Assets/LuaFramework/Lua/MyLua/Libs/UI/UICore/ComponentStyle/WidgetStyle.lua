MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
local this = M;
M.clsName = "WidgetStyle";
GlobalNS[M.clsName] = M;

function M:ctor()

end

return M;