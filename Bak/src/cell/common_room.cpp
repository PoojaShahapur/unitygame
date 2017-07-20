#include <random>
#include <boost/lexical_cast.hpp>
#include <tuple>

#include "log.h"
#include "util.h"
#include "common_room.h"
#include "timer_queue/timer_queue_root.h"
#include "bullet_group.h"
#include "plane/plane.pb.h"
#include "plane/team.pb.h"
#include "plane/plane_push.pb.h"
#include "pb/svr/run_lua.pb.h"
#include "player.h"
#include "aiplayer.h"
#include "csv/csv.h"  // for CsvCfg::Init()
#include "rpc/rpc_helper.h"
#include "formation.h"
#include "plane.h"
#include "sin_cos_map.h"


//#include "plane.h"
const char LOG_NAME[] = "CSvcPlane";
AllPlayerMap CommonRoom::m_playerSharedPtrMap;

CommonRoom::CommonRoom(const uint32_t id) 
    : EntityWithQueue(id, ""), m_currentFrame(0), m_nextEntityID(1), m_holdPlayerNum(0)
      , m_needCreateEnergyNumber(0)
      , m_min(32, 32), m_max(288-32, 288-32)
      , m_roomMode(RoomMode_Normal)
{
    init();
}

CommonRoom::~CommonRoom()
{
    LOG_INFO(Fmt("roomid=%d,CommonRoom::~CommonRoom") % m_id);
    onDestroy();
}

CommonPlayer *CommonRoom::getPlayerByID(uint32_t id)
{
    auto it = m_players.find(id);
    if (it == m_players.end())
    {
        return nullptr;
    }

    return it->second.get();
}

void CommonRoom::playerEnter(const uint16_t serverid, const uint64_t gameClientID, const std::string &account
        , const uint32_t uid, const std::string nickname, const uint32_t skinid, const uint32_t bulletid)
{
	LOG_INFO(Fmt("playerEnter.base=%u,clientid=%lu,acc=%s,uid=%d") % serverid % gameClientID % account % uid);
    if (m_players.size() == 0)
    {
        for (uint32_t i = 1; i < m_holdPlayerNum; ++i)
        {
            addOneAIPlayer();
        }
        if (m_roomMode != RoomMode_Team)
        {
            m_voiceToken = std::to_string(Util::genObjectID());
        }
    }
    else
    {
        eraseOneAIPlayer();
    }
    /*
    PlayerSharedPtr pPlayer = std::make_shared<Player>(serverid, gameClientID
            , account, uid, nickname, skinid, bulletid, this);
            */
    CommonPlayerSharedPtr pPlayer = std::make_shared<Player>(serverid, gameClientID
            , account, uid, nickname, skinid, bulletid, this);
    m_playerSharedPtrMap[pPlayer->m_gameClientID.ToString()] = pPlayer;
    pPlayer->init();

    // 广播其他玩家,有人进来啦
    broadcastPlayerEnter(pPlayer.get());
    // 通知玩家进入无敌状态

    m_players[uid] = pPlayer;
    m_rankManager.onPlayerEnter(pPlayer.get());
    refreshBulletEnemiesOnPlayerEnter(pPlayer.get());
}

std::string CommonRoom::playerReconnect(const uint16_t serverid, const uint64_t gameClientID, const uint32_t uid)
{
    auto it = m_players.find(uid);
    if (it == m_players.end())
    {
        return "";
    }
    it->second->m_gameClientID = CGameCltId(serverid, gameClientID);
    m_playerSharedPtrMap[it->second->m_gameClientID.ToString()] = it->second;
    it->second->m_isOffline = false;
    it->second->onReconnect();
    // 返回 response 给客户端
    plane::EnterRoomResponse response;
    response.mutable_ms_and_id()->set_ms(m_currentFrame);
    response.mutable_ms_and_id()->set_id(uid);
    response.set_leftseconds(getGameOverLeftSeconds());
    response.set_mode(m_roomMode);
    response.set_voice_token(m_voiceToken);
    fulfillPlayersEnterRoomResponse(response);
    fulfillFoodsEnterRoomResponse(response);
    return response.SerializeAsString();
}

void CommonRoom::fulfillPlayersEnterRoomResponse(plane::EnterRoomResponse &response)
{
    // 填充所有玩家的数据
    for (const auto &playerIt : m_players)
    {
        plane::PlayerInfo *pInfo = response.add_players();
        pInfo->set_id(playerIt.second->m_id);
        pInfo->set_name(playerIt.second->m_nickname);
        pInfo->set_speed(std::floor(playerIt.second->getMoveSpeed() * 100));
        pInfo->set_skinid(playerIt.second->m_skinid);
        pInfo->set_bulletskinid(playerIt.second->m_bulletskinid);
        pInfo->set_random_formation_id(playerIt.second->m_formation.getFormationID());
        pInfo->mutable_move()->set_angle(std::floor(playerIt.second->m_angle * 100));
        pInfo->mutable_move()->set_score(playerIt.second->getRankScore());
        pInfo->mutable_move()->set_x(std::floor(playerIt.second->m_pos.m_x * 100));
        pInfo->mutable_move()->set_y(std::floor(playerIt.second->m_pos.m_y * 100));

        for (const auto &plane : playerIt.second->m_planes)
        {
            pInfo->mutable_move()->add_small_plane_ids(plane.first);
        }
    }
}

