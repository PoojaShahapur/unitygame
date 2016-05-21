require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.StaticClass"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.StaticClass();
M.clsName = "UILayerID";
GlobalNS[M.clsName] = M;

function M.ctor()
	M.eUIBtmLayer = 0;
	M.eUIFirstLayer = 1;
	M.eUISecondLayer = 2;
	M.eUIThirdLayer = 3;
	M.eUIForthLayer = 4;
	M.eUITopLayer = 5;
end

M.ctor();

------------------------------------
M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "UILayer";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mName = "";
	self.mGo = nil;
end

function M:setGoName(name)
	self.mName = name;
end

function M:init()
	self.mGo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(GlobalNS.NoDestroyGo.mNoDestroyGo, self.mName);
end

function M:getLayerGo()
	return self.mGo;
end

return M;