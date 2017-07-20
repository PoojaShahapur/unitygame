MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIRankListPanel.RankListPanelNS");
MLoader("MyLua.UI.UIRankListPanel.RankListPanelData");
MLoader("MyLua.UI.UIRankListPanel.RankListPanelCV");
MLoader("MyLua.UI.UIRankListPanel.RankListItem");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIRankListPanel";
GlobalNS.RankListPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIRankListPanel;
	self.mData = GlobalNS.new(GlobalNS.RankListPanelNS.RankListPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
    --返回游戏按钮
	self.mBackGameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBackGameBtn:addEventHandle(self, self.onBtnClk);
    --分享按钮
	self.mShareGameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mShareGameBtn:addEventHandle(self, self.onShareBtnClk);

    self.avatarimages = { };
    self.myavatar = nil;
    self.honerimages = {};
    self.myhoner = nil;
    self.seximages = {};
    self.mysex = nil;
end

function M:onReady()
    M.super.onReady(self);
    self.RankBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "RankListBG");
	self.mBackGameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.RankBG, 
			GlobalNS.RankListPanelNS.RankListPanelPath.BtnBackGame)
		);
    self.mShareGameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.RankBG,"Share_BtnTouch"));
    
    --获取MyRank的GameObject对象
    self.mMyRankArea = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.RankBG, "MyRank");

    self:on_scrollview_loaded();
    self:updateUIData();
end

