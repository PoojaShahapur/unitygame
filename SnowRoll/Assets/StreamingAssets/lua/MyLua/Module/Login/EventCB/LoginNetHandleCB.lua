MLoader("MyLua.Libs.Network.CmdDisp.NetModuleDispHandle");
MLoader("MyLua.Module.Login.LoginNetHandle.LoginCmdHandle");

local M = GlobalNS.Class(GlobalNS.NetModuleDispHandle);
M.clsName = "LoginNetHandleCB";
GlobalNS[M.clsName] = M;

function M:ctor()
    local cmdHandle = GlobalNS.new(GlobalNS.LoginCmdHandle);
    self:addCmdHandle(LoginResponse, cmdHandle, cmdHandle.handleMsg);
end

function M:dtor()

end

return M;