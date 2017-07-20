MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "BeginnerGuideSys";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mIsEnableGuide = false;
	self.mCurGuideTypeId = GlobalNS.GuideTypeId.eGTJoyStickClick;
end

function M:dtor()
	
end

function M:init()
	if(not GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("BeginnerGuide")) then
		self.mIsEnableGuide = true;
		MacroDef.DEBUG_NOTNET = true;
	end
end

function M:dispose()
	
end

function M:isEnableGuide()
	return self.mIsEnableGuide;
end

function M:setIsEnableGuide(value)
	self.mIsEnableGuide = value;
end

function M:enableGuide()
	local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIJoyStick);

	if(nil ~= form) then
		form:addCanvasGroup();
	end
end

function M:disableGuide()
	
end

function M:onLevelLoaded()
	GCtx.mGameData:ShowRollMessageWithTimeLen(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eMessage, 1), 1000000);
	
	local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIOptionPanel);
	if(nil ~= form) then
		form:toggleShootBtn(false);
		form:toggleSplitBtn(false);
	end
end

function M:nextGuide(guideTypeId, isNotifyNative)
	if(nil == isNotifyNative) then
		isNotifyNative = true;
	end
	
	if(self.mCurGuideTypeId == guideTypeId) then
		local form = nil;
		if(GlobalNS.GuideTypeId.eGTJoyStickClick == guideTypeId) then
			GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIMessagePanel);
			GCtx.mGameData:ShowRollMessageWithTimeLen(
				GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eMessage, 2), 
				1000000);
			
			form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIOptionPanel);
			
			if(nil ~= form) then
				form:toggleShootBtn(true);
				form:startSwallowBtnBeginnerGuideEffect();
				GlobalNS.CSSystem.GlobalEventCmd.onGuideFormReady(GlobalNS.UIFormId.eUIOptionPanel, self.mCurGuideTypeId, form:getSwallowBtn());
			end
		elseif(GlobalNS.GuideTypeId.eGTShoot == guideTypeId) then
			form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIOptionPanel);
			
			if(nil ~= form) then
				form:stopSwallowBtnBeginnerGuideEffect();
				form:startSplitBtnBeginnerGuideEffect();
				form:toggleSplitBtn(true);
				
				GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIMessagePanel);
			GCtx.mGameData:ShowRollMessageWithTimeLen(
				GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eMessage, 4), 
				1000000);
			
				GlobalNS.CSSystem.GlobalEventCmd.onGuideFormReady(GlobalNS.UIFormId.eUIOptionPanel, self.mCurGuideTypeId, form:getSplitBtn());
			end
		elseif(GlobalNS.GuideTypeId.eGTSplit == guideTypeId) then
			form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIOptionPanel);
			
			if(nil ~= form) then
				form:stopSplitBtnBeginnerGuideEffect();
			end
			
			GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIMessagePanel);
			GlobalNS.CSSystem.GlobalEventCmd.onGuideFormReady(GlobalNS.UIFormId.eUIOptionPanel, self.mCurGuideTypeId, nil);
		end
		
		if(isNotifyNative) then
			GlobalNS.CSSystem.mBeginnerGuideSys:nextGuide(self.mCurGuideTypeId, false);
		end
		
		self.mCurGuideTypeId = self.mCurGuideTypeId + 1;
	else
		if(GlobalNS.GuideTypeId.eGTEnd == self.mCurGuideTypeId) then
			GlobalNS.CSSystem.mBeginnerGuideSys:execEnd();
		end
	end
end

function M:onOkHandle(dispObj)
	GlobalNS.CSSystem.mBeginnerGuideSys:execEnd();
end

function M:onFormReady(guideTypeId)
	if(self.mCurGuideTypeId == guideTypeId) then
		local form = nil;
		
		if(GlobalNS.GuideTypeId.eGTShoot == self.mCurGuideTypeId) then
			form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIOptionPanel);
			
			if(nil ~= form) then
				form:addSwallowBtnCanvasGroup();
				form:addSplitBtnCanvasGroup();
				
				GlobalNS.CSSystem.GlobalEventCmd.onGuideFormReady(self.mCurGuideTypeId, form:getSwallowBtn());
			end
		end
	end
end

return M;