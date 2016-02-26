require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

--[[
    @brief 主角
]]

local M = GlobalNS.Class(GlobalNS.Player);
M.clsName = "PlayerMain";
GlobalNS[M.clsName] = M;

function M:ctor()

end