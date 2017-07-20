MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UITeamBattlePanel.TeamBattlePanelNS");
MLoader("MyLua.UI.UITeamBattlePanel.TeamBattlePanelData");
MLoader("MyLua.UI.UITeamBattlePanel.TeamBattlePanelCV");
MLoader("MyLua.UI.UITeamBattlePanel.FriendItem");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UITeamBattlePanel";
GlobalNS.TeamBattlePanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUITeamBattlePanel;
	self.mData = GlobalNS.new(GlobalNS.TeamBattlePanelNS.TeamBattlePanelData);
    self.PlayerItem = {};
    self.friendsitems = { };

    self.mTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);--邀请冷却
    self.mTimer:setTotalTime(10);
    self.mTimer:setFuncObject(self, self.onTick);

    self.mNickTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);--随机冷却
    self.mNickTimer:setTotalTime(2);
    self.mNickTimer:setFuncObject(self, self.onNickTick);
    self.mNickBtn = nil;
end

function M:dtor()
	
end

function M:onTick()
	local lefttime = GlobalNS.UtilMath.ceil(self.mTimer:getLeftRunTime());
    if lefttime <= 0 then
        self.mInviteBtn:enable();
        self:setFriendItemsState(true);
    else
    end
end

function M:onNickTick()
	local lefttime = GlobalNS.UtilMath.ceil(self.mNickTimer:getLeftRunTime());
    if lefttime <= 0 then
        self.mNickBtn:enable();
    else
    end
end

function M:onInit()
    M.super.onInit(self);
	
    --TOP
	self.mCloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCloseBtn:addEventHandle(self, self.onCloseBtnClk);
    self.mRuleBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mRuleBtn:addEventHandle(self, self.onRuleBtnClk);

    --FriendPanel
    self.mInviteBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mInviteBtn:addEventHandle(self, self.onInviteBtnClk);
    self.mBeforeBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBeforeBtn:addEventHandle(self, self.onBeforeBtnClk);
    self.mNextBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mNextBtn:addEventHandle(self, self.onNextBtnClk);
    self.mTestBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mTestBtn:addEventHandle(self, self.onTestBtnClk);

    --Bottom
    self.mBeginBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBeginBtn:addEventHandle(self, self.onBeginBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    local TitlePanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "TitlePanel");
    self.mCloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(TitlePanel, "Esc_Btn"));
    self.mRuleBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(TitlePanel, "Ruledescription_Btn"));
    self.mBeginBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Begin_Btn"));

    local MainPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "MainPanel");
    self:initTeamMesPanel(MainPanel);
    self:initFriendPanel(MainPanel);
end

function M:initTeamMesPanel(MainPanel)
    local TeamMesPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(MainPanel, "TeamMesPanel");

    --测试控件
        local TestInv = GlobalNS.UtilApi.TransFindChildByPObjAndPath(TeamMesPanel, "TestInv");
        self.TestName = GlobalNS.UtilApi.getComByPath(TestInv, "MyName", "InputField");
        self.mTestBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(TestInv, "Button"));
    ---END

    self.WhoTeam_Text = GlobalNS.UtilApi.getComByPath(TeamMesPanel, "WhoTeam_Text", "Text");
    self.Player1 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(TeamMesPanel, "Player1");
    self.Player2 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(TeamMesPanel, "Player2");
    self.Player3 = GlobalNS.UtilApi.TransFindChildByPObjAndPath(TeamMesPanel, "Player3");

    table.insert(self.PlayerItem, self.Player1);
    table.insert(self.PlayerItem, self.Player2);
    table.insert(self.PlayerItem, self.Player3);

    local membernum = GCtx.mTeamData.mTeamList:count();
    for i=1, membernum do
        self:updateTeamData(i, GCtx.mTeamData.mTeamList:get(i-1).m_data.uid);
    end
end

function M:initFriendPanel(MainPanel)
    local FriendPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(MainPanel, "FriendPanel");
    self.MaskPanel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(FriendPanel, "Image (2)");
    --获取ScrollRect的GameObject对象
    self.mFriendsScrollRect = GlobalNS.UtilApi.TransFindChildByPObjAndPath(FriendPanel, "ScrollRect");
    local viewport =  GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mFriendsScrollRect, "Viewport");
    --获取ScrollRect下Content中的RectTransform组件
    self.mFriendsContent = GlobalNS.UtilApi.getComByPath(viewport, "Content", "RectTransform");

    self.mInviteBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(FriendPanel, "Button"));
    self.mBeforeBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(FriendPanel, "Button_before"));
    self.mNextBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(FriendPanel, "Button_next"));
    self.mFriendsPageNum = GlobalNS.UtilApi.getComByPath(FriendPanel, "Page_number", "Text");

    self.mFriendsitem_prefab = GlobalNS.new(GlobalNS.AuxPrefabLoader);
	self.mFriendsitem_prefab:setIsNeedInsPrefab(false);
    self.mFriendsItemPrefabLoaded = false;
    self.mFriendsitem_prefab:asyncLoad("UI/UITeamBattlePanel/FriendList_Ol.prefab", self, self.onFriendsPrefabLoaded, nil);

    self.mFriendsCurPage = 1;
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mTeamData:ViewFriendList(1);--打开后主动请求第一页好友
end

