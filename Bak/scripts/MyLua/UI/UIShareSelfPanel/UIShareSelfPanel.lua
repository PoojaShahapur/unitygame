MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIShareSelfPanel.ShareSelfPanelNS");
MLoader("MyLua.UI.UIShareSelfPanel.ShareSelfPanelData");
MLoader("MyLua.UI.UIShareSelfPanel.ShareSelfPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIShareSelfPanel";
GlobalNS.ShareSelfPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIShareSelfPanel;
	self.mData = GlobalNS.new(GlobalNS.ShareSelfPanelNS.ShareSelfPanelData);
    --self.avatar = nil;
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mShareBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mShareBtn:addEventHandle(self, self.onBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
	self.mShareBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Share_BtnTouch"));

    --[[ 头像
    local index = 1;
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("Avatar") then
        index = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("Avatar");
    end
    local avatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Avatar");
    self.avatar = GlobalNS.new(GlobalNS.AuxImage);
    self.avatar:setSelfGo(avatar);
    --self.avatar:setSpritePath("DefaultSkin/Avatar/"..index..".png", GlobalNS.UtilStr.tostring(index));
	self.avatar:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(index));
    ]]--
    -- 数据
    self:setMyInfo(BG);
end

function M:setMyInfo(BG)
    local index = GCtx.mGameData.myRank;
    local name = GlobalNS.UtilApi.getComByPath(BG, "name", "Text");
    local account = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString(SDK.Lib.SystemSetting.USERNAME);
    if account == nil or account == "" then
        name.text = "PC测试账户";
    else
        name.text = account;
    end
    
    --planenum = GlobalNS.UtilApi.getComByPath(BG, "planenum", "Text");
    --killednum = GlobalNS.UtilApi.getComByPath(BG, "killednum", "Text");
    local score = GlobalNS.UtilApi.getComByPath(BG, "score", "Text");
    local rank = GlobalNS.UtilApi.getComByPath(BG, "rank", "Text");
    if 0 == index then
        score.text = "积分：<color=#EEEE00FF>未记录</color>";
        rank.text = "排名：<color=#EEEE00FF>未记录</color>";
    else
        score.text = "积分：<color=#EEEE00FF>" .. GCtx.mGameData.rankinfolist[index].m_swallownum .. "</color>";
        rank.text = "排名：第<color=#EEEE00FF>" .. index .. "</color>名";
    end
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    GCtx.mGameData:requestBackHall();
    M.super.onExit(self);
end

function M:onBtnClk()
	GlobalNS.CSSystem.Ctx.mInstance.mCamSys:ShareTo3Party(GCtx.mPlayerData.mHeroData.mMyselfAccount);
    --[[
    self.avatar:dispose();
    self.avatar = nil;
    ]]--
    self:exit();    
end

return M;