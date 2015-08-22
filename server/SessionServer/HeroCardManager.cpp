/*************************************************************************
 Author: wang
 Created Time: 2014年10月22日 星期三 11时33分35秒
 File Name: SessionServer/HeroCardManager.cpp
 Description: 
 ************************************************************************/
#include "HeroCardManager.h"
#include "SessionManager.h"
#include "SessionServer.h"
#include "TimeTick.h"
#include "HeroCardCmd.h"
#include "zXML.h"
using namespace xml;

int battlezonepos[8][2] = 
{
    {34, 47},
    {32, 116},
    {50, 198},
    {128, 26},
    {128, 98},
    {128, 195},
    {220, 63},
    {182, 208}
};

HeroCardManager::HeroCardManager()
{
    //初始化场景服的局数信息
    BattleConfig::BattleServer::ItemMapIter it = battle.battleServer.item.begin();
    for(; it!= battle.battleServer.item.end(); it++)
    {
	allGameCount[it->first] = 0;
    }
    relax_chanllengeID = new zUniqueID<DWORD>(10000, 20000);
    ranking_chanllengeID = new zUniqueID<DWORD>(20000, 30000);
    competitive_chanllengeID = new zUniqueID<DWORD>(30000, 40000);
    friend_chanllengeID = new zUniqueID<DWORD>(40000, 50000);
    practise_chanllengeID = new zUniqueID<DWORD>(50000, 60000);
    boss_chanllengeID = new zUniqueID<DWORD>(60000, 70000);
    
    relaxBufferList.clear();
    relaxForMatchList.clear();
    relaxList.clear();
}
HeroCardManager::~HeroCardManager()
{
    SAFE_DELETE(relax_chanllengeID);
    SAFE_DELETE(ranking_chanllengeID);
    SAFE_DELETE(competitive_chanllengeID);
    SAFE_DELETE(friend_chanllengeID);
    SAFE_DELETE(practise_chanllengeID);
    SAFE_DELETE(boss_chanllengeID);
}

bool HeroCardManager::checkChanllengeInfo(WaitForMatch list, stChallengeInfo info)
{
    std::list<stChallengeInfo>::iterator it = std::find(list.begin(), list.end(), info);
    if(it != list.end())
    {
	return false;
    }
    return true;
}

bool HeroCardManager::putGameIDBack(BYTE type, DWORD gameID)
{
    using namespace Cmd::Session;
    bool ret = true;
    switch(type)
    {
	case CHALLENGE_GAME_RELAX_TYPE:		//休闲PVP
	    {
		relax_chanllengeID->put(gameID);
	    }
	    break;
	case CHALLENGE_GAME_RANKING_TYPE:
	    {
		ranking_chanllengeID->put(gameID);
	    }
	    break;
	case CHALLENGE_GAME_COMPETITIVE_TYPE:
	    {
		competitive_chanllengeID->put(gameID);
	    }
	    break;
	case CHALLENGE_GAME_PRACTISE_TYPE:
	    {
		practise_chanllengeID->put(gameID);
	    }
	    break;
	case CHALLENGE_GAME_BOSS_TYPE:
	    {
		boss_chanllengeID->put(gameID);
	    }
	    break;
	default:
	    ret = false;
	    break;
    }
    Zebra::logger->debug("[匹配] 回收组ID:%u 对战类型:%u %s", gameID, type, ret?"成功":"失败");
    return ret;
}

bool HeroCardManager::processMessage(Cmd::Session::t_ReqFightMatch_SceneSession *rev)
{
    if(0 == rev->cancel)
    {
	addUser(rev->userID, rev->type, rev->score, rev->cardsNumber);
    }
    else
    {
	cancelUser(rev->userID, rev->type);
    }
    return true;
}

