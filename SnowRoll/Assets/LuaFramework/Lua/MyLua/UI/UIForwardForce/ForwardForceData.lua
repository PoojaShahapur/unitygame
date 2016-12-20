MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIForwardForce.ForwardForceNS");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ForwardForceData";
GlobalNS.ForwardForceNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

return M;