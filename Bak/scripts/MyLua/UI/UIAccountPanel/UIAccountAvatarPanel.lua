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

    self.mAvatarNum = 5;--Avatar目录下的头像数量
    self.index = 1;

    self.datas={};
    for i=1, self.mAvatarNum do
	    local it ={};
	    it.name = ""..i;
        it.avatarItemBtn = nil;
        it.listitem = nil;
	    table.insert(self.datas, it);
    end
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
    
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
    --底部
    local BottomBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "BottomBGImage");
	self.mOKBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BottomBG, "OK_BtnTouch"));

    self:on_scrollview_loaded(BG);
    self:updateUIData();
end

function M:on_scrollview_loaded(BG)
    --Avatar区
    local MiddlePanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "MiddlePanel");    
    self.scrollrect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(MiddlePanel, "ScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.scrollrect, "Viewport");
    self.scroll_rect_table = GlobalNS.UtilApi.getComByPath(viewport, "Content", "ScrollRectTable");

    self.scroll_rect_table.onItemRender = 
        function(scroll_rect_item, index)
            scroll_rect_item.gameObject:SetActive(true);
            scroll_rect_item.name = "Avatar" .. index;

            self.datas[index].avatarItemBtn = GlobalNS.new(GlobalNS.AuxButton);
            self.datas[index].avatarItemBtn:setSelfGo(scroll_rect_item.gameObject);
            self.datas[index].avatarItemBtn.param1 = index;
            self.datas[index].listitem = scroll_rect_item;
            self.datas[index].avatarItemBtn:addEventHandle(self, self.onChoiceBtnClk);
        
            self.datas[index].avatarItemBtn.mImage:setSelfGo(scroll_rect_item.gameObject);
        	self.datas[index].avatarItemBtn.mImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(index));

            local HotImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(scroll_rect_item.gameObject, "Flag");
            if self.index == index then
                HotImage:SetActive(true);
            else
                HotImage:SetActive(false);
            end
        end
         
    self.scroll_rect_table.onItemDispear = 
        function(index)
            if nil ~= self.datas[index].m_AvatarBtn then
                self.datas[index].avatarItemBtn:clearEventHandle();
            end
        end
end

function M:onChoiceBtnClk(dispObj)
    local index = dispObj.param1;
	self:SetAvatarItems(index);
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

function M:updateUIData()
     self.scroll_rect_table.recordCount= self.mAvatarNum;
     self.scroll_rect_table:init();
     self.scroll_rect_table:Refresh(-1,-1);
    
     self:SetAvatarItems(GCtx.mPlayerData.mHeroData.mAvatarIndex);
end

--物品数据
function M:SetAvatarItems(index)
    self.index = index;

    for i=1, self.mAvatarNum do
        --标识
        if nil ~= self.datas[i].listitem then
            local HotImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.datas[i].listitem.gameObject, "Flag");
            HotImage:SetActive(false);
        end
    end
    if nil ~= self.datas[index].listitem then
        local HotImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.datas[index].listitem.gameObject, "Flag");
        HotImage:SetActive(true);
    end
end

function M:onOKBtnClk()
    self:clearObj();
    
    if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUIAccountPanel) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIAccountPanel);
        if nil ~= form and form.mIsReady then
            form:resetAvatar(self.index);
        end
    end

    if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUIStartGame) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIStartGame);
        if nil ~= form and form.mIsReady then
            form:resetAvatar(self.index);
        end
    end

    GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("Avatar", self.index);
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ChangeHeaderImg(self.index);
	self:exit();
end

function M:onCloseBtnClk()
    self:clearObj();

	self:exit();
end

function M:clearObj()
    --清空
    for i=1, self.mAvatarNum do
        local avataritem = self.datas[i];
        if avataritem.avatarItemBtn ~= nil then
            avataritem.avatarItemBtn:dispose();
        end
    end
    self.datas = {};
end

return M;