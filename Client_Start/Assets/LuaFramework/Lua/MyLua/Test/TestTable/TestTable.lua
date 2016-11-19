MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TestTable";
GlobalNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

function M:dispose()
	
end

function M:run()
	self:testTableLoad();
end

function M:testTableLoad()
	GCtx.mTableSys:getItem(GlobalNS.TableID.TABLE_OBJECT, 100);
end

return M;