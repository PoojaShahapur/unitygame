MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIServerHistoryRankListPanel.ServerHistoryRankListPanelNS");
MLoader("MyLua.UI.UIServerHistoryRankListPanel.ServerHistoryRankListPanelData");
MLoader("MyLua.UI.UIServerHistoryRankListPanel.ServerHistoryRankListPanelCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIServerHistoryRankListPanel";
GlobalNS.ServerHistoryRankListPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIServerHistoryRankListPanel;
	self.mData = GlobalNS.new(GlobalNS.ServerHistoryRankListPanelNS.ServerHistoryRankListPanelData);

    self.ranktype = 0;
    self.cur_open = 0;

    self.mTabPanel = GlobalNS.new(GlobalNS.TabPageMgr);
    self.mTabPanel.mTabClickEventDispatch:addEventHandle(self, self.onTabClick);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
    --返回游戏按钮
	self.mBackGameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBackGameBtn:addEventHandle(self, self.onBtnClk);
    self.mDanBackGameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mDanBackGameBtn:addEventHandle(self, self.onBtnClk);

    self.avatarimages = { };
    self.myavatar = nil;
    self.honerimages = {};
    self.myhoner = nil;
    self.seximages = {};
    self.mysex = nil;
    self.m_LevelImage = nil;
    self.m_Star1Image = nil;
    self.m_Star2Image = nil;
    self.m_Star3Image = nil;
    self.m_Star4Image = nil;
    self.m_Star5Image = nil;
end

function M:onReady()
    M.super.onReady(self);
    self.RankBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "RankListBG");
    self.DanRankBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Danlist_Panel");
	self.mBackGameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.RankBG, "BackGame_BtnTouch"));
    self.mDanBackGameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.DanRankBG, "BackGame_BtnTouch"));

    local page = self.mTabPanel:addTabPage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Lianyu_Btn"), self.RankBG);
    page:setTag(0);
    page = self.mTabPanel:addTabPage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Dan_Btn"), self.DanRankBG);
    page:setTag(1);

    --获取MyRank的GameObject对象
    self.mMyRankArea = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.RankBG, "MyRank");
    self.mDropdown = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.RankBG, "Dropdown");
    GlobalNS.UtilApi.addDropdownHandle(self.mDropdown, self, self.onValueChanged);

    self.mMyDanRankArea = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.DanRankBG, "MyRank");
    
    self:on_scrollview_loaded();
    self:on_ydscrollview_loaded();
    self:on_danscrollview_loaded();
    self.mTabPanel:openPage(0);
end

function M:onValueChanged(index)
    if 0 == index then
        if GCtx.mGameData.isgethistorydata then
            self:updateUIData(0);
        else
            GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:GetPurgatoryRank(0);
        end
    else
        if GCtx.mGameData.isgetydhistorydata then
            self:updateUIData(1);
        else
            GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:GetPurgatoryRank(1);
        end
    end
end

