--[[
 * @brief 添加一个表的步骤总共分 4 步
 * // 添加一个表的步骤一
 * // 添加一个表的步骤二
 * // 添加一个表的步骤三
 * // 添加一个表的步骤四
]]

require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject:new())
M.clsName = "TableSys"
GlobalNS[M.clsName] = M

function M:ctor()
	self.m_dicTable = GlobalNS.Dictionary:new();
    self.m_dicTable.Add(GlobalNS.TableID.TABLE_OBJECT, GlobalNS.TableBase:new("ObjectBase_client.bytes", "ObjectBase_client"));
    self.m_dicTable.Add(GlobalNS.TableID.TABLE_CARD, GlobalNS.TableBase:new("CardBase_client.bytes", "CardBase_client"));
    self.m_dicTable.Add(GlobalNS.TableID.TABLE_SKILL, GlobalNS.TableBase:new("SkillBase_client.bytes", "SkillBase_client"));    -- 添加一个表的步骤三
    self.m_dicTable.Add(GlobalNS.TableID.TABLE_JOB, GlobalNS.TableBase:new("proBase_client.bytes", "proBase_client"));
    self.m_dicTable.Add(GlobalNS.TableID.TABLE_SPRITEANI, GlobalNS.TableBase:new("FrameAni_client.bytes", "FrameAni_client"));
    self.m_dicTable.Add(GlobalNS.TableID.TABLE_RACE, GlobalNS.TableBase:new("RaceBase_client.bytes", "RaceBase_client"));
    self.m_dicTable.Add(GlobalNS.TableID.TABLE_STATE, GlobalNS.TableBase:new("StateBase_client.bytes", "StateBase_client"));
end

-- 返回一个表
function getTable(tableID)
	table = m_dicTable.value(tableID);
	if (nil == table) then
		self.loadOneTable(tableID);
		table = m_dicTable[tableID];
	end
	return table.m_List;
end

-- 返回一个表中一项，返回的时候表中数据全部加载到 Item 中
function M:getItem(tableID, itemID)
    table = m_dicTable.value(tableID);
    if (nil == table.m_byteBuffer) then
		self.loadOneTable(tableID);
		table = m_dicTable[tableID];
	end
    ret = M.findDataItem(table, itemID);

    if (nil ~= ret and nil == ret.m_itemBody) then
        self.loadOneTableOneItemAll(tableID, table, ret);
    end

    if (nil == ret) then
        -- 日志
    end

	return ret;
end

-- 加载一个表
function loadOneTable(tableID)
	table = m_dicTable.value(tableID);

    --LoadParam param = Ctx.m_instance.m_poolSys.newObject<LoadParam>();
    --LocalFileSys.modifyLoadParam(Path.Combine(Ctx.m_instance.m_cfg.m_pathLst[(int)ResPathType.ePathTablePath], table.m_resName), param);
    --param.m_loadEventHandle = onLoadEventHandle;
    --param.m_loadNeedCoroutine = false;
    --param.m_resNeedCoroutine = false;
    --Ctx.m_instance.m_resLoadMgr.loadResources(param);
    --Ctx.m_instance.m_poolSys.deleteObj(param);
end

-- 加载一个表完成
function onLoadEventHandle(dispObj)
--[[
    m_res = dispObj as ResItem;
    if (m_res.refCountResLoadResultNotify.resLoadState.hasSuccessLoaded())
    {
        Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem0, m_res.GetPath());

        byte[] bytes = m_res.getBytes("");
        if (bytes != null)
        {
            m_byteArray = Ctx.m_instance.m_factoryBuild.buildByteBuffer();
            m_byteArray.clear();
            m_byteArray.writeBytes(bytes, 0, (uint)bytes.Length);
            m_byteArray.setPos(0);
            readTable(getTableIDByPath(m_res.GetPath()), m_byteArray);
        }
    }
    else if (m_res.refCountResLoadResultNotify.resLoadState.hasFailed())
    {
        Ctx.m_instance.m_logSys.debugLog_1(LangItemID.eItem1, m_res.GetPath());
    }

    // 卸载资源
    Ctx.m_instance.m_resLoadMgr.unload(m_res.GetPath(), onLoadEventHandle);
]]
end

-- 根据路径查找表的 ID
function getTableIDByPath(path)
    for key, value in pairs(m_dicTable) do
        if (Ctx.m_instance.m_cfg.m_pathLst[ResPathType.ePathTablePath] + kv.Value.m_resName == path) then
            return kv.Key;
        end
    end

    return 0;
end

-- 加载一个表中一项的所有内容
function loadOneTableOneItemAll(tableID, table, itemBase)
    if (GlobalNS.TableID.TABLE_OBJECT == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, TableObjectItemBody);
    else if (GlobalNS.TableID.TABLE_CARD == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, TableCardItemBody);
    else if (GlobalNS.TableID.TABLE_SKILL == tableID) then  -- 添加一个表的步骤四
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, TableSkillItemBody);
    else if (GlobalNS.TableID.TABLE_JOB == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, TableJobItemBody);
    else if (GlobalNS.TableID.TABLE_SPRITEANI == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, TableSpriteAniItemBody);
    else if (GlobalNS.TableID.TABLE_RACE == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, TableRaceItemBody);
    else if (GlobalNS.TableID.TABLE_STATE == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, TableStateItemBody);
    end
end

-- 获取一个表的名字
function getTableName(tableID)
	table = m_dicTable.value(tableID);
	if (nil ~= table) then
		return table.m_tableName;
	end
	return "";
end

-- 读取一个表，仅仅读取表头
function readTable(tableID, bytes)
    table = m_dicTable.value(tableID);
    table.m_byteBuffer = bytes;

    bytes.setEndian(EEndian.eLITTLE_ENDIAN);
    local len = 0;
    bytes.readUnsignedInt32(len);
    local i = 0;
    item = nil;
    for i = 0, i < len, 1 do
        item = GlobalNS.TableItemBase:new();
        item.parseHeaderByteBuffer(bytes);
        table.m_List.Add(item);
    end
end

-- 查找表中的一项
function findDataItem(table, id)
	local size = table.m_List.Count();
	local low = 0;
	local high = size - 1;
	local middle = 0;
	local idCur = 0;
	
	while (low <= high)
	do
		middle = (low + high) / 2;
        idCur = table.m_List.at(middle).m_itemHeader.m_uID;
		if (idCur == id) then
			break;
		end
		if (id < idCur)then
			high = middle - 1;
		else
			low = middle + 1;
		end
	end
	
	if (low <= high) then
        return table.m_List[middle];
	end
	return nil;
end

return M