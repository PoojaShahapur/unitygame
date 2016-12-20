MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIDefaultForm.DefaultFormNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "DefaultFormPath";
GlobalNS.DefaultFormNS[M.clsName] = M;

function M.init()
	--M.BtnDefault = "Default_BtnTouch";
end

M.init();

return M;