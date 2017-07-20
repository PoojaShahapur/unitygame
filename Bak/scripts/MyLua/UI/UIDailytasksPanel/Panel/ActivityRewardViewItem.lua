MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.ObjectItem.ItemViewBase");

local M = GlobalNS.Class(GlobalNS.ItemViewBase);
M.clsName = "ActivityRewardViewItem";
GlobalNS.DailytasksPanelNS[M.clsName] = M;

function M:ctor()
	self.mGuiWin = nil;
	
	self.mRewardImage = GlobalNS.new(GlobalNS.AuxObjectImage);
	self.mRewardImage:setIsDestroySelf(false);
	self.mStarImage = GlobalNS.new(GlobalNS.AuxSimpleAtlasImage);
	self.mStarImage:setIsDestroySelf(false);
	self.mActivityValueText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mActivityValueText:setIsDestroySelf(false);
end

function M:dtor()
	
end

function M:setGuiWin(value)
	self.mGuiWin = value;
end

function M:getTag()
	return self.mItemData:getUniqueId();
end

function M:init()
	GCtx.mPlayerData.mDailyTaskData:addActivityChangeEventHandle(self, self.onActivityValueChange);
	
	if(GlobalNS.ActivityRewardTypeId.eActivityReward_0 == self:getTag()) then
		self.mRewardImage:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityRewardImage_0);
		self.mActivityValueText:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityValueText_0);
		self.mStarImage:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityStarImage_0);
	elseif(GlobalNS.ActivityRewardTypeId.eActivityReward_1 == self:getTag()) then
		self.mRewardImage:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityRewardImage_1);
		self.mActivityValueText:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityValueText_1);
		self.mStarImage:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityStarImage_1);
	elseif(GlobalNS.ActivityRewardTypeId.eActivityReward_2 == self:getTag()) then
		self.mRewardImage:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityRewardImage_2);
		self.mActivityValueText:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityValueText_2);
		self.mStarImage:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityStarImage_2);
	elseif(GlobalNS.ActivityRewardTypeId.eActivityReward_3 == self:getTag()) then
		self.mRewardImage:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityRewardImage_3);
		self.mActivityValueText:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityValueText_3);
		self.mStarImage:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityStarImage_3);
	elseif(GlobalNS.ActivityRewardTypeId.eActivityReward_4 == self:getTag()) then
		self.mRewardImage:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityRewardImage_4);
		self.mActivityValueText:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityValueText_4);
		self.mStarImage:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.ActivityStarImage_4);
	end
	
	self.mRewardImage:setObjectBaseId(self.mItemData:getRewardObjectId());
	self.mActivityValueText:setText('' .. self.mItemData:getActivityValue());
	
	self:onActivityValueChange(nil);
end

function M:dispose()
	GCtx.mPlayerData.mDailyTaskData:removeActivityChangeEventHandle(self, self.onActivityValueChange);
	
	self.mGuiWin = nil;
	
	if(nil ~= self.mRewardImage) then
		self.mRewardImage:dispose();
		self.mRewardImage = nil;
	end
	if(nil ~= self.mStarImage) then
		self.mStarImage:dispose();
		self.mStarImage = nil;
	end
	if(nil ~= self.mActivityValueText) then
		self.mActivityValueText:dispose();
		self.mActivityValueText = nil;
	end
end

function M:onActivityValueChange(dispObj)
	if(self.mItemData:isCurActivityValueGreatEqualConfigValue()) then
		self.mStarImage:setSimpleImageId(GCtx.mPlayerData.mDailyTaskData:getLightStarId());
	else
		self.mStarImage:setSimpleImageId(GCtx.mPlayerData.mDailyTaskData:getDarkStarId());
	end
end

return M;