require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

--[[
    @brief 玩家管理器
]]

local M = GlobalNS.Class(GlobalNS.EntityMgrBase);
M.clsName = "PlayerMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_hero = nil;
end

function M:onTickExec(delta)
    M.super.onTickExec(self, delta);
end

function M:createHero()
    return GlobalNS.new(GloablNS.PlayerMain);
end

function M:addHero(hero)
    self.m_hero = hero;
    self:addPlayer(self.m_hero);
end

function M:addPlayer(being)
    self:addObject(being);
end

function M:removePlayer(being)
    self:delObject(being);
end

function M:getHero()
    return self.m_hero;
end

function M:getPlayerByThisId(thisId)
    return self:getEntityByThisId(thisId);
end