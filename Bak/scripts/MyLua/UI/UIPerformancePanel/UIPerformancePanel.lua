MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIPerformancePanel.PerformancePanelNS");
MLoader("MyLua.UI.UIPerformancePanel.PerformancePanelData");
MLoader("MyLua.UI.UIPerformancePanel.PerformancePanelCV");
MLoader("MyLua.UI.UIPerformancePanel.Panel.PerformViewItem");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIPerformancePanel";
GlobalNS.PerformancePanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIPerformancePanel;
	self.mData = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformancePanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	self.mObjectImageList = GlobalNS.new(GlobalNS.MList);
	
	self:setAwardInfo();
	self:setInfo();
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachWidget();
	self:addWidgetEventHandle();
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
end

function M:attachWidget()
	local index = 0;
	local listLen = self.mObjectImageList:count();
	local item = nil;
	
	while(index < listLen) do
		item = self.mObjectImageList:get(index);
		item:setGuiWin(self.mGuiWin);
		
		index = index + 1;
	end
end

function M:addWidgetEventHandle()
	self:attachClosePanel(GlobalNS.PerformancePanelNS.PerformancePanelPath.CloseBgPanel);
end

function M:setAwardInfo()
	local num = 5;
	
	local objectImage = nil;
	
	if(1 == num) then
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.OnePanelImage_0);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.OnePanelText_0);
	elseif(2 == num) then
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.TwoPanelImage_0);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.TwoPanelText_0);
		
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.TwoPanelImage_1);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.TwoPanelText_1);
	elseif(3 == num) then
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.ThreePanelImage_0);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.ThreePanelText_0);
		
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.ThreePanelImage_1);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.ThreePanelText_1);
		
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.ThreePanelImage_2);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.ThreePanelText_2);
	elseif(4 == num) then
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FourPanelImage_0);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FourPanelText_0);
		
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FourPanelImage_1);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FourPanelText_1);
		
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FourPanelImage_2);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FourPanelText_2);
		
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FourPanelImage_3);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FourPanelText_3);
	elseif(5 == num) then
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FivePanelImage_0);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FivePanelText_0);
		
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FivePanelImage_1);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FivePanelText_1);
		
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FivePanelImage_2);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FivePanelText_2);
		
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FivePanelImage_3);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FivePanelText_3);
		
		objectImage = GlobalNS.new(GlobalNS.PerformancePanelNS.PerformViewItem);
		objectImage:init();
		self.mObjectImageList:add(objectImage);
		objectImage:setObjectImageNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FivePanelImage_4);
		objectImage:setObjectNumNodePath(GlobalNS.PerformancePanelNS.PerformancePanelPath.FivePanelText_4);
	end
end

function M:setInfo()
	local index = 0;
	local listLen = self.mObjectImageList:count();
	local item = nil;
	local baseObjectId = 10000;
	
	while(index < listLen) do
		item = self.mObjectImageList:get(index);
		item:setObjectBaseId(10000 + 1);
		item:setObjectNum(10000 + 1);
		
		index = index + 1;
	end
end

return M;