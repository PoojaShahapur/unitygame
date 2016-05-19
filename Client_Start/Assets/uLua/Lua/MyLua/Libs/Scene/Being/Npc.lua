require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

--[[	
    @brief 基本 NPC
]]

local M = GlobalNS.Class(GlobalNS.BeingEntity);
M.clsName = "Npc";
GlobalNS[M.clsName] = M;

function M:ctor()

end