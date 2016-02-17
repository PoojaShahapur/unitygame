require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.DataStruct.MList"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestDataStruct";
GlobalNS[M.clsName] = M;

function M:run()
    self:testNum();
end

function M:testNum()
    local arr = GlobalNS.new(GlobalNS.MList);
    arr:add(1);
    arr:add(2);
    arr:add(3);
    
    arr:find(1);
end

return M;