require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestTable";
GlobalNS[M.clsName] = M;

function M:run()
    --self(self, 1, 2, 3);
    self:testTableEqual();
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
    local c = {atta = 10, attb = 20}
    local d = {atta = 11, attb = 20}
    
    if(a == b) then
        print("a == b");
    end
    
    if(a == c) then
        print("a == c");
    end
    
    if(a == d) then
        print("a == d");
    end
end

return M;