function M:onFriendsPrefabLoaded()
    self.mFriendsItemPrefabLoaded = true;
end

function M:updateFriendsData(isupdatebyself)
    if not self.mFriendsItemPrefabLoaded then
        return;
    end

    if not isupdatebyself then --服务器推送的新数据
        self.mFriendsCurPage = GCtx.mTeamData.mFriendsLastReqPage;
    end

    if GCtx.mTeamData.mFriendsTotalNum > 0 then
        self.MaskPanel:SetActive(false);
    end

    local itemscount = #self.friendsitems;
    if 0 == itemscount then --第一次生成5个
        self.mFriendsitemPrefab = self.mFriendsitem_prefab:getPrefabTmpl();
        for i=1, GCtx.mTeamData.mFriendsEveryPageNum do
            local friendsitem = GlobalNS.new(GlobalNS.FriendItem);
            friendsitem:init(self.mFriendsitemPrefab, self.mFriendsContent, i);
            self.friendsitems[i] = friendsitem;
        end
    end

    local CurPageItemsNum = self:getFriendsCurPageItemsNum();
    for i=1, CurPageItemsNum do
        local item = self:getFriendsCurPageItem(i);
        if nil ~= item then
            self.friendsitems[i].m_go:SetActive(true);
            self.friendsitems[i]:updateValue(item.account, item.header_imgid, item.sex, item.level, item.state, item.uid);
        end
    end
    
    --隐藏多余的item
    for i=CurPageItemsNum + 1, GCtx.mTeamData.mFriendsEveryPageNum do
        self.friendsitems[i].m_go:SetActive(false);
    end

    self.mFriendsPageNum.text = self.mFriendsCurPage .. "/" .. GCtx.mTeamData.mFriendsTotalPage;
    --滚动到起始位置，默认会在中间
    GlobalNS.UtilApi.GetComponent(self.mFriendsScrollRect, "ScrollRect").verticalNormalizedPosition = 1;
end

function M:getFriendsCurPageItemsNum()
    local num = GCtx.mTeamData.mFriendsEveryPageNum;
    if self.mFriendsCurPage < GCtx.mTeamData.mFriendsTotalPage then
        num = GCtx.mTeamData.mFriendsEveryPageNum;
    else
        num = GCtx.mTeamData.mFriendsTotalNum - (self.mFriendsCurPage - 1) * GCtx.mTeamData.mFriendsEveryPageNum;
    end
    return num;
end

function M:getFriendsCurPageItem(i)
    local item = nil;
    local beforeItemsNum = (self.mFriendsCurPage - 1) * GCtx.mTeamData.mFriendsEveryPageNum;
    local itemindex = beforeItemsNum + i - 1;
    item = GCtx.mTeamData:getFriendItemByIndex(itemindex);--MKeyIndexList从0开始
    return item;
end

function M:memberLeave(index)
    local MaskImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.PlayerItem[index], "MaskImage");
    MaskImage:SetActive(true);
    self.WhoTeam_Text.text = GCtx.mTeamData.mTeamName .. " <color=#00FF00FF>(" .. GCtx.mTeamData.mTeamList:count() .. "/" .. GCtx.mTeamData.mTotalNum .. ")</color>";
end

