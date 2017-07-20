MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AreaWidget";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mGuiWin = nil;
	self.mAreaViewItemList = GlobalNS.new(GlobalNS.MList);
	self.mAreaViewItemList:setIsSpeedUpFind(false);
	self.mAreaViewItemList:setIsOpKeepSort(true);
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	self.mGuiWin = nil;
	
	M.super.dispose(self);
end

function M:setGuiWin(value)
	self.mGuiWin = value;
end

function M:addAreaViewItemByGoAndPath(parentGo, path, areaViewCls)
	local areaViewItem = nil;

	if(nil == areaViewCls) then
		areaViewItem = GlobalNS.new(GlobalNS.AreaViewItem);
	else
		areaViewItem = GlobalNS.new(areaViewCls);
	end
	
	areaViewItem:setContentByPath(parentGo, path);
	areaViewItem:setAreaWidget(self);
	areaViewItem:onInitView();
	self.mAreaViewItemList:add(areaViewItem);
	
	return areaViewItem;
end

function M:getAreaViewItemByTag(tag)
	local index = 0;
	local listLen = self.mAreaViewItemList:count();
	local areaViewItem = nil;
	local ret = nil;
	
	while(index < listLen) do
		areaViewItem = self.mAreaViewItemList:get(index);

		if(areaViewItem:isEqualTag(tag)) then
			ret = areaViewItem;
			break;
		end
	end
	
	return ret;
end

function M:removeAreaViewItemByTag(tag)
	local index = 0;
	local listLen = self.mAreaViewItemList:count();
	local areaViewItem = nil;
	
	while(index < listLen) do
		areaViewItem = self.mAreaViewItemList:get(index);

		if(areaViewItem:isEqualTag(tag)) then
			self.mAreaViewItemList:removeAt(index);
			break;
		end
	end
end

function M:getAndRemoveAreaViewItemByTag(tag)
	local index = 0;
	local listLen = self.mAreaViewItemList:count();
	local areaViewItem = nil;
	local ret = nil;
	
	while(index < listLen) do
		areaViewItem = self.mAreaViewItemList:get(index);

		if(areaViewItem:isEqualTag(tag)) then
			ret = areaViewItem;
			self.mAreaViewItemList:removeAt(index);
			break;
		end
	end
	
	return ret;
end

return M;