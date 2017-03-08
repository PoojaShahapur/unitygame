MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIStartGame.StartGameNS");
MLoader("MyLua.UI.UIStartGame.StartGameData");
MLoader("MyLua.UI.UIStartGame.StartGameCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIStartGame";
GlobalNS.StartGameNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIStartGame;
	self.mData = GlobalNS.new(GlobalNS.StartGameNS.StartGameData);
    self.mAvatarBtn = nil;
end

function M:dtor()
	self.mAvatarBtn:dispose();
end

function M:onInit()
    M.super.onInit(self);
	--右侧收缩动画状态
    self.isPlay = false;
    self.password = "111111";
        
    --头像
	self.mAvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mAvatarBtn:addEventHandle(self, self.onAvatarBtnClk);

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
	
	self.mBarImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mBarImage:hide();	-- 默认隐藏
	self.mBarImage:setScale(0);
	self.mBgImage = GlobalNS.new(GlobalNS.AuxImage);
	self.mBgImage:hide();

    --联系我们
    self.mEmailBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mEmailBtn:addEventHandle(self, self.onEmailBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    self:initForm(); --初始化组件
    --self:setUsernameAndPassword();--设置用户名密码
    self:setNickName(); --昵称
    GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:SetServerIP();
end

function M:initForm()
    local bg_image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG_Image");

    --头像
    self.mAvatarBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "AvatarBG");
    --self.mAvatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "AvatarImage");
    self.mAvatarBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Avatar_BtnTouch"));
    self.mGoldImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "GoldImage");
    self.mLevelName = GlobalNS.UtilApi.getComByPath(self.mAvatarBG, "LevelName", "Text");
    self.mStar1 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Star1");
    self.mStar2 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Star2");
    self.mStar3 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Star3");
    self.mStar4 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Star4");
    self.mStar5 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Star5");
	self.mLevelNum = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "LevelNum");

    --头像
    local index = 1;
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("Avatar") then
        index = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("Avatar");
    end
    self.mAvatarBtn.mImage:setSelfGo(self.mAvatarBtn:getSelfGo());
    self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/"..index..".png", GlobalNS.UtilStr.tostring(index));
	--self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(index));

    --账号
    local username = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString(SDK.Lib.SystemSetting.USERNAME);
    if username == nil then
        username = "游客";
    end
    self.mLevelName.text = username;

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
	
	-- 进度条
	self.mBarImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "ProgressBar/Bar"));
	self.mBgImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "ProgressBar/BG"));

    -- 联系我们
    self.mEmailBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "Email_BtnTouch"));
end

function M:setUsernameAndPassword()
    --self.inputText.text = M:getRandomNickName();
   
    self.username = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString(SDK.Lib.SystemSetting.USERNAME);
    if self.username == nil or self.username == "" then
        self.inputText.text = self:getRandomNickName();
    else
        self.inputText.text = self.username;
    end

    --self.password = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString(SDK.Lib.SystemSetting.PASSWORD);
end

function M:setNickName()
    self.nickname = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString(SDK.Lib.SystemSetting.NICKNAME);
    if self.nickname == nil or self.nickname == "" then
        self.inputText.text = self:getRandomNickName();
    else
        self.inputText.text = self.nickname;
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

function M:GetNickNameWordNum(str)
    local lenInByte = #str;
    local count = 0;
    local i = 1;
    while i <= lenInByte do
        local curByte = string.byte(str, i);
        local byteCount = 1;
        if curByte > 0 and curByte < 128 then
            byteCount = 1;
        elseif curByte>=128 and curByte<224 then
            byteCount = 2;
        elseif curByte>=224 and curByte<240 then
            byteCount = 3;
        elseif curByte>=240 and curByte<=247 then
            byteCount = 4;
        else
            break;
        end

        i = i + byteCount;
        count = count + 1;
    end
    return count;
end

function M:LoginOrCreateAccount_new(selectEnterMode)
    local nickname = self.inputText.text;
    if GlobalNS.CSSystem.Ctx.mInstance.mLoginSys:getLoginState() ~= SDK.Lib.LoginState.eLoginingLoginServer and GlobalNS.CSSystem.Ctx.mInstance.mLoginSys:getLoginState() ~= SDK.Lib.LoginState.eLoginingGateServer then  -- 如果没有正在登陆登陆服务器和网关服务器
         self.nickname = nickname;
         if string.len(self.nickname) > 0 and self:GetNickNameWordNum(self.nickname) < 9 then
             GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setString(SDK.Lib.SystemSetting.NICKNAME, self.nickname);
         else
             GCtx.mGameData:ShowMessageBox("昵称不能为空或多于8个字符"..self:GetNickNameWordNum(self.nickname));
             return;
         end

         if not GCtx.mGameData.isRelogin then
             GlobalNS.CSSystem.Ctx.mInstance.mLoginSys.mLoginNetHandleCB_KBE:login();
         else
             GlobalNS.CSSystem.Ctx.mInstance.mLoginSys.mLoginNetHandleCB_KBE:relogin();
         end
    end
end

function M:onAvatarBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountPanel);
end

function M:onNickNameBtnClk()
    --随机昵称
    self.inputText.text = self:getRandomNickName();
end

function M:getRandomNickName()
    local socket = require("socket") -- 需要用到luasocket库  
    local t = string.format("%f", socket.gettime())  
    local st = string.sub(t, string.find(t, "%.") + 1, -1)
    math.randomseed(tonumber(string.reverse(st))); 
    local index = math.random(1, #self.mData.nicknames);
    return self.mData.nicknames[index];
end

function M:onStartGameBtnClk()
    --self:loginOrCreateAccount(SDK.Lib.SelectEnterMode.eLoginAccount);
    if not GCtx.mGameData.isRelogin then
        self.mBgImage:show();
	    self.mBarImage:show();
    end

    self:LoginOrCreateAccount_new(SDK.Lib.SelectEnterMode.eLoginAccount);
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
    -- GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUISignPanel);
    GCtx.mGameData:ShowRollMessage("暂未开放");
end

function M:onSettingBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUISettingsPanel);
end

function M:onShareBtnClk()
    --GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountPanel);
    GCtx.mGameData:ShowRollMessage("暂未开放");
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

function M:onEmailBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIBugReportPanel);
end

function M:setProgress(value)
	self.mBarImage:setScale(value);
end

function M:resetAvatar(index)
	self.mAvatarBtn.mImage:setSelfGo(self.mAvatarBtn:getSelfGo());
	self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/"..index..".png", GlobalNS.UtilStr.tostring(index));
    --self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(index));
end

return M;