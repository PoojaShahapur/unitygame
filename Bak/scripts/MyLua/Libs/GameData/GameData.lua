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
    --结算排行榜data
    self.rankinfolist = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.rankinfolist:setIsSpeedUpFind(true);
	self.rankinfolist:setIsOpKeepSort(true);

    self.reliveTime = 0; --复活倒计时
    self.enemyName = 0; --敌人名称
    self.iskilledbyself = false; --自杀

    self.totalTime = 0;
    self.mTimer = GlobalNS.new(GlobalNS.DaoJiShiTimer);

    self.mMessageType = 1; --消息类型：1.弹出框 2.滚动提示
    self.mMessageText = ""; --消息内容
    self.mMessageMethond = 0; --对应调用方法
	self.mMessageKeepTime = 1; 	--消息显示时间

    self.mMyScore = 0;
    self.top10ranklist = {}; --top10 排行
    self.top10Count = 0;  --top10 个数
    self.top10_myrank = 1;
    self.mMyName = "";

    --历史总榜
    self.historyranklistCount = 0;
    self.myhistoryRank = nil;
    self.historyrankinfolist = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.historyrankinfolist:setIsSpeedUpFind(true);
	self.historyrankinfolist:setIsOpKeepSort(true);
    self.isgethistorydata = false;

    self.ydhistoryranklistCount = 0;
    self.myydhistoryRank = nil;
    self.ydhistoryrankinfolist = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.ydhistoryrankinfolist:setIsSpeedUpFind(true);
	self.ydhistoryrankinfolist:setIsOpKeepSort(true);
    self.isgetydhistorydata = false;

    --段位奖励
    self.levellistCount = 0;
    self.levellist = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.levellist:setIsSpeedUpFind(true);
	self.levellist:setIsOpKeepSort(true);
    self.isgetleveldata = false;
    --段位排行
    self.levelranklistCount = 0;
    self.mylevelRank = nil;
    self.levelranklist = GlobalNS.new(GlobalNS.MKeyIndexList);
	self.levelranklist:setIsSpeedUpFind(true);
	self.levelranklist:setIsOpKeepSort(true);
    self.isgetlevelrankdata = false;
end

function M:dtor()
    self:clearRanklist();
    self:clearHistoryRanklist();
    self:clearlevellist();
    self:clearlevelranklist();
end

function M:clearRanklist()
    local count = self.rankinfolist:count();
    for i=1, count do
        local item = self.rankinfolist:get(i-1);
        if item.m_FocusBtn ~= nil then
            item.m_FocusBtn:dispose();
            item.m_FocusBtn = nil;
        end
        if item.m_AvatarBtn ~= nil then
            item.m_AvatarBtn:dispose();
            item.m_AvatarBtn = nil;
        end
    end
    self.rankinfolist:clear();
end

function M:setRankInfoList(args)
    self:clearRanklist();

    local ranklist = args[0];
    self.myRank = args[1];
    self.ranklistCount = args[2];
    for i=1, self.ranklistCount do
        local item = 
        {
            m_rank = ranklist[i-1].rank;
            m_eid = ranklist[i-1].eid;
            m_name = ranklist[i-1].name;
            m_nickname = ranklist[i-1].nickname;
            m_sex = ranklist[i-1].sex;
            m_killnum = ranklist[i-1].killnum;
            m_score = ranklist[i-1].score;
            m_award_1 = ranklist[i-1].award_1;
            m_award_2 = ranklist[i-1].award_2;
            m_avatarindex = ranklist[i-1].avatarindex;
            m_isAI = ranklist[i-1].is_ai;
            m_uid = ranklist[i-1].uid;
            m_isFllowing = 0;--1已关注
            m_isReqed = false; 
            m_FocusBtn = nil;  --关注
            m_AvatarBtn = nil; --头像
        };
        local key = item.m_uid;
        if(not self.rankinfolist:ContainsKey(key)) then
		    self.rankinfolist:add(key, item);
	    end
    end

    if GlobalNS.CSSystem.Ctx.mInstance.mShareData:getGameMode() == 1 then --炼狱模式
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIRankListPanel);
        if nil ~= form and form.mIsReady then
            form:updateUIData();
        end
    end
