MLoader("MyLua.Libs.Module.ILoginSys");
MLoader("MyLua.Module.Login.EventCB.LoginNetHandleCB");

local M = GlobalNS.Class(GlobalNS.ILoginSys);
M.clsName = "LoginSys";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:dtor()

end

function M:init()
    local loginNetHandleCB = GlobalNS.new(GlobalNS.LoginNetHandleCB);
    GCtx.mNetCmdNotify:addOneDisp(loginNetHandleCB);    -- 设置网络模块处理
end

return M;