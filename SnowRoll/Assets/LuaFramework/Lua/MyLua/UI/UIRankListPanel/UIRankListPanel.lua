MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

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

    --listitems数组
    self.listitems = { };
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
	self.mListitem_prefab:asyncLoad("UI/UIRankListPanel/ListItem.prefab", self, self.onPrefabLoaded); 
    
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
    self:exit();
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:dispose();
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIStartGame);
    --GlobalNS.CSSystem.Ctx.mInstance.mModuleSys:unloadModule(GlobalNS.CSSystem.ModuleId.GAMEMN);
    GlobalNS.CSSystem.Ctx.mInstance.mModuleSys:loadModule(GlobalNS.CSSystem.ModuleId.LOGINMN);
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
            local myHoner = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Honer", "Image");
            --myAvatar.Name = "Avatar/DefaultAvatar";
            --Sprite avatarSprite = Resources.Load("Avatar/DefaultAvatar", typeof(Sprite)) as Sprite;
            --myAvatar.overrideSprite = avatarSprite;

            --排名
            local myRank = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Rank", "Text");
            myRank.text = "" .. i;

            --头像
            local myAvatar = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Avatar", "Image");
            --myAvatar.Name = "Avatar/DefaultAvatar";
            --Sprite avatarSprite = Resources.Load("Avatar/DefaultAvatar", typeof(Sprite)) as Sprite;
            --myAvatar.overrideSprite = avatarSprite;

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
        local Honer = GlobalNS.UtilApi.getComByPath(listitem, "Honer", "Image");
        --Sprite avatarSprite = Resources.Load("Avatar/DefaultAvatar", typeof(Sprite)) as Sprite;
        --Avatar.overrideSprite = avatarSprite;

        --排名
        local Rank = GlobalNS.UtilApi.getComByPath(listitem, "Rank", "Text");
        Rank.text = "" .. i;

        --头像
        local Avatar = GlobalNS.UtilApi.getComByPath(listitem, "Avatar", "Image");
        --Sprite avatarSprite = Resources.Load("Avatar/DefaultAvatar", typeof(Sprite)) as Sprite;
        --Avatar.overrideSprite = avatarSprite;

        --用户名
        local Name = GlobalNS.UtilApi.getComByPath(listitem, "Name", "Text");
        Name.text = GCtx.mGameData.rankinfolist[i].m_name;

        --本轮质量
        local Mass = GlobalNS.UtilApi.getComByPath(listitem, "Mass", "Text");
        local radius = GlobalNS.UtilMath.getRadiusByMass(GCtx.mGameData.rankinfolist[i].m_radius); --服务器传过来的是质量
        Mass.text = GlobalNS.UtilMath.getShowMass(radius);

        --吞食数量
        local SwallowNum = GlobalNS.UtilApi.getComByPath(listitem, "SwallowNum", "Text");
        SwallowNum.text = GCtx.mGameData.rankinfolist[i].m_swallownum;   
    end
end

function M:updateUIData()
    if #self.listitems == GCtx.mGameData.ranklistCount then        
        self:SetMyRankInfo();
        self:SetTopXRankInfo();
    end
end

return M;