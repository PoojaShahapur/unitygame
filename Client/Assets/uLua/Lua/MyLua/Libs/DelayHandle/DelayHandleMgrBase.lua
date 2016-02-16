require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.DataStruct.MList"
require "MyLua.Libs.DelayHandle.DelayHandleObject"
require "MyLua.Libs.DelayHandle.DelayAddParam"
require "MyLua.Libs.DelayHandle.DelayDelParam"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "DelayHandleMgrBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_deferredAddQueue = GlobalNS.new(GlobalNS.MList);
    self.m_deferredDelQueue = GlobalNS.new(GlobalNS.MList);

    self.m_loopDepth = 0;
end

function M:dtor()

end

function M:addObject(delayObject, priority)
    if (self.m_loopDepth > 0) then
        if (not self:existAddList(delayObject)) then       -- 如果添加列表中没有
            if (self:existDelList(delayObject)) then   -- 如果已经添加到删除列表中
                self:delFromDelayDelList(delayObject);
            end

            local delayHandleObject = GlobalNS.new(GlobalNS.DelayHandleObject);
            delayHandleObject.m_delayParam = GlobalNS.new(GlobalNS.DelayAddParam);
            self.m_deferredAddQueue:Add(delayHandleObject);

            delayHandleObject.m_delayObject = delayObject;
            delayHandleObject.m_delayParam.m_priority = priority;
        end
    end
end

function M:delObject(delayObject)
    if (self.m_loopDepth > 0) then
        if (not self:existDelList(delayObject)) then
            if (self:existAddList(delayObject)) then    -- 如果已经添加到删除列表中
                self:delFromDelayAddList(delayObject);
            end

            delayObject:setClientDispose();

            local delayHandleObject = GlobalNS.new(GlobalNS.DelayHandleObject);
            delayHandleObject.m_delayParam = GlobalNS.new(GlobalNS.DelayDelParam);
            self.m_deferredDelQueue:Add(delayHandleObject);
            delayHandleObject.m_delayObject = delayObject;
        end
    end
end

-- 只有没有添加到列表中的才能添加
function M:existAddList(delayObject)
    for _, item in ipairs(self.m_deferredAddQueue:list()) do
        if (item.m_delayObject == delayObject) then
            return true;
        end
    end

    return false;
end

-- 只有没有添加到列表中的才能添加
function M:existDelList(delayObject)
    for _, item in ipairs(self.m_deferredAddQueue:list()) do
        if (item.m_delayObject == delayObject) then
            return true;
        end
    end

    return false;
end

-- 从延迟添加列表删除一个 Item
function M:delFromDelayAddList(delayObject)
    for _, item in ipairs(self.m_deferredAddQueue:list()) do
        if (item.m_delayObject == delayObject) then
            self.m_deferredAddQueue:Remove(item);
        end
    end
end

-- 从延迟删除列表删除一个 Item
function M:delFromDelayDelList(delayObject)
    for _, item in ipairs(self.m_deferredDelQueue:list()) do
        if (item.m_delayObject == delayObject) then
            self.m_deferredDelQueue:Remove(item);
        end
    end
end

function M:processDelayObjects()
    if 0 == self.m_loopDepth then       -- 只有全部退出循环后，才能处理添加删除
        if (self.m_deferredAddQueue:Count() > 0) then
            local idx = 0;
            for idx = 0, self.m_deferredAddQueue:Count() - 1, 1 do
                self:addObject(self.m_deferredAddQueue:at(idx).m_delayObject, self.m_deferredAddQueue:at(idx).m_delayParam.m_priority);
            end

            self.m_deferredAddQueue:Clear();
        end

        if (self.m_deferredDelQueue:Count() > 0) then
            local idx = 0;
            for idx = 0, self.m_deferredDelQueue:Count() - 1, 1 do
                self:delObject(self.m_deferredDelQueue:at(idx).m_delayObject);
            end

            self.m_deferredDelQueue:Clear();
        end
    end
end

function M:incDepth()
    self.m_loopDepth = self.m_loopDepth + 1;
end

function M:decDepth()
    self.m_loopDepth = self.m_loopDepth - 1;
    self:processDelayObjects();
end

function M:bInDepth()
    return self.m_loopDepth > 0;
end

return M;