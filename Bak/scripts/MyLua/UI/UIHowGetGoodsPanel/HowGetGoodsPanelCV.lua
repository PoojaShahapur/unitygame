MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIHowGetGoodsPanel.HowGetGoodsPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "HowGetGoodsPanelPath";
GlobalNS.HowGetGoodsPanelNS[M.clsName] = M;

function M.init()
	M.OkBtn = "BG/Confirm_Btn";
	M.DetailDescText = "BG/Message_Text";
end

M.init();

return M;