--[[
 * @brief 添加一个表的步骤总共分 4 步
 * // 添加一个表的步骤一
 * // 添加一个表的步骤二
 * // 添加一个表的步骤三
 * // 添加一个表的步骤四
]]

MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TableSys";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.m_dicTable = GlobalNS.new(GlobalNS.MDictionary);
    self.m_dicTable:Add(GlobalNS.TableID.TABLE_OBJECT, GlobalNS.new(GlobalNS.TableBase, "ObjectBase_client.bytes", "ObjectBase_client"));
    self.m_dicTable:Add(GlobalNS.TableID.TABLE_CARD, GlobalNS.new(GlobalNS.TableBase, "CardBase_client.bytes", "CardBase_client"));
    self.m_dicTable:Add(GlobalNS.TableID.TABLE_SKILL, GlobalNS.new(GlobalNS.TableBase, "SkillBase_client.bytes", "SkillBase_client"));    -- 添加一个表的步骤三
    self.m_dicTable:Add(GlobalNS.TableID.TABLE_JOB, GlobalNS.new(GlobalNS.TableBase, "proBase_client.bytes", "proBase_client"));
    self.m_dicTable:Add(GlobalNS.TableID.TABLE_SPRITEANI, GlobalNS.new(GlobalNS.TableBase, "FrameAni_client.bytes", "FrameAni_client"));
    self.m_dicTable:Add(GlobalNS.TableID.TABLE_RACE, GlobalNS.new(GlobalNS.TableBase, "RaceBase_client.bytes", "RaceBase_client"));
    self.m_dicTable:Add(GlobalNS.TableID.TABLE_STATE, GlobalNS.new(GlobalNS.TableBase, "StateBase_client.bytes", "StateBase_client"));
end

-- 返回一个表
function getTable(tableID)
	local table = self.m_dicTable:value(tableID);
	if (nil == table) then
		self:loadOneTable(tableID);
		table = self.m_dicTable:value(tableID);
	end
	return table.m_List;
end

-- 返回一个表中一项，返回的时候表中数据全部加载到 Item 中
function M:getItem(tableID, itemID)
    local table = self.m_dicTable:value(tableID);
    if (nil == table.m_byteBuffer) then
		self:loadOneTable(tableID);
		table = self.m_dicTable:value(tableID);
	end
    local ret = self:findDataItem(table, itemID);

    if (nil ~= ret and nil == ret.m_itemBody) then
        self:loadOneTableOneItemAll(tableID, table, ret);
    end

    if (nil == ret) then
        -- 日志
    end

	return ret;
end

-- 加载一个表
function M:loadOneTable(tableID)
	local table = self.m_dicTable:value(tableID);
	
	local auxBytesLoader = GlobalNS.new(GlobalNS.AuxBytesLoader);
	local path = GlobalNS.UtilPath.CombineTwo(GCtx.m_config.m_pathLst[GlobalNS.ResPathType.ePathTablePath], table.m_resName);
	
	auxBytesLoader:syncLoad(path, self, self.onLoadEventHandle);
end

-- 加载一个表完成
function M:onLoadEventHandle(dispObj)
    self.m_res = dispObj;
    if (self.m_res:hasSuccessLoaded()) then
        GCtx.mLogSys:log(self.m_res:getLogicPath(), GlobalNS.LogTypeId.eLogCommon);

        local bytes = self.m_res:getBytes();
        if (nil ~= bytes) then
            self.m_byteArray = GlobalNS.CSSystem.buildByteBuffer();
            self.m_byteArray:clear();
            self.m_byteArray:writeBytes(bytes, 0, bytes.Length, true);
            self.m_byteArray:setPos(0);
            self:readTable(self:getTableIDByPath(self.m_res:getLogicPath()), self.m_byteArray);
        end
    elseif (self.m_res:hasFailed()) then
		GCtx.mLogSys:log(self.m_res:getLogicPath(), GlobalNS.LogTypeId.eLogCommon);
    end

    -- 卸载资源
    self.m_res:dispose();
	self.m_res = nil;
end

-- 根据路径查找表的 ID
function M:getTableIDByPath(path)
	local tablePath = "";
	
    for key, value in pairs(self.m_dicTable:getData()) do
		tablePath = GlobalNS.UtilPath.CombineTwo(GCtx.m_config.m_pathLst[GlobalNS.ResPathType.ePathTablePath], value.m_resName);
        if (tablePath == path) then
            return key;
        end
    end

    return 0;
end

-- 加载一个表中一项的所有内容
function M:loadOneTableOneItemAll(tableID, table, itemBase)
    if (GlobalNS.TableID.TABLE_OBJECT == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, GlobalNS.TableObjectItemBody);
    elseif (GlobalNS.TableID.TABLE_CARD == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, GlobalNS.TableCardItemBody);
    elseif (GlobalNS.TableID.TABLE_SKILL == tableID) then  -- 添加一个表的步骤四
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, GlobalNS.TableSkillItemBody);
    elseif (GlobalNS.TableID.TABLE_JOB == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, GlobalNS.TableJobItemBody);
    elseif (GlobalNS.TableID.TABLE_SPRITEANI == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, GlobalNS.TableSpriteAniItemBody);
    elseif (GlobalNS.TableID.TABLE_RACE == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, GlobalNS.TableRaceItemBody);
    elseif (GlobalNS.TableID.TABLE_STATE == tableID) then
        itemBase.parseBodyByteBuffer(table.m_byteBuffer, itemBase.m_itemHeader.m_offset, GlobalNS.TableStateItemBody);
    end
end

-- 获取一个表的名字
function M:getTableName(tableID)
	local table = self.m_dicTable:value(tableID);
	if (nil ~= table) then
		return table.m_tableName;
	end
	return "";
end

-- 读取一个表，仅仅读取表头
function M:readTable(tableID, bytes)
    local table = self.m_dicTable:value(tableID);
    table.m_byteBuffer = bytes;

    --bytes:setEndian(GlobalNS.EEndian.eLITTLE_ENDIAN);
	bytes:setEndian(GlobalNS.CSSystem.EEndian.eLITTLE_ENDIAN);
    local count = 0;
    bytes:readUnsignedInt32(count);
    local i = 0;
    local item = nil;
    while(i < count) do
        item = GlobalNS.new(GlobalNS.TableItemBase);
        item:parseHeaderByteBuffer(bytes);
        table.m_List:Add(item);
		
		i = i + 1
    end
end

-- 查找表中的一项
function M:findDataItem(table, id)
	local size = table.m_List:Count();
	local low = 0;
	local high = size - 1;
	local middle = 0;
	local idCur = 0;
	
	while (low <= high) do
		middle = (low + high) / 2;
        idCur = table.m_List:at(middle).m_itemHeader.m_uID;
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
        return table.m_List:at(middle);
	end
	
	return nil;
end

return M;