MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UITopXRankPanel.TopXRankPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "TopXRankPanelPath";
GlobalNS.TopXRankPanelNS[M.clsName] = M;

function M.init()
    M.BtnDrop = "Drop_BtnTouch";
end

M.init();

return M;