void HeroCardManager::addUser(DWORD userID, BYTE type, DWORD score, DWORD cardsNumber)
{
    using namespace Cmd::Session;
    UserSession *pUser = UserSessionManager::getInstance()->getUserByID(userID);
    if(!pUser)
	return;
    if(getMinCountCountry() == 0)
    {
	Zebra::logger->error("[匹配] 对战服全部挂掉了");
	return;
    }
    switch(type)
    {
	case CHALLENGE_GAME_RELAX_TYPE:		//休闲PVP
	    {
		stChallengeInfo info;
		info.dwCharID = userID;
		strncpy(info.name, pUser->name, MAX_NAMESIZE);
		info.matchScore = score;
		info.cardsNumber = cardsNumber;
		info.reqMatchTime = SessionTimeTick::currentTime.sec();
		if(checkChanllengeInfo(relaxBufferList, info) && checkChanllengeInfo(relaxForMatchList, info))	//待匹配列表中不存在
		{
		    relaxBufferList.push_back(info);
		    Zebra::logger->debug("[休闲匹配]玩家请求匹配 休闲模式PVP，玩家%s(%u)",pUser->name, pUser->id);
		}
		else
		{
		    Zebra::logger->error("[休闲匹配]正在匹配中，玩家%s(%u) 重复发送匹配请求",pUser->name, pUser->id);
		}
	    }
	    break;
	case CHALLENGE_GAME_RANKING_TYPE:
	    {}
	    break;
	case CHALLENGE_GAME_COMPETITIVE_TYPE:
	    {}
	    break;
	case CHALLENGE_GAME_PRACTISE_TYPE:
	    {
		doApplyPractice(userID, 10000/*bossID*/, cardsNumber);
	    }
	    break;
	case CHALLENGE_GAME_BOSS_TYPE:
	    {}
	    break;
	default:
	    break;
    }
}

void HeroCardManager::cancelUser(DWORD userID, BYTE type)
{
    using namespace Cmd::Session;
    UserSession *pUser = UserSessionManager::getInstance()->getUserByID(userID);
    if(!pUser)
	return;
    switch(type)
    {
	case CHALLENGE_GAME_RELAX_TYPE:		//休闲PVP
	    {
		stChallengeInfo info;
		info.dwCharID = userID;
		if(!checkChanllengeInfo(relaxBufferList, info) || !checkChanllengeInfo(relaxForMatchList, info))    //还没匹配到
		{
		    Zebra::logger->error("[休闲匹配]玩家请求取消 可以取消，玩家%s(%u) ",pUser->name, pUser->id);
		}
		else
		{
		    Zebra::logger->debug("[休闲匹配]玩家请求取消 休闲模式PVP，玩家%s(%u) 取消失败",pUser->name, pUser->id);
		}
	    }
	    break;
	case CHALLENGE_GAME_RANKING_TYPE:
	    {}
	    break;
	case CHALLENGE_GAME_COMPETITIVE_TYPE:
	    {}
	    break;
	case CHALLENGE_GAME_PRACTISE_TYPE:
	    {}
	    break;
	case CHALLENGE_GAME_BOSS_TYPE:
	    {}
	    break;
	default:
	    break;
    }
}

void HeroCardManager::timer()
{
    //checkMatchedList();
    doRelaxGroup();
    doRankingGroup();
    doCompetitiveGroup();
}

