MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Functor.CallFuncObjectFixParam");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "LoopDepth";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mLoopDepth = 0;       -- 是否在循环中，支持多层嵌套，就是循环中再次调用循环
	self.mIncHandle = nil;     -- 增加处理器
	self.mDecHandle = nil;     -- 减少处理器
	self.mZeroHandle = nil;    -- 减少到 0 处理器
end

function M:dtor()
	
end

function M:setIncHandle(pThis, handle)
	if(nil == self.mIncHandle) then
		self.mIncHandle = GlobalNS.new(GlobalNS.CallFuncObjectFixParam);
	end
	
	self.mIncHandle:setPThisAndHandle(pThis, handle, nil);
end

function M:setDecHandle(pThis, handle)
	if(nil == self.mDecHandle) then
		self.mDecHandle = GlobalNS.new(GlobalNS.CallFuncObjectFixParam);
	end
	
	self.mDecHandle:setPThisAndHandle(pThis, handle, nil);
end

function M:setZeroHandle(pThis, handle)
	if(nil == self.mZeroHandle) then
		self.mZeroHandle = GlobalNS.new(GlobalNS.CallFuncObjectFixParam);
	end
	
	self.mZeroHandle:setPThisAndHandle(pThis, handle, nil);
end

function M:incDepth()
	self.mLoopDepth = self.mLoopDepth + 1;

	if(nil ~= self.mIncHandle) then
		self.mIncHandle:call();
	end
end

function M:decDepth()
	self.mLoopDepth = self.mLoopDepth - 1;

	if (nil ~= self.mDecHandle) then
		self.mDecHandle:call();
	end

	if(0 == self.mLoopDepth) then
		if (nil ~= self.mZeroHandle) then
			self.mZeroHandle:call();
		end
	end

	if(self.mLoopDepth < 0) then
		-- 错误，不对称
		GlobalNS.UtilApi.error("LoopDepth::decDepth, Error !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
	end
end

function M:isInDepth()
	return self.mLoopDepth > 0;
end

return M;