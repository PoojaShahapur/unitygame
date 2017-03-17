MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIRelivePanel.RelivePanelNS");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "RelivePanelData";
GlobalNS.RelivePanelNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

return M;