void CommonRoom::fulfillFoodsEnterRoomResponse(plane::EnterRoomResponse &response)
{
    // 填充所有食物的数据,以及能源飞机数据
    // 注意,这个map将所有A玩家分裂出的能源飞机放一起了
    // ,会有BUG:假设A玩家分裂很多次,B进入时,发现所有A玩家的能源飞机是一个朝向.实际上并不是
    // 但这个BUG能容忍,毕竟进去之后看到能源飞机的朝向不同,并不会对战局有任何影响
    std::map<uint32_t, std::vector<FoodSharedPtr>> player2Foods;
    for (const auto &foodIt: m_foodSharedPtrMap)
    {
        if (foodIt.second->m_ftype == FoodType_SplitPlane)
        {
            player2Foods[foodIt.second->m_id].push_back(foodIt.second);
        }
        else
        {
            plane::FoodMsg *pFood = response.add_foods();
            pFood->set_food_id(foodIt.second->m_id);
            pFood->set_x(std::floor(foodIt.second->m_pos.m_x * 100));
            pFood->set_y(std::floor(foodIt.second->m_pos.m_y * 100));
        }
    }

    for (const auto &energyIt : player2Foods)
    {
        plane::BatchAddEnergyPlaneBcMsg *pEnergy = response.add_planes();
        pEnergy->set_skinid(energyIt.second[0]->m_skinid);
        pEnergy->set_dir_angle(energyIt.second[0]->m_angle * 100);
        //遍历填充每个小飞机
        for (FoodSharedPtr ptr : energyIt.second)
        {
            plane::OneBatchPlane *pSmallPlane = pEnergy->add_add_planes();
            pSmallPlane->set_plane_id(ptr->m_id);
            pSmallPlane->set_x(std::floor(ptr->m_pos.m_x * 100));
            pSmallPlane->set_y(std::floor(ptr->m_pos.m_y * 100));
        }
    }
}

void CommonRoom::playerExit(CommonPlayerSharedPtr pPlayer)
{
    onPlayerExit(pPlayer);

    svr::ExitRoomMsg exitmsg;
    exitmsg.set_account(pPlayer->m_account);
    exitmsg.set_player_server_id(Util::GetMySvrId());
    exitmsg.set_room_id(m_id);
    exitmsg.set_uid(pPlayer->m_id);
    RpcHelper::RequestSvr(sessionServerID, "svr.RunLua", "ExitRoom", exitmsg);

    LOG_INFO(Fmt("%u,%s,%s退出房间id=%u,剩余玩家=%u个") 
            % pPlayer->m_id % pPlayer->m_account % pPlayer->m_nickname % m_id % m_players.size());

    CommonRoom::m_playerSharedPtrMap.erase(pPlayer->m_gameClientID.ToString());
    pPlayer->m_gameClientID = CGameCltId(0, 0);
    pPlayer->m_isOffline = true;
}

void CommonRoom::onPlayerExit(CommonPlayerSharedPtr pPlayer)
{
    // 清除排行榜数据
    m_rankManager.onPlayerLeave(pPlayer.get());
    m_players.erase(pPlayer->m_id);
    // 通知房间内其他玩家,该玩家退出游戏
    plane::MsAndId exit;
    exit.set_ms(m_currentFrame);
    exit.set_id(pPlayer->m_id);
    broadcast("plane.PlanePush", "PlayerExit", exit);
    // 添加AI玩家
    if (!pPlayer->isAI())
    {
        addOneAIPlayer();
    }
}

void CommonRoom::eraseOneAIPlayer()
{
    for (const auto &it : m_players)
    {
        if (it.second->isAI())
        {
            onPlayerExit(it.second);
            break;
        }
    }
}

void CommonRoom::addOneAIPlayer()
{
            std::string name("ai_");
            uint32_t entityID = getNextEntityID();
            name += std::to_string(entityID);
            CommonPlayerSharedPtr pPlayer = std::make_shared<AIPlayer>(
                    entityID, name, name, 1, 20001, this, HexagonFormation::get_mutable_instance());
            pPlayer->init();
            m_players[entityID] = pPlayer;
            m_rankManager.onPlayerEnter(pPlayer.get());
            broadcastPlayerEnter(pPlayer.get());
}

