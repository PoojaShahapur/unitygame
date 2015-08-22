/*************************************************************************
 Author: wang
 Created Time: 2014年12月11日 星期四 10时23分17秒
 File Name: ScenesServer/ChallengeGameManager.cpp
 Description: 
 ************************************************************************/
#include "ChallengeGameManager.h"
#include "ChallengeGame.h"
#include "Zebra.h"
#include "TimeTick.h"
#include "SceneManager.h"
#include "SessionClient.h"
#include "SceneUserManager.h"
#include "SceneUser.h"

ChallengeGameManager::ChallengeGameManager() : _ten_sec(10)
{
    gameCount = 0;
}

ChallengeGameManager::~ChallengeGameManager()
{
    clean();
}

void ChallengeGameManager::clean()
{
    for(GAMEMAP_ITER iter=gameMap.begin(); iter!=gameMap.end();)
    {
	SAFE_DELETE(((iter++)->second));
    }
    gameMap.clear();
}

/**
 * \brief   创建一个game只包含了部分基础信息
 * \param
 * \return
*/
DWORD ChallengeGameManager::createGame(Cmd::Session::t_CreateNewPkGame_SceneSession *cmd)
{
    FunctionTime func_time(0, __FUNCTION__, "[对战]创建对战总耗时", 1000);

    if(!cmd)
	return 0;
    SceneUser *u1 = SceneUserManager::getMe().getUserByID(cmd->userID1);
    SceneUser *u2 = SceneUserManager::getMe().getUserByID(cmd->userID2);
    if(!u1 || !u2)
    {//创建对战的时候，玩家还没到对战目标场景
	Zebra::logger->error("[卡牌]ChallengeGameManager::createGame 发现有玩家不在线");
    }
    ChallengeGame *newGame = new ChallengeGame(cmd->groupID, ((Cmd::Session::ChallengeGameType)cmd->type), cmd->userID1, cmd->cardsNumber1, cmd->userID2, cmd->cardsNumber2, cmd->sceneNumber);
    if(newGame)
    {
	gameMap.insert(std::make_pair(cmd->groupID, newGame));
	increaseGameCount();
	return newGame->getGameID();
    }
    else
    {
	Zebra::logger->error("[卡牌]建立对战分配内存失败");
	return 0;
    }
    return 0;
}

bool ChallengeGameManager::isFighting(SceneUser *pUser)
{
    if(!pUser)
	return false;
    GAMEMAP_ITER it=gameMap.find(pUser->ctData.groupID);
    if(it == gameMap.end())
    {
	return false;
    }
    return true;
}

bool ChallengeGameManager::hasUnfinishedGame(SceneUser *pUser)
{
    if(!pUser)
	return false;
    GAMEMAP_ITER it=gameMap.find(pUser->ctData.groupID);
    if(it == gameMap.end())
    {
	return false;
    }
    if(it->second)
    {
	if(it->second->state == CHALLENGE_STATE_PREPARE 
		|| it->second->state == CHALLENGE_STATE_BATTLE)
	    return true;
    }
    return false;
}

zCard* ChallengeGameManager::getUserCardByThisID(DWORD gameID, DWORD thisID)
{
    GAMEMAP_ITER it=gameMap.find(gameID);
    if(it == gameMap.end())
    {
	return false;
    }
    ChallengeGame *game = it->second;
    if(game)
    {
	return game->slots.gcm.getObjectByThisID(thisID);
    }
    return NULL;
}

bool ChallengeGameManager::getUserHero(DWORD gameID, DWORD userID, zCard *ret)
{
    GAMEMAP_ITER it=gameMap.find(gameID);
    if(it == gameMap.end())
    {
	return false;
    }
    ChallengeGame *game = it->second;
    if(game)
    {
	ret = game->getSelfHero(userID);
	return true;
    }
    return false;
}

ChallengeGame* ChallengeGameManager::getGameByID(DWORD gameID)
{
    ChallengeGame *game = NULL;
    GAMEMAP_ITER it=gameMap.find(gameID);
    if(it != gameMap.end())
    {
	game = it->second;
    }
    return game;
}
/**
 * \brief   初始化一个game中的对战数据
 * \param
 * \return
*/
bool ChallengeGameManager::initGameData(SceneUser *pUser)
{
    if(!pUser)
	return false;
    GAMEMAP_ITER it=gameMap.find(pUser->ctData.groupID);
    if(it == gameMap.end())
    {
	pUser->ctData.clear();
	return false;
    }
    ChallengeGame *game = it->second;
    if(game)
    {
	game->init(pUser);
    }
    return true;
}

bool ChallengeGameManager::handlesecondEnter(SceneUser *pUser)
{
    if(!pUser)
	return false;
    GAMEMAP_ITER it=gameMap.find(pUser->ctData.groupID);
    if(it == gameMap.end())
    {
	return false;
    }
    ChallengeGame *game = it->second;
    if(game)
    {
	game->secondEnter(pUser);
    }
    return true;
}

