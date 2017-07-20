MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIStartGame.StartGameNS");
MLoader("MyLua.UI.UIStartGame.StartGameData");
MLoader("MyLua.UI.UIStartGame.StartGameCV");
MLoader("MyLua.LuaTable.level");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIStartGame";
GlobalNS.StartGameNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIStartGame;
	self.mData = GlobalNS.new(GlobalNS.StartGameNS.StartGameData);
    self.mAvatarBtn = nil;

    self.mCanClick = true;
    self.mTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);
    self.mTimer:setTotalTime(0.5);
    self.mTimer.mInternal = 0.1;
    self.mTimer:setFuncObject(self, self.onTick);

    self.mUFOMgr = GlobalNS.new(GlobalNS.UFOMgr);
    self.myAccount = "";
    self.myUid = 0;
end

function M:dtor()
	self.mAvatarBtn:dispose();
    self.mLevelBtn:dispose();
    self.mUFOMgr:dispose();
    self.mNickNameBtn:dispose();
    self.mStartGameBtn:dispose();
    self.mHardLevelBtn:dispose();
    self.mHardLevelLockedBtn:dispose();
    self.mTeamGameBtn:dispose();
    self.mTeamGameLockedBtn:dispose();
    self.mDropBtn:dispose();
    self.mSignBtn:dispose();
    self.mSettingBtn:dispose();
    self.mShareBtn:dispose();
    self.mCorpsBtn:dispose();
    self.mFriendBtn:dispose();
    self.mRankBtn:dispose();
    self.mShopBtn:dispose();
    self.mEmailBtn:dispose();
    self.mFirstPlayBtn:dispose();
    self.mAccountBtn:dispose();
    self.mFriendBtn:dispose();

    self.mLevelImage:dispose();
    self.mStar1Image:dispose();
    self.mStar2Image:dispose();
    self.mStar3Image:dispose();
    self.mStar4Image:dispose();
    self.mStar5Image:dispose();
end

function M:onInit()
    M.super.onInit(self);
	--右侧收缩动画状态
    self.isPlay = false;
    self.password = "111111";
        
    --头像
	self.mAvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mAvatarBtn:addEventHandle(self, self.onAvatarBtnClk);
    self.mLevelBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mLevelBtn:addEventHandle(self, self.onLevelBtnClk);
    self.mLevelText = nil;
    self.mLevelImage = GlobalNS.new(GlobalNS.AuxImage);
    self.mStar1Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mStar2Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mStar3Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mStar4Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mStar5Image = GlobalNS.new(GlobalNS.AuxImage);

    --昵称随机
	self.mNickNameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mNickNameBtn:addEventHandle(self, self.onNickNameBtnClk);
    
    --开始游戏
	self.mStartGameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mStartGameBtn:addEventHandle(self, self.onStartGameBtnClk);
    --炼狱模式
	self.mHardLevelBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mHardLevelBtn:addEventHandle(self, self.onHardLevelBtnClk);
    self.mHardLevelLockedBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mHardLevelLockedBtn:addEventHandle(self, self.onHardLevelLockedBtnClk);
    --团队模式
	self.mTeamGameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mTeamGameBtn:addEventHandle(self, self.onTeamGameBtnClk);
    self.mTeamGameLockedBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mTeamGameLockedBtn:addEventHandle(self, self.onTeamGameLockedBtnClk);

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
   
    --内测总榜排行
    self.mEmailBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mEmailBtn:addEventHandle(self, self.onEmailBtnClk);

    --新手引导
    self.mFirstPlayBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mFirstPlayBtn:addEventHandle(self, self.onFirstPlayBtnClk);

    --账号管理
    self.mAccountBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mAccountBtn:addEventHandle(self, self.onAccountBtnClk);
	
	--商店
	self.mShopBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mShopBtn:addEventHandle(self, self.onShopBtnClk);

    --找朋友
	self.mFriendBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mFriendBtn:addEventHandle(self, self.onFriendBtnClk);

    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mLoginResultDispatch:addEventHandle(nil, nil, 0, self, self.refreshUserInfo, 0);
end

function M:onReady()
    M.super.onReady(self);
    self:initForm(); --初始化组件
    --self:setUsernameAndPassword();--设置用户名密码
    self:setNickName(); --昵称
    GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:SetServerIP();
    self:ShowNoticeMsg();
    self:refreshUserInfo();
end

