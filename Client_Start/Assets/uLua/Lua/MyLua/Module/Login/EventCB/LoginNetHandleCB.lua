require "MyLua.Libs.Network.CmdDisp.NetDispHandle"
require "MyLua.Module.Login.LoginNetHandle.LoginCmdHandle"

local M = GlobalNS.Class(GlobalNS.NetDispHandle);
M.clsName = "LoginNetHandleCB";
GlobalNS[M.clsName] = M;

function M:ctor()
    local cmdHandle = GlobalNS.new(GlobalNS.LoginCmdHandle);
    self:addCmdHandle(LoginResponse, cmdHandle, cmdHandle.handleMsg);
end

function M:dtor()

end

return M;