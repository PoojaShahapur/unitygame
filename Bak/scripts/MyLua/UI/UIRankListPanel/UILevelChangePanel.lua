MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIRankListPanel.RankListPanelNS");
MLoader("MyLua.UI.UIRankListPanel.RankListPanelData");
MLoader("MyLua.UI.UIRankListPanel.RankListPanelCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UILevelChangePanel";
GlobalNS.RankListPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUILevelChangePanel;
	self.mData = GlobalNS.new(GlobalNS.RankListPanelNS.RankListPanelData);

    self.oldlevel = 1;
    self.newlevel = 1;
    self.maxlevel = 1;

    self.mTimer = GlobalNS.new(GlobalNS.TimerItemBase);
    self.mTimer.mInternal = 0.02;
    self.mTimer.mIsContinuous = true;
    self.mTimer.mIsInfineLoop = true;
    self.mTimer:setFuncObject(self, self.onTick);
    self.index = 1;
    self.showend = false;
    self.scale = 0;
    self.delta = 0.04;
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
    --返回游戏按钮
	self.mBackBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBackBtn:addEventHandle(self, self.onBtnClk);

    self.mbeforeDanImage = GlobalNS.new(GlobalNS.AuxImage);
    self.mbeforeStar1Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mbeforeStar2Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mbeforeStar3Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mbeforeStar4Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mbeforeStar5Image = GlobalNS.new(GlobalNS.AuxImage);

    self.mnextDanImage = GlobalNS.new(GlobalNS.AuxImage);
    self.mnextStar1Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mnextStar2Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mnextStar3Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mnextStar4Image = GlobalNS.new(GlobalNS.AuxImage);
    self.mnextStar5Image = GlobalNS.new(GlobalNS.AuxImage);

    self.maxlevel = #LuaExcelManager.level_level;
end

function M:onReady()
    M.super.onReady(self);

    self.LevelChangeBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "LevelChangeBG");
	self.mBackBtn:setSelfGo(self.LevelChangeBG);

    self:initForm();
    self:setuidata(self.oldlevel, self.newlevel);
end

function M:initForm()
    self.beforelevel_Image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.LevelChangeBG, "beforelevel_Image");
    self.beforelevel_Danname = GlobalNS.UtilApi.getComByPath(self.beforelevel_Image, "Danname", "Text");
    self.mbeforeDanImage:setSelfGo(self.beforelevel_Image);
    self.beforeTwostar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforelevel_Image, "Twostar_Panel");
    self.beforeThreestar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforelevel_Image, "Threestar_Panel");
    self.beforeFourstar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforelevel_Image, "Fourstar_Panel");
    self.beforeFivestar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforelevel_Image, "Fivestar_Panel");
    self.beforeKing_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforelevel_Image, "King_Panel");

    self.nextlevel_Image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.LevelChangeBG, "nextlevel_Image");
    self.nextlevel_Danname = GlobalNS.UtilApi.getComByPath(self.nextlevel_Image, "Danname", "Text");
    self.mnextDanImage:setSelfGo(self.nextlevel_Image);
    self.nextTwostar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextlevel_Image, "Twostar_Panel");
    self.nextThreestar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextlevel_Image, "Threestar_Panel");
    self.nextFourstar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextlevel_Image, "Fourstar_Panel");
    self.nextFivestar_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextlevel_Image, "Fivestar_Panel");
    self.nextKing_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextlevel_Image, "King_Panel");

    self.Arrow = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.LevelChangeBG, "Arrow");
    self.Dan_Text = GlobalNS.UtilApi.getComByPath(self.LevelChangeBG, "Dan_Text", "Text");
    self.Continue_Text = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.LevelChangeBG, "Continue_Text");

    self.beforelevel_Image:SetActive(false);
    self.nextlevel_Image:SetActive(false);
    self.Arrow:SetActive(false);
    self.Continue_Text:SetActive(false);
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:setuidata(oldlevel, newlevel)
    self:setoldlevelUI(oldlevel);
    self:setnewlevelUI(newlevel);
    self:showani(oldlevel, newlevel);    

    self.mTimer:reset();
    self.mTimer:Start();
end

