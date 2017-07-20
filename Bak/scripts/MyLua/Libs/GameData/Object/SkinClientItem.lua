MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.GameData.Object.SkinItem");

--[[
@brief SkinClientItem
]]

local M = GlobalNS.Class(GlobalNS.SkinItem);
M.clsName = "SkinClientItem";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mItemType = GlobalNS.DataItemType.eSkinClientItem;
	self.mBaseItem = nil;
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
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