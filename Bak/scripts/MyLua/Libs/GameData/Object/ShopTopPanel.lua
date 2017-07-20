MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
@brief ShopTopPanel
]]

local M = GlobalNS.Class(GlobalNS.ObjectPanelBase);
M.clsName = "ShopTopPanel";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mPanelArray = GlobalNS.new(GlobalNS.MList);
	self.mShopTopType = 0;
	
	self.mRedProperty = GlobalNS.new(GlobalNS.AuxBoolProperty);
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
end

function M:getRedProperty()
	return self.mRedProperty;
end

function M:setShopTopType(value)
	self.mShopTopType = value;

	if (GlobalNS.ShopTopType.ePiFu == self.mShopTopType) then
		local index = 0;
		local shopPanel = nil;

		while (index < GlobalNS.ObjectPanelType.eCount) do
			shopPanel = GlobalNS.new(GlobalNS.ShopPanel);
			self.mPanelArray:add(shopPanel);
			shopPanel:setPanelType(index + 1);
			shopPanel:setTopType(self.mPanelType);

			index = index + 1;
		end
	end
end

function M:addItem(item)
	local panelType = item:getSubTypeZeroIndex();
	self.mPanelArray:get(panelType):addObjectById(0, item:getBaseId(), item);
end

function M:getShopPanelByIndex(index)
	return self.mPanelArray:get(index);
end

return M;