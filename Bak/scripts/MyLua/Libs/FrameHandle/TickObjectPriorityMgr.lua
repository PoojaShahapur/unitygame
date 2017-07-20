MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.DelayHandle.DelayPriorityHandleMgr");

-- 每一帧执行的对象管理器
local M = GlobalNS.Class(GlobalNS.DelayPriorityHandleMgr);
M.clsName = "TickObjectPriorityMgr";
GlobalNS[M.clsName] = M;

function M:ctor()

end

function M:init()
	M.super.init(self);
end

function M:dispose()
	M.super.dispose(self);
end

function M:setClientDispose(isDispose)

end

function M:isClientDispose()
	return false;
end

function M:onTick(delta, tickMode)
	self:Advance(delta, tickMode);
end

function M:Advance(delta, tickMode)
	self:incDepth();

	self:onPreAdvance(delta, tickMode);
	self:onExecAdvance(delta, tickMode);
	self:onPostAdvance(delta, tickMode);

	self:decDepth();
end

function M:onPreAdvance(delta, tickMode)

end

function M:onExecAdvance(delta, tickMode)
	local idx = 0;
	local count = self.mNoOrPriorityList:Count();
	local tickObject = nil;

	while (idx < count) do
		tickObject = self.mNoOrPriorityList:get(idx);

		if (nil ~= tickObject) then
			if (not tickObject:isClientDispose()) then
				tickObject:onTick(delta, tickMode);
			end
		else
			if (MacroDef.ENABLE_LOG) then
				GCtx.mLogSys:log("TickObjectPriorityMgr::onExecAdvance, failed", GlobalNS.LogTypeId.eLogCommon);
			end
		end

		idx = idx + 1;
	end
end

function M:onPostAdvance(delta, tickMode)

end

return M;