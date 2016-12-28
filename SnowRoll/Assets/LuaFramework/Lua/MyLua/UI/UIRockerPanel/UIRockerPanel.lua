MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIRockerPanel.RockerPanelNS");
MLoader("MyLua.UI.UIRockerPanel.RockerPanelData");
MLoader("MyLua.UI.UIRockerPanel.RockerPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIRockerPanel";
GlobalNS.RockerPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIRockerPanel;
	self.mData = GlobalNS.new(GlobalNS.RockerPanelNS.RockerPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mRockerBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mRockerBtn:addEventHandle(self, self.onBtnClk);
end

function M:onReady()
    M.super.onReady(self);
	self.mRockerBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.RockerPanelNS.RockerPanelPath.BtnRocker)
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
    --self:exit();
	--GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIForwardForce);
end

return M;