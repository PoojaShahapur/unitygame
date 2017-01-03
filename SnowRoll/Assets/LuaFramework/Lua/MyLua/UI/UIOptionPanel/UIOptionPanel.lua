MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIOptionPanel.OptionPanelNS");
MLoader("MyLua.UI.UIOptionPanel.OptionPanelData");
MLoader("MyLua.UI.UIOptionPanel.OptionPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIOptionPanel";
GlobalNS.OptionPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIOptionPanel;
	self.mData = GlobalNS.new(GlobalNS.OptionPanelNS.OptionPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mSplitBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mSplitBtn:addEventHandle(self, self.onSplitBtnClk);

    self.mSwallowBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mSwallowBtn:addEventHandle(self, self.onSwallowBtnClk);
end

function M:onReady()
    M.super.onReady(self);
	self.mSplitBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.OptionPanelNS.OptionPanelPath.BtnSplit)
		);

    self.mSwallowBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.OptionPanelNS.OptionPanelPath.BtnSwallow)
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

function M:onSplitBtnClk()
	GlobalNS.CSSystem.startSplit();
end

function M:onSwallowBtnClk()
	--GCtx.mLogSys:log("Swallow", GlobalNS.LogTypeId.eLogCommon);
	GlobalNS.CSSystem.emitSnowBlock();
end

return M;