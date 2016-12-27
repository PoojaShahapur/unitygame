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
    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mMainChildMassChangedDispatch:addEventHandle(nil, nil, self, self.refreshMass);
    
    self:refreshMass(); --加载完成主动刷新一次质量
end

function M:refreshMass()
    --获取Mass_Text的Text组件
    local mass = GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero().mPlayerSplitMerge:getAllChildMass();
    self.mMass = GlobalNS.UtilApi.getComByPath(self.mGuiWin, "Mass_Text", "Text");
    self.mMass.text = "重量：" .. GlobalNS.UtilMath.getShowMass(GlobalNS.UtilMath.getRadiusByMass(mass));
end

function M:refreshLeftTime(leftseconds)
    --获取Time_Text的Text组件
    if self.mGuiWin == nil then
        return;
    end
    self.mMass = GlobalNS.UtilApi.getComByPath(self.mGuiWin, "Time_Text", "Text");
    self.mMass.text = "时间：" .. self:getTimeText(leftseconds);
end

function M:getTimeText(leftseconds)
    local min = leftseconds / 60;
    min = math.floor(min);
    local second = leftseconds % 60;

    local timestr = "";
    if min > 9 then
        timestr = min .. ":";
    else
        timestr = "0" .. min .. ":";
    end

    if second > 9 then
        timestr = timestr .. second;
    else
        timestr = timestr .. "0" .. second;
    end

    return timestr;
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
    GlobalNS.CSSystem.Ctx.mInstance.mGlobalDelegate.mMainChildMassChangedDispatch:removeEventHandle(nil, nil, self, self.refreshMass);
end

return M;