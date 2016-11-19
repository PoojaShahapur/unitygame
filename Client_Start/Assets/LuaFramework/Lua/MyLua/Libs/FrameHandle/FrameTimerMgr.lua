MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.DataStruct.MList");
MLoader("MyLua.Libs.DelayHandle.DelayHandleMgrBase");

local M = GlobalNS.Class(GlobalNS.DelayHandleMgrBase);
M.clsName = "FrameTimerMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_timerLists = GlobalNS.new(GlobalNS.MList);
    self.m_delLists = GlobalNS.new(GlobalNS.MList);
end

function M:addObject(delayObject, priority)
    -- 检查当前是否已经在队列中
    if self.m_timerLists:IndexOf(delayObject) == -1 then
        if self:bInDepth() then
            M.super.addObject(self, delayObject, priority);
        else
            self.m_timerLists:Add(delayObject);
        end
    end
end

function M:removeObject(delayObject)
    -- 检查当前是否在队列中
    if not self.m_timerLists:IndexOf(delayObject) == -1 then
        delayObject.m_disposed = true;
        if self:bInDepth() then
            M.super.addObject(self, delayObject);
        else
            for key, item in ipairs(self.m_timerLists.list()) do
                if item == delayObject then
                    self.m_timerLists:Remove(item);
                    break;
                end
            end
        end
    end
end

function M:Advance(delta)
    self.incDepth();

    for key, timerItem in ipairs(self.m_timerLists.list()) do
        if not timerItem:getClientDispose() then
            timerItem:OnFrameTimer();
        end
        if timerItem.m_disposed then
            self:removeObject(timerItem);
        end
    end

    self:decDepth();
end

return M;