end

function M:clearHistoryRanklist()
    self.isgethistorydata = false;
    local count = self.historyrankinfolist:count();
    for i=1, count do
        local item = self.historyrankinfolist:get(i-1);
        if item.m_AvatarBtn ~= nil then
            item.m_AvatarBtn:dispose();
            item.m_AvatarBtn = nil;
        end
    end
    self.historyrankinfolist:clear();
end

function M:setHistoryRank(args)
    self.isgethistorydata = true;
    local historyranklist = args[0];
    self.myhistoryRank = historyranklist[0];--0元素为自己的数据
    self.historyranklistCount = args[1];
    for i=1, self.historyranklistCount do
        local item = 
        {
            m_rank = i; 
            m_name = historyranklist[i].acc;
            m_score = historyranklist[i].score;
            m_award = historyranklist[i].sugar;
            m_avatarindex = historyranklist[i].imageid;
            m_sex = historyranklist[i].sex;
            m_uid = historyranklist[i].uid;
            m_AvatarBtn = nil; --头像
        };

        local key = item.m_name;
        if(not self.historyrankinfolist:ContainsKey(key)) then
		    self.historyrankinfolist:add(key, item);
	    end
    end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIServerHistoryRankListPanel);
    if nil ~= form and form.mIsReady then
        form:updateUIData(0);
    end
end

function M:clearYDHistoryRanklist()
    self.isgetydhistorydata = false;
    local count = self.ydhistoryrankinfolist:count();
    for i=1, count do
        local item = self.ydhistoryrankinfolist:get(i-1);
        if item.m_AvatarBtn ~= nil then
            item.m_AvatarBtn:dispose();
            item.m_AvatarBtn = nil;
        end
    end
    self.ydhistoryrankinfolist:clear();
end

function M:setYDHistoryRank(args)
    self.isgetydhistorydata = true;
    local ydhistoryranklist = args[0];
    self.myydhistoryRank = ydhistoryranklist[0];--0元素为自己的数据
    self.ydhistoryranklistCount = args[1];
    for i=1, self.ydhistoryranklistCount do
        local item = 
        {
            m_rank = i; 
            m_name = ydhistoryranklist[i].acc;
            m_score = ydhistoryranklist[i].score;
            m_award = ydhistoryranklist[i].sugar;
            m_avatarindex = ydhistoryranklist[i].imageid;
            m_sex = ydhistoryranklist[i].sex;
            m_uid = ydhistoryranklist[i].uid;
            m_AvatarBtn = nil; --头像
        };

        local key = item.m_name;
        if(not self.ydhistoryrankinfolist:ContainsKey(key)) then
		    self.ydhistoryrankinfolist:add(key, item);
	    end
    end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIServerHistoryRankListPanel);
    if nil ~= form and form.mIsReady then
        form:updateUIData(1);
    end
end

function M:clearlevellist()
    self.isgetleveldata = false;
    local count = self.levellist:count();
    for i=1, count do
        local item = self.levellist:get(i-1);
        if item.m_LevelImage ~= nil then
            item.m_LevelImage:dispose();
            item.m_LevelImage = nil;
        end
        if item.m_Gift1Image ~= nil then
            item.m_Gift1Image:dispose();
            item.m_Gift1Image = nil;
        end
        if item.m_Gift2Image ~= nil then
            item.m_Gift2Image:dispose();
            item.m_Gift2Image = nil;
        end
        if item.m_Gift3Image ~= nil then
            item.m_Gift3Image:dispose();
            item.m_Gift3Image = nil;
        end
        if item.m_Gift4Image ~= nil then
            item.m_Gift4Image:dispose();
            item.m_Gift4Image = nil;
        end
    end
    self.levellist:clear();
end

function M:setlevellist(args)
    self.isgetleveldata = true;
    local _list = args[0];
    self.levellistCount = args[1];
    for i=1, self.levellistCount do
        local item = 
        {
            m_leveltype = _list[i-1].type;
            m_gift = _list[i-1].objs;
            m_LevelImage = nil;
            m_Gift1Image = nil;
            m_Gift2Image = nil;
            m_Gift3Image = nil;
            m_Gift4Image = nil;
        };
        
        local key = _list[i-1].type;
        if(not self.levellist:ContainsKey(key)) then
		    self.levellist:add(key, item);
	    end
    end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIgiftPanel);
    if nil ~= form and form.mIsReady then
        form:updateUIData();
    end
