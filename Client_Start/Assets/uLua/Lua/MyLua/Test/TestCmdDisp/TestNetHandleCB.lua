require "MyLua.Libs.Network.CmdDisp.NetDispHandle"
require "MyLua.Test.TestCmdDisp.TestCmdHandle"

local M = GlobalNS.Class(GlobalNS.NetDispHandle);
M.clsName = "TestNetHandleCB";
GlobalNS[M.clsName] = M;

function M:ctor()
    local cmdHandle = GlobalNS.new(GlobalNS.TestCmdHandle);
    self:addCmdHandle(MSG_ReqTest, cmdHandle, cmdHandle.call);
end

function M:dtor()

end

return M;