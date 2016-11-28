MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIStartGame.StartGameNS");
MLoader("MyLua.UI.UIStartGame.StartGameData");
MLoader("MyLua.UI.UIStartGame.StartGameCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIStartGame";
GlobalNS.StartGameNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormID.eUIStartGame;
	self.mData = GlobalNS.new(GlobalNS.StartGameNS.StartGameData);
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
	self.mStartGameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.StartGameNS.StartGamePath.BtnStartGame)
		);
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
	GCtx.mLogSys:log("Start Game Btn Touch", GlobalNS.LogTypeId.eLogCommon);
end

return M;