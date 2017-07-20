MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.UI.UIPerformancePanel.PerformancePanelNS");
MLoader("MyLua.UI.UIPerformancePanel.PerformancePanelData");
MLoader("MyLua.UI.UIPerformancePanel.PerformancePanelCV");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "PerformViewItem";
GlobalNS.PerformancePanelNS[M.clsName] = M;

function M:ctor()
	self.mObjectImage = GlobalNS.new(GlobalNS.AuxObjectImage);
	self.mObjectImage:setIsDestroySelf(false);
	self.mObjectNumText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mObjectNumText:setIsDestroySelf(false);
	
	self.mObjectImageNodePath = "";
	self.mObjectNumNodePath = "";
	self.mGuiWin = nil;
	
	self.mObjectId = 0;
	self.mObjectNum = 0;
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	if(nil ~= self.mObjectImage) then
		self.mObjectImage:dispose();
		self.mObjectImage = nil;
	end
	if(nil ~= self.mObjectNumText) then
		self.mObjectNumText:dispose();
		self.mObjectNumText = nil;
	end
	
	self.mGuiWin = nil;
end

function M:setObjectImageNodePath(value)
	self.mObjectImageNodePath = value;
end

function M:setObjectNumNodePath(value)
	self.mObjectNumNodePath = value;
end

function M:setGuiWin(value)
	self.mGuiWin = value;
	
	self.mObjectImage:setSelfGoByPath(self.mGuiWin, self.mObjectImageNodePath);
	self.mObjectNumText:setSelfGoByPath(self.mGuiWin, self.mObjectNumNodePath);
end

function M:setItemData()
	
end

function M:setObjectId(value)
	self.mObjectId = value;
	self.mObjectImage:setObjectId(self.mObjectId);
end

function M:setObjectNum(value)
	self.mObjectNum = value;
	self.mObjectNumText:setText('' .. self.mObjectNum);
end

return M;