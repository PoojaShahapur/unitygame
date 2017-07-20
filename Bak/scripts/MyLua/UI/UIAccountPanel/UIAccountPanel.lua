MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIAccountPanel.AccountPanelNS");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelData");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelCV");
MLoader("MyLua.UI.UIAccountPanel.Panel.GameShowPanel");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIAccountPanel";
GlobalNS.AccountPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIAccountPanel;
	self.mData = GlobalNS.new(GlobalNS.AccountPanelNS.AccountPanelData);
    self.index = 1; --头像索引
    self.username = "幽浮大作战";
    self.sex = 1; --0:M,1:W
    self.age = 20;
    self.area_code = 1;
    self.cur_open = 0;
    self.sign = "";
    self.signSuc = false;
    self.showleveltype = 1;
    self.state = 0;
    self.maxlevel = 1;
    self.showlevellist = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.showlevellist:setIsSpeedUpFind(true);
	self.showlevellist:setIsOpKeepSort(true);
    self.showPage = 0;

    self.mTabPanel = GlobalNS.new(GlobalNS.TabPageMgr);
    self.mTabPanel.mTabClickEventDispatch:addEventHandle(self, self.onTabClick);
end

function M:dtor()
	self.showlevellist:clear();
end

function M:onInit()
    M.super.onInit(self);
	
    self.mAvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mAvatarBtn:addEventHandle(self, self.onAvatarBtnClk);

	self.mCloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCloseBtn:addEventHandle(self, self.onCloseBtnClk);

    self.mBackBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBackBtn:addEventHandle(self, self.onBackBtnClk);

    --对应Clk事件已在Table中注册
    --self.mAccountBtn = GlobalNS.new(GlobalNS.AuxButton);
    --self.mGameDataBtn = GlobalNS.new(GlobalNS.AuxButton);
    --self.mGameShowBtn = GlobalNS.new(GlobalNS.AuxButton);
    --self.mChatBtn = GlobalNS.new(GlobalNS.AuxButton);

    self.mFocusBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mFocusBtn:addEventHandle(self, self.onFocusBtnClk);

    self.mFansBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mFansBtn:addEventHandle(self, self.onFansBtnClk);

    self.mEditBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mEditBtn:addEventHandle(self, self.onEditBtnClk);

    self.mJoinRoomBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mJoinRoomBtn:addEventHandle(self, self.onJoinRoomBtnClk);
    self.mNewFocusBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mNewFocusBtn:addEventHandle(self, self.onFocusBtnClk);

    --数据区的头像img
    self.mGDAvatarImage = GlobalNS.new(GlobalNS.AuxImage);
    self.mSexImage = GlobalNS.new(GlobalNS.AuxImage);

    --段位区
    self.mLeftBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mLeftBtn:addEventHandle(self, self.onLeftBtnClk);

    self.mRightBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mRightBtn:addEventHandle(self, self.onRightBtnClk);

    self.mAwardBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mAwardBtn:addEventHandle(self, self.onAwardBtnClk);

    self.mDanImage = GlobalNS.new(GlobalNS.AuxImage);
    self.mStar1Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mStar2Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mStar3Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mStar4Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mStar5Image = GlobalNS.new(GlobalNS.AuxImage);
    self.maxlevel = #LuaExcelManager.level_level;

    for i = 1, self.maxlevel do
        local item = LuaExcelManager.level_level[i];
        if item.type < 17 then
            if (not self.showlevellist:ContainsKey(item.type)) then
	    	    self.showlevellist:add(item.type, item);
	        end
        end
    end
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");    
	self.mCloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Close_BtnTouch"));
    self.mBackBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Back_BtnTouch"));
    self.mBackBtn:hide();

    --账户
    self:initPersonalInfoCom(BG);
    
    --游戏数据
    self:initGameDataCom(BG);

    --游戏展示
    self:initGameShowCom(BG);

    --聊天
    self:initChatShowCom(BG);

    --段位
    self:initDantitlesShowCom(BG);

    --默认打开账户页
    if 0 == self.showPage then
        self:OpenPage(0);
    end
    if 4 == self.showPage then
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ReqMainPageInfo(GCtx.mPlayerData.mHeroData.mViewUid); --先请求数据
        self:OpenPage(4);
    end
