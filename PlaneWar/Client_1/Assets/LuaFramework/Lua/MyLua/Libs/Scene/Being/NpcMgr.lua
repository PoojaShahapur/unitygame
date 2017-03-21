MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @brief 所有的 npc 
]]

local M = GlobalNS.Class(GlobalNS.EntityMgrBase);
M.clsName = "NpcMgr";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:onTickExec(delta, tickMode)
    M.super.onTickExec(self, delta, tickMode);
end

function M:addNpc(being)
    self:addObject(being);
end

function M:removeNpc(being)
    self:removeObject(being);
end