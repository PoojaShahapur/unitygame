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

    self.itemCount = 10;
    self.myRank = 0;
    --排名信息
    self.topN = { };
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);

    self.mDropBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mDropBtn:addEventHandle(self, self.onDropBtnClk);    
end

function M:onReady()
    M.super.onReady(self);

    self.topXBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "TopXBG");
	self.mDropBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.topXBG, 
			GlobalNS.TopXRankPanelNS.TopXRankPanelPath.BtnDrop)
		);

    self.heroentity = GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:getHero():getEntity();
    self.heroentity:cellCall("reqRankData");
    --self:showTop10Rank();
end

function M:showTop10Rank()
    for i=1, self.itemCount do
        if self.topXBG == nil then
            return;
        end
        --获取topx的GameObject对
        local topx = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.topXBG, "Top" .. i);
        topx:SetActive(true);
        if i % 2 == 0 then
			GlobalNS.UtilApi.setImageColor(topx, 255, 255, 255, 0);
        end

        --Name
        local rankname = GlobalNS.UtilApi.getComByPath(topx, "Name", "Text");
        if i < 10 then
             rankname.text = "  " .. self.topN[i].m_rank .. ". " .. self.topN[i].m_name;
        else
             rankname.text = self.topN[i].m_rank .. ". " .. self.topN[i].m_name;
        end       

        --Rank
        local rankImage = GlobalNS.UtilApi.getComByPath(topx, "Rank", "Image");
        
    end

    for j=self.itemCount + 1, 10 do
        if self.topXBG == nil then
            return;
        end
        --获取topx的GameObject对
        local topx = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.topXBG, "Top" .. j);
        topx:SetActive(false);
    end
end

function M:onDropBtnClk()
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormID.eUIConsoleDlg);
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

function M:onSetRankInfo(args) --args是C#数组 
    local ranklist = args[0];
    self.myRank = args[1];
    self.itemCount = args[2];
    for i=1, self.itemCount do
        self.topN[i] = 
        {
            m_rank = ranklist[i-1].rank;
            m_eid = ranklist[i-1].eid;
            m_name = ranklist[i-1].name;
        };
    end
    
    self:showTop10Rank();
end

return M;