/**
 * File: src/cell/common_player.cpp
 * Author: cxz <cxz@qq.com>
 * Date: 2017-06-30 10:53:19
 * Last Modified Date: 2017-06-30 10:53:19
 * Last Modified By: cxz <cxz@qq.com>
 */
#include "common_player.h"
#include "util.h"
#include "formation.h"
#include "common_room.h"
#include "log.h"
#include "csv/csv.h"
#include "sin_cos_map.h"
#include <boost/lexical_cast.hpp>
#include "plane/plane_push.pb.h"
#include "plane.h"
#include "rpc/rpc_helper.h"
#include "timer_queue/timer_queue_root.h"
#include "bullet_group.h"
#include "food.h"


const char LOG_NAME[] = "CommonPlayer";
CommonPlayer::CommonPlayer(const uint16_t serverid, const uint16_t gameClientID
        , const uint32_t uid, const std::string &acc
        , const std::string &nickname, const uint32_t skinid
        , const uint32_t bulletid, CommonRoom *pRoom, Formation &formation)
    : EntityWithQueue(uid, nickname) ,m_gameClientID(serverid, gameClientID), m_nickname(nickname)
      , m_skinid(skinid), m_bulletskinid(bulletid)
      , m_pRoom(pRoom)
      , m_isGod(true)
      , m_isStop(true)
      , m_formation(formation)
      , m_account(acc)
      , m_splitTimerID(0)
      , m_speedupTimerID(0)
      , m_killerID(0)
      , m_isOffline(true)
{
}

CommonPlayer::~CommonPlayer()
{
    onDestroy();
}

bool CommonPlayer::canAddPlane()
{
    if (isDead())
    {
        return false;
    }

    if (m_planes.size() >= m_pRoom->m_maxPlayerPlaneNumber)
    {
        return false;
    }

    return true;
}

// 先检查能不能添加飞机,再调用该函数添加飞机
uint32_t CommonPlayer::addOneSmallPlane()
{
    uint32_t planeid = m_pRoom->getNextEntityID();
    //m_planes.emplace_front(planeid, shared_from_this());
    m_planes.emplace(std::piecewise_construct,
            std::forward_as_tuple(planeid),
            std::forward_as_tuple(planeid, this));
    onSmallPlaneNumberChanged();
    return planeid;
}

void CommonPlayer::deleteOneSmallPlane(uint32_t planeid, bool isOutOfEdge, bool isFrameDelete)
{
    m_planes.erase(planeid);
    onSmallPlaneNumberChanged(isFrameDelete);

    // 如果出界,需要通知客户端删除飞机
    if (isOutOfEdge)
    {
        plane::PlaneBcMsg msg;
        msg.mutable_ms_and_id()->set_ms(0);
        msg.mutable_ms_and_id()->set_id(m_id);
        msg.mutable_new_()->set_plane_id(planeid);
        msg.mutable_new_()->set_move_speed(std::floor(m_moveSpeed * 100));
        m_pRoom->broadcast("plane.PlanePush", "RemovePlane", msg);
    }
}

void CommonPlayer::init()
{
    Pos pos = m_pRoom->randomEntityBornPos();
    m_pos.m_x = pos.m_x;
    m_pos.m_y = pos.m_y;
    setAngle(0);

    auto &tbl = CsvCfg::GetTable("param_SnowBallBasic.csv");
    const auto& cfg = tbl.GetRecord("key", "player_born_plane_num");
    m_bornPlaneNum = boost::lexical_cast<uint32_t>(cfg.GetString("value"));
    for (int i = 0; i < m_bornPlaneNum; ++i)
    {
        addOneSmallPlane();
    }

    const auto& godCfg = tbl.GetRecord("key", "god_last_seconds");
    m_godLastSeconds = boost::lexical_cast<double>(godCfg.GetString("value"));

    m_pTimerQueue->InsertSingleFromNow(0, [this]() { notifyGod(); });
    m_pTimerQueue->InsertSingleFromNow(std::floor(m_godLastSeconds* 1000), [this]() { notifyGod(true); });
}

