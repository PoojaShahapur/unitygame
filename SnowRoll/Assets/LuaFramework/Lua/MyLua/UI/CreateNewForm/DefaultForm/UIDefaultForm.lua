MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIDefaultForm.DefaultFormNS");
MLoader("MyLua.UI.UIDefaultForm.DefaultFormData");
MLoader("MyLua.UI.UIDefaultForm.DefaultFormCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIDefaultForm";
GlobalNS.DefaultFormNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIDefaultForm;
	self.mData = GlobalNS.new(GlobalNS.DefaultFormNS.DefaultFormData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	--self.mDefaultBtn = GlobalNS.new(GlobalNS.AuxButton);
	--self.mDefaultBtn:addEventHandle(self, self.onBtnClk);
end

function M:onReady()
    M.super.onReady(self);
	--[[self.mDefaultBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.DefaultFormNS.DefaultFormPath.BtnDefault)
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