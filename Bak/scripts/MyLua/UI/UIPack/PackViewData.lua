MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIPack.PackNS");

local M = GlobalNS.Class(GlobalNS.GObject)
M.clsName = "PackViewData"
GlobalNS.PackNS[M.clsName] = M

function M:ctor()
	
end

function M:dtor()
	
end

return M;