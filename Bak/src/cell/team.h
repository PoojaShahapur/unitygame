#pragma once
#include <set>
#include <assert.h>

// 一个队伍,目前队伍里要么全是AI,要么全是玩家
struct Team{
    uint32_t m_score;
    uint32_t m_oldScore;
    uint32_t m_id;
    uint32_t m_skinid;
    uint32_t m_leaderUid;
    uint32_t m_totalNum;
    std::string m_voiceToken;

    //队伍成员,用raw pointer是可以的
    //因为对于玩家,现在玩家直到房间销毁时,CommonPlayer才被析构
    //当AI玩家队伍被顶替掉时候,
    std::set<CommonPlayer*> m_members;

    Team(const uint32_t id, const uint32_t leaderid, const uint32_t total
            , const uint32_t skinid, const std::string &token = "")
        : m_id(id), m_leaderUid(leaderid), m_totalNum(total)
        , m_skinid(skinid)
        , m_voiceToken(token)
          , m_score(0), m_oldScore(0)
    {
        m_members.clear();
    }

    // 若队伍里第1个玩家是AI,那么该队伍即为AI队伍
    bool isAITeam() const
    {
        if (m_members.size() <= 0)
        {
            assert(false);
        }

        auto it = m_members.begin();
        if ((*it)->isAI())
        {
            return true;
        }
        return false;
    }

    // 当队伍里有玩家积分变化时候
    // 重新计算队伍积分=队伍所有成员积分之和
    uint32_t reCalculateTeamScore()
    {
        m_oldScore = m_score;
        uint32_t newScore = 0;
        for (const auto &pPlayer : m_members)
        {
            newScore += pPlayer->getRankScore();
        }
        m_score = newScore;
    }

    uint32_t getRankScore() const {return m_score;}
    uint32_t getRankOldScore() const {return m_oldScore;}
};