end

function M:OpenPage(tag)
    self.mTabPanel:openPage(tag);
end

function M:initPersonalInfoCom(BG)
    local PersonalInfo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "PersonalInfo");
    local Avatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(PersonalInfo, "Avatar");
    self.mAvatarBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "Avatar_BtnTouch"));
    local Info = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "Info");

    local page = self.mTabPanel:addTabPage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Account_Btn"), PersonalInfo);
    page:setTag(0);

    self.ViewedNum = GlobalNS.UtilApi.getComByPath(Info, "ViewedNum", "Text");
    self.Name = GlobalNS.UtilApi.getComByPath(Info, "Name", "Text");
    local username = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString(SDK.Lib.SystemSetting.USERNAME);
    if username == nil then
        username = "幽浮大作战";
    end
    self.Name.text = username;
    self.mSexImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Info, "Sex"));
    self.Age = GlobalNS.UtilApi.getComByPath(Info, "Age", "Text");

    self.Sign = GlobalNS.UtilApi.getComByPath(Info, "Sign", "InputField");
    GlobalNS.UtilApi.addInputEndHandle(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Info, "Sign"), self, self.onSignInputEnd);

    self.mFocusBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Info, "Focus_Btn"));
    self.mFansBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Info, "Fans_Btn"));
    self.mEditBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "Edit_BtnTouch"));
    self.mJoinRoomBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "JoinRoom_BtnTouch"));
    self.mNewFocusBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "Foucs_BtnTouch"));

    if not GCtx.mPlayerData.mHeroData:isMyself() then
        self.mJoinRoomBtn:show();
        self.mEditBtn:hide();
        self.mNewFocusBtn:show();
    else
        self.mEditBtn:show();
        self.mJoinRoomBtn:hide();
        self.mNewFocusBtn:hide();
    end
end

function M:initGameDataCom(BG)
    local Game_Data = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "GameData");
    local Avatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Game_Data, "Avatar");
    self.mGDAvatarImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "AvatarImg"));
    local Info = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "Info");
    
    local page = self.mTabPanel:addTabPage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "GameData_Btn"), Game_Data);
    page:setTag(1);

    self.GDName = GlobalNS.UtilApi.getComByPath(Info, "Name", "Text");
    self.GDGameTimes = GlobalNS.UtilApi.getComByPath(Info, "GameTimes", "Text");
    self.GDGameNo1 = GlobalNS.UtilApi.getComByPath(Info, "GameNo1", "Text");
    self.GDGameMVP = GlobalNS.UtilApi.getComByPath(Info, "GameMVP", "Text");
    self.GDKillPlayerNum = GlobalNS.UtilApi.getComByPath(Info, "KillPlayerNum", "Text");
    self.GDKillPlaneNum = GlobalNS.UtilApi.getComByPath(Info, "KillPlaneNum", "Text");
    self.GDHistoryMaxScore = GlobalNS.UtilApi.getComByPath(Info, "HistoryMaxScore", "Text");
    self.GDHistoryMaxMegKill = GlobalNS.UtilApi.getComByPath(Info, "HistoryMaxMegKill", "Text");
end

function M:initGameShowCom(BG)
    local Game_Show = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "GameShow");
    local page = self.mTabPanel:addTabPage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "GameShow_Btn"), Game_Show, GlobalNS.AccountPanelNS.GameShowPanel);
    page:setTag(2);
	page:setGuiWin(self.mGuiWin);
end

function M:initChatShowCom(BG)
    local Chat_Show = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "ChatShow");
    local page = self.mTabPanel:addTabPage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Chat_Btn"), Chat_Show);
    page:setTag(3);
