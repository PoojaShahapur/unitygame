MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.UI.UICore.ComponentStyle.WidgetStyle");

local M = GlobalNS.Class(GlobalNS.WidgetStyle);
M.clsName = "ButtonStyleBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

return M;