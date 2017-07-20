MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIPlayerDataPanel.PlayerDataPanelNS");
MLoader("MyLua.UI.UIPlayerDataPanel.PlayerDataPanelData");
MLoader("MyLua.UI.UIPlayerDataPanel.PlayerDataPanelCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIPlayerDataPanel";
GlobalNS.PlayerDataPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIPlayerDataPanel;
	self.mData = GlobalNS.new(GlobalNS.PlayerDataPanelNS.PlayerDataPanelData);    
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);

    --语音按钮
	self.mVoiceBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mVoiceBtn:addEventHandle(self, self.voicebtnclick);

    self.mMicBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mMicBtn:addEventHandle(self, self.micbtnclick);

    self.mTop1Arrow = GlobalNS.new(GlobalNS.AuxImage);
    self.mTop2Arrow = GlobalNS.new(GlobalNS.AuxImage);
    self.mTop3Arrow = GlobalNS.new(GlobalNS.AuxImage);
end

function M:onReady()
    M.super.onReady(self);
    self.moc_time = GlobalNS.UtilApi.getComByPath(self.mGuiWin, "oc_time_Text", "Text");
    self.mMass = GlobalNS.UtilApi.getComByPath(self.mGuiWin, "Mass_Text", "Text");
    self.mTime = GlobalNS.UtilApi.getComByPath(self.mGuiWin, "Time_Text", "Text");

    self.top1 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "top1");
    self.top1Rect = GlobalNS.UtilApi.GetComponent(self.top1, "RectTransform");
    self.mTop1Arrow:setSelfGo(self.top1);

    self.top2 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "top2");
    self.top2Rect = GlobalNS.UtilApi.GetComponent(self.top2, "RectTransform");
    self.mTop2Arrow:setSelfGo(self.top2);

    self.top3 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "top3");
    self.top3Rect = GlobalNS.UtilApi.GetComponent(self.top3, "RectTransform");
    self.mTop3Arrow:setSelfGo(self.top3);

    local voicebtn = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Voice_Button");
    self.mVoiceBtn:setSelfGo(voicebtn);
    self.mVoiceText = GlobalNS.UtilApi.getComByPath(voicebtn, "Text", "Text");
    self.mVoiceText.text = "关";
    self.openorclosevoice = true; --false:语音关闭状态， true:语音开放状态
    local micbtn = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Mic_Button");
    self.mMicBtn:setSelfGo(micbtn);
    self.mMicText = GlobalNS.UtilApi.getComByPath(micbtn, "Text", "Text");
    self.mMicText.text = "关";
    self.openorclosemic = true; --false:mic关闭状态， true:mic开放状态

    self.voiceok = false;--语音模块设置完成
    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mGiantVoiceLoadedDispatch:addEventHandle(nil, nil, 0, self, self.voicecompleted, 0);

    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mTopxPosChangedDispatch:addEventHandle(nil, nil, 0, self, self.refreshPos, 0);
    self:refreshScore(GCtx.mGameData.mMyScore); --加载完成主动刷新一次分数
    self:refreshLeftTime(GCtx.mGameData.totalTime);
    self:setArrowImage();
    self:refreshPos();
end

function M:setArrowImage()
    if 2 ~= GlobalNS.CSSystem.Ctx.mInstance.mShareData:getGameMode() then
        --普通模式和炼狱
        self.mTop1Arrow:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "first");
        self.mTop2Arrow:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "second");
        self.mTop3Arrow:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "third");
    else
        --组队模式
        self.mTop1Arrow:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "partner");
        self.mTop2Arrow:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "partner");
        --self.mTop3Arrow:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "partner");
    end

    if 1 == GlobalNS.CSSystem.Ctx.mInstance.mShareData:getGameMode() then
        --炼狱
        self.mVoiceBtn:hide();
        self.mMicBtn:hide();
    else
        self.mVoiceBtn:show();
        self.mMicBtn:show();
    end
end

function M:refreshScore(score)
    if score == nil then
        score = 0;
    end
	
	if self.mGuiWin == nil or self.mMass == nil then
        return;
    end
    
    self.mMass.text = "分数：" .. score;
end

function M:refreshLeftTime(leftseconds)
    --获取Time_Text的Text组件
    if self.mGuiWin == nil or self.mTime == nil then
        return;
    end

    self.mTime.text = "时间：" .. self:getTimeText(leftseconds);
end

