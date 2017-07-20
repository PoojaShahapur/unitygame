MLoader("MyLua.Libs.Network.CmdDisp.NetCmdDispHandle_KBE");

local M = GlobalNS.Class(GlobalNS.NetCmdDispHandle_KBE);
M.clsName = "GameEventHandle";
GlobalNS[M.clsName] = M;

function M:ctor()
end

function M:dtor()
	GCtx.mNetCmdNotify_KBE:removeParamHandle("Client_onHelloCB", self, self.handleTest);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("Client_notifyReliveSeconds", self, self.Client_notifyReliveSeconds);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("handleSendAndGetMessage", self, self.handleSendAndGetMessage);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("notifyTop10RankInfoList", self, self.notifyTop10RankInfoList);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("notifyGameLeftSeconds", self, self.notifyGameLeftSeconds);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("notifyResultRankInfoList", self, self.notifyResultRankInfoList);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("notifyNetworkInvalid", self, self.notifyNetworkInvalid);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("notifySomeMessage", self, self.notifySomeMessage);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("notifyMessageBox", self, self.notifyMessageBox);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("ShowEmoticon", self, self.ShowEmoticon);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("UpdateMyScore", self, self.UpdateMyScore);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("NotifyOnFire", self, self.NotifyOnFire);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("NotifyOnSplit", self, self.NotifyOnSplit);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("NotifyBackHall", self, self.NotifyBackHall);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("ExitSomeForm", self, self.ExitSomeForm);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("SetMyLevel", self, self.SetMyLevel);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("NotifyPlayerInGame", self, self.NotifyPlayerInGame);

    --社交
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnMainPageInfoMsg", self, self.OnMainPageInfoMsg);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnOnePageFollowingDataMsg", self, self.OnOnePageFollowingDataMsg);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnOnePageFollowedDataMsg", self, self.OnOnePageFollowedDataMsg);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnGamePageInfoMsg", self, self.OnGamePageInfoMsg);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnSkinPageInfoMsg", self, self.OnSkinPageInfoMsg);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnFollowSuccess", self, self.OnFollowSuccess);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnUnfollowSuccess", self, self.OnUnfollowSuccess);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnSocialOpResultMsg", self, self.OnSocialOpResultMsg);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnFindFriend", self, self.OnFindFriend);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnFindFriendInFollowing", self, self.OnFindFriendInFollowing);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnFindFriendInFollowed", self, self.OnFindFriendInFollowed);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnShareDataMsg", self, self.OnShareDataMsg);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnNotifyFocusState", self, self.OnNotifyFocusState);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("NotifyHistoryRank", self, self.NotifyHistoryRank);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("NotifyYDHistoryRank", self, self.NotifyYDHistoryRank);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("NotifyLevelList", self, self.NotifyLevelList);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("NotifyLevelRankList", self, self.NotifyLevelRankList);

    --团队
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnEnterTeam", self, self.OnEnterTeam);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnViewFriendList", self, self.OnViewFriendList);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnNotifyAddTeamMember", self, self.OnNotifyAddTeamMember);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnNotifyLeaveTeam", self, self.OnNotifyLeaveTeam);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnNotifyAbadonTeam", self, self.OnNotifyAbadonTeam);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnAskAgreeJoin", self, self.OnAskAgreeJoin);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("OnNotifyMemberNicknameChanged", self, self.OnNotifyMemberNicknameChanged);
	
	-- 签到
	GCtx.mNetCmdNotify_KBE:removeParamHandle("OnNotifyMonthSigninInfo", self, self.OnNotifyMonthSigninInfo);
	GCtx.mNetCmdNotify_KBE:removeParamHandle("OnNotifyDaySigninResult", self, self.OnNotifyDaySigninResult);
	GCtx.mNetCmdNotify_KBE:removeParamHandle("OnNotifyReceiveCumulaRewardResult", self, self.OnNotifyReceiveCumulaRewardResult);
end

