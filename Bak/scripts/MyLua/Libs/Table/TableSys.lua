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
	self.mDicTable = GlobalNS.new(GlobalNS.MDictionary);
	
    self.mDicTable:Add(GlobalNS.TableId.TABLE_OBJECT, GlobalNS.new(GlobalNS.TableBase, "ObjectBase_client.bytes", "ObjectBase_client"));
    self.mDicTable:Add(GlobalNS.TableId.TABLE_SKILL, GlobalNS.new(GlobalNS.TableBase, "SkillBase_client.bytes", "SkillBase_client"));    -- 添加一个表的步骤三
    self.mDicTable:Add(GlobalNS.TableId.TABLE_STATE, GlobalNS.new(GlobalNS.TableBase, "StateBase_client.bytes", "StateBase_client"));
	self.mDicTable:Add(GlobalNS.TableId.TABLE_SKIN, GlobalNS.new(GlobalNS.TableSkin));
end

function M:dtor()
	
end

function M:init()
	self.mDicTable:value(GlobalNS.TableId.TABLE_SKIN):init();
end

function M:dispose()
	
end

-- 返回一个表
function M:getTable(tableId)
	--[[
	local table = self.mDicTable:value(tableId);
	
	if (nil == table) then
		self:loadOneTable(tableId);
		table = self.mDicTable:value(tableId);
	end
	
	return table.mList;
	]]
	
	local table = self.mDicTable:value(tableId);
	return table;
end

-- 返回一个表中一项，返回的时候表中数据全部加载到 Item 中
function M:getItem(tableId, itemId)
	--[[
    local table = self.mDicTable:value(tableId);
	
    if (nil == table.mByteBuffer) then
		self:loadOneTable(tableId);
		table = self.mDicTable:value(tableId);
	end
	
    local ret = self:findDataItem(table, itemId);

    if (nil ~= ret and nil == ret.mItemBody) then
        self:loadOneTableOneItemAll(tableId, table, ret);
    end

    if (nil == ret) then
        -- 日志
    end

	return ret;
	]]
	local tableItem = nil;
	
	if(GlobalNS.TableId.TABLE_OBJECT == tableId) then
		tableItem = LuaExcelManager.object_object[itemId];
	elseif(GlobalNS.TableId.TABLE_SKIN == tableId) then
		tableItem = LuaExcelManager.skin_skin[itemId];
	elseif(GlobalNS.TableId.TABLE_ATLAS_AND_IMAGE == tableId) then
		tableItem = nil;
	end
	
	return tableItem;
end

-- 加载一个表
function M:loadOneTable(tableId)
	local table = self.mDicTable:value(tableId);
	
	local auxBytesLoader = GlobalNS.new(GlobalNS.AuxBytesLoader);
	local path = GlobalNS.UtilPath.CombineTwo(GCtx.mConfig.mPathLst[GlobalNS.ResPathType.ePathTablePath], table.mResName);
	
	auxBytesLoader:syncLoad(path, self, self.onLoadEventHandle);
end

-- 加载一个表完成
function M:onLoadEventHandle(dispObj)
    self.mRes = dispObj;
    if (self.mRes:hasSuccessLoaded()) then
        GCtx.mLogSys:log(self.mRes:getLogicPath(), GlobalNS.LogTypeId.eLogCommon);

        local bytes = self.mRes:getBytes();
        if (nil ~= bytes) then
            self.m_byteArray = GlobalNS.CSSystem.buildByteBuffer();
            self.m_byteArray:clear();
            self.m_byteArray:writeBytes(bytes, 0, bytes.Length, true);
            self.m_byteArray:setPos(0);
            self:readTable(self:getTableIdByPath(self.mRes:getLogicPath()), self.m_byteArray);
        end
    elseif (self.mRes:hasFailed()) then
		GCtx.mLogSys:log(self.mRes:getLogicPath(), GlobalNS.LogTypeId.eLogCommon);
    end

    -- 卸载资源
    self.mRes:dispose();
	self.mRes = nil;
