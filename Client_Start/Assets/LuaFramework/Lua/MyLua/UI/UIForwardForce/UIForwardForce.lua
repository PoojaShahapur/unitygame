MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIForwardForce.ForwardForceNS");
MLoader("MyLua.UI.UIForwardForce.ForwardForceData");
MLoader("MyLua.UI.UIForwardForce.ForwardForceCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIForwardForce";
GlobalNS.ForwardForceNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormID.eUIForwardForce;
	self.mData = GlobalNS.new(GlobalNS.ForwardForceNS.ForwardForceData);
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
			GlobalNS.ForwardForceNS.ForwardForcePath.BtnRelive)
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