MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIConsoleDlg.ConsoleDlgNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ConsoleDlgPath";
GlobalNS.ConsoleDlgNS[M.clsName] = M;

function M.init()
	M.BtnSend = "Send_BtnTouch";
    M.BtnExit = "Exit_BtnTouch";
end

M.init();

return M;