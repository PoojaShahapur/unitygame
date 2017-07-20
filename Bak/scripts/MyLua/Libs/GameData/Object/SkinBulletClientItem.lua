MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.GameData.Object.SkinBulletItem");

--[[
@brief SkinClientItem
]]

local M = GlobalNS.Class(GlobalNS.SkinBulletItem);
M.clsName = "SkinBulletClientItem";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mItemType = GlobalNS.DataItemType.eSkinClientItem_Bullet;
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
	return self.mBaseItem:getBaseId();
end

function M:setBaseId(value)
	
end

function M:setBaseItem(value)
	self.mBaseItem = value;
	self.mNativeItem = self.mBaseItem:getNativeItem();
end

return M;