end

function M:initDantitlesShowCom(BG)
    local Dantitles_Show = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "DantitlesShow");
    local Main_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dantitles_Show, "Main_Panel");
    self.Explain_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dantitles_Show, "Explain_Panel");
    local page = self.mTabPanel:addTabPage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Dantitles_Btn"), Dantitles_Show);
    page:setTag(4);
    
    local Dan_Image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Main_Panel, "Dan_Image");
    self.DS_Danname = GlobalNS.UtilApi.getComByPath(Main_Panel, "Danname", "Text");
    self.CurrentDan = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Main_Panel, "CurrentDan");
    self.mLeftBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Main_Panel, "Left_Btn"));
    self.mRightBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Main_Panel, "Right_Btn"));
    self.mDanImage:setSelfGo(Dan_Image);
    self.mRuleText = GlobalNS.UtilApi.getComByPath(self.Explain_Panel, "rendanText", "Text");
    self.mTeamRuleText = GlobalNS.UtilApi.getComByPath(self.Explain_Panel, "teamtext", "Text");
    self.mAwardBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Explain_Panel, "AwardButton"));
    self.Twostar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Image, "Twostar_Panel");
    self.Threestar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Image, "Threestar_Panel");
    self.Fourstar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Image, "Fourstar_Panel");
    self.Fivestar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Image, "Fivestar_Panel");
    self.King_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Image, "King_Panel");

    if not GCtx.mPlayerData.mHeroData:isMyself() then
        self.Explain_Panel:SetActive(false);
        self.mLeftBtn:hide();
        self.mRightBtn:hide();
    else
        self.Explain_Panel:SetActive(true);
        self.mLeftBtn:show();
        self.mRightBtn:show();
    end
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    self.mAvatarBtn:dispose();
	self.mCloseBtn:dispose();
    self.mBackBtn:dispose();
    --self.mAccountBtn:dispose();
    --self.mGameDataBtn:dispose();
    --self.mGameShowBtn:dispose();
    self.mFocusBtn:dispose();
    self.mNewFocusBtn:dispose();
    self.mFansBtn:dispose();
    self.mGDAvatarImage:dispose();
    self.mSexImage:dispose();
    self.mJoinRoomBtn:dispose();
    self.mLeftBtn:dispose();
    self.mRightBtn:dispose();
    self.mAwardBtn:dispose();
    self.mDanImage:dispose();
    self.mStar1Image:dispose();
    self.mStar2Image:dispose();
    self.mStar3Image:dispose();
    self.mStar4Image:dispose();
    self.mStar5Image:dispose();

    if(nil ~= self.mTabPanel) then
        self.mTabPanel.mTabClickEventDispatch:removeEventHandle(self, self.onTabClick);
		GlobalNS.delete(self.mTabPanel);
		self.mTabPanel = nil;
	end

    GCtx.mGameData:clearlevellist();

    M.super.onExit(self);
end

function M:onSignInputEnd(text)
    local num = GlobalNS.UtilApi.GetTextWordNum(text);
    self.signSuc = true;
    if num > 24 then
         GCtx.mGameData:ShowRollMessage("签名不能多于24个字符(" .. GlobalNS.UtilApi.GetTextWordNum(text) .. ")");
         self.signSuc = false;
    end

    local isFilter = GlobalNS.CSSystem.Ctx.mInstance.mWordFilterManager:IsMatch(text);
    if isFilter then
         GCtx.mGameData:ShowRollMessage("签名中含有敏感词，请重新设置");
         self.signSuc = false;
    end

    local isnospaceorper = false;
    isnospaceorper = GlobalNS.UtilApi.GetIsContainKeyword(text);
    if isnospaceorper then
        GCtx.mGameData:ShowRollMessage("签名中不能包含<color=#00FF00FF>空格</color>和<color=#00FF00FF>%</color>，请重新设置");
        self.signSuc = false;
    end
