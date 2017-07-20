MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIAccountPanel.AccountPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "AccountPanelPath";
GlobalNS.AccountPanelNS[M.clsName] = M;

function M.init()
	M.SkinContent = "BG/GameShow/Skin_Panel/ScrollViewOut/Viewport/Content/Skin_Show/Content";
	M.BulletContent = "BG/GameShow/Skin_Panel/ScrollViewOut/Viewport/Content/Bullet_Show/Content";
end

M.init();

return M;