MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIShareSelfPanel.ShareSelfPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ShareSelfPanelPath";
GlobalNS.ShareSelfPanelNS[M.clsName] = M;

function M.init()

end

M.init();

return M;