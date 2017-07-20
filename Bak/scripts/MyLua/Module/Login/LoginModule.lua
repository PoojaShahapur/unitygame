MLoader("MyLua.Libs.Module.ILoginModule");
MLoader("MyLua.Module.Login.EventCB.LoginNetHandleCB");

local M = GlobalNS.Class(GlobalNS.ILoginModule);
M.clsName = "LoginModule";
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