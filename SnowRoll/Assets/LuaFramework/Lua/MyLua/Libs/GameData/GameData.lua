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
end

function M:dtor()

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

    local form = GCtx.mUiMgr:getForm(GlobalNS.UIFormID.eUIRankListPanel);
    if nil ~= form and form.mIsReady then
        form:updateUIData();
    end
end

return M;
--endregion
