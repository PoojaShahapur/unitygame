MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.CallFuncObjectBase);
M.clsName = "CallFuncObjectFixParam";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:dtor()

end

function M:clear()
	self.mThis = nil;
	self.mHandle = nil;
	self.mParam = nil;
end

function M:setPThisAndHandle(pThis, handle, param)
	self.mThis = pThis;
	self.mHandle = handle;
	self.mParam = param;
end

function M:call()
    if(nil ~= self.mThis and nil ~= self.mHandle) then
		if(nil ~= self.mParam) then
			return self.mHandle(self.mThis, self.mParam);
		else
			return self.mHandle(self.mThis);
		end
    elseif (nil ~= self.mHandle) then
		if(nil ~= self.mParam) then
			return self.mHandle(self.mParam);
		else
			return self.mHandle();
		end
    else
        return 0
    end
end

return M;