function M:on_scrollview_loaded()
    --获取ScrollRect的GameObject对象
    self.mScrollRect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.RankBG, "ScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mScrollRect, "Viewport");
    --获取ScrollRect下Content中的ScrollRectTable组件
    self.scroll_rect_table = GlobalNS.UtilApi.getComByPath(viewport, "Content", "ScrollRectTable");
    self.pagesize = self.scroll_rect_table.pageSize;

    self.scroll_rect_table.onItemRender = 
        function(scroll_rect_item, index)
            if GlobalNS.UtilApi.IsUObjNil(scroll_rect_item) then
                return;
            end

            local i = index - 1;
            scroll_rect_item.gameObject:SetActive(true);
            scroll_rect_item.name = "RankItem" .. index;
            local rank = GCtx.mGameData.rankinfolist:get(i).m_rank;
            --排名图标
            if rank <= 3 then
               local Honer = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Honer");
               Honer:SetActive(true);
               local honer = GlobalNS.new(GlobalNS.AuxImage);
               honer:setSelfGo(Honer);
               if rank == 1 then
		    		--honer:setSpritePath("DefaultSkin/SkyWarSkin/rank1.png", "rank1");
		    		honer:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank1");
                   --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_gold");
               elseif rank == 2 then
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
            if rank <= 3 then
                Rank.text = "";
            else
                Rank.text = "" .. rank;
                if GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mGameData.rankinfolist:get(i).m_uid) then
                    Rank.text = "<color=#32c832ff>".. rank .."</color>";
                end
            end
            
            --头像
            local Avatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Avatar");
            local avatarImage = GlobalNS.new(GlobalNS.AuxImage);
            avatarImage:setSelfGo(Avatar);
            local avatarindex = GCtx.mGameData.rankinfolist:get(i).m_avatarindex;
            if avatarindex == 0 then
                if GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mGameData.rankinfolist:get(i).m_uid) then
                    avatarindex = 1;
                else
                    local _time = os.clock();
                    math.randomseed(_time + i);
                    avatarindex = math.random(1, 4);
                    GCtx.mGameData.rankinfolist:get(i).m_avatarindex = avatarindex;
                end
            end
	    	--avatarImage:setSpritePath("DefaultSkin/Avatar/"..avatarindex..".png", GlobalNS.UtilStr.tostring(avatarindex));
	    	avatarImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(avatarindex));
            --avatarImage:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(avatarindex));
            self.avatarimages[index] = avatarImage;

            --性别
            --[[local Sex = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Sex");
            local sexImage = GlobalNS.new(GlobalNS.AuxImage);
            sexImage:setSelfGo(Sex);
            if 0 == GCtx.mGameData.rankinfolist:get(i).m_sex then
                sexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
            else
                sexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
            end
            self.seximages[index] = sexImage;]]--

            GCtx.mGameData.rankinfolist:get(i).m_AvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
            GCtx.mGameData.rankinfolist:get(i).m_AvatarBtn:addEventHandle(self, self.onAvatarBtnClk);
            GCtx.mGameData.rankinfolist:get(i).m_AvatarBtn.param1 = GCtx.mGameData.rankinfolist:get(i).m_uid;
            GCtx.mGameData.rankinfolist:get(i).m_AvatarBtn.param2 = GCtx.mGameData.rankinfolist:get(i).m_name;
            GCtx.mGameData.rankinfolist:get(i).m_AvatarBtn:setSelfGo(scroll_rect_item.gameObject);
            GCtx.mGameData.rankinfolist:get(i).m_AvatarBtn:setIsDestroySelf(false);
            --AI不可查看
            if 1 == GCtx.mGameData.rankinfolist:get(i).m_isAI or GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mGameData.rankinfolist:get(i).m_uid) then
                GCtx.mGameData.rankinfolist:get(i).m_AvatarBtn:disable();
            else
                GCtx.mGameData.rankinfolist:get(i).m_AvatarBtn:enable();
            end

            --昵称
            local NickName = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "NickName", "Text");
            NickName.text = GCtx.mGameData.rankinfolist:get(i).m_nickname;
            if GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mGameData.rankinfolist:get(i).m_uid) then
                NickName.text = "<color=#32c832ff>"..GCtx.mGameData.rankinfolist:get(i).m_nickname.."</color>";
            end

            --用户名
            local AccountName = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "AccountName", "Text");
            if 1 == GCtx.mGameData.rankinfolist:get(i).m_isAI then
                AccountName.text = "笨蛋电脑";
            else
                AccountName.text = GCtx.mGameData.rankinfolist:get(i).m_name;
            end
            if GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mGameData.rankinfolist:get(i).m_uid) then
                AccountName.text = "<color=#32c832ff>"..GCtx.mGameData.rankinfolist:get(i).m_name.."</color>";
            end

            --积分
            local Score = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "Score", "Text");
            Score.text = GCtx.mGameData.rankinfolist:get(i).m_score;
            if GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mGameData.rankinfolist:get(i).m_uid) then
                Score.text = "<color=#32c832ff>"..GCtx.mGameData.rankinfolist:get(i).m_score.."</color>";
            end

            --击杀
            local KillNum = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "KillNum", "Text");
            KillNum.text = GCtx.mGameData.rankinfolist:get(i).m_killnum;
            if GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mGameData.rankinfolist:get(i).m_uid) then
                KillNum.text = "<color=#32c832ff>"..GCtx.mGameData.rankinfolist:get(i).m_killnum.."</color>";
            end

            --奖励1
            local Award_1 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Award_1");
            local Award_1_Num = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "Award_1_Num", "Text");
            local num = GCtx.mGameData.rankinfolist:get(i).m_award_1;
            local text = "" .. num;
            if 0 == num then
                Award_1:SetActive(false);
                text = "";
            else
                Award_1:SetActive(true);
            end
            Award_1_Num.text = text;
            if GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mGameData.rankinfolist:get(i).m_uid) then
                Award_1_Num.text = "<color=#32c832ff>".. text .."</color>";
            end

            --奖励2
            local Award_2 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Award_2");
            local Award_2_Num = GlobalNS.UtilApi.getComByPath(scroll_rect_item, "Award_2_Num", "Text");
            local num = GCtx.mGameData.rankinfolist:get(i).m_award_2;
            local text = "" .. num;
            if 0 == num then
                Award_2:SetActive(false);
                text = "";
            else
                Award_2:SetActive(true);
            end
            Award_2_Num.text = text;
            if GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mGameData.rankinfolist:get(i).m_uid) then
                Award_2_Num.text = "<color=#32c832ff>".. text .."</color>";
            end

            --关注按钮 (AI或自己不显示)
            GCtx.mGameData.rankinfolist:get(i).m_FocusBtn = GlobalNS.new(GlobalNS.AuxButton);
            GCtx.mGameData.rankinfolist:get(i).m_FocusBtn:addEventHandle(self, self.onFocusBtnClk);
            GCtx.mGameData.rankinfolist:get(i).m_FocusBtn.param1 = GCtx.mGameData.rankinfolist:get(i).m_uid;
            GCtx.mGameData.rankinfolist:get(i).m_FocusBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Focus_Btn"));
            GCtx.mGameData.rankinfolist:get(i).m_FocusBtn:setIsDestroySelf(false);
            if 1 == GCtx.mGameData.rankinfolist:get(i).m_isAI or GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mGameData.rankinfolist:get(i).m_uid) then
                GCtx.mGameData.rankinfolist:get(i).m_FocusBtn:hide();
            else
                GCtx.mGameData.rankinfolist:get(i).m_FocusBtn:show();
                if 1 == GCtx.mGameData.rankinfolist:get(i).m_isFllowing then
                    GCtx.mGameData.rankinfolist:get(i).m_FocusBtn:setText("已关注");
                    GCtx.mGameData.rankinfolist:get(i).m_FocusBtn:disable();
                else
                    GCtx.mGameData.rankinfolist:get(i).m_FocusBtn:setText("关注");
                    GCtx.mGameData.rankinfolist:get(i).m_FocusBtn:enable();
                end
            end
        end
     
     self.scroll_rect_table.onItemDispear = 
         function(index)
             if nil ~= GCtx.mGameData.rankinfolist:get(index-1).m_AvatarBtn then
                 GCtx.mGameData.rankinfolist:get(index-1).m_AvatarBtn:clearEventHandle();
             end
             if nil ~= GCtx.mGameData.rankinfolist:get(index-1).m_FocusBtn then
                 GCtx.mGameData.rankinfolist:get(index-1).m_FocusBtn:clearEventHandle();
             end
         end

     self.scroll_rect_table.onShowNewPage = 
         function(index)
             --请求是否已关注状态
             local endindex = index + self.pagesize - 1;
             local uids = "";
             local uidnum = 0;
             for i = index, endindex do
                if i <= GCtx.mGameData.ranklistCount then
                    if not GCtx.mGameData.rankinfolist:get(i-1).m_isReqed and 1 ~= GCtx.mGameData.rankinfolist:get(i-1).m_isAI and GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mGameData.rankinfolist:get(i-1).m_uid) then
                        uids = uids .. "," .. math.floor(GCtx.mGameData.rankinfolist:get(i-1).m_uid);
                        uidnum = uidnum + 1;
                    end
                end
             end

             if uidnum > 0 then
                GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ReqFocusState(uids);
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
    local uid = dispObj.param1;
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:Follow(uid);
end