void CommonRoom::init()
{
    uint64_t millSeconds = Util::getSystemMs();
    m_roomCreateMillSeconds = millSeconds;
    m_playersLastUpdateMillSeconds = millSeconds;
    m_bulletGroupsLastUpdateMillSeconds = millSeconds;
    auto &tbl = CsvCfg::GetTable("param_Common.csv");
    const auto& cfg = tbl.GetRecord("key", "forbid_join_seconds");
    m_forbidJoinSeconds = boost::lexical_cast<uint32_t>(cfg.GetString("value"));

    m_energyNumber = boost::lexical_cast<uint32_t>(tbl.GetRecord("key", "food_num").GetString("value"));
    m_needShotNumber = boost::lexical_cast<uint32_t>(tbl.GetRecord("key", "need_shot_num").GetString("value"));
    m_roomEndMillSeconds = boost::lexical_cast<uint32_t>(tbl.GetRecord("key", "room_last_seconds")
            .GetString("value")) * 1000 + m_roomCreateMillSeconds;
    m_holdPlayerNum = boost::lexical_cast<uint32_t>(tbl.GetRecord("key", "room_hold_num").GetString("value"));
	LOG_INFO(Fmt("roomid=%u,init,createms=%lu,forbid_join_seconds=%u,holdnum=%u")
            % m_id % m_roomCreateMillSeconds % m_forbidJoinSeconds % m_holdPlayerNum);
    LOG_INFO(Fmt("energynum=%u") % m_energyNumber);
    auto &basicTbl = CsvCfg::GetTable("param_SnowBallBasic.csv");
    m_moveSpeedK = boost::lexical_cast<double>(basicTbl.GetRecord("key", "MoveSpeed_k").GetString("value"));
    m_moveSpeedB = boost::lexical_cast<double>(basicTbl.GetRecord("key", "MoveSpeed_b").GetString("value"));
    m_moveSpeedA = boost::lexical_cast<double>(basicTbl.GetRecord("key", "MoveSpeed_a").GetString("value"));
    m_shotMinSeconds = boost::lexical_cast<double>(basicTbl.GetRecord("key", "shot_minseconds").GetString("value"));
    m_shotMaxSeconds = boost::lexical_cast<double>(basicTbl.GetRecord("key", "shot_maxseconds").GetString("value"));
    m_shotAddCoef = boost::lexical_cast<double>(basicTbl.GetRecord("key", "shot_add_coef").GetString("value"));
    m_shotDelaySeconds = boost::lexical_cast<double>(basicTbl.GetRecord("key", "shot_delayseconds").GetString("value"));
    m_lifeTimeK = boost::lexical_cast<double>(basicTbl.GetRecord("key", "lifetime_k").GetString("value"));
    m_lifeTimeA = boost::lexical_cast<uint32_t>(basicTbl.GetRecord("key", "lifetime_a").GetString("value"));
    m_lifeTimeB = boost::lexical_cast<double>(basicTbl.GetRecord("key", "lifetime_b").GetString("value"));
    m_bulletSpeedCoef = boost::lexical_cast<double>(basicTbl.GetRecord("key", "bullet_speed_coef").GetString("value"));
    double seconds = boost::lexical_cast<double>(basicTbl.GetRecord("key", "split_last_seconds").GetString("value"));
    m_splitLastMillSeconds = std::floor(seconds * 1000);
    m_splitCD = boost::lexical_cast<double>(basicTbl.GetRecord("key", "split_cd").GetString("value"));
    m_splitSpeedFactor = boost::lexical_cast<double>(basicTbl.GetRecord("key", "split_speed_factor").GetString("value"));
    m_maxPlayerPlaneNumber = boost::lexical_cast<double>(basicTbl.GetRecord("key", "player_max_plane_num").GetString("value"));
    m_playerFastestSpeed = getMoveSpeedByPlaneNumber(2) * m_splitSpeedFactor;
    m_reliveSeconds = boost::lexical_cast<double>(basicTbl.GetRecord("key", "relive_seconds").GetString("value"));
    m_splitLifeTimeCoef = boost::lexical_cast<double>(basicTbl.GetRecord("key", "split_lifetime_coef").GetString("value"));
    m_speedupLastMillSeconds = boost::lexical_cast<double>(basicTbl.GetRecord("key", "speedup_last_seconds").GetString("value")) * 1000;


    // 创建指定数量的能源
    for (uint32_t i = 0; i < m_energyNumber; ++i)
    {
        Pos bornPos = randomEntityBornPos();
        createFood(bornPos.m_x, bornPos.m_y);
    }
    //batchCreateEnergy();

    //创建N个AI,N=房间可容纳人数
    // 添加定时器
	LOG_INFO(Fmt("roomid=%u,insert frameUpdate timer") % m_id);
    m_pTimerQueue->InsertRepeatFromNow(0, 33
            , [this]()
            {
                frameUpdate();
            });
    // 每1秒通知客户端排行榜数据
    m_pTimerQueue->InsertRepeatFromNow(0, 1000
            , [this]()
            {
                notifyAllPlayerRankData();
                batchCreateEnergy();
            });
}

void CommonRoom::broadcastPlayerEnter(CommonPlayer *pPlayer)
{
    plane::PlayerInfo msg;
    msg.set_id(pPlayer->m_id);
    msg.set_name(pPlayer->m_nickname);
    msg.set_speed(std::floor(pPlayer->getMoveSpeed() * 100));
    msg.set_skinid(pPlayer->m_skinid);
    msg.set_bulletskinid(pPlayer->m_bulletskinid);
    msg.set_random_formation_id(pPlayer->m_formation.getFormationID());
    msg.mutable_move()->set_angle(std::floor(pPlayer->m_angle * 100));
    msg.mutable_move()->set_score(pPlayer->getRankScore());
    msg.mutable_move()->set_x(std::floor(pPlayer->m_pos.m_x * 100));
    msg.mutable_move()->set_y(std::floor(pPlayer->m_pos.m_y * 100));

    for (const auto &plane : pPlayer->m_planes)
    {
        msg.mutable_move()->add_small_plane_ids(plane.first);
    }

    broadcast("plane.PlanePush", "PlayerEnter", msg);
}

FoodSharedPtr CommonRoom::createFood(uint32_t x, uint32_t y)
{
    uint32_t id = getNextEntityID();
    FoodSharedPtr ptr = std::make_shared<Food>(id, x, y);
    m_foodSharedPtrMap[id] = ptr;

    return ptr;
}

FoodSharedPtr CommonRoom::createSplitPlane(double x, double y
        , uint32_t skinid, uint32_t angle, uint32_t ownerid)
{
    uint32_t id = getNextEntityID();
    FoodSharedPtr ptr = std::make_shared<SplitPlane>(id, x, y , skinid, angle, ownerid);
    m_foodSharedPtrMap[id] = ptr;

    return ptr;
}

Pos CommonRoom::randomEntityBornPos()
{
    std::mt19937 rng;
    rng.seed(std::random_device()());
    std::uniform_int_distribution<std::mt19937::result_type> distx(m_min.m_x,m_max.m_x);
    std::uniform_int_distribution<std::mt19937::result_type> disty(m_min.m_y,m_max.m_y);

    return Pos(distx(rng), disty(rng));
}

