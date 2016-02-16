require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestFunction";
GlobalNS[M.clsName] = M;

function M:run()
    --self:testEqual();
    self:testType();
end

function M:func_1()

end

function M:func_2()

end

function M:testEqual()
    local a = self.func_1
    local b = a;
    local c = self.func_2;
    
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
end

function M:testType()
    print(type(self.func_2))
end

return M;