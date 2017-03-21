MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.EntityMgrBase);
M.clsName = "MonsterMgr";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:onTickExec(delta, tickMode)
    m.super.onTickExec(self, delta, tickMode);
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
    self:removeObject(being);
end