--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "FansItem";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.m_go = nil;
    self.index = 0;
    self.account = "";
    self.mSexImage = nil;
    self.mLevelImage = nil;
    self.m_FocusBtn = nil;
    self.m_AvatarImage = nil;
    self.m_AvatarBtn = nil;
    self.m_ImageClk = nil;
    self.m_Account = nil;
    self.m_Level = nil;
    self.m_FansNum = nil;
    self.mFriendImage = nil;
    self.account = "";
    self.already_follow = 1;
    self.uid = 0;
    self.m_Star1Image = nil;
    self.m_Star2Image = nil;
    self.m_Star3Image = nil;
    self.m_Star4Image = nil;
    self.m_Star5Image = nil;
end

function M:dtor()
    
end

function M:dispose()
    if GlobalNS.UtilApi.IsUObjNil(self.m_go) then
        GlobalNS.UtilApi.Destroy(self.m_go);
    end
    if self.m_FocusBtn ~= nil then
        self.m_FocusBtn:dispose();
        self.m_FocusBtn = nil;
    end
    if self.m_AvatarBtn ~= nil then
        self.m_AvatarBtn:dispose();
        self.m_AvatarBtn = nil;
    end
    if self.m_AvatarImage ~= nil then
        self.m_AvatarImage:dispose();
        self.m_AvatarImage = nil;
    end
    if self.mSexImage ~= nil then
        self.mSexImage:dispose();
        self.mSexImage = nil;
    end
    if self.mLevelImage ~= nil then
        self.mLevelImage:dispose();
        self.mLevelImage = nil;
    end
    if self.mFriendImage ~= nil then
        self.mFriendImage:dispose();
        self.mFriendImage = nil;
    end
    if self.m_ImageClk ~= nil then
        self.m_ImageClk:dispose();
        self.m_ImageClk = nil;
    end
    if self.m_Star1Image ~= nil then
        self.m_Star1Image:dispose();
        self.m_Star1Image = nil;
    end
    if self.m_Star2Image ~= nil then
        self.m_Star2Image:dispose();
        self.m_Star2Image = nil;
    end
    if self.m_Star3Image ~= nil then
        self.m_Star3Image:dispose();
        self.m_Star3Image = nil;
    end
    if self.m_Star4Image ~= nil then
        self.m_Star4Image:dispose();
        self.m_Star4Image = nil;
    end
    if self.m_Star5Image ~= nil then
        self.m_Star5Image:dispose();
        self.m_Star5Image = nil;
    end
end

function M:init(_Prefab,  _Content, _index)
    self.m_go = GlobalNS.UtilApi.Instantiate(_Prefab);
    --self.m_go.transform.parent = _Content;
	GlobalNS.CSSystem.SetParent(self.m_go.transform, _Content);
    self.m_go.transform.localScale = Vector3.New(1.0, 1.0, 1.0);
    self.m_go.name = "FansItem" .. _index;
    self.index = _index;

    self.m_AvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.m_AvatarBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Image_touxiang"));
    self.m_AvatarBtn:addEventHandle(self, self.onAvatarBtnClk);
    self.m_AvatarImage = GlobalNS.new(GlobalNS.AuxImage);
    self.m_AvatarImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Image_touxiang"));

    self.m_Account = GlobalNS.UtilApi.getComByPath(self.m_go, "Text_name", "Text");
    
    self.mSexImage = GlobalNS.new(GlobalNS.AuxImage);
    self.mSexImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Image_sex"));
    if 0 == sex then
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
    else
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
    end

    --段位
    local Dan_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Dan_Panel")
    self.m_Level = GlobalNS.UtilApi.getComByPath(Dan_Panel, "Text", "Text");
    self.mLevelImage = GlobalNS.new(GlobalNS.AuxImage);
    self.mLevelImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Panel, "Image_duanwei"));
    self.Star_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Panel, "Star_Panel");
    self.king_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Panel, "king_Panel");
    self.m_Star1Image = GlobalNS.new(GlobalNS.AuxImage);
    self.m_Star1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Star_Panel, "Star1_Image"));
    self.m_Star2Image = GlobalNS.new(GlobalNS.AuxImage);
    self.m_Star2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Star_Panel, "Star2_Image"));
    self.m_Star3Image = GlobalNS.new(GlobalNS.AuxImage);
    self.m_Star3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Star_Panel, "Star3_Image"));
    self.m_Star4Image = GlobalNS.new(GlobalNS.AuxImage);
    self.m_Star4Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Star_Panel, "Star4_Image"));
    self.m_Star5Image = GlobalNS.new(GlobalNS.AuxImage);
    self.m_Star5Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Star_Panel, "Star5_Image"));
    local kingImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.king_Panel, "Image");
    self.starnum = GlobalNS.UtilApi.getComByPath(kingImage, "Text", "Text");

    self.m_FansNum = GlobalNS.UtilApi.getComByPath(self.m_go, "Text_Fans", "Text");

    self.mFriendImage = GlobalNS.new(GlobalNS.AuxImage);
    self.mFriendImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Image_Friend"));

    self.m_FocusBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.m_FocusBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Button_focus"));
    self.m_FocusBtn:addEventHandle(self, self.onFocusBtnClk);

    self.m_ImageClk = GlobalNS.new(GlobalNS.AuxButton);
    self.m_ImageClk:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Image_Clk"));
    self.m_ImageClk:addEventHandle(self, self.onImageClk);
