require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);    -- 定义一个类，必须从返回的类中添加成员
M.clsName = "TestMisc";
GlobalNS[M.clsName] = M;

function M:run()
    self:testArray();
    self:testLen();
    self:testFuncEnv();
    self:testDispatcher();
    self:testTimerMgr();
    self:testUI();
end

function M:testArray()
    local array = GlobalNS.new(GlobalNS.MList);
    local metatable = array.metatable;   -- 在 lua 中是不能直接这样取值的
    array:add(1);
    array:add(2);
    array:add(3);
    
    array:remove(2);
end

function M:testLen()
    local tbs = {};
    tbs = {[2] = 1};
    tbs["aaa"] = "bbb";
    local len = #tbs;
    len = table.getn(tbs);
    print(len);
end

function M:testFuncEnv()
    require "MyLua.Test.TestEnv.TestEnv";
    local aaa = 411;
end

function M:testDispatcher()
    local callOnceEventDispatch = GlobalNS.new(GlobalNS.CallOnceEventDispatch);
    callOnceEventDispatch:addEventHandle(eventCall);
    callOnceEventDispatch:dispatchEvent(nil);
end

function M:eventCall(dispObj)

end

function M:testTimerMgr()
    local timerMgr =  GlobalNS.new(GlobalNS.TimerMgr);
    local aaa = 10;
end

function M:testUI()
    
end

return M;