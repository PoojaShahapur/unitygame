MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIFindFriend.FindFriendNS");
MLoader("MyLua.UI.UIFindFriend.FindFriendData");
MLoader("MyLua.UI.UIFindFriend.FindFriendCV");
MLoader("MyLua.UI.UIFindFriend.FocusItem");
MLoader("MyLua.UI.UIFindFriend.FansItem");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIFindFriend";
GlobalNS.FindFriendNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIFindFriend;
	self.mData = GlobalNS.new(GlobalNS.FindFriendNS.FindFriendData);

    self.cur_open = 0;
    self.mTabPanel = GlobalNS.new(GlobalNS.TabPageMgr);
    self.mTabPanel.mTabClickEventDispatch:addEventHandle(self, self.onTabClick);
    self.mFindFriendAccount = "";
    self.mFindFrienduid = 0;

    self.focusitems = { };
    self.fansitems = { };
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mBackBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBackBtn:addEventHandle(self, self.onBackBtnClk);

    self.mWechatBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mWechatBtn:addEventHandle(self, self.onWechatBtnClk);
    self.mQQBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mQQBtn:addEventHandle(self, self.onQQBtnClk);
    self.mShareBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mShareBtn:addEventHandle(self, self.onShareBtnClk);
    self.mAvatarImage = GlobalNS.new(GlobalNS.AuxImage);
    self.mAvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.mAvatarBtn:addEventHandle(self, self.onAvatarBtnClk);
    self.mSexImage = GlobalNS.new(GlobalNS.AuxImage);
    self.mFocusBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mFocusBtn:addEventHandle(self, self.onFocusBtnClk);

    self.mFocusitem_prefab = GlobalNS.new(GlobalNS.AuxPrefabLoader);
	self.mFocusitem_prefab:setIsNeedInsPrefab(false);
    self.mFocusItemPrefabLoaded = false;
    self.mFocusBeforeBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mFocusBeforeBtn:addEventHandle(self, self.onFocusBeforeBtnClk);
    self.mFocusNextBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mFocusNextBtn:addEventHandle(self, self.onFocusNextBtnClk);

    self.mFansitem_prefab = GlobalNS.new(GlobalNS.AuxPrefabLoader);
	self.mFansitem_prefab:setIsNeedInsPrefab(false);
    self.mFansItemPrefabLoaded = false;
    self.mFansBeforeBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mFansBeforeBtn:addEventHandle(self, self.onFansBeforeBtnClk);
    self.mFansNextBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mFansNextBtn:addEventHandle(self, self.onFansNextBtnClk);

    --加载listitem prefab
	self.mFocusitem_prefab:asyncLoad("UI/UIFindFriend/Item_guanzhu.prefab", self, self.onFocusPrefabLoaded, nil);
    self.mFansitem_prefab:asyncLoad("UI/UIFindFriend/Item_fans.prefab", self, self.onFansPrefabLoaded, nil);
end

function M:onFocusPrefabLoaded()
    self.mFocusItemPrefabLoaded = true;
end

function M:onFansPrefabLoaded()
    self.mFansItemPrefabLoaded = true;
end

function M:onReady()
    M.super.onReady(self);
	self.mBackBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Button_back"));

    --找朋友
    self:initFindFriendCom();
    
    --关注
    self:initFocusCom();

    --粉丝
    self:initFansCom();
    
    --默认打开
    self.mTabPanel:openPage(GCtx.mSocialData.mCurOpenTag);
end

