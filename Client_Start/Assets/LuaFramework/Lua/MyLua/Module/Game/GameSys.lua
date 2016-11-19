MLoader("MyLua.Libs.Module.IGameSys");
MLoader("MyLua.Module.Game.EventCB.GameNetHandleCB");

local M = GlobalNS.Class(GlobalNS.IGameSys);
M.clsName = "GameSys";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:dtor()

end

function M:init()
    local gameNetHandleCB = GlobalNS.new(GlobalNS.GameNetHandleCB);
    GCtx.m_netCmdNotify:addOneDisp(gameNetHandleCB);    -- 设置网络模块处理
end

return M;