require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

-- 保护函数调用
local M = GlobalNS.Class(GlobalNS.CallFuncObjectBase);
M.clsName = "PCallFuncObjectVarParam";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:dtor()

end

function M:setPThisAndHandle(pThis, handle, ...)
	self.m_pThis = pThis;
	self.m_handle = handle;
	self.m_param = {...};
end

function M:call()
    local func;
    local flag;
    local msg;
    
    if(nil ~= self.m_pThis and nil ~= self.m_handle) then
        func = function() 
            return self.m_handle(self.m_pThis, unpack(self.m_param)) 
        end
        flag, msg = xpcall(func, traceback)
        if(not flag) then
            self:error(flag, msg);
        end
        return msg
    elseif nil ~= self.m_handle then
        func = function() 
            self.func(unpack(self.m_param)) 
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
        value = debug.traceback(self.m_handle, value)              
        error(value)              
    end
end

return M;