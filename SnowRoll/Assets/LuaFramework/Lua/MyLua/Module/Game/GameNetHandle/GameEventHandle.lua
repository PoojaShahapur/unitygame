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
    GCtx.mNetCmdNotify_KBE:removeParamHandle("Client_notifyTop10RankInfoList", self, self.Client_notifyTop10RankInfoList);
    GCtx.mNetCmdNotify_KBE:removeParamHandle("notifyGameLeftSeconds", self, self.notifyGameLeftSeconds);
end

function M:init()
	GCtx.mNetCmdNotify_KBE:addParamHandle("Client_onHelloCB", self, self.handleTest);
    GCtx.mNetCmdNotify_KBE:addParamHandle("Client_notifyReliveSeconds", self, self.Client_notifyReliveSeconds);
    GCtx.mNetCmdNotify_KBE:addParamHandle("handleSendAndGetMessage", self, self.handleSendAndGetMessage);
    GCtx.mNetCmdNotify_KBE:addParamHandle("Client_notifyTop10RankInfoList", self, self.Client_notifyTop10RankInfoList);
    GCtx.mNetCmdNotify_KBE:addParamHandle("notifyGameLeftSeconds", self, self.notifyGameLeftSeconds);
end

function M:dtor()
    
end

function M:handleTest(cmd)
    
end

function M:handleSendAndGetMessage(params)
    local msgName = params[0];
    if not self:filterMessage(msgName) then
        if GCtx.mUiMgr:hasForm(GlobalNS.UIFormID.eUIConsoleDlg) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormID.eUIConsoleDlg);
            if nil ~= form and form:isVisible() then
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
       string.find(msgname, "Client_onAppActiveTickCB") ~= nil
    then
        return true;
    else
        return false;
    end
end

function M:Client_notifyReliveSeconds(params)
    local reliveseconds = params[0]; --param是C#的数组，从0开始
    local entityID = params[1];
    local form = GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormID.eUIRelivePanel);
    form:Client_notifyReliveSeconds(reliveseconds, entityID);
end

function M:Client_notifyTop10RankInfoList(params)
    if GCtx.mUiMgr:hasForm(GlobalNS.UIFormID.eUITopXRankPanel) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormID.eUITopXRankPanel);
        if nil ~= form and form:isVisible() then            
            form:onSetRankInfo(params);
        end
    end
end

function M:notifyGameLeftSeconds(params)
    local leftseconds = params[0];
    if GCtx.mUiMgr:hasForm(GlobalNS.UIFormID.eUIPlayerDataPanel) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormID.eUIPlayerDataPanel);
        if nil ~= form and form:isVisible() then
            form:refreshLeftTime(leftseconds);
        end
    end    
end

return M;