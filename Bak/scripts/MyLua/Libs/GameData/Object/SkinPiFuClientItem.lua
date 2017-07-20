MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.GameData.Object.SkinPiFuItem");

--[[
@brief SkinPiFuClientItem
]]

local M = GlobalNS.Class(GlobalNS.SkinPiFuItem);
M.clsName = "SkinPiFuClientItem";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mItemType = GlobalNS.DataItemType.eSkinClientItem_PiFu;
	self.mBaseItem = nil;
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
	return self.mBaseItem.id;
end

function M:setBaseId(value)
	self.mBaseItem = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_SKIN, value);
end

function M:setBaseItem(value)
	self.mBaseItem = value;
end

return M;