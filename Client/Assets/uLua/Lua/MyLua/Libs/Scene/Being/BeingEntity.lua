require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

--[[
    @brief 生物 
]]

local M = GlobalNS.Class(GlobalNS.SceneEntity);
M.clsName = "BeingEntity";
GlobalNS[M.clsName] = M;

function M:ctor()

end