require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

-- CS 中的绑定
local M = {};
M.clsName = "CSSystem";
GlobalNS[M.clsName] = M;
local this = M;

function M.init()
    this.Ctx = SDK.Lib.Ctx;
    this.MsgLocalStorage = SDK.Lib.MsgLocalStorage;
    this.LuaToCS = SDK.Lib.LuaToCS;
end

function M.setNeedUpdate(value)
    
end

-- 日志区域
function M.log(message, logTypeId)
    this.Ctx.m_logSys:log(message, logTypeId);
end

function M.warn(message, logTypeId)
    this.Ctx.m_logSys:warn(message, logTypeId);
end

function M.error(message, logTypeId)
    this.Ctx.m_logSys:error(message, logTypeId);
end

-- lua cs 交互区域
function M.onTestProtoBuf(msg)
    this.LuaToCS.onTestProtoBuf(msg);
end

-- 网络区域
function M.sendFromLua(id, buffer)
    this.Ctx.m_luaSystem.sendFromLua(id, buffer);
end

function M.readLuaBufferToFile(file)
    return this.MsgLocalStorage.readLuaBufferToFile(file);
end

return M;