MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UITopXRankPanel.TopXRankPanelNS");
MLoader("MyLua.UI.UITopXRankPanel.TopXRankPanelData");
MLoader("MyLua.UI.UITopXRankPanel.TopXRankPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UITopXRankPanel";
GlobalNS.TopXRankPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUITopXRankPanel;
	self.mData = GlobalNS.new(GlobalNS.TopXRankPanelNS.TopXRankPanelData);
    
    self.mDropBtn = nil;
    self.mDownBtn = nil;

    --listitem prefab
    self.mListitem_prefab = GlobalNS.new(GlobalNS.AuxPrefabLoader);
	self.mListitem_prefab:setIsNeedInsPrefab(false);

    --listitems数组
    self.listitems = { };
    self.topxitemnum = 8;
    self.honerimages = {};
    self.myhoner = nil;
end

function M:dtor()
	self.mDropBtn:dispose();
    self.mDownBtn:dispose();
end

function M:onInit()
    M.super.onInit(self);

    self.mDropBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mDropBtn:addEventHandle(self, self.onDropBtnClk);

    self.mDownBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mDownBtn:addEventHandle(self, self.onDownBtnClk);
end

function M:onReady()
    M.super.onReady(self);

    --topx
    self.topXBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "TopXBG");
    local title = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.topXBG, "Title");
	self.mDropBtn:setSelfGo(title);
    --获取ScrollRect的GameObject对象
    self.mScrollRect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.topXBG, "ScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mScrollRect, "Viewport");
    --获取ScrollRect下Content中的RectTransform组件
    self.mRankContent = GlobalNS.UtilApi.getComByPath(viewport, "Content", "RectTransform");

    --获取MyRank的GameObject对象
    self.mMyRankArea = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.topXBG, "myrank");

    --加载listitem prefab
	self.mListitem_prefab:asyncLoad("UI/UITopXRankPanel/TopxListItem.prefab", self, self.onPrefabLoaded, nil);

    --收起排行榜
    self.UpBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "UP_bg");
    title = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.UpBG, "Title");
	self.mDownBtn:setSelfGo(title);

    if #self.listitems == self.topxitemnum then
        self:showTopxRank();
    end
end

function M:createItems()
    --获取listitemprefab对象
    self.mListitemPrefab = self.mListitem_prefab:getPrefabTmpl();
    
    if nil ~= self.mListitemPrefab then
        for i=1, self.topxitemnum do
            --用listitemprefab生成GameObject对象
            local listitem = GlobalNS.UtilApi.Instantiate(self.mListitemPrefab);
            --listitem.transform.parent = self.mRankContent;
			GlobalNS.CSSystem.SetParent(listitem.transform, self.mRankContent);
            listitem.transform.localScale = Vector3.New(1.0, 1.0, 1.0);
    
            listitem.name = "Top" .. i;
            if i > 3 then
                local Honer = GlobalNS.UtilApi.TransFindChildByPObjAndPath(listitem, "rank");
                GlobalNS.UtilApi.DestroyImmediate(Honer);
            else
                local Rank = GlobalNS.UtilApi.TransFindChildByPObjAndPath(listitem, "rank_Text");
                GlobalNS.UtilApi.DestroyImmediate(Rank);
            end

            self.listitems[i] = listitem;
        end
    
        --滚动到起始位置，默认会在中间
        GlobalNS.UtilApi.GetComponent(self.mScrollRect, "ScrollRect").verticalNormalizedPosition = 1;
    end
end

function M:onPrefabLoaded(dispObj)
    self:createItems();
    if #self.listitems == self.topxitemnum then
        self:showTopxRank();
    end
end

