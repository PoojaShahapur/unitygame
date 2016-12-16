MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

-- 保护函数调用
local M = GlobalNS.Class(GlobalNS.CallFuncObjectBase);
M.clsName = "PCallFuncObjectVarParam";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:dtor()

end

function M:setPThisAndHandle(pThis, handle, ...)
	self.mThis = pThis;
	self.mHandle = handle;
	self.mParam = {...};
end

function M:call()
    local func;
    local flag;
    local msg;
    
    if(nil ~= self.mThis and nil ~= self.mHandle) then
        func = function() 
            return self.mHandle(self.mThis, unpack(self.mParam)) 
        end
        flag, msg = xpcall(func, traceback)
        if(not flag) then
            self:error(flag, msg);
        end
        return msg
    elseif nil ~= self.mHandle then
        func = function() 
            self.func(unpack(self.mParam)) 
        end
        flag, msg = xpcall(func, traceback)
        if(not flag) then
            self:error(flag, msg);
        end
        return msg
    else
        return 0
    end
end

function M:error(status, value)
    if not status then
        -- 获取当前堆栈信息
        value = debug.traceback(self.mHandle, value)              
        error(value)              
    end
end

return M;