/**
 * \brief   休闲对战匹配处理
 * \param
 * \return
*/
void HeroCardManager::doRelaxGroup()
{
    DWORD curTime = SessionTimeTick::currentTime.sec();
    for(std::list<stChallengeInfo>::iterator it=relaxBufferList.begin(); it!=relaxBufferList.end();)
    {
	if(curTime >= (it->reqMatchTime+5))
	{
	    relaxForMatchList.push_back(*it);	    //加入到待匹配列表
	    relaxBufferList.erase(it++);
	}
	else
	{
	    ++it;
	}
    }
    if(relaxForMatchList.size() >= 2)
    {
	relaxForMatchList.sort();	    //按照战力排序
	for(std::list<stChallengeInfo>::iterator it2=relaxForMatchList.begin(); it2!=relaxForMatchList.end(); it2++)
	{
	    stMatchedInfo mInfo;
	    mInfo.matchTime = curTime;
	    mInfo.info[0] = *it2;
	    it2->matched = 1;
	    it2++;
	    if(it2 != relaxForMatchList.end())
	    {
		mInfo.info[1] = *it2;
		it2->matched = 1;
		relaxList.push_back(mInfo);	    //两两一组,加入到已匹配列表,准备发送给客户端
	    }
	    else
	    {
		it2--;
		it2->matched = 0;
		break;			    //前一个轮空了,把前一个置为未匹配
	    }
	}
    }
    for(std::list<stMatchedInfo>::iterator Lit = relaxList.begin();
	    Lit!=relaxList.end(); Lit++)
    {
	if(Lit->hasSend == 0)
	{
	    //发送消息到场景
	    UserSession *pUser1 = UserSessionManager::getInstance()->getUserByID(Lit->info[0].dwCharID);
	    UserSession *pUser2 = UserSessionManager::getInstance()->getUserByID(Lit->info[1].dwCharID);
	    DWORD groupID = relax_chanllengeID->get();
	    DWORD cardsNumber1 = Lit->info[0].cardsNumber;
	    DWORD cardsNumber2 = Lit->info[1].cardsNumber;
	    if(groupID == relax_chanllengeID->invalid())
	    {
		Zebra::logger->debug("[休闲匹配]当前打对战的人太多了哦，这个类型的唯一ID都被用完了");
		continue;
	    }
	    if(pUser1 && pUser2 && pUser1->scene && pUser2->scene)
	    {
		Zebra::logger->debug("[休闲匹配]匹配好了,对战信息 组:%u(%s-VS-%s) 通知场景",groupID,pUser1->name, pUser2->name);

		WORD destCountry = getMinCountCountry();
		Zebra::logger->debug("[卡牌]最少局数的界域是 :%u", destCountry);
		std::string cName = SessionService::getInstance().country_info.getCountryName(destCountry)+"·天空城";
		const char* mapName = cName.c_str();
		SceneSession *scene= SceneSessionManager::getInstance()->getSceneByName(mapName);
		DWORD sceneNumber = randomOnePVPSceneID();
		if(scene && sceneNumber>0)
		{
		    matchPVPSuccess(scene, pUser1, pUser2, groupID, cardsNumber1, cardsNumber2,
			    Cmd::Session::CHALLENGE_GAME_RELAX_TYPE, sceneNumber, (char*)mapName);
		    Lit->hasSend = 1;
		}
		else if(sceneNumber == 0)
		{
		    relax_chanllengeID->put(groupID);
		    Zebra::logger->debug("[休闲匹配]准备通知场景创建对战时 找不到对战场景(客户端)");
		    continue;
		}
		else
		{
		    relax_chanllengeID->put(groupID);
		    Zebra::logger->debug("[休闲匹配]准备通知场景创建对战时 找不到地图:%s", mapName);
		    continue;
		}


	    }
	    else
	    {
		relax_chanllengeID->put(groupID);
		//是不是要通知其中一个玩家呢?
		Zebra::logger->error("[休闲匹配]都匹配好了(%s-VS-%s) 通知场景时发现有其中一个玩家下线了",Lit->info[0].name, Lit->info[1].name);
		continue;
	    }
	}
    }

    for(std::list<stChallengeInfo>::iterator it2=relaxForMatchList.begin(); it2!=relaxForMatchList.end(); )	//清除老数据
    {
	if(it2->matched == 1)
	{
	    relaxForMatchList.erase(it2++);
	}
	else
	{
	    ++it2;
	}
    }
    for(std::list<stMatchedInfo>::iterator Lit = relaxList.begin(); Lit!=relaxList.end(); Lit++)	    //把没有发送成功的再次加入到待匹配列表relaxForMatchList
    {
	if(Lit->hasSend == 0)
	{
	    Lit->info[0].matchTimes++;
	    Lit->info[1].matchTimes++;
	    relaxForMatchList.push_back(Lit->info[0]);
	    relaxForMatchList.push_back(Lit->info[1]);
	}
    }
    for(std::list<stChallengeInfo>::iterator it2=relaxForMatchList.begin(); it2!=relaxForMatchList.end(); )	//待匹配列表中发现玩家好久不在线
    {
	if(it2->matchTimes >= 5)
	{
	    Zebra::logger->debug("[休闲匹配] 匹配了很多轮 都是发送失败 当前玩家可能下线了:%s(%u)", it2->name, it2->dwCharID);
	    relaxForMatchList.erase(it2++);
	}
	else
	{
	    ++it2;
	}
    }
    relaxList.clear();
}

