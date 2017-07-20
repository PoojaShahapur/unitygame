MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.AnimSys.NumNodeAnim.NumNodeAniBase");

--[[
 @brief 数字增加或者减少动画
]]
local M = GlobalNS.Class(GlobalNS.NumNodeAniBase);
M.clsName = "NumIncOrDecAnim";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mMinValue = 0;
	self.mMaxValue = 0;
	self.mCurValue = 0;
	self.mSpeed = 1;
	self.mIsInc = true;
end

function M:getMinValue()
	return self.mMinValue;
end

function M:setMinValue(value)
	self.mMinValue = value;
end

function M:getMaxValue()
	return self.mMaxValue;
end

function M:setMaxValue(value)
	self.mMaxValue = value;
end

function M:getCurValue()
	return self.mCurValue;
end

function M:setCurValue(value)
	self.mCurValue = value;
end

function M:getSpeed()
	return self.mSpeed;
end

function M:setSpeed(value)
	self.mSpeed = value;
end

function M:onTick(delta, tickMode)
	M.super.onTick(self, delta, tickMode);

	if(self:isStopAnim()) then
		self:stopAnim();
	else
		if(GlobalNS.NumIncOrDecAnimMode.eInc == self.mNumIncOrDecAnimMode) then
			self.mCurValue = self.mCurValue + delta * self.mSpeed;

			if(self.mCurValue > self.mMaxValue) then
				self.mCurValue = self.mMaxValue;
			end
		elseif(GlobalNS.NumIncOrDecAnimMode.eDec == self.mNumIncOrDecAnimMode) then
			self.mCurValue = self.mCurValue - delta * self.mSpeed;

			if (self.mCurValue < self.mMinValue) then
				self.mCurValue = self.mMinValue;
			end
		elseif(GlobalNS.NumIncOrDecAnimMode.ePingPong == self.mNumIncOrDecAnimMode) then
			if(self.mIsInc) then
				self.mCurValue = self.mCurValue + delta * self.mSpeed;

				if(self.mCurValue > self.mMaxValue) then
					self.mIsInc = false;

					self.mCurValue = self.mCurValue - (self.mMaxValue - self.mCurValue);

					if(self.mCurValue < self.mMinValue) then
						self.mCurValue = self.mMinValue;
					end
				end
			else
				self.mCurValue = self.mCurValue - delta * self.mSpeed;

				if (self.mCurValue < self.mMinValue) then
					self.mIsInc = true;

					self.mCurValue = self.mCurValue + (self.mMinValue - self.mCurValue);

					if (self.mCurValue > self.mMaxValue) then
						self.mCurValue = self.mMaxValue;
					end
				end
			end
		end
	end

	self:onNextAnim();
end

function M:isStopAnim()
	local ret = false;

	if(GlobalNS.NumIncOrDecAnimMode.eInc == self.mNumIncOrDecAnimMode) then
		if (self.mCurValue >= self.mMaxValue) then
			ret = true;
		end
	elseif(GlobalNS.NumIncOrDecAnimMode.eDec == self.mNumIncOrDecAnimMode) then
		if (self.mCurValue <= self.mMinValue) then
			ret = true;
		end
	end

	return ret;
end

return M;