function M:on_scrollview_loaded()
    --获取ScrollRect的GameObject对象
    self.mScrollRect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.RankBG, "ScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mScrollRect, "Viewport");
    --获取ScrollRect下Content中的ScrollRectTable组件
    self.scroll_rect_table = GlobalNS.UtilApi.getComByPath(viewport, "Content", "ScrollRectTable");

    self.scroll_rect_table.onItemRender = 
        function(scroll_rect_item, index)
            if 1 == self.ranktype then
                return;
            end
            if GlobalNS.UtilApi.IsUObjNil(scroll_rect_item) then
                return;
            end

            local i = index - 1;
            scroll_rect_item.gameObject:SetActive(true);
            scroll_rect_item.name = "RankItem" .. index;
            --排名图标      
            if index <= 3 then
               local Honer = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Honer");
               Honer:SetActive(true);
               local honer = GlobalNS.new(GlobalNS.AuxImage);
               honer:setSelfGo(Honer);
               if index == 1 then
		    		--honer:setSpritePath("DefaultSkin/SkyWarSkin/rank1.png", "rank1");
		    		honer:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank1");
                   --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_gold");
               elseif index == 2 then
		    		--honer:setSpritePath("DefaultSkin/SkyWarSkin/rank2.png", "rank2");
		    		honer:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank2");
                   --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_yin");
               else
		    		--honer:setSpritePath("DefaultSkin/SkyWarSkin/rank3.png", "rank3");
		    		honer:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank3");
                   --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_tong");
               end
               self.honerimages[index] = honer;
            else
                local Honer = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Honer");
                Honer:SetActive(false);
            end

            --排名
            local Rank = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "Rank", "Text");
            if index <= 3 then
                Rank.text = "";
            else
                Rank.text = "" .. index;
                if GCtx.mGameData.historyrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                    Rank.text = "<color=#32c832ff>"..index.."</color>";
                end
            end
            
            --头像
            local Avatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Avatar");
            local avatarImage = GlobalNS.new(GlobalNS.AuxImage);
            avatarImage:setSelfGo(Avatar);
            local avatarindex = GCtx.mGameData.historyrankinfolist:get(i).m_avatarindex;
            if avatarindex == 0 then
                if GCtx.mGameData.historyrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                    avatarindex = 1;
                else
                    local _time = os.clock();
                    math.randomseed(_time + i);
                    avatarindex = math.random(1, 4);
                end
            end
	    	--avatarImage:setSpritePath("DefaultSkin/Avatar/"..avatarindex..".png", GlobalNS.UtilStr.tostring(avatarindex));
	    	avatarImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(avatarindex));
            --avatarImage:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(avatarindex));
            self.avatarimages[index] = avatarImage;

            GCtx.mGameData.historyrankinfolist:get(i).m_AvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
            GCtx.mGameData.historyrankinfolist:get(i).m_AvatarBtn:addEventHandle(self, self.onAvatarBtnClk);
            GCtx.mGameData.historyrankinfolist:get(i).m_AvatarBtn.param1 = GCtx.mGameData.historyrankinfolist:get(i).m_uid;
            GCtx.mGameData.historyrankinfolist:get(i).m_AvatarBtn.param2 = GCtx.mGameData.historyrankinfolist:get(i).m_name;
            GCtx.mGameData.historyrankinfolist:get(i).m_AvatarBtn:setSelfGo(scroll_rect_item.gameObject);
            GCtx.mGameData.historyrankinfolist:get(i).m_AvatarBtn:setIsDestroySelf(false);

            if GCtx.mGameData.historyrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                GCtx.mGameData.historyrankinfolist:get(i).m_AvatarBtn:disable();
            else
                GCtx.mGameData.historyrankinfolist:get(i).m_AvatarBtn:enable();
            end

            --性别
            local Sex = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Sex");
            local sexImage = GlobalNS.new(GlobalNS.AuxImage);
            sexImage:setSelfGo(Sex);
            if 0 == GCtx.mGameData.historyrankinfolist:get(i).m_sex then
                sexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
            else
                sexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
            end
            self.seximages[index] = sexImage;

            --用户名
            local AccountName = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "AccountName", "Text");
            AccountName.text = GCtx.mGameData.historyrankinfolist:get(i).m_name;
            if GCtx.mGameData.historyrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                AccountName.text = "<color=#32c832ff>"..GCtx.mGameData.historyrankinfolist:get(i).m_name.."</color>";
            end

            --积分
            local Score = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "Score", "Text");
            Score.text = GCtx.mGameData.historyrankinfolist:get(i).m_score;
            if GCtx.mGameData.historyrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                Score.text = "<color=#32c832ff>"..GCtx.mGameData.historyrankinfolist:get(i).m_score.."</color>";
            end

            --奖励
            local AwardImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Award");
            local Award = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "Award_Text", "Text");
            local num = GCtx.mGameData.historyrankinfolist:get(i).m_award;
            local text = "" .. num;
            if 0 == num then
                AwardImage:SetActive(false);
                text = "";
            else
                AwardImage:SetActive(true);
            end
            Award.text = text;
            if GCtx.mGameData.historyrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                Award.text = "<color=#32c832ff>".. text .."</color>";
            end

            --[[关注按钮
            GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn = GlobalNS.new(GlobalNS.AuxButton);
            GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:addEventHandle(self, self.onFocusBtnClk);
            GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn.param1 = GCtx.mGameData.historyrankinfolist:get(i).m_name;
            GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Focus_Btn"));
            GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:setIsDestroySelf(false);
            if GCtx.mGameData.historyrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:hide();
            else
                GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:show();
                if 1 == GCtx.mGameData.historyrankinfolist:get(i).m_isFllowing then
                    GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:setText("已关注");
                    GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:disable();
                end
            end
            ]]--
        end
     
     self.scroll_rect_table.onItemDispear = 
         function(index)
             if 1 == self.ranktype then
                return;
             end
             if nil ~= GCtx.mGameData.historyrankinfolist:get(index-1).m_AvatarBtn then
                 GCtx.mGameData.historyrankinfolist:get(index-1).m_AvatarBtn:clearEventHandle();
             end
             --[[if nil ~= GCtx.mGameData.historyrankinfolist:get(index-1).m_FocusBtn then
                 GCtx.mGameData.historyrankinfolist:get(index-1).m_FocusBtn:clearEventHandle();
             end]]--
         end
end

