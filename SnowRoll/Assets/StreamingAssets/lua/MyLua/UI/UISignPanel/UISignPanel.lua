MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UISignPanel.SignPanelNS");
MLoader("MyLua.UI.UISignPanel.SignPanelData");
MLoader("MyLua.UI.UISignPanel.SignPanelCV");
MLoader("MyLua.UI.UISignPanel.ItemData");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UISignPanel";
GlobalNS.SignPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUISignPanel;
	self.mData = GlobalNS.new(GlobalNS.SignPanelNS.SignPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mCloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCloseBtn:addEventHandle(self, self.onCloseBtnClk);

    --底部签到按钮
    self.mSignBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mSignBtn:addEventHandle(self, self.onSignBtnClk);

    --前/后一天
    self.mBeforeBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBeforeBtn:addEventHandle(self, self.onBeforeBtnClk);
    self.mNextBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mNextBtn:addEventHandle(self, self.onNextBtnClk);

    --连续签到奖励
    self.m3DaysBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.m3DaysBtn:addEventHandle(self, self.on3DaysBtnClk);
    self.m5DaysBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.m5DaysBtn:addEventHandle(self, self.on5DaysBtnClk);
    self.m7DaysBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.m7DaysBtn:addEventHandle(self, self.on7DaysBtnClk);

    --item prefab
    self.mItem_prefab = GlobalNS.new(GlobalNS.AuxPrefabLoader);
    self.isPrefabLoaded = false;
    --items gameobject数组
    self.items = { };
end

function M:onReady()
    M.super.onReady(self);
    local titlePanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "TitlePanel");
	self.mCloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(titlePanel, "Close_BtnTouch"));

    local dataPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "DataPanel");
    local bottomPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(dataPanel, "BottomPanel");
	self.mSignBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(bottomPanel, "Sign_BtnTouch"));

    local middlePanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(dataPanel, "MiddlePanel");
    local date = GlobalNS.UtilApi.TransFindChildByPObjAndPath(middlePanel, "Date");
    self.dateText = GlobalNS.UtilApi.getComByPath(date, "Text", "Text");
    self.mBeforeBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(date, "Before_BtnTouch"));
    self.mNextBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(date, "Next_BtnTouch"));

    local otherAward = GlobalNS.UtilApi.TransFindChildByPObjAndPath(middlePanel, "OtherAward");
    self.Award3 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(otherAward, "Award3");
    self.m3DaysBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Award3, "3Days_BtnTouch"));
    self.Award5 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(otherAward, "Award5");
    self.m5DaysBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Award5, "5Days_BtnTouch"));
    self.Award7 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(otherAward, "Award7");
    self.m7DaysBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.Award7, "7Days_BtnTouch"));

    local dayAward = GlobalNS.UtilApi.TransFindChildByPObjAndPath(middlePanel, "DayAward");
    self.Tip = GlobalNS.UtilApi.getComByPath(dayAward, "Tip", "Text");

    self.scrollrect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(dayAward, "ScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.scrollrect, "Viewport");
    self.Content = GlobalNS.UtilApi.getComByPath(viewport, "Content", "RectTransform");
    --加载items
	self.mItem_prefab:asyncLoad("UI/UISignPanel/DayItem.prefab", self, self.onPrefabLoaded);
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
    GCtx.mSignData.day = 0;
end

function M:onCloseBtnClk()
    --清空
    for i=1, #self.items do
        local item = self.items[i];
        GlobalNS.UtilApi.Destroy(item.m_go);
    end
    self.items = {};

	self:exit();
end

function M:onSignBtnClk()
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIDayAwardPanel);
end

function M:onBeforeBtnClk()
	GCtx.mLogSys:log("Before", GlobalNS.LogTypeId.eLogCommon);
end

function M:onNextBtnClk()
	GCtx.mLogSys:log("next", GlobalNS.LogTypeId.eLogCommon);
end

function M:on3DaysBtnClk()
    GCtx.mSignData.rangeDay = 3;
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIOtherAwardPanel);
end

function M:on5DaysBtnClk()
    GCtx.mSignData.rangeDay = 5;
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIOtherAwardPanel);
end

function M:on7DaysBtnClk()
    GCtx.mSignData.rangeDay = 7;
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIOtherAwardPanel);
end

function M:onPrefabLoaded(dispObj)
    --获取item prefab对象
    self.mItemprefab = self.mItem_prefab:getPrefabTmpl();
    self.isPrefabLoaded = true;

    GCtx.mSignData:getSignAwards(nil);
end

function M:CreateItems()
    if not self.isPrefabLoaded then
        return;
    end

    --清空
    for i=1, #self.items do
        local item = self.items[i];
        GlobalNS.UtilApi.Destroy(item.m_go);
    end
    self.items = {};

    --重新生成
    for i=1, GCtx.mSignData.daysCount do
        local item = GlobalNS.new(GlobalNS.ItemData);
        item:init(self.mItemprefab, self.Content, i);

        self.items[i] = item;
    end

    --滚动到起始位置，默认会在中间
    --GlobalNS.UtilApi.GetComponent(self.scrollrect, "ScrollRect").verticalNormalizedPosition = 1;
end

function M:updateUIData()
    self:CreateItems();

    if #self.items == GCtx.mSignData.daysCount then        
        self:SetItems();
    end
end

--物品数据
function M:SetItems()
    self.dateText.text = "mm月dd日";
    self.Tip.text = "已连续签到:x天";

    for i=1, GCtx.mSignData.daysCount do
        local item = self.items[i].m_go;
        
        local day = GlobalNS.UtilApi.getComByPath(item, "Text", "Text");
        day.text = i;

        if not GCtx.mSignData.items[i].m_itemEnable then
            GlobalNS.UtilApi.disableBtn(self.items[i].m_go);
        end
    end
end

function M:enableBtn(index)
    GlobalNS.UtilApi.enableBtn(self.items[index].m_go);
end

return M;