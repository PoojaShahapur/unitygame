MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxImage");

--[[
  @brief 显示使用图集中一个图像
]]

local M = GlobalNS.Class(GlobalNS.AuxImage);
M.clsName = "AuxSimpleAtlasImage";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mAtlasAndImageId = 0;
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	M.super.dispose(self);
end

function M:setSimpleImageId(value)
	if(self.mAtlasAndImageId ~= value) then
		self.mAtlasAndImageId = value;
		
		local altasPath = "";
		local imageName = "";
		local tableItem = GCtx.mTableSys:getItem(GlobalNS.TableId.TABLE_ATLAS_AND_IMAGE, self.mAtlasAndImageId);
		
		if(nil ~= tableItem) then
			altasPath = tableItem.AtlasPath;
			imageName = tableItem.ImageName;
		else
			altasPath = "Atlas/Itemsicon/0000Plane/0001.asset";
			imageName = "0001";
		end

		self:setSpritePath(altasPath, imageName);
	end
end

return M;