/**
 * \brief   排名对战匹配处理
 * \param
 * \return
*/
void HeroCardManager::doRankingGroup()
{

}

/**
 * \brief   竞技模式匹配处理
 * \param
 * \return
*/
void HeroCardManager::doCompetitiveGroup()
{

}

void HeroCardManager::updateSessionGameCount(WORD country, DWORD count)
{
    std::map<WORD, DWORD>::iterator iter = allGameCount.find(country);
    if(iter != allGameCount.end())
    {
	allGameCount[country] = count;
    }
    BattleConfig::BattleServer::ItemMapIter it = battle.battleServer.item.begin();
    for(; it!= battle.battleServer.item.end(); it++)
    {
	std::map<WORD, DWORD>::iterator itgame = allGameCount.find(it->first);
	if(itgame == allGameCount.end())
	{
	    allGameCount[it->first] = 0;    //动态加载新添的需要初始化进去
	}
    }
    
    std::map<WORD, DWORD>::iterator it2 = allGameCount.begin();
    for(; it2 != allGameCount.end();)
    {
	BattleConfig::BattleServer::ItemMapIter itcfg = battle.battleServer.item.find(it2->first);
	if(itcfg == battle.battleServer.item.end())
	{
	    allGameCount.erase(it2++);			//不在配置中的要删除
	}
	else
	{
	    ++it2;
	}
    }
#if 0
    for(std::map<WORD, DWORD>::iterator it=allGameCount.begin(); it!=allGameCount.end(); it++)
    {
	Zebra::logger->debug("[卡牌]当前服务器对战局数分布情况 界域:%u 局数:%u",it->first, it->second);
    }
#endif
}

WORD HeroCardManager::getMinCountCountry()
{
    std::map<WORD, DWORD>::iterator it = allGameCount.begin();
    WORD minCountry = 0;
    DWORD minCount = it->second;
    for(; it!=allGameCount.end(); it++)
    {
	std::string cName = SessionService::getInstance().country_info.getCountryName(it->first)+"·天空城";
	const SceneSession* pScene = SceneSessionManager::getInstance()->getSceneByName(cName.c_str());
	if(NULL == pScene)
	{
	    continue;
	}
	if(minCount >= it->second)
	{
	    minCountry = it->first;
	    minCount = it->second;
	}
    }
    return minCountry;
}

std::string HeroCardManager::getCreateGameMapName()
{
    WORD destCountry = getMinCountCountry();
    Zebra::logger->debug("[卡牌]最少局数的界域是 :%u", destCountry);
    std::string cName = SessionService::getInstance().country_info.getCountryName(destCountry)+"·天空城";
    return cName;
}

