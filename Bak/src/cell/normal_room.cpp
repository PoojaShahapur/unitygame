#include <algorithm>
#include <random>
#include <boost/lexical_cast.hpp>
#include "normal_room.h"
#include "timer_queue/timer_queue_root.h"
#include "bullet_group.h"
#include "pb/svr/run_lua.pb.h"
#include "plane/team.pb.h"
#include "player.h"
#include "aiplayer.h"
#include "csv/csv.h"  // for CsvCfg::Init()
#include "formation.h"
#include "rpc/rpc_helper.h"

// todo, 将 Team 换成 Team*,因为Team对象的地址可能会被map挪到另一位置
// 或者所有Team*的地方先去查找下
const uint32_t teamMaxMemberNumber = 3;//一个队伍最多3个人
Room::Room(const uint32_t id) : CommonRoom(id){}

TeamRoom::TeamRoom(const uint32_t id) 
    : CommonRoom(id), m_maxTeamNumber(0)
{
    init();
    m_roomMode = RoomMode_Team;
}

// 读取皮肤表中策划配置团队皮肤id,有团队进来的时候,随机一个没用过的皮肤id
void TeamRoom::init()
{
    m_skinids.clear();

    auto &tbl = CsvCfg::GetTable("skin_skin.csv");
	const CsvRecordSptrVec& vRec = tbl.GetRecords();
    std::vector<uint32_t> skinids;
    for (size_t i = 0; i < vRec.size(); ++i)
    {
        uint32_t idUsedInTeamRoom = vRec[i]->GetInt<uint16_t>("team_use");
        if (1 == idUsedInTeamRoom)
        {
            skinids.push_back(vRec[i]->GetInt<uint32_t>("id"));
        }
    }

    std::random_device rd;
    std::mt19937 g(rd());
    std::shuffle(skinids.begin(), skinids.end(), g);

    for (size_t i = 0; i < skinids.size(); ++i)
    {
        m_skinids.insert(skinids[i]);
    }

    auto &commonTbl = CsvCfg::GetTable("param_Common.csv");
    m_maxTeamNumber = boost::lexical_cast<uint32_t>(commonTbl.GetRecord("key", "room_hold_team_num").GetString("value"));
}

