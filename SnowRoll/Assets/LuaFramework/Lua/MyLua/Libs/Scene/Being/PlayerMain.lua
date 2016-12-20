MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @brief 主角
]]

local M = GlobalNS.Class(GlobalNS.Player);
M.clsName = "PlayerMain";
GlobalNS[M.clsName] = M;

function M:ctor()

end