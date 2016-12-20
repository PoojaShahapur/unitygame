MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[	
    @brief 基本 NPC
]]

local M = GlobalNS.Class(GlobalNS.BeingEntity);
M.clsName = "Npc";
GlobalNS[M.clsName] = M;

function M:ctor()

end