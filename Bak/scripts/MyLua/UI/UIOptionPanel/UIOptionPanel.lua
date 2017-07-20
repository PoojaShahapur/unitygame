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

    self.childNum = 0;

    self.mTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);
    self.mCoolTime = 1;
    self.mInterval = 0.01;
    self.mIsOnFire = false;
    self.mOldCoolTime = 1;

    self.mSplitTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);
    self.mSplitCoolTime = 1.5;
    self.mSplitInterval = 0.03;
    self.mIsOnSplit = false;
	
	--self:startBeginnerGuideEffect();
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);

    self.mFireBtn = GlobalNS.new(GlobalNS.AuxButton);
	--self.mFireBtn:addEventHandle(self, self.onFireBtnClk);
	self.mFireBtn:addDownEventHandle(self, self.onFireBtnDown);
	self.mFireBtn:addUpEventHandle(self, self.onFireBtnUp);

    self.mSplitBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mSplitBtn:addEventHandle(self, self.onSplitBtnClk);
	
	self.mSwallowBtnActor = GlobalNS.new(GlobalNS.AuxComponent);
	self.mSwallowMaskBtnActor = GlobalNS.new(GlobalNS.AuxComponent);
	self.mSplitBtnActor = GlobalNS.new(GlobalNS.AuxComponent);
	self.mSplitMaskBtnActor = GlobalNS.new(GlobalNS.AuxComponent);
end

function M:onReady()
    M.super.onReady(self);

    --射击
    self.mFireBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.OptionPanelNS.OptionPanelPath.BtnSwallow)
		);
    self.mSwallowImage = GlobalNS.UtilApi.getComByPath(self.mGuiWin, GlobalNS.OptionPanelNS.OptionPanelPath.BtnSwallow, "Image");
    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mMainChildNumChangedDispatch:addEventHandle(nil, nil, 0, self, self.refreshNum, 0);
    
    --分裂
    self.mSplitBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.OptionPanelNS.OptionPanelPath.BtnSplit)
		);
    self.mSplitImage = GlobalNS.UtilApi.getComByPath(self.mGuiWin, GlobalNS.OptionPanelNS.OptionPanelPath.BtnSplit, "Image");
    self.mSplitCoolTime = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mSplitCoolTime + GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mShotDelaySeconds;
	
	self.mSwallowBtnActor:setSelfGoByPath(self.mGuiWin, GlobalNS.OptionPanelNS.OptionPanelPath.BtnSwallow);
	self.mSwallowMaskBtnActor:setSelfGoByPath(self.mGuiWin, GlobalNS.OptionPanelNS.OptionPanelPath.BtnSwallowMask);
	self.mSplitBtnActor:setSelfGoByPath(self.mGuiWin, GlobalNS.OptionPanelNS.OptionPanelPath.BtnSplit);
	self.mSplitMaskBtnActor:setSelfGoByPath(self.mGuiWin, GlobalNS.OptionPanelNS.OptionPanelPath.BtnSplitMask);

    self:refreshNum();
	
	if(GCtx.mBeginnerGuideSys:isEnableGuide()) then
		GCtx.mBeginnerGuideSys:onFormReady(GlobalNS.GuideTypeId.eGTShoot);
	end
end

function M:refreshNum()
    if GlobalNS.CSSystem.Ctx.mInstance.mShareData:getGameMode() ~= 1 then --正常模式
        if(GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero() ~= nil and
           GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero().mPlayerSplitMerge ~= nil) then
             self.childNum = GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero().mPlayerSplitMerge.mPlayerChildMgr:getEntityCount();
             self.mCoolTime = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mMinShotSeconds + self.childNum * GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mShotInteval;
             if self.mCoolTime > GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mMaxShotSeconds then
                self.mCoolTime = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mMaxShotSeconds;
             else
                self.mCoolTime = GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mMinShotSeconds + self.childNum * GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mShotInteval;
             end
             self.mCoolTime = GlobalNS.UtilMath.keepTwoDecimalPlaces(self.mCoolTime);
             self.mCoolTime = self.mCoolTime + GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mShotDelaySeconds;
        end
    else--炼狱模式服务器设置cd
        if(GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero() ~= nil and
           GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero().mPlayerSplitMerge ~= nil) then
            self.childNum = GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero().mPlayerSplitMerge.mPlayerChildMgr:getEntityCount();
            self.mCoolTime = GlobalNS.CSSystem.Ctx.mInstance.mShareData:getShotCD() / 1000;
            self.mCoolTime = GlobalNS.UtilMath.keepTwoDecimalPlaces(self.mCoolTime);
            self.mCoolTime = self.mCoolTime + GlobalNS.CSSystem.Ctx.mInstance.mSnowBallCfg.mShotDelaySeconds;
        end
    end
