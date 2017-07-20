MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

-- CS 中的绑定
local M = GlobalNS.StaticClass();
M.clsName = "CSSystem";
GlobalNS[M.clsName] = M;
local this = M;

function M.init()
	--[[
    this.Ctx = SDK.Lib.Ctx;
    this.UtilPath = SDK.Lib.UtilPath;
    this.GlobalEventCmd = SDK.Lib.GlobalEventCmd;
    this.AuxPrefabLoader = SDK.Lib.AuxPrefabLoader;
	this.AuxBytesLoader = SDK.Lib.AuxBytesLoader;
	this.AuxSpriteAtlasLoader = SDK.Lib.AuxSpriteAtlasLoader;
	this.AuxUnityAtlasLoader = SDK.Lib.AuxUnityAtlasLoader;
	this.UtilApi = SDK.Lib.UtilApi;
	this.MFileSys = SDK.Lib.MFileSys;
	this.EEndian = SDK.Lib.EEndian;
	this.GkEncode = SDK.Lib.GkEncode;
    this.ModuleId = SDK.Lib.ModuleId;
	
	this.mPackData = this.Ctx.mInstance.mPlayerData.mPackData;
	this.mShopData = this.Ctx.mInstance.mPlayerData.mShopData;
	this.mHeroData = this.Ctx.mInstance.mPlayerData.mHeroData;
	this.mBeginnerGuideSys = this.Ctx.mInstance.mBeginnerGuideSys;
	]]
end

-- 需要从外部更新 Lua 系统
function M.setNeedUpdateFromExternal(value)
    this.Ctx.mInstance.mLuaSystem:setNeedUpdateLua(value);
end

-- LogSys 日志区域
function M.log(message, logTypeId)
    if(this.Ctx.mInstance.mLogSys ~= nil) then
		this.Ctx.mInstance.mLogSys:lua_log(message, logTypeId);
    else
        GlobalNS.UtilApi.error("CSSystem LogSys is nil");
    end
end

function M.warn(message, logTypeId)
    this.Ctx.mInstance.mLogSys:lua_warn(message, logTypeId);
end

function M.error(message, logTypeId)
    this.Ctx.mInstance.mLogSys:lua_error(message, logTypeId);
end

-- GlobalEventCmd 交互区域
function M.onTestProtoBuf(msg)
    this.GlobalEventCmd.onTestProtoBuf(msg);
end

function M.onTestProtoBufBuffer(commandID, buffer)
    this.GlobalEventCmd.onTestProtoBufBuffer(commandID, buffer);
end

--测试消息
function M.testMsg(msgId)
    this.GlobalEventCmd.testFromLua(msgId);
end

--测试移除皮肤
function M.testRemovePiFu(baseId)
    this.GlobalEventCmd.testRemovePiFu(baseId);
end

--测试移除皮肤
function M.testRemovePiFuStr(baseId)
    this.GlobalEventCmd.testRemovePiFuStr(baseId);
end

-- 网络区域
function M.sendFromLua(id, buffer)
    this.Ctx.mInstance.mLuaSystem:sendFromLua(id, buffer);
end

function M.sendFromLuaRpc(buffer)
    this.Ctx.mInstance.mLuaSystem:sendFromLuaRpc(buffer);
end

function M.readLuaBufferToFile(file)
    return this.MFileSys.readLuaBufferToFile(file);
end

-- UtilApi 接口
function M.addEventHandleByPath(go, path, luaTable, luaFunction)
	if(not GlobalNS.UtilApi.IsUObjNil(go)) then
		this.UtilApi.addEventHandle(go, path, luaTable, luaFunction);
	end
end

function M.addEventHandleSelf(go, luaTable, luaFunction)
	if(not GlobalNS.UtilApi.IsUObjNil(go)) then
		this.UtilApi.addEventHandle(go, luaTable, luaFunction, 0, false);
	end
end

function M.addButtonDownEventHandle(go, luaTable, luaFunction)
	if(not GlobalNS.UtilApi.IsUObjNil(go)) then
		this.UtilApi.addButtonDownEventHandle(go, luaTable, luaFunction, 0);
	end
end

function M.addButtonUpEventHandle(go, luaTable, luaFunction)
	if(not GlobalNS.UtilApi.IsUObjNil(go)) then
		this.UtilApi.addButtonUpEventHandle(go, luaTable, luaFunction, 0);
	end
end

function M.addButtonExitEventHandle(go, luaTable, luaFunction)
	if(not GlobalNS.UtilApi.IsUObjNil(go)) then
		this.UtilApi.addButtonExitEventHandle(go, luaTable, luaFunction, 0);
	end
end

function M.removeEventHandleSelf(go, luaTable, luaFunction)
	if(not GlobalNS.UtilApi.IsUObjNil(go)) then
		this.UtilApi.removeEventHandleSelf(go, luaTable, luaFunction, 0);
	end
