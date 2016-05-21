require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.StaticClass"
require "MyLua.Libs.Core.GObject"
require "MyLua.Libs.UI.UICore.UILayer"

local M = GlobalNS.StaticClass();
M.clsName = "UICanvasID";
GlobalNS[M.clsName] = M;

function M.ctor()
	M.eUIFirstCanvas = 0;
	M.eUISecondCanvas = 1;
end

M.ctor();

-----------------------------
M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "UICanvas";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mName = "";
	self.mLayerList = GlobalNS.new(GlobalNS.MList);
end

function M:setGoName(name)
	self.mName = name;
end

function M:init()
	local layer = nil;
	local layerName = '';
	
	layer = GlobalNS.new(GlobalNS.UILayer);
	self.mLayerList:Add(layer);
	layerName = string.format("%s/%s", self.mName, GlobalNS.NoDestroyId.ND_CV_UIBtmLayer);
	layer:setGoName(layerName);
	layer:init();
	
	layer = GlobalNS.new(GlobalNS.UILayer);
	self.mLayerList:Add(layer);
	layerName = string.format("%s/%s", self.mName, GlobalNS.NoDestroyId.ND_CV_UIFirstLayer);
	layer:setGoName(layerName);
	layer:init();
	
	layer = GlobalNS.new(GlobalNS.UILayer);
	self.mLayerList:Add(layer);
	layerName = string.format("%s/%s", self.mName, GlobalNS.NoDestroyId.ND_CV_UISecondLayer);
	layer:setGoName(layerName);
	layer:init();
	
	layer = GlobalNS.new(GlobalNS.UILayer);
	self.mLayerList:Add(layer);
	layerName = string.format("%s/%s", self.mName, GlobalNS.NoDestroyId.ND_CV_UIThirdLayer);
	layer:setGoName(layerName);
	layer:init();
	
	layer = GlobalNS.new(GlobalNS.UILayer);
	self.mLayerList:Add(layer);
	layerName = string.format("%s/%s", self.mName, GlobalNS.NoDestroyId.ND_CV_UIForthLayer);
	layer:setGoName(layerName);
	layer:init();
	
	layer = GlobalNS.new(GlobalNS.UILayer);
	self.mLayerList:Add(layer);
	layerName = string.format("%s/%s", self.mName, GlobalNS.NoDestroyId.ND_CV_UITopLayer);
	layer:setGoName(layerName);
	layer:init();
end

return M;