require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.EntityMgrBase);
M.clsName = "MonsterMgr";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:onTickExec(delta)
    m.super.onTickExec(self, delta);
end

function M:createMonster()
    return GlobalNS.new(GlobalNS.Monster);
end

function M:addGroupMember(monster)
    
end

function M:addMonster(being)
    self:addObject(being);
end

function M:removeMonster(being)
    self:delObject(being);
end