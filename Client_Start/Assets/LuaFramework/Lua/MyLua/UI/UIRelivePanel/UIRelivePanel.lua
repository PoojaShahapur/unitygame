MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIRelivePanel.RelivePanelNS");
MLoader("MyLua.UI.UIRelivePanel.RelivePanelData");
MLoader("MyLua.UI.UIRelivePanel.RelivePanelCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIRelivePanel";
GlobalNS.RelivePanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormID.eUIRelivePanel;
	self.mData = GlobalNS.new(GlobalNS.RelivePanelNS.RelivePanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);

    self.mBackRoomBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBackRoomBtn:addEventHandle(self, self.onBtnBackRoomClk);
	
	self.mReliveBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mReliveBtn:addEventHandle(self, self.onBtnReliveClk);
end

function M:onReady()
    M.super.onReady(self);
    local roomFatherBtn = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BackRoom");
    self.mBackRoomBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			roomFatherBtn, 
			GlobalNS.RelivePanelNS.RelivePanelPath.BtnBackRoom)
		);

	self.mReliveBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.RelivePanelNS.RelivePanelPath.BtnRelive)
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

function M:onBtnReliveClk()
	GCtx.mLogSys:log("Relive", GlobalNS.LogTypeId.eLogCommon);
end

function M:onBtnBackRoomClk()
	GCtx.mLogSys:log("Back Room", GlobalNS.LogTypeId.eLogCommon);
end

return M;