function M:on_ydscrollview_loaded()
    --获取ScrollRect的GameObject对象
    self.mYDScrollRect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.RankBG, "YDScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mYDScrollRect, "Viewport");
    --获取ScrollRect下Content中的ScrollRectTable组件
    self.ydscroll_rect_table = GlobalNS.UtilApi.getComByPath(viewport, "Content", "ScrollRectTable");

    self.ydscroll_rect_table.onItemRender = 
        function(scroll_rect_item, index)
            if 0 == self.ranktype then
                return;
            end
            if GlobalNS.UtilApi.IsUObjNil(scroll_rect_item) then
                return;
            end

            local i = index - 1;
            scroll_rect_item.gameObject:SetActive(true);
            scroll_rect_item.name = "RankItem" .. index;
            --排名图标      
            if index <= 3 then
               local Honer = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Honer");
               Honer:SetActive(true);
               local honer = GlobalNS.new(GlobalNS.AuxImage);
               honer:setSelfGo(Honer);
               if index == 1 then
		    		--honer:setSpritePath("DefaultSkin/SkyWarSkin/rank1.png", "rank1");
		    		honer:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank1");
                   --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_gold");
               elseif index == 2 then
		    		--honer:setSpritePath("DefaultSkin/SkyWarSkin/rank2.png", "rank2");
		    		honer:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank2");
                   --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_yin");
               else
		    		--honer:setSpritePath("DefaultSkin/SkyWarSkin/rank3.png", "rank3");
		    		honer:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank3");
                   --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_tong");
               end
               self.honerimages[index] = honer;
            else
                local Honer = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Honer");
                Honer:SetActive(false);
            end

            --排名
            local Rank = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "Rank", "Text");
            if index <= 3 then
                Rank.text = "";
            else
                Rank.text = "" .. index;
                if GCtx.mGameData.ydhistoryrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                    Rank.text = "<color=#32c832ff>"..index.."</color>";
                end
            end
            
            --头像
            local Avatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Avatar");
            local avatarImage = GlobalNS.new(GlobalNS.AuxImage);
            avatarImage:setSelfGo(Avatar);
            local avatarindex = GCtx.mGameData.ydhistoryrankinfolist:get(i).m_avatarindex;
            if avatarindex == 0 then
                if GCtx.mGameData.ydhistoryrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                    avatarindex = 1;
                else
                    local _time = os.clock();
                    math.randomseed(_time + i);
                    avatarindex = math.random(1, 4);
                end
            end
	    	--avatarImage:setSpritePath("DefaultSkin/Avatar/"..avatarindex..".png", GlobalNS.UtilStr.tostring(avatarindex));
	    	avatarImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(avatarindex));
            --avatarImage:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(avatarindex));
            self.avatarimages[index] = avatarImage;

            GCtx.mGameData.ydhistoryrankinfolist:get(i).m_AvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
            GCtx.mGameData.ydhistoryrankinfolist:get(i).m_AvatarBtn:addEventHandle(self, self.onAvatarBtnClk);
            GCtx.mGameData.ydhistoryrankinfolist:get(i).m_AvatarBtn.param1 = GCtx.mGameData.ydhistoryrankinfolist:get(i).m_uid;
            GCtx.mGameData.ydhistoryrankinfolist:get(i).m_AvatarBtn.param2 = GCtx.mGameData.ydhistoryrankinfolist:get(i).m_name;
            GCtx.mGameData.ydhistoryrankinfolist:get(i).m_AvatarBtn:setSelfGo(scroll_rect_item.gameObject);
            GCtx.mGameData.ydhistoryrankinfolist:get(i).m_AvatarBtn:setIsDestroySelf(false);

            if GCtx.mGameData.ydhistoryrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                GCtx.mGameData.ydhistoryrankinfolist:get(i).m_AvatarBtn:disable();
            else
                GCtx.mGameData.ydhistoryrankinfolist:get(i).m_AvatarBtn:enable();
            end

            --性别
            local Sex = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Sex");
            local sexImage = GlobalNS.new(GlobalNS.AuxImage);
            sexImage:setSelfGo(Sex);
            if 0 == GCtx.mGameData.ydhistoryrankinfolist:get(i).m_sex then
                sexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
            else
                sexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
            end
            self.seximages[index] = sexImage;

            --用户名
            local AccountName = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "AccountName", "Text");
            AccountName.text = GCtx.mGameData.ydhistoryrankinfolist:get(i).m_name;
            if GCtx.mGameData.ydhistoryrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                AccountName.text = "<color=#32c832ff>"..GCtx.mGameData.ydhistoryrankinfolist:get(i).m_name.."</color>";
            end

            --积分
            local Score = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "Score", "Text");
            Score.text = GCtx.mGameData.ydhistoryrankinfolist:get(i).m_score;
            if GCtx.mGameData.ydhistoryrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                Score.text = "<color=#32c832ff>"..GCtx.mGameData.ydhistoryrankinfolist:get(i).m_score.."</color>";
            end

            --奖励
            local AwardImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Award");
            local Award = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "Award_Text", "Text");
            local num = GCtx.mGameData.ydhistoryrankinfolist:get(i).m_award;
            local text = "" .. num;
            if 0 == num then
                AwardImage:SetActive(false);
                text = "";
            else
                AwardImage:SetActive(true);
            end
            Award.text = text;
            if GCtx.mGameData.ydhistoryrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                Award.text = "<color=#32c832ff>".. text .."</color>";
            end

            --[[关注按钮
            GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn = GlobalNS.new(GlobalNS.AuxButton);
            GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:addEventHandle(self, self.onFocusBtnClk);
            GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn.param1 = GCtx.mGameData.historyrankinfolist:get(i).m_name;
            GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Focus_Btn"));
            GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:setIsDestroySelf(false);
            if GCtx.mGameData.historyrankinfolist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:hide();
            else
                GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:show();
                if 1 == GCtx.mGameData.historyrankinfolist:get(i).m_isFllowing then
                    GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:setText("已关注");
                    GCtx.mGameData.historyrankinfolist:get(i).m_FocusBtn:disable();
                end
            end
            ]]--
        end
     
     self.scroll_rect_table.onItemDispear = 
         function(index)
             if 0 == self.ranktype then
                return;
             end
             if nil ~= GCtx.mGameData.ydhistoryrankinfolist:get(index-1).m_AvatarBtn then
                 GCtx.mGameData.ydhistoryrankinfolist:get(index-1).m_AvatarBtn:clearEventHandle();
             end
             --[[if nil ~= GCtx.mGameData.historyrankinfolist:get(index-1).m_FocusBtn then
                 GCtx.mGameData.historyrankinfolist:get(index-1).m_FocusBtn:clearEventHandle();
             end]]--
         end
