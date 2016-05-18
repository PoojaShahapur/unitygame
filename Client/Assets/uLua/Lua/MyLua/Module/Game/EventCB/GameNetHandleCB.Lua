local M = GlobalNS.Class(GlobalNS.NetDispHandle);
M.clsName = "GameNetHandleCB";
GlobalNS[M.clsName] = M;

function M:ctor()
    local cmdHandle = GlobalNS.new(GlobanNS.GameTestCmdHandle);
    self.addCmdHandle(MSG_ReqTest, cmdHandle, cmdHandle.call);
end

function M:dtor()

end

return M;