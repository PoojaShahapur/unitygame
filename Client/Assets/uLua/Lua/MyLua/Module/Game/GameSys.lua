require "MyLua.Libs.Module.IGameSys"
require "MyLua.Module.GameNetHandleCB"

local M = GlobalNS.Class(GlobalNS.IGameSys);
M.clsName = "GameSys";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:dtor()

end

function M:init()
    GCtx.setNetDispList(GlobalNS.new(GlobalNS.GameNetHandleCB));    -- 设置网络模块处理
end

return M;