MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.Auxiliary.AuxProperty.AuxPropertyBase");

local M = GlobalNS.Class(GlobalNS.AuxPropertyBase);
M.clsName = "AuxBoolProperty";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mData = false;
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()	
	M.super.dispose(self);
end

function M:getData()
	return self.mData;
end

function M:setData(value)
	if(self.mData ~= value) then
		self.mData = value;
		
		M.super.setData(self, value);
	end
end

function M:isValid()
	return self.mData;
end

function M:reset()
	M.super.reset(self);
	self.mData = false;
end

return M;