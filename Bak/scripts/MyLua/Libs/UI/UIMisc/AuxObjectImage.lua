MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxImage");

--[[
  @brief 显示基本道具图像
]]

local M = GlobalNS.Class(GlobalNS.AuxImage);
M.clsName = "AuxObjectImage";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mObjectId = 0;
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	M.super.dispose(self);
end

function M:setObjectBaseId(value)
	if(self.mObjectId ~= value) then
		self.mObjectId = value;
		local altasPath, imageName = GlobalNS.UtilLogic.getAtlasAndImageByObjid(self.mObjectId);
		self:setSpritePath(altasPath, imageName);
	end
end

return M;