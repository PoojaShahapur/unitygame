MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
@brief SkinItem
]]

local M = GlobalNS.Class(GlobalNS.ObjectItemBase);
M.clsName = "SkinItem";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mNativeItem = nil;
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	M.super.dispose(self);
end

function M:getBaseId()
	return self.mNativeItem:getBaseId();
end

function M:getSkinId()
	local tableItem = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_SKIN, self:getBaseId());
	return tableItem.skinid;
end

function M:getImage()
	local tableItem = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_SKIN, self:getBaseId());
	return tableItem.image;
end

function M:getObjectImage()
	local ret = GlobalNS.CSSystem.getObjectImageByBaseId(self:getObjectId());
	return ret;
end

-- 获取对应道具类型
function M:getObjectType()
	local ret = GlobalNS.CSSystem.getObjectTypeByBaseId(self:getObjectId());
	return ret;
end

--[[
-- 获取皮肤表中的类型
function M:getType()
	local retType = GlobalNS.UtilMath.MaxNum;
	local tableItem = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_SKIN, self:getBaseId());
	
	if(nil ~= tableItem) then
		retType = tableItem.type;
	end
	
	return retType;
end
]]

-- 获取对应道具 Id
function M:getObjectId()
	--return 10000 + self:getSkinId();
	return GlobalNS.UtilLogic.convSkinTableSkinId2ObjectBaseId(self:getSkinId());
end

function M:getName()
	return GlobalNS.CSSystem.getObjectNameByBaseId(self:getObjectId());
end

--获取皮肤激活的价格
function M:getActivePrice()
	local tableItem = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_SKIN, self:getBaseId());
	return tableItem.money;	
end

--获取道具表中皮肤的价格
function M:getPrice()
	local ret = GlobalNS.CSSystem.getObjectPriceByBaseId(self:getObjectId());
	return ret;
end

-- 获取激活的钱的类型
function M:getActiveMoneyType()
	return GlobalNS.MoneyType.eMoney;
end

function M:getMoneyType()
	local ret = GlobalNS.CSSystem.getObjectMontyTypeByBaseId(self:getObjectId());
	return ret;
end

--获取对应道具的精灵
function M:getObjectSpriteName()
	local name = GlobalNS.UtilLogic.getSpriteName(self:getObjectImage());
	return name;
end

function M:getSpriteName()
	local name = GlobalNS.UtilLogic.getSpriteName(self:getImage());
	return name;
end

-- 获取道具的图集目录
function M:getObjectAtlasPath()
	local atlasPath = GlobalNS.UtilLogic.getAtlasPath(self:getObjectType(), self:getObjectImage());
	return atlasPath;
end

-- 获取图集路径
function M:getAtlasPath()
	local atlasPath = GlobalNS.UtilLogic.getAtlasPath(self:getObjectType(), self:getImage());
	return atlasPath;
end

function M:getNativeItem()
	return self.mNativeItem;
end

function M:setNativeItem(value)
	self.mNativeItem = value;
end

function M:getPanelTypeZeroIndex()
	local ret = GlobalNS.UtilMath.MaxNum;
	
	ret = GlobalNS.UtilLogic.convObjectType2PanelType(self:getObjectType()) - 1;
	
	return ret;
end

function M:getBaseIdListBySkinId()
	local skinId = self:getSkinId();
	local skinTable = GCtx.mTableSys:getTable(GlobalNS.TableId.TABLE_SKIN);
	local baseIdList = skinTable:getBaseIdListBySkinId(skinId);
	return baseIdList;
end

function M:isPiFu()
	return self:getBaseId() <= 20000;
end

function M:isBullet()
	return self:getBaseId() > 20000;
end

--获取对应道具的描述
function M:getUsageDesc()
	local ret = GlobalNS.CSSystem.getObjectUsageDescByBaseId(self:getObjectId());
	return ret;
end

function M:getAcquireDesc()
	local ret = GlobalNS.CSSystem.getObjectAcquireDescByBaseId(self:getObjectId());
	return ret;
end

return M;