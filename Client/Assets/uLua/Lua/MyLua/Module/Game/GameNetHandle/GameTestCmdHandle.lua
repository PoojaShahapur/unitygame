local M = GlobalNS.Class(GlobalNS.NetCmdHandleBase);
M.clsName = "GameTestCmdHandle";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_id2HandleDic[MSG_ReqTest] = handleTest;
end

function M:dtor()

end

function M:handleTest(msg)

end

return M;