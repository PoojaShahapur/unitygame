MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIShop_SkinPanel.Shop_SkinPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "Shop_SkinPanelPath";
GlobalNS.Shop_SkinPanelNS[M.clsName] = M;

function M.init()
end

M.init();

return M;