void CommonPlayer::onReconnect()
{
    if (m_isGod)
    {
        m_pTimerQueue->InsertSingleFromNow(0, [this]() { notifyGod(); });
    }
}

void CommonPlayer::notifyGod(bool isGodOver)
{
    m_isGod = true;
    plane::StateMsg msg;
    msg.set_playerid(m_id);
    msg.set_state_num(plane::StateMsg::State_God);
    msg.set_op(plane::StateMsg::Op_On);
    if (isGodOver)
    {
        m_isGod = false;
        msg.set_op(plane::StateMsg::Op_Off);
    }
    m_pRoom->broadcast("plane.PlanePush", "NotifyStateChanged", msg);
}

void CommonPlayer::setScore(uint32_t score)
{
    m_gameData.m_oldScore = m_gameData.m_score;
    m_gameData.m_score = score;
    m_pRoom->m_rankManager.onPlayerScoreChanged(this);
}

void CommonPlayer::onKilled(CommonPlayer *pKiller)
{
    m_gameData.m_killNumber = 0;
    m_gameData.m_comboNumber = 0;
    m_isStop = true;
    m_splitTimerID = 0;
    const std::string nickname = pKiller ? pKiller->m_nickname : "";
    //LOG_INFO(Fmt("%d,%s,被%s杀死") % m_id % m_nickname % nickname);
    if (m_gameData.m_score != 0)
    {
        uint32_t minusScore = std::floor((m_gameData.m_score + 1) / 2);
        setScore(getRankScore() - minusScore);
        if (pKiller)
        {
            pKiller->setScore(pKiller->getRankScore() + minusScore);
        }
    }
    // 先注释掉,策划说感受太奇怪了
    /*
    if (pKiller)
    {
        pKiller->speedUp();
    }
    */
    m_pTimerQueue->InsertSingleFromNow(m_pRoom->m_reliveSeconds * 1000
            ,[this, nickname]()
            {
                notifyDeath(nickname, false);
            }
            );

    // 已经处理完击杀消息了,可以清除缓存了
    m_killerID = 0;
}

void CommonPlayer::speedUp()
{
    //如果玩家在分裂状态,判断分裂状态结束时间 >= x,若大于,则return;否则,移除定时器,进入加速状态,速度保持不变;
    if (isInSplit())
    {
        uint64_t leftMillSeconds = m_pTimerQueue->GetLeftMs(m_splitTimerID);
        if (leftMillSeconds >= m_pRoom->m_speedupLastMillSeconds)// do nothing
        {
            //LOG_DEBUG(Fmt("玩家正处于分裂状态,分裂剩余ms=%u>=%u") % leftMillSeconds % m_pRoom->m_speedupLastMillSeconds);
            return ;
        }
        else
        {
            // 速度不变,状态不变,不需要通知客户端新速度,移除飞机,添加能源飞机,改变移动状态
            m_pTimerQueue->Erase(m_splitTimerID);
            m_splitTimerID = 0;
            //LOG_DEBUG("玩家正处于分裂状态,移除分裂状态,开始加速");
            m_speedupTimerID = m_pTimerQueue->InsertSingleFromNow(m_pRoom->m_speedupLastMillSeconds
                    ,[this]()
                    {
                        onSpeedUpOver();
                    }
                    );
        }
    }
    else
    {
        // 在加速状态,移除旧加速,添加新加速状态
        if (isInSpeedUp())
        {
            m_pTimerQueue->Erase(m_speedupTimerID);
            //LOG_DEBUG("玩家不是分裂状态,处于加速状态,移除旧加速");
            m_speedupTimerID = m_pTimerQueue->InsertSingleFromNow(m_pRoom->m_speedupLastMillSeconds
                    ,[this]()
                    {
                        onSpeedUpOver();
                    }
                    );
        }
        // 不在分裂,也不在加速状态,通知客户端改变速度,加拖尾特效
        else
        {
            double newSpeed = std::floor(m_moveSpeed * getBuleltGroupLifetime() * 100) / 100;
            m_moveSpeed = newSpeed;
            onMoveSpeedChanged(true);
            //LOG_DEBUG(Fmt("玩家不是分裂状态,不是加速状态,通知客户端速度=%f") % m_moveSpeed);
            m_speedupTimerID = m_pTimerQueue->InsertSingleFromNow(m_pRoom->m_speedupLastMillSeconds
                    ,[this]()
                    {
                        onSpeedUpOver();
                    }
                    );
        }
    }

}

