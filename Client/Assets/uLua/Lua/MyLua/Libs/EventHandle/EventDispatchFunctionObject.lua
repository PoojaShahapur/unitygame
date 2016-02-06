--[[
    @brief 
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "EventDispatchFunctionObject";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_bClientDispose = false;       -- 是否释放了资源
    self.m_handle = nil;
    self.m_pThis = nil;
end

function M:dtor()

end

function M:call(dispObj)
    if(nil ~= self.m_pThis and nil ~= self.m_handle) then
        -- self.m_pThis:self.m_handle(dispObj);     -- 这么写好像不行
        self.m_handle(self.m_pThis, dispObj);
    elseif(nil ~= self.m_handle) then
        self.m_handle(dispObj);
    end
end

function M:setClientDispose()
    self.m_bClientDispose = true;
end

function M:getClientDispose()
    return self.m_bClientDispose;
end

return M;