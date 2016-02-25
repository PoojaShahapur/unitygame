require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.Scene.SceneEntityBase.SceneEntityBase"

--[[
    @brief 场景中基本的实体
]]

local M = GlobalNS.Class(GlobalNS.SceneEntityBase);
M.clsName = "SceneEntity";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end