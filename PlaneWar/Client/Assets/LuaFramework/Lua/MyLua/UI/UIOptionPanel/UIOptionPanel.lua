MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

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

    self.mSwallowBtn = GlobalNS.new(GlobalNS.AuxButton);
	--self.mSwallowBtn:addEventHandle(self, self.onSwallowBtnClk);
	self.mSwallowBtn:addDownEventHandle(self, self.onSwallowBtnDown);
	self.mSwallowBtn:addUpEventHandle(self, self.onSwallowBtnUp);
end

function M:onReady()
    M.super.onReady(self);

    self.mSwallowBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.OptionPanelNS.OptionPanelPath.BtnSwallow)
		);

    -- self.mSwallowBtn:disable();

    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mMainChildMassChangedDispatch:addEventHandle(nil, nil, 0, self, self.refreshMass, 0);
end

function M:refreshMass()
    if(GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero() ~= nil and
       GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero().mPlayerSplitMerge ~= nil) then
         local canemit = GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero().mPlayerSplitMerge:isCanEmit();
		--[[
         if not canemit then
            self.mSwallowBtn:disable();
         else
            self.mSwallowBtn:enable();
         end
		]] 
    end
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mMainChildMassChangedDispatch:removeEventHandle(nil, nil, self, self.refreshMass);
end

function M:onSplitBtnClk()
	GlobalNS.CSSystem.startSplit();
end

function M:onSwallowBtnClk()
	--GCtx.mLogSys:log("Swallow", GlobalNS.LogTypeId.eLogCommon);
	GlobalNS.CSSystem.emitSnowBlock();
end

function M:onSwallowBtnDown()
	GlobalNS.CSSystem.startEmitSnowBlock();
end

function M:onSwallowBtnUp()
	GlobalNS.CSSystem.stopEmitSnowBlock();
end

return M;