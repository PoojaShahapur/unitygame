require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestTable";
GlobalNS[M.clsName] = M;

function M:run()
    --self(self, 1, 2, 3);
    --self:testTableEqual();
    self:testDel();
end

function M:call(...)
    local arr = {...};
    print(arr);
    local tuple = unpack(arr);
    print(tuple);
end

-- 表的引用比较
function M:testTableEqual()
    local a = {atta = 10, attb = 20};
    local b = a;
    local c = {atta = 10, attb = 20};
    local d = {atta = 11, attb = 20};
    
    if(a == b) then
        print("a == b");
    end
    
    if(a == c) then
        print("a == c");
    end
    
    --[[ error
    if(a > c) then
        print("a > c");
    end
    
    if(a < c) then
        print("a < c");
    end
    ]]
    
    if(a == d) then
        print("a == d");
    end
end

function M:testDel()
    local tbl = {};
    tbl.aaa = 10;
    tbl.bbb = 20;
    tbl.ccc = 30;
    -- table.remove 只能用于数组
    -- table.remove(tbl, "bbb");
    -- 删除元素，赋值 nil 后，立马就删除了这个元素
    tbl.bbb = nil;
    
    print("testDel");
end

return M;