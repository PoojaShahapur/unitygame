MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxWindow");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxUITypeId");

local M = GlobalNS.Class(GlobalNS.AuxWindow);
M.clsName = "AuxButton";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self:AuxButton_1(...);
	
	self.mTextPath = "Text";
    self.mTextStr = "";
	self.mTextStrDirty = false;
	
    self.mImage = GlobalNS.new(GlobalNS.AuxImage);
    self.param1 = nil;
    self.param2 = nil;
	
	self.mInteractable = true;		-- 是否可交互
	self.mIsInteractableDirty = false;
end

function M:dtor()
	
end

function M:dispose()
	if (self.mClickEventDispatch ~= nil) then
        GlobalNS.UtilApi.removeEventHandleSelf(self.mSelfGo, self, self.onBtnClk);
		
		self.mClickEventDispatch:clearEventHandle();
		self.mClickEventDispatch = nil;
    end
	if (self.mDownEventDispatch ~= nil) then
        GlobalNS.UtilApi.removeButtonDownEventHandle(self.mSelfGo, self, self.OnPointerDown);
		
		self.mDownEventDispatch:clearEventHandle();
		self.mDownEventDispatch = nil;
    end
	if (self.mUpEventDispatch ~= nil) then
        GlobalNS.UtilApi.removeButtonUpEventHandle(self.mSelfGo, self, self.OnPointerUp);
		
		self.mUpEventDispatch:clearEventHandle();
		self.mUpEventDispatch = nil;
    end
	if (self.mExitEventDispatch ~= nil) then
        GlobalNS.UtilApi.removeButtonExitEventHandle(self.mSelfGo, self, self.OnPointerExit);
		
		self.mExitEventDispatch:clearEventHandle();
		self.mExitEventDispatch = nil;
    end
	
    if self.mImage ~= nil then
        self.mImage:dispose();
    end

    M.super.dispose(self);
end

function M:AuxButton_1(...)
    local pntNode, path, styleId = ...;
    if(path == nil) then
        path = '';
    end
    if(styleId == nil) then
        styleId = GlobalNS.BtnStyleID.eBSID_None;
    end
    
    self.mClickEventDispatch = GlobalNS.new(GlobalNS.EventDispatch);
	self.mDownEventDispatch = GlobalNS.new(GlobalNS.EventDispatch);
	self.mUpEventDispatch = GlobalNS.new(GlobalNS.EventDispatch);
	self.mExitEventDispatch = GlobalNS.new(GlobalNS.EventDispatch);
	
    if (pntNode ~= nil) then
        self.mSelfGo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(pntNode, path);
        self:updateBtnCom(nil);
    end
end

function M:onSelfChanged()
	M.super.onSelfChanged(self);
	
	self:updateBtnCom(nil);
	
	self:syncInteractable();
end

function M:updateBtnCom(dispObj)
    self.mBtn = GlobalNS.UtilApi.getComFromSelf(self.mSelfGo, GlobalNS.AuxUITypeId.Button);
    --GlobalNS.UtilApi.addEventHandle(self.mBtn, self, self.onBtnClk);
	GlobalNS.UtilApi.addEventHandleSelf(self.mSelfGo, self, self.onBtnClk);
	GlobalNS.UtilApi.addButtonDownEventHandle(self.mSelfGo, self, self.OnPointerDown);
	GlobalNS.UtilApi.addButtonUpEventHandle(self.mSelfGo, self, self.OnPointerUp);
	GlobalNS.UtilApi.addButtonExitEventHandle(self.mSelfGo, self, self.OnPointerExit);

    self:synText();
end

function M:setInteractable(value)
	if(self.mInteractable ~= value) then
		self.mIsInteractableDirty = true;
		self.mInteractable = value;
	end
	
	self:syncInteractable();
end

function M:getInteractable()
	return self.mInteractable;
end

function M:syncInteractable()
	if(nil ~= self.mBtn) then
		if(self.mIsInteractableDirty) then
			self.mIsInteractableDirty = false;
			
			if(self.mBtn.interactable ~= self.mInteractable) then
				self.mBtn.interactable = self.mInteractable;
			end
		else
			self.mInteractable = self.mBtn.interactable;
		end
	end
end

function M:enable()
	--[[
	if(nil ~= self.mBtn and self.mBtn.interactable ~= true) then
		self.mBtn.interactable = true;
	end
	]]
	
	self:setInteractable(true);
end

function M:disable()
	--[[
	if(nil ~= self.mBtn and self.mBtn.interactable ~= false) then
		self.mBtn.interactable = false;
	end
	]]
	
	self:setInteractable(false);
end

function M:toggleEnable(value)
	if(value) then
		self:enable();
	else
		self:disable();
	end
end

-- 点击回调
function M:onBtnClk()
    self.mClickEventDispatch:dispatchEvent(self);

    GlobalNS.CSSystem.Ctx.mInstance.mSoundMgr:play(0, "Sound/Music/click.wav", 0.6, 2, false, false);
end

function M:OnPointerDown(dispObj)
	self.mDownEventDispatch:dispatchEvent(self);
end

function M:OnPointerUp(dispObj)
	self.mUpEventDispatch:dispatchEvent(self);
end

function M:OnPointerExit(dispObj)
	self.mExitEventDispatch:dispatchEvent(self);
end

function M:addEventHandle(pThis, btnClk)
    self.mClickEventDispatch:addEventHandle(pThis, btnClk);
end

function M:removeEventHandle(pThis, btnClk)
    self.mClickEventDispatch:removeEventHandle(pThis, btnClk);
end

function M:addDownEventHandle(pThis, btnClk)
    self.mDownEventDispatch:addEventHandle(pThis, btnClk);
end

function M:removeDownEventHandle(pThis, btnClk)
    self.mDownEventDispatch:removeEventHandle(pThis, btnClk);
end

function M:addUpEventHandle(pThis, btnClk)
    self.mUpEventDispatch:addEventHandle(pThis, btnClk);
end

function M:removeUpEventHandle(pThis, btnClk)
    self.mUpEventDispatch:removeEventHandle(pThis, btnClk);
end

function M:addExitEventHandle(pThis, btnClk)
    self.mExitEventDispatch:addEventHandle(pThis, btnClk);
end

function M:removeExitEventHandle(pThis, btnClk)
    self.mExitEventDispatch:removeEventHandle(pThis, btnClk);
end

function M:syncUpdateCom()

end

function M:setText(text)
	if(self.mTextStr ~= text) then
		self.mTextStrDirty = true;
		self.mTextStr = text;
	end

	self:synText();
end

function M:synText()
	if(self.mBtn ~= nil) then
		local btnText = GlobalNS.UtilApi.getComByPath(self.mSelfGo, self.mTextPath, "Text");
		
		if(self.mTextStrDirty) then
			self.mTextStrDirty = false;
			
			if(nil ~= btnText) then
				btnText.text = self.mTextStr;
			end
		else
			if(nil ~= btnText) then
				self.mTextStr = btnText.text;
			end
		end
	end
end

function M:clearEventHandle()
	if (self.mClickEventDispatch ~= nil) then
		self.mClickEventDispatch:clearEventHandle();
    end
	if (self.mDownEventDispatch ~= nil) then
		self.mDownEventDispatch:clearEventHandle();
    end
	if (self.mUpEventDispatch ~= nil) then
		self.mUpEventDispatch:clearEventHandle();
    end
	if (self.mExitEventDispatch ~= nil) then
		self.mExitEventDispatch:clearEventHandle();
    end
end

return M;