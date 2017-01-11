MLoader("MyLua.Libs.Network.CmdDisp.NetCmdDispHandle");

local M = GlobalNS.Class(GlobalNS.NetCmdDispHandle);
M.clsName = "TestCmdHandle";
GlobalNS[M.clsName] = M;

function M:ctor()
    self:addParamHandle(MSG_ReqTest, self, self.handleTest);
end

function M:dtor()
    
end

function M:handleTest(cmd)
    GlobalNS.CSSystemTest.onTestProtoBuf(cmd);
end

return M;