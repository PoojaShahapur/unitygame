MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Functor.CallFuncObjectFixParam");

-- MCoroutine 状态
local M
M = {};
M.clsName = "MCoroutineState";
GlobalNS[M.clsName] = M;

M.suspended = 0
M.running = 1
M.dead = 2


-- 协程，只有一段代码需要分多次执行的时候才需要协程，一次性执行完成的代码不需要协程，协程也是串行执行的
M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "MCoroutine";
GlobalNS[M.clsName] = M;

function M:ctor()
    self.m_funcObj = nil;
    self.m_handle = 0;
    self.m_bError = false;       -- 协程最后是否执行发生错误
end

function M:createFixParam(pThis, func, param)
    -- 使用闭包保存 self，如果不使用闭包， self 就没地方保存
    local runInternal;
    runInternal = function()
        self:run();
    end
    
    self.m_funcObj = GlobalNS.new(GlobalNS.CallFuncObjectFixParam);
    self.m_funcObj:setPThisAndHandle(pThis, func, param);
    self.m_handle = coroutine.create(runInternal);
end

function M:createVarParam(pThis, func, ...)
    -- 使用闭包保存 self，如果不使用闭包， self 就没地方保存
    local runInternal;
    runInternal = function()
        self:run();
    end
    
    self.m_funcObj = GlobalNS.new(GlobalNS.CallFuncObjectVarParam);
    self.m_funcObj:setPThisAndHandle(pThis, func, ...);
    self.m_handle = coroutine.create(runInternal);
end

function M:resume()
    -- 如果发生错误， status 值是 false ， value 是字符串类型的错误提示信息，如果正确执行， status 是 true， value 就是执行完成后的返回结果，或者 coroutine.yield 返回值 
    local status, value = coroutine.resume(self.m_handle)
    self.m_bError = not status;
    self:error(status, value);
end

function M:createAndResume(pThis, func, param)
    self:create(pThis, func, param);
    self:resume();
end

function M:status()
    return coroutine.status(self.m_handle);
end

function M:getStatus()
    local status = coroutine.status(self.m_handle);
    if("suspended" == status) then
        return GlobalNS.MCoroutineState.suspended;
    elseif("running" == status) then
        return GlobalNS.MCoroutineState.running;
    elseif("dead" == status) then
        return GlobalNS.MCoroutineState.dead;
    else
        return GlobalNS.MCoroutineState.dead;
    end
end

-- 是否是暂停状态
function M:isSuspended()
    local status = coroutine.status(self.m_handle);
    return status == "suspended";
end

-- 是否是运行状态
function M:isRunning()
    local status = coroutine.status(self.m_handle);
    return status == "running";
end

-- 是否是死亡状态
function M:isDead()
    local status = coroutine.status(self.m_handle);
    return status == "dead";
end

-- 执行过程是否发生错误
function M:hasError()
    return self.m_bError;
end

-- yield 只能内部调用，不能从外部调用
function M:yield()
    coroutine.yield()
end

function M:error(status, value)
    if not status then
        -- 获取当前堆栈信息，如果是获取的是协程的堆栈，就是 debug.traceback 中传递了协程句柄，那么堆栈必然只有协程的信息，没有调用者的信息，因此也就不需要指定堆栈获取的层，如果获取在主线程中发生的异常，这个时候可能需要指定获取堆栈的层数。 
        --value = debug.traceback(self.m_handle, "", 2);
        value = debug.traceback(self.m_handle);                 -- 这个比较准
        --value = debug.traceback(self.m_handle, value);        -- 直接获取当前堆栈，并且添加一些信息在前面
        --value = debug.traceback(self.m_handle, value, 2);     -- 获取堆栈上第二层，将当前函数层去掉              
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