MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIMyskinPanel.MyskinPanelNS");

--数据区
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MyskinPanelData";
GlobalNS.MyskinPanelNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

return M;