function M:init()
	GCtx.mNetCmdNotify_KBE:addParamHandle("Client_onHelloCB", self, self.handleTest);
    GCtx.mNetCmdNotify_KBE:addParamHandle("Client_notifyReliveSeconds", self, self.Client_notifyReliveSeconds);
    GCtx.mNetCmdNotify_KBE:addParamHandle("handleSendAndGetMessage", self, self.handleSendAndGetMessage);
    GCtx.mNetCmdNotify_KBE:addParamHandle("notifyTop10RankInfoList", self, self.notifyTop10RankInfoList);
    GCtx.mNetCmdNotify_KBE:addParamHandle("notifyGameLeftSeconds", self, self.notifyGameLeftSeconds);
    GCtx.mNetCmdNotify_KBE:addParamHandle("notifyResultRankInfoList", self, self.notifyResultRankInfoList);
    GCtx.mNetCmdNotify_KBE:addParamHandle("notifyNetworkInvalid", self, self.notifyNetworkInvalid);
    GCtx.mNetCmdNotify_KBE:addParamHandle("notifySomeMessage", self, self.notifySomeMessage);
    GCtx.mNetCmdNotify_KBE:addParamHandle("notifyMessageBox", self, self.notifyMessageBox);
    GCtx.mNetCmdNotify_KBE:addParamHandle("ShowEmoticon", self, self.ShowEmoticon);
    GCtx.mNetCmdNotify_KBE:addParamHandle("UpdateMyScore", self, self.UpdateMyScore);
    GCtx.mNetCmdNotify_KBE:addParamHandle("NotifyOnFire", self, self.NotifyOnFire);
    GCtx.mNetCmdNotify_KBE:addParamHandle("NotifyOnSplit", self, self.NotifyOnSplit);
    GCtx.mNetCmdNotify_KBE:addParamHandle("NotifyBackHall", self, self.NotifyBackHall);
    GCtx.mNetCmdNotify_KBE:addParamHandle("ExitSomeForm", self, self.ExitSomeForm);
    GCtx.mNetCmdNotify_KBE:addParamHandle("SetMyLevel", self, self.SetMyLevel);
    GCtx.mNetCmdNotify_KBE:addParamHandle("NotifyPlayerInGame", self, self.NotifyPlayerInGame);

    --社交
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnMainPageInfoMsg", self, self.OnMainPageInfoMsg);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnOnePageFollowingDataMsg", self, self.OnOnePageFollowingDataMsg);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnOnePageFollowedDataMsg", self, self.OnOnePageFollowedDataMsg);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnGamePageInfoMsg", self, self.OnGamePageInfoMsg);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnSkinPageInfoMsg", self, self.OnSkinPageInfoMsg);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnFollowSuccess", self, self.OnFollowSuccess);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnUnfollowSuccess", self, self.OnUnfollowSuccess);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnSocialOpResultMsg", self, self.OnSocialOpResultMsg);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnFindFriend", self, self.OnFindFriend);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnFindFriendInFollowing", self, self.OnFindFriendInFollowing);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnFindFriendInFollowed", self, self.OnFindFriendInFollowed);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnShareDataMsg", self, self.OnShareDataMsg);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnNotifyFocusState", self, self.OnNotifyFocusState);
    GCtx.mNetCmdNotify_KBE:addParamHandle("NotifyHistoryRank", self, self.NotifyHistoryRank);
    GCtx.mNetCmdNotify_KBE:addParamHandle("NotifyYDHistoryRank", self, self.NotifyYDHistoryRank);
    GCtx.mNetCmdNotify_KBE:addParamHandle("NotifyLevelList", self, self.NotifyLevelList);
    GCtx.mNetCmdNotify_KBE:addParamHandle("NotifyLevelRankList", self, self.NotifyLevelRankList);

    --团队
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnEnterTeam", self, self.OnEnterTeam);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnViewFriendList", self, self.OnViewFriendList);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnNotifyAddTeamMember", self, self.OnNotifyAddTeamMember);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnNotifyLeaveTeam", self, self.OnNotifyLeaveTeam);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnNotifyAbadonTeam", self, self.OnNotifyAbadonTeam);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnAskAgreeJoin", self, self.OnAskAgreeJoin);
    GCtx.mNetCmdNotify_KBE:addParamHandle("OnNotifyMemberNicknameChanged", self, self.OnNotifyMemberNicknameChanged);
	
	-- 签到
	GCtx.mNetCmdNotify_KBE:addParamHandle("OnNotifyMonthSigninInfo", self, self.OnNotifyMonthSigninInfo);
	GCtx.mNetCmdNotify_KBE:addParamHandle("OnNotifyDaySigninResult", self, self.OnNotifyDaySigninResult);
	GCtx.mNetCmdNotify_KBE:addParamHandle("OnNotifyReceiveCumulaRewardResult", self, self.OnNotifyReceiveCumulaRewardResult);
end

function M:handleTest(cmd)
    
end

