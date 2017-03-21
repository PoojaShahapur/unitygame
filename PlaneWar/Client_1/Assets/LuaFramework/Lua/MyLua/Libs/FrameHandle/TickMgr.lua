MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.FrameHandle.TickObjectPriorityMgr");

-- 心跳管理器
local M = GlobalNS.Class(GlobalNS.TickObjectPriorityMgr);
M.clsName = "TickMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	M.super.dispose(self);
end

function M:addTick(tickObj, priority)
	self:addObject(tickObj, priority);
end

function M:removeTick(tickObj)
	self:removeObject(tickObj);
end

return M;