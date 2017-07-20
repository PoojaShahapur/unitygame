MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIAccountPanel.AccountPanelCV");
MLoader("MyLua.UI.UIAccountPanel.Panel.GameShowAreaViewItem");
MLoader("MyLua.UI.UIAccountPanel.Panel.GameShowViewItem");

local M = GlobalNS.Class(GlobalNS.TabPage);
M.clsName = "GameShowPanel";
GlobalNS.AccountPanelNS[M.clsName] = M;

function M:ctor()
	self.mGuiWin = nil;
	self.mAreaWidget = GlobalNS.new(GlobalNS.AreaWidget);
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

function M:setGuiWin(value)
	self.mGuiWin = value;
	
	self:initAreaWidget();
end

function M:initAreaWidget()
	local areaView = nil;
	
	areaView = self.mAreaWidget:addAreaViewItemByGoAndPath(self.mGuiWin, GlobalNS.AccountPanelNS.AccountPanelPath.SkinContent, GlobalNS.AccountPanelNS.GameShowAreaViewItem);
	areaView:setTag(0);
	
	areaView = self.mAreaWidget:addAreaViewItemByGoAndPath(self.mGuiWin, GlobalNS.AccountPanelNS.AccountPanelPath.BulletContent, GlobalNS.AccountPanelNS.GameShowAreaViewItem);
	areaView:setTag(1);
end

return M;