function M:updateTeamData(index, key)
    local i = index - 1;
    local item = GCtx.mTeamData:getMemberItembyKey(key);
    local MaskImage = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.PlayerItem[index], "MaskImage");
    if item == nil then
        MaskImage:SetActive(true);
        return;
    end
    MaskImage:SetActive(false);
    self.WhoTeam_Text.text = GCtx.mTeamData.mTeamName .. " <color=#00FF00FF>(" .. GCtx.mTeamData.mTeamList:count() .. "/" .. GCtx.mTeamData.mTotalNum .. ")</color>";

    if GCtx.mPlayerData.mHeroData:isMyselfbyUid(GCtx.mTeamData.mTeamLeaderUid) then
        self.mBeginBtn:show();
    else
        self.mBeginBtn:hide();
    end

    --item
    item.m_playerBtn = GlobalNS.new(GlobalNS.AuxButton);
    item.m_playerBtn:addEventHandle(self, self.onPlayerBtnClk);
    item.m_playerBtn:setIsDestroySelf(false);
    item.m_playerBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.PlayerItem[index], "Headportrait"));
    item.m_playerBtn.param1 = item.m_data.uid;
    item.m_playerBtn.param2 = item.m_data.account;
    if GCtx.mPlayerData.mHeroData:isMyselfbyUid(item.m_data.uid) then
        item.m_playerBtn:disable();
    else
        item.m_playerBtn:enable();
    end

    --头像
    item.m_playerAvatarImage = GlobalNS.new(GlobalNS.AuxImage);
    item.m_playerAvatarImage:setIsDestroySelf(false);
    item.m_playerAvatarImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.PlayerItem[index], "Headportrait"));
    local avatarindex = item.m_data.header_imgid;
    if avatarindex == 0 then
        if GCtx.mPlayerData.mHeroData:isMyselfbyUid(item.m_data.uid) then
            avatarindex = 1;
        else
            local _time = os.clock();
            math.randomseed(_time + i);
            avatarindex = math.random(1, 4);
        end
    end
	item.m_playerAvatarImage:setSpritePath("Atlas/DefaultSkin/Avatar.asset", GlobalNS.UtilStr.tostring(avatarindex));

    --性别
    local sex = item.m_data.sex;
    item.m_playerSexImage = GlobalNS.new(GlobalNS.AuxImage);
    item.m_playerSexImage:setIsDestroySelf(false);
    item.m_playerSexImage:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.PlayerItem[index], "SexImage"));
    if 0 == sex then
        item.m_playerSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "man");
    else
        item.m_playerSexImage:setSpritePath("Atlas/DefaultSkin/CommonSkin.asset", "woman");
    end
    
    local NameBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.PlayerItem[index], "NameBG");
    --昵称
    item.m_playerNickBtn = GlobalNS.new(GlobalNS.AuxButton);
    item.m_playerNickBtn:addEventHandle(self, self.onPlayerNickBtnClk);
    item.m_playerNickBtn:setIsDestroySelf(false);
    item.m_playerNickBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(NameBG, "RandomName_BtnTouch"));
    item.m_playerNickBtn.param1 = i;
    self.mNickBtn = item.m_playerNickBtn;
    
    --用户名
    local account = item.m_data.account;
    local nickname = item.m_data.nickname;
    local playername = GlobalNS.UtilApi.getComByPath(NameBG, "playername", "Text");
    playername.text = account;
    if GCtx.mPlayerData.mHeroData:isMyselfbyUid(item.m_data.uid) then
        playername.text = "<color=#32c832ff>" .. account .. "</color>";
    end
    local Name = GlobalNS.UtilApi.getComByPath(NameBG, "Name", "Text");
    if GCtx.mPlayerData.mHeroData:isMyselfbyUid(item.m_data.uid) then  
        Name.text = "";
        self.MyName = GlobalNS.UtilApi.getComByPath(NameBG, "MyName", "InputField");
        self.MyName.text = nickname;
        self.MyNameInput = GlobalNS.UtilApi.TransFindChildByPObjAndPath(NameBG, "MyName");
        GlobalNS.UtilApi.addInputEndHandle(self.MyNameInput, self, self.onMyNameInputEnd);
        item.m_playerNickBtn:show();
    else
        Name.text = nickname;
        local MyName = GlobalNS.UtilApi.TransFindChildByPObjAndPath(NameBG, "MyName");
        MyName:SetActive(false);
        item.m_playerNickBtn:hide();
    end
end

