require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

require "MyLua.Libs.Thread.MCoroutine"

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
    --self:testBasicB();
    --self:testBasicD();
    --self:testBasicF();
    self:testBasicG();
end

function M:testBasic()
    local coroutineFunc = function (a, b)
        for i = 1, 10 do
            print(i, a, b);
            coroutine.yield();
        end
    end

    local co2 = coroutine.create(coroutineFunc);        --创建协同程序co2
    coroutine.resume(co2, 100, 200);                -- 1 100 200 开启协同，传入参数用于初始化
    coroutine.resume(co2);                        -- 2 100 200 
    coroutine.resume(co2, 500, 600);                -- 3 100 200 继续协同，传入参数无效

    local co3 = coroutine.create(coroutineFunc)        --创建协同程序co3
    coroutine.resume(co3, 300, 400);                -- 1 300 400 开启协同，传入参数用于初始化
    coroutine.resume(co3);                        -- 2 300 400 
    coroutine.resume(co3);                        -- 3 300 400 
end

function M:testBasicA()
    local co = coroutine.create(function (a, b) print("co", a, b, coroutine.yield()) end);
    coroutine.resume(co, 1, 2);        --没输出结果，注意两个数字参数是传递给函数的
    coroutine.resume(co, 3, 4, 5);        --co 1 2 3 4 5，这里的两个数字参数由resume传递给yield　
end

function M:testBasicB()
    local produceFunc = function()
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
    
    local consumer = function(p)
        while true do
            -- coroutine.resume 执行的 coroutine ，如果正确执行， status 为 true， value 就是 coroutine.yield 返回值，如果执行错误，status 为 false， value 是错误字符串 
            local status, value = coroutine.resume(p);        --唤醒生产者进行生产
            print("consume: ", value);
        end
    end
    
    --消费者驱动的设计，也就是消费者需要产品时找生产者请求，生产者完成生产后提供给消费者
    local producer = coroutine.create(produceFunc);
    consumer(producer);
end

function M:testBasicC()
    local produceFunc = function()
        while true do
            local value = io.read();
            print("produce: ", value);
            coroutine.yield(value);        --返回生产的值
        end
    end

    local filteFunc = function(p)
        while true do
            local status, value = coroutine.resume(p);
            value = value *100;            --放大一百倍
            coroutine.yield(value);
        end
    end
    
    local consumer = function(f, p)
        while true do
            local status, value = coroutine.resume(f, p);        --唤醒生产者进行生产
            print("consume: ", value);
        end
    end
    
    --消费者驱动的设计，也就是消费者需要产品时找生产者请求，生产者完成生产后提供给消费者
    local producer = coroutine.create(produceFunc);
    local filter = coroutine.create(filteFunc);
    consumer(filter, producer);
end

-- 启动一个 coroutine ，调用 coroutine.resume 的 coroutine 将阻塞，然后被调用的程序  coroutine 开始执行
function M:testBasicD()
    local produceFunc = function()
        local base = 0;
        while true do
            print("produceFunc");
        end
    end
    
    local consumer = function(p)
        while true do 
            local status, value = coroutine.resume(p);
            print("consumer: ");
        end
    end
    
    --消费者驱动的设计，也就是消费者需要产品时找生产者请求，生产者完成生产后提供给消费者
    local producer = coroutine.create(produceFunc);
    consumer(producer);
end

function M:testBasicF()
    local cor = GlobalNS.new(GlobalNS.MCoroutine);
    cor:createAndResume(self, self.produceFunc, nil);
    print('end');
end

function M:produceFunc()
    local index = 0;
    while(true) do
        print(index, ' produceFunc');
        index = index + 1;
        --index = aaa;
        
        if(index > 100) then
            break;
        end
    end
end

function M:testBasicG()
    local producer;
    local produceFunc = function()
        local base = 0;
        while true do
            local value = base;
            print("produce: ", value);
            coroutine.yield(value);
            base = base + 1;
            base = base + aaa
        end
    end
    
    local consumer = function(p)
        while true do 
            local status, value = coroutine.resume(p);
            
            if not status then
                -- 获取当前堆栈信息
                value = debug.traceback(producer, value)              
                error(value)              
            end
            
            print("consume: ", value);
        end
    end
    
    producer = coroutine.create(produceFunc);
    consumer(producer);
end

return M;