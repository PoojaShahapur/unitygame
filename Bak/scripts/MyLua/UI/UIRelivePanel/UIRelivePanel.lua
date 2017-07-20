MLoader("MyLua.Libs.Core.GlobalNS");
MLoader("MyLua.Libs.Core.Class");
MLoader("MyLua.Libs.UI.UICore.Form");

MLoader("MyLua.Libs.Auxiliary.AuxUIComponent.AuxButton");

MLoader("MyLua.UI.UIRelivePanel.RelivePanelNS");
MLoader("MyLua.UI.UIRelivePanel.RelivePanelData");
MLoader("MyLua.UI.UIRelivePanel.RelivePanelCV");

local M = GlobalNS.Class(GlobalNS.Form);
M.clsName = "UIRelivePanel";
GlobalNS.RelivePanelNS[M.clsName] = M;

function M:ctor()
	self.mId = GlobalNS.UIFormId.eUIRelivePanel;
	self.mData = GlobalNS.new(GlobalNS.RelivePanelNS.RelivePanelData);
    self.mTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);
end

function M:dtor()
	
end

function M:onInit()
    M.super.onInit(self);

    self.roomFatherBtn = GlobalNS.new(GlobalNS.AuxButton);
    self.roomFatherBtn:addEventHandle(self, self.onBtnReliveClk);

    self.mBackRoomBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mBackRoomBtn:addEventHandle(self, self.onBtnBackRoomClk);
	
	self.mReliveBtn = GlobalNS.new(GlobalNS.AuxButton);
	self.mReliveBtn:addEventHandle(self, self.onBtnReliveClk);
end

function M:onReady()
    M.super.onReady(self);
    self.roomFatherBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(self.mGuiWin, "BackRoom"));
    self.mBackRoomBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.roomFatherBtn:getSelfGo(), 
			GlobalNS.RelivePanelNS.RelivePanelPath.BtnBackRoom)
		);

	self.mReliveBtn:setSelfGo(GlobalNS.UtilApi.TransFindChildByPObjAndPath(
			self.mGuiWin, 
			GlobalNS.RelivePanelNS.RelivePanelPath.BtnRelive)
		);

    if GCtx.mGameData.enemyId ~= 0 then
        self:UpdateReliveTimeAndEnemyName(GCtx.mGameData.reliveTime, GCtx.mGameData.enemyName);
    end
    
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mHeroData:setCanMove(false);
    if 2 == GlobalNS.CSSystem.Ctx.mInstance.mShareData:getGameMode() then
        --组队模式禁止返回大厅
        self.mBackRoomBtn:hide();
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
    self.mTimer:Stop();
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mHeroData:setCanMove(true);
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerData.mHeroData:setIsSendMoveCmd(false);
end

function M:onBtnReliveClk()
    self.mTimer:Stop();
	self:exit();
end

function M:onBtnBackRoomClk()
    self.mTimer:Stop();
	GCtx.mGameData:requestBackHall();
end

function M:UpdateReliveTimeAndEnemyName(reliveseconds, enemyName)
    --self.mReliveBtn:setText("无敌时间（<color=#00FF01FF>" .. reliveseconds .. "</color>）");

    if GCtx.mGameData.iskilledbyself then
        self.roomFatherBtn:setText("你在危险区 <color=#CC0033FF>自杀了</color>");
    else
        self.roomFatherBtn:setText("你被  <color=#00FF01FF>" .. enemyName .. "</color>  击败了");
    end

    self.mTimer:setTotalTime(reliveseconds);
    self.mTimer:setFuncObject(self, self.onTick);
    self.mTimer:reset();
    self.mTimer:Start();
end

function M:onTick()
	local lefttime = GlobalNS.UtilMath.ceil(self.mTimer:getLeftRunTime());
    if lefttime <= 0 then
        self:exit();
    else
        --self.mReliveBtn:setText("无敌时间（<color=#00FF01FF>" .. lefttime .. "</color>）");
    end
end

return M;