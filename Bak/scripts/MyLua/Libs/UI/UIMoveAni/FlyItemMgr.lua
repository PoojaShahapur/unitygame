--region *.lua
--Date
--此文件由[BabeLua]插件自动生成
MLoader("MyLua.Libs.UI.UIMoveAni.FlyItem");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "FlyItemMgr";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.flyitemlist = GlobalNS.new(GlobalNS.MList);
end

function M:dtor()
	self:dispose();
end

function M:dispose()
    local index = 0;
	local listLen = self.flyitemlist:Count();
	
    while(index < listLen) do
        local flyitem = self.flyitemlist:get(index);
        flyitem:dispose();
		
		index = index + 1;
    end

    self.flyitemlist:Clear();
end

function M:addFlyItem(index, flyitem_Go, destPosItem)
	local flyitem = nil;
	
	flyitem = GlobalNS.new(GlobalNS.FlyItem);
	
	flyitem:init();
    flyitem:setGo(flyitem_Go, destPosItemTran);
	
    self.flyitemlist:add(flyitem);
end

function M:flyItem(index)
	local flyitem = nil;
	
	flyitem = self.flyitemlist:get(index);
    flyitem:doFlyItem();
end

return M;

--endregion
