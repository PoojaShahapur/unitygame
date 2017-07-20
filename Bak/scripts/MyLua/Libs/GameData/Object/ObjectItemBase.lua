MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
@brief ObjectItemBase
]]

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ObjectItemBase";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mItemType = 0;
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
end

function M:getItemType()
	return self.mItemType;
end

return M;