function M:initFindFriendCom()
     local findfriend_panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "findfriend_panel");
     self.mFindFriendNameInput = GlobalNS.UtilApi.getComByPath(findfriend_panel, "InputName", "InputField");
     --self.mWechatBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(findfriend_panel, "Button_wechat"));
     --self.mQQBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(findfriend_panel, "Button_QQ"));
     self.mShareBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(findfriend_panel, "Button_Share"));
     --输入完成事件
     self.mFindFriendInput = GlobalNS.UtilApi.TransFindChildByPObjAndPath(findfriend_panel, "InputName");
     GlobalNS.UtilApi.addInputEndHandle(self.mFindFriendInput, self, self.onFindFriendInputEnd);

     self.FindFriend_Account = GlobalNS.UtilApi.TransFindChildByPObjAndPath(findfriend_panel, "Items_Account");
     self.mAvatarImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.FindFriend_Account, "Image_touxiang"));
     self.mAvatarBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.FindFriend_Account, "Image_touxiang"));
     self.mSexImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.FindFriend_Account, "Image_sex"));
     self.mFocusBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.FindFriend_Account, "Button_guanzhu"));
     self.mAccount = GlobalNS.UtilApi.getComByPath(self.FindFriend_Account, "Text_name", "Text");
     self.mAge_City = GlobalNS.UtilApi.getComByPath(self.FindFriend_Account, "Text_place", "Text");

     local page = self.mTabPanel:addTabPage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Button_findfriend"), findfriend_panel);
     page:setTag(0);
end

function M:onFindFriendInputEnd(text)
    if not GlobalNS.UtilStr.IsNullOrEmpty(text) then
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:FindFriend(text);
    end
end

function M:initFocusCom()
    local focus_panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "focus_panel");
    local Items_sousuo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(focus_panel, "Items_sousuo");
    self.mFoucsNameInput = GlobalNS.UtilApi.getComByPath(Items_sousuo, "FocusName_Input", "InputField");
    --输入完成事件
    self.mFoucsInput = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Items_sousuo, "FocusName_Input");
    GlobalNS.UtilApi.addInputEndHandle(self.mFoucsInput, self, self.onFocusInputEnd);

    --获取ScrollRect的GameObject对象
    self.mFocusScrollRect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(focus_panel, "ScrollView");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mFocusScrollRect, "Viewport");
    --获取ScrollRect下Content中的RectTransform组件
    self.mFocusContent = GlobalNS.UtilApi.getComByPath(viewport, "Content", "RectTransform");
    
    self.mFocusBeforeBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(focus_panel, "Button_before"));
    self.mFocusNextBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(focus_panel, "Button_next"));
    self.mFocusPageNum = GlobalNS.UtilApi.getComByPath(focus_panel, "Page_number", "Text");
    self.mFocusCurPage = 1;

    local page = self.mTabPanel:addTabPage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Button_focus"), focus_panel);
    page:setTag(1);
end

function M:onFocusInputEnd(text)
    if not GlobalNS.UtilStr.IsNullOrEmpty(text) then
        local account = text;
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:FindFriendInFocuslist(account);
    end
end

