MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIPerformancePanel.PerformancePanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "PerformancePanelPath";
GlobalNS.PerformancePanelNS[M.clsName] = M;

function M.init()
	M.CloseBgPanel = "CloseBgPanel";
	
	M.OnePanelImage_0 = "BG/Item_Panel/OnePanel/Image/Image";
	M.OnePanelText_0 = "BG/Item_Panel/OnePanel/Image/Text";

	M.TwoPanelImage_0 = "BG/Item_Panel/TwoPanel/Image/Image";
	M.TwoPanelText_0 = "BG/Item_Panel/TwoPanel/Image/Text";
	M.TwoPanelImage_1 = "BG/Item_Panel/TwoPanel/Image (1)/Image";
	M.TwoPanelText_1 = "BG/Item_Panel/TwoPanel/Image (1)/Text";
	
	M.ThreePanelImage_0 = "BG/Item_Panel/ThreePanel/Image/Image";
	M.ThreePanelText_0 = "BG/Item_Panel/ThreePanel/Image/Text";
	M.ThreePanelImage_1 = "BG/Item_Panel/ThreePanel/Image (1)/Image";
	M.ThreePanelText_1 = "BG/Item_Panel/ThreePanel/Image (1)/Text";
	M.ThreePanelImage_2 = "BG/Item_Panel/ThreePanel/Image (2)/Image";
	M.ThreePanelText_2 = "BG/Item_Panel/ThreePanel/Image (2)/Text";
	
	M.FourPanelImage_0 = "BG/Item_Panel/FourPanel/Image/Image";
	M.FourPanelText_0 = "BG/Item_Panel/FourPanel/Image/Text";
	M.FourPanelImage_1 = "BG/Item_Panel/FourPanel/Image (1)/Image";
	M.FourPanelText_1 = "BG/Item_Panel/FourPanel/Image (1)/Text";
	M.FourPanelImage_2 = "BG/Item_Panel/FourPanel/Image (2)/Image";
	M.FourPanelText_2 = "BG/Item_Panel/FourPanel/Image (2)/Text";
	M.FourPanelImage_3 = "BG/Item_Panel/FourPanel/Image (3)/Image";
	M.FourPanelText_3 = "BG/Item_Panel/FourPanel/Image (3)/Text";
	
	M.FivePanelImage_0 = "BG/Item_Panel/FivePanel/Image/Image";
	M.FivePanelText_0 = "BG/Item_Panel/FivePanel/Image/Text";
	M.FivePanelImage_1 = "BG/Item_Panel/FivePanel/Image (1)/Image";
	M.FivePanelText_1 = "BG/Item_Panel/FivePanel/Image (1)/Text";
	M.FivePanelImage_2 = "BG/Item_Panel/FivePanel/Image (2)/Image";
	M.FivePanelText_2 = "BG/Item_Panel/FivePanel/Image (2)/Text";
	M.FivePanelImage_3 = "BG/Item_Panel/FivePanel/Image (3)/Image";
	M.FivePanelText_3 = "BG/Item_Panel/FivePanel/Image (3)/Text";
	M.FivePanelImage_4 = "BG/Item_Panel/FivePanel/Image (4)/Image";
	M.FivePanelText_4 = "BG/Item_Panel/FivePanel/Image (4)/Text";
end

M.init();

return M;