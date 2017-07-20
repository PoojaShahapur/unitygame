MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.Core.GObject");

--[[
@brief ShopPanel
]]

local M = GlobalNS.Class(GlobalNS.ObjectPanelBase);
M.clsName = "ShopPanel";
GlobalNS[M.clsName] = M;

function M:ctor()
	self.mIsReqData = false;
	
	self.mTopType = 0;
end

function M:dtor()
	
end

function M:init()
	
end

function M:dispose()
	
end

function M:setTopType(value)
	self.mTopType = 0;
end

--是否请求数据
function M:hasReqData()
	return self.mIsReqData == true;
end

function M:reqData()
	if(not self.mIsReqData) then
		self.mIsReqData = true;
		-- 请求数据
		local shopId = GlobalNS.UtilLogic.convTopIndex2ShopIndex(self.mTopType, self.mPanelType);
		GlobalNS.CSSystem.reqShopData(shopId);
	end
end

return M;