MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.DataStruct.MList");
MLoader("MyLua.Libs.DelayHandle.DelayHandleObject");
MLoader("MyLua.Libs.DelayHandle.DelayAddParam");
MLoader("MyLua.Libs.DelayHandle.DelayDelParam");
MLoader("MyLua.Libs.FrameHandle.LoopDepth");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "DelayHandleMgrBase";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.mDeferredAddQueue = GlobalNS.new(GlobalNS.MList);
    self.mDeferredDelQueue = GlobalNS.new(GlobalNS.MList);

    self.mLoopDepth = GlobalNS.new(GlobalNS.LoopDepth);
	self.mLoopDepth.setZeroHandle(self, self.processDelayObjects);
end

function M:dtor()

end

function M:addObject(delayObject, priority)
    if (self.mLoopDepth:isInDepth()) then
        if (not self:existAddList(delayObject)) then       -- 如果添加列表中没有
            if (self:existDelList(delayObject)) then   -- 如果已经添加到删除列表中
                self:delFromDelayDelList(delayObject);
            end

            local delayHandleObject = GlobalNS.new(GlobalNS.DelayHandleObject);
            delayHandleObject.mDelayParam = GlobalNS.new(GlobalNS.DelayAddParam);
            self.mDeferredAddQueue:Add(delayHandleObject);

            delayHandleObject.mDelayObject = delayObject;
            delayHandleObject.mDelayParam.mPriority = priority;
        end
    end
end

function M:removeObject(delayObject)
    if (self.mLoopDepth:isInDepth()) then
        if (not self:existDelList(delayObject)) then
            if (self:existAddList(delayObject)) then    -- 如果已经添加到删除列表中
                self:delFromDelayAddList(delayObject);
            end

            delayObject:setClientDispose();

            local delayHandleObject = GlobalNS.new(GlobalNS.DelayHandleObject);
            delayHandleObject.mDelayParam = GlobalNS.new(GlobalNS.DelayDelParam);
            self.mDeferredDelQueue:Add(delayHandleObject);
            delayHandleObject.mDelayObject = delayObject;
        end
    end
end

-- 只有没有添加到列表中的才能添加
function M:existAddList(delayObject)
    for _, item in ipairs(self.mDeferredAddQueue:list()) do
        if (item.mDelayObject == delayObject) then
            return true;
        end
    end

    return false;
end

-- 只有没有添加到列表中的才能添加
function M:existDelList(delayObject)
    for _, item in ipairs(self.mDeferredAddQueue:list()) do
        if (item.mDelayObject == delayObject) then
            return true;
        end
    end

    return false;
end

-- 从延迟添加列表删除一个 Item
function M:delFromDelayAddList(delayObject)
    for _, item in ipairs(self.mDeferredAddQueue:list()) do
        if (item.mDelayObject == delayObject) then
            self.mDeferredAddQueue:Remove(item);
        end
    end
end

-- 从延迟删除列表删除一个 Item
function M:delFromDelayDelList(delayObject)
    for _, item in ipairs(self.mDeferredDelQueue:list()) do
        if (item.mDelayObject == delayObject) then
            self.mDeferredDelQueue:Remove(item);
        end
    end
end

function M:processDelayObjects()
    if(not self.mLoopDepth:isInDepth()) then       -- 只有全部退出循环后，才能处理添加删除
        if (self.mDeferredAddQueue:Count() > 0) then
            local idx = 0;
            for idx = 0, self.mDeferredAddQueue:Count() - 1, 1 do
                self:addObject(self.mDeferredAddQueue:at(idx).mDelayObject, self.mDeferredAddQueue:at(idx).mDelayParam.mPriority);
            end

            self.mDeferredAddQueue:Clear();
        end

        if (self.mDeferredDelQueue:Count() > 0) then
            local idx = 0;
            for idx = 0, self.mDeferredDelQueue:Count() - 1, 1 do
                self:removeObject(self.mDeferredDelQueue:at(idx).mDelayObject);
            end

            self.mDeferredDelQueue:Clear();
        end
    end
end

function M:incDepth()
    self.mLoopDepth:incDepth();
end

function M:decDepth()
    self.mLoopDepth:decDepth();
end

function M:isInDepth()
    return self.mLoopDepth:isInDepth();
end

return M;