/**
 * \brief 拉对战双方去对战场景
 * \param
 * \return
*/
bool HeroCardManager::putTwoUserToBattleScene(UserSession *pUser1, UserSession *pUser2, char* mapName)
{
    if(!pUser1 || !pUser2 || !mapName)
	return false;

    int areaNum = zMisc::randBetween(0, 7);
    DWORD x = zMisc::randBetween(battlezonepos[areaNum][0], battlezonepos[areaNum][0]+3);
    DWORD y = zMisc::randBetween(battlezonepos[areaNum][1], battlezonepos[areaNum][1]+3);

    Cmd::Session::t_TelePKGame_SceneSession send;		    //拉玩家去对战场景
    strncpy(send.mapName, mapName, MAX_NAMESIZE-1);
    send.userID = pUser1->id;
    send.x = x;
    send.y = y;
    send.type = 1;
    pUser1->scene->sendCmd(&send, sizeof(send));

    Cmd::Session::t_TelePKGame_SceneSession send2;		    //拉玩家去对战场景
    strncpy(send2.mapName, mapName, MAX_NAMESIZE-1);
    send2.userID = pUser2->id;
    send2.x = x;
    send2.y = y;
    send2.type = 1;
    pUser2->scene->sendCmd(&send2, sizeof(send2));
    return true;
}

void HeroCardManager::checkMatchedList()
{
    for(std::list<stMatchedInfo>::iterator Lit = relaxList.begin();
	    Lit!=relaxList.end(); )
    {
	if(Lit->canClear == 1)
	{
	    relaxList.erase(Lit++);
	}
	else
	{
	    ++Lit;
	}
    }
}

/**
 * \brief   随机对战场景
 * \param
 * \return
*/
DWORD HeroCardManager::randomOnePVPSceneID()
{
    if(battle.scenesPVP.item.empty())
	return 0;
    DWORD index = 0;
    index = zMisc::randBetween(0, battle.scenesPVP.item.size()-1);
    DWORD num = battle.scenesPVP.item[index].id;
    return num;
}

bool HeroCardManager::matchPVPSuccess(SceneSession *scene, UserSession *pUser1, UserSession *pUser2,
	DWORD groupID, DWORD cardsNumber1, DWORD cardsNumber2, DWORD type, DWORD sceneNumber, char* mapName)
{
    if(!scene || !pUser1 || !pUser2 || !mapName)
	return false;

    Cmd::Session::t_CreateNewPkGame_SceneSession create;	    //发往场景通知创建游戏
    create.userID1 = pUser1->id;
    create.userID2 = pUser2->id;
    create.cardsNumber1 = cardsNumber1;
    create.cardsNumber2 = cardsNumber2;
    create.groupID = groupID;
    create.sceneNumber = sceneNumber;
    create.type = type;
    scene->sendCmd(&create, sizeof(create));

    Cmd::Session::t_RetSceneuserPkGame_SceneSession send1;	//返回场景角色对战数据
    send1.userID = pUser1->id;
    send1.groupID = groupID;
    send1.cardsNumber = cardsNumber1;
    send1.type = type;
    pUser1->scene->sendCmd(&send1, sizeof(send1));

    Cmd::Session::t_RetSceneuserPkGame_SceneSession send2;	//返回场景角色对战数据
    send2.userID = pUser2->id;
    send2.groupID = groupID;
    send2.cardsNumber = cardsNumber2;
    send2.type = type;
    pUser2->scene->sendCmd(&send2, sizeof(send2));

    Cmd::stRetHeroFightMatchUserCmd send3;			//告诉玩家匹配成功
    send3.fightType = type;
    send3.success = 1;
    pUser1->sendCmdToMe(&send3, sizeof(send3));
    pUser2->sendCmdToMe(&send3, sizeof(send3));

    Cmd::stRetHeroIntoBattleSceneUserCmd into;
    into.sceneNumber = sceneNumber;
    pUser1->sendCmdToMe(&into, sizeof(into));
    pUser2->sendCmdToMe(&into, sizeof(into));

    putTwoUserToBattleScene(pUser1, pUser2, (char*)mapName);	//拉对战双方去对战场景

    return true;
}

