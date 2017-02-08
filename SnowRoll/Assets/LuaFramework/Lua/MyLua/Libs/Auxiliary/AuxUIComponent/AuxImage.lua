MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxWindow");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxUITypeId");

local M = GlobalNS.Class(GlobalNS.AuxWindow);
M.clsName = "AuxImage";
GlobalNS[M.clsName] = M;

function M:ctor(...)
	self.mScale = 1;
end

function M:dtor()
	
end

function M:dispose()
	
end

function M:setScale(value)
	self.mScale = value;
	self:setGoRectScale();
end

function M:onSelfChanged()
	M.super.onSelfChanged(self);
	
	self:setGoRectScale();
end

function M:setGoRectScale()
	if(nil ~= self.mSelfGo) then
		GlobalNS.UtilApi.setGoRectScale(self.mSelfGo, Vector3.New(self.mScale , 1.0, 1.0));
	end
end

return M;