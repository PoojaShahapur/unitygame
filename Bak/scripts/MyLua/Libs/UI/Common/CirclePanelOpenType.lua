MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

local M = GlobalNS.StaticClass();
M.clsName = "CirclePanelOpenType";
GlobalNS[M.clsName] = M;

function M.init()
	M.eOpenFromPack = 0;
	M.eOpenFromShop = 1;
	M.eOpenFromSkin = 1;
end

M.init();

return M;