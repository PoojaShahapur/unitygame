MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIStartGame.StartGameNS");
MLoader("MyLua.UI.UIStartGame.StartGameData");
MLoader("MyLua.UI.UIStartGame.StartGameCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIStartGame";
GlobalNS.StartGameNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIStartGame;
	self.mData = GlobalNS.new(GlobalNS.StartGameNS.StartGameData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	--右侧收缩动画状态
    self.isPlay = false;
    self.password = "111111";

    --昵称随机
	self.mNickNameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mNickNameBtn:addEventHandle(self, self.onNickNameBtnClk);
    
    --开始游戏
	self.mStartGameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mStartGameBtn:addEventHandle(self, self.onStartGameBtnClk);

    --收缩按钮
    self.mDropBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mDropBtn:addEventHandle(self, self.onDropBtnClk);
    --签到按钮
    self.mSignBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mSignBtn:addEventHandle(self, self.onSignBtnClk);
    --设置按钮
    self.mSettingBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mSettingBtn:addEventHandle(self, self.onSettingBtnClk);
    --分享按钮
    self.mShareBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mShareBtn:addEventHandle(self, self.onShareBtnClk);

    --战队按钮
    self.mCorpsBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mCorpsBtn:addEventHandle(self, self.onCorpsBtnClk);
    --关系按钮
    self.mFriendBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mFriendBtn:addEventHandle(self, self.onFriendBtnClk);
    --排行按钮
    self.mRankBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mRankBtn:addEventHandle(self, self.onRankBtnClk);
    --商城按钮
    self.mShopBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mShopBtn:addEventHandle(self, self.onShopBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    self:initForm(); --初始化组件
    self:setUsernameAndPassword();--设置用户名密码
    GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:SetServerIP();
end

function M:initForm()
    local bg_image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG_Image");

    --头像
    self.mAvatarBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "AvatarBG");
    self.mAvatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "AvatarImage");
    self.mGoldImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "GoldImage");
    self.mLevelName = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "LevelName");
    self.mStar1 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Star1");
    self.mStar2 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Star2");
    self.mStar3 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Star3");
    self.mStar4 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Star4");
    self.mStar5 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Star5");
	self.mLevelNum = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "LevelNum");

    --昵称
    self.mNameBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "NameBG");
    self.mNickNameInput = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mNameBG, "MyName");
    self.inputText = GlobalNS.UtilApi.GetComponent(self.mNickNameInput, "InputField");    
    self.mNickNameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mNameBG, "RandomName_BtnTouch"));
    self.mStartGameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "StartGame_BtnTouch"));
    
    --功能设置区相关控件
    self.mSettingsPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "SettingsPanel");
    self.mSettingsAnimator = GlobalNS.UtilApi.GetComponent(self.mSettingsPanel, "Animator");

    --收缩按钮
    self.mDropBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mSettingsPanel, "Drop_BtnTouch"));
    --签到按钮
    self.mSignBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mSettingsPanel, "Sign_BtnTouch"));
    --设置按钮
    self.mSettingBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mSettingsPanel, "Setting_BtnTouch"));
    --分享按钮
    self.mShareBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mSettingsPanel, "Share_BtnTouch"));
    
    --底部按钮区
    self.mBottomImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "BottomImage");
    --战队按钮
    self.mCorpsBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mBottomImage, "Corps_BtnTouch"));
    --关系按钮
    self.mFriendBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mBottomImage, "Friend_BtnTouch"));
    --排行按钮
    self.mRankBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mBottomImage, "Ranking_BtnTouch"));
    --商城按钮
    self.mShopBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mBottomImage, "Shop_BtnTouch"));
end

function M:setUsernameAndPassword()
    --self.inputText.text = M:getRandomNickName();
   
    self.username = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString(SDK.Lib.SystemSetting.USERNAME);
    if self.username ~= "" then
           self.inputText.text = self.username;
    end

    --self.password = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString(SDK.Lib.SystemSetting.PASSWORD);
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

function M:getRandomNickName()
    local nickname = { "Bone", "哈哈哈", "红红火火", "吃豆人", "哆啦A梦"};
    local index = math.random(1, 5);
    return nickname[index];
end

