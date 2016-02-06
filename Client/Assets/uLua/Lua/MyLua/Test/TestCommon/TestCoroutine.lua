require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

require "MyLua.Libs.DataStruct.MLinkList"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestCoroutine";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:dtor()
    
end

function M:run()
    self:testBasic()
end

function M:testBasic()
    coroutineFunc = function (a, b)
        for i = 1, 10 do
            print(i, a, b)
            coroutine.yield()
        end
    end

    co2 = coroutine.create(coroutineFunc)        --创建协同程序co2
    coroutine.resume(co2, 100, 200)                -- 1 100 200 开启协同，传入参数用于初始化
    coroutine.resume(co2)                        -- 2 100 200 
    coroutine.resume(co2, 500, 600)                -- 3 100 200 继续协同，传入参数无效

    co3 = coroutine.create(coroutineFunc)        --创建协同程序co3
    coroutine.resume(co3, 300, 400)                -- 1 300 400 开启协同，传入参数用于初始化
    coroutine.resume(co3)                        -- 2 300 400 
    coroutine.resume(co3)                        -- 3 300 400 
end

return M;