function M:initFansCom()
    local fans_panel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "fans_panel");
    local Items_sousuo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(fans_panel, "Items_sousuo");
    self.mFansNameInput = GlobalNS.UtilApi.getComByPath(Items_sousuo, "FansName_Input", "InputField");
    --输入完成事件
    self.mFansInput = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Items_sousuo, "FansName_Input");
    GlobalNS.UtilApi.addInputEndHandle(self.mFansInput, self, self.onFansInputEnd);
    --获取ScrollRect的GameObject对象
    self.mFansScrollRect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(fans_panel, "ScrollView");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mFansScrollRect, "Viewport");
    --获取ScrollRect下Content中的RectTransform组件
    self.mFansContent = GlobalNS.UtilApi.getComByPath(viewport, "Content", "RectTransform");

    self.mFansBeforeBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(fans_panel, "Button_before"));
    self.mFansNextBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(fans_panel, "Button_next"));
    self.mFansPageNum = GlobalNS.UtilApi.getComByPath(fans_panel, "Page_number", "Text");
    self.mFansCurPage = 1;

    local page = self.mTabPanel:addTabPage(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Button_fans"), fans_panel);
    page:setTag(2);
end

function M:onFansInputEnd(text)
    if not GlobalNS.UtilStr.IsNullOrEmpty(text) then
        local account = text;
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:FindFriendInFanslist(account);
    end
end

function M:onTabClick(dispObj)
	local tag = dispObj:getCurPageTag();
    self.cur_open = tag;
    if 0 == tag then --找好友
        --GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ReqMainPageInfo(GCtx.mPlayerData.mHeroData.mViewAccount);
        self:updateFindFriendData(false, nil);
    elseif 1 == tag then--关注列表
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ViewFollowingList(GCtx.mPlayerData.mHeroData.mViewAccount, 1);
        self.mFoucsNameInput.text = "";
    elseif 2 == tag then--粉丝列表
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ViewFollowedList(GCtx.mPlayerData.mHeroData.mViewAccount, 1);
        self.mFansNameInput.text = "";
    else
        
    end
end

function M:updateFindFriendData(isservernotify, args)
    if 0 ~= self.cur_open then
        return;
    end

    local avatarindex = 1;
    local sex = 1;
    local account = "未知";
    local age = 0;
    local area_code = 1;
    local level = 0;
    local state = 1;
    local already_follow = 0;
    local uid = 0;
    self.FindFriendSuccess = false;

    if isservernotify then --服务器推送的数据
        avatarindex = args.header_imgid;
        sex = args.sex;
        account = args.account;
        age = args.age;
        area_code = args.area_code;
        level = args.level;
        state = args.state;
        already_follow = args.already_follow;
        uid = args.uid;

        self.FindFriendSuccess = true;
        self.FindFriend_Account:SetActive(true);
    else
        self.FindFriend_Account:SetActive(false);
    end

    self.mAvatarImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(avatarindex));
    if 0 == sex then
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
    else
        self.mSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
    end

    self.mFindFriendAccount = account;
    self.mFindFrienduid = uid;
    self.mAccount.text = account;
    self.mAge_City.text = age .. "岁 /" .. GCtx.mSocialData.citys[area_code];

    self:updateFoucsBtnText(uid, already_follow);
end

function M:updateFoucsBtnText(uid, already_follow)
    if 0 == self.cur_open then
        if 1 == already_follow then
            self.mFocusBtn:setText("已关注");
            self.mFocusBtn:disable();
        else
            self.mFocusBtn:setText("关注");
            self.mFocusBtn:enable();
        end
    else
        self:updatefocusItem(uid, already_follow);
    end
end

function M:updatefocusItem(uid, already_follow)
    if 1 == self.cur_open then
       for i=1, #self.focusitems do
           local acc = self.focusitems[i].uid;
           if acc == uid then
               if 1 == already_follow then
                   self.focusitems[i].m_FocusBtn:setText("已关注");
                   self.focusitems[i].m_FocusBtn:disable();
               else
                   self.focusitems[i].m_FocusBtn:setText("关注");
                   self.focusitems[i].m_FocusBtn:disable();
               end
               
               break;
           end
       end
    elseif 2 == self.cur_open then
        for i=1, #self.fansitems do
           local acc = self.fansitems[i].uid;
           if acc == uid then
               if 1 == already_follow then
                   self.fansitems[i].m_FocusBtn:setText("已关注");
                   self.fansitems[i].m_FocusBtn:disable();
               else
                   self.fansitems[i].m_FocusBtn:setText("关注");
                   self.fansitems[i].m_FocusBtn:disable();
               end

               break;
           end
       end
    else

    end
end

function M:updateFocusSearchItem(item)
    local areadyCount = #self.focusitems;
    for i=1, areadyCount do
        self.focusitems[i].m_go:SetActive(false);
    end

    if nil ~= item then
        self.focusitems[1].m_go:SetActive(true);
        self.focusitems[1]:updateValue(item.account, item.header_imgid, item.sex, item.level, item.state, item.already_follow, item.uid);
    end
end

