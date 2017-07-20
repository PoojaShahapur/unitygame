MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.GameData.Object.SkinItem");

--[[
@brief SkinBulletItem
]]

local M = GlobalNS.Class(GlobalNS.SkinItem);
M.clsName = "SkinBulletItem";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mItemType = GlobalNS.DataItemType.eSkinItem_Bullet;
	self.mBaseIdList = nil;
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	M.super.dispose(self);
end

--子弹皮肤 Id 就是道具 Id
function M:getSkinId()
	return self:getBaseId();
end

--[[
function M:getType()
	return self.mNativeItem:getType();
end
]]

function M:getBaseIdListBySkinId()
	if(nil == self.mBaseIdList) then
		self.mBaseIdList = GlobalNS.new(GlobalNS.MList);
		self.mBaseIdList:add(self:getBaseId());
	end
	
	return self.mBaseIdList;
end

function M:getImage()
	local ret = GlobalNS.CSSystem.getObjectImageByBaseId(self:getObjectId());
	return ret;
end

function M:getPrice()
	local ret = GlobalNS.CSSystem.getObjectPriceByBaseId(self:getObjectId());
	return ret;
end

function M:getObjectId()
	return self:getBaseId();
end

return M;