MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIShareMoney.ShareMoneyNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "ShareMoneyPath";
GlobalNS.ShareMoneyNS[M.clsName] = M;

function M.init()

end

M.init();

return M;