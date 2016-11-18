require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

--[[
    @brief 所有的 npc 
]]

local M = GlobalNS.Class(GlobalNS.EntityMgrBase);
M.clsName = "NpcMgr";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:onTickExec(delta)
    M.super.onTickExec(self, delta);
end

function M:addNpc(being)
    self:addObject(being);
end

function M:removeNpc(being)
    self:removeObject(being);
end