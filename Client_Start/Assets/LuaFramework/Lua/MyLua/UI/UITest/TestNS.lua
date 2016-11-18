require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

-- 功能模块 NS
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "TestNS";
GlobalNS[M.clsName] = M;

return M;