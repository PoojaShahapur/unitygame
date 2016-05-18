require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.StaticClass"

--[[
    处理 CS 到 Lua 的全局事件
]]
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "GlobalEventMgr";
GlobalNS[M.clsName] = M;

function M.onReceiveToLua(id, buffer)
    GCtx.mNetMgr:receiveCmd(id, buffer);
end

return M;