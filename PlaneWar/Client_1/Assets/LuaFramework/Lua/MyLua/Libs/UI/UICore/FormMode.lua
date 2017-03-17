MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "PreFormModeWhenOpen";
GlobalNS[M.clsName] = M;

function M.ctor()
	M.eNONE = 0;
	M.eHIDE = 1;
	M.eCLOSE = 2;
end

M.ctor();

-------------------------------------
M = GlobalNS.StaticClass();
M.clsName = "PreFormModeWhenClose";
GlobalNS[M.clsName] = M;

function M.ctor()
	M.eNONE = 0;
	M.eSHOW = 1;
end

M.ctor();

return M;