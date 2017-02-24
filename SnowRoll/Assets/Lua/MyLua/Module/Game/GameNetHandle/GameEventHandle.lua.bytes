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
    GCtx.mNetCmdNotify_KBE:removeParamHandle("ShowNoticeMsg", self, self.ShowNoticeMsg);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("ShowEmoticon", self, self.ShowEmoticon);
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
    GCtx.mNetCmdNotify_KBE:addParamHandle("ShowNoticeMsg", self, self.ShowNoticeMsg);
    GCtx.mNetCmdNotify_KBE:addParamHandle("ShowEmoticon", self, self.ShowEmoticon);
end

function M:dtor()
    
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

    --重生后停止移动
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:setMoveVec(Vector2.New(0, 0));

    GCtx.mGameData.reliveTime = reliveTime;
    GCtx.mGameData.enemyName = enemyName;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIRelivePanel);
end

function M:notifyTop10RankInfoList(params)
    if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUITopXRankPanel) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUITopXRankPanel);
        if nil ~= form and form.mIsReady then            
            form:onSetRankInfo(params);
        end
    end
end

function M:notifyGameLeftSeconds(params)
    local leftseconds = params[0];
    GCtx.mGameData:setGameTime(leftseconds);
end

function M:notifyResultRankInfoList(params)
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIPlayerDataPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIOptionPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUITopXRankPanel);

    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIRankListPanel);
    GCtx.mGameData:setRankInfoList(params);
end

function M:notifyNetworkInvalid()
    GCtx.mGameData.mMessageMethond = 1;
    GCtx.mGameData:ShowMessageBox("已与服务器断开连接");
end

function M:notifySomeMessage(params)
    local msg = params[0];
    GCtx.mGameData:ShowRollMessage(msg);
end

function M:ShowNoticeMsg()
    local times = 0;
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("NoticeTimes") then
        times = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("NoticeTimes");
    end
    
    if GlobalNS.CSSystem.Ctx.mInstance.mShareData.noticeTimes > times then
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("NoticeTimes", times+1);
        
        local msg = string.gsub(GlobalNS.CSSystem.Ctx.mInstance.mShareData.noticeMsg, "\\n", "\n");
        GCtx.mGameData:ShowMessageBox(msg);
    end
end

function M:ShowEmoticon()
    -- 结算时就不显示了
    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIRankListPanel);
    if nil == form or not form:isVisible() then            
         GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIEmoticonPanel);
    end
end

return M;