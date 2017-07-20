MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.Libs.AnimSys.NumNodeAnim.NumIncOrDecAnim");

local M = GlobalNS.Class(GlobalNS.NumIncOrDecAnim);
M.clsName = "CanvasGroupAlphaAnim";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mCanvasGroupList = GlobalNS.new(GlobalNS.MList);
	self.mCanvasGroupList:setIsSpeedUpFind(true);
	self.mCanvasGroupList:setIsOpKeepSort(false);
	
	self.mIsNeedResetAlpha = false;
	self.mResetAlpha = 1;
end

function M:setIsNeedResetAlpha(value)
	self.mIsNeedResetAlpha = value;
end

function M:setResetAlpha(value)
	self.mResetAlpha = value;
end

function M:onPutInPool()	
	if(self.mIsNeedResetAlpha) then
		local index = 0;
		local listLen = self.mCanvasGroupList:Count();
		local canvasGroup = nil;

		while(index < listLen) do
			canvasGroup = self.mCanvasGroupList:get(index);
			GlobalNS.UtilApi.setCanvasGroupAlpha(canvasGroup, self.mResetAlpha);
			index = index + 1;
		end
	end

	self.mCanvasGroupList:clear();

	M.super.onPutInPool(self);
end

function M:onDestroy()
	self.mCanvasGroupList = nil;

	M.super.onDestroy(self);
end

function M:addCanvasGroup(value)
	if(nil ~= value and not self.mCanvasGroupList:Contains(value)) then
		self.mCanvasGroupList:add(value);
	end
end

function M:addCanvasGroupByGo(go)
	local canvasGroup = GlobalNS.UtilApi.getCanvasGroupComponent(go);
	self:addCanvasGroup(canvasGroup);
end

function M:onNextAnim()
	M.super.onNextAnim(self);

	local index = 0;
	local listLen = self.mCanvasGroupList:Count();
	local canvasGroup = nil;

	while(index < listLen) do
		canvasGroup = self.mCanvasGroupList:get(index);
		GlobalNS.UtilApi.setCanvasGroupAlpha(canvasGroup, self.mCurValue);
		index = index +  1;
	end
end

return M;