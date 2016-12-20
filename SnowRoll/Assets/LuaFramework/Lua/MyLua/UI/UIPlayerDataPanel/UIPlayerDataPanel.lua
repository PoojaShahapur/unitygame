MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIPlayerDataPanel.PlayerDataPanelNS");
MLoader("MyLua.UI.UIPlayerDataPanel.PlayerDataPanelData");
MLoader("MyLua.UI.UIPlayerDataPanel.PlayerDataPanelCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIPlayerDataPanel";
GlobalNS.PlayerDataPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormID.eUIPlayerDataPanel;
	self.mData = GlobalNS.new(GlobalNS.PlayerDataPanelNS.PlayerDataPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
end

function M:onReady()
    M.super.onReady(self);
    
    --self:refreshMassAndTime();    
end

function M:refreshMassAndTime()
    --获取Mass_Text的Text组件
    self.hero = GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero();
    local mass = self.hero:transform().localScale.x;
    self.mMass = GlobalNS.UtilApi.getComByPath(self.mGuiWin, "Mass_Text", "Text");
    self.mMass.text = "重量：" .. mass .. "t";

    --获取Time_Text的Text组件
    self.mMass = GlobalNS.UtilApi.getComByPath(self.mGuiWin, "Time_Text", "Text");
    self.mMass.text = "时间：" .. "2:08";
end

function M:onShow()
    M.super.onShow(self);

end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
end

return M;