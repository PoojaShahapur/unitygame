require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"
require "MyLua.Libs.EventHandle.IDispatchObject"

--local M = GlobalNS.StaticClass(GlobalNS.IDispatchObject);
local M = GlobalNS.StaticClass();
M.clsName = "CmdDispInfo";
GlobalNS[M.clsName] = M;

function M.init()
    M.bu = nil;
    M.byCmd = 0;
    M.byParam = 0;
end

M.init();

return M;