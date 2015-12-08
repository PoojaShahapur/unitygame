require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.DataStruct.MList"
require "MyLua.Libs.DelayHandle.DelayHandleMgrBase"
require "MyLua.Libs.FrameHandle.TickProcessObject"

local M = GlobalNS.Class(GlobalNS.DelayHandleMgrBase)
M.clsName = "TickMgr"
GlobalNS[M.clsName] = M

function M:ctor()
    self.m_tickLst = GlobalNS.MList:new();
end

function M:addTick(tickObj, priority)
    self:addObject(tickObj, priority);
end

function M:addObject(delayObject, priority)
    if self:bInDepth() then
        self.super.addObject(self, delayObject, priority);
    else
        local position = -1;
        local i = 0
        for i = 0, i < self.m_tickLst:Count(), 1 do
            while true do
                if self.m_tickLst.at(i) == nil then
                    break;
                end
    
                if self.m_tickLst.at(i).m_tickObject == delayObject then
                    return;
                end
    
                if self.m_tickLst.at(i).m_priority < priority then
                    position = i;
                    break;
                end
                
                break
            end
        end

        local processObject = GlobalNS.TickProcessObject:new();
        processObject.m_tickObject = delayObject;
        processObject.m_priority = priority;

        if position < 0 or position >= self.m_tickLst:Count() then
            self.m_tickLst:Add(processObject);
        else
            self.m_tickLst:Insert(position, processObject);
        end
    end
end

function M:delObject(delayObject)
    if self:bInDepth() then
        self.super.delObject(self, delayObject);
    else
        for key, item in ipairs(self.m_tickLst:list()) do
            if item.m_tickObject == delayObject then
                self.m_tickLst:Remove(item);
                break;
            end
        end
    end
end

function M:Advance(delta)
    self:incDepth();

    for key, tk in ipairs(self.m_tickLst:list()) do
        if not tk.m_tickObject:getClientDispose() then
            tk.m_tickObject:onTick(delta);
        end
    end

    self:decDepth();
end

return M