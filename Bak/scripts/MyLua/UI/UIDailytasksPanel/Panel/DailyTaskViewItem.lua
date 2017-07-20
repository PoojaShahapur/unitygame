MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.ObjectItem.ItemViewBase");

local M = GlobalNS.Class(GlobalNS.ItemViewBase);
M.clsName = "DailyTaskViewItem";
GlobalNS.DailytasksPanelNS[M.clsName] = M;

function M:ctor()
	self.mRootNode = nil;
end

function M:dtor()
	
end

function M:init()
	self.mItemData:addFinishCountChangeHandle(self, self.onFinishCountChangeHandle);
	self.mItemData:addIsReceiveTaskChangeHandle(self, self.onIsReceiveTaskChangeHandle);
	
	self.mTaskNameText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mTaskNameText:setIsDestroySelf(false);
	
	self.mFinishCountText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mFinishCountText:setIsDestroySelf(false);
	
	self.mActivityValueText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mActivityValueText:setIsDestroySelf(false);
	
	self.mTaskDescText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mTaskDescText:setIsDestroySelf(false);
	
	self.mTaskRewardOneImage = GlobalNS.new(GlobalNS.AuxObjectImage);
	self.mTaskRewardOneImage:setIsDestroySelf(false);
	self.mTaskRewardOneText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mTaskRewardOneText:setIsDestroySelf(false);
	
	self.mTaskRewardTwoImage = GlobalNS.new(GlobalNS.AuxObjectImage);
	self.mTaskRewardTwoImage:setIsDestroySelf(false);
	self.mTaskRewardTwoText = GlobalNS.new(GlobalNS.AuxLabel);
	self.mTaskRewardTwoText:setIsDestroySelf(false);
	
	self.mReceiveBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mReceiveBtn:setIsDestroySelf(false);
	self.mReceiveBtn:addEventHandle(self, self.onReceiveBtnClick);
	self.mGoToBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mGoToBtn:setIsDestroySelf(false);
	self.mGoToBtn:addEventHandle(self, self.onGoToBtnClick);
	self.mCompletedBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mCompletedBtn:setIsDestroySelf(false);
	self.mCompletedBtn:addEventHandle(self, self.onCompletedBtnClick);
	
	self.mTaskImage = GlobalNS.new(GlobalNS.AuxSimpleAtlasImage);
	self.mTaskImage:setIsDestroySelf(false);
end

function M:dispose()
	self.mItemData:removeFinishCountChangeHandle(self, self.onFinishCountChangeHandle);
	self.mItemData:removeIsReceiveTaskChangeHandle(self, self.onIsReceiveTaskChangeHandle);
	
	if(nil ~= self.mTaskNameText) then
		self.mTaskNameText:dispose();
		self.mTaskNameText = nil;
	end
	if(nil ~= self.mFinishCountText) then
		self.mFinishCountText:dispose();
		self.mFinishCountText = nil;
	end
	if(nil ~= self.mActivityValueText) then
		self.mActivityValueText:dispose();
		self.mActivityValueText = nil;
	end
	if(nil ~= self.mTaskDescText) then
		self.mTaskDescText:dispose();
		self.mTaskDescText = nil;
	end
	if(nil ~= self.mTaskRewardOneImage) then
		self.mTaskRewardOneImage:dispose();
		self.mTaskRewardOneImage = nil;
	end
	if(nil ~= self.mTaskRewardOneText) then
		self.mTaskRewardOneText:dispose();
		self.mTaskRewardOneText = nil;
	end
	if(nil ~= self.mTaskRewardTwoImage) then
		self.mTaskRewardTwoImage:dispose();
		self.mTaskRewardTwoImage = nil;
	end
	if(nil ~= self.mTaskRewardTwoText) then
		self.mTaskRewardTwoText:dispose();
		self.mTaskRewardTwoText = nil;
	end
	if(nil ~= self.mReceiveBtn) then
		self.mReceiveBtn:dispose();
		self.mReceiveBtn = nil;
	end
	if(nil ~= self.mGoToBtn) then
		self.mGoToBtn:dispose();
		self.mGoToBtn = nil;
	end
	if(nil ~= self.mCompletedBtn) then
		self.mCompletedBtn:dispose();
		self.mCompletedBtn = nil;
	end
	if(nil ~= self.mAlreadyFinishedCount) then
		self.mAlreadyFinishedCount:dispose();
		self.mAlreadyFinishedCount = nil;
	end
	if(nil ~= self.mTaskImage) then
		self.mTaskImage:dispose();
		self.mTaskImage = nil;
	end
	
	self.mRootNode = nil;
