local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NetDispHandle";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_id2DispDic = GlobalNS.new(GlobalNS.Dictionary);
    self.mCmdDispInfo = GlobalNS.new(GlobalNS.CmdDispInfo);
end

function M:addCmdHandle(cmdId, pThis, func)
    if (not self.m_id2DispDic:ContainsKey(cmdId)) then
        self.m_id2DispDic[cmdId] = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
    end

    self.m_id2DispDic[cmdId]:addEventHandle(pThis, func);
end

function M:removeCmdHandle(cmdId, pThis, func)
    if(not self.m_id2DispDic.ContainsKey(cmdId)) then
        GCtx.mLogSys:log("Cmd Handle Not Register");
    end

    self.m_id2DispDic[cmdId]:removeEventHandle(pThis, func);
end

function M:handleMsg(msg)
    local byCmd = 0;
    byCmd = msg.readUnsignedInt8(byCmd);
    local byParam = 0;
    byParam = msg.readUnsignedInt8(byParam);
    msg.setPos(0);

    if(self.m_id2DispDic.ContainsKey(byCmd)) then
        self.mCmdDispInfo.bu = msg;
        self.mCmdDispInfo.byCmd = byCmd;
        self.mCmdDispInfo.byParam = byParam;
        self.m_id2DispDic[byCmd].dispatchEvent(self.mCmdDispInfo);
    else
        
    end
end

return M;