end

--射击冷却
function M:Fire(totalTime)
    self.mOldCoolTime = totalTime;
    self.mTimer.mIsDisposed = false;
    self.mTimer:setTotalTime(totalTime);
    self.mTimer.mInternal = self.mInterval;
    self.mTimer:setFuncObject(self, self.onTick);
    self.mTimer:reset();
    self.mTimer:Start();
end

function M:onTick()
    local lefttime = GlobalNS.UtilMath.keepTwoDecimalPlaces(self.mTimer:getLeftRunTime());
    self.mSwallowImage.fillAmount = lefttime / self.mOldCoolTime;
	if lefttime <= 0 then
        self.mSwallowImage.fillAmount = 1.0;
        self.mFireBtn:enable();
        self.mIsOnFire = false;
    end
end

--分裂冷却
function M:Split(totalTime)
    self.mSplitTimer.mIsDisposed = false;
    self.mSplitTimer:setTotalTime(totalTime);
    self.mSplitTimer.mInternal = self.mSplitInterval;
    self.mSplitTimer:setFuncObject(self, self.onSplitTick);
    self.mSplitTimer:reset();
    self.mSplitTimer:Start();
end

function M:onSplitTick()
    local lefttime = GlobalNS.UtilMath.keepTwoDecimalPlaces(self.mSplitTimer:getLeftRunTime());
    self.mSplitImage.fillAmount = lefttime / self.mSplitCoolTime;
	if lefttime <= 0 then
        self.mSplitImage.fillAmount = 1.0;
        self.mSplitBtn:enable();
        self.mIsOnSplit = false;
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
    self.mSplitTimer:Stop();
    self.mTimer:Stop();
    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mMainChildNumChangedDispatch:removeEventHandle(nil, nil, 0, self, self.refreshNum, 0);
end

function M:onSplitBtnClk()
	if(GCtx.mBeginnerGuideSys:isEnableGuide()) then
		GCtx.mBeginnerGuideSys:nextGuide(GlobalNS.GuideTypeId.eGTSplit);
	end
	
    if self.childNum < 2 then
        return;
    end
    if self.mIsOnSplit then
        return;
    end
    self.mIsOnSplit = true;
	GlobalNS.CSSystem.startSplit();
    self.mSplitBtn:disable();
    self:Split(self.mSplitCoolTime);
end

function M:onFireBtnClk()
    if self.mIsOnFire then
        return;
    end
    self.mIsOnFire = true;
	GlobalNS.CSSystem.Fire();
    self.mFireBtn:disable();
    self:Fire(self.mCoolTime);
end

function M:onFireBtnDown()
	GlobalNS.CSSystem.startFire();
end

function M:onFireBtnUp()
	if(GCtx.mBeginnerGuideSys:isEnableGuide()) then
		GCtx.mBeginnerGuideSys:nextGuide(GlobalNS.GuideTypeId.eGTShoot);
	end
	GlobalNS.CSSystem.stopFire();
end

function M:startSwallowBtnBeginnerGuideEffect()
	if(nil == self.mSwallowBtnCanvasGroupAlphaAnim) then
		self.mSwallowBtnCanvasGroupAlphaAnim = GlobalNS.new(GlobalNS.CanvasGroupAlphaAnim);
		self.mSwallowBtnCanvasGroupAlphaAnim:setNumIncOrDecAnimMode(GlobalNS.NumIncOrDecAnimMode.ePingPong);
		self.mSwallowBtnCanvasGroupAlphaAnim:setMinValue(0);
		self.mSwallowBtnCanvasGroupAlphaAnim:setMaxValue(1);
		self.mSwallowBtnCanvasGroupAlphaAnim:setCurValue(1);
		
		self.mSwallowBtnCanvasGroupAlphaAnim:setIsNeedResetAlpha(true);
		self.mSwallowBtnCanvasGroupAlphaAnim:setResetAlpha(1);

		self:addSwallowBtnCanvasGroup();
	end

	self.mSwallowBtnCanvasGroupAlphaAnim:startAnim();
end

