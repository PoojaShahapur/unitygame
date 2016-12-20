MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @brief 生物 
]]

local M = GlobalNS.Class(GlobalNS.SceneEntityBase);
M.clsName = "BeingEntity";
GlobalNS[M.clsName] = M;

function M:ctor()

end