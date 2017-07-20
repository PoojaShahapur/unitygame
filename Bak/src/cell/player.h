#pragma once

#include "common_player.h"

class Player : public CommonPlayer
{
    public:
    Player(const uint16_t serverid, const uint16_t gameClientID, const std::string &account, const uint32_t uid
        , const std::string &nickname, const uint32_t skinid, const uint32_t bulletid, CommonRoom *pRoom);

    /*
    std::shared_ptr<Player> shared_from_this()
    {
        return std::static_pointer_cast<Player>(CommonPlayer::shared_from_this());
    }
    */
};
using PlayerSharedPtr = std::shared_ptr<Player>;

class Team;
class TeamPlayer : public CommonPlayer{
    public:
        Team *m_pTeam;
        TeamPlayer(const uint16_t serverid, const uint16_t gameClientID, const std::string &account, const uint32_t uid
        , const std::string &nickname, const uint32_t skinid, const uint32_t bulletid, CommonRoom *pRoom);

        void setScore(uint32_t score);
        void HandleReqRankMsg();
};