function M:initForm()
    local bg_image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG_Image");

    --头像
    self.mAvatarBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "AvatarBG");
    self.mAvatarBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "Avatar_BtnTouch"));
    self.mUserName = GlobalNS.UtilApi.getComByPath(self.mAvatarBG, "UserName", "Text");
    local level = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mAvatarBG, "LevelNum_BtnTouch");
    self.mLevelBtn:setSelfGo(level);
    self.mLevelText = GlobalNS.UtilApi.getComByPath(level, "Text", "Text");
    self.Star_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(level, "Star_Panel");
    self.mLevelImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(level, "Dan_Image"));
    self.mStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Star_Panel, "Star1_Image"));
    self.mStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Star_Panel, "Star2_Image"));
    self.mStar3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Star_Panel, "Star3_Image"));
    self.mStar4Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Star_Panel, "Star4_Image"));
    self.mStar5Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Star_Panel, "Star5_Image"));

    self.king_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(level, "king_Panel");
    local kingimg = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.king_Panel, "Image");
    self.king_starnum = GlobalNS.UtilApi.getComByPath(kingimg, "Text", "Text");
    
    --头像
    self.mAvatarBtn.mImage:setSelfGo(self.mAvatarBtn:getSelfGo());
    --self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/"..index..".png", GlobalNS.UtilStr.tostring(index));
	self.mAvatarBtn.mImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(GCtx.mPlayerData.mHeroData.mAvatarIndex));
	--self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(index));

    --账号
    local username = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString(SDK.Lib.SystemSetting.USERNAME);
    if username == nil then
        username = "幽浮大作战";
    end
    self.mUserName.text = username;

    --昵称
    self.mNameBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "NameBG");
    self.mNickNameInput = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mNameBG, "MyName");
    self.inputText = GlobalNS.UtilApi.GetComponent(self.mNickNameInput, "InputField");    
    self.mNickNameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mNameBG, "RandomName_BtnTouch"));
    self.mStartGameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "StartGame_BtnTouch"));
    self.mHardLevelBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "BtGame_BtnTouch"));
    self.mHardLevelLockedBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "BtGameLocked_BtnTouch"));
    self.mHardLevelBtn:hide();
    self.mTeamGameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "TeamGame_BtnTouch"));
    self.mTeamGameLockedBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "TeamGameLocked_BtnTouch"));
    self.mTeamGameBtn:hide();
    
    --功能设置区相关控件
    self.mSettingsPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "SettingsPanel");
    self.mSettingsAnimator = GlobalNS.UtilApi.GetComponent(self.mSettingsPanel, "Animator");

    --收缩按钮
    self.mDropBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mSettingsPanel, "Drop_BtnTouch"));
    self.mDropBtn.mImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mSettingsPanel, "Drop_BtnTouch"));
    --签到按钮
    self.mSignBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mSettingsPanel, "Sign_BtnTouch"));
    --设置按钮
    self.mSettingBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mSettingsPanel, "Setting_BtnTouch"));
    --分享按钮
    self.mShareBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mSettingsPanel, "Share_BtnTouch"));
    self.mAccountBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mSettingsPanel, "Account_BtnTouch"));
    
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
    self.mProgress = GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "Progress");
    self.mProgressSlider = GlobalNS.UtilApi.GetComponent(self.mProgress, "Slider");
    self.mProgress:SetActive(false);

    -- 联系我们
    self.mEmailBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "Email_BtnTouch"));
    self.mFirstPlayBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "FirstPlay_BtnTouch"));
	self.mShopBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "Shop_BtnTouch"));
    self.mFriendBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "Friend_BtnTouch"));
        
    --FlyUFO
    local logo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(bg_image, "Logo");
    --self.mUFOMgr:addUFO(GlobalNS.UtilApi.TransFindChildByPObjAndPath(logo, "FlyUFO"));
    --self.mUFOMgr:addUFO(GlobalNS.UtilApi.TransFindChildByPObjAndPath(logo, "FlyUFO_2"));
    --self.mUFOMgr:addUFO(GlobalNS.UtilApi.TransFindChildByPObjAndPath(logo, "FlyUFO_3"));
    self.mUFOMgr:addUFO(GlobalNS.UtilApi.TransFindChildByPObjAndPath(logo, "FlyUFO_4"));
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

    GCtx.mPlayerData.mHeroData.mMyselfNickName = self.inputText.text;
end

