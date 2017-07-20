#pragma once

#include "common_room.h"
#include "team.h"
#include <unordered_set>
#include <vector>

//class Room : public CommonRoom, public std::enable_shared_from_this<Room>{
class Room : public CommonRoom{
    public:
        Room(const uint32_t id);
        ~Room(){};
};

namespace svr
{
    class TeamData;
};

class TeamPlayer;
using TeamSharedPtr = std::shared_ptr<Team>;
// 组队模式房间
class TeamRoom : public CommonRoom{
    private:
        // 所有本房间团队皮肤id集合
        std::unordered_set<uint32_t> m_skinids;
        std::map<uint32_t, TeamSharedPtr> m_teams;
        uint32_t m_maxTeamNumber;

    public:
        OneRoomRankManager<Team> m_teamRankManager;
        void init();
        /*
         * 判断这个队伍能否进本房间,判断的规则是:
         * 1.先判断房间是否到了禁止加入时间了,若到了,不能进
         * 2.根据队伍人数分别判断,若队伍有3人,看能否找到一个AI队伍,找到,就用该队数据顶替AI队伍
         * 3.若队伍人数=2,能否找到一个AI队伍,若能,用该队数据替换AI数据;若找不到,判断是否能找到人数为1的玩家队伍
         * 4.若队伍人数=1,能否找到一个玩家队伍,剩余人数为2,是否能找到一个AI队伍,是否能找到剩余人数为1的玩家队伍
         * 若能进,则在该函数中创建 TeamPlayer
         */
        bool canThisTeamInRoom(std::string teamdata);
        bool isEnemy(CommonPlayer *pPlayer1, CommonPlayer *pPlayer2);
        std::string playerReconnect(const uint16_t serverid, const uint64_t gameClientID, const uint32_t uid);
        TeamRoom(const uint32_t id);

        // 踢出该队伍
        void kickAITeam(const Team *pTeam);
        // 创建一个AI队伍
        void createAITeam(const uint32_t teamid, const uint32_t skinid, const uint32_t leaderid);
        // 创建一个玩家队伍
        void createPlayerTeam(const uint32_t skinid, svr::TeamData &teaminfo);
        void appendTeamToPlayerTeam(Team *pTeam, const uint32_t appendTeamSize, svr::TeamData &teaminfo);
        // 获得第一个AI队伍
        Team* getFirstAITeam();

        // 在EnterRoomResponse 之后立即通知客户端队伍信息
        void notifyTeamInfo(const uint32_t playerid);

        // 通知玩家排行榜数据
        void notifyAllPlayerRankData();
        void notifyAllPlayerRankData(TeamPlayer *pPlayer);

        void getAllPlayerRankData(std::vector<LuaRankData> &ret);
};
