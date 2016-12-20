MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Tools.UtilStr");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestStr";
GlobalNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
end

function M:run()
	self:testReplace();
end

function M:testReplace()
	local srcStr = "asdfhhhh";
	local destStr = GlobalNS.UtilStr.Replace(srcStr, "asdf", "qqqq");
	local aaa = "end";
end

return M;