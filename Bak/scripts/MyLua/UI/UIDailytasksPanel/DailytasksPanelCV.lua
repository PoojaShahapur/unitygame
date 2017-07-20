MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIDailytasksPanel.DailytasksPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "DailytasksPanelPath";
GlobalNS.DailytasksPanelNS[M.clsName] = M;

function M.init()
	M.CloseBtn = "BG/Close_BtnTouch";
	M.TplPrefab = "BG/MainPanel/Task_Panel/ScrollRect/Viewport/Content/TaskItem";
	M.Content = "BG/MainPanel/Task_Panel/ScrollRect/Viewport/Content";
	M.Slider = "BG/MainPanel/Gift_Panel/Progress";

	M.ActivityRewardImage_0 = "BG/MainPanel/Gift_Panel/gift1";
	M.ActivityValueText_0 = "BG/MainPanel/Gift_Panel/gift1/Text";
	M.ActivityStarImage_0 = "BG/MainPanel/Gift_Panel/gift1/Image";

	M.ActivityRewardImage_1 = "BG/MainPanel/Gift_Panel/gift2";
	M.ActivityValueText_1 = "BG/MainPanel/Gift_Panel/gift2/Text";
	M.ActivityStarImage_1 = "BG/MainPanel/Gift_Panel/gift2/Image";

	M.ActivityRewardImage_2 = "BG/MainPanel/Gift_Panel/gift3";
	M.ActivityValueText_2 = "BG/MainPanel/Gift_Panel/gift3/Text";
	M.ActivityStarImage_2 = "BG/MainPanel/Gift_Panel/gift3/Image";

	M.ActivityRewardImage_3 = "BG/MainPanel/Gift_Panel/gift4";
	M.ActivityValueText_3 = "BG/MainPanel/Gift_Panel/gift4/Text";
	M.ActivityStarImage_3 = "BG/MainPanel/Gift_Panel/gift4/Image";

	M.ActivityRewardImage_4 = "BG/MainPanel/Gift_Panel/gift5";
	M.ActivityValueText_4 = "BG/MainPanel/Gift_Panel/gift5/Text";
	M.ActivityStarImage_4 = "BG/MainPanel/Gift_Panel/gift5/Image";
end

M.init();

return M;