function M:handleSendAndGetMessage(params)
    local msgName = params[0];
    if not self:filterMessage(msgName) then
        if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUIConsoleDlg) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIConsoleDlg);
            if nil ~= form and form.mIsReady then
                form:onSetLogText(msgName);
            end
        end
    end    
end

function M:filterMessage(msgname) --消息过滤
    if string.find(msgname, "Client_onUpdateBasePosXZ") ~= nil or
       string.find(msgname, "Baseapp_onUpdateDataFromClient") ~= nil or
       string.find(msgname, "Client_onUpdateData_xyz") ~= nil or
       string.find(msgname, "Baseapp_onClientActiveTick") ~= nil or
       string.find(msgname, "Client_onAppActiveTickCB") ~= nil or
       string.find(msgname, "Client_onEntityEnterWorld") ~= nil or
       string.find(msgname, "Client_onUpdatePropertys") ~= nil or
       string.find(msgname, "Client_setSpaceData") ~= nil or
       string.find(msgname, "Client_onEntityLeaveWorldOptimized") ~= nil or
       string.find(msgname, "Client_onRemoteMethodCall") ~= nil or
       string.find(msgname, "Client_onUpdateData_xz") ~= nil
    then
        return true;
    else
        return false;
    end
end

function M:Client_notifyReliveSeconds(params)
    local reliveTime = params[0]; --param是C#的数组，从0开始
    local enemyName = params[1];
    local isKilledBySelf = params[2];

    --重生后停止移动
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mHeroData:setMoveVec(Vector2.New(0, 0));

    GCtx.mGameData.reliveTime = reliveTime;
    GCtx.mGameData.enemyName = enemyName;
    GCtx.mGameData.iskilledbyself = isKilledBySelf;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIRelivePanel);
end

function M:SetMyLevel(params)
    local level = params[0]; --param是C#的数组，从0开始
    if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUIStartGame) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIStartGame);
        if nil ~= form then
            GCtx.mPlayerData.mHeroData.mMyselfLevel = level;
            form:updatelevelinfo();
            return;  --如果玩家不在游戏房间内，不提示降段等消息
        end
    end

    local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUILevelChangePanel);
    if nil ~= form then
        form:updateUIData(GCtx.mPlayerData.mHeroData.mMyselfLevel, level);
    end
    GCtx.mPlayerData.mHeroData.mMyselfLevel = level;
end

function M:NotifyPlayerInGame(params)
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIReconnectionPanel);
end

function M:notifyTop10RankInfoList(params)
    GCtx.mGameData:setTop10RankList(params);
end

function M:notifyGameLeftSeconds(params)
    local leftseconds = params[0];
    GCtx.mGameData:setGameTime(leftseconds);
end

function M:notifyResultRankInfoList(params)
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIPlayerDataPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIOptionPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUITopXRankPanel);

    if GlobalNS.CSSystem.Ctx.mInstance.mShareData:getGameMode() == 1 then --炼狱模式
        GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIRankListPanel);
    end
    GCtx.mGameData:setRankInfoList(params);
    GCtx.mGameData:clearResource();
end

function M:notifyNetworkInvalid()
    GCtx.mGameData.mMessageMethond = 1;
    GCtx.mGameData:ShowMessageBox("已与服务器断开连接");
end

function M:notifySomeMessage(params)
    local msg = params[0];
    GCtx.mGameData:ShowRollMessage(msg);
end

function M:notifyMessageBox(params)
    local msg = params[0];
    GCtx.mGameData:ShowMessageBox(msg);
end

function M:ShowEmoticon()
    -- 结算时就不显示了
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIRankListPanel);
    if nil == form or not form:isVisible() then            
         GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIEmoticonPanel);
    end
end

function M:UpdateMyScore(params)
    local score = params[0];
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIPlayerDataPanel);
    if nil ~= form and form:isVisible() then
        form:refreshScore(score);
    else
        GCtx.mGameData.mMyScore = score;
    end
end

function M:NotifyOnFire()
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIOptionPanel);
    if nil ~= form and form.mIsReady then
        form:onFireBtnClk();
    end
end

function M:NotifyOnSplit()
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIOptionPanel);
    if nil ~= form and form.mIsReady then
        form:onSplitBtnClk();
    end
end

function M:NotifyHistoryRank(params)
    GCtx.mGameData:setHistoryRank(params);
end

function M:NotifyYDHistoryRank(params)
    GCtx.mGameData:setYDHistoryRank(params);
end

