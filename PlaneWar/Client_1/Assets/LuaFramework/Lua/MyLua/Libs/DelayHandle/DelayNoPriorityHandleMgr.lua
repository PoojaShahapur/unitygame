MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.DataStruct.NoOrPriorityList.NoPriorityList.NoPriorityList");
MLoader("MyLua.Libs.DelayHandle.DelayNoOrPriorityHandleMgr");

-- 延迟优先级处理管理器
local M = GlobalNS.Class(GlobalNS.DelayNoOrPriorityHandleMgr);
M.clsName = "DelayNoPriorityHandleMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mDeferredAddQueue = GlobalNS.new(GlobalNS.NoPriorityList);
	self.mDeferredAddQueue:setIsSpeedUpFind(true);
	self.mDeferredDelQueue = GlobalNS.new(GlobalNS.NoPriorityList);
	self.mDeferredDelQueue:setIsSpeedUpFind(true);

	self.mNoOrPriorityList = GlobalNS.new(GlobalNS.NoPriorityList);
	self.mNoOrPriorityList:setIsSpeedUpFind(true);
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	M.super.dispose(self);
end

function M:addNoPriorityObject(priorityObject)
	self:addNoOrPriorityObject(priorityObject);
end

function M:removeNoPriorityObject(tickObj)
	self:removeNoOrPriorityObject(tickObj);
end

return M;