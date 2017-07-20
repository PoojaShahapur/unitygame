MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
@brief 基本显示 Item
]]

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "ItemViewBase";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mIsDestroySelf = true;
	
	self.mPath = "";		-- 资源目录，暂时没有用
	self.mRootGo = nil; 	-- 显示的预制
	self.mParentGo = nil;
	
	self.mItemData = nil;
	
	-- Item 点击处理函数
	self.mItemBtn = nil;
end

function M:dtor()
	
end

function M:init()
    
end

function M:dispose()
	if(nil ~= self.mItemBtn) then
		self.mItemBtn:dispose();
		self.mItemBtn = nil;
	end
	
	if(self.mIsDestroySelf) then
		if(nil ~= self.mRootGo) then
			GlobalNS.UtilApi.Destroy(self.mRootGo);
			self.mRootGo = nil;
		end
	end
end

function M:isDestroySelf()
	return self.mIsDestroySelf;
end

function M:setIsDestroySelf(value)
	self.mIsDestroySelf = value;
end

function M:setParentGo(value)
	self.mParentGo = value;
	self:attachToParent();
end

function M:setRootGo(value)
	self.mRootGo = value;
	GlobalNS.UtilApi.SetActive(self.mRootGo, true);
	self:attachToParent();
end

function M:setItemData(value)
    self.mItemData = value;
end

function M:attachToParent()
    if(nil ~= self.mRootGo and nil ~= self.mParentGo) then
        GlobalNS.UtilApi.SetParent(self.mRootGo, self.mParentGo, false);
    end
end

function M:addItemClickHandle()
	if(nil == self.mItemBtn) then
		self.mItemBtn = GlobalNS.new(GlobalNS.AuxButton);
		self.mItemBtn:setIsDestroySelf(false);
		self.mItemBtn:addEventHandle(self, self.onItemClick);
		self.mItemBtn:setSelfGo(self.mRootGo);
	end
end

function M:onItemClick(dispObj)
	
end

function M:moveToTop()
	if(nil ~= self.mRootGo) then
		GlobalNS.UtilApi.SetSiblingIndexToLastTwoByGo(self.mRootGo);
	end
end

function M:moveToBottom()
	if(nil ~= self.mRootGo) then
		GlobalNS.UtilApi.SetSiblingIndexToZero(self.mRootGo);
	end
end

return M;