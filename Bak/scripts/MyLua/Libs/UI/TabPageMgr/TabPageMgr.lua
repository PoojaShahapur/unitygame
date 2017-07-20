--region *.lua
--Date
--此文件由[BabeLua]插件自动生成
MLoader("MyLua.Libs.UI.TabPageMgr.TabPage");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "TabPageMgr";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.tablist = GlobalNS.new(GlobalNS.MList);
    self.oldTabBtn = nil;
    self.oldTabView = nil;
    self.curTabBtn = nil;
    self.curTabView = nil;
	
	self.prePage = nil;
	self.curPage = nil;

    self.mTabClickEventDispatch = GlobalNS.new(GlobalNS.AddOnceEventDispatch);
end

function M:dtor()
	local index = 0;
	local listLen = self.tablist:Count();
	local tab = nil;
	
    while(index < listLen) do
        tab = self.tablist:get(index);
        tab.mTabClickEventDispatch:removeEventHandle(self, self.onTabClick);
        tab:dispose();
		
		index = index + 1;
    end

    self.tablist:Clear();
end

function M:addTabPage(tabBtn, tabView, pageCls)
	local tab = nil;
	
	if(nil ~= pageCls) then
		tab = GlobalNS.new(pageCls);
	else
		tab = GlobalNS.new(GlobalNS.TabPage);
	end
	
	tab:setTabPageMgr(self);
	tab:init();
    tab:setGo(tabBtn, tabView);

    tab.mTabClickEventDispatch:addEventHandle(self, self.onTabClick);
    self.tablist:add(tab);
	
	return tab;
end

function M:onTabClick(dispObj)
	if(self.curPage ~= dispObj) then
		self.prePage = self.curPage;
		self.curPage = dispObj;
		
		if(nil ~= self.prePage) then
			self.prePage:onLostFocus();
		end	
		
		self.curPage = dispObj;
		
		self.oldTabBtn = self.curTabBtn;
		self.oldTabView = self.curTabView;
		self.curTabBtn = dispObj.TabBtn;
		self.curTabView = dispObj.TabView;

		if self.oldTabBtn ~= nil then
			self.oldTabBtn:enable();
		end
		if self.oldTabView ~= nil then
			self.oldTabView:hide();
		end
		if self.curTabBtn ~= nil then
			self.curTabBtn:disable();
		end
		if self.curTabView ~= nil then
			self.curTabView:show();
		end

		self.mTabClickEventDispatch:dispatchEvent(self);
	end
end

function M:openPage(index)
	self.tablist:get(index):onTabBtnClk(nil);
end

function M:getCurPageTag()
	local tag = GlobalNS.UtilMath.InvalidIndex;
	
	if(nil ~= self.curPage) then
		tag = self.curPage:getTag();
	end
	
	return tag;
end

function M:getPageByTag(tag)
	return self.tablist:get(tag);
end

return M;

--endregion
