MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.FrameHandle.TickObjectPriorityMgr");

local M = GlobalNS.Class(GlobalNS.TickObjectPriorityMgr);
M.clsName = "NumNodeAnimSys";
GlobalNS[M.clsName] = M;

--[[
 @数字节点动画系统
]]
function M:ctor()

end

function M:init()
	M.super.init(self);

	GCtx.mTickMgr:addTick(self, GlobalNS.TickPriority.eTPSNodeNumAnimSys);
end

function M:dispose()
	if (nil ~= GCtx.mTickMgr) then
		GCtx.mTickMgr.removeTick(self, GlobalNS.TickPriority.eTPSNodeNumAnimSys);
	end

	M.super.dispose(self);
end

function M:addNodeNumAnim(anim)
	self:addObject(anim);
	
	GCtx.mFrameUpdateStatistics:setNeedUpdateByTypeId(GlobalNS.FrameUpdateStatisticsTypeId.eFUST_NumNodeAnimSys, true);
end

-- 移除节点动画，不释放
function M:removeNodeNumAnim(anim)
	self:removeObject(anim);
end

function M:removeObject(anim)
	M.super.removeObject(self, anim);
	
	if(self:isEmpty()) then
		GCtx.mFrameUpdateStatistics:setNeedUpdateByTypeId(GlobalNS.FrameUpdateStatisticsTypeId.eFUST_NumNodeAnimSys, false);
	end
end

-- 移除并且释放节点动画
function M:removeAndDisposeNodeNumAnim(anim)
	anim:dispose();
end

function M:onExecAdvance(delta, tickMode)
	M.super.onExecAdvance(self, delta, tickMode);
end

return M;