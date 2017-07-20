MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIFirstPlayTipPanel.FirstPlayTipPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "FirstPlayTipPanelPath";
GlobalNS.FirstPlayTipPanelNS[M.clsName] = M;

function M.init()
end

M.init();

return M;