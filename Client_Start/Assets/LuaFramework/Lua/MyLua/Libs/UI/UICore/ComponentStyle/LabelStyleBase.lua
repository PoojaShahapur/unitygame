require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.UI.UICore.ComponentStyle.WidgetStyle"

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