end

function M:clearlevelranklist()
    self.isgetlevelrankdata = false;
    local count = self.levelranklist:count();
    for i=1, count do
        local item = self.levelranklist:get(i-1);
        if item.m_AvatarImage ~= nil then
            item.m_AvatarImage:dispose();
            item.m_AvatarImage = nil;
        end
        if item.m_HonerImage ~= nil then
            item.m_HonerImage:dispose();
            item.m_HonerImage = nil;
        end
        if item.m_SexImage ~= nil then
            item.m_SexImage:dispose();
            item.m_SexImage = nil;
        end
        if item.m_LevelImage ~= nil then
            item.m_LevelImage:dispose();
            item.m_LevelImage = nil;
        end
        if item.m_Star1Image ~= nil then
            item.m_Star1Image:dispose();
            item.m_Star1Image = nil;
        end
        if item.m_Star2Image ~= nil then
            item.m_Star2Image:dispose();
            item.m_Star2Image = nil;
        end
        if item.m_Star3Image ~= nil then
            item.m_Star3Image:dispose();
            item.m_Star3Image = nil;
        end
        if item.m_Star4Image ~= nil then
            item.m_Star4Image:dispose();
            item.m_Star4Image = nil;
        end
        if item.m_Star5Image ~= nil then
            item.m_Star5Image:dispose();
            item.m_Star5Image = nil;
        end
        if item.m_AvatarBtn ~= nil then
            item.m_AvatarBtn:dispose();
            item.m_AvatarBtn = nil;
        end
    end
    self.levelranklist:clear();
end

function M:setlevelranklist(args)
    self.isgetlevelrankdata = true;
    local _list = args[0];
    self.levelranklistCount = args[1];
    self.mylevelRank = _list[0];
    for i=1, self.levelranklistCount do
        local item = 
        {
            m_rank = i; 
            m_name = _list[i].acc;
            m_avatarindex = _list[i].imageid;
            m_sex = _list[i].sex;
            m_uid = _list[i].uid;
            m_level = _list[i].level;
            m_AvatarBtn = nil; --头像
            m_AvatarImage = nil;
            m_HonerImage = nil;
            m_SexImage = nil;
            m_LevelImage = nil;
            m_Star1Image = nil;
            m_Star2Image = nil;
            m_Star3Image = nil;
            m_Star4Image = nil;
            m_Star5Image = nil;
        };
        
        local key = _list[i].uid;
        if(not self.levelranklist:ContainsKey(key)) then
		    self.levelranklist:add(key, item);
	    end
    end

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIServerHistoryRankListPanel);
    if nil ~= form and form.mIsReady then
        form:updateDanUIData();
    end
end

function M:setTop10RankList(args)
    local top10 = args[0];
    self.top10_myrank = args[1];
    self.top10Count = args[2];
    self.mMyName = args[3];
    for i=1, self.top10Count do
        self.top10ranklist[i] = 
        {
            m_rank = top10[i-1].rank;
            m_eid = top10[i-1].eid;
            m_name = top10[i-1].name;
        };
    end

    if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUITopXRankPanel) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUITopXRankPanel);
        if nil ~= form and form.mIsReady then            
            form:updateUIData();
        end
    end
end

--每局游戏倒计时
function M:setGameTime(totalTime)
    --Debugger.Log(" time1 " .. os.date("%H:%M:%S"))
    self.totalTime = totalTime;
	self.mTimer.mIsDisposed = false;
    self.mTimer:setTotalTime(totalTime);
    self.mTimer:setFuncObject(self, self.onTick);
    self.mTimer:reset();
    self.mTimer:Start();
end

function M:onTick()
    --Debugger.Log(" time2 " .. os.date("%H:%M:%S"))
    local lefttime = GlobalNS.UtilMath.ceil(self.mTimer:getLeftRunTime());
	if GCtx.mUiMgr:hasForm(GlobalNS.UIFormId.eUIPlayerDataPanel) then
        local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormId.eUIPlayerDataPanel);
        if nil ~= form and form.mIsReady then
            form:refreshLeftTime(lefttime);
        end
    end