function M:onTick()
    self.scale = self.scale + self.delta;
    if self.scale >= 1 then
        self.scale = 1;
    end

    if 1 == self.index then
        self.beforelevel_Image:SetActive(true);
        GlobalNS.UtilApi.GetComponent(self.beforelevel_Image, "RectTransform").transform.localScale = Vector2.New(self.scale, self.scale);
        if self.scale >= 1 then
            self.index = 2;
            self.scale = 0;
        end
    end

    if 2 == self.index then
        self.Arrow:SetActive(true);
        GlobalNS.UtilApi.GetComponent(self.Arrow, "RectTransform").transform.localScale = Vector2.New(self.scale, self.scale);
        if self.scale >= 1 then
            self.index = 3;
            self.scale = 0;
        end
    end

    if 3 == self.index then
        self.nextlevel_Image:SetActive(true);
        GlobalNS.UtilApi.GetComponent(self.nextlevel_Image, "RectTransform").transform.localScale = Vector2.New(self.scale, self.scale);
        if self.scale >= 1 then
            self.showend = true;
            self.Continue_Text:SetActive(true);
            self.mTimer:Stop();
        end
    end
end

function M:showani(oldlevel, newlevel)
    if oldlevel > newlevel then
        self.Dan_Text.text = "很遗憾，段位降低了！";
    elseif oldlevel < newlevel then
        self.Dan_Text.text = "恭喜你，段位提升了！";
    else
        self.Dan_Text.text = "再接再厉，段位没有变化！";
    end
end

function M:setoldlevelUI(level)
    if level > self.maxlevel then
        level = self.maxlevel;
    end
    if 0 == level then
        level = 1;
    end

    local leveldata = LuaExcelManager.level_level[level];

    self.mbeforeDanImage:setSpritePath("Atlas/DefaultSkin/Level.asset", leveldata.image);
    self.beforelevel_Danname.text = leveldata.name;
    
    if leveldata.type >= 16 then
        self.beforeTwostar_Panel:SetActive(false);
        self.beforeThreestar_Panel:SetActive(false);
        self.beforeFourstar_Panel:SetActive(false);
        self.beforeFivestar_Panel:SetActive(false);
        self.beforeKing_Panel:SetActive(true);

        local image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeKing_Panel, "Image");
        local startext = GlobalNS.UtilApi.getComByPath(image, "Text", "Text");
        startext.text = leveldata.star;
    else
        if 2 == leveldata.maxstar then
            self.beforeTwostar_Panel:SetActive(true);
            self.beforeThreestar_Panel:SetActive(false);
            self.beforeFourstar_Panel:SetActive(false);
            self.beforeFivestar_Panel:SetActive(false);
            self.beforeKing_Panel:SetActive(false);
            self.mbeforeStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeTwostar_Panel, "star1"));
            self.mbeforeStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeTwostar_Panel, "star2"));
    
            self.mbeforeStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mbeforeStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
        elseif 3 == leveldata.maxstar then
            self.beforeTwostar_Panel:SetActive(false);
            self.beforeThreestar_Panel:SetActive(true);
            self.beforeFourstar_Panel:SetActive(false);
            self.beforeFivestar_Panel:SetActive(false);
            self.beforeKing_Panel:SetActive(false);
            self.mbeforeStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeThreestar_Panel, "star1"));
            self.mbeforeStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeThreestar_Panel, "star2"));
            self.mbeforeStar3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeThreestar_Panel, "star3"));
    
            self.mbeforeStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mbeforeStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mbeforeStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
        elseif 4 == leveldata.maxstar then
            self.beforeTwostar_Panel:SetActive(false);
            self.beforeThreestar_Panel:SetActive(false);
            self.beforeFourstar_Panel:SetActive(true);
            self.beforeFivestar_Panel:SetActive(false);
            self.beforeKing_Panel:SetActive(false);
            self.mbeforeStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeFourstar_Panel, "star1"));
            self.mbeforeStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeFourstar_Panel, "star2"));
            self.mbeforeStar3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeFourstar_Panel, "star3"));
            self.mbeforeStar4Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeFourstar_Panel, "star4"));
    
            self.mbeforeStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mbeforeStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mbeforeStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.mbeforeStar4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
        elseif 5 == leveldata.maxstar then
            self.beforeTwostar_Panel:SetActive(false);
            self.beforeThreestar_Panel:SetActive(false);
            self.beforeFourstar_Panel:SetActive(false);
            self.beforeFivestar_Panel:SetActive(true);
            self.beforeKing_Panel:SetActive(false);
            self.mbeforeStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeFivestar_Panel, "star1"));
            self.mbeforeStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeFivestar_Panel, "star2"));
            self.mbeforeStar3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeFivestar_Panel, "star3"));
            self.mbeforeStar4Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeFivestar_Panel, "star4"));
            self.mbeforeStar5Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.beforeFivestar_Panel, "star5"));
    
            self.mbeforeStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mbeforeStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mbeforeStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.mbeforeStar4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
            self.mbeforeStar5Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(5, leveldata.star, leveldata.maxstar));
        else
    
        end
    end
