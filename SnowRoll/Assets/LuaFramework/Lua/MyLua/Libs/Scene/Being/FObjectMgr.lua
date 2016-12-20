MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @brief 掉落物管理器   
]]

local M = GlobalNS.Class(GlobalNS.EntityMgrBase);
M.clsName = "FObjectMgr";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:onTickExec(delta)
    M.super.onTickExec(self, delta);
end

function M:addFObject(being)
    self:addObject(being);
end

function M:removeFObject(being)
    self:removeObject(being);
end