void CommonPlayer::onSpeedUpOver()
{
    //LOG_DEBUG("玩家加速状态结束");
    m_speedupTimerID = 0;
    m_moveSpeed = m_pRoom->getMoveSpeedByPlaneNumber(m_planes.size());
    onMoveSpeedChanged();
}

void CommonPlayer::onKillOtherPlayer()
{
    m_gameData.m_comboNumber += 1;
    m_gameData.m_destroyNumber += 1;
    if (m_gameData.m_comboNumber > m_gameData.m_highestComboNumber)
    {
        m_gameData.m_highestComboNumber = m_gameData.m_comboNumber;
    }
}

void CommonPlayer::notifyDeath(const std::string &name, bool isOutOfEdge)
{
    for (int i = 0; i < m_bornPlaneNum; ++i)
    {
        addOneSmallPlane();
    }
    //LOG_INFO(Fmt("玩家%d,%s复活,飞机数量=%u") % m_id % m_nickname %m_planes.size());

    plane::DeathMsg msg;
    msg.set_killedbyname(name);
    msg.set_relive_seconds(std::floor(m_godLastSeconds * 100));
    if (isOutOfEdge)
    {
        msg.set_is_out_of_bound(1);
    }
    else
    {
        msg.set_is_out_of_bound(0);
    }
    if (!isAI() && !m_isOffline)
    {
        RpcHelper::Request(m_gameClientID, "plane.PlanePush", "NotifyDeath", msg);
    }
    Pos bornPos = m_pRoom->randomEntityBornPos();
    m_pos.m_x = bornPos.m_x;
    m_pos.m_y = bornPos.m_y;
    m_pRoom->broadcastPlayerEnter(this);
    notifyGod();
    m_pTimerQueue->InsertSingleFromNow(std::floor(m_godLastSeconds* 1000), [this]() { notifyGod(true); });
}

void CommonPlayer::frameMove(double deltaSeconds)
{
    if (m_isOffline)
    {
        return ;
    }

    if (!m_isStop)
    {
        m_pos.m_x = m_pos.m_x + m_moveSpeed * deltaSeconds * m_dir.m_x;
        m_pos.m_y = m_pos.m_y + m_moveSpeed * deltaSeconds * m_dir.m_y;
        /*
        double movex = m_moveSpeed * deltaSeconds * m_dir.m_x;
        double movey = m_moveSpeed *deltaSeconds * m_dir.m_y;
        LOG_INFO(Fmt("%d,%s,move=(%f,%f),deltaseconds=%f,speed=%f,nowPos=(%f,%f)") 
                % m_id % m_nickname % movex % movey % deltaSeconds % m_moveSpeed % m_pos.m_x % m_pos.m_y);
                */
    }

    for (auto &plane : m_planes)
    {
        plane.second.frameMove(deltaSeconds);
    }

    // 检查下机群是否出界,若机群出界,判断具体哪个小飞机出界了
    // 判断机群出界的方法:
    // 若包裹圆圆心到矩形4条边中某条边的距离小于圆的半径,那么认为机群出界,再具体判断哪个飞机出界
}

