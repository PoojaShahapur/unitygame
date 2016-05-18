local M = GlobalNS.Class(GlobalNS.NetDispHandle);
M.clsName = "GameNetHandleCB";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_id2DispDic[MSG_ReqTest] = GameDataUserCmdHandle();
end

function M:dtor()

end

return M;