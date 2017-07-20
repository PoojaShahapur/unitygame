MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
@brief DataItemShop
]]

local M = GlobalNS.Class(GlobalNS.ObjectItemBase);
M.clsName = "ShopDataItem";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mItemType = GlobalNS.DataItemType.eShopItem;
	self.mNativeItem = nil;
	self.mBaseOjectItem = nil;
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	self.mBaseOjectItem = nil;
end

function M:getBaseId()
	local ret = 0;
	
	ret = self.mNativeItem:getBaseId();
	
	return ret;
end

function M:getBaseObjectId()
	local ret = 0;
	
	if(nil ~= self.mBaseOjectItem) then
		ret = self.mBaseOjectItem.id;
	else
		ret = self.mNativeItem:getBaseObjectId();
	end
	
	return ret;
end

function M:getComposeId()
	return self.mNativeItem:getComposeId();
end

function M:setNativeItem(value)
	self.mNativeItem = value;
	self.mBaseOjectItem = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_OBJECT, self:getBaseObjectId());
end

function M:getName()
	return self.mNativeItem:getName();
end

function M:getPrice()
	return self.mNativeItem:getPrice();
end

function M:getMoneyType()
	return self.mNativeItem:getMoneyType();
end

function M:getImage()
	return self.mNativeItem:getImage();
end

function M:getType()
	return self.mNativeItem:getType();
end

function M:getTopType()
	return self.mNativeItem:getTopType();
end

function M:getSubType()
	return self.mNativeItem:getSubType();
end

function M:getTopTypeZeroIndex()
	return self.mNativeItem:getTopTypeZeroIndex();
end

function M:getSubTypeZeroIndex()
	return self.mNativeItem:getSubTypeZeroIndex();
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

function M:getShopId()
	return self.mNativeItem:getShopId();
end

function M:getUsageDesc()
	local ret = self.mNativeItem:getUsageDesc();
	return ret;
end

function M:getAcquireDesc()
	local ret = self.mNativeItem:getAcquireDesc();
	return ret;
end

function M:isMoneyEnough()
	local ret = true;
	local money = self:getPrice();
	
	if(GlobalNS.MoneyType.eMoney == self:getMoneyType()) then
		if(GCtx.mPlayerData.mHeroData.mMoney < money) then
			ret = false;
		end
	elseif(GlobalNS.MoneyType.eTicket == self:getMoneyType()) then
		if(GCtx.mPlayerData.mHeroData.mTicket < money) then
			ret = false;
		end
	elseif(GlobalNS.MoneyType.ePlastic == self:getMoneyType()) then
		if(GCtx.mPlayerData.mHeroData.mPlastic < money) then
			ret = false;
		end
	end
	
	return ret;
end

--获取道具表中的价值
function M:getObjectWorthValue()
	local ret = 0;
	
	if(nil ~= self.mBaseOjectItem.plastic and self.mBaseOjectItem.plastic > 0) then
		ret = self.mBaseOjectItem.plastic;
	elseif(nil ~= self.mBaseOjectItem.ticket and self.mBaseOjectItem.ticket > 0) then
		ret = self.mBaseOjectItem.ticket;
	elseif(nil ~= self.mBaseOjectItem.money and self.mBaseOjectItem.money > 0) then
		ret = self.mBaseOjectItem.money;
	end
	
	return ret;
end

function M:getStoreAcquireMode()
	return self.mBaseOjectItem.storeacquiremode;
end

function M:isEndAcquireMode()
	return self.mBaseOjectItem.storeacquiremode == 2;
	--return true;
end

function M:getAcquireWayDesc()
	return self.mBaseOjectItem.acquirewaydesc;
	--return "aaaaaaaaaaaa";
end

function M:getAcquireDetailDesc()
	return self.mBaseOjectItem.acquiredetaildesc;
	--return "bbbbbb";
end

function M:getBaseObjectItem()
	return self.mBaseOjectItem;
end

return M;