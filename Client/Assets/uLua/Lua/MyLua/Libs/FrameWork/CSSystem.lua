require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "CSSystem";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:setNeedUpdate(value)
    
end

return M;