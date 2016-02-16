require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestEventDispatch";
GlobalNS[M.clsName] = M;

function M:run()
    
end

function M:test()
    
end

return M;