function M:NotifyLevelList(params)
    GCtx.mGameData:setlevellist(params);
end

function M:NotifyLevelRankList(params)
    GCtx.mGameData:setlevelranklist(params);
end

function M:NotifyBackHall()
    GCtx.mGameData:notifyBackHall();
end

function M:ExitSomeForm()
    GCtx.mGameData:ExitSomeForm();
end

--社交
function M:OnMainPageInfoMsg(args)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIAccountPanel);
    if nil ~= form and form.mIsReady then
        form:serverNofityPersonalInfo(args);
    end
end

function M:OnOnePageFollowingDataMsg(args)
    GCtx.mSocialData:updateFocusList(args[0], args[1], args[2], args[3]);
end

function M:OnOnePageFollowedDataMsg(args)
    GCtx.mSocialData:updateFansList(args[0], args[1], args[2], args[3]);
end

function M:OnGamePageInfoMsg(args)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIAccountPanel);
    if nil ~= form and form.mIsReady then
        form:serverNofityGameData(args);
    end
end

function M:OnSkinPageInfoMsg(args)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIAccountPanel);
    if nil ~= form and form.mIsReady then
        form:serverSkinPageInfo(args[0], args[1]);
    end
end

function M:OnFollowSuccess(args)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIRankListPanel);
    if nil ~= form and form.mIsReady then
        form:updateRankItem(args[0], true);
    end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIFindFriend);
    if nil ~= form and form.mIsReady then
        form:updateFoucsBtnText(args[0], 1);
    end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIAccountPanel);
    if nil ~= form and form.mIsReady then
        form:updateFocusBtnState(1);
    end
end

function M:OnUnfollowSuccess(args)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIRankListPanel);
    if nil ~= form and form.mIsReady then
        form:updateRankItem(args[0], false);
    end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIFindFriend);
    if nil ~= form and form.mIsReady then
        form:updateFoucsBtnText(args[0], 0);
    end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIAccountPanel);
    if nil ~= form and form.mIsReady then
        form:updateFocusBtnState(0);
    end
end

function M:OnNotifyFocusState(args)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIRankListPanel);
    if nil ~= form and form.mIsReady then
        form:updateRankItem(args[0], args[1]);
    end
end

function M:OnSocialOpResultMsg(args)
    local account = args[0];
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIAccountPanel);
    if nil ~= form and form.mIsReady then
        form:updatePersonalAccount(account);
    end
    form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIStartGame);
    if nil ~= form and form.mIsReady then
        form:updatePersonalInfo(account);
    end
end

function M:OnFindFriend(args)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIFindFriend);
    if nil ~= form and form.mIsReady then
        form:updateFindFriendData(true, args[0]);
    end
end

function M:OnFindFriendInFollowing(args)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIFindFriend);
    if nil ~= form and form.mIsReady then
        form:updateFocusSearchItem(args[0]);
    end
end

function M:OnFindFriendInFollowed(args)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIFindFriend);
    if nil ~= form and form.mIsReady then
        form:updateFansSearchItem(args[0]);
    end
end

function M:OnShareDataMsg(args)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIShareMoney);
    if nil ~= form and form.mIsReady then
        form:updateData(args);
    end
end

--团队
function M:OnEnterTeam(args)
    GCtx.mTeamData:EnterTeam(args[0], args[1], args[2], args[3]);
end

function M:OnViewFriendList(args)
    GCtx.mTeamData:updateFriendList(args[0], args[1], args[2], args[3]);
end

function M:OnNotifyAddTeamMember(args)
    GCtx.mTeamData:updateTeamList(args[0], args[1]);
end

function M:OnNotifyLeaveTeam(args)
    GCtx.mTeamData:memberLeave(args[0], args[1]);
end

function M:OnNotifyAbadonTeam(args)
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUITeamBattlePanel);
    if nil ~= form then
        form:exit();
    end
end

function M:OnAskAgreeJoin(args)
    GCtx.mTeamData:InviteGame(args[0], args[1]);
end

function M:OnNotifyMemberNicknameChanged(args)
    GCtx.mTeamData:memberChangeNickname(args[0], args[1]);
end

-- 签到
-- 上线通知签到信息
function M:OnNotifyMonthSigninInfo(args)
	GCtx.mSignData:setDailySigninAndReceiveCumulaReward(args[0], args[1], args[2], args[3]);
end

function M:OnNotifyDaySigninResult(args)
	
end

function M:OnNotifyReceiveCumulaRewardResult(args)
	
end

return M;