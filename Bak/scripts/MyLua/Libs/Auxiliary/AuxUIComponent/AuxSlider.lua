MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxWindow");
MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxUITypeId");

local M = GlobalNS.Class(GlobalNS.AuxWindow);
M.clsName = "AuxSlider";
GlobalNS[M.clsName] = M;

function M:ctor(...)
	self.mSlider = nil;
	self.mSliderValue = 0;
	self.mIsSliderValueDirty = false;
end

function M:onSelfChanged()
	M.super.onSelfChanged(self);

	self.mSlider = GlobalNS.UtilApi.getSliderCompNoPath(self.mSelfGo);

	self:syncSliderValue();
end

function M:setSliderValue(value)
	if(value ~= self.mSliderValue) then
		self.mIsSliderValueDirty = true;
		self.mSliderValue = value;
	end

	self:syncSliderValue();
end

function M:syncSliderValue()
	if(nil ~= self.mSlider) then
		if(self.mIsSliderValueDirty) then
			self.mIsSliderValueDirty = false;
			
			if(self.mSlider.value ~= self.mSliderValue) then
				self.mSlider.value = self.mSliderValue;
			end
		else
			self.mSliderValue = self.mSlider.value;
		end
	end
end
    
function M:getSliderValue()
    return self.mSliderValue;
end

return M;