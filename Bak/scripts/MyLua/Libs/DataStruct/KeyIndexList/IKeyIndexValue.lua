MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "IKeyIndexValue";
GlobalNS[M.clsName] = M;

function M.ctor()
	
end

function M.dispose()
	
end

return M;