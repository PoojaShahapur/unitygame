MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIAccountPanel.AccountPanelCV");

local M = GlobalNS.Class(GlobalNS.ItemViewBase);
M.clsName = "GameShowViewItem";
GlobalNS.AccountPanelNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	self.mGuiWin = nil;
	
	M.super.dispose(self);
end

return M;