end

function M:updateUI()
	self.mTaskNameText:setText(self.mItemData:getTaskName());
	self.mFinishCountText:setText(string.format("%d/%d", self.mItemData:getFinishCount(), self.mItemData:getNeedFinishCount()));
	self.mActivityValueText:setText(string.format("活跃度加+%d", self.mItemData:getAddActivityValue()));
	self.mTaskDescText:setText(self.mItemData:getTaskDesc());
	
	self.mTaskRewardOneImage:setObjectBaseId(self.mItemData:getTaskRewardObjectIdById(0));
	self.mTaskRewardOneText:setText('' .. self.mItemData:getTaskRewardObjectNumById(0));
	self.mTaskRewardTwoImage:setObjectBaseId(self.mItemData:getTaskRewardObjectIdById(1));
	self.mTaskRewardTwoText:setText('' .. self.mItemData:getTaskRewardObjectNumById(1));
	
	self.mTaskImage:setSimpleImageId(self.mItemData:getTaskImageId());
	
	self:onIsReceiveTaskChangeHandle(nil);
	self:onFinishCountChangeHandle(nil);
end

function M:onItemViewShow(rootNode)
	self.mRootNode = rootNode;
	
	self.mTaskNameText:setSelfGoByPath(rootNode, "Taskname_Image/Text");
	self.mFinishCountText:setSelfGoByPath(rootNode, "ScheduleText");
	self.mActivityValueText:setSelfGoByPath(rootNode, "LivenessText");
	self.mTaskDescText:setSelfGoByPath(rootNode, "Taskdescription");
	
	self.mTaskRewardOneImage:setSelfGoByPath(rootNode, "Gift1_Btn/Image");
	self.mTaskRewardOneText:setSelfGoByPath(rootNode, "Gift1_Btn/Text");
	self.mTaskRewardTwoImage:setSelfGoByPath(rootNode, "Gift1_Btn (1)/Image");
	self.mTaskRewardTwoText:setSelfGoByPath(rootNode, "Gift1_Btn (1)/Text");
	
	self.mReceiveBtn:setSelfGoByPath(rootNode, "Receive_Btn");
	self.mGoToBtn:setSelfGoByPath(rootNode, "Goto_Btn");
	self.mCompletedBtn:setSelfGoByPath(rootNode, "Completed_Btn");
	self.mTaskImage:setSelfGoByPath(rootNode, "Image");
	
	self:updateUI();
end

function M:onItemViewHide()
	self.mTaskNameText:setSelfGo(nil);
	self.mFinishCountText:setSelfGo(nil);
	self.mActivityValueText:setSelfGo(nil);
	self.mTaskDescText:setSelfGo(nil);
	self.mTaskRewardOneImage:setSelfGo(nil);
	self.mTaskRewardOneText:setSelfGo(nil);
	self.mTaskRewardTwoImage:setSelfGo(nil);
	self.mTaskRewardTwoText:setSelfGo(nil);
	self.mReceiveBtn:setSelfGo(nil);
	self.mGoToBtn:setSelfGo(nil);
	self.mCompletedBtn:setSelfGo(nil);
end

function M:onReceiveBtnClick(dispObj)
	
end

function M:onGoToBtnClick(dispObj)
	
end

function M:onCompletedBtnClick(dispObj)
	
end

function M:onFinishCountChangeHandle(dispObj)
	if(self.mItemData:getIsReceiveTask()) then
		self.mFinishCountText:setText(string.format("%d/%d", self.mItemData:getFinishCount(), self.mItemData:getNeedFinishCount()));
		
		if(self.mItemData:isFinishedTask()) then
			self.mReceiveBtn:hide();
			self.mGoToBtn:hide();
			self.mCompletedBtn:show();
		end
	end
end

function M:onIsReceiveTaskChangeHandle(dispObj)
	if(self.mItemData:getIsReceiveTask()) then
		self.mReceiveBtn:hide();
		self.mGoToBtn:show();
		self.mCompletedBtn:hide();
	else
		self.mReceiveBtn:show();
		self.mGoToBtn:hide();
		self.mCompletedBtn:hide();
	end
end

return M;