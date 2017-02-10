MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIAccountPanel.AccountPanelNS");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelData");
MLoader("MyLua.UI.UIAccountPanel.AccountPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIAccountPanel";
GlobalNS.AccountPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIAccountPanel;
	self.mData = GlobalNS.new(GlobalNS.AccountPanelNS.AccountPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
    self.mAvatarBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mAvatarBtn:addEventHandle(self, self.onAvatarBtnClk);

	self.mCloseBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCloseBtn:addEventHandle(self, self.onBtnClk);
end

function M:onReady()
    M.super.onReady(self);
    local BG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BG");    
	self.mCloseBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			BG, 
			GlobalNS.AccountPanelNS.AccountPanelPath.BtnClose)
		);

    local Avatar = GlobalNS.UtilApi.TransFindChildByPObjAndPath(BG, "Avatar");
    self.mAvatarBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "Avatar_BtnTouch"));
    local Info = GlobalNS.UtilApi.TransFindChildByPObjAndPath(Avatar, "Info");

    local Name = GlobalNS.UtilApi.getComByPath(Info, "Name", "Text");
    local username = GlobalNS.CSSystem.Ctx.mInstance.mSystemSetting:getString(SDK.Lib.SystemSetting.USERNAME);
    if username == nil then
        username = "游客";
    end
    Name.text = username;
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

function M:onBtnClk()
	self:exit();
end

function M:onAvatarBtnClk()
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIAccountAvatarPanel);
end

function M:resetAvatar(index)
	GlobalNS.UtilApi.setImageSprite(self.mAvatarBtn:getSelfGo(), "DefaultSkin/Avatar/"..index..".png");
end

return M;