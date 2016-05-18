require "MyLua.Libs.Network.Cmd.NetCmdHandleBase"

local M = GlobalNS.Class(GlobalNS.NetCmdHandleBase);
M.clsName = "GameTestCmdHandle";
GlobalNS[M.clsName] = M;

function M:ctor()
    self:addParamHandle(MSG_ReqTest, self, self.handleTest);
end

function M:dtor()
    
end

function M:handleTest(cmd)
    GlobalNS.CSSystem.onTestProtoBuf(cmd);
end

return M;