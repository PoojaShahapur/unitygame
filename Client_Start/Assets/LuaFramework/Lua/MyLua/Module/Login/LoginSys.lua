require "MyLua.Libs.Module.ILoginSys"
require "MyLua.Module.Login.EventCB.LoginNetHandleCB"

local M = GlobalNS.Class(GlobalNS.ILoginSys);
M.clsName = "LoginSys";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:dtor()

end

function M:init()
    local loginNetHandleCB = GlobalNS.new(GlobalNS.LoginNetHandleCB);
    GCtx.m_netCmdNotify:addOneDisp(loginNetHandleCB);    -- 设置网络模块处理
end

return M;