function M:loginOrCreateAccount(selectEnterMode)
    local username = self.inputText.text;
    if GlobalNS.CSSystem.Ctx.mInstance.mLoginSys:getLoginState() ~= SDK.Lib.LoginState.eLoginingLoginServer and GlobalNS.CSSystem.Ctx.mInstance.mLoginSys:getLoginState() ~= SDK.Lib.LoginState.eLoginingGateServer then  -- 如果没有正在登陆登陆服务器和网关服务器
         self.username = username;
         --GCtx.mLogSys:log("UserName is :" .. self.username .. ", Password is " .. self.password, GlobalNS.LogTypeId.eLogCommon);
         GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setString(SDK.Lib.SystemSetting.USERNAME, self.username);
         GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setString(SDK.Lib.SystemSetting.PASSWORD, self.password);
         
         if string.len(self.username) > 0 and string.len(self.password) > 5 then
            if not MacroDef.DEBUG_NOTNET then
               GlobalNS.CSSystem.Ctx.mInstance.mLoginSys.mLoginNetHandleCB_KBE:setAccountAndPasswd(self.username, self.password);
         
               if SDK.Lib.SelectEnterMode.eLoginAccount == selectEnterMode then
                  if not GCtx.mGameData.isRelogin then
                      GlobalNS.CSSystem.Ctx.mInstance.mLoginSys.mLoginNetHandleCB_KBE:login();
                      GCtx.mGameData.isRelogin = true;
                  else
                      GlobalNS.CSSystem.Ctx.mInstance.mLoginSys.mLoginNetHandleCB_KBE:relogin();
                  end                  
               elseif SDK.Lib.SelectEnterMode.eCreateAccount == selectEnterMode then
                  GlobalNS.CSSystem.Ctx.mInstance.mLoginSys.mLoginNetHandleCB_KBE:createAccount();
               else
               end
            else
               GlobalNS.CSSystem.Ctx.mInstance.mModuleSys:loadModule(SDK.Lib.ModuleId.GAMEMN);
            end
         else
             GCtx.mLogSys:log("account or password is error, length < 6!(账号或者密码错误，长度必须大于5!)", GlobalNS.LogTypeId.eLogCommon);
         end
    end
end

function M:onNickNameBtnClk()
    --随机昵称
    self.inputText.text = self:getRandomNickName();
end

function M:onStartGameBtnClk()
    --GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIStartGame);
    --GlobalNS.CSSystem.Ctx.mInstance.mModuleSys:unloadModule(GlobalNS.CSSystem.ModuleId.LOGINMN);
    --GlobalNS.CSSystem.Ctx.mInstance.mModuleSys:loadModule(GlobalNS.CSSystem.ModuleId.GAMEMN);
    self:loginOrCreateAccount(SDK.Lib.SelectEnterMode.eLoginAccount);
end

function M:onDropBtnClk()

    if(not self.isPlay) then
        self.mSettingsAnimator:SetFloat("Speed", 1);
        self.mSettingsAnimator:SetInteger("isPlay", 1);
        self.isPlay = true;
    else
        --动画倒播，Inspector面板中速度设置为-1即可
        self.mSettingsAnimator:SetFloat("Speed", 1);
        self.mSettingsAnimator:SetInteger("isPlay", 2);
        self.isPlay = false;
    end
end

function M:onSignBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUISignPanel);
end

function M:onSettingBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUISettingsPanel);
end

function M:onShareBtnClk()
    GCtx.mGameData:ShowRollMessage("hello world");
end

function M:onCorpsBtnClk()
    GCtx.mLogSys:log("Corps Btn Touch", GlobalNS.LogTypeId.eLogCommon);
end

function M:onFriendBtnClk()
    GCtx.mLogSys:log("Friend Btn Touch", GlobalNS.LogTypeId.eLogCommon);
end

function M:onRankBtnClk()
    GCtx.mLogSys:log("Rank Btn Touch", GlobalNS.LogTypeId.eLogCommon);
end

function M:onShopBtnClk()
    GCtx.mGoodsData:init();--打开商店时加载配置
    GCtx.mGoodsData.CurrentShopType = GlobalNS.UIFormId.eUIShop_SkinPanel;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIShop_SkinPanel);
end

return M;