Vector2 CommonPlayer::getPosAfterSeconds(double seconds)
{
    Vector2 dest{m_pos.m_x + m_moveSpeed * seconds * m_dir.m_x
        ,m_pos.m_y + m_moveSpeed * seconds * m_dir.m_y};

    return dest;
}
void CommonPlayer::onOffline()
{
    HandleMoveMsg(true, m_angle);
}

void CommonPlayer::HandleMoveMsg(bool is_stop, uint32_t angle)
{
    // 验证数值合法性
    uint32_t realAngle = std::floor(angle / 100);
    if (!(realAngle <= 359))
    {
        return ;
    }

    plane::StopOrMoveBeginBcMsg response;
    response.mutable_ms_and_id()->set_ms(0);
    response.mutable_ms_and_id()->set_id(m_id);
    response.mutable_info()->set_is_stop(is_stop);
    response.mutable_info()->set_angle(angle);
    m_pRoom->broadcast("plane.PlanePush", "PlayerStopOrMove", response);

    m_isStop = is_stop;
    if (!m_isStop)
    {
        setAngle(realAngle);
    }

    // 如果停止移动时,正处于分裂状态,那么停止分裂状态,并重新设置移动速度
    /*
    if (is_stop && isInSplit())
    {
        cancelSplit();
    }
    */
}
void CommonPlayer::HandleTurnToMsg(uint32_t angle)
{
    uint32_t realAngle = std::floor(angle / 100);
    if (!(realAngle <= 359))
    {
        return ;
    }

    setAngle(realAngle);
}

void CommonPlayer::HandleReqRankMsg()
{
    LOG_INFO("req rank");
    // 先从排行榜中取出前N名的数据,再获得我自己的排名得分,发送给客户端
    plane::RankDataMsg msg;
    msg.set_my_rank(m_pRoom->m_rankManager.getRankByID(m_id));
    msg.set_my_score(getRankScore());
    std::vector<const CommonPlayer*> topN;
    m_pRoom->m_rankManager.getRankData(rankShowNum, topN);
    for (size_t i = 0; i < topN.size(); ++i)
    {
        plane::RankData *pData = msg.add_data();
        pData->set_playerid(topN[i]->m_id);
        pData->set_playername(topN[i]->m_nickname);
    }
    if (!isAI())
    {
        RpcHelper::Request(m_gameClientID, "plane.PlanePush", "NotifyRankData", msg);
    }
}

uint32_t CommonPlayer::getRankScore() const
{
    return m_gameData.m_score;
}

uint32_t CommonPlayer::getRankOldScore() const
{
    return m_gameData.m_oldScore;
}

void CommonPlayer::onMoveSpeedChanged(bool isStartSplit)
{
    plane::UpdateSpeedBcMsg msg;
    msg.set_playerid(m_id);
    msg.set_speed(std::floor(m_moveSpeed * 100));
    if (isStartSplit)
    {
        msg.set_is_start_split(1);
    }
    //LOG_DEBUG(Fmt("速度改变,通知客户端,speed=%f,isStartSplit=%s") % m_moveSpeed % (isStartSplit ? "true" : "false"));
    m_pRoom->broadcast("plane.PlanePush", "UpdateSpeed", msg);
}