function M:onShow()
    M.super.onShow(self);    
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
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

    M.super.onExit(self);
end

function M:onBtnClk()
    self:exit();
    --结束后返回大厅不需要发消息给服务器
    GCtx.mGameData:notifyBackHall();
end

function M:onShareBtnClk()
    GlobalNS.CSSystem.Ctx.mInstance.mCamSys:ShareTo3Party(GCtx.mPlayerData.mHeroData.mMyselfAccount);
end

--我的排名数据
function M:SetMyRankInfo()
    for i=1, GCtx.mGameData.ranklistCount do
        local index = i - 1;
        if(GCtx.mGameData.rankinfolist:get(index).m_rank == GCtx.mGameData.myRank) then

            --荣誉
            local myHoner = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Honer");
            if GCtx.mGameData.myRank > 3 then
                myHoner:SetActive(false);
            else
                myHoner:SetActive(true);
                self.myhoner = GlobalNS.new(GlobalNS.AuxImage);
                self.myhoner:setSelfGo(myHoner);
                if GCtx.mGameData.myRank == 1 then
					--self.myhoner:setSpritePath("DefaultSkin/SkyWarSkin/rank1.png", "rank1");
					self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "rank1");
                    --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_gold");
                elseif GCtx.mGameData.myRank == 2 then
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
            if GCtx.mGameData.myRank <= 3 then
                myRank.text = "";
            else
                myRank.text = "" .. GCtx.mGameData.myRank;
            end
            
            --头像
            local myAvatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Avatar");
            self.myavatar = GlobalNS.new(GlobalNS.AuxImage);
            self.myavatar:setSelfGo(myAvatar);
            local avatarindex = GCtx.mGameData.rankinfolist:get(index).m_avatarindex;
            if avatarindex == 0 then
                avatarindex = 1;
            end
			--self.myavatar:setSpritePath("DefaultSkin/Avatar/"..avatarindex..".png", GlobalNS.UtilStr.tostring(avatarindex));
			self.myavatar:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(avatarindex));
            --self.myavatar:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(avatarindex));

            --性别
            --[[local mySex = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Sex");
            self.mysex = GlobalNS.new(GlobalNS.AuxImage);
            self.mysex:setSelfGo(mySex);
            if 0 == GCtx.mGameData.rankinfolist:get(index).sex then
                self.mysex:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
            else
                self.mysex:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
            end]]--

            --昵称
            local NickName = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "NickName", "Text");
            NickName.text = GCtx.mGameData.rankinfolist:get(index).m_nickname;

            --用户名
            local AccountName = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "AccountName", "Text");
            AccountName.text = GCtx.mGameData.rankinfolist:get(index).m_name;
            
            --积分
            local myScoreNum = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Score", "Text");
            myScoreNum.text = GCtx.mGameData.rankinfolist:get(index).m_score;

            --击杀
            local KillNum = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "KillNum", "Text");
            KillNum.text = GCtx.mGameData.rankinfolist:get(index).m_killnum;

            --奖励1
            local Award_1 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Award_1");
            local Award_1_Num = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Award_1_Num", "Text");
            local num = GCtx.mGameData.rankinfolist:get(index).m_award_1;
            local text = "" .. num;
            if 0 == num then
                Award_1:SetActive(false);
                text = "";
            else
                Award_1:SetActive(true);
            end
            Award_1_Num.text = text;
        
            --奖励2
            local Award_2 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Award_2");
            local Award_2_Num = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Award_2_Num", "Text");
            local num = GCtx.mGameData.rankinfolist:get(index).m_award_2;
            local text = "" .. num;
            if 0 == num then
                Award_2:SetActive(false);
                text = "";
            else
                Award_2:SetActive(true);
            end
            Award_2_Num.text = text;

            break;
        end
    end
end

function M:updateRankItem(uid, is_fllowing)
    local item = nil;
    if(GCtx.mGameData.rankinfolist:ContainsKey(uid)) then
		item = GCtx.mGameData.rankinfolist:value(uid);
        item.m_isReqed = true;
        if is_fllowing then
            if nil ~= item.m_FocusBtn then
                item.m_FocusBtn:setText("已关注");
                item.m_FocusBtn:disable();
            end
            item.m_isFllowing = 1;
        else
            if nil ~= item.m_FocusBtn then
                item.m_FocusBtn:setText("关注");
            end
            item.m_isFllowing = 0;
        end
	end
end

function M:updateUIData()
    if GCtx.mGameData.ranklistCount > 0 then
        self:SetMyRankInfo();
        self.scroll_rect_table.recordCount= GCtx.mGameData.ranklistCount;
        self.scroll_rect_table:init();
        self.scroll_rect_table:Refresh(-1, -1);
    end
end

return M;