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
	this.UtilApi = SDK.Lib.UtilApi;
	this.MFileSys = SDK.Lib.MFileSys;
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
    return this.MFileSys.readLuaBufferToFile(file);
end

-- UtilApi 接口
function M.addEventHandle(go, luaTable, func)
    this.UtilApi.addEventHandle(go, luaTable, func);
end

function M.GoFindChildByName(name)
    return this.UtilApi.GoFindChildByName(name);
end

function M.TransFindChildByPObjAndPath(pObject, path)
    return this.UtilApi.TransFindChildByPObjAndPath(pObject, path);
end

function M.SetParent(child, parent, worldPositionStays)
    this.UtilApi.SetParent(child, parent, worldPositionStays);
end

function M.SetRectTransformParent(child, parent, worldPositionStays)
    this.UtilApi.SetRectTransParent(child, parent, worldPositionStays);
end

return M;