void CommonPlayer::HandleSplitMsg()
{
    LOG_INFO("split");
    if (m_planes.size() <= 1)
    {
        return;
    }

    uint64_t now = Util::getSystemMs();
    if((now - m_lastSplitTime) < (m_pRoom->m_splitCD -m_pRoom->m_shotDelaySeconds) * 999)
    {
	    LOG_INFO("split cd is not ok");
        return;
    }

    if (isInSplit())
    {
        m_pTimerQueue->Erase(m_splitTimerID);
        m_splitTimerID = 0;
        m_moveSpeed = m_pRoom->getMoveSpeedByPlaneNumber(m_planes.size());
        //处在分裂状态,再摁分裂,此时速度的基准值要变
    }

    uint32_t energy_skinid = std::floor((m_skinid + 9) / 10);

    // 移除一半飞机,添加等量的能源飞机
    uint32_t needRemovePlaneNum = (m_planes.size() + 1) / 2;
    plane::BatchAddEnergyPlaneBcMsg addenergy;
    plane::BatchRemovePlaneBcMsg removeplane;
    removeplane.set_playerid(m_id);
    addenergy.set_skinid(energy_skinid);
    addenergy.set_dir_angle(m_angle * 100);
    std::vector<FoodSharedPtr> ptrVec;
    auto it = m_planes.rbegin();
    auto nit = it;
    for (uint32_t i = 0; i < needRemovePlaneNum && it != m_planes.rend(); ++i, it = nit)
    {
        plane::OneBatchPlane *pPlane = addenergy.add_add_planes();
        const Vector2 pos = it->second.getWorldPos();
        FoodSharedPtr ptr = m_pRoom->createSplitPlane(pos.m_x, pos.m_y,m_skinid,m_angle, m_id);
        ptrVec.push_back(ptr);
        pPlane->set_plane_id(ptr->m_id);
        pPlane->set_x(std::floor(pos.m_x * 100));
        pPlane->set_y(std::floor(pos.m_y * 100));
        //LOG_INFO(Fmt("玩家分裂添加能源飞机,id=%d,pos=(%f,%f)") % ptr->m_id % pos.m_x % pos.m_y);
        removeplane.add_plane_ids(it->first);

        nit = decltype(it){m_planes.erase(std::next(it).base())};
    }

    m_pRoom->broadcast("plane.PlanePush", "BatchRemovePlane", removeplane);
    m_pRoom->broadcast("plane.PlanePush", "BatchAddEnergyPlane", addenergy);
    m_pRoom->batchCheckEnergy(ptrVec);

    // 设置新的移动速度
    double newSpeed = std::floor(m_moveSpeed * getBuleltGroupLifetime() * 100) / 100;
    m_moveSpeed = newSpeed;
    onMoveSpeedChanged(true);

    bool oldIsStop = m_isStop;
    if (m_isStop)
    {
        plane::StopOrMoveBeginBcMsg move;
        move.mutable_ms_and_id()->set_ms(m_pRoom->m_currentFrame);
        move.mutable_ms_and_id()->set_id(m_id);
        move.mutable_info()->set_is_stop(false);
        move.mutable_info()->set_angle(m_angle);
        m_pRoom->broadcast("plane.PlanePush", "PlayerStopOrMove", move);
    }

    m_splitTimerID = m_pTimerQueue->InsertSingleFromNow(m_pRoom->m_splitLastMillSeconds
            ,[this, oldIsStop]()
            {
                onSplitTimeOver(oldIsStop);
            }
            );
    m_isStop = false;

    m_lastSplitTime = now;
}

void CommonPlayer::onSplitTimeOver(bool isStop)
{
    m_splitTimerID = 0;
    m_moveSpeed = m_pRoom->getMoveSpeedByPlaneNumber(m_planes.size());
    onMoveSpeedChanged();

    // 如果分裂期间收到客户端的移动相关消息,那就不用还原分裂前的移动状态
    // 没收到的话,分裂前是静止状态,现在还是静止状态
    if (!m_receiverStopMsgWhenSplit && isStop)
    {
        m_isStop = isStop;
        plane::StopOrMoveBeginBcMsg move;
        move.mutable_ms_and_id()->set_ms(m_pRoom->m_currentFrame);
        move.mutable_ms_and_id()->set_id(m_id);
        move.mutable_info()->set_is_stop(true);
        m_pRoom->broadcast("plane.PlanePush", "PlayerStopOrMove", move);
    }
}

void CommonPlayer::cancelSplit()
{
        m_pTimerQueue->Erase(m_splitTimerID);
        m_splitTimerID = 0;
        m_moveSpeed = m_pRoom->getMoveSpeedByPlaneNumber(m_planes.size());
        // 通知客户端玩家移动速度改变
        onMoveSpeedChanged();
}

