--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

MLoader("MyLua.Libs.GameData.TeamInvitedItemData");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TeamData";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self:init();

    self.mInvitedTimer = GlobalNS.new(GlobalNS.TimerItemBase);
    self.mInvitedTimer.mInternal = 0.02;
    self.mInvitedTimer.mIsContinuous = true;
    self.mInvitedTimer.mIsInfineLoop = true;
    self.mInvitedTimer:setFuncObject(self, self.onInvitedTick);

    self.mIsTimerStarted = false;
    self.mDoOneInviteMsg = false;--正在执行一个邀请信息显示
    self.mDoShowItem = nil;

    self.mTeamName = "";--队伍名称
    self.mTotalNum = 3;--队伍总人数
    self.mTeamLeaderUid = 0;--队长uid
end

function M:onInvitedTick()
    if not self.mDoOneInviteMsg then
        local count = self.mInvitedList:count();
        if count > 0 then
            if nil ~= self.mDoShowItem then
                self.mDoShowItem:notifyCanShow(false);
            end
            local item = self.mInvitedList:get(count - 1);--取第后一个
            item:notifyCanShow(true);
            self.mDoShowItem = item;
            self.mDoOneInviteMsg = true;
        end
    end
end

function M:init(args)
    -- 好友列表
    self.mFriendsEveryPageNum = 5; --每页最多5个
    self.mFriendsList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mFriendsList:setIsSpeedUpFind(true);
	self.mFriendsList:setIsOpKeepSort(true);
    
    self.mFriendsTotalPage = 1; --总页数
    self.mFriendsLastReqPage = 1; --最后一次请求的页数
    self.mFriendsTotalNum = 0; --总个数

    -- 队友列表
    self.mTeamNum = 3;
    self.mTeamList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mTeamList:setIsSpeedUpFind(true);
	self.mTeamList:setIsOpKeepSort(true);

    -- 邀请列表
    self.mInvitedList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mInvitedList:setIsSpeedUpFind(true);
	self.mInvitedList:setIsOpKeepSort(true);
end

--好友
function M:getFriendsListCount()
	return self.mFriendsList:count();
end

function M:getFriendItemByIndex(index)
	return self.mFriendsList:get(index);
end

function M:updateFriendList(totalnum, reqpage, count, args)
    self.mFriendsTotalNum = totalnum;
    self.mFriendsTotalPage = math.floor((self.mFriendsTotalNum + self.mFriendsEveryPageNum - 1) / self.mFriendsEveryPageNum);
    self.mFriendsLastReqPage = reqpage;

    local key = 0;
    for i=1, count do
        key = args[i-1].uid;
        if(not self.mFriendsList:ContainsKey(key)) then
            local item = args[i-1];
		    self.mFriendsList:add(key, item);
	    end
    end
    
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUITeamBattlePanel);
    if nil ~= form and form.mIsReady then
        form:updateFriendsData(false);
    end
end

function M:getFriendItembyKey(key)
    local item = nil;
    if(self.mFriendsList:ContainsKey(key)) then
		item = self.mFriendsList:value(key);
	end
    return item;
end

--队友
function M:updateTeamList(index, data)
    local key = 0;
    key = data.uid;
    if(not self.mTeamList:ContainsKey(key)) then
        local item = 
        {
            m_data = data;
            m_playerBtn = nil;
            m_playerNickBtn = nil;
            m_playerAvatarImage = nil;
            m_playerSexImage = nil;
        }
	    self.mTeamList:add(key, item);
	end
    
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUITeamBattlePanel);
    if nil ~= form and form.mIsReady then
        form:updateTeamData(index, key);
    end
end

function M:getMemberItembyKey(key)
    local item = nil;
    if(self.mTeamList:ContainsKey(key)) then
		item = self.mTeamList:value(key);
	end
    return item;
end

function M:clearTeamlist()
    local count = self.mTeamList:count();
    for i=1, count do
        local item = self.mTeamList:get(i-1);
        if item.m_playerBtn ~= nil then
            item.m_playerBtn:dispose();
            item.m_playerBtn = nil;
        end
        if item.m_playerNickBtn ~= nil then
            item.m_playerNickBtn:dispose();
            item.m_playerNickBtn = nil;
        end
        if item.m_playerAvatarImage ~= nil then
            item.m_playerAvatarImage:dispose();
            item.m_playerAvatarImage = nil;
        end
        if item.m_playerSexImage ~= nil then
            item.m_playerSexImage:dispose();
            item.m_playerSexImage = nil;
        end
    end
    self.mTeamList:clear();

    self.mTeamName = "";
    self.mTotalNum = 3;
    self.mTeamLeaderUid = 0;
end

function M:clearInvitedlist()
    local count = self.mInvitedList:count();
    for i=1, count do
        local item = self.mInvitedList:get(i-1);
        item:dispose();
    end
    self.mInvitedList:clear();
    self.mDoOneInviteMsg = false;
    self.mDoShowItem = nil;
end

--组队邀请，uid为teamid
function M:InviteGame(uid, name)
    if not self.mIsTimerStarted then
        self.mIsTimerStarted = true;
        self.mInvitedTimer:reset();
        self.mInvitedTimer:Start();
    end
    local inviteditem = GlobalNS.new(GlobalNS.TeamInvitedItemData);
    inviteditem:setdata(uid, name, 10);--显示10s
    if(not self.mInvitedList:ContainsKey(uid)) then
	    self.mInvitedList:add(uid, inviteditem);
        self.mDoOneInviteMsg = false;--收到新邀请，马上显示
	end
end

function M:RemoveInvitedItemByKey(uid)
    if(self.mInvitedList:ContainsKey(uid)) then
	    self.mInvitedList:Remove(uid);
	end
end

function M:dtor()
    self:clear();
    self.mInvitedTimer:Stop();
    self.mIsTimerStarted = false;
    self.mDoOneInviteMsg = false;
    self.mDoShowItem = nil;
end

function M:clear()
    self.mFriendsList:clear();
    self:clearTeamlist();
    self:clearInvitedlist();
end

function M:EnterTeam(teamname, total_num, leader_uid, members)
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUITeamBattlePanel);
    self.mTeamName = teamname;--队伍名称
    self.mTotalNum = total_num;--队伍总人数
    self.mTeamLeaderUid = leader_uid;--队长uid

    local count = members.Count;
    for i = 1, count do
        self:updateTeamList(i, members[i-1]);
    end
end

function M:memberLeave(index, uid)
    if self.mTeamList:ContainsKey(uid) then
        local item = self.mTeamList:value(uid);
        item.m_data = nil;
        item.m_playerBtn:dispose();
        item.m_playerBtn = nil;
        item.m_playerNickBtn:dispose();
        item.m_playerNickBtn = nil;
        item.m_playerAvatarImage:dispose();
        item.m_playerAvatarImage = nil;
        item.m_playerSexImage:dispose();
        item.m_playerSexImage = nil;
        self.mTeamList:remove(uid);
	end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUITeamBattlePanel);
    if nil ~= form and form.mIsReady then
        form:memberLeave(index);
    end
end

function M:memberChangeNickname(index, nickname)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUITeamBattlePanel);
    if nil ~= form and form.mIsReady then
        form:memberChangeNickName(index, nickname);
    end
end

return M;
--endregion