end

function M:onTabClick(dispObj)
	local tag = dispObj:getCurPageTag();
    self.cur_open = tag;
    if 0 == tag then --账号
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ReqMainPageInfo(GCtx.mPlayerData.mHeroData.mViewUid);
    elseif 1 == tag then--数据
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ViewGamePageInfo(GCtx.mPlayerData.mHeroData.mViewAccount);
    elseif 2 == tag then--展示
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ViewSkinPageInfo(GCtx.mPlayerData.mHeroData.mViewAccount);
    elseif 3 == tag then--聊天

    elseif 4 == tag then--段位
        local level = GCtx.mPlayerData.mHeroData.mViewLevel;
        if 0 == level then
            level = 1;
        end
        self:updateDantitlesInfo(level);
        self.showleveltype = LuaExcelManager.level_level[level].type;
        self.CurrentDan:SetActive(true);
    else
        
    end
end

function M:onCloseBtnClk()
    if self.signSuc and self.sign ~= self.Sign.text then
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ChangeSign(self.Sign.text);
    end
	self:exit();
end

function M:onBackBtnClk()

end

function M:onAvatarBtnClk()
    if GCtx.mPlayerData.mHeroData:isMyself() then
        GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountAvatarPanel);
    end
end

function M:onFocusBtnClk()
    if 1 == self.areadyfllowed then
        local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIConfirmAgain);
	    form:addOkEventHandle(self, self.onOkHandle);
        form:setDesc("是否取消关注该玩家？");
    else
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:Follow(GCtx.mPlayerData.mHeroData.mViewUid);
    end
end

function M:onOkHandle(dispObj)
	GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:UnFollow(GCtx.mPlayerData.mHeroData.mViewUid);
end

function M:onFansBtnClk()
    --[[if GCtx.mPlayerData.mHeroData:isMyself() then
        GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIFindFriend);
        GCtx.mSocialData.mCurOpenTag = 2;
        self:exit();
    end]]--
end

function M:onEditBtnClk()
    if GCtx.mPlayerData.mHeroData:isMyself() then
        GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIEditInfoPanel);
    end
end

function M:onJoinRoomBtnClk()
    if self.state ~= 2 then
        GCtx.mGameData:ShowRollMessage("该玩家目前不在战斗中，无法加入");
        return;
    end
    if not GCtx.mPlayerData.mHeroData:isMyself() then
		if(not GlobalNS.UtilApi.IsUObjNil(GlobalNS.CSSystem.Ctx.mInstance.mLoginModule)) then
			GlobalNS.CSSystem.Ctx.mInstance.mLoginModule.mLoginNetNotify:JoinRoom(GCtx.mPlayerData.mHeroData.mViewUid, GCtx.mPlayerData.mHeroData.mMyselfNickName);
		end
    end
end

function M:onLeftBtnClk()
    if self.showleveltype > 1 then
        self.showleveltype = self.showleveltype - 1;
        local level = self.showlevellist:value(self.showleveltype).id;
        if self.showleveltype == LuaExcelManager.level_level[GCtx.mPlayerData.mHeroData.mViewLevel].type then
            level = GCtx.mPlayerData.mHeroData.mViewLevel;
            self.CurrentDan:SetActive(true);
        else
            self.CurrentDan:SetActive(false);
        end
        self:updateDantitlesInfo(level);
    end
end

function M:onRightBtnClk()
    if self.showlevellist:count() > self.showleveltype then
        self.showleveltype = self.showleveltype + 1;
        local level = self.showlevellist:value(self.showleveltype).id;
        if self.showleveltype == LuaExcelManager.level_level[GCtx.mPlayerData.mHeroData.mViewLevel].type then
            level = GCtx.mPlayerData.mHeroData.mViewLevel;
            self.CurrentDan:SetActive(true);
        else
            self.CurrentDan:SetActive(false);
        end
        self:updateDantitlesInfo(level);
    end