uint32_t CommonRoom::getLeftCanInPlayerNumber()
{
    int leftNum = m_holdPlayerNum - GetPlayersCount();
    leftNum = leftNum > 0 ? leftNum : 0;


    if (!isTimeNearOver())
    {
        return leftNum;
    }

    return 0;
}

bool CommonRoom::isTimeNearOver()
{
    if (getGameOverLeftSeconds() > m_forbidJoinSeconds)
    {
        return false;
    }

    return true;
}

uint32_t CommonRoom::getGameOverLeftSeconds()
{
    uint64_t now = Util::getSystemMs();
    int64_t leftMillSeconds = m_roomEndMillSeconds - now;
    if (leftMillSeconds > 0)
    {
        return leftMillSeconds / 1000;
    }

    return 0;
}

double CommonRoom::getMoveSpeedByPlaneNumber(uint32_t number)
{
    double speed = m_moveSpeedK / (number + m_moveSpeedA) + m_moveSpeedB;
    speed = std::floor(speed * 100) / 100;
    speed = speed < 4 ? 4 : speed;
    return speed;
}

void CommonRoom::frameUpdate()
{
    double seconds = (Util::getSystemMs() - m_playersLastUpdateMillSeconds) / 1000.0;
    for (const auto &playerIt : m_players)
    {
        playerIt.second->frameMove(seconds);
    }
    m_playersLastUpdateMillSeconds = Util::getSystemMs();

    double bulletSeconds = (Util::getSystemMs() - m_bulletGroupsLastUpdateMillSeconds) / 1000.0;
    for (auto &bulletGroupIt : m_bulletGroupMap)
    {
        bulletGroupIt.second.frameMove(bulletSeconds);
        /*
        BulletGroup &group = bulletGroupIt.second;
        double movex = group.m_moveSpeed * bulletSeconds * m_dir.m_x;
        double movey = group.m_moveSpeed * bulletSeconds * m_dir.m_y;
        LOG_INFO(Fmt("子弹组移动%d,move=(%f,%f),deltaseconds=%f,speed=%f,nowPos=(%f,%f)") 
                % group.m_id % movex % movey % bulletSeconds % group.m_moveSpeed % group.m_pos.m_x % group.m_pos.m_y);
                */
    }
    m_bulletGroupsLastUpdateMillSeconds = Util::getSystemMs();

    frameCheckHit();

    broadcastAllPlayerPos();
}

