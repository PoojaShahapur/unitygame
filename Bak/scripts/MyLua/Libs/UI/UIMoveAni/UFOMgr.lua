--region *.lua
--Date
--此文件由[BabeLua]插件自动生成
MLoader("MyLua.Libs.UI.UIMoveAni.FlyUFO");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "UFOMgr";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.ufolist = GlobalNS.new(GlobalNS.MList);
end

function M:dtor()
	self:dispose();
end

function M:dispose()
    local index = 0;
	local listLen = self.ufolist:Count();
	
    while(index < listLen) do
        local ufo = self.ufolist:get(index);
        ufo:dispose();
		
		index = index + 1;
    end

    self.ufolist:Clear();
end

function M:addUFO(ufo_Go)
	local ufo = nil;
	
	ufo = GlobalNS.new(GlobalNS.FlyUFO);
	
	ufo:init();
    ufo:setGo(ufo_Go);
	
    self.ufolist:add(ufo);
end

return M;

--endregion
