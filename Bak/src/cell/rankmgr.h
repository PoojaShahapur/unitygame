#pragma once
#include <map>
#include <vector>
#include <functional>  // for greater<>,windows下编译需要

/*
 * 房间排行榜类,该类主要用处:
 * 根据玩家得分(场内积分,杀敌数等等),维护一个有序列表.
 * 玩家进出房间时候,重新计算排名;
 * 查询排名;
 * 比赛结束后,发放奖励.
 *
 * 排行榜需要及时刷新吗?
 * lua版本的排行榜,每次玩家进出,加减分, 都会重新计算排名,如果排行榜上排名发生变化,就通知客户端.
 * 感觉lua版本的消息量太大,而且没必要,c++版本优化如下:
 * 玩家进入,离开,得分变化更新排行榜
 * 每1秒通知下客户端排行榜数据
 *
 * 排行榜的数据结构:
 * std::map<uint64_t, std::string>
 * 其中key为 (score << 32 | entityid),只用score作为key会重复,导致插入不进map
 * value位本局昵称名
 *
 * todo : 完全不需要发昵称给客户端,浪费带宽,修改为uid
 */

// 排行榜上显示的排名条数
const uint32_t rankShowNum = 8;

template <class RankObj>
class OneRoomRankManager{
    private:
        // 指针会非法吗?
        // 玩家进入时候指针被 make_shared 构造出来,离开房间析构之前,会先从排行榜中移除
        // 所以,排行榜中指针都是有效的
        std::map<uint64_t, const RankObj*, std::greater<uint64_t>> m_data;
    public:
        // 玩家进入,插入map
        void onPlayerEnter(const RankObj *pPlayer);
        // 玩家离开,从map移除
        void onPlayerLeave(const RankObj *pPlayer);
        // 玩家得分变化
        void onPlayerScoreChanged(const RankObj *pPlayer);
        // 返回排行榜前N名数据
        void getRankData(uint32_t N, std::vector<const RankObj*> &ret);
        // 通过id取到玩家的排名
        uint32_t getRankByID(uint32_t id);
};