void CommonRoom::frameCheckHit()
{
    plane::FrameHitBcMsg msg;
    for (auto &bulletGroupIt : m_bulletGroupMap)
    {
        BulletGroup &group = bulletGroupIt.second;
        plane::OneBulletGroupHit *pOneHit = nullptr;
        // 遍历潜在玩家列表,去掉死亡玩家,去掉无敌玩家
        // 1.检测子弹群的包裹圆,是否与 玩家的包裹圆相交
        // 2.若相交,筛选出玩家的飞机列表中,哪些小飞机是有可能和子弹群碰撞的
        // 3.对每个可能被碰到的小飞机,逆序遍历该子弹组,找到打中该小飞机的子弹
        for (size_t i = 0; i < group.m_possibleEnemies.size(); ++i)
        {
            auto it = m_players.find(group.m_possibleEnemies[i]);
            // 玩家不见了,直接return,下一个
            if (it == m_players.end())
            {
                continue;
            }

            CommonPlayerSharedPtr & player = it->second;
            if (player->isDead() || player->isGod())
            {
                continue;
            }

            // 算下子弹群和玩家群中心的距离的平方
            double centerSquareDistance = group.m_pos.squareDistance(player->m_pos);
            double wrapRadiusSum = player->m_wrapRadius + group.m_wrapRadius;
            if (centerSquareDistance >= wrapRadiusSum * wrapRadiusSum)
            {
                continue;
            }

            // 筛选出可能会碰撞的小飞机
            std::map<uint32_t, const Plane*> possiblePlanes;
            for (const auto &planeIt : player->m_planes)
            {
                Vector2 worldPos = planeIt.second.getWorldPos();
                double plane2bulletGroupDistance = worldPos.squareDistance(group.m_pos);
                wrapRadiusSum = moduleRadius + group.m_wrapRadius;
                if (plane2bulletGroupDistance >= wrapRadiusSum * wrapRadiusSum)
                {
                    continue;
                }
                possiblePlanes[planeIt.second.m_id] = &planeIt.second;
            }

            // 逆序遍历每个子弹,依次碰撞,并记录哪个子弹打中了哪个飞机
            // 记录的数据存在tuple中,依次为 击中子弹id对应的飞机id,被击中的飞机id
            std::vector<std::tuple<uint32_t,uint32_t>> hitinfos;
            auto bulletIt = group.m_map.rbegin();
            //LOG_DEBUG(Fmt("开始检测子弹组Id=%u") % group.m_id);
            for (; bulletIt != group.m_map.rend(); )
            {
                Vector2 bulletWorldPos = bulletIt->second.getWorldPos();
                uint32_t hitPlaneid = 0;
                for (auto planeIt = possiblePlanes.begin(); planeIt != possiblePlanes.end(); ++planeIt)
                {
                    Vector2 worldPos = planeIt->second->getWorldPos();
                    double plane2bulletGroupDistance = worldPos.squareDistance(bulletWorldPos);
                    wrapRadiusSum = moduleRadius + moduleRadius;
                    if (plane2bulletGroupDistance < wrapRadiusSum * wrapRadiusSum)
                    {
                        hitPlaneid = planeIt->second->m_id;
                        possiblePlanes.erase(planeIt);
                        //LOG_DEBUG(Fmt("子弹Id=%u 击中飞机id=%u") % bulletIt->second.m_id % hitPlaneid);
                        break;
                    }
                }

                // 说明该子弹打中了一架飞机,那么子弹和飞机都需要从列表中移除,
                // 一个子弹只能打中一架飞机,一架飞机也只能被一个子弹打中
                if (0 != hitPlaneid)
                {
                    hitinfos.emplace_back(bulletIt->second.m_correspondPlaneID, hitPlaneid);
                    bulletIt = decltype(bulletIt){group.m_map.erase(std::next(bulletIt).base())};
                }
                else
                {
                    ++bulletIt;
                }
            }
            //LOG_DEBUG(Fmt("结束检测子弹组Id=%u") % group.m_id);

            // 构造帧击中消息,通知客户端,同时处理玩家加分,死亡等
            size_t hitinfoSize = hitinfos.size();
            if (hitinfoSize > 0)
            {
                if (!pOneHit)
                {
                    pOneHit = msg.add_hits();
                    pOneHit->set_bulletgroupid(group.m_id);
                }

                for (size_t i = 0; i < hitinfoSize; ++i)
                {
                    plane::OnePlaneHit *pPlaneHit = pOneHit->add_planehits();
                    pPlaneHit->set_bulletid(std::get<0>(hitinfos[i]));
                    pPlaneHit->set_hit_playerid(player->m_id);
                    pPlaneHit->set_hit_planeid(std::get<1>(hitinfos[i]));
                    player->deleteOneSmallPlane(std::get<1>(hitinfos[i]), false, true);
                    pPlaneHit->set_ownerspeed(std::floor(player->getMoveSpeed() * 100));

                    // 给击杀者增加得分
                    auto pAttacker = m_players.find(group.m_shooterid);
                    CommonPlayer *pAttackerPlayer = nullptr;
                    if (pAttacker != m_players.end())
                    {
                        pAttackerPlayer = pAttacker->second.get();
                        pAttackerPlayer->setScore(pAttackerPlayer->getRankScore() + 100);
                        pAttackerPlayer->m_gameData.m_killNumber += 1;
                        pAttackerPlayer->m_gameData.m_totalKillNumber += 1;
                        if (pAttackerPlayer->m_gameData.m_killNumber % m_needShotNumber == 0
                            && pAttackerPlayer->canAddPlane())
                        {
                            pPlaneHit->set_attackerid(group.m_shooterid);
                            pPlaneHit->set_addplaneid(pAttackerPlayer->addOneSmallPlane());
                            pPlaneHit->set_speed(std::floor(pAttackerPlayer->getMoveSpeed() * 100));
                        }
                    }

                    // 被击中的玩家如果死亡
                    if (player->isDead() && pAttackerPlayer)
                    {
                        player->m_killerID = pAttackerPlayer->m_id;
                    }
                }
            }
        }
        
        // 遍历潜在能源列表,
        // 1. 先检查整个子弹群是否和该能源相交
        // 2. 若相交,逆序遍历该子弹群,找到第一个能击中的子弹,然后break掉
        // (逆序的原因是最外围的子弹离能源最近,而最外围的子弹是最后生成的,id最大)
        for (size_t i = 0; i < group.m_possibleEnergies.size(); ++i)
        {
            auto it = m_foodSharedPtrMap.find(group.m_possibleEnergies[i]);
            if (it == m_foodSharedPtrMap.end())
            {
                continue;
            }

            FoodSharedPtr& food = it->second;
            // 子弹群和能源中心的距离
            double centerSquareDistance = group.m_pos.squareDistance(food->m_pos);
            if (centerSquareDistance >= 
                (moduleRadius + group.m_wrapRadius) * (moduleRadius + group.m_wrapRadius))
            {
                continue;
            }

            // 找到一个能击中的子弹
            const Bullet *pBullet = nullptr;
            auto bulletIt = group.m_map.rbegin();
            for (; bulletIt != group.m_map.rend(); ++bulletIt)
            {
                Vector2 worldPos(bulletIt->second.m_pos.m_x + group.m_pos.m_x
                    , bulletIt->second.m_pos.m_y + group.m_pos.m_y);
                double bulletSquareDistance = worldPos.squareDistance(food->m_pos);
                if (bulletSquareDistance < (2 * moduleRadius) * (2 * moduleRadius))
                {
                    pBullet = &bulletIt->second;
                    break;
                }
            }

            if (!pBullet)
            {
                continue;
            }

            // 移除该子弹,通知客户端移除能源,不管玩家能不能添加飞机,都要通知客户端移除子弹和能源
            if (!pOneHit)
            {
                pOneHit = msg.add_hits();
                pOneHit->set_bulletgroupid(group.m_id);
            }

            plane::OneEnergyHit *pEnerygyHit = pOneHit->add_eneryhits();
            pEnerygyHit->set_bulletid(pBullet->m_correspondPlaneID);
            pEnerygyHit->set_id(food->m_id);
            pEnerygyHit->set_type(food->m_ftype);
            // 如果主人还在的话,给主人加1架飞机,通知客户端主人新的速度
            auto pPlayerIt = m_players.find(group.m_shooterid);
            if (pPlayerIt != m_players.end())
            {
                if (pPlayerIt->second->canAddPlane())
                {
                    pEnerygyHit->set_playerid(pPlayerIt->second->m_id);
                    pEnerygyHit->set_planeid(pPlayerIt->second->addOneSmallPlane());
                    pEnerygyHit->set_speed(std::floor(pPlayerIt->second->getMoveSpeed() * 100));
                }
            }
            if (!food->isSplitPlane())
            {
                ++m_needCreateEnergyNumber;
            }
            m_foodSharedPtrMap.erase(food->m_id);
            group.m_map.erase(pBullet->m_id);
        }
    }
    broadcast("plane.PlanePush", "NotifyFrameHit", msg);

    for (const auto &playerIt : m_players)
    {
        if (playerIt.second->isDead())
        {
            CommonPlayer *pAttackerPlayer = getPlayerByID(playerIt.second->m_killerID);
            if (pAttackerPlayer)
            {
                playerIt.second->onKilled(pAttackerPlayer);
                pAttackerPlayer->onKillOtherPlayer();
            }
        }
    }
}

