MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

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

    self.TipBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.TipBtn:addEventHandle(self, self.onTipBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
	self.CloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			BG, 
			GlobalNS.SettingsPanelNS.SettingsPanelPath.CloseBtn)
		);

    self:setOptModel();
    self:setServerAddr();
    self:setBgMusic();
    self:setSwallowState();
end

function M:setBgMusic()
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    local Bg_Center = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Bg_Center");
    local Op_Bg = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Bg_Center, "Op_Bg");

    local Music_Switch = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Op_Bg, "Music_Switch");
    local MusicModel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Music_Switch, "MusicModel");
    self.BG_Music = GlobalNS.UtilApi.getComByPath(MusicModel, "BG_Music", "Toggle");

    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("MusicModel") then
        if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("MusicModel") == 1 then
            self.BG_Music.isOn = true;
        else
            self.BG_Music.isOn = false;
        end
    else
        self.BG_Music.isOn = true;
    end
end

function M:setOptModel()
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    local Bg_Center = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Bg_Center");
    local Op_Bg = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Bg_Center, "Op_Bg");

    local Opt_Switch = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Op_Bg, "Opt_Switch");
    local OpModel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Opt_Switch, "OptModel");
    self.JoyStickOp = GlobalNS.UtilApi.getComByPath(OpModel, "JoyStick", "Toggle");
    self.GravityOp = GlobalNS.UtilApi.getComByPath(OpModel, "Gravity", "Toggle");
    self.TipBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(OpModel, "Tip_BtnTouch"));

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
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    local Bg_Center = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Bg_Center");
    local Op_Bg = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Bg_Center, "Op_Bg");

    local Server_Switch = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Op_Bg, "Server_Switch");
    local ServerAddr = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Server_Switch, "ServerAddr");
    self.Server1 = GlobalNS.UtilApi.getComByPath(ServerAddr, "Server1", "Toggle");
    self.Server2 = GlobalNS.UtilApi.getComByPath(ServerAddr, "Server2", "Toggle");
    self.Server_input = GlobalNS.UtilApi.getComByPath(ServerAddr, "Server_input", "Toggle");

    self.ip = GlobalNS.UtilApi.getComByPath(ServerAddr, "ip", "InputField");
    self.port = GlobalNS.UtilApi.getComByPath(ServerAddr, "port", "InputField");
    self:setServerIP();

    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("ServerAddr") then
        if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("ServerAddr") == 1 then
            self.Server1.isOn = true;
            self.Server2.isOn = false;
            self.Server_input.isOn = false;
        elseif GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("ServerAddr") == 2 then
            self.Server1.isOn = false;
            self.Server2.isOn = true;
            self.Server_input.isOn = false;
        else
            self.Server1.isOn = false;
            self.Server2.isOn = false;
            self.Server_input.isOn = true;
        end
    else
        self.Server1.isOn = true;
        self.Server2.isOn = false;
        self.Server_input.isOn = false;
    end
end

function M:setServerIP()
    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("ip") then
        local ipStr = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString("ip");
        self.ip.text = ipStr;
    else
        self.ip.text = "192.168.93.187";
    end

    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("port") then
        local port = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("port");
        self.port.text = tostring(port);
    else
        self.port.text = "8002";
    end
end

function M:setSwallowState()
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");
    local Bg_Center = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Bg_Center");
    local Op_Bg = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Bg_Center, "Op_Bg");

    local Swallow_Switch = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Op_Bg, "Swallow_Switch");
    local SwallowModel = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Swallow_Switch, "SwallowModel");
    self.BtnState = GlobalNS.UtilApi.getComByPath(SwallowModel, "BtnState", "Toggle");

    if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:hasKey("SwallowModel") then
        if GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getInt("SwallowModel") == 1 then
            self.BtnState.isOn = true;
        else
            self.BtnState.isOn = false;
        end
    else
        self.BtnState.isOn = true;
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
    if self.BG_Music.isOn then
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("MusicModel", 1);
    else
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("MusicModel", 0);
    end

    if self.JoyStickOp.isOn then
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("OptionModel", 1);
    else
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("OptionModel", 2);
    end

    if self.BtnState.isOn then
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("SwallowModel", 1);
    else
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("SwallowModel", 0);
    end

    if self.Server1.isOn then
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("ServerAddr", 1);
    elseif self.Server2.isOn then
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("ServerAddr", 2);
    else
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("ServerAddr", 10086);
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setString("ip", self.ip.text);
        GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:setInt("port", tonumber(self.port.text));
    end
    GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:SetServerIP();

	self:exit();
end

function M:onTipBtnClk()
    GCtx.mGameData:ShowMessageBox("摇杆模式：使用左侧摇杆控制方向\n重力模式：使用重力控制方向，按\n                   下左侧界面时向前移动");
end

return M;