MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UITest.PackNS");
MLoader("MyLua.UI.UITest.PackData");
MLoader("MyLua.UI.UITest.PackCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIPack";
GlobalNS.TestNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormID.eUIPack;
	self.mData = GlobalNS.new(GlobalNS.PackNS.PackData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	self.mTestBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mTestBtn:addEventHandle(self, self.onBtnClk);
end

function M:onReady()
    M.super.onReady(self);
	self.mTestBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, GlobalNS.TestNS.TestPath.BtnTest));
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
	
end

return M;

