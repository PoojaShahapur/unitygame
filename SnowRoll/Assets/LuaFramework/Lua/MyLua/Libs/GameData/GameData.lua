--region *.lua
--Date
--此文件由[BabeLua]插件自动生成

local M = GlobalNS.Class(GlobalNS.GObject);
M.clsName = "GameData";
GlobalNS[M.clsName] = M;

function M:ctor(...)
    self.isRelogin = false; --是否重新登录游戏

    self.ranklistCount = 0; --结算排行数量
    self.myRank = 0; --自己结算时排名
    self.rankinfolist = {}; --结算排行榜

    self.reliveTime = 0; --复活倒计时
    self.enemyName = 0; --敌人名称

    self.mTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);

    self.mMessageType = 1; --消息类型：1.弹出框 2.滚动提示
    self.mMessageText = ""; --消息内容
    self.mMessageMethond = 0; --对应调用方法
end

function M:dtor()
    self.mTimer:Stop();
end

function M:setRankInfoList(args)
    local ranklist = args[0];
    self.myRank = args[1];
    self.ranklistCount = args[2];
    for i=1, self.ranklistCount do
        self.rankinfolist[i] = 
        {
            m_rank = ranklist[i-1].rank;
            m_eid = ranklist[i-1].eid;
            m_name = ranklist[i-1].name;
            m_radius = ranklist[i-1].radius;
            m_swallownum = ranklist[i-1].swallownum;
        };
    end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIRankListPanel);
    if nil ~= form and form.mIsReady then
        form:updateUIData();
    end
end

--每局游戏倒计时
function M:setGameTime(totalTime)
	self.mTimer.mIsDisposed = false;
    self.mTimer:setTotalTime(totalTime);
    self.mTimer:setFuncObject(self, self.onTick);
    self.mTimer:Start();
end

function M:onTick()
    local lefttime = GlobalNS.UtilMath.ceil(self.mTimer:getLeftRunTime());
	if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUIPlayerDataPanel) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIPlayerDataPanel);
        if nil ~= form and form.mIsReady then
            form:refreshLeftTime(lefttime);
        end
    end
end

function M:ShowMessageBox(msg)
    self.mMessageType = 1;
    self.mMessageText = msg;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIMessagePanel);
end

function M:ShowRollMessage(msg)
    self.mMessageType = 2;
    self.mMessageText = msg;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIMessagePanel);
end

function M:returnStartGame()
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:dispose();
	GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIStartGame);
    --GlobalNS.CSSystem.Ctx.mInstance.mModuleSys:unloadModule(GlobalNS.CSSystem.ModuleId.GAMEMN);
    GlobalNS.CSSystem.Ctx.mInstance.mModuleSys:loadModule(GlobalNS.CSSystem.ModuleId.LOGINMN);
end

return M;
--endregion
