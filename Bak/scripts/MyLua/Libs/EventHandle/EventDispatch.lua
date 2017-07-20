MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.DataStruct.MList");
MLoader("MyLua.Libs.DelayHandle.DelayPriorityHandleMgrBase");
MLoader("MyLua.Libs.EventHandle.EventDispatchFunctionObject");

--[[
    @brief 事件分发器
]]

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
		if(MacroDef.ENABLE_LOG) then
			GCtx.mLogSys:log("EventDispatch::addEventHandle, error, add failed", GlobalNS.LogTypeId.eLogEventHandle);
		end
	end
end

function M:removeEventHandle(handle, pThis)
    local idx = 0;
	local listLen = self.mHandleList:Count();
	--for idx = 0, self.mHandleList:Count() - 1, 1 do
    while(idx < listLen) do
        if (self.mHandleList:get(idx):isEqual(handle, pThis)) then
            break;
        end
		
		idx = idx + 1;
    end
	
    if (idx < self.mHandleList:Count()) then
        self:removeObject(self.mHandleList:get(idx));
    else
		if(MacroDef.ENABLE_LOG) then
			GCtx.mLogSys:log("EventDispatch::removeEventHandle, error, remove failed", GlobalNS.LogTypeId.eLogEventHandle);
		end
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
    local isFinded = false;
	
    --for _, item in ipairs(self.mHandleList:list()) do
	local index = 0;
	local listLen = self.mHandleList:count();
	local item = nil;
	
	while(index < listLen) do
		item = self.mHandleList:get(index);
		
        if (item:isEqual(pThis, handle)) then
            isFinded = true;
            break;
        end
		
		index = index + 1;
    end

    return isFinded;
end

function M:copyFrom(rhv)
    --for _, handle in ipairs(rhv.handleList:list()) do
	
	local index = 0;
	local listLen = rhv.handleList:count();
	local handle = nil;
	
	while(index < listLen) do
		handle = rhv.handleList:get(index);
        self.mHandleList:Add(handle);
		
		index = index + 1;
    end
end

return M;