function M:getTimeText(leftseconds)
    local min = leftseconds / 60;
    min = math.floor(min);
    local second = leftseconds % 60;

    local timestr = "";
    if min > 9 then
        timestr = min .. ":";
    else
        timestr = "0" .. min .. ":";
    end

    if second > 9 then
        timestr = timestr .. second;
    else
        timestr = timestr .. "0" .. second;
    end

    return timestr;
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

--语音初始化完成
function M:voicecompleted()
    self.voiceok = true;
end

function M:voicebtnclick()
    if not self.voiceok then
        return;
    end

    if not self.openorclosevoice then
        --打开
        GlobalNS.CSSystem.Ctx.mInstance.mGameModule:OnOpenLoudSpeaker();
        self.mVoiceText.text = "关";
    else
        --关闭
        GlobalNS.CSSystem.Ctx.mInstance.mGameModule:OnCloseLoudSpeaker();
        self.mVoiceText.text = "开";
    end
    self.openorclosevoice = not self.openorclosevoice;
end

function M:micbtnclick()
    if not self.voiceok then
        return;
    end

    if not self.openorclosemic then
        --打开
        GlobalNS.CSSystem.Ctx.mInstance.mGameModule:OnOpenMic();
        self.mMicText.text = "关";
    else
        --关闭
        GlobalNS.CSSystem.Ctx.mInstance.mGameModule:OnCloseMic();
        self.mMicText.text = "开";
    end
    self.openorclosemic = not self.openorclosemic;
end

function M:refreshPos()
    if GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero() ~= nil then
        if GlobalNS.CSSystem.Ctx.mInstance.mShareData.isTop1ShowArrow then
            local arrow_pos = GlobalNS.CSSystem.Ctx.mInstance.mShareData.top1_arrow_pos;
            local arrow_rotation = GlobalNS.CSSystem.Ctx.mInstance.mShareData.top1_arrow_rotation;
            self.top1Rect.localPosition = arrow_pos; --Vector3.New(arrow_pos.x, arrow_pos.y, 0);
            self.top1Rect.localRotation = arrow_rotation;
            self.top1:SetActive(true);
        else
            self.top1:SetActive(false);
        end

        if GlobalNS.CSSystem.Ctx.mInstance.mShareData.isTop2ShowArrow then
            local arrow_pos = GlobalNS.CSSystem.Ctx.mInstance.mShareData.top2_arrow_pos;
            local arrow_rotation = GlobalNS.CSSystem.Ctx.mInstance.mShareData.top2_arrow_rotation;
            self.top2Rect.localPosition = arrow_pos; --Vector3.New(arrow_pos.x, arrow_pos.y, 0);
            self.top2Rect.localRotation = arrow_rotation;
            self.top2:SetActive(true);
        else
            self.top2:SetActive(false);
        end

        if GlobalNS.CSSystem.Ctx.mInstance.mShareData.isTop3ShowArrow then
            local arrow_pos = GlobalNS.CSSystem.Ctx.mInstance.mShareData.top3_arrow_pos;
            local arrow_rotation = GlobalNS.CSSystem.Ctx.mInstance.mShareData.top3_arrow_rotation;
            self.top3Rect.localPosition = arrow_pos; --Vector3.New(arrow_pos.x, arrow_pos.y, 0);
            self.top3Rect.localRotation = arrow_rotation;
            self.top3:SetActive(true);
        else
            self.top3:SetActive(false);
        end
    end

    --系统时间
    local time_str = "00:00";
    time_str = os.date("%H:%M");
    if self.moc_time ~= nil then
        self.moc_time.text = time_str;
    end
end

function M:onExit()
    if self.mVoiceBtn ~= nil then
        self.mVoiceBtn:dispose();
        self.mVoiceBtn = nil;
    end

    if self.mMicBtn ~= nil then
        self.mMicBtn:dispose();
        self.mMicBtn = nil;
    end

    if self.mTop1Arrow ~= nil then
        self.mTop1Arrow:dispose();
        self.mTop1Arrow = nil;
    end
    if self.mTop2Arrow ~= nil then
        self.mTop2Arrow:dispose();
        self.mTop2Arrow = nil;
    end
    if self.mTop3Arrow ~= nil then
        self.mTop3Arrow:dispose();
        self.mTop3Arrow = nil;
    end

    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mGiantVoiceLoadedDispatch:removeEventHandle(nil, nil, 0, self, self.voicecompleted, 0);
    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mTopxPosChangedDispatch:removeEventHandle(nil, nil, 0, self, self.refreshPos, 0);

    M.super.onExit(self);
end

return M;