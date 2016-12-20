MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @brief 可访问的 npc
]]

local M = GlobalNS.Class(GlobalNS.Npc);
M.clsName = "NpcVisit";
GlobalNS[M.clsName] = M;

function M:ctor()

end