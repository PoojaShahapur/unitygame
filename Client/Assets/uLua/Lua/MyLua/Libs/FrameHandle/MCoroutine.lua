require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

-- MCoroutine 状态
local M
M = {};
M.clsName = "MCoroutineState";
GlobalNS[M.clsName] = M;

M.suspended = 0
M.running = 1
M.dead = 2


-- 携程
M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MCoroutine";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_handle = 0;
    self.m_param = nil;
end

function M:create(...)
    self.m_param = ...;
    self.m_handle = coroutine.create(self.run);
end

function M:resume()
    local status, value = coroutine.resume(self.m_handle)
end

function M:getStatus()
    return coroutine.status(self.m_handle);
end

function M:yield()
    coroutine.yield()
end

function M:run()
    
end

return M;