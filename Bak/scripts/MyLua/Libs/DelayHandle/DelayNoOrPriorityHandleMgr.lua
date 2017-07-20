MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.DelayHandle.DelayNoOrPriorityHandleMgrBase");

-- 延迟优先级处理管理器
local M = GlobalNS.Class(GlobalNS.DelayNoOrPriorityHandleMgrBase);
M.clsName = "DelayNoOrPriorityHandleMgr";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:init()
	M.super.init(self);
end

function M:dispose()
	self.mNoOrPriorityList:Clear();
	
	M.super.dispose(self);
end

function M:isEmpty()
	local ret = (self.mNoOrPriorityList:Count() == 0);
	return ret;
end

function M:addObject(delayObject, priority)
	if (nil ~= delayObject) then
		if (self:isInDepth()) then
			M.super.addObject(self, delayObject, priority);
		else
			if(not self.mNoOrPriorityList:Contains(delayObject)) then
				self.mNoOrPriorityList:addNoOrPriorityObject(delayObject, priority);
			end
		end
	else
		if (MacroDef.ENABLE_LOG) then
			GCtx.mLogSys:log("DelayPriorityHandleMgr::addObject, failed", GlobalNS.LogTypeId.eLogCommon);
		end
	end
end

function M:removeObject(delayObject)
	if (nil ~= delayObject) then
		if (self:isInDepth()) then
			M.super.removeObject(self, delayObject);
		else
			if(self.mNoOrPriorityList:Contains(delayObject)) then
				self.mNoOrPriorityList:removeNoOrPriorityObject(delayObject);
			end
		end
	else
		if (MacroDef.ENABLE_LOG) then
			GCtx.mLogSys:log("DelayPriorityHandleMgr::removeObject, failed", GlobalNS.LogTypeId.eLogCommon);
		end
	end
end

function M:addNoOrPriorityObject(priorityObject, priority)
	self:addObject(priorityObject, priority);
end

function M:removeNoOrPriorityObject(tickObj)
	self:removeObject(tickObj);
end

return M;