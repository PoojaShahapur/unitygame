MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UITeamBattlePanel.TeamBattlePanelNS");
MLoader("MyLua.UI.UITeamBattlePanel.TeamBattlePanelData");
MLoader("MyLua.UI.UITeamBattlePanel.TeamBattlePanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UITeamRulePanel";
GlobalNS.TeamBattlePanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUITeamRulePanel;
	self.mData = GlobalNS.new(GlobalNS.TeamBattlePanelNS.TeamBattlePanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mCloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCloseBtn:addEventHandle(self, self.onCloseBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    self.mCloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Esc_BtnTouch"));
end


function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    if self.mCloseBtn ~= nil then
        self.mCloseBtn:dispose();
        self.mCloseBtn = nil;
    end

    M.super.onExit(self);
end

function M:onCloseBtnClk()
	self:exit();
end

return M;