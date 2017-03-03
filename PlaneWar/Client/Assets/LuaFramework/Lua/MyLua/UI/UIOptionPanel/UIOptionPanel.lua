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

    self.mTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);
    self.mCoolTime = 1;
    self.mInterval = 0.01;
end

function M:dtor()
	self.mTimer:Stop();
end

function M:onInit()
    M.super.onInit(self);

    self.mSwallowBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mSwallowBtn:addEventHandle(self, self.onSwallowBtnClk);
	--self.mSwallowBtn:addDownEventHandle(self, self.onSwallowBtnDown);
	--self.mSwallowBtn:addUpEventHandle(self, self.onSwallowBtnUp);
end

function M:onReady()
    M.super.onReady(self);

    self.mSwallowBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.OptionPanelNS.OptionPanelPath.BtnSwallow)
		);
    self.mSwallowImage = GlobalNS.UtilApi.getComByPath(self.mGuiWin, GlobalNS.OptionPanelNS.OptionPanelPath.BtnSwallow, "Image");
    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mMainChildNumChangedDispatch:addEventHandle(nil, nil, 0, self, self.refreshNum, 0);
end

function M:refreshNum()
    if(GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero() ~= nil and
       GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero().mPlayerSplitMerge ~= nil) then
         local num = GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero().mPlayerSplitMerge.mPlayerChildMgr:getEntityCount();
         if num > GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mMaxShotNum then
            self.mCoolTime = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mMaxShotSeconds;
         else
            self.mCoolTime = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mMinShotSeconds + num * GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mShotInteval;
         end
         self.mCoolTime = GlobalNS.UtilMath.keepTwoDecimalPlaces(self.mCoolTime);
    end
end

--射击冷却
function M:Fire(totalTime)
    self.mTimer.mIsDisposed = false;
    self.mTimer:setTotalTime(totalTime);
    self.mTimer.mInternal = self.mInterval;
    self.mTimer:setFuncObject(self, self.onTick);
    self.mTimer:Start();
end

function M:onTick()
    local lefttime = GlobalNS.UtilMath.keepTwoDecimalPlaces(self.mTimer:getLeftRunTime());
    self.mSwallowImage.fillAmount = lefttime / self.mCoolTime;
	if lefttime <= 0 then
        self.mSwallowImage.fillAmount = 1.0;
        self.mSwallowBtn:enable();
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
    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mMainChildNumChangedDispatch:removeEventHandle(nil, nil, 0, self, self.refreshNum, 0);
end

function M:onSplitBtnClk()
	GlobalNS.CSSystem.startSplit();
end

function M:onSwallowBtnClk()
	GlobalNS.CSSystem.Fire();
    self.mSwallowBtn:disable();
    self:Fire(self.mCoolTime);
end

function M:onSwallowBtnDown()
	GlobalNS.CSSystem.startEmitSnowBlock();
end

function M:onSwallowBtnUp()
	GlobalNS.CSSystem.stopEmitSnowBlock();
end

return M;