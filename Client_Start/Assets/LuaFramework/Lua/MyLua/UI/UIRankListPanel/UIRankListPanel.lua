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
	self.mId = GlobalNS.UIFormID.eUIRankListPanel;
	self.mData = GlobalNS.new(GlobalNS.RankListPanelNS.RankListPanelData);
    itemCount = 30;
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	self.listitem_height = 75;
    self.listitem_width = 1334;
    --返回游戏按钮
	self.mBackGameBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBackGameBtn:addEventHandle(self, self.onBtnClk);

    --listitems数组
    self.listitems = { };

    --排名信息
    self.topN = { };
    self.topN[1] = {m_isRobot=false, mName="528", m_radius=1, m_swallownum=10};
    for i=2,itemCount do
        self.topN[i] = {m_isRobot=true, mName="god", m_radius=i, m_swallownum=i+10};
    end
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
    --获取ScrollRect下Grid中的RectTransform组件
    self.mRankGrid = GlobalNS.UtilApi.getComByPath(self.mScrollRect, "Grid", "RectTransform");
    
    local gridHeight = itemCount * self.listitem_height - (itemCount - 1) * 2;
    self.mRankGrid.sizeDelta = Vector2.New(self.listitem_width, gridHeight);
    
    --获取MyRank的GameObject对象
    self.mMyRankArea = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.RankBG, "MyRank");

    --加载listitem prefab
    self.mListitem_prefab = GlobalNS.new(GlobalNS.AuxPrefabLoader);
	self.mListitem_prefab:asyncLoad("UI/UIRankListPanel/ListItem.prefab", self, self.onPrefabLoaded);
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
	GCtx.mLogSys:log("Back Game Btn Touch", GlobalNS.LogTypeId.eLogCommon);
end

function M:onPrefabLoaded(dispObj)
    --获取listitemprefab对象
    self.mListitemPrefab = self.mListitem_prefab:getPrefabTmpl();
    
    for i=1, itemCount do
        local y_pos = 197.5 - (self.listitem_height-2) * (i - 1);  --  197.5:第一个item的起始位置
        --用listitemprefab生成GameObject对象
        local listitem = GlobalNS.UtilApi.Instantiate(self.mListitemPrefab);
        listitem.transform.position = Vector3.New(0, y_pos, 0);
        listitem.transform.parent = self.mRankGrid;
        listitem.transform.localScale = Vector3.New(1.0, 1.0, 1.0);

        listitem.name = "Item" .. i;

        self.listitems[i] = listitem;
    end
    
    --滚动到起始位置，默认会在中间
    GlobalNS.UtilApi.getComByPath(self.RankBG, "ScrollRect", "ScrollRect").verticalNormalizedPosition = 1;

    self:SetMyRankInfo();
    self:SetTopXRankInfo();
end

--我的排名数据
function M:SetMyRankInfo()
    for i=1, itemCount do
        if(not self.topN[i].m_isRobot) then
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
            myName.text = self.topN[i].mName;

            --本轮质量
            local myMass = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "Mass", "Text");
            myMass.text = self.topN[i].m_radius;

            --吞食数量
            local mySwallowNum = GlobalNS.UtilApi.getComByPath(self.mMyRankArea, "SwallowNum", "Text");
            mySwallowNum.text = self.topN[i].m_swallownum;

            break;
        end
    end
end

--排行榜数据
function M:SetTopXRankInfo()
    for i=1, itemCount do
        local listitem = self.listitems[i].transform;
        --排名
        local Rank = GlobalNS.UtilApi.getComByPath(listitem, "Rank", "Text");
        Rank.text = "" .. i;

        --头像
        local Avatar = GlobalNS.UtilApi.getComByPath(listitem, "Avatar", "Image");
        --Sprite avatarSprite = Resources.Load("Avatar/DefaultAvatar", typeof(Sprite)) as Sprite;
        --Avatar.overrideSprite = avatarSprite;

        --用户名
        local Name = GlobalNS.UtilApi.getComByPath(listitem, "Name", "Text");
        Name.text = self.topN[i].mName;

        --本轮质量
        local Mass = GlobalNS.UtilApi.getComByPath(listitem, "Mass", "Text");
        Mass.text = self.topN[i].m_radius;

        --吞食数量
        local SwallowNum = GlobalNS.UtilApi.getComByPath(listitem, "SwallowNum", "Text");
        SwallowNum.text = self.topN[i].m_swallownum;   
    end
end

return M;