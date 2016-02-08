require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.Common.CallFuncObject"

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
    self.m_funcObj = GlobalNS.new(GlobalNS.CallFuncObject);
    self.m_handle = 0;
end

function M:create(pThis, func, param)
    -- 使用闭包保存 self，如果不使用闭包， self 就没地方保存
    local runInternal;
    runInternal = function()
        self:run();
    end
    
    self.m_funcObj:setPThisAndHandle(pThis, func, param);
    self.m_handle = coroutine.create(runInternal);
end

function M:resume()
    local status, value = coroutine.resume(self.m_handle)
    self:error(status, value);
end

function M:createAndResume(pThis, func, param)
    self:create(pThis, func, param);
    self:resume();
end

function M:status()
    return coroutine.status(self.m_handle);
end

-- yield 只能内部调用，不能从外部调用
function M:yield()
    coroutine.yield()
end

function M:error(status, value)
    if not status then
        -- 获取当前堆栈信息
        value = debug.traceback(self.m_handle, value)              
        error(value)              
    end
end

-- 这个是没有默认 self 的函数，因为 coroutine.create 只能传递一个参数，就是函数，因此只能这样做，需要在实例化的表中添加 run 这个属性
-- 使用闭包保存 self 后，run 就能传递 self 了
function M:run()
    self.m_funcObj:call();
end

-- 返回当前正在执行的协程，如果它被主线程调用的话，返回 null
function M:running()
    return coroutine.running();
end

return M;