MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AuxPropertyBase";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mEventDispatch = nil;
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	if(nil ~= self.mEventDispatch) then
		self.mEventDispatch:clearEventHandle();
		self.mEventDispatch = nil;
	end
end

function M:setData(value)
	if(nil ~= self.mEventDispatch) then
		self.mEventDispatch:dispatchEvent(self);
	end
end

function M:isValid()
	return true;
end

function M:reset()
	
end

function M:addEventHandle(pThis, handle)
	if(nil == self.mEventDispatch) then
		self.mEventDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
	end
	
	self.mEventDispatch:addEventHandle(pThis, handle);
end

function M:removeEventHandle(pThis, handle)
	if(nil ~= self.mEventDispatch) then
		self.mEventDispatch:removeEventHandle(pThis, handle);
	end
end

return M;