MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

MLoader("MyLua.UI.UIAccountPanel.AccountPanelCV");

local M = GlobalNS.Class(GlobalNS.AreaViewItem);
M.clsName = "GameShowAreaViewItem";
GlobalNS.AccountPanelNS[M.clsName] = M;

function M:ctor()
	self.mGridInfoList = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.mGridInfoList:setIsSpeedUpFind(true);
	self.mGridInfoList:setIsOpKeepSort(true);
end

function M:dtor()
	
end

function M:init()
	M.super.init(self);
end

function M:dispose()
	if(nil ~= self.mTplItem) then
		self.mTplItem:dispose();
		self.mTplItem = nil;
	end
	
	local index = 0;
	local listLen = self.mGridInfoList:Count();
	
	while(index < listLen) do
		self.mGridInfoList:get(index):dispose();
		index = index + 1;
	end
	
	self.mGridInfoList:Clear();
	
	M.super.dispose(self);
end

function M:onInitView()
	M.super.onInitView(self);
	
	self.mTplItem = GlobalNS.new(GlobalNS.AuxPrefabLoader);
	self.mTplItem:setIsNeedInsPrefab(false);
	self.mTplItem:syncLoad("UI/UIAccountPanel/ShowItem.prefab");
	local tplGo = self.mTplItem:getPrefabTmpl();
	local go = GlobalNS.UtilApi.Instantiate(tplGo);

	local key = 1;
	local item = GlobalNS.new(GlobalNS.AccountPanelNS.GameShowViewItem);
	item:setIsDestroySelf(false);
	item:setParentGo(self.mContentGo);
	item:setRootGo(go);
	self.mGridInfoList:add(key, item);
end

return M;