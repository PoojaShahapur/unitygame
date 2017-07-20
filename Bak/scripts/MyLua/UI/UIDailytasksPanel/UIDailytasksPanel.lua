MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIDailytasksPanel.DailytasksPanelNS");
MLoader("MyLua.UI.UIDailytasksPanel.DailytasksPanelData");
MLoader("MyLua.UI.UIDailytasksPanel.DailytasksPanelCV");

MLoader("MyLua.UI.UIDailytasksPanel.Panel.ActivityRewardViewItem");
MLoader("MyLua.UI.UIDailytasksPanel.Panel.DailyTaskViewItem");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIDailytasksPanel";
GlobalNS.DailytasksPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIDailytasksPanel;
	self.mData = GlobalNS.new(GlobalNS.DailytasksPanelNS.DailytasksPanelData);
	
	self.mDailyTaskViewList = GlobalNS.new(GlobalNS.MList);
	self.mActivityRewardViewItemList = GlobalNS.new(GlobalNS.MList);
	self.mSlider = GlobalNS.new(GlobalNS.AuxSlider);
	self.mSlider:setIsDestroySelf(false);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachCloseBtn(GlobalNS.DailytasksPanelNS.DailytasksPanelPath.CloseBtn);
	
	self.mSlider:setSelfGoByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.Slider);
	
	local index = 0;
	local activityCount = 0;
	local dataItem = nil;
	local viewItem = nil;
	
	activityCount = GCtx.mPlayerData.mDailyTaskData:getTaskCount();
	
	while(index < activityCount) do
		viewItem = GlobalNS.new(GlobalNS.DailytasksPanelNS.DailyTaskViewItem);
		self.mDailyTaskViewList:add(viewItem);

		dataItem = GCtx.mPlayerData.mDailyTaskData:getTaskItemDataByTaskId(index);
		viewItem:setItemData(dataItem);
		viewItem:init();

		index = index + 1;
	end
	
	index = 0;
	activityCount = GCtx.mPlayerData.mDailyTaskData:getActivityCount();
	dataItem = nil;
	viewItem = nil;
	
	while(index < activityCount) do
		viewItem = GlobalNS.new(GlobalNS.DailytasksPanelNS.ActivityRewardViewItem);
		self.mActivityRewardViewItemList:add(viewItem);

		dataItem = GCtx.mPlayerData.mDailyTaskData:getActivityItemDataByActivityId(index);
		viewItem:setGuiWin(self.mGuiWin);
		viewItem:setItemData(dataItem);
		viewItem:init();

		index = index + 1;
	end
	
	self:updateUI();
	
	GCtx.mPlayerData.mDailyTaskData:addActivityChangeEventHandle(self, self.onActivityValueChange);
	
	self:on_scrollview_loaded();
	self:setScrollRectTableInfo();
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
	
	if(nil ~= self.mSlider) then
		self.mSlider:dispose();
		self.mSlider = nil;
	end
	
	local index = 0;
	local activityCount = 0;
	local viewItem = nil;
	
	activityCount = GCtx.mPlayerData.mDailyTaskData:getTaskCount();
	
	while(index < activityCount) do
		viewItem = self.mDailyTaskViewList:get(index);
		viewItem:dispose();

		index = index + 1;
	end
	
	self.mDailyTaskViewList:clear();
	self.mDailyTaskViewList = nil;
	
	index = 0;
	activityCount = GCtx.mPlayerData.mDailyTaskData:getActivityCount();
	viewItem = nil;
	
	while(index < activityCount) do
		viewItem = self.mActivityRewardViewItemList:get(index);
		viewItem:dispose();

		index = index + 1;
	end
	
	self.mActivityRewardViewItemList:clear();
	self.mActivityRewardViewItemList = nil;
	
	GCtx.mPlayerData.mDailyTaskData:removeActivityChangeEventHandle(self, self.onActivityValueChange);
end

function M:on_scrollview_loaded()
    --获取ScrollRect下Content中的ScrollRectTable组件
    self.scroll_rect_table = GlobalNS.UtilApi.getComByPath(self.mGuiWin, GlobalNS.DailytasksPanelNS.DailytasksPanelPath.Content, "ScrollRectTable");

    self.scroll_rect_table.onItemRender = function(scroll_rect_item, index)
		local viewItem = self.mDailyTaskViewList:get(index - 1);
		viewItem:onItemViewShow(scroll_rect_item.gameObject);
	end
     
     self.scroll_rect_table.onItemDispear = function(index)
		local viewItem = self.mDailyTaskViewList:get(index - 1);
		viewItem:onItemViewHide();
	end
end

function M:setScrollRectTableInfo()
    if(GCtx.mPlayerData.mDailyTaskData:getTaskCount() > 0) then
        self.scroll_rect_table.recordCount= GCtx.mPlayerData.mDailyTaskData:getTaskCount();
        self.scroll_rect_table:init();
        self.scroll_rect_table:Refresh(-1, -1);
    end
end

function M:updateUI()
	self:onActivityValueChange(nil);
end

function M:onActivityValueChange(dispObj)
	self.mSlider:setSliderValue(GCtx.mPlayerData.mDailyTaskData:getSliderValue());
end

return M;