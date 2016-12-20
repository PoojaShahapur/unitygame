MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIStartGame.StartGameNS");

local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "StartGamePath";
GlobalNS.StartGameNS[M.clsName] = M;

function M.init()
	M.BtnStartGame = "StartGame_BtnTouch";
end

M.init();

return M;