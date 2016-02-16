require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.DataStruct.MList"
require "MyLua.Libs.DelayHandle.DelayHandleMgrBase"

local M = GlobalNS.Class(GlobalNS.DelayHandleMgrBase);
M.clsName = "TimerMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_timerLists = GlobalNS.new(GlobalNS.MList);     -- 当前所有的定时器列表
end

function M:getCount()
    return self.m_timerLists:Count();
end

function M:addObject(delayObject, priority)
    if(nil == priority) then
        priority = 0;
    end
    
    GCtx.m_processSys:refreshUpdateFlag();
    
    -- 检查当前是否已经在队列中
    if (self.m_timerLists:IndexOf(delayObject) == -1) then
        if (self:bInDepth()) then
            M.super.addObject(self, delayObject, priority);
        else
            self.m_timerLists:Add(delayObject);
        end
    end
end

function M:delObject(delayObject)
    -- 检查当前是否在队列中
    if (self.m_timerLists:IndexOf(delayObject) ~= -1) then
        delayObject.m_disposed = true;
        if (self:bInDepth()) then
            M.super.delObject(self, delayObject);
        else
            for key, item in ipairs(self.m_timerLists:list()) do
                if (item == delayObject) then
                    self.m_timerLists:Remove(item);
                    break;
                end
            end
        end
    end
    
    if(self.m_timerLists:Count() == 0) then
        GCtx.m_processSys:refreshUpdateFlag();
    end
end

function M:addTimer(delayObject, priority)
    self:addObject(delayObject, priority);
end

function M:Advance(delta)
    self:incDepth();

    for key, timerItem in ipairs(self.m_timerLists:list()) do
        if (not timerItem:getClientDispose()) then
            timerItem:OnTimer(delta);
        end

        if (timerItem.m_disposed) then       -- 如果已经结束
            self:delObject(timerItem);
        end
    end

    self:decDepth();
end

return M;