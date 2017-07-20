--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "FriendItem";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.m_go = nil;
    self.index = 0;
    self.account = "";
    self.m_AvatarImage = nil;
    self.m_SexImage = nil;
    self.m_LevelImage = nil;
    self.m_AvatarBtn = nil;
    self.m_InviteBtn = nil;
    self.state = 1;
    self.uid = 0;

    self.mTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);--邀请冷却
    self.mTimer:setTotalTime(10);
    self.mTimer:setFuncObject(self, self.onTick);
end

function M:dtor()
    
end

function M:onTick()
	local lefttime = GlobalNS.UtilMath.ceil(self.mTimer:getLeftRunTime());
    if lefttime <= 0 then
        self.m_InviteBtn:enable();
    else
    end
end

function M:dispose()
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
    if self.m_LevelImage ~= nil then
        self.m_LevelImage:dispose();
        self.m_LevelImage = nil;
    end
    if self.m_InviteBtn ~= nil then
        self.m_InviteBtn:dispose();
        self.m_InviteBtn = nil;
    end

    self.m_Star1Image:dispose();
    self.m_Star2Image:dispose();
    self.m_Star3Image:dispose();
    self.m_Star4Image:dispose();
    self.m_Star5Image:dispose();

    if GlobalNS.UtilApi.IsUObjNil(self.m_go) then
        GlobalNS.UtilApi.Destroy(self.m_go);
    end

    if nil ~= self.mTimer then
        self.mTimer:Stop();
        GlobalNS.delete(self.mTimer);
    	self.mTimer = nil;
    end
end

function M:init(_Prefab,  _Content, _index)
    self.m_go = GlobalNS.UtilApi.Instantiate(_Prefab);
	GlobalNS.CSSystem.SetParent(self.m_go.transform, _Content);
    self.m_go.transform.localScale = Vector3.New(1.0, 1.0, 1.0);
    self.m_go.name = "FansItem" .. _index;
    self.index = _index;

    self.m_AvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.m_AvatarBtn:setSelfGo(self.m_go);
    self.m_AvatarBtn:addEventHandle(self, self.onAvatarBtnClk);
    self.m_AvatarBtn.mImage:setSelfGo(self.m_go);
    self.m_AvatarImage = GlobalNS.new(GlobalNS.AuxImage);
    self.m_AvatarImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Player_Image"));

    self.m_Account = GlobalNS.UtilApi.getComByPath(self.m_go, "name", "Text");
    
    self.mSexImage = GlobalNS.new(GlobalNS.AuxImage);
    self.mSexImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Sex_Image"));
    if 0 == sex then
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
    else
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
    end

    self.m_State = GlobalNS.UtilApi.getComByPath(self.m_go, "online", "Text");

    self.m_InviteBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.m_InviteBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Button"));
    self.m_InviteBtn:addEventHandle(self, self.onInviteBtnClk);

    --段位
    local Dan_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.m_go, "Dan_Panel")
    self.m_Level = GlobalNS.UtilApi.getComByPath(Dan_Panel, "Text", "Text");
    self.m_LevelImage = GlobalNS.new(GlobalNS.AuxImage);
    self.m_LevelImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Panel, "Image_duanwei"));
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
end

function M:updateValue(account, header_imgid, sex, level, state, uid)
    self.m_AvatarImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(header_imgid));
    self.m_Account.text = account;
    self.account = account;
    self.uid = uid;
    if 0 == sex then
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
    else
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
    end

    self.m_State.text = self:getStateText(state);

    local levelname = {self:getLevelName(level)};
    self.m_Level.text = levelname[1];
    self.m_LevelImage:setSpritePath("Atlas/DefaultSkin/Level.asset", levelname[2]);
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

function M:getStateText(state)
    local text = "离线";
    if 0 == state then
        text = "离线";
        self.m_InviteBtn:hide();
        GlobalNS.UtilApi.setImageColor(self.m_AvatarBtn.mImage:getSelfGo(), 0.8, 0.8, 0.8, 1);
    elseif 1 == state then    
        text = "在线";
        self.m_InviteBtn:show();
        GlobalNS.UtilApi.setImageColor(self.m_AvatarBtn.mImage:getSelfGo(), 1, 1, 1, 1);
    elseif 2 == state then
        text = "战斗中";
        self.m_InviteBtn:hide();
        GlobalNS.UtilApi.setImageColor(self.m_AvatarBtn.mImage:getSelfGo(), 1, 1, 1, 1);
    elseif 3 == state then
        text = "匹配中";
        self.m_InviteBtn:hide();
        GlobalNS.UtilApi.setImageColor(self.m_AvatarBtn.mImage:getSelfGo(), 1, 1, 1, 1);
    else

    end
    return text;
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

--查看
function M:onAvatarBtnClk()
    GCtx.mPlayerData.mHeroData.mViewAccount = self.account;
    GCtx.mPlayerData.mHeroData.mViewUid = self.uid;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountPanel);
end

--邀请
function M:onInviteBtnClk()
    self.m_InviteBtn:disable();
	self.mTimer:reset();
    self.mTimer:Start();

    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mTeamData:InviteJoinTeam("" .. self.uid);
    GCtx.mGameData:ShowRollMessage("邀请已发出，请等待……");
end

return M;

--endregion