void CommonRoom::broadcastAllPlayerPos()
{
    plane::MoveToBcMsgRoom msg;
    msg.mutable_curframe_and_roomid()->set_ms(0);
    msg.mutable_curframe_and_roomid()->set_id(m_id);
    for (const auto &playerIt : m_players)
    {
        if (!playerIt.second->m_isStop)
        {
            plane::MoveToBcMsg *pMove = msg.add_moves();
            pMove->mutable_ms_and_id()->set_ms(0);
            pMove->mutable_ms_and_id()->set_id(playerIt.second->m_id);
            pMove->mutable_move_to()->set_angle(std::floor(playerIt.second->m_angle * 100));
            pMove->mutable_move_to()->set_score(playerIt.second->getRankScore());
            pMove->mutable_move_to()->set_x(std::floor(playerIt.second->m_pos.m_x * 100));
            pMove->mutable_move_to()->set_y(std::floor(playerIt.second->m_pos.m_y * 100));

            for (const auto &plane : playerIt.second->m_planes)
            {
                pMove->mutable_move_to()->add_small_plane_ids(plane.first);
            }
        }
    }

    broadcast("plane.PlanePush", "PackPlayerMoveTo", msg);
}
uint32_t CommonRoom::getNearestPlayerid(CommonPlayer *pPlayer, uint32_t range)
{
    uint32_t minDistance = (uint32_t)-1;
    uint32_t minPlayerDistance = (uint32_t)-1;
    uint32_t nearestID = 0;
    // 如果有该范围内有玩家,即使不是最近距离,也优先击杀玩家
    uint32_t nearestPlayerid = 0;
    for (const auto &playerIt : m_players)
    {
        if (playerIt.second.get() == pPlayer || playerIt.second->isDead())
        {
            continue;
        }
        if (!isEnemy(pPlayer, playerIt.second.get()))
        {
            continue;
        }
        double distx = pPlayer->m_pos.m_x - playerIt.second->m_pos.m_x;
        double disty = pPlayer->m_pos.m_y - playerIt.second->m_pos.m_y;
        double dis = distx * distx + disty * disty;
        if (!playerIt.second->isAI() && !playerIt.second->isDead())
        {
            if (dis < minPlayerDistance && dis < range)
            {
                nearestPlayerid = playerIt.second->m_id;
                minPlayerDistance = dis;
            }
        }
        else
        {
            if (dis < minDistance && dis < range)
            {
                nearestID = playerIt.second->m_id;
                minDistance = dis;
            }
        }
    }

    //return nearestPlayerid == 0 ? nearestID : nearestPlayerid;
    return nearestPlayerid;
}

// 通知所有非AI玩家,并设置定时器摧毁该子弹
void CommonRoom::newBulletGroup(CommonPlayer *player)
{
    uint32_t entityID = getNextEntityID();
    double lifeTime = player->getBuleltGroupLifetime();
    double speed = player->m_moveSpeed * (1 + m_bulletSpeedCoef / 100);
    speed = std::floor(speed * 100) / 100;
    auto ret = m_bulletGroupMap.emplace(std::piecewise_construct,
           std::forward_as_tuple(entityID),
           std::forward_as_tuple(entityID, player->m_id, player->m_pos
               , Vector2(-sinMap[player->m_angle], cosMap[player->m_angle])
               ,player->m_wrapRadius, speed, lifeTime, player->m_angle));

    BulletGroup &group = ret.first->second;
    player->generateBullets(group);
    plane::FireSimpleBcMsg msg;
    msg.set_dir_angle(group.m_angle * 100);
    msg.set_x(std::floor(group.m_pos.m_x * 100));
    msg.set_y(std::floor(group.m_pos.m_y * 100));
    msg.set_life_seconds(std::floor(group.m_lifeTime * 100));
    msg.set_speed(std::floor(group.m_moveSpeed * 100));
    msg.set_shooter_id(group.m_shooterid);
    msg.set_bullet_group_id(group.m_id);
    msg.set_fire_ms(Util::getSystemMs() - m_roomCreateMillSeconds);
    broadcast("plane.PlanePush", "Fire", msg);
    /*
    LOG_INFO(Fmt("%d,%s %s shot successfully,angle=%u,speed=%u") 
            % player->m_id % player->m_nickname % player->m_account 
            % player->m_angle % speed);
            */

    uint32_t groupid = group.m_id;
    m_pTimerQueue->InsertSingleFromNow(std::floor(group.m_lifeTime * 1000)
            , [this, groupid]() {deleteBulletGroup(groupid);});

    fillBulletGroupPossibleEnergies(group);
    fillBulletGroupPossibleEnemies(group);
}

void CommonRoom::deleteBulletGroup(uint32_t id)
{
    auto it = m_bulletGroupMap.find(id);
    if (it != m_bulletGroupMap.end())
    {
        m_bulletGroupMap.erase(it);
	    //LOG_INFO(Fmt("deleteBulletGroup.id=%u") % id);
    }
}