end

function M:on_danscrollview_loaded()
    --获取ScrollRect的GameObject对象
    self.mDanScrollRect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.DanRankBG, "ScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mDanScrollRect, "Viewport");
    --获取ScrollRect下Content中的ScrollRectTable组件
    self.dan_scroll_rect_table = GlobalNS.UtilApi.getComByPath(viewport, "Content", "ScrollRectTable");

    self.dan_scroll_rect_table.onItemRender = 
        function(scroll_rect_item, index)
            if GlobalNS.UtilApi.IsUObjNil(scroll_rect_item) then
                return;
            end

            local i = index - 1;
            scroll_rect_item.gameObject:SetActive(true);
            scroll_rect_item.name = "RankItem" .. index;
            --排名图标      
            if index <= 3 then
               local Honer = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Honer");
               Honer:SetActive(true);
               GCtx.mGameData.levelranklist:get(i).m_HonerImage = GlobalNS.new(GlobalNS.AuxImage);
               GCtx.mGameData.levelranklist:get(i).m_HonerImage:setSelfGo(Honer);
               if index == 1 then
		    		GCtx.mGameData.levelranklist:get(i).m_HonerImage:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank1");
               elseif index == 2 then
		    		GCtx.mGameData.levelranklist:get(i).m_HonerImage:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank2");
               else
		    		GCtx.mGameData.levelranklist:get(i).m_HonerImage:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank3");
               end
            else
                local Honer = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Honer");
                Honer:SetActive(false);
            end

            --排名
            local Rank = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "Rank", "Text");
            if index <= 3 then
                Rank.text = "";
            else
                Rank.text = "" .. index;
                if GCtx.mGameData.levelranklist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                    Rank.text = "<color=#32c832ff>"..index.."</color>";
                end
            end
            
            --头像
            local Avatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Avatar");
            GCtx.mGameData.levelranklist:get(i).m_AvatarImage = GlobalNS.new(GlobalNS.AuxImage);
            GCtx.mGameData.levelranklist:get(i).m_AvatarImage:setSelfGo(Avatar);
            local avatarindex = GCtx.mGameData.levelranklist:get(i).m_avatarindex;
            if avatarindex == 0 then
                if GCtx.mGameData.levelranklist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                    avatarindex = 1;
                else
                    local _time = os.clock();
                    math.randomseed(_time + i);
                    avatarindex = math.random(1, 4);
                end
            end
	    	GCtx.mGameData.levelranklist:get(i).m_AvatarImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(avatarindex));

            GCtx.mGameData.levelranklist:get(i).m_AvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
            GCtx.mGameData.levelranklist:get(i).m_AvatarBtn:addEventHandle(self, self.onAvatarBtnClk);
            GCtx.mGameData.levelranklist:get(i).m_AvatarBtn.param1 = GCtx.mGameData.levelranklist:get(i).m_uid;
            GCtx.mGameData.levelranklist:get(i).m_AvatarBtn.param2 = GCtx.mGameData.levelranklist:get(i).m_name;
            GCtx.mGameData.levelranklist:get(i).m_AvatarBtn:setSelfGo(scroll_rect_item.gameObject);
            GCtx.mGameData.levelranklist:get(i).m_AvatarBtn:setIsDestroySelf(false);

            if GCtx.mGameData.levelranklist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                GCtx.mGameData.levelranklist:get(i).m_AvatarBtn:disable();
            else
                GCtx.mGameData.levelranklist:get(i).m_AvatarBtn:enable();
            end

            --性别
            local Sex = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Sex");
            GCtx.mGameData.levelranklist:get(i).m_SexImage = GlobalNS.new(GlobalNS.AuxImage);
            GCtx.mGameData.levelranklist:get(i).m_SexImage:setSelfGo(Sex);
            if 0 == GCtx.mGameData.levelranklist:get(i).m_sex then
                GCtx.mGameData.levelranklist:get(i).m_SexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
            else
                GCtx.mGameData.levelranklist:get(i).m_SexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
            end

            --用户名
            local AccountName = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "AccountName", "Text");
            AccountName.text = GCtx.mGameData.levelranklist:get(i).m_name;
            if GCtx.mGameData.levelranklist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                AccountName.text = "<color=#32c832ff>"..GCtx.mGameData.levelranklist:get(i).m_name.."</color>";
            end
            
            --段位
            local Dan_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Dan_Panel");
            local leveldata = LuaExcelManager.level_level[GCtx.mGameData.levelranklist:get(i).m_level];
            local LevelImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Panel, "Image");
            GCtx.mGameData.levelranklist:get(i).m_LevelImage = GlobalNS.new(GlobalNS.AuxImage);
            GCtx.mGameData.levelranklist:get(i).m_LevelImage:setSelfGo(LevelImage);
            GCtx.mGameData.levelranklist:get(i).m_LevelImage:setSpritePath("Atlas/DefaultSkin/Level.asset", leveldata.image);
            local LevelName = GlobalNS.UtilApi.getComByPath(Dan_Panel, "Text", "Text");
            LevelName.text = leveldata.name;
            if GCtx.mGameData.levelranklist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                LevelName.text = "<color=#32c832ff>".. leveldata.name .."</color>";
            end

            local Star_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Panel, "Star_Panel");
            local king_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Panel, "king_Panel");
            if leveldata.type >= 16 then
                Star_Panel:SetActive(false);
                king_Panel:SetActive(true);
                local kingImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(king_Panel, "Image");
                local starnum = GlobalNS.UtilApi.getComByPath(kingImage, "Text", "Text");
                starnum.text = leveldata.star;
                if GCtx.mGameData.levelranklist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
                    starnum.text = "<color=#32c832ff>".. text .."</color>";
                end
            else
                Star_Panel:SetActive(true);
                king_Panel:SetActive(false);
                GCtx.mGameData.levelranklist:get(i).m_Star1Image = GlobalNS.new(GlobalNS.AuxImage);
                GCtx.mGameData.levelranklist:get(i).m_Star1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Star_Panel, "Star1_Image"));
                GCtx.mGameData.levelranklist:get(i).m_Star2Image = GlobalNS.new(GlobalNS.AuxImage);
                GCtx.mGameData.levelranklist:get(i).m_Star2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Star_Panel, "Star2_Image"));
                GCtx.mGameData.levelranklist:get(i).m_Star3Image = GlobalNS.new(GlobalNS.AuxImage);
                GCtx.mGameData.levelranklist:get(i).m_Star3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Star_Panel, "Star3_Image"));
                GCtx.mGameData.levelranklist:get(i).m_Star4Image = GlobalNS.new(GlobalNS.AuxImage);
                GCtx.mGameData.levelranklist:get(i).m_Star4Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Star_Panel, "Star4_Image"));
                GCtx.mGameData.levelranklist:get(i).m_Star5Image = GlobalNS.new(GlobalNS.AuxImage);
                GCtx.mGameData.levelranklist:get(i).m_Star5Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Star_Panel, "Star5_Image"));

                GCtx.mGameData.levelranklist:get(i).m_Star1Image:show();
                GCtx.mGameData.levelranklist:get(i).m_Star2Image:show();
                GCtx.mGameData.levelranklist:get(i).m_Star3Image:show();
                GCtx.mGameData.levelranklist:get(i).m_Star4Image:show();
                GCtx.mGameData.levelranklist:get(i).m_Star5Image:show();

                if 2 == leveldata.maxstar then
                    GCtx.mGameData.levelranklist:get(i).m_Star1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star3Image:hide();
                    GCtx.mGameData.levelranklist:get(i).m_Star4Image:hide();
                    GCtx.mGameData.levelranklist:get(i).m_Star5Image:hide();
                elseif 3 == leveldata.maxstar then
                    GCtx.mGameData.levelranklist:get(i).m_Star1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star4Image:hide();
                    GCtx.mGameData.levelranklist:get(i).m_Star5Image:hide();
                elseif 4 == leveldata.maxstar then
                    GCtx.mGameData.levelranklist:get(i).m_Star1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star5Image:hide();
                elseif 5 == leveldata.maxstar then
                    GCtx.mGameData.levelranklist:get(i).m_Star1Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(1, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star2Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(2, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star3Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(3, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star4Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(4, leveldata.star, leveldata.maxstar));
                    GCtx.mGameData.levelranklist:get(i).m_Star5Image:setSpritePath("Atlas/DefaultSkin/Level.asset", GCtx.mGameData:getStarImage(5, leveldata.star, leveldata.maxstar));
                else
                    
                end
            end
        end
     
     self.dan_scroll_rect_table.onItemDispear = 
         function(index)
             if nil ~= GCtx.mGameData.levelranklist:get(index-1).m_AvatarBtn then
                 GCtx.mGameData.levelranklist:get(index-1).m_AvatarBtn:clearEventHandle();
             end
         end
end

--查看
function M:onAvatarBtnClk(dispObj)
    local uid = dispObj.param1;
    local account = dispObj.param2;
    GCtx.mPlayerData.mHeroData.mViewUid = uid;
    GCtx.mPlayerData.mHeroData.mViewAccount = account;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountPanel);
end

--关注
function M:onFocusBtnClk(dispObj)
    local name = dispObj.param1;
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:Follow(name);
end

function M:onTabClick(dispObj)
	local tag = dispObj:getCurPageTag();
    self.cur_open = tag;
    if 0 == tag then --炼狱
        self:updateUIData(self.ranktype);
    elseif 1 == tag then--段位
        if GCtx.mGameData.isgetlevelrankdata then
            self:updateDanUIData();
        else
            GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:GetLevelRank();
        end
    else
        
    end
end

function M:onShow()
    M.super.onShow(self);    
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    if(nil ~= self.mTabPanel) then
        self.mTabPanel.mTabClickEventDispatch:removeEventHandle(self, self.onTabClick);
		GlobalNS.delete(self.mTabPanel);
		self.mTabPanel = nil;
	end

    self.mBackGameBtn:dispose();
    self.mDanBackGameBtn:dispose();

    M.super.onExit(self);
end

function M:onBtnClk()
    if self.myavatar ~= nil then
        self.myavatar:dispose();
        self.myavatar = nil;
    end
    for i=1, #self.avatarimages do
        self.avatarimages[i]:dispose();
    end
    self.avatarimages = {};

    if self.myhoner ~= nil then
        self.myhoner:dispose();
        self.myhoner = nil;
    end
    for i=1, #self.honerimages do
        self.honerimages[i]:dispose();
    end
    self.honerimages = {};

    if self.mysex ~= nil then
        self.mysex:dispose();
        self.mysex = nil;
    end
    for i=1, #self.seximages do
        self.seximages[i]:dispose();
    end
    self.seximages = {};

    if self.m_LevelImage ~= nil then
        self.m_LevelImage:dispose();
        self.m_LevelImage = nil;
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

    GCtx.mGameData:clearHistoryRanklist();
    GCtx.mGameData:clearYDHistoryRanklist();
    GCtx.mGameData:clearlevelranklist();

    self:exit();
end

--我的排名数据
function M:SetMyRankInfo()
    if GCtx.mGameData.myhistoryRank == nil then
        return;
    end
    if 1 == self.ranktype then
         return;
    end
    --判断自己是否上榜  
    local mytopRank = 0;
    local myAwardNum = 0;
    if GCtx.mGameData.historyrankinfolist:ContainsKey(GCtx.mGameData.myhistoryRank.acc) then
        mytopRank = GCtx.mGameData.historyrankinfolist:value(GCtx.mGameData.myhistoryRank.acc).m_rank;
        myAwardNum = GCtx.mGameData.historyrankinfolist:value(GCtx.mGameData.myhistoryRank.acc).m_award;
    end

    --荣誉
    local myHoner = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Honer");
    if mytopRank > 3 or mytopRank == 0 then
        myHoner:SetActive(false);
    else
        myHoner:SetActive(true);
        self.myhoner = GlobalNS.new(GlobalNS.AuxImage);
        self.myhoner:setSelfGo(myHoner);
        if mytopRank == 1 then
			--self.myhoner:setSpritePath("DefaultSkin/SkyWarSkin/rank1.png", "rank1");
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank1");
            --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_gold");
        elseif mytopRank == 2 then
			--self.myhoner:setSpritePath("DefaultSkin/SkyWarSkin/rank2.png", "rank2");
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank2");
            --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_yin");
        else
			--self.myhoner:setSpritePath("DefaultSkin/SkyWarSkin/rank3.png", "rank3");
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank3");
            --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_tong");
        end
    end

    --排名
    local myRank = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Rank", "Text");
    if mytopRank <= 3 then
        myRank.text = "";
    else
        myRank.text = "" .. mytopRank;
    end

    --头像
    local myAvatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Avatar");
    self.myavatar = GlobalNS.new(GlobalNS.AuxImage);
    self.myavatar:setSelfGo(myAvatar);
    local avatarindex = GCtx.mGameData.myhistoryRank.imageid;
    if avatarindex == 0 then
        avatarindex = 1;
    end
	--self.myavatar:setSpritePath("DefaultSkin/Avatar/"..avatarindex..".png", GlobalNS.UtilStr.tostring(avatarindex));
	self.myavatar:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(avatarindex));
    --self.myavatar:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(avatarindex));

    --性别
    local mySex = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Sex");
    self.mysex = GlobalNS.new(GlobalNS.AuxImage);
    self.mysex:setSelfGo(mySex);
    if 0 == GCtx.mGameData.myhistoryRank.sex then
        self.mysex:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
    else
        self.mysex:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
    end

    --账户
    local myAccount = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "AccountName", "Text");
    myAccount.text = GCtx.mGameData.myhistoryRank.acc;

    --最高分
    local myScore = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Score", "Text");
    myScore.text = GCtx.mGameData.myhistoryRank.score;

    --奖励
    local AwardImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Award");
    local Award = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Award_Text", "Text");
    local num = myAwardNum;
    local text = "" .. num;
    if 0 == num then
        AwardImage:SetActive(false);
        text = "";
    else
        AwardImage:SetActive(true);
    end
    Award.text = text;
