MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @biref 掉落物
]]

local M = GlobalNS.Class(GlobalNS.SceneEntityBase);
M.clsName = "FallObjectEntity";
GlobalNS[M.clsName] = M;

function M:ctor()

end