bool TeamRoom::canThisTeamInRoom(std::string teamdata)
{
    if (isTimeNearOver())
    {
        return false;
    }

    svr::TeamData teaminfo;
    if (!teaminfo.ParseFromString(teamdata))
    {
        return false;
    }

    if (m_teams.empty())
    {
        // 如果队伍为空,创建 m_maxTeamNumber 个AI队伍
        for (uint32_t i = 1; i <= m_maxTeamNumber; ++i)
        {
            uint32_t team_skinid = *(m_skinids.begin());
            uint32_t leaderid = getNextEntityID();
            createAITeam(i, team_skinid, leaderid);
            m_skinids.erase(m_skinids.begin());
        }
    }

    // 这个队伍的成员数量
    uint32_t thisTeamMemberNum = teaminfo.current_num();
    if (3 == thisTeamMemberNum)
    {
        const Team *pTeam = getFirstAITeam();
        if (!pTeam)
        {
            return false;
        }

        // 找到一个AI队伍,将AI队伍里的玩家都踢出去,将玩家队伍成员都进入场景
        kickAITeam(pTeam);//此后指针非法
        uint32_t team_skinid = *(m_skinids.begin());
        createPlayerTeam(team_skinid, teaminfo);
        m_skinids.erase(m_skinids.begin());
    }
    else if (2 == thisTeamMemberNum)
    {
        Team *pTeam = getFirstAITeam();
        if (!pTeam)
        {
            for (auto &teamIt : m_teams)
            {
                if (teamIt.second->m_members.size() == 1)
                {
                    pTeam = teamIt.second.get();
                    break;
                }
            }
            if (pTeam)
            {
                appendTeamToPlayerTeam(pTeam, thisTeamMemberNum, teaminfo);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            kickAITeam(pTeam);
            uint32_t team_skinid = *(m_skinids.begin());
            createPlayerTeam(team_skinid, teaminfo);
            m_skinids.erase(m_skinids.begin());
            return true;
        }
    }
    else if (1 == thisTeamMemberNum)
    {
        // 先找队伍人数为1的队伍
        Team *pTeam = nullptr;
        for (auto &teamIt : m_teams)
        {
            if (1 == teamIt.second->m_members.size())
            {
                pTeam = teamIt.second.get();
                break;
            }
        }
        if (pTeam)
        {
            appendTeamToPlayerTeam(pTeam, thisTeamMemberNum, teaminfo);
            return true;
        }
        else
        {
            // 再去找一个AI队伍
            Team *pTeam = getFirstAITeam();
            if (!pTeam)
            {
                // 最后去找队伍人数为2的队伍
                for (auto &teamIt : m_teams)
                {
                    if (2 == teamIt.second->m_members.size())
                    {
                        pTeam = teamIt.second.get();
                        break;
                    }
                }
                if (pTeam)
                {
                    appendTeamToPlayerTeam(pTeam, thisTeamMemberNum, teaminfo);
                    return true;
                }
            }
            else
            {
                kickAITeam(pTeam);
                uint32_t team_skinid = *(m_skinids.begin());
                createPlayerTeam(team_skinid, teaminfo);
                m_skinids.erase(m_skinids.begin());
                return true;
            }
        }
    }

    /*
    Team &team = teamPair.first->second;
    for (int i = 0; i < size; ++i)
    {
        uint32_t uid = teaminfo.members(i);
        CommonPlayerSharedPtr pPlayer = std::make_shared<TeamPlayer>(0,0 
                , "pp", uid, "aaa", 2, 20001, this);
        TeamPlayer *pTeamPlayer = (TeamPlayer*)pPlayer.get();
        pTeamPlayer->m_teamid = teaminfo.teamid();
        m_playerSharedPtrMap[pPlayer->m_gameClientID.ToString()] = pPlayer;
        pPlayer->init();
    
        // 广播其他玩家,有人进来啦
        broadcastPlayerEnter(pPlayer.get());
        // 通知玩家进入无敌状态

        m_players[uid] = pPlayer;
        m_rankManager.onPlayerEnter(pPlayer.get());
        refreshBulletEnemiesOnPlayerEnter(pPlayer.get());
        team.m_members.insert(pPlayer.get());
    }
    */
    return false;
}

Team* TeamRoom::getFirstAITeam()
{
        Team *pTeam = nullptr;
        for (auto &teamIt : m_teams)
        {
            if (teamIt.second->isAITeam())
            {
                pTeam = teamIt.second.get();
                break;
            }
        }
        return pTeam;
}

std::string TeamRoom::playerReconnect(const uint16_t serverid, const uint64_t gameClientID, const uint32_t uid)
{
    auto playerIt = m_players.find(uid);
    if (playerIt == m_players.end())
    {
        return "";
    }
    CommonRoom::playerReconnect(serverid,gameClientID,uid);
    TeamPlayer *pTeamPlayer = (TeamPlayer*)playerIt->second.get();
    plane::EnterRoomResponse response;
    response.mutable_ms_and_id()->set_ms(m_currentFrame);
    response.mutable_ms_and_id()->set_id(uid);
    response.set_leftseconds(getGameOverLeftSeconds());
    response.set_mode(m_roomMode);
    response.set_voice_token(pTeamPlayer->m_pTeam->m_voiceToken);
    fulfillPlayersEnterRoomResponse(response);
    fulfillFoodsEnterRoomResponse(response);

    m_pTimerQueue->InsertSingleFromNow(1000 , [this, uid]() {notifyTeamInfo(uid);});
    return response.SerializeAsString();
}

void TeamRoom::notifyTeamInfo(const uint32_t playerid)
{
    auto playerIt = m_players.find(playerid);
    if (playerIt == m_players.end())
    {
        return ;
    }

    TeamPlayer *pTeamPlayer = (TeamPlayer*)playerIt->second.get();
    Team *pTeam = pTeamPlayer->m_pTeam;
    if (pTeam->m_members.size() == 0)
    {
        return ;
    }

    plane::TeamEnterRoomResponse msg;
    msg.set_teamid(pTeam->m_id);
    for (const auto &memberIt : pTeam->m_members)
    {
        if (memberIt->m_id != playerid)
        {
            msg.add_members(memberIt->m_id);
        }
    }
    RpcHelper::Request(pTeamPlayer->m_gameClientID, "plane.TeamPush", "NotifyTeamEnterRoom", msg);
}

void TeamRoom::createPlayerTeam(const uint32_t skinid, svr::TeamData &teaminfo)
{
    TeamSharedPtr pTeam = std::make_shared<Team>(teaminfo.teamid(), teaminfo.leaderid()
            , teaminfo.total_num(), skinid, std::to_string(teaminfo.voice_token()));
    int size = teaminfo.members_size();
    m_teams[teaminfo.teamid()] = pTeam;

    for (int i = 0; i < size; ++i)
    {
        uint32_t uid = teaminfo.members(i).uid();
        CommonPlayerSharedPtr pPlayer = std::make_shared<TeamPlayer>(0,0 
                , teaminfo.members(i).account(), uid, teaminfo.members(i).nickname()
                , skinid ,teaminfo.members(i).bulletid(), this);
        pPlayer->init();
        TeamPlayer *pTeamPlayer = (TeamPlayer*)pPlayer.get();
        pTeamPlayer->m_pTeam = pTeam.get();
    
        // 广播其他玩家,有人进来啦
        broadcastPlayerEnter(pPlayer.get());
        // 通知玩家进入无敌状态

        m_players[uid] = pPlayer;
        refreshBulletEnemiesOnPlayerEnter(pPlayer.get());
        pTeam->m_members.insert(pPlayer.get());
    }

    m_teamRankManager.onPlayerEnter(pTeam.get());
}

void TeamRoom::appendTeamToPlayerTeam(Team *pTeam, const uint32_t appendTeamSize, svr::TeamData &teaminfo)
{
    plane::AddMemberInRoomMsg msg;
    msg.set_member_num_before_add(pTeam->m_members.size());
    for (int i = 0; i < appendTeamSize; ++i)
    {
        uint32_t uid = teaminfo.members(i).uid();
        CommonPlayerSharedPtr pPlayer = std::make_shared<TeamPlayer>(0,0 
                , teaminfo.members(i).account(), uid, teaminfo.members(i).nickname()
                , pTeam->m_skinid,teaminfo.members(i).bulletid(), this);
        pPlayer->init();
        TeamPlayer *pTeamPlayer = (TeamPlayer*)pPlayer.get();
        pTeamPlayer->m_pTeam = pTeam;
    
        // 广播其他玩家,有人进来啦
        broadcastPlayerEnter(pPlayer.get());
        // 通知玩家进入无敌状态

        m_players[uid] = pPlayer;
        refreshBulletEnemiesOnPlayerEnter(pPlayer.get());
        pTeam->m_members.insert(pPlayer.get());
        msg.add_uids(uid);
    }
    for (const auto &member : pTeam->m_members)
    {
        if (!member->m_isOffline)
        {
            RpcHelper::Request(member->m_gameClientID, "plane.TeamPush", "NotifyAddMemberInRoom", msg);
        }
    }
}

void TeamRoom::createAITeam(const uint32_t teamid, const uint32_t skinid, const uint32_t leaderid)
{
    TeamSharedPtr pTeam = std::make_shared<Team>(teamid, leaderid, teamMaxMemberNumber, skinid);
    m_teams[teamid] = pTeam;
    bool isLeader = true;
    for (uint32_t i = 0; i < teamMaxMemberNumber; ++i)
    {
        uint32_t entityID = 0;
        if (isLeader)
        {
            entityID = leaderid;
            isLeader = false;
        }
        else
        {
            entityID = getNextEntityID();
        }
        std::string name("ai_");
        name += std::to_string(entityID);
        CommonPlayerSharedPtr pPlayer = std::make_shared<TeamAIPlayer>(
                entityID, name, name, skinid, 20001, this, HexagonFormation::get_mutable_instance());
        pPlayer->init();
        m_players[entityID] = pPlayer;
        pTeam->m_members.insert(pPlayer.get());
        TeamAIPlayer *pTeamAIPlayer = (TeamAIPlayer*)pPlayer.get();
        pTeamAIPlayer->m_pTeam = pTeam.get();
    }
    m_teamRankManager.onPlayerEnter(pTeam.get());
}


void TeamRoom::kickAITeam(const Team *pTeam)
{
    // 踢出队伍就是,分别踢掉每个成员,把队伍从排行榜中移除
    for (const auto &pPlayer : pTeam->m_members)
    {
        plane::MsAndId exit;
        exit.set_ms(m_currentFrame);
        exit.set_id(pPlayer->m_id);
        broadcast("plane.PlanePush", "PlayerExit", exit);
        m_players.erase(pPlayer->m_id);
    }
    m_teamRankManager.onPlayerLeave(pTeam);
    m_skinids.insert(pTeam->m_skinid);
    m_teams.erase(pTeam->m_id);
}

bool TeamRoom::isEnemy(CommonPlayer *pPlayer1, CommonPlayer *pPlayer2)
{
    TeamPlayer *p1 = (TeamPlayer*)pPlayer1;
    TeamPlayer *p2 = (TeamPlayer*)pPlayer2;
    if (p1 == p2)
    {
        return false;
    }
    if (p1->m_pTeam == p2->m_pTeam)
    {
        return false;
    }

    return true;
}

void TeamRoom::notifyAllPlayerRankData()
{
    // 构造好前N名的数据
    plane::TeamRankDataMsg msg;
    std::vector<const Team*> topN;
    m_teamRankManager.getRankData(rankShowNum, topN);
    for (size_t i = 0; i < topN.size(); ++i)
    {
        plane::OneTeamRankData *pData = msg.add_datas();
        pData->set_team_skinid(topN[i]->m_skinid);
        pData->set_member_num(topN[i]->m_members.size());
    }

    for (const auto &playerIt: m_players)
    {
        if (!playerIt.second->isAI() && !playerIt.second->m_isOffline)
        {
            TeamPlayer *pTeamPlayer = (TeamPlayer*)playerIt.second.get();
            msg.set_my_team_rank(m_teamRankManager.getRankByID(pTeamPlayer->m_pTeam->m_id));
            msg.set_my_teamskinid(pTeamPlayer->m_skinid);
            msg.set_my_score(playerIt.second->getRankScore());
            msg.set_my_team_num(pTeamPlayer->m_pTeam->m_members.size());
            RpcHelper::Request(playerIt.second->m_gameClientID, "plane.TeamPush", "NotifyTeamRankData", msg);
        }
    }
}

void TeamRoom::notifyAllPlayerRankData(TeamPlayer *pTeamPlayer)
{
    // 构造好前N名的数据
    plane::TeamRankDataMsg msg;
    std::vector<const Team*> topN;
    m_teamRankManager.getRankData(rankShowNum, topN);
    for (size_t i = 0; i < topN.size(); ++i)
    {
        plane::OneTeamRankData *pData = msg.add_datas();
        pData->set_team_skinid(topN[i]->m_skinid);
        pData->set_member_num(topN[i]->m_members.size());
    }

            msg.set_my_team_rank(m_teamRankManager.getRankByID(pTeamPlayer->m_pTeam->m_id));
            msg.set_my_teamskinid(pTeamPlayer->m_skinid);
            msg.set_my_score(pTeamPlayer->getRankScore());
            msg.set_my_team_num(pTeamPlayer->m_pTeam->m_members.size());
            RpcHelper::Request(pTeamPlayer->m_gameClientID, "plane.TeamPush", "NotifyTeamRankData", msg);
}

void TeamRoom::getAllPlayerRankData(std::vector<LuaRankData> &ret)
{
    std::vector<const Team*> topN;
    m_teamRankManager.getRankData(m_maxTeamNumber, topN);
    uint32_t rank = 0;
    for (size_t i = 0; i < topN.size(); ++i)
    {
        ++rank;
        for (const auto &memberIt : topN[i]->m_members)
        {
            LuaRankData data;
            data.m_id = memberIt->m_id;
            data.m_rank = rank;
            data.m_nickname = memberIt->m_nickname;
            data.m_account = memberIt->m_account;
            data.m_isai = memberIt->isAI() ? 1 : 0;
            data.m_serverid = memberIt->m_gameClientID.GetBaseSvrId();
            data.m_rpcid = memberIt->m_gameClientID.GetBaseRpcCltId();
            data.m_score = memberIt->m_gameData.m_score;
            data.m_totalKillNumber = memberIt->m_gameData.m_totalKillNumber;
            data.m_destroyNumber = memberIt->m_gameData.m_destroyNumber;
            data.m_highestComboNumber = memberIt->m_gameData.m_highestComboNumber;
            ret.push_back(data);
        }
    }
}