end

--我的排名数据
function M:SetMyYDRankInfo()
    if GCtx.mGameData.myydhistoryRank == nil then
        return;
    end
    if 0 == self.ranktype then
         return;
    end

    --判断自己是否上榜  
    local mytopRank = 0;
    local myAwardNum = 0;
    local myydScore = 0;
    if GCtx.mGameData.ydhistoryrankinfolist:ContainsKey(GCtx.mGameData.myydhistoryRank.acc) then
        mytopRank = GCtx.mGameData.ydhistoryrankinfolist:value(GCtx.mGameData.myydhistoryRank.acc).m_rank;
        myAwardNum = GCtx.mGameData.ydhistoryrankinfolist:value(GCtx.mGameData.myydhistoryRank.acc).m_award;
        myydScore = GCtx.mGameData.ydhistoryrankinfolist:value(GCtx.mGameData.myydhistoryRank.acc).m_score;
    end

    --荣誉
    local myHoner = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Honer");
    if mytopRank > 3 or mytopRank == 0 then
        myHoner:SetActive(false);
    else
        myHoner:SetActive(true);
        self.myhoner = GlobalNS.new(GlobalNS.AuxImage);
        self.myhoner:setSelfGo(myHoner);
        if mytopRank == 1 then
			--self.myhoner:setSpritePath("DefaultSkin/SkyWarSkin/rank1.png", "rank1");
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank1");
            --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_gold");
        elseif mytopRank == 2 then
			--self.myhoner:setSpritePath("DefaultSkin/SkyWarSkin/rank2.png", "rank2");
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank2");
            --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_yin");
        else
			--self.myhoner:setSpritePath("DefaultSkin/SkyWarSkin/rank3.png", "rank3");
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank3");
            --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_tong");
        end
    end

    --排名
    local myRank = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Rank", "Text");
    if mytopRank <= 3 then
        myRank.text = "";
    else
        myRank.text = "" .. mytopRank;
    end

    --头像
    local myAvatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Avatar");
    self.myavatar = GlobalNS.new(GlobalNS.AuxImage);
    self.myavatar:setSelfGo(myAvatar);
    local avatarindex = GCtx.mGameData.myydhistoryRank.imageid;
    if avatarindex == 0 then
        avatarindex = 1;
    end
	--self.myavatar:setSpritePath("DefaultSkin/Avatar/"..avatarindex..".png", GlobalNS.UtilStr.tostring(avatarindex));
	self.myavatar:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(avatarindex));
    --self.myavatar:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(avatarindex));

    --性别
    local mySex = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Sex");
    self.mysex = GlobalNS.new(GlobalNS.AuxImage);
    self.mysex:setSelfGo(mySex);
    if 0 == GCtx.mGameData.myydhistoryRank.sex then
        self.mysex:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
    else
        self.mysex:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
    end

    --账户
    local myAccount = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "AccountName", "Text");
    myAccount.text = GCtx.mGameData.myydhistoryRank.acc;

    --最高分
    local myScore = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Score", "Text");
    if 0 == mytopRank then
        myScore.text = "未上榜";
    else
        myScore.text = myydScore;
    end

    --奖励
    local AwardImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Award");
    local Award = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Award_Text", "Text");
    local num = myAwardNum;
    local text = "" .. num;
    if 0 == num then
        AwardImage:SetActive(false);
        text = "";
    else
        AwardImage:SetActive(true);
    end
    Award.text = text;