function M:onMyNameInputEnd(text)
    local nickname = string.gsub(text, "^%s*(.-)%s*$", "%1"); --去除两端的空格
    if nickname == '' then
        nickname = "";
    end

    local isnospaceorper = false;
    isnospaceorper = GlobalNS.UtilApi.GetIsContainKeyword(nickname);
    if isnospaceorper then
        GCtx.mGameData:ShowRollMessage("昵称中不能包含<color=#00FF00FF>空格</color>和<color=#00FF00FF>%</color>，请重新设置");
        return;
    end

    local isFilter = GlobalNS.CSSystem.Ctx.mInstance.mWordFilterManager:IsMatch(nickname);
    if isFilter then
        GCtx.mGameData:ShowRollMessage("昵称中含有敏感词，请重新设置");
        return;
    end

    if string.len(nickname) > 0 and GlobalNS.UtilApi.GetTextWordNum(nickname) < 9 then
        self:setMyNickName();
    else
        GCtx.mGameData:ShowMessageBox("昵称不能为空或多于8个字符(" .. GlobalNS.UtilApi.GetTextWordNum(nickname) .. ")");
        return;
    end
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    if self.mCloseBtn ~= nil then
        self.mCloseBtn:dispose();
        self.mCloseBtn = nil;
    end
    if self.mRuleBtn ~= nil then
        self.mRuleBtn:dispose();
        self.mRuleBtn = nil;
    end
    if self.mBeginBtn ~= nil then
        self.mBeginBtn:dispose();
        self.mBeginBtn = nil;
    end
    if self.mInviteBtn ~= nil then
        self.mInviteBtn:dispose();
        self.mInviteBtn = nil;
    end
    if self.mFriendsitem_prefab ~= nil then
        self.mFriendsitem_prefab:dispose();
        self.mFriendsitem_prefab = nil;
    end
    if self.mBeforeBtn ~= nil then
        self.mBeforeBtn:dispose();
        self.mBeforeBtn = nil;
    end
    if self.mNextBtn ~= nil then
        self.mNextBtn:dispose();
        self.mNextBtn = nil;
    end
    if self.mTestBtn ~= nil then
        self.mTestBtn:dispose();
        self.mTestBtn = nil;
    end
    
    for i=1, #self.friendsitems do
        self.friendsitems[i]:dispose();
    end
    self.friendsitems = {};

    GCtx.mTeamData:clear();

    if nil ~= self.mTimer then
        self.mTimer:Stop();
        GlobalNS.delete(self.mTimer);
    	self.mTimer = nil;
    end
    if nil ~= self.mNickTimer then
        self.mNickTimer:Stop();
        GlobalNS.delete(self.mNickTimer);
    	self.mNickTimer = nil;
    end

    M.super.onExit(self);
end

function M:onCloseBtnClk()
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mTeamData:LeaveTeam();
	self:exit();
end

function M:onRuleBtnClk()
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUITeamRulePanel);
end

function M:onBeginBtnClk()
	GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mTeamData:TeamEnterRoom();
end

function M:onInviteBtnClk()
    self.mInviteBtn:disable();
    self:setFriendItemsState(false);
	self.mTimer:reset();
    self.mTimer:Start();

    --邀请当前页好友
    local CurPageItemsNum = self:getFriendsCurPageItemsNum();
    local uids = "";
    local uidnum = 0;
    for i=1, CurPageItemsNum do
        local item = self:getFriendsCurPageItem(i);
        if 1 == item.state then--只邀请在线状态的好友
            uids = uids .. "," .. math.floor(item.uid);
            uidnum = uidnum + 1;
        end
    end

    if uidnum > 0 then
       GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mTeamData:InviteJoinTeam(uids);
       GCtx.mGameData:ShowRollMessage("邀请已发出，请等待……");
    end
end

--全部邀请当前页好友
function M:setFriendItemsState(state)
    local CurPageItemsNum = self:getFriendsCurPageItemsNum();
    for i=1, CurPageItemsNum do
        local item = self:getFriendsCurPageItem(i);
        if nil ~= item then
            if state then
                self.friendsitems[i].m_InviteBtn:enable();
            else
                self.friendsitems[i].m_InviteBtn:disable();
            end
        end
    end
end

function M:onBeforeBtnClk()
	if self.mFriendsCurPage > 1 then
        self.mFriendsCurPage = self.mFriendsCurPage - 1;
        self:updateFriendsData(true);
    end
end

function M:onNextBtnClk()
	if self.mFriendsCurPage < GCtx.mTeamData.mFriendsLastReqPage then --数据已保存过
        self.mFriendsCurPage = self.mFriendsCurPage + 1;
        self:updateFriendsData(true);
    else
        if self.mFriendsCurPage < GCtx.mTeamData.mFriendsTotalPage then
            --请求数据
            GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mTeamData:ViewFriendList(self.mFriendsCurPage + 1);
        end
    end
end

function M:onTestBtnClk(dispObj)
	local uid = self.TestName.text;
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mTeamData:InviteJoinTeam(uid);
end

function M:onPlayerBtnClk(dispObj)
	local uid = dispObj.param1;
    local account = dispObj.param2;
    GCtx.mPlayerData.mHeroData.mViewUid = uid;
    GCtx.mPlayerData.mHeroData.mViewAccount = account;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountPanel);
end

function M:onPlayerNickBtnClk(dispObj)
    self.mNickBtn:disable();
    self.mNickTimer:reset();
    self.mNickTimer:Start();
	local _time = os.clock();
    math.randomseed(_time);
    local index = math.random(1, #GCtx.mSocialData.nicknames);
    self.MyName.text = GCtx.mSocialData.nicknames[index];
    self:setMyNickName();
end

function M:setMyNickName()
    local nickname = self.MyName.text;
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mTeamData:ChangeNickname(nickname);
end

function M:memberChangeNickName(index, nickname)
    local NameBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.PlayerItem[index], "NameBG");
    local Name = GlobalNS.UtilApi.getComByPath(NameBG, "Name", "Text");
    Name.text = nickname;
end

return M;