function M:refreshUserInfo()
    self.loginresult = GlobalNS.CSSystem.Ctx.mInstance.mShareData:getLoginResult();
    if self.mIsReady and nil ~= self.loginresult then
        local avatarid = self.loginresult.header_imgid;
        if 0 == avatarid then
            local _time = os.clock();
            math.randomseed(_time);
            avatarid = math.random(1, 4);
        end
        if 0 == GCtx.mPlayerData.mHeroData.mAvatarIndex then--头像被修改过
            GCtx.mPlayerData.mHeroData.mAvatarIndex = avatarid;
        end
        self.mAvatarBtn.mImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(GCtx.mPlayerData.mHeroData.mAvatarIndex));

        self.mUserName.text = self.loginresult.account;
        self.myAccount = self.loginresult.account;
        self.myUid = self.loginresult.uid;
        GCtx.mPlayerData.mHeroData.mMyselfAccount = self.loginresult.account;
        GCtx.mPlayerData.mHeroData.mMyselfUid = self.myUid;
        if 0 == GCtx.mPlayerData.mHeroData.mMyselfLevel then--段位被修改过
            GCtx.mPlayerData.mHeroData.mMyselfLevel = self.loginresult.level;
        end
        
        self:UpdateUnlockedState(GlobalNS.CSSystem.Ctx.mInstance.mShareData:getIsUnlocked());
        self:UpdateTeamGameUnlockedState(GlobalNS.CSSystem.Ctx.mInstance.mShareData:getIsUnlockedTeamGame());
        self:updatelevelinfo();
    end
end

function M:updatelevelinfo()
    if 0 == GCtx.mPlayerData.mHeroData.mMyselfLevel then
        GCtx.mPlayerData.mHeroData.mMyselfLevel = 1;
    end

    local leveldata = LuaExcelManager.level_level[GCtx.mPlayerData.mHeroData.mMyselfLevel];
    self.mLevelText.text = leveldata.name;
    self.mLevelImage:setSpritePath("Atlas/DefaultSkin/Level.asset", leveldata.image);

    if leveldata.type >= 16 then
        self.Star_Panel:SetActive(false);
        self.king_Panel:SetActive(true);
        self.king_starnum.text = leveldata.star;
    else
        self.Star_Panel:SetActive(true);
        self.king_Panel:SetActive(false);
        self.mStar1Image:show();
        self.mStar2Image:show();
        self.mStar3Image:show();
        self.mStar4Image:show();
        self.mStar5Image:show();

        if 2 == leveldata.maxstar then
            self.mStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mStar3Image:hide();
            self.mStar4Image:hide();
            self.mStar5Image:hide();
        elseif 3 == leveldata.maxstar then
            self.mStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.mStar4Image:hide();
            self.mStar5Image:hide();
        elseif 4 == leveldata.maxstar then
            self.mStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.mStar4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
            self.mStar5Image:hide();
        elseif 5 == leveldata.maxstar then
            self.mStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.mStar4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
            self.mStar5Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(5, leveldata.star, leveldata.maxstar));
        else
            
        end
    end
end

function M:UpdateUnlockedState(unlocked)
    if not unlocked then
        self.mHardLevelBtn:hide();
        self.mHardLevelLockedBtn:show();
    else
        self.mHardLevelBtn:show();
        self.mHardLevelLockedBtn:hide();
    end
end

function M:UpdateTeamGameUnlockedState(unlocked)
    if not unlocked then
        self.mTeamGameBtn:hide();
        self.mTeamGameLockedBtn:show();
    else
        self.mTeamGameBtn:show();
        self.mTeamGameLockedBtn:hide();
    end
end

function M:updatePersonalInfo(account)
    self.mUserName.text = account;
    self.myAccount = account;
    GCtx.mPlayerData.mHeroData.mMyselfAccount = account;
end

function M:onShow()
    M.super.onShow(self);
    --GlobalNS.CSSystem.Ctx.mInstance.mLoginModule.mLoginNetNotify:loginGiant();
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mLoginResultDispatch:removeEventHandle(nil, nil, 0, self, self.refreshUserInfo, 0);
    self.mTimer:Stop();
    M.super.onExit(self);
end

function M:LoginOrCreateAccount_new(selectEnterMode)
    if not GCtx.mGameData.isRelogin then
		if(not GlobalNS.UtilApi.IsUObjNil(GlobalNS.CSSystem.Ctx.mInstance.mLoginModule)) then
			GlobalNS.CSSystem.Ctx.mInstance.mLoginModule.mLoginNetNotify:login();
		end
    else
		if(not GlobalNS.UtilApi.IsUObjNil(GlobalNS.CSSystem.Ctx.mInstance.mLoginModule)) then
			GlobalNS.CSSystem.Ctx.mInstance.mLoginModule.mLoginNetNotify:relogin();
		end
    end
