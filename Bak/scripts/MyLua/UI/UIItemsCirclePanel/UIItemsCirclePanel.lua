MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIItemsCirclePanel.ItemsCirclePanelNS");
MLoader("MyLua.UI.UIItemsCirclePanel.ItemsCirclePanelData");
MLoader("MyLua.UI.UIItemsCirclePanel.ItemsCirclePanelCV");

MLoader("MyLua.UI.UIItemsCirclePanel.Panel.CirclePanelBase");
MLoader("MyLua.UI.UIItemsCirclePanel.Panel.CirclePackPanel");
MLoader("MyLua.UI.UIItemsCirclePanel.Panel.CircleShopPanel");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIItemsCirclePanel";
GlobalNS.ItemsCirclePanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIItemsCirclePanel;
	self.mData = GlobalNS.new(GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelData);
	self.mPanel = nil;
	
	self.mOpenType = 0;
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	--self.mReliveBtn = GlobalNS.new(GlobalNS.AuxButton);
	--self.mReliveBtn:addEventHandle(self, self.onBtnClk);
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachCloseBtn(GlobalNS.ItemsCirclePanelNS.ItemsCirclePanelPath.CloseBtn);
	self:findPanleWidget();
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    --GlobalNS.ItemsCirclePanelNS = nil; 代码不卸载
    
    M.super.onExit(self);
end

function M:onBtnClk()
	GCtx.mLogSys:log("Hello World", GlobalNS.LogTypeId.eLogCommon);
end

function M:setOpenType(value)
	self.mOpenType = value;
	
	if(GlobalNS.CirclePanelOpenType.eOpenFromPack == self.mOpenType) then
		self.mPanel = GlobalNS.new(GlobalNS.ItemsCirclePanelNS.CirclePackPanel);
	elseif(GlobalNS.CirclePanelOpenType.eOpenFromShop == self.mOpenType) then
		self.mPanel = GlobalNS.new(GlobalNS.ItemsCirclePanelNS.CircleShopPanel);
	end
	
	self.mPanel:init();
	self:findPanleWidget();
end

function M:findPanleWidget()
	if(nil ~= self.mPanel and self:isReady()) then
		self.mPanel:setGuiWin(self.mGuiWin);
		self.mPanel:attachWidget();
	end
end

function M:setObjectItem(value)
	self.mPanel:setObjectItem(value);
end

return M;