--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TabPage";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.TabBtn = nil; --页签按钮
    self.TabView = nil;--显示区
	self.mTag = 0;		-- 唯一 Id
	
	self.mTabPageMgr = nil;
end

function M:dtor()
	self:dispose();
end

function M:dispose()
	if(nil ~= self.TabBtn) then
		GlobalNS.delete(self.TabBtn);
		self.TabBtn = nil;
	end

	if(nil ~= self.TabView) then
		GlobalNS.delete(self.TabView);
		self.TabView = nil;
	end
end

function M:setTabPageMgr(value)
	self.mTabPageMgr = value;
end

function M:getTabPageMgr()
	return self.mTabPageMgr;
end

function M:init()
    self.TabBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.TabBtn:setIsDestroySelf(false);
	self.TabBtn:addEventHandle(self, self.onTabBtnClk);

    self.TabView = GlobalNS.new(GlobalNS.AuxComponent);
	self.TabView:setIsDestroySelf(false);
	
    self.mTabClickEventDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
end

function M:setGo(btn, view)
    self.TabBtn:setSelfGo(btn);
    self.TabView:setSelfGo(view);
	self.TabBtn:enable(); 	-- 默认是开启的
	self.TabView:hide(); 	-- 默认是隐藏的
end

function M:getTag()
	return self.mTag;
end

function M:setTag(value)
	self.mTag = value;
end

function M:onTabBtnClk(dispObj)
    self.mTabClickEventDispatch:dispatchEvent(self);
	self:onGetFocus();
end

function M:onGetFocus()
	
end

function M:onLostFocus()
	
end

return M;

--endregion
