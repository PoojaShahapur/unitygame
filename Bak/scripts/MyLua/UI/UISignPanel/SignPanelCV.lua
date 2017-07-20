MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UISignPanel.SignPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "SignPanelPath";
GlobalNS.SignPanelNS[M.clsName] = M;

function M.init()
	M.Content = "DataPanel/MiddlePanel/DayAward/ScrollRect/Viewport/Content";
	M.CloseBtn = "TitlePanel/Close_BtnTouch";
	M.SignBtn = "DataPanel/BottomPanel/Sign_BtnTouch";
	M.DateText = "DataPanel/MiddlePanel/Date/Text";
	M.BeforeBtn = "DataPanel/MiddlePanel/Date/Before_BtnTouch";
	M.NextBtn = "DataPanel/MiddlePanel/Date/Next_BtnTouch";
	M.Award3Image = "DataPanel/MiddlePanel/OtherAward/Award3";
	M.Award5Image = "DataPanel/MiddlePanel/OtherAward/Award5";
	M.Award7Image = "DataPanel/MiddlePanel/OtherAward/Award7";
	M.N3DaysBtn = "DataPanel/MiddlePanel/OtherAward/Award3/3Days_BtnTouch";
	M.N5DaysBtn = "DataPanel/MiddlePanel/OtherAward/Award5/5Days_BtnTouch";
	M.N7DaysBtn = "DataPanel/MiddlePanel/OtherAward/Award7/7Days_BtnTouch";
	M.N3DaysText = "DataPanel/MiddlePanel/OtherAward/Award3/3Days_BtnTouch/Text";
	M.N5DaysText = "DataPanel/MiddlePanel/OtherAward/Award5/5Days_BtnTouch/Text";
	M.N7DaysText = "DataPanel/MiddlePanel/OtherAward/Award7/7Days_BtnTouch/Text";
	M.TipsText = "DataPanel/MiddlePanel/DayAward/Tip";
	
	
	M.DayAwardImage = "AwardImage/Award";
	M.DayAwardText = "AwardImage/Image/Name";
	M.DayAwardCloseBtn = "Close_BtnTouch";
	
	
	M.OtherAwardCloseBtn = "Close_BtnTouch";
	M.OtherAwardGetBtn = "Get_BtnTouch";
	M.OtherAwardOneImage = "Panel/Award";
	M.OtherAwardOneText = "Panel/Award/Name";
	M.OtherAwardTwoImage = "Panel/Award (1)";
	M.OtherAwardTwoText = "Panel/Award (1)/Name";
	M.OtherAwardTitleText = "Title";
end

M.init();

return M;