end

function M:updateValue(account, header_imgid, sex, level, state, already_follow, uid)
    self.m_AvatarImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(header_imgid));
    self.m_Account.text = account;
    self.account = account;
    self.uid = uid;
    if 0 == sex then
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
    else
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
    end

    local maxlevel = #LuaExcelManager.level_level;
    if level > maxlevel then
        level = maxlevel;
    end
    if 0 == level then
        level = 1;
    end

    local levelname = {self:getLevelName(level)};
    self.m_Level.text = levelname[1];
    self.mLevelImage:setSpritePath("Atlas/DefaultSkin/Level.asset", levelname[2]);

    self.already_follow = already_follow;
    if 1 == already_follow then--我也粉了她
        --self.mFriendImage:show();
        self.m_FocusBtn:setText("已互关");
    else
        --self.mFriendImage:hide();
        self.m_FocusBtn:setText("关注");
    end
    --self.m_FansNum.text = "关注" .. focusnum .. "/粉丝" .. fansnum;

    local leveldata = LuaExcelManager.level_level[level];
    if leveldata.type >= 16 then
        self.Star_Panel:SetActive(false);
        self.king_Panel:SetActive(true);
        self.starnum.text = leveldata.star;
    else
        self.Star_Panel:SetActive(true);
        self.king_Panel:SetActive(false);

        self.m_Star1Image:show();
        self.m_Star2Image:show();
        self.m_Star3Image:show();
        self.m_Star4Image:show();
        self.m_Star5Image:show();

        if 2 == leveldata.maxstar then
            self.m_Star1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.m_Star2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.m_Star3Image:hide();
            self.m_Star4Image:hide();
            self.m_Star5Image:hide();
        elseif 3 == leveldata.maxstar then
            self.m_Star1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.m_Star2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.m_Star3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.m_Star4Image:hide();
            self.m_Star5Image:hide();
        elseif 4 == leveldata.maxstar then
            self.m_Star1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.m_Star2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.m_Star3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.m_Star4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
            self.m_Star5Image:hide();
        elseif 5 == leveldata.maxstar then
            self.m_Star1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.m_Star2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.m_Star3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.m_Star4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
            self.m_Star5Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(5, leveldata.star, leveldata.maxstar));
        else
            
        end
    end
end

function M:getLevelName(level)
    if 0 == level then
        level = 1;
    end
    local name = "无";
    local imgName = "1";
    local leveldata = LuaExcelManager.level_level[level]; 
    name = leveldata.name;
    imgName = leveldata.image;
    return name, imgName;
end

--关注
function M:onFocusBtnClk()
	if 1 == self.already_follow then
        local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIConfirmAgain);
	    form:addOkEventHandle(self, self.onOkHandle);
        form:setDesc("是否取消关注该玩家？");
    else
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:Follow(self.uid);
    end
end

function M:onOkHandle(dispObj)
	GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:UnFollow(self.uid);
end

--查看
function M:onAvatarBtnClk()
    GCtx.mPlayerData.mHeroData.mViewAccount = self.account;
    GCtx.mPlayerData.mHeroData.mViewUid = self.uid;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountPanel);
    --GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIFindFriend);
end

--查看
function M:onImageClk()
    GCtx.mPlayerData.mHeroData.mViewAccount = self.account;
    GCtx.mPlayerData.mHeroData.mViewUid = self.uid;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountPanel);
    --GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIFindFriend);
end

return M;

--endregion
