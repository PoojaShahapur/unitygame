MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.Auxiliary.AuxProperty.AuxIntProperty");

local M = GlobalNS.Class(GlobalNS.AuxIntProperty);
M.clsName = "AuxRecordIntProperty";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mPreData = GlobalNS.UtilMath.MaxNum;
end

function M:dtor()
	
end

function M:setData(value)
	self.mPreData = self.mData;

	M.super.setData(self, value);
end

function M:getPreData()
	return self.mPreData;
end

function M:isPreValid()
	return self.mPreData ~= GlobalNS.UtilMath.MaxNum;
end

return M;