function M:updateFocusData(isupdatebyself)
    if 1 ~= self.cur_open or not self.mFocusItemPrefabLoaded then
        return;
    end

    if not isupdatebyself then --服务器推送的新数据
        self.mFocusCurPage = GCtx.mSocialData.mFocusLastReqPage;
    end

    local itemscount = #self.focusitems;
    if 0 == itemscount then --第一次生成6个
        self.mFocusitemPrefab = self.mFocusitem_prefab:getPrefabTmpl();
        for i=1, GCtx.mSocialData.mEveryPageNum do
            local focusitem = GlobalNS.new(GlobalNS.FocusItem);
            focusitem:init(self.mFocusitemPrefab, self.mFocusContent, i);
            self.focusitems[i] = focusitem;
        end
    end

    local CurPageItemsNum = self:getFocusCurPageItemsNum();
    for i=1, CurPageItemsNum do
        local item = self:getFocusCurPageItem(i);
        if nil ~= item then
            self.focusitems[i].m_go:SetActive(true);
            self.focusitems[i]:updateValue(item.account, item.header_imgid, item.sex, item.level, item.state, item.already_follow, item.uid);
        end
    end
    
    --隐藏多余的item
    for i=CurPageItemsNum + 1, GCtx.mSocialData.mEveryPageNum do
        self.focusitems[i].m_go:SetActive(false);
    end

    self.mFocusPageNum.text = self.mFocusCurPage .. "/" .. GCtx.mSocialData.mFocusTotalPage;
    --滚动到起始位置，默认会在中间
    GlobalNS.UtilApi.GetComponent(self.mFocusScrollRect, "ScrollRect").verticalNormalizedPosition = 1;
end

function M:getFocusCurPageItemsNum()
    local num = GCtx.mSocialData.mEveryPageNum;
    if self.mFocusCurPage < GCtx.mSocialData.mFocusTotalPage then
        num = GCtx.mSocialData.mEveryPageNum;
    else
        num = GCtx.mSocialData.mFocusTotalNum - (self.mFocusCurPage - 1) * GCtx.mSocialData.mEveryPageNum;
    end
    return num;
end

function M:getFocusCurPageItem(i)
    local item = nil;
    local beforeItemsNum = (self.mFocusCurPage - 1) * GCtx.mSocialData.mEveryPageNum;
    local itemindex = beforeItemsNum + i - 1;
    item = GCtx.mSocialData:getFocusItemByIndex(itemindex);--MKeyIndexList从0开始
    return item;
end

function M:updateFansSearchItem(item)
    local areadyCount = #self.fansitems;
    for i=1, areadyCount do
        self.fansitems[i].m_go:SetActive(false);
    end

    if nil ~= item then
        self.fansitems[1].m_go:SetActive(true);
        self.fansitems[1]:updateValue(item.account, item.header_imgid, item.sex, item.level, item.state, item.already_follow, item.uid);
    end
end

function M:updateFansData(isupdatebyself)
    if 2 ~= self.cur_open or not self.mFansItemPrefabLoaded then
        return;
    end

    if not isupdatebyself then --服务器推送的新数据
        self.mFansCurPage = GCtx.mSocialData.mFansLastReqPage;
    end

    local itemscount = #self.fansitems;
    if 0 == itemscount then --第一次生成6个
        self.mFansitemPrefab = self.mFansitem_prefab:getPrefabTmpl();
        for i=1, GCtx.mSocialData.mEveryPageNum do
            local fansitem = GlobalNS.new(GlobalNS.FansItem);
            fansitem:init(self.mFansitemPrefab, self.mFansContent, i);
            self.fansitems[i] = fansitem;
        end
    end

    local CurPageItemsNum = self:getFansCurPageItemsNum();
    for i=1, CurPageItemsNum do
        local item = self:getFansCurPageItem(i);
        if nil ~= item then
            self.fansitems[i].m_go:SetActive(true);
            self.fansitems[i]:updateValue(item.account, item.header_imgid, item.sex, item.level, item.state, item.already_follow, item.uid);
        end
    end
    
    --隐藏多余的item
    for i=CurPageItemsNum + 1, GCtx.mSocialData.mEveryPageNum do
        self.fansitems[i].m_go:SetActive(false);
    end

    self.mFansPageNum.text = self.mFansCurPage .. "/" .. GCtx.mSocialData.mFansTotalPage;
    --滚动到起始位置，默认会在中间
    GlobalNS.UtilApi.GetComponent(self.mFansScrollRect, "ScrollRect").verticalNormalizedPosition = 1;
end

