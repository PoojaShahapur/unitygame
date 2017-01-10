MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UISettingsPanel.SettingsPanelNS");
MLoader("MyLua.UI.UISettingsPanel.SettingsPanelData");
MLoader("MyLua.UI.UISettingsPanel.SettingsPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UISettingsPanel";
GlobalNS.SettingsPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUISettingsPanel;
	self.mData = GlobalNS.new(GlobalNS.SettingsPanelNS.SettingsPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.CloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.CloseBtn:addEventHandle(self, self.onCloseBtnClk);
end

function M:onReady()
    M.super.onReady(self);
	self.CloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.SettingsPanelNS.SettingsPanelPath.CloseBtn)
		);

    self:setOptModel();
    self:setServerAddr();
end

function M:setOptModel()
    local OpModel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "OptModel");
    self.JoyStickOp = GlobalNS.UtilApi.getComByPath(OpModel, "JoyStick", "Toggle");
    self.GravityOp = GlobalNS.UtilApi.getComByPath(OpModel, "Gravity", "Toggle");

    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("OptionModel") then
        if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("OptionModel") == 1 then
            self.JoyStickOp.isOn = true;
            self.GravityOp.isOn = false;
        else
            self.JoyStickOp.isOn = false;
            self.GravityOp.isOn = true;
        end
    else
        self.JoyStickOp.isOn = true;
        self.GravityOp.isOn = false;
    end
end

function M:setServerAddr()
    local ServerAddr = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "ServerAddr");
    self.Server1 = GlobalNS.UtilApi.getComByPath(ServerAddr, "Server1", "Toggle");
    self.Server2 = GlobalNS.UtilApi.getComByPath(ServerAddr, "Server2", "Toggle");

    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("ServerAddr") then
        if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("ServerAddr") == 1 then
            self.Server1.isOn = true;
            self.Server2.isOn = false;
        else
            self.Server1.isOn = false;
            self.Server2.isOn = true;
        end
    else
        self.Server1.isOn = true;
        self.Server2.isOn = false;
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
end

function M:onCloseBtnClk()
    if self.JoyStickOp.isOn then
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("OptionModel", 1);
    else
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("OptionModel", 2);
    end

    if self.Server1.isOn then
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("ServerAddr", 1);
    else
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("ServerAddr", 2);
    end
    GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:SetServerIP();

	self:exit();
end

return M;