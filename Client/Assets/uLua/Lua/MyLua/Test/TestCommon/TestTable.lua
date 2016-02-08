require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestTable";
GlobalNS[M.clsName] = M;

function M:run()
    self(self, 1, 2, 3);
end

function M:call(...)
    local arr = {...};
    print(arr);
    local tuple = unpack(arr);
    print(tuple);
end

return M;