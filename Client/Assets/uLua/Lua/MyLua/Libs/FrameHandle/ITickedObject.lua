require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ITickedObject";
GlobalNS[M.clsName] = M;

function M:onTick(delta)

end

return M;