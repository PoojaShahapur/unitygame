MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIBugReportPanel.BugReportPanelNS");
MLoader("MyLua.UI.UIBugReportPanel.BugReportPanelData");
MLoader("MyLua.UI.UIBugReportPanel.BugReportPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIBugReportPanel";
GlobalNS.BugReportPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIBugReportPanel;
	self.mData = GlobalNS.new(GlobalNS.BugReportPanelNS.BugReportPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mCloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCloseBtn:addEventHandle(self, self.onCloseBtnClk);

    self.mReportBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mReportBtn:addEventHandle(self, self.onReportBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
	self.mCloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Close_BtnTouch"));

    local Game_data = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Game_Data");
    local reportInput = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Game_data, "BugReport_Input");
    self.reportInputText = GlobalNS.UtilApi.GetComponent(reportInput, "InputField");
    self.mReportBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Game_data, "Report_BtnTouch"));
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

function M:onCloseBtnClk()
	self:exit();
end

function M:onReportBtnClk()
    if not GlobalNS.UtilStr.IsNullOrEmpty(self.reportInputText.text) then
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setString("MyReport", self.reportInputText.text);
    end

	self:exit();
end

return M;