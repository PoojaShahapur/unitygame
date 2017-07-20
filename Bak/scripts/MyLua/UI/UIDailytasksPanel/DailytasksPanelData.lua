MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIDailytasksPanel.DailytasksPanelNS");

--数据区
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "DailytasksPanelData";
GlobalNS.DailytasksPanelNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

return M;