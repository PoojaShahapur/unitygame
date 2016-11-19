MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
    @brief 其它玩家
]]

local M = GlobalNS.Class(GlobalNS.Player);
M.clsName = "PlayerOther";
GlobalNS[M.clsName] = M;

function M:ctor()

end