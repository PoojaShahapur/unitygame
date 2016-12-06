MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.AuxComponent.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UITopXRankPanel.TopXRankPanelNS");
MLoader("MyLua.UI.UITopXRankPanel.TopXRankPanelData");
MLoader("MyLua.UI.UITopXRankPanel.TopXRankPanelCV");

--UI区
local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UITopXRankPanel";
GlobalNS.TopXRankPanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormID.eUITopXRankPanel;
	self.mData = GlobalNS.new(GlobalNS.TopXRankPanelNS.TopXRankPanelData);

    itemCount = 10;
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);

    --排名信息
    self.topN = { };
    for i=1,itemCount do
        self.topN[i] = {m_name="Bone"};
    end
end

function M:onReady()
    M.super.onReady(self);

    for i=1, itemCount do
        --获取topx的GameObject对象
        local topx = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "Top" .. i);

        --Name
        local rankname = GlobalNS.UtilApi.getComByPath(topx, "Name", "Text");
        rankname.text = self.topN[i].m_name;

        --Rank
        local rankImage = GlobalNS.UtilApi.getComByPath(topx, "Rank", "Image");
        rankImage.color = Color.New(0, 126, 126, 255);
        local rank = GlobalNS.UtilApi.TransFindChildByPObjAndPath(topx, "Rank");
        local rankText = GlobalNS.UtilApi.getComByPath(rank, "Text", "Text");
        rankText.text = i;
    end
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

return M;