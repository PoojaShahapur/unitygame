MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIRankListPanel.RankListPanelNS");
MLoader("MyLua.UI.UIRankListPanel.RankListPanelData");
MLoader("MyLua.UI.UIRankListPanel.RankListPanelCV");

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

    --listitem prefab
    self.mListitem_prefab = GlobalNS.new(GlobalNS.AuxPrefabLoader);
	self.mListitem_prefab:setIsNeedInsPrefab(false);

    --listitems数组
    self.listitems = { };
    self.avatarimages = { };
    self.myavatar = nil;
    self.honerimages = {};
    self.myhoner = nil;
end

function M:onReady()
    M.super.onReady(self);
    self.RankBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "RankListBG");
	self.mBackGameBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.RankBG, 
			GlobalNS.RankListPanelNS.RankListPanelPath.BtnBackGame)
		);

    --获取ScrollRect的GameObject对象
    self.mScrollRect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.RankBG, "ScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mScrollRect, "Viewport");
    --获取ScrollRect下Content中的RectTransform组件
    self.mRankContent = GlobalNS.UtilApi.getComByPath(viewport, "Content", "RectTransform");
    
    --获取MyRank的GameObject对象
    self.mMyRankArea = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.RankBG, "MyRank");

    --加载listitem prefab
	self.mListitem_prefab:asyncLoad("UI/UIRankListPanel/ListItem.prefab", self, self.onPrefabLoaded, nil); 
    
    self:updateUIData();
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
    self.mListitem_prefab:dispose();
    self:exit();
    
    GCtx.mGameData:returnStartGame();
end

function M:onPrefabLoaded(dispObj)
    --获取listitemprefab对象
    self.mListitemPrefab = self.mListitem_prefab:getPrefabTmpl();
    
    for i=1, GCtx.mGameData.ranklistCount do
        --用listitemprefab生成GameObject对象
        local listitem = GlobalNS.UtilApi.Instantiate(self.mListitemPrefab);
        listitem.transform.parent = self.mRankContent;
        listitem.transform.localScale = Vector3.New(1.0, 1.0, 1.0);

        listitem.name = "Item" .. i;

        self.listitems[i] = listitem;
    end

    --滚动到起始位置，默认会在中间
    GlobalNS.UtilApi.GetComponent(self.mScrollRect, "ScrollRect").verticalNormalizedPosition = 1;

    self:updateUIData();
end

--我的排名数据
function M:SetMyRankInfo()
    for i=1, GCtx.mGameData.ranklistCount do
        if(GCtx.mGameData.rankinfolist[i].m_rank == GCtx.mGameData.myRank) then

            --荣誉
            local myHoner = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Honer");
            if GCtx.mGameData.myRank > 3 then
                myHoner:SetActive(false);
            else
                myHoner:SetActive(true);
                self.myhoner = GlobalNS.new(GlobalNS.AuxImage);
                self.myhoner:setSelfGo(myHoner);
                if GCtx.mGameData.myRank == 1 then
					self.myhoner:setSpritePath("DefaultSkin/GameOption/cup_gold.png", "cup_gold");
                    --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_gold");
                elseif GCtx.mGameData.myRank == 2 then
					self.myhoner:setSpritePath("DefaultSkin/GameOption/cup_yin.png", "cup_yin");
                    --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_yin");
                else
					self.myhoner:setSpritePath("DefaultSkin/GameOption/cup_tong.png", "cup_tong");
                    --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_tong");
                end
            end

            --排名
            local myRank = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Rank", "Text");
            myRank.text = "" .. i;

            --头像
            local myAvatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "Avatar");
            self.myavatar = GlobalNS.new(GlobalNS.AuxImage);
            self.myavatar:setSelfGo(myAvatar);
            local avatarindex = GCtx.mGameData.rankinfolist[i].m_avatarindex;
            if avatarindex == 0 then
                avatarindex = 1;
            end
			self.myavatar:setSpritePath("DefaultSkin/Avatar/"..avatarindex..".png", GlobalNS.UtilStr.tostring(avatarindex));
            --self.myavatar:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(avatarindex));

            --用户名
            local myName = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Name", "Text");
            myName.text = GCtx.mGameData.rankinfolist[i].m_name;

            --本轮质量
            local myMass = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Mass", "Text");
            local radius = GlobalNS.UtilMath.getRadiusByMass(GCtx.mGameData.rankinfolist[i].m_radius); --服务器传过来的是质量
            myMass.text = GlobalNS.UtilMath.getShowMass(radius);

            --吞食数量
            local mySwallowNum = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "SwallowNum", "Text");
            mySwallowNum.text = GCtx.mGameData.rankinfolist[i].m_swallownum;

            break;
        end
    end
