MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Test.CSSystemTest");
MLoader("MyLua.Test.GlobalEventCmdTest");

MLoader("MyLua.Test.TestCmdDisp.TestCmdDisp");
MLoader("MyLua.Test.TestProtoBuf.TestProtoBuf");
MLoader("MyLua.Test.TestUI.TestUI");
MLoader("MyLua.Test.TestTable.TestTable");
MLoader("MyLua.Test.TestStr.TestStr");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestMain";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mTestProtoBuf = GlobalNS.new(GlobalNS.TestProtoBuf);
	self.mTestCmdDisp = GlobalNS.new(GlobalNS.TestCmdDisp);
	self.mTestUI = GlobalNS.new(GlobalNS.TestUI);
	self.mTestTable = GlobalNS.new(GlobalNS.TestTable);
	self.mTestStr = GlobalNS.new(GlobalNS.TestStr);
	
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
	--self.mTestProtoBuf:run();
	--self.mTestCmdDisp:run();
	--self.mTestUI:run();
	--self.mTestTable:run();
	self.mTestStr:run();
end

return M;