void CommonRoom::fillBulletGroupPossibleEnergies(BulletGroup &group)
{
    double left = 0.0, right = 0.0, bottom = 0.0, top = 0.0;
    group.getWrapSquareCoordinate(left, bottom, right, top);
    for (const auto &foodIt: m_foodSharedPtrMap)
    {
        const Vector2 &pos = foodIt.second->m_pos;
        if (pos.m_x >= left && pos.m_x <= right && pos.m_y >= bottom && pos.m_y <= top)
        {
            group.m_possibleEnergies.push_back(foodIt.second->m_id);
        }
    }
}

// 在普通房里,除了自己以外,其他人都是敌人
bool CommonRoom::isEnemy(CommonPlayer *pPlayer1, CommonPlayer *pPlayer2)
{
    return pPlayer1 != pPlayer2;
}

void CommonRoom::fillBulletGroupPossibleEnemies(BulletGroup &group)
{
    auto bulletOwnerIt = m_players.find(group.m_shooterid);
    if (bulletOwnerIt == m_players.end())
    {
        return ;
    }

    // 获得大正方形的最小,最大坐标
    double left = 0.0, right = 0.0, bottom = 0.0, top = 0.0;
    group.getBiggerWrapSquareCoordinate(left, bottom, right, top
            , m_playerFastestSpeed, checkEnemyMillSecondsInterval);
    for (const auto &it : m_players)
    {
        // 自己不能打自己
        if (!isEnemy(bulletOwnerIt->second.get(), it.second.get()))
        {
            continue;
        }
        const Vector2 &pos = it.second->m_pos;
        // 若玩家在该正方形内,说明是潜在敌人
        if (pos.m_x >= left && pos.m_x <= right && pos.m_y >= bottom && pos.m_y <= top)
        {
            group.m_possibleEnemies.push_back(it.second->m_id);
        }
    }
    group.m_nextCheckEnemyMillSeconds = Util::getSystemMs() + checkEnemyMillSecondsInterval;
}

void CommonRoom::batchCheckEnergy(std::vector<FoodSharedPtr> &ptrs)
{
    // 对每个子弹组,判断下这些能源里面有没有潜在能源
    for (auto &bulletGroupIt : m_bulletGroupMap)
    {
        double left = 0.0, right = 0.0, bottom = 0.0, top = 0.0;
        bulletGroupIt.second.getWrapSquareCoordinate(left, bottom, right, top);
        for (size_t i = 0; i < ptrs.size(); ++i)
        {
            const Vector2 &pos = ptrs[i]->m_pos;
            if (pos.m_x >= left && pos.m_x <= right && pos.m_y >= bottom && pos.m_y <= top)
            {
                bulletGroupIt.second.m_possibleEnergies.push_back(ptrs[i]->m_id);
            }
        }
    }
}

void CommonRoom::refreshBulletEnemiesOnPlayerEnter(CommonPlayer *pPlayer)
{
    for (auto &bulletGroupIt : m_bulletGroupMap)
    {
        auto bulletOwnerIt = m_players.find(bulletGroupIt.second.m_shooterid);
        if (bulletOwnerIt == m_players.end())
        {
            continue ;
        }
        if (!isEnemy(pPlayer, bulletOwnerIt->second.get()))
        {
            continue;
        }
        double left = 0.0, right = 0.0, bottom = 0.0, top = 0.0;
        // 这里一定要用最快速度,而不是玩家当前速度,因为玩家随时可能分裂.
        // 这里没有精确的拿 m_nextCheckEnemyMillSeconds-Util::getSystemMs(),感觉500ms内没必要,还多算一步...
        bulletGroupIt.second.getBiggerWrapSquareCoordinate(left, bottom, right, top
            , m_playerFastestSpeed, checkEnemyMillSecondsInterval);
        const Vector2 &pos = pPlayer->m_pos;
        if (pos.m_x >= left && pos.m_x <= right && pos.m_y >= bottom && pos.m_y <= top)
        {
            bulletGroupIt.second.m_possibleEnemies.push_back(pPlayer->m_id);
        }
    }
}

void CommonRoom::batchCreateEnergy()
{
    if (m_needCreateEnergyNumber <= 0)
    {
        return ;
    }
    plane::BatchAddFoodBcMsg msg;
    std::vector<FoodSharedPtr> ptrs;
    for (uint32_t i = 0; i < m_needCreateEnergyNumber; ++i)
    {
        Pos bornPos = randomEntityBornPos();
        FoodSharedPtr p = createFood(bornPos.m_x, bornPos.m_y);
        ptrs.push_back(p);
        plane::FoodMsg *pFood = msg.add_foods();
        pFood->set_food_id(p->m_id);
        pFood->set_x(std::floor(bornPos.m_x * 100));
        pFood->set_y(std::floor(bornPos.m_y * 100));
    }

    m_needCreateEnergyNumber = 0;
    // 通知所有玩家
    broadcast("plane.PlanePush", "BatchAddFood", msg);
    // 更新所有子弹组的潜在能源列表
    batchCheckEnergy(ptrs);
}

void CommonRoom::broadcast(const std::string& sService, const std::string& sMethod
       ,const google::protobuf::Message& req, const RpcCallback &cb)
{
    for (const auto& each : m_players)
    {
        if (!each.second->isAI() && !each.second->m_isOffline)
        {
            RpcHelper::Request(each.second->m_gameClientID, sService, sMethod, req, cb);
	        //LOG_INFO(Fmt("send cmd to %s,service=%s,method=%s") % each.second->m_nickname % sService % sMethod);
        }
    }
}

