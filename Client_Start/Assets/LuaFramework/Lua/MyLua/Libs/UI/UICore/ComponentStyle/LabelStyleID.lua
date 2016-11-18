require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "LabelStyleID";
GlobalNS[M.clsName] = M;

function M.ctor()
    M.eLSID_None = 0;
    M.eTotal = 1;
end

M.ctor();

return M;