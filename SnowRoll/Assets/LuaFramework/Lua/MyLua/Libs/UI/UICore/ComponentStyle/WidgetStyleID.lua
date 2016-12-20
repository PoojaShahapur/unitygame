MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "WidgetStyleID";
GlobalNS[M.clsName] = M;

function M.ctor()
    M.eWSID_Button = 0;
    M.eWSID_Text = 1;
    M.eTotal = 2;
end

M.ctor();

return M;