function M:getFansCurPageItemsNum()
    local num = GCtx.mSocialData.mEveryPageNum;
    if self.mFansCurPage < GCtx.mSocialData.mFansTotalPage then
        num = GCtx.mSocialData.mEveryPageNum;
    else
        num = GCtx.mSocialData.mFansTotalNum - (self.mFansCurPage - 1) * GCtx.mSocialData.mEveryPageNum;
    end
    return num;
end

function M:getFansCurPageItem(i)
    local item = nil;
    local beforeItemsNum = (self.mFansCurPage - 1) * GCtx.mSocialData.mEveryPageNum;
    local itemindex = beforeItemsNum + i - 1;
    item = GCtx.mSocialData:getFansItemByIndex(itemindex);--MKeyIndexList从0开始
    return item;
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    self.mBackBtn:dispose();
    self.mWechatBtn:dispose();
    self.mQQBtn:dispose();
    self.mAvatarImage:dispose();
    self.mSexImage:dispose();
    self.mFocusBtn:dispose();
    self.mFocusitem_prefab:dispose();
    self.mFansitem_prefab:dispose();
    self.mFocusBeforeBtn:dispose();
    self.mFocusNextBtn:dispose();
    self.mFansBeforeBtn:dispose();
    self.mFansNextBtn:dispose();
    self.mShareBtn:dispose();

    for i=1, #self.focusitems do
        self.focusitems[i]:dispose();
    end
    self.focusitems = {};

    for i=1, #self.fansitems do
        self.fansitems[i]:dispose();
    end
    self.fansitems = {};

    if(nil ~= self.mTabPanel) then
        self.mTabPanel.mTabClickEventDispatch:removeEventHandle(self, self.onTabClick);
		GlobalNS.delete(self.mTabPanel);
		self.mTabPanel = nil;
	end

    GCtx.mSocialData:clear();

    M.super.onExit(self);
end

function M:onBackBtnClk()
	self:exit();
end

function M:onWechatBtnClk()

end

function M:onQQBtnClk()

end

function M:onShareBtnClk()
    GlobalNS.CSSystem.Ctx.mInstance.mCamSys:ShareTo3Party(GCtx.mPlayerData.mHeroData.mMyselfAccount);
end

function M:onAvatarBtnClk()
    if self.FindFriendSuccess then
        GCtx.mPlayerData.mHeroData.mViewAccount = self.mFindFriendAccount;
        GCtx.mPlayerData.mHeroData.mViewUid = self.mFindFrienduid;
        GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountPanel);
    end
end

function M:onFocusBtnClk()
    if self.FindFriendSuccess then
        GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:Follow(self.mFindFrienduid);
    end
end

function M:onFocusBeforeBtnClk()
    if self.mFocusCurPage > 1 then
        self.mFocusCurPage = self.mFocusCurPage - 1;
        self:updateFocusData(true);
    end
end

function M:onFocusNextBtnClk()
    if self.mFocusCurPage < GCtx.mSocialData.mFocusLastReqPage then --数据已保存过
        self.mFocusCurPage = self.mFocusCurPage + 1;
        self:updateFocusData(true);
    else
        if self.mFocusCurPage < GCtx.mSocialData.mFocusTotalPage then
            --请求数据
            GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ViewFollowingList(GCtx.mPlayerData.mHeroData.mViewAccount, self.mFocusCurPage + 1);
        end
    end
end

function M:onFansBeforeBtnClk()
    if self.mFansCurPage > 1 then
        self.mFansCurPage = self.mFansCurPage - 1;
        self:updateFansData(true);
    end
end

function M:onFansNextBtnClk()
    if self.mFansCurPage < GCtx.mSocialData.mFansLastReqPage then --数据已保存过
        self.mFansCurPage = self.mFansCurPage + 1;
        self:updateFansData(true);
    else
        if self.mFansCurPage < GCtx.mSocialData.mFansTotalPage then
            --请求数据
            GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mSocialData:ViewFollowedList(GCtx.mPlayerData.mHeroData.mViewAccount, self.mFansCurPage + 1);
        end
    end
end

return M;