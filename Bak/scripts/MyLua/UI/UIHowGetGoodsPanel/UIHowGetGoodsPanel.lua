MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIHowGetGoodsPanel.HowGetGoodsPanelNS");
MLoader("MyLua.UI.UIHowGetGoodsPanel.HowGetGoodsPanelData");
MLoader("MyLua.UI.UIHowGetGoodsPanel.HowGetGoodsPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIHowGetGoodsPanel";
GlobalNS.HowGetGoodsPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIHowGetGoodsPanel;
	self.mData = GlobalNS.new(GlobalNS.HowGetGoodsPanelNS.HowGetGoodsPanelData);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);

	self.mDetailText = GlobalNS.new(GlobalNS.AuxLabel);
end

function M:onReady()
    M.super.onReady(self);
	
	self:attachCloseBtn(GlobalNS.HowGetGoodsPanelNS.HowGetGoodsPanelPath.OkBtn);
	self.mDetailText:setSelfGoByPath(self.mGuiWin, GlobalNS.HowGetGoodsPanelNS.HowGetGoodsPanelPath.DetailDescText);
end

function M:onShow()
    M.super.onShow(self);
end

function M:onHide()
    M.super.onHide(self);
end

function M:onExit()
    M.super.onExit(self);
end

function M:setBaseShopItem(baseShopItem)
	self.mDetailText:setText(baseShopItem:getAcquireDetailDesc());
end

return M;