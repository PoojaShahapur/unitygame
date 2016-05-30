local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NetCmdHandleBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_id2HandleDic = GlobalNS.new(GlobalNS.MDictionary);
end

function M:addParamHandle(paramId, pThis, func)
    if(not self.m_id2HandleDic:ContainsKey(paramId)) then
        local disp = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
        self.m_id2HandleDic:Add(paramId, disp);
    else
        GCtx.mLogSys.log("Msg Id Already Register", GlobalNS.LogTypeId.eLogCommon);
    end

    self.m_id2HandleDic:value(paramId):addEventHandle(pThis, func);
end

function M:removeParamHandle(paramId, pThis, func)
    if(self.m_id2HandleDic.ContainsKey(paramId)) then
        self.m_id2HandleDic:value(paramId):removeEventHandle(pThis, func);
    else
        GCtx.mLogSys.log("ParamId not Register", GlobalNS.LogTypeId.eLogCommon);
    end
end

function M:call(dispObj)
    local cmd = dispObj;
    self:handleMsg(cmd.bu, cmd.byCmd, cmd.byParam);
end

function M:handleMsg(bu, byCmd, byParam)
    GCtx.mLogSys:log("NetCmdHandleBase Start handleMsg", GlobalNS.LogTypeId.eLogCommon);
    if(self.m_id2HandleDic:ContainsKey(byParam)) then
        GCtx.mLogSys:log("NetCmdHandleBase In handleMsg", GlobalNS.LogTypeId.eLogCommon);
        self.m_id2HandleDic:value(byParam):dispatchEvent(bu);
    else
        
    end
end

return M;