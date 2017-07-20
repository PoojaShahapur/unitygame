MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIReconnectionPanel.ReconnectionPanelNS");
MLoader("MyLua.UI.UIReconnectionPanel.ReconnectionPanelData");
MLoader("MyLua.UI.UIReconnectionPanel.ReconnectionPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIReconnectionPanel";
GlobalNS.ReconnectionPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIReconnectionPanel;
	self.mData = GlobalNS.new(GlobalNS.ReconnectionPanelNS.ReconnectionPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mOKBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mOKBtn:addEventHandle(self, self.onBtnClk);
end

function M:onReady()
    M.super.onReady(self);
	local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    local Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Panel");
    local Image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Panel, "Image");
    self.mOKBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Image, "Button"));
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    if self.mOKBtn ~= nil then
        self.mOKBtn:dispose();
        self.mOKBtn = nil;
    end

    M.super.onExit(self);
end

function M:onBtnClk()
    GlobalNS.CSSystem.Ctx.mInstance.mShareData:ReconnectEnterRoom();
end

return M;