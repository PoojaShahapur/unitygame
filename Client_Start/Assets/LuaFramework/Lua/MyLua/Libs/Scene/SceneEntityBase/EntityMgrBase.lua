MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.DelayHandle.DelayHandleMgrBase");

local M = GlobalNS.Class(GlobalNS.DelayHandleMgrBase);
M.clsName = "EntityMgrBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mSceneEntityList = GlobalNS.new(GlobalNS.MList);
end

function M:addObject(entity, priority)
    if(nil == priority) then
        priority = 0.0;
    end
    if (self:bInDepth()) then
        M.super.addObject(self, entity);
    else
        self.mSceneEntityList:Add(entity);
    end
end

function M:removeObject(entity)
    if (self:bInDepth()) then
        M.super.removeObject(self, entity);
    else
        self.mSceneEntityList:Remove(entity);
    end
end

function M:onTick(delta)
    self:incDepth();

    self:onTickExec(delta);

    self:decDepth();
end

function M:onTickExec(delta)
    for _, entity in ipairs(self.mSceneEntityList:list()) do
        if (not entity.getClientDispose()) then
            entity.onTick(delta);
        end
    end
end

function M:setClientDispose()

end

function M:getClientDispose()
    return false;
end

function M:getEntityByThisId(thisId)
    return nil;
end