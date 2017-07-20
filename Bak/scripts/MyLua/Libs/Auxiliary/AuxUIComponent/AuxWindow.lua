MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxComponent");

local M = GlobalNS.Class(GlobalNS.AuxComponent);
M.clsName = "AuxWindow";
GlobalNS[M.clsName] = M;

function M:ctor()
    
end

function M:dtor()
    
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	M.super.dispose(self);
end

return M;