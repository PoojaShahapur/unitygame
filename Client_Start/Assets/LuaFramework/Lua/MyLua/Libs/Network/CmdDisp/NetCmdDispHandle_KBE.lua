local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "NetCmdDispHandle_KBE";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mId2HandleDic = GlobalNS.new(GlobalNS.MDictionary);
end

function M:addParamHandle(paramId, pThis, func)
    if(not self.mId2HandleDic:ContainsKey(paramId)) then
        local disp = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
        self.mId2HandleDic:Add(paramId, disp);
    else
        GCtx.mLogSys:log("Msg Id Already Register paramId = " .. paramId, GlobalNS.LogTypeId.eLogCommon);
    end

    self.mId2HandleDic:value(paramId):addEventHandle(pThis, func);
end

function M:removeParamHandle(paramId, pThis, func)
    if(self.mId2HandleDic.ContainsKey(paramId)) then
        self.mId2HandleDic:value(paramId):removeEventHandle(pThis, func);
    else
        GCtx.mLogSys.log("ParamId not Register", GlobalNS.LogTypeId.eLogCommon);
    end
end

function M:handleMsg(msgName, msg)
    GCtx.mLogSys:log("NetCmdDispHandle Start handleMsg", GlobalNS.LogTypeId.eLogCommon);
    if(self.mId2HandleDic:ContainsKey(msgName)) then
        GCtx.mLogSys:log("NetCmdDispHandle In handleMsg", GlobalNS.LogTypeId.eLogCommon);
        self.mId2HandleDic:value(msgName):dispatchEvent(msg);
    else
        
    end
end

return M;