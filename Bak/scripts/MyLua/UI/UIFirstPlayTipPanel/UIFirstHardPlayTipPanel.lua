MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIFirstPlayTipPanel.FirstPlayTipPanelNS");
MLoader("MyLua.UI.UIFirstPlayTipPanel.FirstPlayTipPanelData");
MLoader("MyLua.UI.UIFirstPlayTipPanel.FirstPlayTipPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIFirstHardPlayTipPanel";
GlobalNS.FirstPlayTipPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.UIFirstHardPlayTipPanel;
	self.mData = GlobalNS.new(GlobalNS.FirstPlayTipPanelNS.FirstPlayTipPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);

	self.mStartGameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mStartGameBtn:addEventHandle(self, self.onBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIStartGame);

    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    self.tip1 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Tip1");
	self.mStartGameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.tip1, "StartGame_BtnTouch"));
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

function M:onBtnClk()
	if(not GlobalNS.UtilApi.IsUObjNil(GlobalNS.CSSystem.Ctx.mInstance.mLoginModule)) then
		GlobalNS.CSSystem.Ctx.mInstance.mLoginModule.mLoginNetNotify:enterRoom(1);
	end
end

return M;