end

--排行榜数据
function M:SetTopXRankInfo()
    for i=1, GCtx.mGameData.ranklistCount do
        local listitem = self.listitems[i].transform;
        
        --荣誉
        local Honer = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.listitems[i], "Honer");
        if i > 3 then
            Honer:SetActive(false);
        else
            Honer:SetActive(true);
            local honer = GlobalNS.new(GlobalNS.AuxImage);
            honer:setSelfGo(Honer);
            if i == 1 then
				honer:setSpritePath("DefaultSkin/GameOption/cup_gold.png", "cup_gold");
                --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_gold");
            elseif i == 2 then
				honer:setSpritePath("DefaultSkin/GameOption/cup_yin.png", "cup_yin");
                --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_yin");
            else
				honer:setSpritePath("DefaultSkin/GameOption/cup_tong.png", "cup_tong");
                --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_tong");
            end
            self.honerimages[i] = honer;
         end

        --排名
        local Rank = GlobalNS.UtilApi.getComByPath(listitem, "Rank", "Text");
        Rank.text = "" .. i;
        if GCtx.mGameData.rankinfolist[i].m_rank == GCtx.mGameData.myRank then
            Rank.text = "<color=#32c832ff>"..i.."</color>";
        end

        --头像
        local Avatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.listitems[i], "Avatar");
        local avatarImage = GlobalNS.new(GlobalNS.AuxImage);
        avatarImage:setSelfGo(Avatar);
        local avatarindex = GCtx.mGameData.rankinfolist[i].m_avatarindex;
        if avatarindex == 0 then
            local socket = require("socket") -- 需要用到luasocket库  
            local t = string.format("%f", socket.gettime())  
            local st = string.sub(t, string.find(t, "%.") + 1, -1)
            math.randomseed(tonumber(string.reverse(st)));
            avatarindex = math.random(1, 4);
        end
		avatarImage:setSpritePath("DefaultSkin/Avatar/"..avatarindex..".png", GlobalNS.UtilStr.tostring(avatarindex));
        --avatarImage:setSpritePath("DefaultSkin/Avatar/Avatar_RGB.png", GlobalNS.UtilStr.tostring(avatarindex));
        self.avatarimages[i] = avatarImage;

        --用户名
        local Name = GlobalNS.UtilApi.getComByPath(listitem, "Name", "Text");
        Name.text = GCtx.mGameData.rankinfolist[i].m_name;
        if GCtx.mGameData.rankinfolist[i].m_rank == GCtx.mGameData.myRank then
            Name.text = "<color=#32c832ff>"..GCtx.mGameData.rankinfolist[i].m_name.."</color>";
        end

        --本轮质量
        local Mass = GlobalNS.UtilApi.getComByPath(listitem, "Mass", "Text");
        local radius = GlobalNS.UtilMath.getRadiusByMass(GCtx.mGameData.rankinfolist[i].m_radius); --服务器传过来的是质量
        Mass.text = GlobalNS.UtilMath.getShowMass(radius);
        if GCtx.mGameData.rankinfolist[i].m_rank == GCtx.mGameData.myRank then
            Mass.text = "<color=#32c832ff>"..GlobalNS.UtilMath.getShowMass(radius).."</color>";
        end

        --吞食数量
        local SwallowNum = GlobalNS.UtilApi.getComByPath(listitem, "SwallowNum", "Text");
        SwallowNum.text = GCtx.mGameData.rankinfolist[i].m_swallownum;
        if GCtx.mGameData.rankinfolist[i].m_rank == GCtx.mGameData.myRank then
            SwallowNum.text = "<color=#32c832ff>"..GCtx.mGameData.rankinfolist[i].m_swallownum.."</color>";
        end
    end
end

function M:updateUIData()
    if #self.listitems == GCtx.mGameData.ranklistCount then        
        self:SetMyRankInfo();
        self:SetTopXRankInfo();
    end
end

return M;