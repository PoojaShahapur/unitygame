MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIShop_SkinPanel.Shop_SkinPanelNS");

--数据区
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "Shop_SkinPanelData";
GlobalNS.Shop_SkinPanelNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

return M;