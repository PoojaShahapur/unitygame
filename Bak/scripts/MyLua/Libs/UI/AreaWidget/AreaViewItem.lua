MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "AreaViewItem";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mGuiWin = nil; 	-- Form 窗口
	self.mAreaWidget = nil;
	self.mContentGo = nil;
	self.mTag = 0;
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	self.mGuiWin = nil;
	self.mAreaWidget = nil;
	self.mContentGo = nil;
	
	M.super.dispose(self);
end

function M:getTag()
	return self.mTag;
end

function M:setTag(value)
	self.mTag = value;
end

function M:isEqualTag(value)
	return self.mTag == value;
end

function M:setAreaWidget(value)
	self.mAreaWidget = value;
end

function M:setContentByPath(parentGo, path)
	self.mGuiWin = parentGo;
	self.mContentGo = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, path);
end

--初始化显示
function M:onInitView()
	
end

return M;