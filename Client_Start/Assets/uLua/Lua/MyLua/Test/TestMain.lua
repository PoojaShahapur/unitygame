require "MyLua.Libs.Core.GlobalNS"
require "MyLua.Libs.Core.Class"
require "MyLua.Libs.Core.GObject"

require "MyLua.Test.TestProtoBuf.TestProtoBuf"

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestMain";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mTestProtoBuf = GlobalNS.new(GlobalNS.TestProtoBuf);
	
	self:run();
end

function M:dtor()
	
end

function M:dispose()
	
end

function M:run()
	self.mTestProtoBuf:run();
end

return M;