void CommonPlayer::onSmallPlaneNumberChanged(bool isFrameDelete)
{
    // 计算新的速度
    size_t size = m_planes.size();
    // 如果不在分裂状态且不再加速状态,需要改变速度
    if (!isInSplit() && !isInSpeedUp())
    {
        m_moveSpeed = m_pRoom->getMoveSpeedByPlaneNumber(size);
        //如果不是帧击中删除飞机,需要通知客户端
        if (!isFrameDelete)
        {
            onMoveSpeedChanged();
        }
    }
    uint32_t i = 0;
    for (auto &plane : m_planes)
    {
        plane.second.setDestPos(m_formation.getFormation()[size][i]);
        ++i;
    }
    m_wrapRadius = m_formation.getWrapRadius(size);
    m_shotCD = m_pRoom->getShotCDByPlaneNumber(size);
}
void CommonPlayer::onSmallPlaneOutOfEdgeTimeout(uint32_t planeid)
{
    //deleteOneSmallPlane(planeid);
    //plane::PlaneBcMsg msg;
    //m_pRoom->
}

void CommonPlayer::HandleSmallPlaneDieMsg(uint32_t planeid)
{
    auto it = m_planes.find(planeid);
    if (it == m_planes.end())
    {
        return ;
    }

    deleteOneSmallPlane(planeid, true, false);
    if (isDead())
    {
        if (m_gameData.m_score != 0)
        {
            uint32_t minusScore = std::floor((m_gameData.m_score + 1) / 2);
            setScore(getRankScore() - minusScore);
        }
        Pos bornPos = m_pRoom->randomEntityBornPos();
        m_pos.m_x = bornPos.m_x;
        m_pos.m_y = bornPos.m_y;
        const std::string &nickname = m_nickname;
        m_pTimerQueue->InsertSingleFromNow(m_pRoom->m_reliveSeconds * 1000
                ,[this, &nickname]()
                {
                    notifyDeath(nickname, true);
                }
                );
    }
}

void CommonPlayer::setAngle(uint32_t angle)
{
    m_angle = angle;
    m_dir.m_x = -sinMap[m_angle];
    m_dir.m_y = cosMap[m_angle];
}

void CommonPlayer::generateBullets(BulletGroup &group)
{
    for (auto &p : m_planes)
    {
        uint32_t bulletid = m_pRoom->getNextEntityID();
        group.m_map.emplace(std::piecewise_construct,
                std::forward_as_tuple(bulletid),
                std::forward_as_tuple(bulletid, p.second.m_id, p.second.m_pos, group));
    }
}

double CommonPlayer::getBuleltGroupLifetime()
{
    double lifeTime = m_pRoom->m_lifeTimeK * 
        std::pow(m_planes.size(), 1.0 / m_pRoom->m_lifeTimeA) + m_pRoom->m_lifeTimeB;

    if (isInSplit() || isInSpeedUp())
    {
        double normalSpeed = m_pRoom->getMoveSpeedByPlaneNumber(m_planes.size());
        lifeTime = lifeTime * normalSpeed / m_moveSpeed * m_pRoom->m_splitLifeTimeCoef;
    }

    return lifeTime;
}

void CommonPlayer::HandleFireMsg()
{
    shoot();
}

void CommonPlayer::shoot()
{
    if(isDead())
    {
        return;
    }

    uint64_t now = Util::getSystemMs();
    if((now - m_lastShotTime) < (m_shotCD -m_pRoom->m_shotDelaySeconds) * 1000)
    {
	    LOG_INFO("shot cd is not ok");
        return;
    }

    m_pRoom->newBulletGroup(this);

    m_lastShotTime = now;
}

bool CommonPlayer::isInSplit()
{
    return m_splitTimerID != 0;
}

void CommonPlayer::onDestroy()
{
    EntityWithQueue::onDestroy();
    // 重置 rpc 路由
}
