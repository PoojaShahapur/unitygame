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
    self.mTimer = GlobalNS.new(GlobalNS.TimerItemBase);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
    self.mTimer:setTotalTime(100);
    self.mTimer:setFuncObject(self, self.refreshMassAndTime);
    self.mTimer:Start();
end

function M:onReady()
    M.super.onReady(self);
    
    --self:refreshMassAndTime();    
end

function M:refreshMassAndTime()
    --获取Mass_Text的Text组件
    self.hero = GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero();
    if self.hero ~= nil and self.mGuiWin ~= nil then
        local mass = GlobalNS.UtilMath.getShowMass(self.hero:getScale().x);
        self.mMass = GlobalNS.UtilApi.getComByPath(self.mGuiWin, "Mass_Text", "Text");
        self.mMass.text = "重量：" .. mass;
    
        --获取Time_Text的Text组件
        self.mMass = GlobalNS.UtilApi.getComByPath(self.mGuiWin, "Time_Text", "Text");
        self.mMass.text = "时间：" .. "2:08";
    end
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
    self.mTimer:Stop();
end

return M;