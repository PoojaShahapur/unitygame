local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NetModuleDispHandle";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_id2DispDic = GlobalNS.new(GlobalNS.Dictionary);
    --self.mCmdDispInfo = GlobalNS.new(GlobalNS.CmdDispInfo);
end

function M:addCmdHandle(cmdId, pThis, func)
    if (not self.m_id2DispDic:ContainsKey(cmdId)) then
        local disp = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
        self.m_id2DispDic:Add(cmdId, disp);
    end

    self.m_id2DispDic:value(cmdId):addEventHandle(pThis, func);
end

function M:removeCmdHandle(cmdId, pThis, func)
    if(not self.m_id2DispDic.ContainsKey(cmdId)) then
        GCtx.mLogSys:log("Cmd Handle Not Register");
    end

    self.m_id2DispDic[cmdId]:removeEventHandle(pThis, func);
end

function M:handleMsg(msg)
    --local byCmd = msg.mByCmd;
    --byCmd = msg.readUnsignedInt8(byCmd);
    --local byParam = msg.mByParam;
    --byParam = msg.readUnsignedInt8(byParam);
    --msg.setPos(0);
    GCtx.mLogSys:log("NetModuleDispHandle Start handleMsg", GlobalNS.LogTypeId.eLogCommon);
    if(self.m_id2DispDic:ContainsKey(msg.byCmd)) then
        GCtx.mLogSys:log("NetModuleDispHandle In handleMsg", GlobalNS.LogTypeId.eLogCommon);
        --self.mCmdDispInfo.bu = msg.mMsgBody;
        --self.mCmdDispInfo.byCmd = byCmd;
        --self.mCmdDispInfo.byParam = byParam;
        self.m_id2DispDic:value(msg.byCmd):dispatchEvent(msg);
    else
        
    end
end

return M;