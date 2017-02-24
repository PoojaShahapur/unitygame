MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIAccountPanel.AccountPanelNS");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelData");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIAccountPanel";
GlobalNS.AccountPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIAccountPanel;
	self.mData = GlobalNS.new(GlobalNS.AccountPanelNS.AccountPanelData);
    self.index = 1;
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
    self.mAvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mAvatarBtn:addEventHandle(self, self.onAvatarBtnClk);

	self.mCloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCloseBtn:addEventHandle(self, self.onBtnClk);

    self.mShareBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mShareBtn:addEventHandle(self, self.onShareBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");    
	self.mCloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			BG, 
			GlobalNS.AccountPanelNS.AccountPanelPath.BtnClose)
		);
    self.mShareBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Share_BtnTouch"));

    local Avatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Avatar");
    self.mAvatarBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "Avatar_BtnTouch"));
    local Info = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "Info");

    --头像   
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("Avatar") then
        self.index = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("Avatar");
    end
    self:resetAvatar(self.index);

    --账号
    local Name = GlobalNS.UtilApi.getComByPath(Info, "Name", "Text");
    local username = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString(SDK.Lib.SystemSetting.USERNAME);
    if username == nil then
        username = "游客";
    end
    Name.text = username;

    --签名
    self.Sign = GlobalNS.UtilApi.getComByPath(Info, "Sign", "InputField");
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("SIGN") then
        local signText = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString("SIGN");
        self.Sign.text = signText;
    end
    
    --游戏数据
    local Game_Data = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Game_Data");

    local zhenzhu = GlobalNS.UtilApi.getComByPath(Game_Data, "ZhenZhu", "Text");
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey(GCtx.mGoodsData.ZhenZhuId) then
        local num = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt(GCtx.mGoodsData.ZhenZhuId);
        zhenzhu.text = "珍珠: "..num;
    end

    local haixing = GlobalNS.UtilApi.getComByPath(Game_Data, "HaiXing", "Text");
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey(GCtx.mGoodsData.HaiXingId) then
        local num = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt(GCtx.mGoodsData.HaiXingId);
        haixing.text = "海星: "..num;
    end

    local HuiHe = GlobalNS.UtilApi.getComByPath(Game_Data, "HuiHe", "Text");
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("HuiHe") then
        local num = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("HuiHe");
        HuiHe.text = "游戏回合数: "..num;
    end

    local SwallowNum = GlobalNS.UtilApi.getComByPath(Game_Data, "SwallowNum", "Text");
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("SwallowNum") then
        local num = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("SwallowNum");
        SwallowNum.text = "总吞噬人数: "..num;
    end

    local MaxMass = GlobalNS.UtilApi.getComByPath(Game_Data, "MaxMass", "Text");
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("MaxMass") then
        local mass = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getFloat("MaxMass");
        local radius = GlobalNS.UtilMath.getRadiusByMass(mass); --服务器传过来的是质量
        MaxMass.text = "历史最大重量: "..GlobalNS.UtilMath.getShowMass(radius);
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
end

function M:onBtnClk()
    GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setString("SIGN", self.Sign.text);
    self.mAvatarBtn:dispose();
	self:exit();
end

function M:onAvatarBtnClk()
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountAvatarPanel);
end

function M:onShareBtnClk()
	--GlobalNS.CSSystem.Ctx.mInstance.mCamSys:ShareTo3Party();
    self:exit();
end

function M:resetAvatar(index)
    self.index = index;
	self.mAvatarBtn.mImage:setSelfGo(self.mAvatarBtn:getSelfGo());
	self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/"..self.index..".png", GlobalNS.UtilStr.tostring(self.index));
    --self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(self.index));
end

return M;