require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

require "MyLua.UI.UITest.TestNS"

local M = GlobalNS.Class(GlobalNS.GObject)
M.clsName = "TestData"
GlobalNS.TestNS[M.clsName] = M

function M:ctor()
	
end

function M:dtor()
	
end

return M;