end

function M:onAvatarBtnClk()
    if nil ~= self.loginresult then
        GCtx.mPlayerData.mHeroData.mViewAccount = self.myAccount;
        GCtx.mPlayerData.mHeroData.mViewUid = self.myUid;
        local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountPanel);
        form.showPage = 0;
    end
end

function M:onLevelBtnClk()
    if nil ~= self.loginresult then
        GCtx.mPlayerData.mHeroData.mViewAccount = self.myAccount;
        GCtx.mPlayerData.mHeroData.mViewUid = self.myUid;
        local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountPanel);
        form.showPage = 4;
    end
end

function M:onNickNameBtnClk()
    --随机昵称
    self.inputText.text = self:getRandomNickName();
    GCtx.mPlayerData.mHeroData.mMyselfNickName = self.inputText.text;
end

function M:getRandomNickName()
    local _time = os.clock();
    math.randomseed(_time);
    local index = math.random(1, #GCtx.mSocialData.nicknames);
    return GCtx.mSocialData.nicknames[index];
end

function M:enterRoom(gametype)
    GlobalNS.CSSystem.Ctx.mInstance.mSoundMgr:play(0, "Sound/Music/startgame.wav", 1.0, 2, false, false);
    self.nickname = self.inputText.text;

    self.nickname = string.gsub(self.nickname, "^%s*(.-)%s*$", "%1"); --去除两端的空格
    if self.nickname == '' then
        self.nickname = "";
    end

    local isnospaceorper = false;
    isnospaceorper = GlobalNS.UtilApi.GetIsContainKeyword(self.nickname);
    if isnospaceorper then
        GCtx.mGameData:ShowRollMessage("昵称中不能包含<color=#00FF00FF>空格</color>和<color=#00FF00FF>%</color>，请重新设置");
        return;
    end

    local isFilter = GlobalNS.CSSystem.Ctx.mInstance.mWordFilterManager:IsMatch(self.nickname);
    if isFilter then
        GCtx.mGameData:ShowRollMessage("昵称中含有敏感词，请重新设置");
        return;
    end

    if string.len(self.nickname) > 0 and GlobalNS.UtilApi.GetTextWordNum(self.nickname) < 9 then
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setString(SDK.Lib.SystemSetting.NICKNAME, self.nickname);
    else
        GCtx.mGameData:ShowMessageBox("昵称不能为空或多于8个字符(" .. GlobalNS.UtilApi.GetTextWordNum(self.nickname) .. ")");
        return;
    end

    if 0 == gametype then
        self:FirstPlay(gametype);
    else
        self:FirstHardPlay(gametype);
    end
end

function M:FirstPlay(gametype)
    if not GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("FirstPlay") then --新手引导
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("FirstPlay", 1);
        GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIMessagePanel);
        GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIFirstPlayTipPanel);
	elseif(GlobalNS.CSSystem.isEnableGuide()) then
		GlobalNS.CSSystem.execGuide();
    else
        if 1 == GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("FirstPlay") then
            GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIMessagePanel);
            GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIFirstPlayTipPanel);
        else
            --self:loginOrCreateAccount(SDK.Lib.SelectEnterMode.eLoginAccount);
            if not GCtx.mGameData.isRelogin then
                self.mProgress:SetActive(true);

                self.mShopBtn:hide();
                self.mFriendBtn:hide();
                self.mEmailBtn:hide();
            end
	        
            --self:LoginOrCreateAccount_new(SDK.Lib.SelectEnterMode.eLoginAccount);
			if(not GlobalNS.UtilApi.IsUObjNil(GlobalNS.CSSystem.Ctx.mInstance.mLoginModule)) then
				GlobalNS.CSSystem.Ctx.mInstance.mLoginModule.mLoginNetNotify:enterRoom(gametype);
			end
        end
    end
end

function M:FirstHardPlay(gametype)
    if not GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("FirstHardPlay") then --新手引导
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("FirstHardPlay", 1);
        GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIMessagePanel);
        GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIFirstHardPlayTipPanel);
    else
        if 1 == GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("FirstHardPlay") then
            GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIMessagePanel);
            GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIFirstHardPlayTipPanel);
        else
            --self:loginOrCreateAccount(SDK.Lib.SelectEnterMode.eLoginAccount);
            if not GCtx.mGameData.isRelogin then
                self.mProgress:SetActive(true);

                self.mShopBtn:hide();
                self.mFriendBtn:hide();
                self.mEmailBtn:hide();
            end
	        
			if(not GlobalNS.UtilApi.IsUObjNil(GlobalNS.CSSystem.Ctx.mInstance.mLoginModule)) then
				GlobalNS.CSSystem.Ctx.mInstance.mLoginModule.mLoginNetNotify:enterRoom(gametype);
			end
        end
    end
