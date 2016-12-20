MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.UI.UICore.ComponentStyle.WidgetStyle");

local M = GlobalNS.Class(GlobalNS.WidgetStyle);
local this = M;
M.clsName = "LabelStyleBase";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:needClearText()
    return true;
end

return M;