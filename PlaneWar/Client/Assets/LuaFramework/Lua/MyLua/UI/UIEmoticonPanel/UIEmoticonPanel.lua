MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIEmoticonPanel.EmoticonPanelNS");
MLoader("MyLua.UI.UIEmoticonPanel.EmoticonPanelData");
MLoader("MyLua.UI.UIEmoticonPanel.EmoticonPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIEmoticonPanel";
GlobalNS.EmoticonPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIEmoticonPanel;
	self.mData = GlobalNS.new(GlobalNS.EmoticonPanelNS.EmoticonPanelData);

    self.mTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);
    self.mRollTime = 1;
    self.mInterval = 0.01; --滚动间隔

    self.emoticon = nil;
    self.emoticonindex = 1;
    self.emoticonstart_y = -167.5;
    self.emoticonnum = 10;
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
end

function M:onReady()
    M.super.onReady(self);
	local rollEmoticonDlg = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "RollEmoticonDlg");

    self.Emoticon = GlobalNS.UtilApi.TransFindChildByPObjAndPath(rollEmoticonDlg, "Emoticon");
    self.Emoticon_Big = GlobalNS.UtilApi.TransFindChildByPObjAndPath(rollEmoticonDlg, "Emoticon_Big");
    self.emoticon = GlobalNS.new(GlobalNS.AuxImage);

    local _time = os.clock();
    math.randomseed(_time);
    self.emoticonindex = math.random(1, self.emoticonnum);

    if self.emoticonindex <= 4 then
        self.emoticon:setSelfGo(self.Emoticon);
        self.Emoticon:SetActive(true);
        self.Emoticon_Big:SetActive(false);
        self.emoticonstart_y = -167.5;
    else
        self.emoticon:setSelfGo(self.Emoticon_Big);
        self.Emoticon:SetActive(false);
        self.Emoticon_Big:SetActive(true);
        self.emoticonstart_y = -117.5;
    end
    
	self.emoticon:setSpritePath("DefaultSkin/Emoticon/"..self.emoticonindex..".png", GlobalNS.UtilStr.tostring(self.emoticonindex));

    self:RollEmoticon();
end

function M:RollEmoticon()
    self.mTimer:setTotalTime(self.mRollTime);
    self.mTimer.mInternal = self.mInterval;
    self.mTimer:setFuncObject(self, self.onTick);
    self.mTimer:Start();
end

function M:onTick()
    local lefttime = GlobalNS.UtilMath.ceil(self.mTimer:getLeftRunTime());
    --消息滚动
    local runtime = self.mTimer:getRunTime();
    local interval = self.mRollTime / self.mInterval;
    local runval = runtime / self.mInterval;
    local y = 187 * runval / interval;
    
    if self.emoticonindex <= 4 then
        GlobalNS.UtilApi.GetComponent(self.Emoticon, "RectTransform").localPosition = Vector3.New(0, self.emoticonstart_y + y, 0); 
    else
        GlobalNS.UtilApi.GetComponent(self.Emoticon_Big, "RectTransform").localPosition = Vector3.New(0, self.emoticonstart_y + y, 0); 
    end 
    
    --
	if lefttime <= 0 then
        self.emoticon:dispose();
        self.mTimer:Stop();
        self:exit();
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
    self.mTimer:Stop();
end

return M;