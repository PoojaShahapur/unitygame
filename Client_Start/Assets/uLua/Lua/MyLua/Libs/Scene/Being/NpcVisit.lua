require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

--[[
    @brief 可访问的 npc
]]

local M = GlobalNS.Class(GlobalNS.Npc);
M.clsName = "NpcVisit";
GlobalNS[M.clsName] = M;

function M:ctor()

end