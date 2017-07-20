MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIFriendSystemPanel.FriendSystemPanelNS");
MLoader("MyLua.UI.UIFriendSystemPanel.FriendSystemPanelData");
MLoader("MyLua.UI.UIFriendSystemPanel.FriendSystemPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIFriendSystemPanel";
GlobalNS.FriendSystemPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIFriendSystemPanel;
	self.mData = GlobalNS.new(GlobalNS.FriendSystemPanelNS.FriendSystemPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	--self.mReliveBtn = GlobalNS.new(GlobalNS.AuxButton);
	--self.mReliveBtn:addEventHandle(self, self.onBtnClk);
end

function M:onReady()
    M.super.onReady(self);
	--[[self.mReliveBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.FriendSystemPanelNS.FriendSystemPanelPath.BtnRelive)
		);
	--]]
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
	GCtx.mLogSys:log("Hello World", GlobalNS.LogTypeId.eLogCommon);
end

return M;