function M:showTopxRank()
    --topx
    for i=1, GCtx.mGameData.top10Count do
        self.listitems[i]:SetActive(true);
        local listitem = self.listitems[i].transform;

        --排名图标
        if i <= 3 then
            local Honer = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.listitems[i], "rank");
            local honerTransform = GlobalNS.UtilApi.GetComponent(Honer, "RectTransform");
            local honer = GlobalNS.new(GlobalNS.AuxImage);
            honer:setSelfGo(Honer);
            if i == 1 then
                honerTransform.sizeDelta = Vector2.New(30, 20);
				--honer:setSpritePath("DefaultSkin/SkyWarSkin/topx1.png", "topx1");
				honer:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "topx1");
                --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_gold");
            elseif i == 2 then
                honerTransform.sizeDelta = Vector2.New(20, 20);
				--honer:setSpritePath("DefaultSkin/SkyWarSkin/topx2.png", "topx2");
				honer:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "topx2");
                --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_yin");
            else
                honerTransform.sizeDelta = Vector2.New(20, 20);
				--honer:setSpritePath("DefaultSkin/SkyWarSkin/topx3.png", "topx3");
				honer:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "topx3");
                --honer:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_tong");
            end
            self.honerimages[i] = honer;
         end

        --排名
        if i > 3 then
            local Rank = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.listitems[i], "rank_Text");
            local Ranktext = GlobalNS.UtilApi.getComByPath(listitem, "rank_Text", "Text");
            if i == GCtx.mGameData.top10_myrank then
                Ranktext.text = "<color=#32c832ff>"..i.."</color>";
            else
                Ranktext.text = "" .. i;
            end
        end

        --用户名
        local Name = GlobalNS.UtilApi.getComByPath(listitem, "name", "Text");        
        if i == GCtx.mGameData.top10_myrank then
            Name.text = "<color=#32c832ff>"..GCtx.mGameData.top10ranklist[i].m_name.."</color>";
        else
            Name.text = GCtx.mGameData.top10ranklist[i].m_name;
        end
    end

    --多余的项隐藏掉
    if GCtx.mGameData.top10Count < self.topxitemnum then
        for j = GCtx.mGameData.top10Count + 1, self.topxitemnum do
            self.listitems[j]:SetActive(false);
        end
    end

    --我的排名
    local myHoner = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "rank");
    local myrank = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mMyRankArea, "rank_Text");
    if GCtx.mGameData.top10_myrank > 3 then
        myHoner:SetActive(false);
        myrank:SetActive(true);
        local myRanktext = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "rank_Text", "Text");
        myRanktext.text = "" .. GCtx.mGameData.top10_myrank;
    else
        myrank:SetActive(false);
        myHoner:SetActive(true);
        local myhonerTransform = GlobalNS.UtilApi.GetComponent(myHoner, "RectTransform");
        self.myhoner = GlobalNS.new(GlobalNS.AuxImage);
        self.myhoner:setSelfGo(myHoner);
        if GCtx.mGameData.top10_myrank == 1 then
            myhonerTransform.sizeDelta = Vector2.New(30, 20);
			--self.myhoner:setSpritePath("DefaultSkin/SkyWarSkin/topx1.png", "topx1");
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "topx1");
            --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_gold");
        elseif GCtx.mGameData.top10_myrank == 2 then
            myhonerTransform.sizeDelta = Vector2.New(20, 20);
			--self.myhoner:setSpritePath("DefaultSkin/SkyWarSkin/topx2.png", "topx2");
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "topx2");
            --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_yin");
        else
            myhonerTransform.sizeDelta = Vector2.New(20, 20);
			--self.myhoner:setSpritePath("DefaultSkin/SkyWarSkin/topx3.png", "topx3");
			self.myhoner:setSpritePath("Atlas/DefaultSkin/SkyWarSkin.asset", "topx3");
            --self.myhoner:setSpritePath("DefaultSkin/GameOption/GameOption_RGB.png", "cup_tong");
        end
    end
    local myName = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "name", "Text");
    myName.text = GCtx.mGameData.mMyName;
end

function M:onDropBtnClk()
    --GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIConsoleDlg);
    self.topXBG:SetActive(false);
    self.UpBG:SetActive(true);
	
	GCtx.mGameData:ShowRollMessage(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eObject, 6));
end

function M:onDownBtnClk()
    --GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIConsoleDlg);
    self.topXBG:SetActive(true);
    self.UpBG:SetActive(false);
	
	GCtx.mGameData:ShowRollMessage(GCtx.mLangMgr:getText(GlobalNS.LangTypeId.eObject, 6));
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
    self:clearResource();
    self.mListitem_prefab:dispose();
end

function M:clearResource()
    if self.myhoner ~= nil then
        self.myhoner:dispose();
        self.myhoner = nil;
    end
    for i=1, #self.honerimages do
        self.honerimages[i]:dispose();
    end
    self.honerimages = {};

    for i=1, #self.listitems do
        local item = self.listitems[i];
        GlobalNS.UtilApi.Destroy(item);
    end
    self.listitems = { };
end

function M:updateUIData()
    if #self.listitems == self.topxitemnum then
        self:showTopxRank();
    end
end

return M;