end

--我的排名数据
function M:SetMyDanRankInfo()
    if GCtx.mGameData.mylevelRank == nil then
        return;
    end

    --判断自己是否上榜  
    local mytopRank = 0;
    if GCtx.mGameData.levelranklist:ContainsKey(GCtx.mGameData.mylevelRank.uid) then
        mytopRank = GCtx.mGameData.levelranklist:value(GCtx.mGameData.mylevelRank.uid).m_rank;
    end

    --荣誉
    local myHoner = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyDanRankArea, "Honer");
    if mytopRank > 3 or mytopRank == 0 then
        myHoner:SetActive(false);
    else
        myHoner:SetActive(true);
        self.myhoner = GlobalNS.new(GlobalNS.AuxImage);
        self.myhoner:setSelfGo(myHoner);
        if mytopRank == 1 then
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank1");
        elseif mytopRank == 2 then
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank2");
        else
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank3");
        end
    end

    --排名
    local myRank = GlobalNS.UtilApi.getComByPath(self.mMyDanRankArea, "Rank", "Text");
    if mytopRank <= 3 then
        myRank.text = "";
    else
        myRank.text = "" .. mytopRank;
    end

    --头像
    local myAvatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyDanRankArea, "Avatar");
    self.myavatar = GlobalNS.new(GlobalNS.AuxImage);
    self.myavatar:setSelfGo(myAvatar);
    local avatarindex = GCtx.mGameData.mylevelRank.imageid;
    if avatarindex == 0 then
        avatarindex = 1;
    end
	self.myavatar:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(avatarindex));

    --性别
    local mySex = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyDanRankArea, "Sex");
    self.mysex = GlobalNS.new(GlobalNS.AuxImage);
    self.mysex:setSelfGo(mySex);
    if 0 == GCtx.mGameData.mylevelRank.sex then
        self.mysex:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
    else
        self.mysex:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
    end

    --账户
    local myAccount = GlobalNS.UtilApi.getComByPath(self.mMyDanRankArea, "AccountName", "Text");
    myAccount.text = GCtx.mGameData.mylevelRank.acc;

    --段位
    local Dan_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyDanRankArea, "Dan_Panel");
    local leveldata = LuaExcelManager.level_level[GCtx.mGameData.mylevelRank.level];
    local LevelImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Panel, "Image");
    self.m_LevelImage = GlobalNS.new(GlobalNS.AuxImage);
    self.m_LevelImage:setSelfGo(LevelImage);
    self.m_LevelImage:setSpritePath("Atlas/DefaultSkin/Level.asset", leveldata.image);
    local LevelName = GlobalNS.UtilApi.getComByPath(Dan_Panel, "Text", "Text");
    LevelName.text = leveldata.name;

    local Star_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Panel, "Star_Panel");
    local king_Panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Dan_Panel, "king_Panel");
    if leveldata.type >= 16 then
        Star_Panel:SetActive(false);
        king_Panel:SetActive(true);
        local kingImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(king_Panel, "Image");
        local starnum = GlobalNS.UtilApi.getComByPath(kingImage, "Text", "Text");
        starnum.text = leveldata.star;
        if GCtx.mGameData.levelranklist:get(i).m_name == GCtx.mPlayerData.mHeroData.mMyselfAccount then
            starnum.text = "<color=#32c832ff>".. text .."</color>";
        end
    else
        Star_Panel:SetActive(true);
        king_Panel:SetActive(false);
        self.m_Star1Image = GlobalNS.new(GlobalNS.AuxImage);
        self.m_Star1Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Star_Panel, "Star1_Image"));
        self.m_Star2Image = GlobalNS.new(GlobalNS.AuxImage);
        self.m_Star2Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Star_Panel, "Star2_Image"));
        self.m_Star3Image = GlobalNS.new(GlobalNS.AuxImage);
        self.m_Star3Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Star_Panel, "Star3_Image"));
        self.m_Star4Image = GlobalNS.new(GlobalNS.AuxImage);
        self.m_Star4Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Star_Panel, "Star4_Image"));
        self.m_Star5Image = GlobalNS.new(GlobalNS.AuxImage);
        self.m_Star5Image:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Star_Panel, "Star5_Image"));

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