end

function M:onStartGameBtnClk()
    self:enterRoom(0);
end

function M:onHardLevelBtnClk()
    --炼狱模式
    self:enterRoom(1);
end

function M:onHardLevelLockedBtnClk()
    --炼狱模式
    GCtx.mGameData:ShowRollMessage("普通模式获得第一名才解锁炼狱模式哦！");
end

function M:onTeamGameBtnClk()
    --主动组队
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mTeamData:EnterTeam(0, GCtx.mPlayerData.mHeroData.mMyselfNickName);
end

function M:onTeamGameLockedBtnClk()
    --团队模式
    GCtx.mGameData:ShowRollMessage("段位达到金徽章才解锁团队模式哦！");
end

function M:onTick()
    local lefttime = GlobalNS.UtilMath.keepTwoDecimalPlaces(self.mTimer:getLeftRunTime());

    local alpha = lefttime / 0.5;
    GlobalNS.UtilApi.setImageColor(self.mSignBtn:getSelfGo(), 1, 1, 1, alpha);
    GlobalNS.UtilApi.setImageColor(self.mSettingBtn:getSelfGo(), 1, 1, 1, alpha);
    GlobalNS.UtilApi.setImageColor(self.mShareBtn:getSelfGo(), 1, 1, 1, alpha);
    GlobalNS.UtilApi.setImageColor(self.mAccountBtn:getSelfGo(), 1, 1, 1, alpha);
    
    if lefttime <= 0 then
        self.mCanClick = true;
        self.isPlay = true;
    end
end

function M:onDropBtnClk()
    if(not self.isPlay) then
        self.mSignBtn:hide();
        self.mSettingBtn:hide();
        self.mShareBtn:hide();
        self.mAccountBtn:hide();
        
        self.mDropBtn.mImage:setSpritePath("Atlas/DefaultSkin/StartGame.asset", "btn_down");
		
		--GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIConsoleDlg);
        self.isPlay = true;
    else
        self.mSignBtn:show();
        self.mSettingBtn:show();
        self.mShareBtn:show();
        self.mAccountBtn:show();

        self.mDropBtn.mImage:setSpritePath("Atlas/DefaultSkin/StartGame.asset", "btn_up");

        self.isPlay = false;
    end
end

function M:onSignBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUISignPanel);
    --GCtx.mGameData:ShowRollMessage("暂未开放");
end

function M:onSettingBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUISettingsPanel);
end

function M:onShareBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIShareMoney);
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
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIShop);
end

function M:onFriendBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIFindFriend);
end

function M:onEmailBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIServerHistoryRankListPanel);
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:GetPurgatoryRank(0);
end

function M:onFirstPlayBtnClk()
    GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("FirstPlay", 1);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIMessagePanel);
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIFirstPlayTipPanel);
end

function M:onAccountBtnClk()
    -- GCtx.mGameData:ShowRollMessage("账号管理");
	if(not GlobalNS.UtilApi.IsUObjNil(GlobalNS.CSSystem.Ctx.mInstance.mLoginModule)) then
		GlobalNS.CSSystem.Ctx.mInstance.mLoginModule.mLoginNetNotify:enterUserCenter();
	end
	--GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUITeamBattlePanel);
end

function M:setProgress(value)
    if value > 0.96 then
        value = 0.96;
    end

	self.mProgressSlider.value = value;
end

function M:resetAvatar(index)
	self.mAvatarBtn.mImage:setSelfGo(self.mAvatarBtn:getSelfGo());
	--self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/"..index..".png", GlobalNS.UtilStr.tostring(index));
	self.mAvatarBtn.mImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(index));
    GCtx.mPlayerData.mHeroData.mAvatarIndex = index;
    --self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(index));
end

function M:ShowNoticeMsg()
    local times = 0;
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("NoticeTimes") then
        times = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("NoticeTimes");
    end
    
    if GlobalNS.CSSystem.Ctx.mInstance.mShareData.noticeTimes > times then
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("NoticeTimes", times+1);
        
        local msg = string.gsub(GlobalNS.CSSystem.Ctx.mInstance.mShareData.noticeMsg, "\\n", "\n");
        GCtx.mGameData:ShowMessageBox(msg);
    end
end

return M;