end

function M:ShowMessageBox(msg)
	self.mMessageKeepTime = 1;
    self.mMessageType = 1;
    self.mMessageText = msg;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIMessagePanel);
end

function M:ShowRollMessage(msg)
	self.mMessageKeepTime = 1;
    self.mMessageType = 2;
    self.mMessageText = msg;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIMessagePanel);
end

function M:ShowRollMessageWithTimeLen(msg, timeLen)
	self.mMessageKeepTime = timeLen;
	self.mMessageType = 2;
    self.mMessageText = msg;
    GCtx.mUiMgr:loadAndShow(GlobalNS.UIFormId.eUIMessagePanel);
end

function M:requestBackHall()
    --客户端请求返回大厅
    GlobalNS.CSSystem.Ctx.mInstance.mShareData:BackHall();
end

function M:notifyBackHall()
    --服务器通知返回大厅
    GCtx.mGameData.isRelogin = true;
    self:clearResource();
    GlobalNS.CSSystem.Ctx.mInstance.mShareData:setShotCD(0);
    GlobalNS.CSSystem.Ctx.mInstance.mModuleSys:loadModule(GlobalNS.CSSystem.ModuleId.LOGINMN);
end

function M:clearResource()
    self.mTimer:Stop();
    GlobalNS.CSSystem.Ctx.mInstance.mPlayerMgr:dispose();
    GlobalNS.CSSystem.Ctx.mInstance.mSnowBlockMgr:dispose();
    GlobalNS.CSSystem.Ctx.mInstance.mComputerBallMgr:dispose();
    GlobalNS.CSSystem.Ctx.mInstance.mAbandonPlaneMgr:dispose();
    GlobalNS.CSSystem.Ctx.mInstance.mFlyBulletMgr:dispose();
    GlobalNS.CSSystem.Ctx.mInstance.mFlyBulletFlockMgr:dispose();
	GlobalNS.CSSystem.Ctx.mInstance.mHudSystem:dispose();

    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIPlayerDataPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIOptionPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUITopXRankPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIRelivePanel);
end

function M:ExitSomeForm()
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIAccountPanel);
    GCtx.mUiMgr:exitForm(GlobalNS.UIFormId.eUIFindFriend);
end

--段位星星
function M:getStarImage(index, star, maxstar)
    local starname = "star_a";
    if 2 == maxstar then
        if 1 == index then
            if 0 == star then
                starname = "star_a";
            else
                starname = "star_b";
            end
        else
            if 2 == star then
                starname = "star_b";
            else
                starname = "star_a";
            end
        end
    elseif 3 == maxstar then
        if 1 == index then
            if 0 == star then
                starname = "star_a";
            else
                starname = "star_b";
            end
        elseif 2 == index then
            if star < 2 then
                starname = "star_a";
            else
                starname = "star_b";
            end
        else
            if star < 3 then
                starname = "star_a";
            else
                starname = "star_b";
            end
        end
    elseif 4 == maxstar then
        if 1 == index then
            if 0 == star then
                starname = "star_a";
            else
                starname = "star_b";
            end
        elseif 2 == index then
            if star < 2 then
                starname = "star_a";
            else
                starname = "star_b";
            end
        elseif 3 == index then
            if star < 3 then
                starname = "star_a";
            else
                starname = "star_b";
            end
        else
            if star < 4 then
                starname = "star_a";
            else
                starname = "star_b";
            end
        end
    elseif 5 == maxstar then
        if 1 == index then
            if 0 == star then
                starname = "star_a";
            else
                starname = "star_b";
            end
        elseif 2 == index then
            if star < 2 then
                starname = "star_a";
            else
                starname = "star_b";
            end
        elseif 3 == index then
            if star < 3 then
                starname = "star_a";
            else
                starname = "star_b";
            end
        elseif 4 == index then
            if star < 4 then
                starname = "star_a";
            else
                starname = "star_b";
            end
        else
            if star < 5 then
                starname = "star_a";
            else
                starname = "star_b";
            end
        end
    else
        
    end

    return starname;
end


return M;
--endregion