void HeroCardManager::doApplyPractice(DWORD userID, DWORD bossID, DWORD cardsNumber)
{
    //发送消息到场景
    UserSession *pUser1 = UserSessionManager::getInstance()->getUserByID(userID);
    DWORD groupID = practise_chanllengeID->get();
    DWORD cardsNumber1 = cardsNumber;
    if(groupID == practise_chanllengeID->invalid())
    {
	Zebra::logger->debug("[练习模式]当前打对战的人太多了哦，这个类型的唯一ID都被用完了");
	return;
    }
    if(pUser1 && pUser1->scene)
    {
	Zebra::logger->debug("[练习模式]匹配好了,对战信息 组:%u(%s-VS-NPC) 通知场景",groupID,pUser1->name);

	WORD destCountry = getMinCountCountry();
	Zebra::logger->debug("[卡牌]最少局数的界域是 :%u", destCountry);
	std::string cName = SessionService::getInstance().country_info.getCountryName(destCountry)+"·天空城";
	const char* mapName = cName.c_str();
	SceneSession *scene= SceneSessionManager::getInstance()->getSceneByName(mapName);
	DWORD sceneNumber = randomOnePVPSceneID();
	if(scene && sceneNumber>0)
	{
	    applyPVESuccess(scene, pUser1, bossID, groupID, cardsNumber1, 0,
		    Cmd::Session::CHALLENGE_GAME_PRACTISE_TYPE, sceneNumber, (char*)mapName);
	}
	else if(sceneNumber == 0)
	{
	    practise_chanllengeID->put(groupID);
	    Zebra::logger->debug("[练习模式]准备通知场景创建对战时 找不到对战场景(客户端)");
	    return;
	}
	else
	{
	    practise_chanllengeID->put(groupID);
	    Zebra::logger->debug("[练习模式]准备通知场景创建对战时 找不到地图:%s", mapName);
	    return;
	}


    }
    else
    {
	practise_chanllengeID->put(groupID);
	return;
    }

}

bool HeroCardManager::applyPVESuccess(SceneSession *scene, UserSession *pUser1, DWORD bossID,
	DWORD groupID, DWORD cardsNumber1, DWORD cardsNumber2, DWORD type, DWORD sceneNumber, char* mapName)
{
    if(!scene || !pUser1 || !mapName)
	return false;

    Cmd::Session::t_CreateNewPkGame_SceneSession create;	    //发往场景通知创建游戏
    create.userID1 = pUser1->id;
    create.userID2 = bossID;
    create.cardsNumber1 = cardsNumber1;
    create.cardsNumber2 = cardsNumber2;
    create.groupID = groupID;
    create.sceneNumber = sceneNumber;
    create.type = type;
    scene->sendCmd(&create, sizeof(create));

    Cmd::Session::t_RetSceneuserPkGame_SceneSession send1;	//返回场景角色对战数据
    send1.userID = pUser1->id;
    send1.groupID = groupID;
    send1.cardsNumber = cardsNumber1;
    send1.type = type;
    pUser1->scene->sendCmd(&send1, sizeof(send1));

    Cmd::stRetHeroFightMatchUserCmd send3;			//告诉玩家匹配成功
    send3.fightType = type;
    send3.success = 1;
    pUser1->sendCmdToMe(&send3, sizeof(send3));

    Cmd::stRetHeroIntoBattleSceneUserCmd into;
    into.sceneNumber = sceneNumber;
    pUser1->sendCmdToMe(&into, sizeof(into));

    int areaNum = zMisc::randBetween(0, 7);
    DWORD x = zMisc::randBetween(battlezonepos[areaNum][0], battlezonepos[areaNum][0]+3);
    DWORD y = zMisc::randBetween(battlezonepos[areaNum][1], battlezonepos[areaNum][1]+3);

    Cmd::Session::t_TelePKGame_SceneSession send;		    //拉玩家去对战场景
    strncpy(send.mapName, mapName, MAX_NAMESIZE-1);
    send.userID = pUser1->id;
    send.x = x;
    send.y = y;
    send.type = 1;
    pUser1->scene->sendCmd(&send, sizeof(send));

    return true;
}
