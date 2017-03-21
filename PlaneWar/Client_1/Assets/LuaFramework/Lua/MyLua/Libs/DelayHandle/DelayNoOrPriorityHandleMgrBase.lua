MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

-- 当需要管理的对象可能在遍历中间添加的时候，需要这个管理器
local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "DelayNoOrPriorityHandleMgrBase";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mLoopDepth = GlobalNS.new(GlobalNS.LoopDepth); 	-- 是否在循环中，支持多层嵌套，就是循环中再次调用循环
	self.mLoopDepth:setZeroHandle(self, M.processDelayObjects);
end

function M:init()

end

function M:dispose()

end

function M:addObject(delayObject, priority)
	if(self.mLoopDepth:isInDepth()) then
		if (not self.mDeferredAddQueue:Contains(delayObject)) then       -- 如果添加列表中没有
			if (self.mDeferredDelQueue:Contains(delayObject)) then    -- 如果已经添加到删除列表中
				self.mDeferredDelQueue.removeNoOrPriorityObject(delayObject);
			end
			
			self.mDeferredAddQueue:addNoOrPriorityObject(delayObject);
		end
	end
end

function M:removeObject(delayObject)
	if (self.mLoopDepth:isInDepth()) then
		if (not self.mDeferredDelQueue:Contains(delayObject)) then
			if (self.mDeferredAddQueue:Contains(delayObject)) then   -- 如果已经添加到删除列表中
				self.mDeferredAddQueue:removeNoOrPriorityObject(delayObject);
			end

			delayObject:setClientDispose(true);
			
			self.mDeferredDelQueue:addNoOrPriorityObject(delayObject);
		end
	end
end

function M:processDelayObjects()
	local idx = 0;
	-- len 是 Python 的关键字
	local elemLen = 0;

	if (not self.mLoopDepth:isInDepth()) then      -- 只有全部退出循环后，才能处理添加删除
		if (self.mDeferredAddQueue:Count() > 0) then
			idx = 0;
			elemLen = self.mDeferredAddQueue:Count();

			while(idx < elemLen) do
				self:addObject(self.mDeferredAddQueue:get(idx));

				idx = idx + 1;
			end

			self.mDeferredAddQueue:Clear();
		end

		if (self.mDeferredDelQueue:Count() > 0) then
			idx = 0;
			elemLen = this.mDeferredDelQueue:Count();

			while(idx < elemLen) do
				self:removeObject(self.mDeferredDelQueue:get(idx));

				idx = idx + 1;
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