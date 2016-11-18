require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestUI";
GlobalNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

function M:dispose()
	
end

function M:run()
	self:testForm();
end

function M:testForm()
	GCtx.mUIMgr:loadAndShow(GlobalNS.UIFormID.eUITest);
end

return M;