function M:stopSwallowBtnBeginnerGuideEffect()
	if(nil ~= self.mSwallowBtnCanvasGroupAlphaAnim) then
		self.mSwallowBtnCanvasGroupAlphaAnim:dispose();
		self.mSwallowBtnCanvasGroupAlphaAnim = nil;
	end
end

function M:addSwallowBtnCanvasGroup()
	if(nil ~= self.mSwallowBtnCanvasGroupAlphaAnim and self:isReady()) then
		local swallowBtn = GlobalNS.UtilApi.TransFindChildByPObjAndPath(
		self.mGuiWin, 
		GlobalNS.OptionPanelNS.OptionPanelPath.BtnSwallow);
		self.mSwallowBtnCanvasGroupAlphaAnim:addCanvasGroupByGo(swallowBtn);
		
		swallowBtn = GlobalNS.UtilApi.TransFindChildByPObjAndPath(
		self.mGuiWin, 
		GlobalNS.OptionPanelNS.OptionPanelPath.BtnSwallowMask)
		self.mSwallowBtnCanvasGroupAlphaAnim:addCanvasGroupByGo(swallowBtn);
	end
end

function M:startSplitBtnBeginnerGuideEffect()
	if(nil == self.mSplitBtnCanvasGroupAlphaAnim) then
		self.mSplitBtnCanvasGroupAlphaAnim = GlobalNS.new(GlobalNS.CanvasGroupAlphaAnim);
		self.mSplitBtnCanvasGroupAlphaAnim:setNumIncOrDecAnimMode(GlobalNS.NumIncOrDecAnimMode.ePingPong);
		self.mSplitBtnCanvasGroupAlphaAnim:setMinValue(0);
		self.mSplitBtnCanvasGroupAlphaAnim:setMaxValue(1);
		self.mSplitBtnCanvasGroupAlphaAnim:setCurValue(1);
		
		self.mSplitBtnCanvasGroupAlphaAnim:setIsNeedResetAlpha(true);
		self.mSplitBtnCanvasGroupAlphaAnim:setResetAlpha(1);

		self:addSplitBtnCanvasGroup();
	end

	self.mSplitBtnCanvasGroupAlphaAnim:startAnim();
end

function M:stopSplitBtnBeginnerGuideEffect()
	if(nil ~= self.mSplitBtnCanvasGroupAlphaAnim) then
		self.mSplitBtnCanvasGroupAlphaAnim:dispose();
		self.mSplitBtnCanvasGroupAlphaAnim = nil;
	end
end

function M:addSplitBtnCanvasGroup()
	if(nil ~= self.mSplitBtnCanvasGroupAlphaAnim and self:isReady()) then
		local splitBtn = GlobalNS.UtilApi.TransFindChildByPObjAndPath(
		self.mGuiWin, 
		GlobalNS.OptionPanelNS.OptionPanelPath.BtnSplit);
		self.mSplitBtnCanvasGroupAlphaAnim:addCanvasGroupByGo(splitBtn);
		
		splitBtn = GlobalNS.UtilApi.TransFindChildByPObjAndPath(
		self.mGuiWin, 
		GlobalNS.OptionPanelNS.OptionPanelPath.BtnSplitMask)
		self.mSplitBtnCanvasGroupAlphaAnim:addCanvasGroupByGo(splitBtn);
	end
end

function M:toggleShootBtn(value)
	if(value) then
		self.mSwallowBtnActor:show();
		self.mSwallowMaskBtnActor:show();
	else
		self.mSwallowBtnActor:hide();
		self.mSwallowMaskBtnActor:hide();		
	end
end

function M:toggleSplitBtn(value)
	if(value) then
		self.mSplitBtnActor:show();
		self.mSplitMaskBtnActor:show();
	else
		self.mSplitBtnActor:hide();
		self.mSplitMaskBtnActor:hide();		
	end
end

function M:getSwallowBtn()
	local ret = nil;
	ret = GlobalNS.UtilApi.TransFindChildByPObjAndPath(
		self.mGuiWin, 
		GlobalNS.OptionPanelNS.OptionPanelPath.BtnSwallow);
		
	return ret;
end

function M:getSplitBtn()
	local ret = nil;
	ret = GlobalNS.UtilApi.TransFindChildByPObjAndPath(
		self.mGuiWin, 
		GlobalNS.OptionPanelNS.OptionPanelPath.BtnSplit);
		
	return ret;
end

return M;