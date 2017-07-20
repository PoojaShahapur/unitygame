#include "rankmgr.h"
#include "common_player.h"
#include "team.h"

template class OneRoomRankManager<CommonPlayer>;
template class OneRoomRankManager<Team>;
template <class RankObj>
void OneRoomRankManager<RankObj>::onPlayerEnter(const RankObj *pPlayer)
{
    m_data[((uint64_t)pPlayer->getRankScore()) << 32 | pPlayer->m_id] = pPlayer;
}

template <class RankObj>
void OneRoomRankManager<RankObj>::onPlayerLeave(const RankObj *pPlayer)
{
    m_data.erase(((uint64_t)pPlayer->getRankScore()) << 32 | pPlayer->m_id);
}

template <class RankObj>
void OneRoomRankManager<RankObj>::onPlayerScoreChanged(const RankObj *pPlayer)
{
    // 删掉旧数据,插入新数据
    m_data.erase(((uint64_t)pPlayer->getRankOldScore()) << 32 | pPlayer->m_id);
    onPlayerEnter(pPlayer);
}

template <class RankObj>
uint32_t OneRoomRankManager<RankObj>::getRankByID(uint32_t id)
{
    uint32_t rank = 1;
    for (auto &it : m_data)
    {
        if (it.second->m_id == id)
        {
            return rank;
        }
        ++rank;
    }

    // 出错了
    return 0;
}

template <class RankObj>
void OneRoomRankManager<RankObj>::getRankData(uint32_t N, std::vector<const RankObj*> &ret)
{
    auto it = m_data.begin();
    uint32_t i = 1;
    for (; i <= N && it != m_data.end(); ++i, ++it)
    {
        ret.push_back(it->second);
    }
}
