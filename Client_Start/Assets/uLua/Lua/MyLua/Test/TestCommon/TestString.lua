require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestString";
GlobalNS[M.clsName] = M;

function M:run()
    self:testEqual();
end

function M:testEqual()
    local a = "aaa";
    local b = a;
    local c = "ccc";
    local d = "ddd";
    
    if(a == b) then
        print("a == b");
    end
    
    if(a == c) then
        print("a == c");
    end
    
    if(a > c) then
        print("a > c");
    end
    
    if(a < c) then
        print("a < c");
    end
    
    if(a == d) then
        print("a == d");
    end
end

return M;