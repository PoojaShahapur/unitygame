MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxWindow");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxUITypeId");

local M = GlobalNS.Class(GlobalNS.AuxWindow);
M.clsName = "AuxInputField";
GlobalNS[M.clsName] = M;

function M:ctor(...)
	self.mTextStr = "";
	self.mIsTextStrDirty = false;
    local pntNode, path, styleId = ...;
	
    if(styleId == nil) then
        styleId = GlobalNS.BtnStyleID.eBSID_None;
    end
	
    self.mSelfGo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(pntNode, path);
    self.mInputField = GlobalNS.UtilApi.getComByPath(pntNode, path, GlobalNS.AuxUITypeId.InputField);
end

function M:onSelfChanged()
	M.super.onSelfChanged(self);
	
	self:syncText();
end

function M:setText(value)
	if(self.mTextStr ~= value) then
		self.mIsTextStrDirty = true;
		self.mTextStr = value;
	end
	
	self:syncText();
end

function M:getText()
    --return self.mInputField.text;
	return self.mTextStr;
end

function M:syncText()
	if(nil ~= self.mInputField) then
		if(self.mIsTextStrDirty) then
			self.mIsTextStrDirty = false;
			
			if(self.mInputField.text ~= self.mTextStr) then
				self.mInputField.text = self.mTextStr;
			end
		else
			self.mTextStr = self.mInputField.text;
		end
	end
end

return M;