bool ChallengeGameManager::handleStartGame(SceneUser &user, BYTE change)
{
    GAMEMAP_ITER it=gameMap.find(user.ctData.groupID);
    if(it == gameMap.end())
    {
	return false;
    }
    ChallengeGame *game = it->second;
    if(game)
    {
	game->startGame(user, change);
    }
    return true;
}

bool ChallengeGameManager::handleEndOneRound(SceneUser &user)
{
    GAMEMAP_ITER it=gameMap.find(user.ctData.groupID);
    if(it == gameMap.end())
    {
	return false;
    }
    ChallengeGame *game = it->second;
    if(game)
    {
	game->endUserRound(user);
    }
    return true;
}

bool ChallengeGameManager::handleGiveUpBattle(SceneUser &user)
{
    GAMEMAP_ITER it=gameMap.find(user.ctData.groupID);
    if(it == gameMap.end())
    {
	return false;
    }
    ChallengeGame *game = it->second;
    if(game)
    {
	game->giveUpBattle(user);
    }
    return true;
}

bool ChallengeGameManager::handleCardAttackMagic(SceneUser &user, const Cmd::stCardAttackMagicUserCmd *rev)
{
    GAMEMAP_ITER it=gameMap.find(user.ctData.groupID);
    if(it == gameMap.end())
    {
	return false;
    }
    ChallengeGame *game = it->second;
    if(game)
    {
	game->cardAttackMagic(user, rev);
    }
    return true;
}

bool ChallengeGameManager::handleCardMoveAndAttackMagic(SceneUser &user, const Cmd::stCardMoveAndAttackMagicUserCmd *rev)
{
    GAMEMAP_ITER it=gameMap.find(user.ctData.groupID);
    if(it == gameMap.end())
    {
	return false;
    }
    ChallengeGame *game = it->second;
    if(game)
    {
	game->cardMoveAndAttack(user, rev);
    }
    return true;
}

void ChallengeGameManager::timer()
{
    std::vector<DWORD> del_vec;
    std::vector<DWORD>::iterator iter;
    for(GAMEMAP_ITER it=gameMap.begin(); it!=gameMap.end(); ++it)
    {
	if(it->second)
	{
	    if(CHALLENGE_STATE_CANCLEAR == it->second->state)
	    {
		Cmd::Session::t_PutOneGameIDBack_SceneSession cmd;		//回收ID
		cmd.type = it->second->gameType;
		cmd.gameID = it->second->getGameID();
		sessionClient->sendCmd(&cmd, sizeof(cmd));

		del_vec.push_back(it->first);
		SAFE_DELETE((it->second));		    //释放掉一个战场的内存
	    }
	    else
	    {
		it->second->timer();
	    }
	}
    }
    for(iter=del_vec.begin(); iter!=del_vec.end(); iter++)
    {
	gameMap.erase(*iter);
	decreaseGameCount();
    }

    if(_ten_sec(SceneTimeTick::currentTime.sec()))
    {//同步服务器战斗总局数到会话
	WORD countryID = SceneManager::getInstance().getServerCountryID();
	Cmd::Session::t_NotifySceneServerGameCount_SceneSession send;
	send.countryID = countryID;
	send.gameCount = getGameCount();
	sessionClient->sendCmd(&send, sizeof(send));
	//Zebra::logger->debug("[卡牌]发送当前场景对战局数到会话 countryID:%u(%u)",countryID, send.gameCount);
    }
}

/////////////////////////////////////无敌的分割线////////////////////////////////////////////
unsigned int ChallengeTmpData::saveChallengeData(unsigned char* dest)
{
    if(!dest)
	return 0;
    unsigned int len = 0;
    bcopy(&groupID, dest, sizeof(groupID));
    len += sizeof(groupID);
    bcopy(&cardsIndex, dest+len, sizeof(cardsIndex));
    len += sizeof(cardsIndex);
    bcopy(&gameType, dest+len, sizeof(gameType));
    len += sizeof(gameType);
#ifdef _WC_DEBUG
    Zebra::logger->debug("[对战临时数据]二进制保存字节数:%u", len);
#endif
    return len;
}


unsigned int ChallengeTmpData::loadChallengeData(unsigned char* src)
{
    if(!src)
	return 0;
    unsigned int len = 0;
    groupID = *((DWORD*)src);
    len += sizeof(DWORD);
    cardsIndex = *((DWORD*)(src+len));
    len += sizeof(DWORD);
    gameType = *((BYTE*)(src+len));
    len += sizeof(BYTE);
#ifdef _WC_DEBUG
    Zebra::logger->debug("[对战临时数据]二进制加载字节数:%u", len);
#endif
    return len;
}
