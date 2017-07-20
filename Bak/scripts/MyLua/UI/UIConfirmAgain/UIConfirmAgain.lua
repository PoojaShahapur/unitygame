MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIConfirmAgain.ConfirmAgainNS");
MLoader("MyLua.UI.UIConfirmAgain.ConfirmAgainData");
MLoader("MyLua.UI.UIConfirmAgain.ConfirmAgainCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIConfirmAgain";
GlobalNS.ConfirmAgainNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIConfirmAgain;
	self.mData = GlobalNS.new(GlobalNS.ConfirmAgainNS.ConfirmAgainData);
    self.text = "是否确定购买该皮肤？";
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
	
	self.mOkBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mOkBtn:setIsDestroySelf(false);
	self.mOkBtn:addEventHandle(self, self.onOkBtnClick);
	
	self.mCancelBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCancelBtn:setIsDestroySelf(false);
	self.mCancelBtn:addEventHandle(self, self.onCancelBtnClick);
	
	self.mDescText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mDescText:setIsDestroySelf(false);
	
	self.mOkEventDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
	self.mCalcelEventDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
end

function M:onReady()
    M.super.onReady(self);
	
	self.mOkBtn:setSelfGoByPath(self.mGuiWin, GlobalNS.ConfirmAgainNS.ConfirmAgainPath.OkBtn);
	self.mCancelBtn:setSelfGoByPath(self.mGuiWin, GlobalNS.ConfirmAgainNS.ConfirmAgainPath.CancelBtn);
	self.mDescText:setSelfGoByPath(self.mGuiWin, GlobalNS.ConfirmAgainNS.ConfirmAgainPath.DescText);

    self.mDescText:setText(self.text);
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
	if(nil ~= self.mOkBtn) then
		self.mOkBtn:dispose();
		self.mOkBtn = nil;
	end
	if(nil ~= self.mCancelBtn) then
		self.mCancelBtn:dispose();
		self.mCancelBtn = nil;
	end
	if(nil ~= self.mDescText) then
		self.mDescText:dispose();
		self.mDescText = nil;
	end
	
	self.mOkEventDispatch:clearEventHandle();
	self.mOkEventDispatch = nil;
	self.mCalcelEventDispatch:clearEventHandle();
	self.mCalcelEventDispatch = nil;
	
    M.super.onExit(self);
end

function M:hideCancelBtn()
	self.mCancelBtn:hide();
end

function M:setDesc(descStr)
    self.text = descStr;
    self.mDescText:setText(self.text);
end

function M:addOkEventHandle(pThis, handle)
	self.mOkEventDispatch:addEventHandle(pThis, handle);
end

function M:removeOkEventHandle()
	self.mOkEventDispatch:clearEventHandle();
end

function M:addCancelEventHandle(pThis, handle)
	self.mCalcelEventDispatch:addEventHandle(pThis, handle);
end

function M:removeCancelEventHandle()
	self.mCalcelEventDispatch:clearEventHandle();
end

function M:onOkBtnClick()
	self.mOkEventDispatch:dispatchEvent(self);
	self:exit();
end

function M:onCancelBtnClick()
	self.mCalcelEventDispatch:dispatchEvent(self);
	self:exit();
end

return M;