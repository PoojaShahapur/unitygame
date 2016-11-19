MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Common.MDebug");

-- 保护函数调用
local M = GlobalNS.Class(GlobalNS.CallFuncObjectBase);
M.clsName = "PCallFuncObjectFixParam";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:dtor()

end

function M:setPThisAndHandle(pThis, handle, param)
	self.m_pThis = pThis;
	self.m_handle = handle;
	self.m_param = param;
end

function M:call()
    local func;
    local flag;
    local msg;
    
    if(nil ~= self.m_pThis and nil ~= self.m_handle) then
        func = function() 
            return self.m_handle(self.m_pThis, self.m_param) 
        end
        flag, msg = xpcall(func, traceback)
        if(not flag) then
            GlobalNS.MDebug.traceback(nil, msg);
        end
        return msg
    elseif nil ~= self.m_handle then
        func = function() 
            self.func(self.m_param)
        end
        flag, msg = xpcall(func, traceback)
        if(not flag) then
            GlobalNS.MDebug.traceback(nil, msg);
        end
        return msg
    else
        return 0
    end
end

return M;