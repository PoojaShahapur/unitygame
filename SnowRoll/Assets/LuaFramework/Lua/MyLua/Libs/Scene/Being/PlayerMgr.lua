MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @brief 玩家管理器
]]

local M = GlobalNS.Class(GlobalNS.EntityMgrBase);
M.clsName = "PlayerMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mHero = nil;
end

function M:onTickExec(delta)
    M.super.onTickExec(self, delta);
end

function M:createHero()
    return GlobalNS.new(GloablNS.PlayerMain);
end

function M:addHero(hero)
    self.mHero = hero;
    self:addPlayer(self.mHero);
end

function M:addPlayer(being)
    self:addObject(being);
end

function M:removePlayer(being)
    self:removeObject(being);
end

function M:getHero()
    return self.mHero;
end

function M:getPlayerByThisId(thisId)
    return self:getEntityByThisId(thisId);
end