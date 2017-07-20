MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIConfirmAgain.ConfirmAgainNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ConfirmAgainPath";
GlobalNS.ConfirmAgainNS[M.clsName] = M;

function M.init()
	M.OkBtn = "BG/Confirm_Btn";
	M.CancelBtn = "BG/Cancel_Btn";
	M.DescText = "BG/Message_Text";
end

M.init();

return M;