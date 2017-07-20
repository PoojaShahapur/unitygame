MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIFirstPlayTipPanel.FirstPlayTipPanelNS");
MLoader("MyLua.UI.UIFirstPlayTipPanel.FirstPlayTipPanelData");
MLoader("MyLua.UI.UIFirstPlayTipPanel.FirstPlayTipPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIFirstPlayTipPanel";
GlobalNS.FirstPlayTipPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIFirstPlayTipPanel;
	self.mData = GlobalNS.new(GlobalNS.FirstPlayTipPanelNS.FirstPlayTipPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);

    self.mTip1Btn = GlobalNS.new(GlobalNS.AuxButton);
	self.mTip1Btn:addEventHandle(self, self.onTip1BtnClk);

    self.mTip3Btn = GlobalNS.new(GlobalNS.AuxButton);
	self.mTip3Btn:addEventHandle(self, self.onTip3BtnClk);
	
	self.mStartGameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mStartGameBtn:addEventHandle(self, self.onBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIStartGame);

    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    self.tip1 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Tip1");
    self.mTip1Btn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.tip1, "Tip1_BtnTouch"));

    self.tip3 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Tip3");
    self.mTip3Btn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.tip3, "Tip3_BtnTouch"));
    
    self.tip5 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Tip5");
	self.mStartGameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.tip5, "StartGame_BtnTouch"));
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

function M:onTip1BtnClk()
    self.tip1:SetActive(false);
    self.tip3:SetActive(true);
end

function M:onTip3BtnClk()
    self.tip3:SetActive(false);
    self.tip5:SetActive(true);
end

function M:onBtnClk()
	if(not GlobalNS.UtilApi.IsUObjNil(GlobalNS.CSSystem.Ctx.mInstance.mLoginModule)) then
		if(GlobalNS.CSSystem.isEnableGuide()) then
			GlobalNS.CSSystem.execGuide();
		else
			GlobalNS.CSSystem.Ctx.mInstance.mLoginModule.mLoginNetNotify:enterRoom(0);
		end
	end
end

return M;