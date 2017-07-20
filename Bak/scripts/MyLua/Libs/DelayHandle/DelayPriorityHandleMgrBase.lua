MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.DataStruct.NoOrPriorityList.PriorityList.PriorityList");
MLoader("MyLua.Libs.DelayHandle.DelayNoOrPriorityHandleMgrBase");

-- 当需要管理的对象可能在遍历中间添加的时候，需要这个管理器
local M = GlobalNS.Class(GlobalNS.DelayNoOrPriorityHandleMgrBase);
M.clsName = "DelayPriorityHandleMgrBase";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mDeferredAddQueue = GlobalNS.new(GlobalNS.PriorityList);
	self.mDeferredAddQueue:setIsSpeedUpFind(true);
	self.mDeferredDelQueue = GlobalNS.new(GlobalNS.PriorityList);
	self.mDeferredDelQueue:setIsSpeedUpFind(true);
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	M.super.dispose(M);
end

return M;