end

function M.removeButtonDownEventHandle(go, luaTable, luaFunction)
	if(not GlobalNS.UtilApi.IsUObjNil(go)) then
		this.UtilApi.removeButtonDownEventHandle(go, luaTable, luaFunction, 0);
	end
end

function M.removeButtonUpEventHandle(go, luaTable, luaFunction)
	if(not GlobalNS.UtilApi.IsUObjNil(go)) then
		this.UtilApi.removeButtonUpEventHandle(go, luaTable, luaFunction, 0);
	end
end

function M.removeButtonExitEventHandle(go, luaTable, luaFunction)
	if(not GlobalNS.UtilApi.IsUObjNil(go)) then
		this.UtilApi.removeButtonExitEventHandle(go, luaTable, luaFunction, 0);
	end
end

function M.GoFindChildByName(name)
    return this.UtilApi.GoFindChildByName(name);
end

function M.TransFindChildByPObjAndPath(pObject, path)
    return this.UtilApi.TransFindChildByPObjAndPath(pObject, path);
end

function M.SetParent(child, parent, worldPositionStays)
	if(nil == worldPositionStays) then
		worldPositionStays = true;
	end
	
    this.UtilApi.SetParent(child, parent, worldPositionStays);
end

function M.SetRectTransformParent(child, parent, worldPositionStays)
    this.UtilApi.SetRectTransParent(child, parent, worldPositionStays);
end

function M.addToggleHandle(go, table, method)
    this.UtilApi.addToggleHandle(go, table, method);
end

function M.addInputEndHandle(go, table, method)
    this.UtilApi.addInputEndHandle(go, table, method);
end

function M.addDropdownHandle(go, table, method)
    this.UtilApi.addDropdownHandle(go, table, method);
end

function M.buildByteBuffer()
	return this.Ctx.mInstance.mFactoryBuild:buildByteBuffer();
end

function M.convPtFromLocal2Local(from, to)
    return this.UtilApi.convPtFromLocal2Local(from, to, Vector3.zero);
end

--进行分裂
function M.startSplit()
	this.Ctx.mInstance.mPlayerMgr:startSplit();
end

--吐一个小球
function M.Fire()
	this.Ctx.mInstance.mPlayerMgr:FireInTheHole();
end

function M.startFire()
	this.Ctx.mInstance.mPlayerMgr:startFire();
end

function M.stopFire()
	this.Ctx.mInstance.mPlayerMgr:stopFire();
end

--设置 Lua 初始化完成
function M.setLuaInited(value)
	this.Ctx.mInstance.mLuaSystem:setLuaInited(value);
end

function M.reqShopData(shopId)
	this.mShopData:reqData(shopId);
end

function M.reqByShopItem(shopId, pos)
	this.mShopData:reqByShopItem(shopId, pos);
end

function M.reqUseObject(uid)
	this.mPackData:reqUseObject(uid);
end

function M.reqUseSkin(baseId)
	this.mPackData:reqUseSkin(baseId);
end

function M.reqUseBullet(baseId)
	this.mPackData:reqUseBullet(baseId);
end

function M.reqActiveSkin(baseId)
	this.mPackData:reqActiveSkin(baseId);
end

function M.getObjectNameByBaseId(baseId)
	local name = "";
	name = this.mPackData:getObjectNameByBaseId(baseId);
	return name;
end

function M.getObjectTypeByBaseId(baseId)
	local ret = 0;
	ret = this.mPackData:getObjectTypeByBaseId(baseId);
	return ret;
end

function M.getObjectImageByBaseId(baseId)
	local ret = 0;
	ret = this.mPackData:getObjectImageByBaseId(baseId);
	return ret;
end

function M.getObjectPriceByBaseId(baseId)
	local ret = 0;
	ret = this.mPackData:getObjectPriceByBaseId(baseId);
	return ret;
end

function M.getObjectMontyTypeByBaseId(baseId)
	local ret = 0;
	ret = this.mPackData:getObjectMontyTypeByBaseId(baseId);
	return ret;
end

function M.getObjectUsageDescByBaseId(baseId)
	local ret = "";
	ret = this.mPackData:getObjectUsageDescByBaseId(baseId);
	return ret;
end

function M.getObjectAcquireDescByBaseId(baseId)
	local ret = "";
	ret = this.mPackData:getObjectAcquireDescByBaseId(baseId);
	return ret;
end

function M.enableGuide(value)
	this.mBeginnerGuideSys:setIsEnableGuide(true);
end

-- 是否开启新手引导
function M:isEnableGuide()
	return this.mBeginnerGuideSys:isEnableGuide();
end

-- 执行新手引导
function M.execGuide()
	this.mBeginnerGuideSys:execGuide();
end

return M;