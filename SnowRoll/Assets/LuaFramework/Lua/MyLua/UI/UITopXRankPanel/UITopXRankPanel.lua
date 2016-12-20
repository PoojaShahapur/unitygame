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

    self.mDropBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mDropBtn:addEventHandle(self, self.onDropBtnClk);

    --排名信息
    self.topN = { };
    for i=1,itemCount do
        self.topN[i] = {m_name="Bone"};
    end
end

function M:onReady()
    M.super.onReady(self);

    self.topXBG = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "TopXBG");
	self.mDropBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.topXBG, 
			GlobalNS.TopXRankPanelNS.TopXRankPanelPath.BtnDrop)
		);
    
    for i=1, itemCount do
        --获取topx的GameObject对        
        local topx = GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.topXBG, "Top" .. i);
        if i % 2 == 0 then
            --topx.color = Color.New(255, 255, 255, 0);
			GlobalNS.UtilApi.setImageColor(topx, 255, 255, 255, 0);
        end

        --Name
        local rankname = GlobalNS.UtilApi.getComByPath(topx, "Name", "Text");
        if i < 10 then
            rankname.text = "  " .. i .. ". " .. self.topN[i].m_name;
        else
            rankname.text = i .. ". " .. self.topN[i].m_name;
        end

        --Rank
        local rankImage = GlobalNS.UtilApi.getComByPath(topx, "Rank", "Image");
        
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

return M;