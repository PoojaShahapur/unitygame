MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIAccountPanel.AccountPanelNS");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelData");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelCV");
MLoader("MyLua.UI.UIAccountPanel.AvatarItemData");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIAccountAvatarPanel";
GlobalNS.AccountPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIAccountAvatarPanel;
	self.mData = GlobalNS.new(GlobalNS.AccountPanelNS.AccountPanelData);

    self.mAvatarNum = 4;
    self.index = 1;
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
    
    self.mAvataritem_prefab = GlobalNS.new(GlobalNS.AuxPrefabLoader);
    self.isPrefabLoaded = false;
    self.avataritems = { };

    self.mCloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCloseBtn:addEventHandle(self, self.onCloseBtnClk);

	self.mOKBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mOKBtn:addEventHandle(self, self.onOKBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    local TopBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "TitleBGImage");
	self.mCloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(TopBG, "Close_BtnTouch"));
    
    --Avatar区
    local MiddlePanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "MiddlePanel");    
    self.scrollrect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(MiddlePanel, "ScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.scrollrect, "Viewport");
    self.AvatarContentRect = GlobalNS.UtilApi.getComByPath(viewport, "Content", "RectTransform");

    --加载avataritems
	self.mAvataritem_prefab:asyncLoad("UI/UIAccountPanel/AvatarItem.prefab", self, self.onPrefabLoaded, nil);    

    --底部
    local BottomBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "BottomBGImage");
	self.mOKBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BottomBG, "OK_BtnTouch"));
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

function M:onPrefabLoaded(dispObj)
    self.mAvatarItemPrefab = self.mAvataritem_prefab:getPrefabTmpl();
    self.isPrefabLoaded = true;

    self:updateUIData();
end

function M:CreateAvatarItem()
    if not self.isPrefabLoaded then
        return;
    end

    --清空
    for i=1, #self.avataritems do
        local avatarsitem = self.avataritems[i];
        GlobalNS.UtilApi.Destroy(avataritem.m_go);
    end
    self.avataritems = {};

    --重新生成
    for i=1, self.mAvatarNum do
        local avataritem = GlobalNS.new(GlobalNS.AvatarItemData);
        
        avataritem:init(self.mAvatarItemPrefab, self.AvatarContentRect, i);
        GlobalNS.UtilApi.setImageSprite(avataritem.m_go, "DefaultSkin/Avatar/"..i..".png");
        
        self.avataritems[i] = avataritem;
    end

    --滚动到起始位置，默认会在中间
    GlobalNS.UtilApi.GetComponent(self.scrollrect, "ScrollRect").verticalNormalizedPosition = 1;
end

function M:updateUIData()   
    self:CreateAvatarItem();

    if #self.avataritems == self.mAvatarNum then        
        self:SetAvatarItems(1);
    end
end

--物品数据
function M:SetAvatarItems(index)
    for i=1, self.mAvatarNum do
        --标识
        local HotImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.avataritems[i].m_go, "Flag");
        HotImage:SetActive(false);
    end
    local HotImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.avataritems[index].m_go, "Flag");
    HotImage:SetActive(true);
    self.index = index;
end

function M:onOKBtnClk()
    --清空
    for i=1, #self.avataritems do
        local avataritem = self.avataritems[i];
        GlobalNS.UtilApi.Destroy(avataritem.m_go);
    end
    self.avataritems = {};
    
    if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUIAccountPanel) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIAccountPanel);
        if nil ~= form and form.mIsReady then
            form:resetAvatar(self.index);
        end
    end

	self:exit();
end

function M:onCloseBtnClk()
	self:exit();
end

return M;