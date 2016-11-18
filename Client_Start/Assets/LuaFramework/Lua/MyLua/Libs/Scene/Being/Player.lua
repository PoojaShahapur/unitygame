require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

--[[
    @brief 场景中的玩家
]]

local M = GlobalNS.Class(GlobalNS.BeingEntity);
M.clsName = "Player";
GlobalNS[M.clsName] = M;

function M:ctor()

end