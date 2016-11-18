require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

--[[
    @biref 掉落物
]]

local M = GlobalNS.Class(GlobalNS.SceneEntityBase);
M.clsName = "FallObjectEntity";
GlobalNS[M.clsName] = M;

function M:ctor()

end