end

function M:onAwardBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIgiftPanel);
    if not GCtx.mGameData.isgetleveldata then
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:GetSeasonReward();
    end
end

function M:resetAvatar(index)
    self.index = index;
	self.mAvatarBtn.mImage:setSelfGo(self.mAvatarBtn:getSelfGo());
	--self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/"..self.index..".png", GlobalNS.UtilStr.tostring(self.index));
	self.mAvatarBtn.mImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(self.index));
    --self.mAvatarBtn.mImage:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(self.index));
end

function M:getPersonalInfo()
    return self.index, self.username, self.sex, self.age, self.area_code;
end

function M:updatePersonalAccount(account)
    self.username = account;
    self.Name.text = account;
end

function M:updatePersonalInfo(sex, age, area_code, viewed_num)
    self.sex = sex;
    self.age = age;
    self.area_code = area_code;

    if nil ~= viewed_num then
        self.ViewedNum.text = "总浏览：" .. viewed_num;
    end

    self.Age.text = age .. "岁 /" .. GCtx.mSocialData.citys[area_code];
    if 0 == sex then
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
    else
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
    end
end

function M:serverNofityPersonalInfo(args)
    if self.cur_open ~= 0 and self.cur_open ~= 4 then
        return;
    end

    local account = args[0];
    local sex = args[1];
    local age = args[2];
    local area_code = args[3];
    local sign = args[4];
    local following_num = args[5];
    local followed_num = args[6];
    local viewed_num = args[7];
    local header_imgid = args[8];
    local level = args[9];
    local areadyfllowed = args[10];
    self.state = args[11];

    if 0 == area_code then
        area_code = 1;
    end
    
    self.username = account;
    self.sex = sex;
    self.age = age;
    self.area_code = area_code;
    self.sign = sign;
    self:updatePersonalAccount(account);
    self:updatePersonalInfo(sex, age, area_code, viewed_num);
    self.mFocusBtn:setText("关注（" .. following_num .. "）");
    self.mFansBtn:setText("粉丝（" .. followed_num .. "）");
    self.mAvatarBtn.mImage:setSelfGo(self.mAvatarBtn:getSelfGo());
    self.mAvatarBtn.mImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(header_imgid));
    self.Sign.text = sign;
    if level > self.maxlevel then
        level = self.maxlevel;
    end
    if 0 == level then
        level = 1;
    end 
    GCtx.mPlayerData.mHeroData.mViewLevel = level;

    if GCtx.mPlayerData.mHeroData:isMyself() then
        self.mAvatarBtn:enable();
    else
        self.mAvatarBtn:disable();
    end

    self:updateFocusBtnState(areadyfllowed);
    self:updateDantitlesInfo(level);
    self.showleveltype = LuaExcelManager.level_level[level].type;
end

function M:updateFocusBtnState(areadyfllowed)
    self.areadyfllowed = areadyfllowed;
    if 1 == areadyfllowed then
        self.mNewFocusBtn:setText("已关注");
    else
        self.mNewFocusBtn:setText("关注");
    end
end

function M:serverNofityGameData(args)
    if self.cur_open ~= 1 then
        return;
    end

    local total_game = args[0];
    local total_champion = args[1];
    local total_mvp = args[2];
    local total_destroy = args[3];
    local total_kill = args[4];
    local highest_score = args[5];
    local highest_combo = args[6];
    local header_imgid = args[7];
    local account = args[8];
    
    self.mGDAvatarImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(header_imgid));
    self.GDName.text = "" .. account;
    self.GDGameTimes.text =  "" .. total_game;
    self.GDGameNo1.text =  "" .. total_champion;
    self.GDGameMVP.text =  "" .. total_mvp;
    self.GDKillPlayerNum.text =  "" .. total_destroy;
    self.GDKillPlaneNum.text =  "" .. total_kill;
    self.GDHistoryMaxScore.text =  "" .. highest_score;
    self.GDHistoryMaxMegKill.text =  "" .. highest_combo;
