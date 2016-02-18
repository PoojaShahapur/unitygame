require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestModeOp";
GlobalNS[M.clsName] = M;

function M:run()
    self:test();
end

function M:test()
    local a = 10.56;
    local b = 2.13;
    local ret = 0;
    ret = a % b;
    print(ret);
    
    local c = 20;
    local d = 8;
    ret = c % d;
    print(ret);
end

return M;