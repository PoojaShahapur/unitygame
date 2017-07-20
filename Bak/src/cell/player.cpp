#include "player.h"
#include "formation.h"
#include "plane.h"
#include "timer_queue/timer_queue_root.h"
#include "team.h"
#include "normal_room.h"


Player::Player(const uint16_t serverid, const uint16_t gameClientID
        , const std::string &acc, const uint32_t uid
        , const std::string &nickname, const uint32_t skinid
        , const uint32_t bulletid, CommonRoom* pRoom)
    : CommonPlayer(serverid, gameClientID, uid, acc, nickname
            , skinid, bulletid, pRoom, CircleFormation::get_mutable_instance())
{}

TeamPlayer::TeamPlayer(const uint16_t serverid, const uint16_t gameClientID
        , const std::string &acc, const uint32_t uid
        , const std::string &nickname, const uint32_t skinid
        , const uint32_t bulletid, CommonRoom* pRoom)
    : CommonPlayer(serverid, gameClientID, uid, acc, nickname
            , skinid, bulletid, pRoom, CircleFormation::get_mutable_instance())
{}

void TeamPlayer::setScore(uint32_t score)
{
    m_gameData.m_oldScore = m_gameData.m_score;
    m_gameData.m_score = score;
    m_pTeam->reCalculateTeamScore();
    TeamRoom *pTeamRoom = (TeamRoom*)m_pRoom;
    pTeamRoom->m_teamRankManager.onPlayerScoreChanged(m_pTeam);
}

void TeamPlayer::HandleReqRankMsg()
{
    TeamRoom *pTeamRoom = (TeamRoom*)m_pRoom;
    pTeamRoom->notifyAllPlayerRankData(this);
}
