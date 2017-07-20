MLoader("MyLua.Libs.Network.CmdDisp.NetModuleDispHandle");
MLoader("MyLua.Module.Game.GameNetHandle.GameTestCmdHandle");

local M = GlobalNS.Class(GlobalNS.NetModuleDispHandle);
M.clsName = "GameNetHandleCB";
GlobalNS[M.clsName] = M;

function M:ctor()
    local cmdHandle = GlobalNS.new(GlobalNS.GameTestCmdHandle);
    self:addCmdHandle(MSG_ReqTest, cmdHandle, cmdHandle.handleMsg);
end

function M:dtor()

end

return M;