function M:updateUIData(ranktype)
    if self.cur_open ~= 0 then
        return;
    end
    self.ranktype = ranktype;
    if 0 == ranktype then --今日
        self.mScrollRect:SetActive(true);
        self.mYDScrollRect:SetActive(false);
        if GCtx.mGameData.historyranklistCount > 0 then
            self:SetMyRankInfo();
            self.scroll_rect_table.recordCount= GCtx.mGameData.historyranklistCount;
            self.scroll_rect_table:init();
            self.scroll_rect_table:Refresh(-1, -1);
        end
    
        --自己可能未进榜（积分为0）而排行榜也没有其他数据
        if GCtx.mGameData.historyranklistCount == 0 then
            self:SetMyRankInfo();
        end
    else --昨日
        self.mScrollRect:SetActive(false);
        self.mYDScrollRect:SetActive(true);
        if GCtx.mGameData.ydhistoryranklistCount > 0 then
            self:SetMyYDRankInfo();
            self.ydscroll_rect_table.recordCount= GCtx.mGameData.ydhistoryranklistCount;
            self.ydscroll_rect_table:init();
            self.ydscroll_rect_table:Refresh(-1, -1);
        end
    
        --自己可能未进榜（积分为0）而排行榜也没有其他数据
        if GCtx.mGameData.ydhistoryranklistCount == 0 then
            self:SetMyYDRankInfo();
        end
    end
end

function M:updateDanUIData()
    if self.cur_open ~= 1 then
        return;
    end
    if GCtx.mGameData.levelranklistCount > 0 then
        self:SetMyDanRankInfo();
        self.dan_scroll_rect_table.recordCount= GCtx.mGameData.levelranklistCount;
        self.dan_scroll_rect_table:init();
        self.dan_scroll_rect_table:Refresh(-1, -1);
    end
    
    --自己可能未进榜而排行榜也没有其他数据
    if GCtx.mGameData.levelranklistCount == 0 then
        self:SetMyDanRankInfo();
    end
end

return M;