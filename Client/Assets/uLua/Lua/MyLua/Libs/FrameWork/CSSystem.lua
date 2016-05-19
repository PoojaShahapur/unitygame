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
    this.UtilPath = SDK.Lib.UtilPath;
    this.GlobalEventCmd = SDK.Lib.GlobalEventCmd;
    this.AuxPrefabLoader = SDK.Lib.AuxPrefabLoader;
end

--[[
function M.setNeedUpdate(value)
    
end
]]

-- 日志区域
function M.log(message, logTypeId)
    if(this.Ctx.m_instance.m_logSys ~= nil) then
        if(logTypeId == nil) then
            GlobalNS.UtilApi.error("CSSystem logTypeId is nil");
        else
            this.Ctx.m_instance.m_logSys:lua_log(message, logTypeId);
        end
    else
        GlobalNS.UtilApi.error("CSSystem LogSys is nil");
    end
end

function M.warn(message, logTypeId)
    this.Ctx.m_instance.m_logSys:lua_warn(message, logTypeId);
end

function M.error(message, logTypeId)
    this.Ctx.m_instance.m_logSys:lua_error(message, logTypeId);
end

-- lua cs 交互区域
function M.onTestProtoBuf(msg)
    this.GlobalEventCmd.onTestProtoBuf(msg);
end

-- 网络区域
function M.sendFromLua(id, buffer)
    this.Ctx.m_luaSystem.sendFromLua(id, buffer);
end

function M.readLuaBufferToFile(file)
    return this.UtilPath.readLuaBufferToFile(file);
end

return M;