void CommonRoom::luaBroadcast(const std::string& sService, const std::string& sMethod, const std::string &req)
{
    for (const auto& each : m_players)
    {
        if (!each.second->isAI() && !each.second->m_isOffline)
        {
            RpcHelper::Request(each.second->m_gameClientID, sService, sMethod, req);
	        //LOG_INFO(Fmt("send cmd to %s,service=%s,method=%s") % each.second->m_nickname % sService % sMethod);
        }
    }
}

void CommonRoom::notifyAllPlayerRankData()
{
    // 构造好前N名的数据
    plane::RankDataMsg msg;
    msg.set_my_rank(0);
    msg.set_my_score(0);
    std::vector<const CommonPlayer*> topN;
    m_rankManager.getRankData(rankShowNum, topN);
    for (size_t i = 0; i < topN.size(); ++i)
    {
        plane::RankData *pData = msg.add_data();
        pData->set_playerid(topN[i]->m_id);
        pData->set_playername(topN[i]->m_nickname);
    }

    for (const auto &playerIt: m_players)
    {
        if (!playerIt.second->isAI() && !playerIt.second->m_isOffline)
        {
            msg.set_my_rank(m_rankManager.getRankByID(playerIt.second->m_id));
            msg.set_my_score(playerIt.second->getRankScore());
            RpcHelper::Request(playerIt.second->m_gameClientID, "plane.PlanePush", "NotifyRankData", msg);
        }
    }
}

void CommonRoom::sendRankResultData()
{
	LOG_INFO(Fmt("roomid=%d发放排行榜奖励.") % m_id);
}

void CommonRoom::getAllPlayerGameClientIDStrings(std::vector<std::string> &ret)
{
    for (const auto &it : m_players)
    {
        ret.push_back(it.second->m_gameClientID.ToString());
    }
}

uint32_t CommonRoom::getMVPPlayerUid()
{
    // 先算下最大连杀排名
    std::vector<CommonPlayer*> maxComboRank;
    std::vector<CommonPlayer*> destroyRank;
    for (const auto &playerIt : m_players)
    {
        maxComboRank.push_back(playerIt.second.get());
        destroyRank.push_back(playerIt.second.get());
    }

    std::sort(maxComboRank.begin(), maxComboRank.end(), [](const CommonPlayer *l, const CommonPlayer *r)
            {
                 return l->m_gameData.m_highestComboNumber < r->m_gameData.m_highestComboNumber;
            });
    std::sort(destroyRank.begin(), destroyRank.end(), [](const CommonPlayer *l, const CommonPlayer *r)
            {
                 return l->m_gameData.m_destroyNumber < r->m_gameData.m_destroyNumber;
            });

    std::map<uint32_t, MVPCalculateData> mvp;
    uint32_t rank = 1;
    for (size_t i = 0; i < maxComboRank.size(); ++i, ++rank)
    {
        mvp.emplace(std::piecewise_construct,
                std::forward_as_tuple(maxComboRank[i]->m_id),
                std::forward_as_tuple(maxComboRank[i]->m_id, rank));
    }

    rank = 1;
    //总排名到uid的映射
    std::map<uint32_t, uint32_t, std::greater<uint32_t>> rank2uid;
    for (size_t i = 0; i < destroyRank.size(); ++i, ++rank)
    {
        mvp[destroyRank[i]->m_id].m_rankSum += rank;
        rank2uid[mvp[destroyRank[i]->m_id].m_rankSum] = destroyRank[i]->m_id;
    }

    return rank2uid.begin()->second;
}

void CommonRoom::getAllPlayerRankData(std::vector<LuaRankData> &ret)
{
    std::vector<const CommonPlayer*> topN;
    m_rankManager.getRankData(m_players.size(), topN);
    for (size_t i = 0; i < topN.size(); ++i)
    {
        LuaRankData data;
        data.m_id = topN[i]->m_id;
        data.m_nickname = topN[i]->m_nickname;
        data.m_account = topN[i]->m_account;
        data.m_isai = topN[i]->isAI() ? 1 : 0;
        data.m_serverid = topN[i]->m_gameClientID.GetBaseSvrId();
        data.m_rpcid = topN[i]->m_gameClientID.GetBaseRpcCltId();
        data.m_score = topN[i]->m_gameData.m_score;
        data.m_totalKillNumber = topN[i]->m_gameData.m_totalKillNumber;
        data.m_destroyNumber = topN[i]->m_gameData.m_destroyNumber;
        data.m_highestComboNumber = topN[i]->m_gameData.m_highestComboNumber;
        ret.push_back(data);
    }
}

double CommonRoom::getShotCDByPlaneNumber(uint32_t planeNumber)
{
    double cd = m_shotMinSeconds + m_shotAddCoef * planeNumber;
    cd = cd > m_shotMaxSeconds ? m_shotMaxSeconds : cd;
    return cd;
}

uint32_t CommonRoom::GetPlayersCount()
{
    uint32_t ret = 0;
    for (const auto &it : m_players)
    {
        if (it.second->isAI())
        {
            continue;
        }
        ++ret;
    }

    return ret;
}

void CommonRoom::onDestroy()
{
    for (const auto &it : m_players)
    {
        CommonRoom::m_playerSharedPtrMap.erase(it.second->m_gameClientID.ToString());
    }
    // 清除房间内的玩家,能源,子弹组
    m_players.clear();
    m_foodSharedPtrMap.clear();
    m_bulletGroupMap.clear();
    // 清除定时器
    EntityWithQueue::onDestroy();
}
