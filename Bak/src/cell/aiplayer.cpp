#include "aiplayer.h"
#include "common_room.h"
#include "timer_queue/timer_queue.h"
#include "log.h"
#include "util.h"
#include "normal_room.h"
#include <cmath>

const double PI  = std::acos(-1);
const char LOG_NAME[] = "CommonPlayer";
AIPlayer::AIPlayer( const uint32_t uid, const std::string &acc
    , const std::string &nickname, const uint32_t skinid
    , const uint32_t bulletid, CommonRoom *pRoom, Formation &formation)
    : CommonPlayer(Util::GetMySvrId(), 0, uid, acc, nickname, skinid, bulletid, pRoom, formation)
      , m_searchRange(144)
      , m_shotcdTimes(3)
      , m_alreadySetNextAngle(false)
      , m_nextAngle(0)
      , m_state(AIState_Eat)
      , m_shotTimerID(0)
      , m_enemyPlayerID(0)
      , m_attackChangeDirTimerID(0)
      , m_searchEnemyTimerID(0)
      , m_findEnemyTriggerTimes(0)
{
    //LOG_INFO(Fmt("AIPlayer::AIPlayer,ID=%u") % m_id);
    m_isStop = false;
    m_isOffline = false;
}

AIPlayer::~AIPlayer()
{
    onDestroy();
}

void AIPlayer::init()
{
    CommonPlayer::init();
    m_shotTimerID = m_pTimerQueue->InsertRepeatFromNow(0, m_shotcdTimes * m_shotCD * 1000
        , [this]() { shoot(); });
    m_changeDirTimerID = m_pTimerQueue->InsertRepeatFromNow(0, 1500
        , [this]() { changeDirOnTimer(); });
    m_searchEnemyTimerID = m_pTimerQueue->InsertRepeatFromNow(0, 500
        , [this]() { findNearestEnemy(); });
    m_shotCDWhenTimerCreated = m_shotCD;
}

bool AIPlayer::isCloseToEdge(const Vector2 &pos)
{
    if (pos.m_x <= m_pRoom->m_min.m_x + 5 || pos.m_x >= m_pRoom->m_max.m_x - 5
            || pos.m_y <= m_pRoom->m_min.m_y + 5 || pos.m_y >= m_pRoom->m_max.m_y - 5)
    {
        return true;
    }

    return false;
}
void AIPlayer::frameMove(double deltaSeconds)
{
    CommonPlayer::frameMove(deltaSeconds);
    if (!m_alreadySetNextAngle && isCloseToEdge(m_pos))
    {
        m_alreadySetNextAngle = true;
        m_nextAngle = (m_angle + 180) % 360;
	    //LOG_INFO(Fmt("AI要出界啦,m_angle=%u,设置新的角度为%u") % m_angle % m_nextAngle);
    }

    if (std::abs(m_shotCDWhenTimerCreated - m_shotCD) > 0.1)
    {
        m_shotCDWhenTimerCreated = m_shotCD;
        m_pTimerQueue->Erase(m_shotTimerID);
        m_shotTimerID = m_pTimerQueue->InsertRepeatFromNow(0, m_shotcdTimes * m_shotCD * 1000
                , [this]() {shoot();});
    }
}

void AIPlayer::notifyDeath(const std::string &name, bool isOutOfEdge)
{
    CommonPlayer::notifyDeath(name, isOutOfEdge);
    m_isStop = false;
}

// 设置搜索范围,重新设置射击CD
void AIPlayer::onStageChanged(uint32_t searchRange, uint32_t shotcdTimes)
{
    m_searchRange = searchRange;
    m_shotcdTimes = shotcdTimes;
    m_pTimerQueue->Erase(m_shotTimerID);
    m_shotTimerID = m_pTimerQueue->InsertRepeatFromNow(0, m_shotcdTimes * m_shotCD * 1000
            , [this]() {shoot();});
}

void AIPlayer::changeDirOnTimer()
{
    if (isDead() || m_state == AIState_Attack)
    {
        return ;
    }

    if (m_nextAngle != 0)
    {
        setAngle(m_nextAngle);
	    LOG_INFO(Fmt("设置新的角度为%u") % m_nextAngle);
        m_nextAngle = 0;
    }
    else
    {
        m_alreadySetNextAngle = false;
        setAngle(rand() % 360);
    }
}

void AIPlayer::findNearestEnemy()
{
    ++m_findEnemyTriggerTimes;
    if (10 == m_findEnemyTriggerTimes)
    {
        m_enemyPlayerID = m_pRoom->getNearestPlayerid(this, m_searchRange);
        m_findEnemyTriggerTimes = 0;
        return ;
    }
    if (isDead() || m_state == AIState_Attack)
    {
        return ;
    }

    uint32_t destPlayerid = m_pRoom->getNearestPlayerid(this, m_searchRange);
    if (0 != destPlayerid)
    {
        changeState(AIState_Attack, destPlayerid);
    }
}

void AIPlayer::changeState(uint32_t state, uint32_t enemyid)
{
    if (AIState_Attack == state)
    {
        m_enemyPlayerID = enemyid;
        m_attackChangeDirTimerID = m_pTimerQueue->InsertRepeatFromNow(0, 500
            , [this]() { changeDirWhenAttack(); });
    }
    else
    {
        m_enemyPlayerID = 0;
        if (0 != m_attackChangeDirTimerID)
        {
            m_pTimerQueue->Erase(m_attackChangeDirTimerID);
        }
        m_attackChangeDirTimerID = 0;
        m_searchEnemyTimerID = m_pTimerQueue->InsertRepeatFromNow(0, 500
            , [this]() { findNearestEnemy(); });
    }
    m_state = state;
}

void AIPlayer::onKilled(CommonPlayer *pKiller)
{
    CommonPlayer::onKilled(pKiller);
    changeState(AIState_Eat);
}

void AIPlayer::changeDirWhenAttack()
{
    if (isDead() || m_enemyPlayerID == 0)
    {
        return ;
    }

    CommonPlayer *pPlayer = m_pRoom->getPlayerByID(m_enemyPlayerID);
    if (!pPlayer || pPlayer->isDead())
    {
        changeState(AIState_Eat);
        return;
    }

    Vector2 dest = pPlayer->getPosAfterSeconds(0.1);
    if (dest.m_x == m_pos.m_x && dest.m_y == m_pos.m_y)
    {
        return ;
    }

    double rad = std::atan2(dest.m_y - m_pos.m_y, dest.m_x - m_pos.m_x) - PI / 2;
    int angle = std::floor(rad * 180 / PI);
    if (angle < 0)
    {
        angle += 360;
    }
    setAngle(angle);
}

TeamAIPlayer::TeamAIPlayer(const uint32_t uid, const std::string &acc
    , const std::string &nickname, const uint32_t skinid
    , const uint32_t bulletid, CommonRoom *pRoom, Formation &formation)
    : AIPlayer(uid, acc, nickname, skinid, bulletid, pRoom, formation)
      , m_pTeam(nullptr)
{}

void TeamAIPlayer::setScore(uint32_t score)
{
    m_gameData.m_oldScore = m_gameData.m_score;
    m_gameData.m_score = score;
    m_pTeam->reCalculateTeamScore();
    TeamRoom *pTeamRoom = (TeamRoom*)m_pRoom;
    pTeamRoom->m_teamRankManager.onPlayerScoreChanged(m_pTeam);
}
