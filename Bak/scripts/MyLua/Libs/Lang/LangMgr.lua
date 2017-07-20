MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "LangMgr";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mLangId = GlobalNS.LangId.zh_CN;
	self.mDataSource = nil;
	self.mIsLoad = false;
end

function M:dtor()
	
end

function M:init()
	--[[
	if(GlobalNS.LangId.zh_CN == self.mLangId) then
		self.mDataSource = zh_CN;
	end
	]]
end

function M:dispose()
	
end

function M:getText(typeId, itemIdx)
	if(not self.mIsLoad) then
		self:loadTable();
	end
	
	return self.mDataSource[typeId][itemIdx];
end

function M:loadTable()
	self.mIsLoad = true;
	
	if(GlobalNS.LangId.zh_CN == self.mLangId) then
		self.mDataSource = zh_CN;
	end
end

return M;