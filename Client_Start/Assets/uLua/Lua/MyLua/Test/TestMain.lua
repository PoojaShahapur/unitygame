require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

require "MyLua.Test.CSSystemTest"
require "MyLua.Test.GlobalEventCmdTest"

require "MyLua.Test.TestCmdDisp.TestCmdDisp"
require "MyLua.Test.TestProtoBuf.TestProtoBuf"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestMain";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mTestProtoBuf = GlobalNS.new(GlobalNS.TestProtoBuf);
	self.mTestCmdDisp = GlobalNS.new(GlobalNS.TestCmdDisp);
	
	self:init();
end

function M:init()
	GlobalNS.CSSystemTest.init();
end

function M:dtor()
	
end

function M:dispose()
	
end

function M:run()
	self.mTestProtoBuf:run();
	self.mTestCmdDisp:run();
end

return M;