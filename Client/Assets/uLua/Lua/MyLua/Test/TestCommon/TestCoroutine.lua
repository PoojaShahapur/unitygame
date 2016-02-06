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
    --self:testBasic();
    --self:testBasicA();
    self:testBasicB();
end

function M:testBasic()
    coroutineFunc = function (a, b)
        for i = 1, 10 do
            print(i, a, b);
            coroutine.yield();
        end
    end

    co2 = coroutine.create(coroutineFunc);        --创建协同程序co2
    coroutine.resume(co2, 100, 200);                -- 1 100 200 开启协同，传入参数用于初始化
    coroutine.resume(co2);                        -- 2 100 200 
    coroutine.resume(co2, 500, 600);                -- 3 100 200 继续协同，传入参数无效

    co3 = coroutine.create(coroutineFunc)        --创建协同程序co3
    coroutine.resume(co3, 300, 400);                -- 1 300 400 开启协同，传入参数用于初始化
    coroutine.resume(co3);                        -- 2 300 400 
    coroutine.resume(co3);                        -- 3 300 400 
end

function M:testBasicA()
    co = coroutine.create(function (a, b) print("co", a, b, coroutine.yield()) end);
    coroutine.resume(co, 1, 2);        --没输出结果，注意两个数字参数是传递给函数的
    coroutine.resume(co, 3, 4, 5);        --co 1 2 3 4 5，这里的两个数字参数由resume传递给yield　
end

function M:testBasicB()
    produceFunc = function()
        local base = 0;
        while true do
            --local value = io.read();
            local value = base;
            print("produce: ", value);
            coroutine.yield(value);        --返回生产的值
            base = base + 1;
            -- base = base + aaa
        end
    end
    
    consumer = function(p)
        while true do
            -- 
            local status, value = coroutine.resume(p);        --唤醒生产者进行生产
            print("consume: ", value);
        end
    end
    
    --消费者驱动的设计，也就是消费者需要产品时找生产者请求，生产者完成生产后提供给消费者
    producer = coroutine.create(produceFunc);
    consumer(producer);
end

function M:testBasicC()
    produceFunc = function()
    while true do
        local value = io.read();
        print("produce: ", value);
        coroutine.yield(value);        --返回生产的值
    end
end

    filteFunc = function(p)
        while true do
            local status, value = coroutine.resume(p);
            value = value *100;            --放大一百倍
            coroutine.yield(value);
        end
    end
    
    consumer = function(f, p)
        while true do
            local status, value = coroutine.resume(f, p);        --唤醒生产者进行生产
            print("consume: ", value);
        end
    end
    
    --消费者驱动的设计，也就是消费者需要产品时找生产者请求，生产者完成生产后提供给消费者
    producer = coroutine.create(produceFunc);
    filter = coroutine.create(filteFunc);
    consumer(filter, producer);
end

return M;