end

function M:updateDantitlesInfo(level)
    if self.cur_open ~= 4 then
        return;
    end
    if level > self.maxlevel then
        level = self.maxlevel;
    end
    if 0 == level then
        level = 1;
    end

    local leveldata = LuaExcelManager.level_level[level];

    if GCtx.mPlayerData.mHeroData:isMyself() then
        if leveldata.type == 1 then
            self.mLeftBtn:hide();
            self.mRightBtn:show();
        elseif leveldata.type == self.showlevellist:count() then
            self.mLeftBtn:show();
            self.mRightBtn:hide();
        else
            self.mLeftBtn:show();
            self.mRightBtn:show();
        end
    end

    self.mDanImage:setSpritePath("Atlas/DefaultSkin/Level.asset", leveldata.image);
    self.DS_Danname.text = leveldata.name;
    self.mRuleText.text = leveldata.rule;
    self.mTeamRuleText.text = leveldata.teamrule;
    
    if leveldata.type >= 16 then
        self.Twostar_Panel:SetActive(false);
        self.Threestar_Panel:SetActive(false);
        self.Fourstar_Panel:SetActive(false);
        self.Fivestar_Panel:SetActive(false);
        self.King_Panel:SetActive(true);

        local image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.King_Panel, "Image");
        local startext = GlobalNS.UtilApi.getComByPath(image, "Text", "Text");
        startext.text = leveldata.star;
    else
        if 2 == leveldata.maxstar then
            self.Twostar_Panel:SetActive(true);
            self.Threestar_Panel:SetActive(false);
            self.Fourstar_Panel:SetActive(false);
            self.Fivestar_Panel:SetActive(false);
            self.King_Panel:SetActive(false);
            self.mStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Twostar_Panel, "star1"));
            self.mStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Twostar_Panel, "star2"));
    
            self.mStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
        elseif 3 == leveldata.maxstar then
            self.Twostar_Panel:SetActive(false);
            self.Threestar_Panel:SetActive(true);
            self.Fourstar_Panel:SetActive(false);
            self.Fivestar_Panel:SetActive(false);
            self.King_Panel:SetActive(false);
            self.mStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Threestar_Panel, "star1"));
            self.mStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Threestar_Panel, "star2"));
            self.mStar3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Threestar_Panel, "star3"));
    
            self.mStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
        elseif 4 == leveldata.maxstar then
            self.Twostar_Panel:SetActive(false);
            self.Threestar_Panel:SetActive(false);
            self.Fourstar_Panel:SetActive(true);
            self.Fivestar_Panel:SetActive(false);
            self.King_Panel:SetActive(false);
            self.mStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Fourstar_Panel, "star1"));
            self.mStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Fourstar_Panel, "star2"));
            self.mStar3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Fourstar_Panel, "star3"));
            self.mStar4Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Fourstar_Panel, "star4"));
    
            self.mStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.mStar4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
        elseif 5 == leveldata.maxstar then
            self.Twostar_Panel:SetActive(false);
            self.Threestar_Panel:SetActive(false);
            self.Fourstar_Panel:SetActive(false);
            self.Fivestar_Panel:SetActive(true);
            self.King_Panel:SetActive(false);
            self.mStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Fivestar_Panel, "star1"));
            self.mStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Fivestar_Panel, "star2"));
            self.mStar3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Fivestar_Panel, "star3"));
            self.mStar4Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Fivestar_Panel, "star4"));
            self.mStar5Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Fivestar_Panel, "star5"));
    
            self.mStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.mStar4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
            self.mStar5Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(5, leveldata.star, leveldata.maxstar));
        else
    
        end
    end
end

function M:serverSkinPageInfo(account, args)
    Debugger.Log(account .. " " .. args[0]);
end

return M;