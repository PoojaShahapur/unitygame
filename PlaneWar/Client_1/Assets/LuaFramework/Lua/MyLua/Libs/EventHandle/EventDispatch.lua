--[[
    @brief 事件分发器
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.DataStruct.MList");
MLoader("MyLua.Libs.DelayHandle.DelayPriorityHandleMgrBase");
MLoader("MyLua.Libs.EventHandle.EventDispatchFunctionObject");

local M = GlobalNS.Class(GlobalNS.DelayPriorityHandleMgrBase);
M.clsName = "EventDispatch";
GlobalNS[M.clsName] = M;

function M:ctor(eventId_)
    self.mEventId = eventId_;
    self.mHandleList = GlobalNS.new(GlobalNS.MList);
    self.mUniqueId = 0;       -- 唯一 Id ，调试使用
end

function M:dtor()

end

function M:getHandleList()
    return self.mHandleList;
end

function M:getUniqueId()
    return self.mUniqueId;
end

function M:setUniqueId(value)
    self.mUniqueId = value;
    --self.mHandleList.uniqueId = mUniqueId;
end

function M:addEventHandle(pThis, handle)
    if (nil ~= handle) then
        local funcObject = GlobalNS.new(GlobalNS.EventDispatchFunctionObject);
        funcObject:setFuncObject(pThis, handle);
        self:addObject(funcObject);
    else
        -- 日志
    end
end

function M:addObject(delayObject, priority)
    if (self:isInDepth()) then
        M.super.addObject(self, delayObject, priority); -- super 使用需要自己填充 Self 参数
    else
        -- 这个判断说明相同的函数只能加一次，但是如果不同资源使用相同的回调函数就会有问题，但是这个判断可以保证只添加一次函数，值得，因此不同资源需要不同回调函数
        self.mHandleList:Add(delayObject);
    end
end

function M:removeEventHandle(handle, pThis)
    local idx = 0;
	
    for idx = 0, self.mHandleList:Count() - 1, 1 do
        if (self.mHandleList:at(idx):isEqual(handle, pThis)) then
            break;
        end
    end
	
    if (idx < self.mHandleList:Count()) then
        self:removeObject(self.mHandleList[idx]);
    else
        -- 日志
    end
end

function M:removeObject(delayObject)
    if (self:isInDepth()) then
        M.super.removeObject(self, delayObject);
    else
        if (self.mHandleList:Remove(delayObject) == false) then
            -- 日志
        end
    end
end

function M:dispatchEvent(dispatchObject)
    self:incDepth();

    for _, handle in ipairs(self.mHandleList:list()) do
        if (handle.mIsClientDispose == false) then
            handle:call(dispatchObject);
        end
    end

    self:decDepth();
end

function M:clearEventHandle()
    if (self:isInDepth()) then
        for _, item in ipairs(self.mHandleList:list()) do
            self:removeObject(item);
        end
    else
        self.mHandleList:Clear();
    end
end

-- 这个判断说明相同的函数只能加一次，但是如果不同资源使用相同的回调函数就会有问题，但是这个判断可以保证只添加一次函数，值得，因此不同资源需要不同回调函数
function M:isExistEventHandle(pThis, handle)
    local bFinded = false;
	
    for _, item in ipairs(self.mHandleList:list()) do
        if (item:isEqual(pThis, handle)) then
            bFinded = true;
            break;
        end
    end

    return bFinded;
end

function M:copyFrom(rhv)
    for _, handle in ipairs(rhv.handleList:list()) do
        self.mHandleList:Add(handle);
    end
end

return M;