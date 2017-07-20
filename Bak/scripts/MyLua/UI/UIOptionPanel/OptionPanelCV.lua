MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.StaticClass");

MLoader("MyLua.UI.UIOptionPanel.OptionPanelNS");

--常量区
local M = GlobalNS.StaticClass();
local this = M;
M.clsName = "OptionPanelPath";
GlobalNS.OptionPanelNS[M.clsName] = M;

function M.init()
	M.BtnSplit = "Split_BtnTouch";
    M.BtnSwallow = "Swallow_BtnTouch";
	
	M.BtnSwallowMask = "Mask";
	M.BtnSplitMask = "Mask2";
end

M.init();

return M;