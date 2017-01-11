MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIStartGame.StartGameNS");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "StartGameData";
GlobalNS.StartGameNS[M.clsName] = M;

function M:ctor()
	
end

function M:dtor()
	
end

return M;