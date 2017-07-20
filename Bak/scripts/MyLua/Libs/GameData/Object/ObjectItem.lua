MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
@brief ObjectItem 一项背包数据
]]

local M = GlobalNS.Class(GlobalNS.ObjectItemBase);
M.clsName = "ObjectItem";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mItemType = GlobalNS.DataItemType.eObjectItem;
	self.mNativeItem = nil;
	self.mBaseItem = nil;
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	self.mBaseItem = nil;
end

function M:setNativeItem(value)
	self.mNativeItem = value;
	self.mBaseItem = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_OBJECT, self:getBaseId());
end

function M:getBaseId()
	local baseId = 0;
	
	if(nil ~= self.mBaseItem) then
		baseId = self.mBaseItem.id;
	else
		baseId = self.mNativeItem:getBaseId();
	end
	
	return baseId;
end

function M:getName()
	return self.mNativeItem:getName();
end

function  M:getPrice()
	return self.mNativeItem:getPrice();
end

function M:getThisId()
	--[[
	local thisId = GlobalNS.UtilApi.tonumber(self.mNativeItem:getStrThisId());
    return thisId;
	]]
	return self.mNativeItem:getStrThisId();
end

function M:getStrThisId()
	return self.mNativeItem:getStrThisId();
end

function M:getImage()
	return self.mNativeItem:getImage();
end

function M:getNum()
    return self.mNativeItem:getNum();
end

function M:getType()
	return self.mNativeItem:getType();
end

function M:getSpriteName()
	local name = GlobalNS.UtilLogic.getSpriteName(self:getImage());
	return name;
end

-- 获取图集路径
function M:getAtlasPath()
	local atlasPath = GlobalNS.UtilLogic.getAtlasPath(self:getType(), self:getImage());
	return atlasPath;
end

function M:getPanelType()
	return self:getPanelType();
end

function M:getPanelTypeZeroIndex()
	return self.mNativeItem:getPanelTypeZeroIndex();
end

function M:getMoneyType()
	return self.mNativeItem:getPanelTypeZeroIndex();
end

function M:getUsageDesc()
	local ret = self.mNativeItem:getUsageDesc();
	return ret;
end

function M:getAcquireDesc()
	local ret = self.mNativeItem:getAcquireDesc();
	return ret;
end

function M:getStoreAcquireMode()
	return self.mBaseItem.storeacquiremode;
end

function M:isEndAcquireMode()
	return self.mBaseItem.storeacquiremode == 2;
end

function M:getAcquireWayDesc()
	return self.mBaseItem.acquirewaydesc;
end

function M:getAcquireDetailDesc()
	return self.mBaseItem.acquiredetaildesc;
end

--获取道具表中的价值
function M:getObjectWorthValue()
	local ret = 0;
	
	if(nil ~= self.mBaseItem.plastic and self.mBaseItem.plastic > 0) then
		ret = self.mBaseItem.plastic;
	elseif(nil ~= self.mBaseItem.ticket and self.mBaseItem.ticket > 0) then
		ret = self.mBaseItem.ticket;
	elseif(nil ~= self.mBaseItem.money and self.mBaseItem.money > 0) then
		ret = self.mBaseItem.money;
	end
	
	return ret;
end

-- 是否是碎片类型
function M:isPiece()
	local ret = false;
	
	if(self:getType() == GlobalNS.ObjectPanelType.ePiece) then
		ret = true;
	end
	
	return ret;
end

function M:isGift()
	local ret = false;
	
	if(self:getType() == GlobalNS.ObjectPanelType.eGift) then
		ret = true;
	end
	
	return ret;
end

return M;