end

function M:setnewlevelUI(level)
    if level > self.maxlevel then
        level = self.maxlevel;
    end
    if 0 == level then
        level = 1;
    end

    local leveldata = LuaExcelManager.level_level[level];

    self.mnextDanImage:setSpritePath("Atlas/DefaultSkin/Level.asset", leveldata.image);
    self.nextlevel_Danname.text = leveldata.name;
    
    if leveldata.type >= 16 then
        self.nextTwostar_Panel:SetActive(false);
        self.nextThreestar_Panel:SetActive(false);
        self.nextFourstar_Panel:SetActive(false);
        self.nextFivestar_Panel:SetActive(false);
        self.nextKing_Panel:SetActive(true);

        local image = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextKing_Panel, "Image");
        local startext = GlobalNS.UtilApi.getComByPath(image, "Text", "Text");
        startext.text = leveldata.star;
    else
        if 2 == leveldata.maxstar then
            self.nextTwostar_Panel:SetActive(true);
            self.nextThreestar_Panel:SetActive(false);
            self.nextFourstar_Panel:SetActive(false);
            self.nextFivestar_Panel:SetActive(false);
            self.mnextStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextTwostar_Panel, "star1"));
            self.mnextStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextTwostar_Panel, "star2"));
    
            self.mnextStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mnextStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
        elseif 3 == leveldata.maxstar then
            self.nextTwostar_Panel:SetActive(false);
            self.nextThreestar_Panel:SetActive(true);
            self.nextFourstar_Panel:SetActive(false);
            self.nextFivestar_Panel:SetActive(false);
            self.mnextStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextThreestar_Panel, "star1"));
            self.mnextStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextThreestar_Panel, "star2"));
            self.mnextStar3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextThreestar_Panel, "star3"));
    
            self.mnextStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mnextStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mnextStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
        elseif 4 == leveldata.maxstar then
            self.nextTwostar_Panel:SetActive(false);
            self.nextThreestar_Panel:SetActive(false);
            self.nextFourstar_Panel:SetActive(true);
            self.nextFivestar_Panel:SetActive(false);
            self.mnextStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextFourstar_Panel, "star1"));
            self.mnextStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextFourstar_Panel, "star2"));
            self.mnextStar3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextFourstar_Panel, "star3"));
            self.mnextStar4Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextFourstar_Panel, "star4"));
    
            self.mnextStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mnextStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mnextStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.mnextStar4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
        elseif 5 == leveldata.maxstar then
            self.nextTwostar_Panel:SetActive(false);
            self.nextThreestar_Panel:SetActive(false);
            self.nextFourstar_Panel:SetActive(false);
            self.nextFivestar_Panel:SetActive(true);
            self.mnextStar1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextFivestar_Panel, "star1"));
            self.mnextStar2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextFivestar_Panel, "star2"));
            self.mnextStar3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextFivestar_Panel, "star3"));
            self.mnextStar4Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextFivestar_Panel, "star4"));
            self.mnextStar5Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.nextFivestar_Panel, "star5"));
    
            self.mnextStar1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
            self.mnextStar2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
            self.mnextStar3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
            self.mnextStar4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
            self.mnextStar5Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(5, leveldata.star, leveldata.maxstar));
        else
    
        end
    end
end

function M:updateUIData(oldlevel, newlevel)
    self.oldlevel = oldlevel;
    self.newlevel = newlevel;

    if self.mIsReady then
        self:setuidata(oldlevel, newlevel);
    end
end

function M:onExit()
    self.mBackBtn:dispose();

    self.mbeforeDanImage:dispose();
    self.mbeforeStar1Image:dispose();
    self.mbeforeStar2Image:dispose();
    self.mbeforeStar3Image:dispose();
    self.mbeforeStar4Image:dispose();
    self.mbeforeStar5Image:dispose();

    self.mnextDanImage:dispose();
    self.mnextStar1Image:dispose();
    self.mnextStar2Image:dispose();
    self.mnextStar3Image:dispose();
    self.mnextStar4Image:dispose();
    self.mnextStar5Image:dispose();

    self.mTimer:Stop();

    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIRankListPanel);

    M.super.onExit(self);
end

function M:onBtnClk()
    if self.showend then
        self:exit();
    end
end

return M;