end

-- 根据路径查找表的 ID
function M:getTableIdByPath(path)
	local tablePath = "";
	
    for key, value in pairs(self.mDicTable:getData()) do
		tablePath = GlobalNS.UtilPath.CombineTwo(GCtx.mConfig.mPathLst[GlobalNS.ResPathType.ePathTablePath], value.mResName);
        if (tablePath == path) then
            return key;
        end
    end

    return 0;
end

-- 加载一个表中一项的所有内容
function M:loadOneTableOneItemAll(tableID, table, itemBase)
    if (GlobalNS.TableId.TABLE_OBJECT == tableID) then
        itemBase:parseBodyByteBuffer(
			table.mByteBuffer, 
			itemBase.mItemHeader.mOffset, 
			GlobalNS.TableObjectItemBody
			);
    elseif (GlobalNS.TableId.TABLE_CARD == tableID) then
        itemBase:parseBodyByteBuffer(
			table.mByteBuffer, 
			itemBase.mItemHeader.mOffset, 
			GlobalNS.TableCardItemBody
			);
    elseif (GlobalNS.TableId.TABLE_SKILL == tableID) then  -- 添加一个表的步骤四
        itemBase:parseBodyByteBuffer(
			table.mByteBuffer, 
			itemBase.mItemHeader.mOffset, 
			GlobalNS.TableSkillItemBody
			);
    elseif (GlobalNS.TableId.TABLE_JOB == tableID) then
        itemBase:parseBodyByteBuffer(
			table.mByteBuffer, 
			itemBase.mItemHeader.mOffset, 
			GlobalNS.TableJobItemBody
			);
    elseif (GlobalNS.TableId.TABLE_SPRITEANI == tableID) then
        itemBase:parseBodyByteBuffer(
			table.mByteBuffer, 
			itemBase.mItemHeader.mOffset, 
			GlobalNS.TableSpriteAniItemBody
			);
    elseif (GlobalNS.TableId.TABLE_RACE == tableID) then
        itemBase:parseBodyByteBuffer(
			table.mByteBuffer, 
			itemBase.mItemHeader.mOffset, 
			GlobalNS.TableRaceItemBody
			);
    elseif (GlobalNS.TableId.TABLE_STATE == tableID) then
        itemBase:parseBodyByteBuffer(
			table.mByteBuffer, 
			itemBase.mItemHeader.mOffset, 
			GlobalNS.TableStateItemBody
			);
    end
end

-- 获取一个表的名字
function M:getTableName(tableID)
	local table = self.mDicTable:value(tableID);
	if (nil ~= table) then
		return table.mTableName;
	end
	return "";
end

-- 读取一个表，仅仅读取表头
function M:readTable(tableID, bytes)
    local table = self.mDicTable:value(tableID);
    table.mByteBuffer = bytes;

    --bytes:setEndian(GlobalNS.EEndian.eLITTLE_ENDIAN);
	bytes:setEndian(GlobalNS.CSSystem.EEndian.eLITTLE_ENDIAN);
	--local tmp = nil;
    local count = 0;
    --tmp, count = bytes:readUnsignedInt32(count);
	_, count = bytes:readUnsignedInt32(count);
    local i = 0;
    local item = nil;
    while(i < count) do
        item = GlobalNS.new(GlobalNS.TableItemBase);
        item:parseHeaderByteBuffer(bytes);
        table.mList:Add(item);
		
		i = i + 1
    end
end

-- 查找表中的一项
function M:findDataItem(table, id)
	local size = table.mList:Count();
	local low = 0;
	local high = size - 1;
	local middle = 0;
	local idCur = 0;
	
	while (low <= high) do
		middle = GlobalNS.UtilMath.floor((low + high) / 2);
        idCur = table.mList:get(middle).mItemHeader.mId;
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
        return table.mList:get(middle);
	end
	
	return nil;
end

return M;