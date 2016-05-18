local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NetCmdHandleBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_id2HandleDic = GlobalNS.new(GlobalNS.Dictionary);
end

function M:addParamHandle(paramId, pThis, func)
    if(not self.m_id2HandleDic:ContainsKey(paramId)) then
        self.m_id2HandleDic[paramId] = GlobalNS.new(GlobalNS.AddOnceEventDispatch);   
    else
        GCtx.mLogSys.log("Msg Id Already Register");
    end

    self.m_id2HandleDic[paramId]:addEventHandle(pThis, func);
end

function M:removeParamHandle(paramId, pThis, func)
    if(self.m_id2HandleDic.ContainsKey(paramId)) then
        self.m_id2HandleDic[paramId].removeEventHandle(pThis, func);
    else
        GCtx.mLogSys.log("ParamId not Register");
    end
end

function M:call(dispObj)
    local cmd = dispObj;
    self:handleMsg(cmd.bu, cmd.byCmd, cmd.byParam);
end

function M:handleMsg(bu, byCmd, byParam)
    if(self.m_id2HandleDic:ContainsKey(byParam)) then
        self.m_id2HandleDic:key(byParam):dispatchEvent(bu);
    else
        
    end
end

return M;