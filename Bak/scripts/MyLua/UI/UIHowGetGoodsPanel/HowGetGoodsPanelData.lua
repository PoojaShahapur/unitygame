MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIHowGetGoodsPanel.HowGetGoodsPanelNS");

--数据区
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "HowGetGoodsPanelData";
GlobalNS.HowGetGoodsPanelNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

return M;