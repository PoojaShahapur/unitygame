/*************************************************************************
 Author: wang
 Created Time: 2014年12月11日 星期四 10时47分11秒
 File Name: ScenesServer/ChallengeGame.cpp
 Description: 
 ************************************************************************/
#include "ChallengeGame.h"
#include "Zebra.h"
#include "TimeTick.h"
#include "SceneUser.h"
#include "SceneUserManager.h"
#include "zDatabaseManager.h"
#include "zXML.h"
#include "Chat.h"

using namespace xml;

ChallengeGame::ChallengeGame(const DWORD groupID, const Cmd::Session::ChallengeGameType type, const DWORD playerID1, const DWORD cardsNumber1, const DWORD playerID2, const DWORD cardsNumber2, const DWORD sceneNumber):slots(this)
{
    gameType = type;
    gameID = groupID;
    players[0].playerID = playerID1;
    players[1].playerID = playerID2;
    players[0].cardsNumber = cardsNumber1;
    players[1].cardsNumber = cardsNumber2;

    _sceneNumber = sceneNumber;

    createTime = SceneTimeTick::currentTime.sec();
    challengePrepareTime = 0;
    challengeLastTime = 0;
    setState(CHALLENGE_STATE_NONE);	    
    totalRoundCount = 0;
    setPrivilege(0);
    currRoundStartTime = 0;
    currRoundEndTime = 0;
    cfg_prepareTime = battle.limit.preparetime;
    cfg_roundTime = battle.limit.roundtime;
    notOnlineTime = 0;
    Zebra::logger->debug("[卡牌]创建了一个game,场次(%u)类型:%s(%u) 玩家1(%u，套牌:%u) 玩家2(%u, 套牌:%u)",gameID, gameType2Name(gameType), gameType, playerID1, cardsNumber1, playerID2, cardsNumber2);
    
    actionList.clear();
    hurtList.clear();
    cureList.clear();
    actionHaloList.clear();
}

ChallengeGame::~ChallengeGame()
{
}

bool ChallengeGame::checkMp(DWORD playerID, DWORD needMp)
{
    if(!isInGame(playerID))
	return false;
    if(playerID == players[0].playerID)
    {
	return players[0].checkMp(needMp);	
    }
    else if(playerID == players[1].playerID)
    {
	return players[1].checkMp(needMp);	
    }
    return false;
}

bool ChallengeGame::reduceMp(DWORD playerID, DWORD needMp)
{
    if(!isInGame(playerID))
	return false;
    bool ret = false;
    if(playerID == players[0].playerID)
    {
	ret = players[0].reduceMp(needMp);	
    }
    else if(playerID == players[1].playerID)
    {
	ret = players[1].reduceMp(needMp);	
    }
    if(ret)
    {
	sendMpInfo();
    }
    return ret;
}

bool ChallengeGame::isInGame(DWORD playerID)
{
    if(playerID == players[0].playerID || playerID == players[1].playerID)
	return true;
    return false;
}

/**
 * \brief   获得先手角色ID
 * \param
 * \return
*/
DWORD ChallengeGame::generateUpperHand()
{
    DWORD id= 0;
    if(zMisc::selectByPercent(50))
    {
	id = players[0].playerID;
    }
    else
    {
	id = players[1].playerID;
    }
    return id;
}

void ChallengeGame::logToObj(zCard* o, char *desc)
{
    if(!o)
	return;
    zCard::logger(o->createid,o->data.qwThisID,o->base->name,0,0,0,0,NULL,o->playerID, getPlayerName(o->playerID),desc,o->base,o->base->kind,o->gameID);
}

void ChallengeGame::logScene(zCard* o, int slot)
{
    if(!o)
	return;
    Zebra::logger->debug("[卡牌] PVP 战场:%u 增加了一张:(%s)牌 到:(%s(%u,%u)) 里 角色(%s,%u)",o->gameID, o->base->name, slotType2Name(slot).c_str(), o->data.pos.x, o->data.pos.y, getPlayerName(o->playerID), o->playerID);
}

/**
 * \brief 初始化角色牌库
 * \param
 * \return
*/
bool ChallengeGame::initCardsLib(SceneUser *pUser, std::vector<DWORD> &libVec)
{
    if(!pUser)
	return false;
    DWORD index = pUser->ctData.cardsIndex;
    if(index < 1000)	//基本套牌算法(从自身职业+中立中随机出M张)
    {}
    else if(index >= 1000)	//自定义套牌算法(用玩家自己设定的套牌去填充)
    {
	if(GroupCardManager::getMe().initOneChallengeCards(*pUser, index, libVec))
	{
	    std::random_shuffle(libVec.begin(), libVec.end());
	    Zebra::logger->debug("[卡牌] (%u)场次 (%s,%u) 初始化牌库", gameID, pUser->name, pUser->id);
	    for(std::vector<DWORD>::iterator it=libVec.begin(); it!=libVec.end(); it++)
	    {
		zCardB *cb = cardbm.get(*it);
		if(cb)
		{
		    Zebra::logger->debug("[卡牌] (%u)场次 (%s,%u) (%s,%u)", gameID, pUser->name, pUser->id, cb->name, cb->id);
		}
	    }
	    return true;
	}
    }
    return false;
}

/**
 * \brief 初始化对战基础信息
 * \param
 * \return
*/
bool ChallengeGame::initBaseBattleInfo()
{
    if(isPVP())
    {
	SceneUser *pUser1 = SceneUserManager::getMe().getUserByID(players[0].playerID);
	if(pUser1)
	{
	    DWORD occupation1 = GroupCardManager::getMe().getOccupationByIndex(*pUser1, pUser1->ctData.cardsIndex);
	    HeroInfoManager::getMe().setHeroInUse(*pUser1, occupation1);
	    DWORD hCard = HeroInfoManager::getMe().getHCardByOccupation(occupation1);
	    DWORD sCard = HeroInfoManager::getMe().getSCardByOccupation(occupation1);
	    CardToHeroSlot(players[0].playerID, hCard);
	    CardToSkillSlot(players[0].playerID, sCard);
	}

	SceneUser *pUser2 = SceneUserManager::getMe().getUserByID(players[1].playerID);
	if(pUser2)
	{
	    DWORD occupation2 = GroupCardManager::getMe().getOccupationByIndex(*pUser2, pUser2->ctData.cardsIndex);
	    HeroInfoManager::getMe().setHeroInUse(*pUser2, occupation2);
	    DWORD hCard2 = HeroInfoManager::getMe().getHCardByOccupation(occupation2);
	    DWORD sCard2 = HeroInfoManager::getMe().getSCardByOccupation(occupation2);
	    CardToHeroSlot(players[1].playerID, hCard2);
	    CardToSkillSlot(players[1].playerID, sCard2);
	}
    }
    else if(isPVE())
    {
	SceneUser *pUser1 = SceneUserManager::getMe().getUserByID(players[0].playerID);
	if(pUser1)
	{
	    DWORD occupation1 = GroupCardManager::getMe().getOccupationByIndex(*pUser1, pUser1->ctData.cardsIndex);
	    HeroInfoManager::getMe().setHeroInUse(*pUser1, occupation1);
	    DWORD hCard = HeroInfoManager::getMe().getHCardByOccupation(occupation1);
	    DWORD sCard = HeroInfoManager::getMe().getSCardByOccupation(occupation1);
	    CardToHeroSlot(players[0].playerID, hCard);
	    CardToSkillSlot(players[0].playerID, sCard);
	}
	
	//初始化BOSS的英雄和英雄技能
    }

	
    //初始化起手牌1
    for(BYTE i=0; i<players[0].prepareHand.size(); i++)
    {
	CardToHandSlot(players[0].playerID, players[0].prepareHand[i], true);
    }

    //初始化起手牌2
    for(BYTE i=0; i<players[1].prepareHand.size(); i++)
    {
	CardToHandSlot(players[1].playerID, players[1].prepareHand[i], true);
    }

    DWORD lowerHand = 0;
    if(upperHand == players[0].playerID)
    {
	lowerHand = players[1].playerID;
    }
    else if(upperHand == players[1].playerID)
    {
	lowerHand = players[0].playerID;
    }
    CardToHandSlot(lowerHand, battle.limit.luckyCoin, true);

    players[0].clearPrepareHand();
    players[1].clearPrepareHand();

    return true;
}

bool ChallengeGame::init(SceneUser *pUser)
{
    if(!pUser)
	return false;
    if(getGameType() != pUser->ctData.gameType)
	return false;
    if(getState() != CHALLENGE_STATE_NONE)
	return false;
    if(isPVP())
    {
	if(pUser->id == players[0].playerID && pUser->ctData.cardsIndex == players[0].cardsNumber)
	{
	    strncpy(players[0].playerName, pUser->name, MAX_NAMESIZE);
	    if(initCardsLib(pUser, players[0].cardsLibVec))
	    {
		players[0].setInit();
	    }
	    Zebra::logger->debug("[卡牌]初始化第(%u)场次对战玩家1(%s,%u)数据 %s",gameID, pUser->name, pUser->id, players[0].isInited()?"OK":"FAIL");
	}
	else if(pUser->id == players[1].playerID && pUser->ctData.cardsIndex == players[1].cardsNumber)
	{
	    strncpy(players[1].playerName, pUser->name, MAX_NAMESIZE);
	    if(initCardsLib(pUser, players[1].cardsLibVec))
	    {
		players[1].setInit();
	    }
	    Zebra::logger->debug("[卡牌]初始化第(%u)场次对战玩家2(%s,%u)数据 %s",gameID, pUser->name, pUser->id, players[1].isInited()?"OK":"FAIL");
	}
    }
    else if(isPVE())
    {
	if(pUser->id == players[0].playerID && pUser->ctData.cardsIndex == players[0].cardsNumber)
	{
	    strncpy(players[0].playerName, pUser->name, MAX_NAMESIZE);
	    if(initCardsLib(pUser, players[0].cardsLibVec))
	    {
		players[0].setInit();
	    }
	    Zebra::logger->debug("[卡牌]初始化第(%u)场次对战玩家1(%s,%u)数据 %s",gameID, pUser->name, pUser->id, players[0].isInited()?"OK":"FAIL");

	    //初始化BOSS的牌库
	}
    }
    return true;
}


/**
 * \brief 玩家掉线后二次进入
 * \param
 * \return
*/
bool ChallengeGame::secondEnter(SceneUser *pUser)
{
    if(!pUser)
	return false;
    if(getGameType() != pUser->ctData.gameType)
	return false;
    if(!isInGame(pUser->id))
    {
	return false;
    }

    if(isPVP())
    {
	if(pUser->id == players[0].playerID && pUser->ctData.cardsIndex == players[0].cardsNumber)
	{
	    Zebra::logger->debug("[卡牌] PVP 战场:%u 玩家1(%s,%u)掉线后再次进入游戏",gameID, pUser->name, pUser->id);
	}
	else if(pUser->id == players[1].playerID && pUser->ctData.cardsIndex == players[1].cardsNumber)
	{
	    Zebra::logger->debug("[卡牌] PVP 战场:%u 玩家2(%s,%u)掉线后再次进入游戏",gameID, pUser->name, pUser->id);
	}
	else
	    return false;

	if(getState() == CHALLENGE_STATE_PREPARE)
	{
	    Cmd::stRetHeroIntoBattleSceneUserCmd into;
	    into.sceneNumber = _sceneNumber;
	    pUser->sendCmdToMe(&into, sizeof(into));

	    sendEnemyBaseInfo();
	    sendAllCardList(pUser);

	    //sendFirstHandCard(upperHand);
	    Cmd::stRetRefreshBattleStateUserCmd send2;
	    send2.state = getState();
	    pUser->sendCmdToMe(&send2, sizeof(send2));
	}
	else if(getState() == CHALLENGE_STATE_BATTLE)
	{
	    Cmd::stRetHeroIntoBattleSceneUserCmd into;
	    into.sceneNumber = _sceneNumber;
	    pUser->sendCmdToMe(&into, sizeof(into));

	    sendEnemyBaseInfo();
	    sendAllCardList(pUser);
	    sendLibNumInfo();
	    sendMpInfo();
	    sendEnemyHandNum(pUser); 

	    Cmd::stRetRefreshBattlePrivilegeUserCmd send;
	    if(isHavePrivilege(pUser->id))
	    {
		send.priv = 1;
	    }
	    else
	    {
		send.priv = 2;
	    }
	    pUser->sendCmdToMe(&send, sizeof(send));

	    Cmd::stRetRefreshBattleStateUserCmd send2;
	    send2.state = getState();
	    pUser->sendCmdToMe(&send2, sizeof(send2));
	}
    }
    else if(isPVE())
    {

    }
    return true;
}

bool ChallengeGame::cardAttackMagic(SceneUser &user, const Cmd::stCardAttackMagicUserCmd *rev)
{
    if(!isHavePrivilege(user.id))
    {
	Zebra::logger->error("[PK] 不是你(%s:%u)的回合 你点个毛线", user.name, user.id);
	return false;
    }
    if(!rev)
	return false;
    bool ret = false;
    ret = cardNormalAttack(user, rev);
    if(!ret)
    {
	Cmd::stRetCardAttackFailUserCmd send;
	send.dwAttThisID = rev->dwAttThisID;
	user.sendCmdToMe(&send, sizeof(send));
    }
    return ret;
}

/**
 * \brief 普通攻击
 * \param
 * \return
*/
bool ChallengeGame::cardNormalAttack(SceneUser &user, const Cmd::stCardAttackMagicUserCmd *rev)
{
    zCard *pAtt = this->slots.gcm.getObjectByThisID(rev->dwAttThisID);
    zCard *pDef = this->slots.gcm.getObjectByThisID(rev->dwDefThisID);
    if(!pDef || !pAtt)
	return false;

    if(!checkSelectedTarget(pAtt, 0, rev->dwDefThisID))
    {
	Zebra::logger->error("普通攻击  你的目标选择错误");
	Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "普通攻击 攻击验证失败或者目标选择错误");
	return false;
    }
    Zebra::logger->debug("[PK] 攻击者%s(%s,%u) 防御者%s", pAtt->base->name, user.name, user.id, pDef->base->name);
    
    DWORD now = SceneTimeTick::currentTime.sec();
    DWORD pAttHisID = 0;
    DWORD pDefHisID = 0;
    CardToRecordSlot(user.id, pAtt, now, pAttHisID);
    CardToRecordSlot(user.id, pDef, now, pDefHisID);

    std::vector<DWORD> vec;
    vec.push_back(pDefHisID);
    sendBattleHistoryInfo(pAttHisID, vec);	    //普通攻击历史

    if(pAtt->pk.sAttackID)
    {
	Cmd::stCardAttackMagicUserCmd in;
	in.dwAttThisID = rev->dwAttThisID;
	in.dwDefThisID = 0;
	in.dwMagicType = pAtt->pk.sAttackID;
	addActionList(in);
	startOneFlow();
	action("开始攻击");		    //开始攻击时触发
	endOneFlow();
	pAtt = this->slots.gcm.getObjectByThisID(rev->dwAttThisID);
	pDef = this->slots.gcm.getObjectByThisID(rev->dwDefThisID);
	if(!pAtt || !pDef)
	{
	    return true;
	}
    }

    if(pDef->pk.beAttackID)
    {
	Cmd::stCardAttackMagicUserCmd in;
	in.dwAttThisID = rev->dwDefThisID;
	in.dwDefThisID = 0;
	in.dwMagicType = pDef->pk.beAttackID;
	addActionList(in);
	startOneFlow();
	action("被攻击");		    //被攻击时触发
	endOneFlow();
	pAtt = this->slots.gcm.getObjectByThisID(rev->dwAttThisID);
	pDef = this->slots.gcm.getObjectByThisID(rev->dwDefThisID);
	if(!pAtt || !pDef)
	{
	    return true;
	}
    }
    
    bool attHurt = false;
    bool defHurt = false;
    DWORD oldDefHp = pDef->data.hp;
    DWORD oldAttHp = pAtt->data.hp;

    startOneFlow();

    pDef->doNormalPK(pAtt, pDef);
    std::vector<DWORD> defList;
    defList.push_back(pDef->data.qwThisID);
    refreshClient(pAtt->data.qwThisID, defList, 0);
    if(oldDefHp > pDef->data.hp)
    {
	defHurt = true;
    }
    if(oldAttHp > pAtt->data.hp)
    {
	attHurt = true;
    }

    dealAttackEndAction(pAtt->data.qwThisID, pDef->data.qwThisID, defHurt);	//pAtt攻击结束效果
    dealAttackEndAction(pDef->data.qwThisID, pAtt->data.qwThisID, attHurt);	//pDef攻击结束效果

    dealHaloEffectAction();

    const DWORD NONE_SKILL_ID = 1;	    //一个空的技能
    Cmd::stCardAttackMagicUserCmd in;
    in.dwAttThisID = pAtt->data.qwThisID;
    in.dwDefThisID = 0;
    in.dwMagicType = NONE_SKILL_ID;
    addActionList(in);

    action( "普通攻击");
    endOneFlow();
    

    return true;

}

/**
 * \brief 法术牌攻击
 * \param
 * \return
*/
bool ChallengeGame::cardSkillAttack(SceneUser &user, const Cmd::stCardAttackMagicUserCmd *rev)
{
    zCard *pAtt = this->slots.gcm.getObjectByThisID(rev->dwAttThisID);
    if(!pAtt)
	return false;
    if(!pAtt->isMagicCard() && !pAtt->isHeroMagicCard())
    {
	Zebra::logger->error("SKILL  %s 不是法术卡 也不是英雄技能 ",pAtt->base->name);
	Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "释放技能失败  技能拥有者既不是法术 也不是英雄能力");
	return false;
    }
    if(pAtt->base->needTarget)
    {
	zCard *pDef = this->slots.gcm.getObjectByThisID(rev->dwDefThisID);
	if(pDef && pDef->canNotAsFashuTarget())
	{
	    Zebra::logger->error("SKILL  你的目标选择错误 该目标不能被法术和英雄技能选择");
	    Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "法术牌 释放技能失败 该目标不能被法术和英雄技能选择");
	    return false;
	}
	if(!checkSelectedTarget(pAtt, pAtt->base->needTarget, rev->dwDefThisID))
	{
	    Zebra::logger->error("SKILL  你的目标选择错误");
	    Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "法术牌 释放技能失败 你的目标选择错误");
	    return false;
	}
    }
    if(pAtt->isHeroMagicCard() && !pAtt->checkAttackTimes())
    {
	Zebra::logger->error("SKILL  英雄技能你(%u)只能使用一次 ", pAtt->playerID);
	Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "释放技能失败  英雄能力每回合只能使用一次");
	return false;
    }
    
    if(!checkMp(pAtt->playerID, pAtt->data.mpcost))
    {
	Zebra::logger->error("SKILL  这个技能需要你(%u) 有(%u)个法力水晶 ", pAtt->playerID, pAtt->data.mpcost);
	Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "释放技能失败  这个技能需要你有%u个法力水晶才可以",pAtt->data.mpcost);
	return false;
    }
    else
    {
	reduceMp(pAtt->playerID, pAtt->data.mpcost);
    }

    DWORD pAttHisID = 0;
    CardToRecordSlot(pAtt->playerID, pAtt, SceneTimeTick::currentTime.sec(), pAttHisID);

    if(pAtt->isMagicCard())
    {
	pAtt->processDeath(NULL, pAtt);
    }
    else if(pAtt->isHeroMagicCard())
    {
	pAtt->addAttackTimes();
    }

    dealUseMagicCardAction();	    //使用法术牌触发其他效果
    action( "法术牌使用");
   
    zCard *hero = getSelfHero(user.id);
    if(hero)
    {
	Cmd::stCardAttackMagicUserCmd in;
	in.dwAttThisID = hero->data.qwThisID;
	in.dwDefThisID = rev->dwDefThisID;
	in.dwMagicType = rev->dwMagicType;
	addActionList(in);
	action( "法术牌或英雄技能", true);
    }

    return true;
}

/**
 * \brief 出牌处理
 * \param
 * \return
*/
bool ChallengeGame::cardMoveAndAttack(SceneUser &user, const Cmd::stCardMoveAndAttackMagicUserCmd *rev)
{
    if(!isInGame(user.id))
	return false;
    if(!isHavePrivilege(user.id))
    {
	Zebra::logger->error("[卡牌] 不是你的回合。操作非法  %s(%u)", user.name, user.id);
	return false;
    }
    zCard *pAtt = this->slots.gcm.getObjectByThisID(rev->dwAttThisID);
    if(!pAtt)
	return false;
    DWORD cardID = pAtt->base->id;
    bool magicCard = false;
    if(pAtt->isMagicCard())
	magicCard = true;
    if(pAtt->isMagicCard() || pAtt->isHeroMagicCard())
    {
	Cmd::stCardAttackMagicUserCmd fashu;
	fashu.dwAttThisID = rev->dwAttThisID;
	fashu.dwDefThisID = rev->dwDefThisID;
	fashu.dwMagicType = pAtt->pk.magicID;
	
	startOneFlow();
	bool ret = cardSkillAttack(user, &fashu);
	if(!ret)
	{
	    Cmd::stRetCardAttackFailUserCmd send;
	    send.dwAttThisID = rev->dwAttThisID;
	    user.sendCmdToMe(&send, sizeof(send));
	}
	else
	{
	    if(magicCard)
		sendOutCardInfo(cardID);
	}
	endOneFlow();
    }
    else
    {
	if(pAtt->isAttend())
	{
	    if(user.id == players[0].playerID)
	    {
		if(this->slots.main1.space() == 0)
		{
		    Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "你的随从已经满了");
		    return false;
		}
	    }
	    else
	    {
		if(this->slots.main2.space() == 0)
		{
		    Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "你的随从已经满了");
		    return false;
		}
	    }
	}
	//处理逻辑主要思路
	//如果战吼效果需要选择目标，判断选择的目标是否合法
	//继续判断移动位置是否合法
	//判断蓝耗是否满足
	//接下来做移动处理
	//接下来运行战吼效果
	//处理结束


	bool noTarget = false;
	if(pAtt->hasShout())    //战吼处理
	{
	    if(pAtt->base->shoutTarget) //需要目标的战吼
	    {
		if(canFindTarget(pAtt, pAtt->base->shoutTarget))    //确实有可选的目标
		{
		    if(!checkSelectedTarget(pAtt, pAtt->base->shoutTarget, rev->dwDefThisID))
		    {
			Zebra::logger->error("SKILL  你的目标选择错误");
			Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "战吼 释放技能失败 你的目标选择错误");
			return false;
		    }
		}
		else
		{
		    noTarget = true;
		}
	    }
	}
	
	startOneFlow();
	if(!moveUserCard(user, rev->dwAttThisID, rev->dst))  //这里扣了蓝,处理移动,做了历史记录
	{
	    Zebra::logger->error("处理%s移动失败",pAtt->base->name);
	    return false;
	}
	dealOtherEffectAction(pAtt->data.qwThisID, Cmd::ACTION_TYPE_USEATTEND);	    //使用随从牌
	action( "使用随从牌");		    //使用随从牌触发

	if(pAtt->hasShout() && !noTarget)    //战吼处理
	{
	    Cmd::stCardAttackMagicUserCmd in;
	    in.dwAttThisID = rev->dwAttThisID;
	    in.dwDefThisID = rev->dwDefThisID;
	    in.dwMagicType = pAtt->pk.shoutID;
	    addActionList(in);
	    action("释放战吼", true);		    //释放战吼
	}

	dealOtherEffectAction(pAtt->data.qwThisID, Cmd::ACTION_TYPE_ATTEND_IN);	    //随从上场
	dealHaloEffectAction();	    //光环检测
	sendOutCardInfo(cardID);
	endOneFlow();
    }
    return false;
}

void ChallengeGame::setState(ChallengeState s)
{
    state = s;

    Cmd::stRetRefreshBattleStateUserCmd send;
    send.state = state;

    SceneUser *pUser1 = SceneUserManager::getMe().getUserByID(players[0].playerID);
    if(pUser1)
    {
	pUser1->sendCmdToMe(&send, sizeof(send));
    }

    SceneUser *pUser2 = SceneUserManager::getMe().getUserByID(players[1].playerID);
    if(pUser2)
    {
	pUser2->sendCmdToMe(&send, sizeof(send));
    }
}

ChallengeState ChallengeGame::getState() const
{
    return state;
}

void ChallengeGame::setGameType(Cmd::Session::ChallengeGameType type)
{
    gameType = type;
}

Cmd::Session::ChallengeGameType ChallengeGame::getGameType() const
{
    return gameType;
}

void ChallengeGame::timer()
{
    switch(getState())
    {
	case CHALLENGE_STATE_NONE:	//游戏已创建(待双方数据填充)
	    {
		DWORD now = SceneTimeTick::currentTime.sec();
		if(now > createTime+200)
		{
		    Zebra::logger->debug("[卡牌] 第:%u场次 对战创建好:%u 秒的时候还没有初始化双方数据", gameID, now-createTime);
		    on_GameOver();
		    break;
		}
		if(players[0].isInited() && players[1].isInited())	//如果对战双方数据都初始化完毕
		{
		    sendEnemyBaseInfo();
		    //do something at end of init
		    upperHand = generateUpperHand();
		    //初始化第一把手牌
		    DWORD now = SceneTimeTick::currentTime.sec();
		    if(now > createTime)
		    {
			Zebra::logger->debug("[卡牌] 第:%u场次 初始化总耗时 :%u 秒", gameID, now-createTime);
		    }
		    setPrivilege(upperHand);
		    Zebra::logger->debug("[卡牌]初始化第(%u)场次对战玩家数据完毕 先手的玩家是:%u",gameID, upperHand);
		    sendFirstHandCard(upperHand);
		    setState(CHALLENGE_STATE_PREPARE);	   
		}
	    }
	    break;
	case CHALLENGE_STATE_PREPARE:	//准备状态，还没开打
	    {
		challengePrepareTime++;
		Zebra::logger->debug("[卡牌]现在是对战准备阶段哦~ 秒数:%u",challengePrepareTime);
		if(challengePrepareTime >= cfg_prepareTime || (players[0].prepare && players[1].prepare))	    //准备时间过长，直接开战
		{
		    setState(CHALLENGE_STATE_BATTLE);

		    initBaseBattleInfo();

		    on_RoundStart();
		}
	    }
	    break;
	case CHALLENGE_STATE_BATTLE:	//对战状态
	    {
		challengeLastTime++;
		if(isPVP())
		{
		    if(SceneTimeTick::currentTime.sec() >= currRoundStartTime + cfg_roundTime)	    //玩家操作超时了
		    {
			on_RoundEnd();
			on_RoundStart();
		    }
		    SceneUser *pUser1 = SceneUserManager::getMe().getUserByID(players[0].playerID);
		    SceneUser *pUser2 = SceneUserManager::getMe().getUserByID(players[1].playerID);
		    if(!pUser1 && !pUser2)
		    {
			notOnlineTime++;
			if(notOnlineTime >= 5)
			{
			    Zebra::logger->debug("[卡牌] 第:%u场次 双方不在线时间超过 %u 秒，系统强制结束", gameID, notOnlineTime);
			    dealGameResult(upperHand, true);
			    on_GameOver();
			    break;
			}
		    }
		    if(totalRoundCount >= battle.limit.peaceNum)
		    {
			Zebra::logger->debug("[卡牌] 第:%u场次 对战进行了 %u 回合 ，系统强制结束", gameID, totalRoundCount);
			dealGameResult(upperHand, true);
			on_GameOver();
			break;
		    }
		}
		else if(isPVE())
		{

		}
	    }
	    break;
	case CHALLENGE_STATE_CANCLEAR:	//等待回收状态
	    {

	    }
	    break;
	default:
	    break;
    }
}

bool ChallengeGame::isPVP()
{
    using namespace Cmd::Session;
    switch(getGameType())
    {
	case CHALLENGE_GAME_RELAX_TYPE:
	case CHALLENGE_GAME_RANKING_TYPE:
	case CHALLENGE_GAME_COMPETITIVE_TYPE:
	case CHALLENGE_GAME_FRIEND_TYPE:
	    return true;
	    break;
	default:
	    break;
    }
    return false;
}

bool ChallengeGame::isPVE()
{
    using namespace Cmd::Session;
    switch(getGameType())
    {
	case CHALLENGE_GAME_PRACTISE_TYPE:
	case CHALLENGE_GAME_BOSS_TYPE:
	    return true;
	    break;
	default:
	    break;
    }
    return false;
}

void ChallengeGame::setPrivilege(DWORD playerID)
{
    privilegeUser = playerID;
    

    SceneUser *pUser = SceneUserManager::getMe().getUserByID(privilegeUser);
    SceneUser *pOther = getOther(privilegeUser);
    if(pUser)
    {
	Cmd::stRetRefreshBattlePrivilegeUserCmd send;
	send.priv = 1;
	pUser->sendCmdToMe(&send, sizeof(send));
    }
    if(pOther)
    {
	Cmd::stRetRefreshBattlePrivilegeUserCmd send;
	send.priv = 2;
	pOther->sendCmdToMe(&send, sizeof(send));
    }
}

bool ChallengeGame::isHavePrivilege(DWORD playerID)
{
    if(playerID == privilegeUser)
	return true;
    return false;
}

/**
 * \brief 回合开始
 * \param
 * \return
*/
bool ChallengeGame::on_RoundStart()
{
    currRoundStartTime = SceneTimeTick::currentTime.sec();
    totalRoundCount++;
    if(isPVP())
    {
	Cmd::stNotifyResetAttackTimesUserCmd send;
	SceneUser *pUser = SceneUserManager::getMe().getUserByID(privilegeUser);
	if(pUser)
	{
	    pUser->sendCmdToMe(&send, sizeof(send));
	}
	if(privilegeUser == players[0].playerID)
	{
	    players[0].resetMp();
	    drawOneCard(privilegeUser, players[0].cardsLibVec);
	}
	else if(privilegeUser == players[1].playerID)
	{
	    players[1].resetMp();
	    drawOneCard(privilegeUser, players[1].cardsLibVec);
	}
	else 
	    return false;

	if(pUser)
	{
	    Zebra::logger->debug("[卡牌] PVP 战场:%u %s(%u) 的回合开始 第:(%u)回合 战斗已经进行了:(%u)秒",gameID, pUser->name, pUser->id, totalRoundCount, challengeLastTime);
	}
	else
	{
	    Zebra::logger->debug("[卡牌] PVP 战场:%u 角色不在线(%u) 的回合开始 第:(%u)回合 战斗已经进行了:(%u)秒",gameID, privilegeUser, totalRoundCount, challengeLastTime);
	}

	wakeUpAllAttend();
	dealResetGameCardAttackTimes();
	dealRefreshCardState();
	dealEquipState();

	sendLibNumInfo();
	sendMpInfo();
	dealRoundStartAction();
    }
    return true;
}

/**
 * \brief 回合结束
 * \param
 * \return
*/
bool ChallengeGame::on_RoundEnd()
{
    currRoundEndTime = SceneTimeTick::currentTime.sec();
    DWORD tmpTime = 0;
    if(currRoundEndTime > currRoundStartTime)
    {
	tmpTime = currRoundEndTime - currRoundStartTime;
    }
    else
    {
	Zebra::logger->error("服务器时间被回调了 结束这场GAME");
	setState(CHALLENGE_STATE_CANCLEAR);
	return false;
    }

    dealRoundEndAction();

    if(isHavePrivilege(players[0].playerID))
    {
	setPrivilege(players[1].playerID);
	SceneUser *pUser = SceneUserManager::getMe().getUserByID(players[0].playerID);
	if(pUser)
	{
	    Zebra::logger->debug("%s 的回合结束 用时:%u秒 总回合数:%u",pUser->name, tmpTime, totalRoundCount);
	}
    }
    else if(isHavePrivilege(players[1].playerID))
    {
	setPrivilege(players[0].playerID);
	SceneUser *pUser = SceneUserManager::getMe().getUserByID(players[1].playerID);
	if(pUser)
	{
	    Zebra::logger->debug("%s 的回合结束 用时:%u秒 总回合数:%u",pUser->name, tmpTime, totalRoundCount);
	}
    }
    dealCheckFreezeState();	//清理随从的冻结状态
    return true;
}

bool ChallengeGame::on_GameOver()
{
    SceneUser *pUser1 = SceneUserManager::getMe().getUserByID(players[0].playerID);
    if(pUser1)
    {
	HeroInfoManager::getMe().clearHeroInUse(*pUser1);
    }
    SceneUser *pUser2 = SceneUserManager::getMe().getUserByID(players[1].playerID);
    if(pUser2)
    {
	HeroInfoManager::getMe().clearHeroInUse(*pUser2);
    }
    setState(CHALLENGE_STATE_CANCLEAR);
    return true;
}

/**
 * \brief 从牌库抽一张牌
 * \param playerID 玩家ID
 *	  lib 牌库的引用
 * \return
*/
bool ChallengeGame::drawOneCard(DWORD playerID, std::vector<DWORD> &lib, DWORD dwSkillID)
{
    if(!lib.empty())
    {
	DWORD id = 0;
	if(playerID == players[0].playerID)
	{
	    id = players[0].extractOneCardFromLib();
	}
	else if(playerID == players[1].playerID)
	{
	    id = players[1].extractOneCardFromLib();
	}
	if(id > 0)
	{
	    CardToHandSlot(playerID, id, false, dwSkillID);
	}
    }
    else
    {
	Zebra::logger->debug("[卡牌] 你疲劳了 玩家(%u)", playerID);
	zCardB *base = cardbm.get(battle.limit.tiredCard);
	if(!base)
	    return false;
	zCard *o = zCard::create(base, gameID, 0);
	if(!o)
	    return false;
	
	DWORD dam = 0;
	if(playerID == players[0].playerID)
	{
	    players[0].increaseTiredTimes();
	    dam = players[0].getTiredTimes();
	}
	else if(playerID == players[1].playerID)
	{
	    players[1].increaseTiredTimes();
	    dam = players[1].getTiredTimes();
	}

	DWORD pAttHisID = 0;
	DWORD pDefHisID = 0;
	CardToRecordSlot(playerID, o, SceneTimeTick::currentTime.sec(), pAttHisID);
	std::vector<DWORD> vec;
	vec.clear();
	zCard *hero = getSelfHero(playerID);
	if(hero)
	{
	    CardToRecordSlot(playerID, hero, SceneTimeTick::currentTime.sec(), pDefHisID);
	    vec.push_back(pDefHisID);
	}
	sendBattleHistoryInfo(pAttHisID, vec);	    //疲劳伤害英雄历史

	if(o)
	{//用完销毁
	    zCard::destroy(o);
	}
	if(hero)
	{
	    hero->directDamage(NULL, hero, dam);
	    hero->processDeath(NULL, hero);
	}
    }
    return true;
}

/**
 * \brief 添加到牌库
 * \param   playerID 玩家ID
 *	    cardID 卡牌ID
 * \return
*/
bool ChallengeGame::addCardToLib(const DWORD playerID, const DWORD cardID)
{
    if(!isInGame(playerID))
    {
	return false;
    }
    if(playerID == players[0].playerID)
    {
	players[0].addOneCardToLib(cardID);
    }
    else if(playerID ==players[1].playerID)
    {
	players[1].addOneCardToLib(cardID);
    }
    sendLibNumInfo();
    Zebra::logger->debug("[卡牌] 卡牌:(%u)添加到了 角色(%u)的牌库中 ", cardID, playerID);
    return true;
}

/**
 * \brief 添加到手牌槽
 * \param   playerID 玩家ID
 *	    cardID 卡牌ID
 *	    first 是否是起手添加
 * \return
*/
bool ChallengeGame::CardToHandSlot(DWORD playerID, DWORD cardID, bool first, DWORD dwSkillID)
{
    int slot = -1;
    SceneUser *pOther = NULL;
    if(playerID == players[0].playerID)
    {
	slot = CardSlots::HAND1_PACK;
	if(this->slots.hand1.space() == 0)
	{
	    sendHandFullInfo(playerID, cardID);
	    Zebra::logger->debug("[卡牌] PVP 战场:%u %s 满了 角色(%s,%u) 卡牌(%u)报废", gameID, slotType2Name(slot).c_str(), getPlayerName(playerID), playerID, cardID);
	    return false;
	}
    }
    else if(playerID == players[1].playerID)
    {
	slot = CardSlots::HAND2_PACK;
	if(this->slots.hand2.space() == 0)
	{
	    sendHandFullInfo(playerID, cardID);
	    Zebra::logger->debug("[卡牌] PVP 战场:%u %s 满了 角色(%s,%u) 卡牌(%u)报废", gameID, slotType2Name(slot).c_str(), getPlayerName(playerID), playerID, cardID);
	    return false;
	}
    }
    pOther = getOther(playerID);
    if(slot == CardSlots::HAND2_PACK || slot == CardSlots::HAND1_PACK)
    {
	zCard *outObj = NULL;
	if(CardToOneSlot(playerID, cardID, slot, outObj, "ADD 手牌槽"))
	{
	    if(outObj)
	    {
		SceneUser *pUser = SceneUserManager::getMe().getUserByID(playerID);
		if(pUser)
		{
		    Cmd::stAddBattleCardPropertyUserCmd send;
		    send.byActionType = Cmd::CARD_DATA_ADD;
		    send.slot = Cmd::CARDCELLTYPE_HAND;
		    bcopy(&outObj->data, &send.object, sizeof(t_Card));
		    send.who = 1;
		    pUser->sendCmdToMe(&send, sizeof(send));
		}
		if(pOther)
		{
		    Cmd::stAddEnemyHandCardPropertyUserCmd cmd;
		    pOther->sendCmdToMe(&cmd, sizeof(cmd));
		}

		dealHaloEffectAction();
		if(!first)
		{
		    dealDrawCardSuccessAction(outObj->data.qwThisID);
		}
	    }
	}
    }
    return false;
}

/**
 * \brief 召唤到英雄槽
 * \param   playerID 玩家ID
 *	    cardID 卡牌ID
 * \return
*/
bool ChallengeGame::CardToHeroSlot(DWORD playerID, DWORD cardID)
{
    int slot = -1;
    if(playerID == players[0].playerID)
    {
	slot = CardSlots::HERO1_PACK;
    }
    else if(playerID == players[1].playerID)
    {
	slot = CardSlots::HERO2_PACK;
    }
    zCard *outObj = NULL;
    return CardToOneSlot(playerID, cardID, slot, outObj, "ADD 英雄槽");
}

/**
 * \brief 召唤到技能槽
 * \param   playerID 玩家ID
 *	    cardID 卡牌ID
 * \return
*/
bool ChallengeGame::CardToSkillSlot(DWORD playerID, DWORD cardID)
{
    int slot = -1;
    if(playerID == players[0].playerID)
    {
	slot = CardSlots::SKILL1_PACK;
    }
    else if(playerID == players[1].playerID)
    {
	slot = CardSlots::SKILL2_PACK;
    }
    zCard *outObj = NULL;
    return CardToOneSlot(playerID, cardID, slot, outObj, "ADD 技能槽");
}

/**
 * \brief 召唤到武器槽
 * \param   playerID 玩家ID
 *	    cardID 卡牌ID
 * \return
*/
bool ChallengeGame::CardToEquipSlot(DWORD playerID, DWORD cardID)
{
    int slot = -1;
    zCard *oldEquip = NULL;
    if(playerID == players[0].playerID)
    {
	slot = CardSlots::EQUIP1_PACK;
    }
    else if(playerID == players[1].playerID)
    {
	slot = CardSlots::EQUIP2_PACK;
    }
    if(slot == CardSlots::EQUIP1_PACK || slot == CardSlots::EQUIP2_PACK)
    {
	oldEquip = getSelfEquip(playerID);
	if(oldEquip)
	{
	    equipLeave(playerID, oldEquip);
	}

	zCard *outObj = NULL;
	if(CardToOneSlot(playerID, cardID, slot, outObj, "ADD 武器槽"))
	{
	    if(outObj)
	    {
		equipEnter(playerID, outObj);

		dealOtherEffectAction(outObj->data.qwThisID, Cmd::ACTION_TYPE_ATTEND_IN);	    //随从上场
		dealHaloEffectAction();	    //光环检测
		return true;
	    }
	}
    }
    return false;
}

/**
 * \brief 召唤到战场槽
 * \param   playerID 玩家ID
 *	    cardID 卡牌ID
 *	    dwThisID 召唤到的东西的ThisID
 * \return
*/
bool ChallengeGame::CardToCommonSlot(DWORD playerID, DWORD cardID, DWORD dwSkillID)
{
    int slot = -1;
    if(playerID == players[0].playerID)
    {
	slot = CardSlots::MAIN1_PACK;
    }
    else if(playerID == players[1].playerID)
    {
	slot = CardSlots::MAIN2_PACK;
    }
    if(slot == CardSlots::MAIN1_PACK || slot == CardSlots::MAIN2_PACK)
    {
	zCard *outObj = NULL;
	if(CardToOneSlot(playerID, cardID, slot, outObj, "ADD 战场槽 召唤"))
	{
	    if(outObj)
	    {
		if(outObj->isCharge())
		{
		    outObj->setAwake();
		}
#if 0
		zCard* hero = getSelfHero(playerID);
		if(hero)
		{
		    std::vector<DWORD> summonVec;
		    summonVec.push_back(outObj->data.qwThisID);
		    refreshClient(hero->data.qwThisID, summonVec, dwSkillID, 1);
		}
#endif
		dealOtherEffectAction(outObj->data.qwThisID, Cmd::ACTION_TYPE_ATTEND_IN);	    //随从上场
		dealHaloEffectAction();	    //光环检测
		return true;
	    }
	}
    }
    return false;
}

bool ChallengeGame::copyCardToCommonSlot(DWORD playerID, zCard* card, DWORD dwSkillID)
{
    if(!card)
	return false;
    if(!isInGame(playerID))
	return false;

    int slot = -1;
    if(playerID == players[0].playerID)
    {
	slot = CardSlots::MAIN1_PACK;
    }
    else if(playerID == players[1].playerID)
    {
	slot = CardSlots::MAIN2_PACK;
    }
    if(slot == CardSlots::MAIN1_PACK || slot == CardSlots::MAIN2_PACK)
    {
	zCard *o = zCard::create(card);
	if(!o)
	    return false;

	o->playerID = playerID;
	o->playingTime = SceneTimeTick::currentTime.sec();
	logToObj(o, "ADD 战场槽 复制");
	if(this->slots.addObject(o, true, slot))
	{//通知双方玩家英雄技能情况
	    logScene(o, slot);

	    if(o->isCharge())
	    {
		o->setAwake();
	    }
	    zCard* hero = getSelfHero(playerID);
	    if(hero)
	    {
		std::vector<DWORD> summonVec;
		summonVec.push_back(o->data.qwThisID);
		refreshClient(hero->data.qwThisID, summonVec, dwSkillID, 1);
	    }

	    dealOtherEffectAction(o->data.qwThisID, Cmd::ACTION_TYPE_ATTEND_IN);	    //随从上场
	    dealHaloEffectAction();	    //光环检测
	}
	else
	{
	    zCard::destroy(o);
	    return false;
	}
	return true;
    }
    return false;
}

bool ChallengeGame::CardToRecordSlot(DWORD playerID, zCard* card, DWORD timeSequence, DWORD &dwThisID)
{
    if(!card)
	return false;
    if(!isInGame(playerID))
	return false;
    zCard *o = zCard::create(card);
    if(!o)
	return false;

    int slot = CardSlots::TOMB_PACK; 
    o->opTimeSequence = timeSequence;
    logToObj(o, "ADD 对战记录槽");
    if(this->slots.addObject(o, true, slot))
    {//通知双方玩家记录槽情况
	Zebra::logger->debug("[卡牌] PVP 战场:%u 增加了一张:(%s)牌 从(%s(%u,%u))->到:(%s(%u,%u)) 里,剩余空间%u 角色(%u)",
		gameID, o->base->name,
		"哪里", card->data.pos.x, card->data.pos.y,
		slotType2Name(slot).c_str(), o->data.pos.x, o->data.pos.y, this->slots.record.space(), playerID);
	dwThisID = o->data.qwThisID;
    }
    else
    {
	zCard::destroy(o);
	return false;
    }
    return true;
}

/**
 * \brief 随机丢弃手牌
 * \param playerID 玩家ID
 * \return
*/
bool ChallengeGame::dropOneRandomCardFromHandSlot(DWORD playerID)
{
    if(!isInGame(playerID))
	return false;
    zCard* card = NULL;
    if(playerID == players[0].playerID)
    {
	this->slots.hand1.getObjectByRandom(&card);
    }
    else if(playerID == players[1].playerID)
    {
	this->slots.hand2.getObjectByRandom(&card);
    }
    if(card)
    {
	logToObj(card, "DELETE 弃牌");
	this->slots.removeObject(playerID, card, true, true, Cmd::OP_DROPCARD_DELETE);
	return true;
    }
    return false;
}

bool ChallengeGame::CardToOneSlot(DWORD playerID, DWORD cardID, int slot, zCard*& object, char* desc)
{
    if(slot == CardSlots::HERO1_PACK || slot == CardSlots::HERO2_PACK
	    || slot == CardSlots::MAIN1_PACK || slot == CardSlots::MAIN2_PACK
	    || slot == CardSlots::HAND1_PACK || slot == CardSlots::HAND2_PACK
	    || slot == CardSlots::EQUIP1_PACK || slot == CardSlots::EQUIP2_PACK
	    || slot == CardSlots::SKILL1_PACK || slot == CardSlots::SKILL2_PACK)
    {
	zCardB *base = cardbm.get(cardID);
	if(!base)
	    return false;
	zCard *o = zCard::create(base, gameID, 0);
	if(!o)
	    return false;
	
	o->data.side = 1;
	if(playerID == players[1].playerID)
	{
	    o->data.side = 2;
	}
	o->playerID = playerID;
	logToObj(o, desc);
	if(this->slots.addObject(o, true, slot))
	{//通知双方玩家英雄情况
	    logScene(o, slot);
	    if(slot != CardSlots::HAND1_PACK && slot != CardSlots::HAND2_PACK)
	    {
		sendCardInfo(o);
	    }
	    object = o;
	}
	else
	{
	    zCard::destroy(o);
	    return false;
	}
	return true;
    }
    return false;
}

std::string ChallengeGame::slotType2Name(DWORD slot)
{
    std::string retstr = "";
    std::string name1 = players[0].playerName;
    std::string name2 = players[1].playerName;

    switch(slot)
    {
	case CardSlots::MAIN1_PACK:
	    retstr += (name1+"主战场");
	    break;
	case CardSlots::MAIN2_PACK:
	    retstr += (name2+"主战场");
	    break;
	case CardSlots::HAND1_PACK:
	    retstr += (name1+"手牌槽");
	    break;
	case CardSlots::HAND2_PACK:
	    retstr += (name2+"手牌槽");
	    break;
	case CardSlots::EQUIP1_PACK:
	    retstr += (name1+"武器槽");
	    break;
	case CardSlots::EQUIP2_PACK:
	    retstr += (name2+"武器槽");
	    break;
	case CardSlots::SKILL1_PACK:
	    retstr += (name1+"技能槽");
	    break;
	case CardSlots::SKILL2_PACK:
	    retstr += (name2+"技能槽");
	    break;
	case CardSlots::HERO1_PACK:
	    retstr += (name1+"英雄槽");
	    break;
	case CardSlots::HERO2_PACK:
	    retstr += (name2+"英雄槽");
	    break;
	case CardSlots::TOMB_PACK:
	    retstr += "对战记录槽";
	    break;
	default:
	    break;
    }
    return retstr;
}

char* ChallengeGame::gameType2Name(DWORD gameType)
{
    char* retval = NULL;
    using namespace Cmd::Session;

    switch(gameType)
    {
	case CHALLENGE_GAME_RELAX_TYPE:
	    retval = "休闲对战";
	    break;
	case CHALLENGE_GAME_RANKING_TYPE:
	    retval = "排名对战";
	    break;
	case CHALLENGE_GAME_COMPETITIVE_TYPE:
	    retval = "竞技对战";
	    break;
	case CHALLENGE_GAME_FRIEND_TYPE:
	    retval = "好友对战";
	    break;
	case CHALLENGE_GAME_PRACTISE_TYPE:
	    retval = "练习模式";
	    break;
	case CHALLENGE_GAME_BOSS_TYPE:
	    retval = "BOSS模式";
	    break;
	default:
	    break;
    }
    return retval;
}

char* ChallengeGame::getPlayerName(DWORD playerID)
{
    char* retval = NULL;
    if(playerID == players[0].playerID)
	retval = players[0].playerName;
    else if(playerID ==players[1].playerID)
	retval = players[1].playerName;
    return retval;
}

bool ChallengeGame::endUserRound(SceneUser &user)
{
    if(getState() != CHALLENGE_STATE_BATTLE)
	return false;
    if(isHavePrivilege(user.id))
    {
	on_RoundEnd();
	on_RoundStart();
	return true;
    }
    else
    {
	Zebra::logger->error("[卡牌] 不是你(%s:%u)的回合 你点个毛线", user.name, user.id);
    }
    return false;
}

bool ChallengeGame::giveUpBattle(SceneUser &user)
{
    if(!isInGame(user.id))
    {
	return false;
    }
    Zebra::logger->debug("[卡牌]对战:%u 玩家(%s:%u)认输了", gameID, user.name, user.id);
    dealGameResult(user.id, false);
    on_GameOver();
    return false;
}

bool ChallengeGame::moveUserCard(SceneUser &user, DWORD thisID, stObjectLocation dst)
{
    zCard *srcobj = this->slots.gcm.getObjectByThisID(thisID);
    if(!srcobj)
	return false;
    if(srcobj->playerID != user.id)
	return false;

    SceneUser *pOther = NULL;
    if(user.id == players[0].playerID)
    {
	pOther = SceneUserManager::getMe().getUserByID(players[1].playerID);
    }
    else if(user.id == players[1].playerID)
    {
	pOther = SceneUserManager::getMe().getUserByID(players[0].playerID);
    }

    DWORD oldLoc = srcobj->data.pos.loc();
    if(oldLoc == Cmd::CARDCELLTYPE_RECORD)
    {
	return false;
    }

    if(!checkMp(srcobj->playerID, srcobj->data.mpcost))
    {
	Zebra::logger->error("出随从牌  这个随从需要你(%u) 有(%u)个法力水晶 ", srcobj->playerID, srcobj->data.mpcost);
	Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "出普通随从牌失败  这个随从需要你有%u个法力水晶才可以",srcobj->data.mpcost);
	return false;
    }
    else
    {
	reduceMp(srcobj->playerID, srcobj->data.mpcost);
    }

    stObjectLocation org = srcobj->data.pos;
    Zebra::logger->info("[卡牌]%s 请求移动卡牌 %s(%s,%d,%d,%d)->(%s,%u,%d,%d)",user.name,srcobj->base->name,
	    "哪里",org.loc(),org.xpos(),org.ypos(),
	    "哪里",dst.loc(),dst.xpos(),dst.ypos());
    
    if(dst.dwLocation == Cmd::CARDCELLTYPE_EQUIP && srcobj->isEquip()) //目标位置是武器,摧毁旧的武器
    {
	zCard *oldEquip = getSelfEquip(user.id);
	if(oldEquip)
	{
	    equipLeave(user.id, oldEquip);
	}
    }
    else if(dst.dwLocation == Cmd::CARDCELLTYPE_SKILL && srcobj->isHeroMagicCard())	//目标位置是技能,摧毁旧的技能
    {
	zCard *oldEquip = NULL;
	if(user.id == players[0].playerID)
	{
	    this->slots.skill1.getObjectByZone(&oldEquip, dst.x, dst.y);
	}
	else if(user.id == players[1].playerID)
	{
	    this->slots.skill2.getObjectByZone(&oldEquip, dst.x, dst.y);
	}
	if(oldEquip)
	{
	    oldEquip->toDie();
	    oldEquip->processDeath(NULL, oldEquip);
	}
    }
    else if(dst.dwLocation == Cmd::CARDCELLTYPE_HERO && srcobj->isHero())	//目标位置是英雄,摧毁旧的英雄
    {
	zCard *oldEquip = NULL;
	if(user.id == players[0].playerID)
	{
	    this->slots.hero1.getObjectByZone(&oldEquip, dst.x, dst.y);
	}
	else if(user.id == players[1].playerID)
	{
	    this->slots.hero2.getObjectByZone(&oldEquip, dst.x, dst.y);
	}
	if(oldEquip)
	{
	    oldEquip->toDie();
	    oldEquip->processDeath(NULL, oldEquip);
	}
    }
    else if(dst.dwLocation == Cmd::CARDCELLTYPE_COMMON && srcobj->isAttend())	    //目标位置是主战场
    {
	if(user.id == players[0].playerID)
	{
	    if(this->slots.main1.space() == 0)
	    {
		Zebra::logger->error("PVP战场 战场上随从满了 移动失败");
		return false;
	    }
	}
	else if(user.id == players[1].playerID)
	{
	    if(this->slots.main2.space() == 0)
	    {
		Zebra::logger->error("PVP战场 战场上随从满了 移动失败");
		return false;
	    }
	}
    }
    else
    {
	return false;
    }

    WORD index = srcobj->data.pos.y;

    Cmd::stRetMoveGameCardUserCmd ret;
    ret.qwThisID = thisID;
    if(org != dst && this->slots.moveObject(&user, srcobj, dst))
    {
	srcobj->playingTime = SceneTimeTick::currentTime.sec();
	if(srcobj->isCharge())
	{
	    srcobj->setAwake();
	}
	equipEnter(user.id, srcobj);

	ret.dst = dst;
	ret.success = 1;
	if(srcobj)
	{
	    DWORD now = SceneTimeTick::currentTime.sec();
	    DWORD pMainHisID = 0;
	    CardToRecordSlot(user.id, srcobj, now, pMainHisID);
	    
	    std::vector<DWORD> vec;
	    vec.clear();
	    sendBattleHistoryInfo(pMainHisID, vec);	    //移动卡牌成功历史
	}

	if(pOther)
	{
	    if(oldLoc == Cmd::CARDCELLTYPE_HAND)
	    {
		Cmd::stDelEnemyHandCardPropertyUserCmd send;
		send.index = index;
		pOther->sendCmdToMe(&send, sizeof(send));
	    }

	    if(dst.dwLocation)
	    {
		Cmd::stAddBattleCardPropertyUserCmd send2;
		send2.slot = dst.dwLocation;
		send2.byActionType = Cmd::CARD_DATA_ADD;
		bcopy(&srcobj->data, &send2.object, sizeof(t_Card));
		send2.who = 2;
		pOther->sendCmdToMe(&send2, sizeof(send2));
	    }
	}
	Zebra::logger->info("[卡牌]%s 移动卡牌成功 %s(%s,%d,%d,%d)->(%s,%u,%d,%d)",user.name,srcobj->base->name,
		"哪里",org.loc(),org.xpos(),org.ypos(),
		"哪里",dst.loc(),dst.xpos(),dst.ypos());
    }
    else
    {
	ret.success = 0;
	Zebra::logger->info("[卡牌]%s 移动卡牌失败 %s(%s,%d,%d,%d)->(%s,%u,%d,%d)",user.name,srcobj->base->name,
		"哪里",org.loc(),org.xpos(),org.ypos(),
		"哪里",dst.loc(),dst.xpos(),dst.ypos());
    }
    user.sendCmdToMe(&ret, sizeof(ret));
    return true;
}

bool ChallengeGame::startGame(SceneUser &user, BYTE change)
{
    if(getState() != CHALLENGE_STATE_PREPARE)	//只有准备状态才可以换牌
    {
	Zebra::logger->error("[卡牌] 现在是:%u 状态 已经不能换牌了",getState());
	return false;
    }
    const DWORD countUp = 3;	//先手3张
    const DWORD countLow = 4;	//后手4张

    DWORD count = countLow;	//默认后手
    if(user.id == upperHand)
    {
	count = countUp;
    }
    if(isPVP())
    {
	if(user.id == players[0].playerID && !players[0].prepare)
	{
	    if(change > 0)
	    {
		if(players[0].changeFirstHand(count, change))
		{
		    Cmd::stRetFirstHandCardUserCmd send;
		    for(BYTE i=0; i<count; i++)
		    {
			send.id[i] = players[0].prepareHand[i];
		    }
		    user.sendCmdToMe(&send, sizeof(send));
		}
	    }
	    players[0].prepare = true;
	}
	else if(user.id == players[1].playerID && !players[1].prepare)
	{
	    if(change > 0)
	    {
		if(players[1].changeFirstHand(count, change))
		{
		    Cmd::stRetFirstHandCardUserCmd send;
		    for(BYTE i=0; i<count; i++)
		    {
			send.id[i] = players[1].prepareHand[i];
		    }
		    user.sendCmdToMe(&send, sizeof(send));
		}
	    }
	    players[1].prepare = true;
	}
    }
    else if(isPVE())
    {}
    return true;
}

void ChallengeGame::sendEnemyBaseInfo()
{
    SceneUser *pUser1 = SceneUserManager::getMe().getUserByID(players[0].playerID);
    SceneUser *pUser2 = SceneUserManager::getMe().getUserByID(players[1].playerID);
    if(pUser1 && pUser2)
    {
	Cmd::stNotifyFightEnemyInfoUserCmd send;
	strncpy(send.name, pUser2->name, MAX_NAMESIZE);
	send.occupation = GroupCardManager::getMe().getOccupationByIndex(*pUser2, players[1].cardsNumber);
	zXMLParser xml;
	BYTE *transName = xml.charConv((BYTE*)(send.name), "GB2312", "UTF-8");
	if(transName)
	{
	    strncpy(send.name, (char *)transName, MAX_NAMESIZE);
	    SAFE_DELETE_VEC(transName);
	}
	pUser1->sendCmdToMe(&send, sizeof(send));

	Cmd::stNotifyFightEnemyInfoUserCmd send2;
	strncpy(send2.name, pUser1->name, MAX_NAMESIZE);
	send2.occupation = GroupCardManager::getMe().getOccupationByIndex(*pUser1, players[0].cardsNumber);
	BYTE *transName2 = xml.charConv((BYTE*)(send2.name), "GB2312", "UTF-8");
	if(transName2)
	{
	    strncpy(send2.name, (char *)transName2, MAX_NAMESIZE);
	    SAFE_DELETE_VEC(transName2);
	}
	pUser2->sendCmdToMe(&send2, sizeof(send2));
	Zebra::logger->debug("有没有发呢 有");
    }
    else
    {
	Zebra::logger->debug("有没有发呢 没有");
    }
}

void ChallengeGame::sendEnemyHandNum(SceneUser *pUser)
{
    if(!pUser)
	return;
    Cmd::stRetEnemyHandCardNumUserCmd send;
    if(pUser->id == players[0].playerID)
    {
	send.count = this->slots.hand2.size()-this->slots.hand2.space();
    }
    else if(pUser->id == players[1].playerID)
    {
	send.count = this->slots.hand1.size()-this->slots.hand1.space();
    }
    pUser->sendCmdToMe(&send, sizeof(send));
}

void ChallengeGame::sendBattleHistoryInfo(DWORD mainID, std::vector<DWORD> otherIDVec)
{//暂时屏蔽历史记录显示
#if 0
    zCard *pMainHis = this->slots.gcm.getObjectByThisID(mainID);
    zCard *pOtherHis = NULL;
    if(pMainHis)
    {
	BUFFER_CMD(Cmd::stRetBattleHistoryInfoUserCmd, history, zSocket::MAX_USERDATASIZE);	    //发送历史
	bcopy(&pMainHis->data, &(history->maincard), sizeof(t_Card));
	history->opType = 0;
	for(std::vector<DWORD>::iterator it=otherIDVec.begin(); it!=otherIDVec.end(); it++)
	{
	    pOtherHis = this->slots.gcm.getObjectByThisID(*it);
	    if(pOtherHis)
	    {
		bcopy(&pOtherHis->data, &(history->othercard[history->count]), sizeof(t_Card));
		history->count++;
	    }
	}

	SceneUser *pUser = SceneUserManager::getMe().getUserByID(players[0].playerID);
	SceneUser *pOther = getOther(players[0].playerID);
	if(pUser)
	{
	    pUser->sendCmdToMe(history, sizeof(Cmd::stRetBattleHistoryInfoUserCmd)+history->count*sizeof(history->othercard[0]));
	}
	if(pOther)
	{
	    pOther->sendCmdToMe(history, sizeof(Cmd::stRetBattleHistoryInfoUserCmd)+history->count*sizeof(history->othercard[0]));
	}
    }
#endif
}

void ChallengeGame::sendCardStateInfo(zCard* o)
{
    if(!o)
	return;
    Cmd::stRetRefreshCardAllStateUserCmd send;
    send.dwThisID = o->data.qwThisID;
    bcopy(o->data.state, send.state, sizeof(send.state));
    SceneUser *pUser = SceneUserManager::getMe().getUserByID(o->playerID);
    SceneUser *pOther = getOther(o->playerID);
    if(pUser)
    {
	send.who = 1;
	pUser->sendCmdToMe(&send, sizeof(send));
    }
    if(pOther)
    {
	send.who = 2;
	pOther->sendCmdToMe(&send, sizeof(send));
    }
}

void ChallengeGame::sendLibNumInfo()
{
    SceneUser *pUser1 = SceneUserManager::getMe().getUserByID(players[0].playerID);
    if(pUser1)
    {
	Cmd::stRetLeftCardLibNumUserCmd send;
	send.selfNum = players[0].cardsLibVec.size();
	send.otherNum = players[1].cardsLibVec.size();
	pUser1->sendCmdToMe(&send, sizeof(send));
    }
    SceneUser *pUser2 = SceneUserManager::getMe().getUserByID(players[1].playerID);
    if(pUser2)
    {
	Cmd::stRetLeftCardLibNumUserCmd send;
	send.selfNum = players[1].cardsLibVec.size();
	send.otherNum = players[0].cardsLibVec.size();
	pUser2->sendCmdToMe(&send, sizeof(send));
    }
}

void ChallengeGame::sendMpInfo()
{
    SceneUser *pUser1 = SceneUserManager::getMe().getUserByID(players[0].playerID);
    if(pUser1)
    {
	Cmd::stRetMagicPointInfoUserCmd sendMp;
	sendMp.self.mp = players[0].getMp();
	sendMp.self.maxmp = players[0].getMaxMp();
	sendMp.self.forbid = players[0].getForbid();
	sendMp.other.mp = players[1].getMp();
	sendMp.other.maxmp = players[1].getMaxMp();
	sendMp.other.forbid = players[1].getForbid();
	pUser1->sendCmdToMe(&sendMp, sizeof(sendMp));
    }
    SceneUser *pUser2 = SceneUserManager::getMe().getUserByID(players[1].playerID);
    if(pUser2)
    {
	Cmd::stRetMagicPointInfoUserCmd sendMp;
	sendMp.self.mp = players[1].getMp();
	sendMp.self.maxmp = players[1].getMaxMp();
	sendMp.self.forbid = players[1].getForbid();
	sendMp.other.mp = players[0].getMp();
	sendMp.other.maxmp = players[0].getMaxMp();
	sendMp.other.forbid = players[0].getForbid();
	pUser2->sendCmdToMe(&sendMp, sizeof(sendMp));
    }
}

void ChallengeGame::sendCardInfo(zCard* object)
{
    if(!object)
	return;
    SceneUser *pUser = SceneUserManager::getMe().getUserByID(object->playerID);
    SceneUser *pOther = getOther(object->playerID);

    Cmd::stAddBattleCardPropertyUserCmd send;
    send.slot = object->data.pos.loc();
    send.byActionType = Cmd::CARD_DATA_ADD;
    bcopy(&object->data, &send.object, sizeof(t_Card));
    if(pUser)
    {
	send.who = 1;
	pUser->sendCmdToMe(&send, sizeof(send));
    }
    if(pOther)
    {
	send.who = 2;
	pOther->sendCmdToMe(&send, sizeof(send));
    }

}

void ChallengeGame::sendOutCardInfo(DWORD cardID)
{
    SceneUser *pUser = SceneUserManager::getMe().getUserByID(players[0].playerID);
    SceneUser *pOther = getOther(players[1].playerID);

    Cmd::stNotifyOutCardInfoUserCmd send;
    send.cardID = cardID;
    if(pUser)
    {
	pUser->sendCmdToMe(&send, sizeof(send));
    }
    if(pOther)
    {
	pOther->sendCmdToMe(&send, sizeof(send));
    }
}


void ChallengeGame::refreshCardInfo(zCard* object)
{
    if(!object)
	return;
    SceneUser *pUser = SceneUserManager::getMe().getUserByID(object->playerID);
    SceneUser *pOther = getOther(object->playerID);

    Cmd::stAddBattleCardPropertyUserCmd send;
    send.slot = object->data.pos.loc();
    send.byActionType = Cmd::CARD_DATA_REFRESH;
    bcopy(&object->data, &send.object, sizeof(t_Card));
    if(pUser)
    {
	send.who = 1;
	pUser->sendCmdToMe(&send, sizeof(send));
    }
    if(pOther)
    {
	send.who = 2;
	pOther->sendCmdToMe(&send, sizeof(send));
    }
}

/**
 * \brief 向客户端刷新战斗数据
 * \param pAttThisID 攻击者
 *	  defThisIDVec 防御者列表
 *	  dwMagicType 技能ID
 *	  type 数据类型(1,召唤;0,其他)
 *
 * \return
*/
void ChallengeGame::refreshClient(DWORD pAttThisID, std::vector<DWORD> defThisIDVec, DWORD dwMagicType, BYTE type)
{
    if(defThisIDVec.empty())
	return;
    zCard *pAtt = this->slots.gcm.getObjectByThisID(pAttThisID);
    if(!pAtt)
	return;
    SceneUser *pUser = SceneUserManager::getMe().getUserByID(pAtt->playerID);
    SceneUser *pOther = getOther(pAtt->playerID);

    BUFFER_CMD(Cmd::stNotifyBattleCardPropertyUserCmd, send, zSocket::MAX_USERDATASIZE);
    bcopy(&pAtt->data, &send->A_object, sizeof(t_Card));
    send->dwMagicType = dwMagicType;
    send->type = type;
    for(std::vector<DWORD>::iterator it=defThisIDVec.begin(); it!=defThisIDVec.end(); it++)
    {
	zCard* pOtherHis = this->slots.gcm.getObjectByThisID(*it);
	if(pOtherHis)
	{
	    bcopy(&pOtherHis->data, &(send->defList[send->count]), sizeof(t_Card));
	    send->count++;
	}
    }
    if(pUser)
    {
	pUser->sendCmdToMe(send, sizeof(Cmd::stNotifyBattleCardPropertyUserCmd)+send->count*sizeof(t_Card));
    }
    if(pOther)
    {
	pOther->sendCmdToMe(send, sizeof(Cmd::stNotifyBattleCardPropertyUserCmd)+send->count*sizeof(t_Card));
    }

    std::vector<DWORD> affectVec(defThisIDVec);
    affectVec.push_back(pAttThisID);
    for(std::vector<DWORD>::iterator it=affectVec.begin(); it != affectVec.end(); it++)
    {
	zCard* pAffected = this->slots.gcm.getObjectByThisID(*it);
	if(pAffected)
	{
	    pAffected->data.popHpValue = 0;
	    pAffected->data.popDamValue = 0;
	}
    }
    affectVec.clear();
}

bool ChallengeGame::dealGameResult(const DWORD loserID, bool isPeace)
{
    if(getState() != CHALLENGE_STATE_BATTLE)
	return false;
    if(isPVP())
    {
	if(!isInGame(loserID))
	    return false;
	DWORD winnerID = 0;
	if(loserID == players[0].playerID)
	{
	    winnerID = players[1].playerID;
	}
	else if(loserID == players[1].playerID)
	{
	    winnerID = players[0].playerID;
	}
	SceneUser *winner = SceneUserManager::getMe().getUserByID(winnerID);
	SceneUser *loser = SceneUserManager::getMe().getUserByID(loserID);
	if(!isPeace)
	{
	    if(winner && loser)
	    {
		Zebra::logger->info("[卡牌] PVP 对战结束 胜负已分 胜者:(%s,%u) 败者:(%s,%u)",winner->name, winner->id, loser->name, loser->id);
	    }
	    Cmd::stRetBattleGameResultUserCmd send;
	    if(winner)
	    {
		send.win = 1;
		winner->sendCmdToMe(&send, sizeof(send));
	    }
	    if(loser)
	    {
		send.win = 0;
		loser->sendCmdToMe(&send, sizeof(send));
	    }
	}
	else
	{
	    if(winner && loser)
	    {
		Zebra::logger->info("[卡牌] PVP 对战结束 平局收场 胜者:(%s,%u) 败者:(%s,%u)",winner->name, winner->id, loser->name, loser->id);
	    }
	    Cmd::stRetBattleGameResultUserCmd send;
	    if(winner)
	    {
		send.win = 0;
		winner->sendCmdToMe(&send, sizeof(send));
	    }
	    if(loser)
	    {
		send.win = 0;
		loser->sendCmdToMe(&send, sizeof(send));
	    }
	}
    }
    return true;
}

/**
 * \brief 唤醒随从
 * \param
 * \return
*/
bool ChallengeGame::wakeUpAllAttend()
{
    zCard *pDef = NULL;
    for(WORD i=0; i<this->slots.main1.size(); i++)
    {
	pDef = this->slots.main1.container[i];
	if(pDef && !pDef->isAwake())
	{
	    pDef->setAwake();
	}
    }

    for(WORD i=0; i<this->slots.main2.size(); i++)
    {
	pDef = this->slots.main2.container[i];
	if(pDef && !pDef->isAwake())
	{
	    pDef->setAwake();
	}
    }
    return true;
}

/**
 * \brief 检查敌方的嘲讽情况
 * \param
 * \return
*/
bool ChallengeGame::checkEnemyTaunt(DWORD playerID)
{
    zCard *pDef = NULL;
    if(playerID == players[0].playerID)
    {
	for(WORD i=0; i<this->slots.main2.size(); i++)
	{
	    pDef = this->slots.main2.container[i];
	    if(pDef && pDef->hasTaunt() && !pDef->isSneak())
	    {
		return true;
	    }
	}
    }
    else if(playerID == players[1].playerID)
    {
	for(WORD i=0; i<this->slots.main1.size(); i++)
	{
	    pDef = this->slots.main1.container[i];
	    if(pDef && pDef->hasTaunt() && !pDef->isSneak())
	    {
		return true;
	    }
	}
    }
    return false;
}

bool ChallengeGame::checkSelectedTarget(zCard* pAtt, const DWORD needTarget, const DWORD dwDefThisID)
{
    if(!pAtt)
	return false;
    zCard* pDef = this->slots.gcm.getObjectByThisID(dwDefThisID);
    if(!pDef)
    {
	Zebra::logger->error("这个操作 需要你(%u)手动选择一个目标 目标已经不存在了", pAtt->playerID);
	return false;
    }
    if(pAtt->data.pos.loc() == Cmd::CARDCELLTYPE_RECORD || pDef->data.pos.loc() == Cmd::CARDCELLTYPE_RECORD)
    {
	Zebra::logger->error("居然用历史记录中的数据 你(pAtt->playerID:%u) 开挂了吧~",pAtt->playerID);
	return false;
    }
    if(pAtt == pDef)
    {
	Zebra::logger->error("目标怎么能和攻击者一样 攻击者%s() 防御者%s", pAtt->base->name, pDef->base->name);
	return false;
    }
    if(needTarget)
    {
	if(pDef->isSneak())
	{
	    Zebra::logger->error("[PK] 目标选择错误(施法不能指定隐形单位) 攻击者%s() 防御者%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "普通攻击失败  普通攻击不能打到潜行单位");
	    return false;
	}
	if(rightTarget(pDef, pAtt, needTarget))
	{

	}
	else
	{
	    Zebra::logger->error("SKILL  你的目标选择错误");
	    return false;
	}
	return true;
    }
    else	//普通攻击目标检测
    {
	if(pAtt->isFreeze())
	{
	    Zebra::logger->error("[PK] 普通攻击失败(你处于冻结状态不能进行攻击) 攻击者%s() 防御者%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "普通攻击失败  你自己处于冻结中无法攻击");
	    return false;
	}
	if(!pDef->isHero() && !pDef->isAttend())
	{
	    Zebra::logger->error("[PK] 普通攻击失败(只能攻击随从或者英雄) 攻击者%s() 防御者%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "普通攻击失败  你所攻击的对象既不是英雄也不是随从");
	    return false;
	}
	if(!pDef->preAttackMe(pAtt))
	{
	    Zebra::logger->error("[PK] 普通攻击失败(preAttackMe 验证失败) 攻击者%s() 防御者%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "普通攻击失败  攻击验证失败");
	    return false;
	}
	if(!pAtt->hasDamage())
	{
	    Zebra::logger->error("[PK] 普通攻击失败(你没有攻击力啊) 攻击者%s() 防御者%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "普通攻击失败  你都没有攻击力 不能发起攻击");
	    return false;
	}
	if(pDef->isSneak())
	{
	    Zebra::logger->error("[PK] 普通攻击失败(普通攻击不能打到隐形单位) 攻击者%s() 防御者%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "普通攻击失败  普通攻击不能打到潜行单位");
	    return false;
	}
	if(!pDef->isSneak())	//被攻击者隐形情况下,嘲讽无效
	{
	    if(checkEnemyTaunt(pAtt->playerID) && !pDef->hasTaunt())
	    {
		Zebra::logger->error("[PK] 普通攻击失败(你得攻击一个有嘲讽的随从才行) 攻击者%s() 防御者%s", pAtt->base->name, pDef->base->name);
		//Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "普通攻击失败  你得攻击一个具有嘲讽的随从才行");
		return false;
	    }
	}
	return true;
    }

    return false;
}

bool ChallengeGame::rightTarget(zCard* main, zCard* other, DWORD range)
{
    if(((range & Cmd::ATTACK_TARGET_EHERO) && /*敌方英雄*/main->isHero() && other->isEnemy(main))
	    || ((range & Cmd::ATTACK_TARGET_EATTEND) && /*敌方随从*/main->isAttend() && other->isEnemy(main))
	    || ((range & Cmd::ATTACK_TARGET_SHERO) && /*己方英雄*/main->isHero() && !other->isEnemy(main))
	    || ((range & Cmd::ATTACK_TARGET_SATTEND) && /*己方随从*/main->isAttend() && !other->isEnemy(main))
	    || ((range & Cmd::ATTACK_TARGET_MYSELF) && /*自身*/(main->data.qwThisID == other->data.qwThisID)))
    {
	return true;
    }
    return false;
}

/**
 * \brief 场上是否有可用目标
 * \param
 * \return
*/
bool ChallengeGame::canFindTarget(zCard *pAtt, DWORD needTarget)
{
    std::vector<DWORD> targets;
    collectTarget(pAtt->playerID, needTarget, targets);
    if(targets.empty())
	return false;
    return true;
}

bool ChallengeGame::equipEnter(DWORD playerID, zCard* equip)
{
    if(!equip || !equip->base)
	return false;
    if(equip->isEquip())
    {
	zCard* hero = NULL;
	hero = getSelfHero(playerID);
	if(hero)
	{
	    hero->addDamage(equip->data.damage);
	    sendCardInfo(hero);
	    Zebra::logger->debug("[PK] 装备武器 英雄(%s)攻击变化 当前攻击:%u",hero->base->name, hero->data.damage);
	}
    }
    return false;
}

bool ChallengeGame::equipLeave(DWORD playerID, zCard* equip)
{
    if(!equip || !equip->base)
	return false;
    if(equip->isEquip())
    {
	zCard* hero = NULL;
	hero = getSelfHero(playerID);
	if(hero)
	{
	    hero->subDamage(equip->data.damage);
	    equip->toDie();
	    equip->processDeath(NULL, equip);
	    Zebra::logger->debug("[PK] 武器离场 英雄(%s)攻击力变化 当前攻击:%u",hero->base->name, hero->data.damage);
	}
    }
    return false;
}

/**
 * \brief 法术伤害增加加成
 * \param
 * \return
*/
DWORD ChallengeGame::recalMagicAddDam(DWORD playerID)
{
    if(!isInGame(playerID))
	return 0;
    DWORD dam = 0;
    if(playerID == players[0].playerID)
    {
	for(WORD i=0; i<this->slots.main1.size(); i++)
	{
	    zCard *pDef = NULL;
	    pDef = this->slots.main1.container[i];
	    if(pDef && pDef->data.magicDamAdd)
	    {
		dam += pDef->data.magicDamAdd;
	    }
	}
    }
    else if(playerID == players[1].playerID)
    {
	for(WORD i=0; i<this->slots.main2.size(); i++)
	{
	    zCard *pDef = NULL;
	    pDef = this->slots.main2.container[i];
	    if(pDef && pDef->data.magicDamAdd)
	    {
		dam += pDef->data.magicDamAdd;
	    }
	}
    }
    Zebra::logger->debug("[PK] %u 伤害增加总加成:%u",playerID, dam);
    return dam;
}

void ChallengeGame::sendFirstHandCard(DWORD upper)
{
    SceneUser *pUser = SceneUserManager::getMe().getUserByID(upper);
    SceneUser *pOther = NULL;
    if(upper == players[0].playerID)
    {
	if(pUser)
	{
	    Cmd::stRetFirstHandCardUserCmd send;
	    send.id[0] = players[0].cardsLibVec[0];
	    send.id[1] = players[0].cardsLibVec[1];
	    send.id[2] = players[0].cardsLibVec[2];
	    send.upperHand = 1;
	    pUser->sendCmdToMe(&send, sizeof(send));
	}
	pOther = SceneUserManager::getMe().getUserByID(players[1].playerID);
	if(pOther)
	{
	    Cmd::stRetFirstHandCardUserCmd send2;
	    send2.id[0] = players[1].cardsLibVec[0];
	    send2.id[1] = players[1].cardsLibVec[1];
	    send2.id[2] = players[1].cardsLibVec[2];
	    send2.id[3] = players[1].cardsLibVec[3];
	    send2.upperHand = 0;
	    pOther->sendCmdToMe(&send2, sizeof(send2));
	}

	for(BYTE i=0; i<3; i++)
	{
	    players[0].prepareHand.push_back(players[0].cardsLibVec[i]);
	}
	players[0].cardsLibVec.erase(players[0].cardsLibVec.begin(), players[0].cardsLibVec.begin()+3);

	for(BYTE i=0; i<4; i++)
	{
	    players[1].prepareHand.push_back(players[1].cardsLibVec[i]);
	}
	players[1].cardsLibVec.erase(players[1].cardsLibVec.begin(), players[1].cardsLibVec.begin()+4);
    }
    else if(upper == players[1].playerID)
    {
	if(pUser)
	{
	    Cmd::stRetFirstHandCardUserCmd send;
	    send.id[0] = players[1].cardsLibVec[0];
	    send.id[1] = players[1].cardsLibVec[1];
	    send.id[2] = players[1].cardsLibVec[2];
	    send.upperHand = 1;
	    pUser->sendCmdToMe(&send, sizeof(send));
	}
	pOther = SceneUserManager::getMe().getUserByID(players[0].playerID);
	if(pOther)
	{
	    Cmd::stRetFirstHandCardUserCmd send2;
	    send2.id[0] = players[0].cardsLibVec[0];
	    send2.id[1] = players[0].cardsLibVec[1];
	    send2.id[2] = players[0].cardsLibVec[2];
	    send2.id[3] = players[0].cardsLibVec[3];
	    send2.upperHand = 0;
	    pOther->sendCmdToMe(&send2, sizeof(send2));
	}

	for(BYTE i=0; i<3; i++)
	{
	    players[1].prepareHand.push_back(players[1].cardsLibVec[i]);
	}
	players[1].cardsLibVec.erase(players[1].cardsLibVec.begin(), players[1].cardsLibVec.begin()+3);

	for(BYTE i=0; i<4; i++)
	{
	    players[0].prepareHand.push_back(players[0].cardsLibVec[i]);
	}
	players[0].cardsLibVec.erase(players[0].cardsLibVec.begin(), players[0].cardsLibVec.begin()+4);
    }
}

void ChallengeGame::sendHandFullInfo(DWORD playerID, DWORD id)
{
    SceneUser *pUser = SceneUserManager::getMe().getUserByID(playerID);
    SceneUser *pOther = getOther(playerID);
    if(pUser)
    {
	Cmd::stRetNotifyHandIsFullUserCmd send;
	send.id = id;
	send.who = 1;
	pUser->sendCmdToMe(&send, sizeof(send));
    }
    if(pOther)
    {
	Cmd::stRetNotifyHandIsFullUserCmd send;
	send.id = id;
	send.who = 2;
	pOther->sendCmdToMe(&send, sizeof(send));
    }
}

class sendAllCardListToUser:public UserCardExec
{
public:
	SceneUser *pUser;
	char buffer[zSocket::MAX_USERDATASIZE];
	Cmd::stAddBattleCardListPropertyUserCmd *send;
	sendAllCardListToUser(SceneUser *u):pUser(u)
	{
		send = (Cmd::stAddBattleCardListPropertyUserCmd *)buffer;
		constructInPlace(send);
	}
	bool exec(zCard *object)
	{
	    if (sizeof(Cmd::stAddBattleCardListPropertyUserCmd) + (send->count + 1) * sizeof(send->list[0]) >= sizeof(buffer))
	    {
		pUser->sendCmdToMe(send,sizeof(Cmd::stAddBattleCardListPropertyUserCmd) + (send->count * sizeof(send->list[0])));
		send->count=0;
	    }
	    if(object->data.pos.loc() >= Cmd::CARDCELLTYPE_COMMON && object->data.pos.loc() <= Cmd::CARDCELLTYPE_HERO)
	    {
		if(object->playerID == pUser->id)
		{
		    send->list[send->count].who = 1;
		    bcopy(&object->data, &send->list[send->count].object, sizeof(t_Card));
		}
		else	
		{
		    if(object->data.pos.loc() == Cmd::CARDCELLTYPE_HAND)    //对方手牌
		    {
			return true;
		    }
		    send->list[send->count].who = 2;
		    bcopy(&object->data, &send->list[send->count].object, sizeof(t_Card));
		}
		send->count++;
	    }
	    return true;
	}
};
void ChallengeGame::sendAllCardList(SceneUser *pUser)
{
    if(!pUser)
	return;
    sendAllCardListToUser sendall(pUser);
    this->slots.gcm.execEvery(sendall);
    if(sendall.send->count)
    {
	pUser->sendCmdToMe(sendall.send, sizeof(Cmd::stAddBattleCardListPropertyUserCmd)+(sendall.send->count* sizeof(sendall.send->list[0])));
	sendall.send->count = 0;
    }
}

/**
 * \brief 处理当使用法术牌触发效果
 * \param
 * \return
*/
void ChallengeGame::dealUseMagicCardAction()
{
    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_SEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);
    
    for(DWORD i=0; i<thisID_Vec.size(); i++)
    {
	DWORD finalThisID = thisID_Vec[i];
	zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	if(pDef && pDef->pk.selfUseMagic)
	{
	    Cmd::stCardAttackMagicUserCmd in;
	    in.dwAttThisID = finalThisID;
	    in.dwDefThisID = 0;
	    in.dwMagicType = pDef->pk.selfUseMagic;
	    addActionList(in);
	}
    }

    thisID_Vec.clear();
    range = Cmd::ATTACK_TARGET_EHERO + Cmd::ATTACK_TARGET_EATTEND + Cmd::ATTACK_TARGET_EEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);
    
    for(DWORD i=0; i<thisID_Vec.size(); i++)
    {
	DWORD finalThisID = thisID_Vec[i];
	zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	if(pDef && pDef->pk.enemyUseMagic)
	{
	    Cmd::stCardAttackMagicUserCmd in;
	    in.dwAttThisID = finalThisID;
	    in.dwDefThisID = 0;
	    in.dwMagicType = pDef->pk.enemyUseMagic;
	    addActionList(in);
	}
    }


}

/**
 * \brief 处理回合开始触发效果
 * \param
 * \return
 */
void ChallengeGame::dealRoundStartAction()
{
    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_SEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);

    bool flag = false;
    for(DWORD i=0; i<thisID_Vec.size(); i++)
    {
	DWORD finalThisID = thisID_Vec[i];
	zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	if(pDef && pDef->hasRoundSID())
	{
	    Cmd::stCardAttackMagicUserCmd in;
	    in.dwAttThisID = finalThisID;
	    in.dwDefThisID = 0;
	    for(std::vector<t_EffectUnit>::iterator it=pDef->pk.roundSIDVec.begin(); it!=pDef->pk.roundSIDVec.end(); it++)
	    {
		in.dwMagicType = it->id;
		addActionList(in);
		flag = true;
	    }
	    clearTempEffectUnit(pDef->pk.roundSIDVec);
	}
    }


    thisID_Vec.clear();
    range = Cmd::ATTACK_TARGET_EHERO + Cmd::ATTACK_TARGET_EATTEND + Cmd::ATTACK_TARGET_EEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);

    for(DWORD i=0; i<thisID_Vec.size(); i++)
    {
	DWORD finalThisID = thisID_Vec[i];
	zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	if(pDef && pDef->hasEnemyRoundSID())
	{
	    Cmd::stCardAttackMagicUserCmd in;
	    in.dwAttThisID = finalThisID;
	    in.dwDefThisID = 0;
	    for(std::vector<t_EffectUnit>::iterator it=pDef->pk.enemyroundSIDVec.begin(); it!=pDef->pk.enemyroundSIDVec.end(); it++)
	    {
		in.dwMagicType = it->id;
		addActionList(in);
		flag = true;
	    }
	    clearTempEffectUnit(pDef->pk.enemyroundSIDVec);
	}
    }
    if(flag)
    {
	startOneFlow();
	action( "回合开始");		    //回合开始和敌方回合开始时触发
	endOneFlow();
    }
}

/**
 * \brief 处理回合结束触发效果
 * \param
 * \return
*/
void ChallengeGame::dealRoundEndAction()
{
    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_SEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);
    
    bool flag = false;
    for(DWORD i=0; i<thisID_Vec.size(); i++)
    {
	DWORD finalThisID = thisID_Vec[i];
	zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	if(pDef && pDef->hasRoundEID())
	{
	    Cmd::stCardAttackMagicUserCmd in;
	    in.dwAttThisID = finalThisID;
	    in.dwDefThisID = 0;
	    for(std::vector<t_EffectUnit>::iterator it=pDef->pk.roundEIDVec.begin(); it!=pDef->pk.roundEIDVec.end(); it++)
	    {
		in.dwMagicType = it->id;
		addActionList(in);
		flag = true;
	    }
	    clearTempEffectUnit(pDef->pk.roundEIDVec);
	}
    }

    thisID_Vec.clear();
    range = Cmd::ATTACK_TARGET_EHERO + Cmd::ATTACK_TARGET_EATTEND + Cmd::ATTACK_TARGET_EEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);
    
    for(DWORD i=0; i<thisID_Vec.size(); i++)
    {
	DWORD finalThisID = thisID_Vec[i];
	zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	if(pDef && pDef->hasEnemyRoundEID())
	{
	    Cmd::stCardAttackMagicUserCmd in;
	    in.dwAttThisID = finalThisID;
	    in.dwDefThisID = 0;
	    for(std::vector<t_EffectUnit>::iterator it=pDef->pk.enemyroundEIDVec.begin(); it!=pDef->pk.enemyroundEIDVec.end(); it++)
	    {
		in.dwMagicType = it->id;
		addActionList(in);
		flag = true;
	    }
	    clearTempEffectUnit(pDef->pk.enemyroundEIDVec);
	}
    }

    if(flag)
    {
	startOneFlow();
	action( "回合结束");		    //回合结束和敌方回合结束时触发
	endOneFlow();
    }
}

void ChallengeGame::dealDrawCardSuccessAction(DWORD thisID)
{
    zCard *pMain = this->slots.gcm.getObjectByThisID(thisID);
    if(pMain && pMain->pk.drawedID)
    {
	Cmd::stCardAttackMagicUserCmd in;
	in.dwAttThisID = thisID;
	in.dwDefThisID = 0;
	in.dwMagicType = pMain->pk.drawedID;
	actionList.push_front(in);	    //加到前面
	action( "抽到某张牌");		    //抽到牌时触发
    }
    
    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_SEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);
    if(thisID_Vec.empty())	    
	return;
    bool flag = false;
    DWORD i=thisID_Vec.size();
    for(; i>=1; i--)
    {
	DWORD finalThisID = thisID_Vec[i-1];
	zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	if(pDef && pDef->pk.drawID && !pDef->isDie())
	{
	    Cmd::stCardAttackMagicUserCmd in;
	    in.dwAttThisID = finalThisID;
	    in.dwDefThisID = 0;
	    in.dwMagicType = pDef->pk.drawID;
	    actionList.push_front(in);	    //加到前面
	    flag = true;
	}
    }
    if(flag)
    {
	action( "抽牌效果");		    //抽牌时触发
    }
}

void ChallengeGame::dealDeadAction()
{
    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_SEQUIP
	+ Cmd::ATTACK_TARGET_EHERO + Cmd::ATTACK_TARGET_EATTEND + Cmd::ATTACK_TARGET_EEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);
    if(thisID_Vec.empty())	    
	return;

    for(DWORD i=0; i<thisID_Vec.size(); i++)
    {
	DWORD finalThisID = thisID_Vec[i];
	zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	if(pDef && pDef->isDie())
	{
	    pDef->processDeath(NULL, pDef);
	}
    }
    action( "执行");		   
    dealHaloEffectAction();
}

void ChallengeGame::dealHurtAction()
{
    for(std::vector<DWORD>::iterator it=hurtList.begin(); it!=hurtList.end(); ++it)
    {
	DWORD mainThisID = *it;
	dealOtherEffectAction(mainThisID, Cmd::ACTION_TYPE_HURT);
    }
    hurtList.clear();
}

void ChallengeGame::dealCureAction()
{
    for(std::vector<DWORD>::iterator it=cureList.begin(); it!=cureList.end(); ++it)
    {
	DWORD mainThisID = *it;
	dealOtherEffectAction(mainThisID, Cmd::ACTION_TYPE_CURE);
    }
    cureList.clear();
}

/**
 * \brief   当mainThisID被某效果影响后,
 *	    检查场上其他角色是否需要触发效果
 * \param
 * \return
*/
void ChallengeGame::dealOtherEffectAction(DWORD mainThisID, DWORD type)
{   
    zCard *main = this->slots.gcm.getObjectByThisID(mainThisID);	//当被影响的对象
    if(!main)
    {
	return;
    }

    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_EHERO + Cmd::ATTACK_TARGET_EATTEND+ Cmd::ATTACK_TARGET_SEQUIP + Cmd::ATTACK_TARGET_EEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);
    if(thisID_Vec.empty())	    
	return;
    for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end(); )	//排除没有效果的对象
    {
	zCard *p = this->slots.gcm.getObjectByThisID(*it);
	ConditionStatus status;
	if(type == Cmd::ACTION_TYPE_HURT)
	{
	    status = p->pk.hurtStatus;
	}
	else if(type == Cmd::ACTION_TYPE_CURE)
	{
	    status = p->pk.cureStatus;
	}
	else if(type == Cmd::ACTION_TYPE_DEAD)
	{
	    status = p->pk.deadStatus;
	}
	else if(type == Cmd::ACTION_TYPE_USEATTEND)
	{
	    status = p->pk.useAttendStatus;
	}
	else if(type == Cmd::ACTION_TYPE_ATTEND_IN)
	{
	    status = p->pk.attendInStatus;
	}
	if((!p)
		|| (p->pk.otherBeCureID==0 && type==Cmd::ACTION_TYPE_CURE)
		|| (p->pk.otherBeHurtID==0 && type==Cmd::ACTION_TYPE_HURT)
		|| (p->pk.otherDeadID==0 && type==Cmd::ACTION_TYPE_DEAD)
		|| (p->pk.otherUseAttendID==0 && type==Cmd::ACTION_TYPE_USEATTEND)
		|| (p->pk.otherAttendInID==0 && type==Cmd::ACTION_TYPE_ATTEND_IN)
		|| (p->isDie())
		|| (status.mode==0 && main==p))
	{
	    it = thisID_Vec.erase(it);
	}
	else
	{
	    ++it;
	}
    }
    for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end(); )	//排除不符合作用范围的对象
    {
	zCard *p = this->slots.gcm.getObjectByThisID(*it);
	ConditionStatus status;
	if(type == Cmd::ACTION_TYPE_HURT)
	{
	    status = p->pk.hurtStatus;
	}
	else if(type == Cmd::ACTION_TYPE_CURE)
	{
	    status = p->pk.cureStatus;
	}
	else if(type == Cmd::ACTION_TYPE_DEAD)
	{
	    status = p->pk.deadStatus;
	}
	else if(type == Cmd::ACTION_TYPE_USEATTEND)
	{
	    status = p->pk.useAttendStatus;
	}
	else if(type == Cmd::ACTION_TYPE_ATTEND_IN)
	{
	    status = p->pk.attendInStatus;
	}

	WORD range = status.range;
	if(rightTarget(main, p, range))
	{
	    ++it;
	}
	else
	{
	    it = thisID_Vec.erase(it);
	}
    }
    if(thisID_Vec.empty())	    
	return;
    
    std::vector<DWORD> realVec;
    for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end(); it++)		//取出符合condition的
    {
	zCard *p = this->slots.gcm.getObjectByThisID(*it);
	ConditionStatus status;
	if(type == Cmd::ACTION_TYPE_HURT)
	{
	    status = p->pk.hurtStatus;
	}
	else if(type == Cmd::ACTION_TYPE_CURE)
	{
	    status = p->pk.cureStatus;
	}
	else if(type == Cmd::ACTION_TYPE_DEAD)
	{
	    status = p->pk.deadStatus;
	}
	else if(type == Cmd::ACTION_TYPE_USEATTEND)
	{
	    status = p->pk.useAttendStatus;
	}
	else if(type == Cmd::ACTION_TYPE_ATTEND_IN)
	{
	    status = p->pk.attendInStatus;
	}

	WORD condition = status.condition;
	if(((condition & Cmd::TARGET_CONDITION_INJURED) && !main->injured())
		|| ((condition & Cmd::TARGET_CONDITION_DAMG2) && !main->damageGreat2())
		|| ((condition & Cmd::TARGET_CONDITION_DAML3) && !main->damageLess3())
		|| ((condition & Cmd::TARGET_CONDITION_DEADLAN) && !main->hasDeadLanguage()))	
	{
	    continue;
	}
	realVec.push_back(*it);
    }

    bool flag = false;
    for(DWORD i=0; i<realVec.size(); i++)
    {
	DWORD finalThisID = realVec[i];
	zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	if(pDef)
	{
	    DWORD dwMagicType = 0;
	    if(type == Cmd::ACTION_TYPE_HURT)
	    {
		dwMagicType = pDef->pk.otherBeHurtID;
	    }
	    else if(type == Cmd::ACTION_TYPE_CURE)
	    {
		dwMagicType = pDef->pk.otherBeCureID;
	    }
	    else if(type == Cmd::ACTION_TYPE_DEAD)
	    {
		dwMagicType = pDef->pk.otherDeadID;
	    }
	    else if(type == Cmd::ACTION_TYPE_USEATTEND)
	    {
		dwMagicType = pDef->pk.otherUseAttendID;
	    }
	    else if(type == Cmd::ACTION_TYPE_ATTEND_IN)
	    {
		dwMagicType = pDef->pk.otherAttendInID;
	    }

	    Cmd::stCardAttackMagicUserCmd in;
	    in.dwAttThisID = finalThisID;
	    in.dwDefThisID = 0;
	    in.dwMagicType = dwMagicType;
	    if(type == Cmd::ACTION_TYPE_ATTEND_IN)
	    {
		actionList.push_front(in);
		action( "随从进场");
	    }
	    else
	    {
		addActionList(in);
	    }
	    flag = true;
	}
    }
}

/**
 * \brief 攻击结束效果
 * \param
 * \return
*/
void ChallengeGame::dealAttackEndAction(DWORD dwAttThisID, DWORD dwDefThisID, bool defHurt)
{
    zCard* main = this->slots.gcm.getObjectByThisID(dwAttThisID);
    if(main && main->pk.attackEndCondition && main->pk.attackEndID)
    {
	bool condition = false;
	if(main->pk.attackEndCondition == 1)	//受伤需要单独判断
	{
	    if(defHurt)
		condition = true;
	}
	else if(main->pk.attackEndCondition == 10000)	//成立
	{
	    condition = true;
	}
	else
	{
	    condition = checkHasState(main->pk.attackEndCondition, dwDefThisID);
	}
	if(condition)
	{
	    Cmd::stCardAttackMagicUserCmd in;
	    in.dwAttThisID = dwAttThisID;
	    in.dwDefThisID = dwDefThisID;
	    in.dwMagicType = main->pk.attackEndID;
	    in.flag = 1;
	    addActionList(in);
	}
    }
}

/**
 * \brief 处理光环效果
 * \param
 * \return
*/
void ChallengeGame::dealHaloEffectAction()
{
    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_SEQUIP
	+ Cmd::ATTACK_TARGET_EHERO + Cmd::ATTACK_TARGET_EATTEND + Cmd::ATTACK_TARGET_EEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);

    for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end(); )	//去掉非光环的ID
    {
	zCard *p = this->slots.gcm.getObjectByThisID(*it);
	if(p && p->hasHaloID())
	{
	    ++it;
	}
	else
	{
	    it = thisID_Vec.erase(it);
	}
    }
    for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end();++it)
    {
	zCard *main = this->slots.gcm.getObjectByThisID(*it);
	if(main && checkHaloOpen(main->pk.halo_Ctype, main->pk.halo_Cid, main))	//开着
	{
	    zCard *p = this->slots.gcm.getObjectByThisID(*it);
	    if(p)
	    {
		Cmd::stCardAttackMagicUserCmd open;
		open.dwAttThisID = p->data.qwThisID;
		open.dwDefThisID = 0;
		open.dwMagicType = p->pk.haloID;
		actionHaloList.push_back(open);
	    }
	}
	else	//关着
	{
	    clearAllHalo(*it);
	}
    }
    actionHalo("光环");

    clearIllegalHalo();
}

void ChallengeGame::clearIllegalHalo()
{
    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_SEQUIP
	+ Cmd::ATTACK_TARGET_EHERO + Cmd::ATTACK_TARGET_EATTEND + Cmd::ATTACK_TARGET_EEQUIP
	+ Cmd::ATTACK_TARGET_SHAND + Cmd::ATTACK_TARGET_EHAND;
    collectTarget(privilegeUser, range, thisID_Vec);
    for(std::vector<DWORD>::iterator itV=thisID_Vec.begin(); itV!=thisID_Vec.end();++itV)
    {
	zCard *p = this->slots.gcm.getObjectByThisID(*itV);
	if(p)	    //存在
	{//清除p身上的所有非法光环
	    if(p->haloInfoMap.empty())
		continue;
	    for(std::map<DWORD, t_haloInfo>::iterator it = p->haloInfoMap.begin(); it != p->haloInfoMap.end(); )
	    {
		zCard *main = this->slots.gcm.getObjectByThisID(it->first);
		if(main && checkHaloOpen(main->pk.halo_Ctype, main->pk.halo_Cid, main))   //开着
		{
		    ++it;
		}
		else
		{
		    p->clearOneHaloInfo(it->first);	//清除
		    p->haloInfoMap.erase(it++);	//删除
		}
	    }
	}
    }
}

bool ChallengeGame::getLeftRightID(std::vector<DWORD> &vec, DWORD mainThisID)
{
    zCard *main = this->slots.gcm.getObjectByThisID(mainThisID);
    if(main)
    {
	if(main->playerID == players[0].playerID)
	{
	    DWORD left = this->slots.main1.getLeftObjectThisID(main->data.pos.x, main->data.pos.y);
	    DWORD right = this->slots.main1.getRightObjectThisID(main->data.pos.x, main->data.pos.y);
	    if(left)
		vec.push_back(left);
	    if(right)
		vec.push_back(right);
	    return true;
	}
	else if(main->playerID == players[1].playerID)
	{
	    DWORD left = this->slots.main2.getLeftObjectThisID(main->data.pos.x, main->data.pos.y);
	    DWORD right = this->slots.main2.getRightObjectThisID(main->data.pos.x, main->data.pos.y);
	    if(left)
		vec.push_back(left);
	    if(right)
		vec.push_back(right);
	    return true;
	}
    }
    return false;
}

/**
 * \brief   处理武器状态切换(开\关)
 * \param
 * \return
*/
void ChallengeGame::dealEquipState()
{
    zCard *equip = getSelfEquip(privilegeUser);
    if(equip)	
    {
	//开启
	zCard *hero = getSelfHero(privilegeUser);
	if(hero)
	{
	    Zebra::logger->debug("[PK]回合开始 英雄武器 %s 开启", equip->base->name);
	    hero->data.equipOpen = 1;
	    sendCardInfo(hero);
	}
    }

    equip = NULL;
    equip = getEnemyEquip(privilegeUser);
    if(equip)
    {
	//关闭
	zCard *hero = getEnemyHero(privilegeUser);
	if(hero && hero->checkAttackTimes())
	{
	    Zebra::logger->debug("[PK]回合开始 英雄武器 %s 关闭", equip->base->name);
	    hero->data.equipOpen = 0;
	    sendCardInfo(hero);
	}
    }
}

void ChallengeGame::startOneFlow()
{
    SceneUser *pUser = SceneUserManager::getMe().getUserByID(players[0].playerID);
    SceneUser *pOther = getOther(players[0].playerID);
    Cmd::stNotifyBattleFlowStartUserCmd start;
    if(pUser)
	pUser->sendCmdToMe(&start, sizeof(start));
    if(pOther)
	pOther->sendCmdToMe(&start, sizeof(start));

}

void ChallengeGame::endOneFlow()
{
    SceneUser *pUser = SceneUserManager::getMe().getUserByID(players[0].playerID);
    SceneUser *pOther = getOther(players[0].playerID);
    Cmd::stNotifyBattleFlowEndUserCmd end;
    if(pUser)
	pUser->sendCmdToMe(&end, sizeof(end));
    if(pOther)
	pOther->sendCmdToMe(&end, sizeof(end));

}

void ChallengeGame::clearTempEffectUnit(std::vector<t_EffectUnit> &vec)
{
    for(std::vector<t_EffectUnit>::iterator it=vec.begin(); it!=vec.end();)
    {
	if(it->temp)
	{
	    it = vec.erase(it);
	}
	else
	{
	    ++it;
	}
    }
}

class ResetGameCardAttackTimes:public UserCardExec
{
public:
	ResetGameCardAttackTimes()
	{
	}
	bool exec(zCard *object)
	{
	    if(object->data.pos.loc() >= Cmd::CARDCELLTYPE_COMMON && object->data.pos.loc() <= Cmd::CARDCELLTYPE_HERO)
	    {
		object->resetAttackTimes();
	    }
	    return true;
	}
};
/**
 * \brief 重置可攻击次数
 * \param
 * \return
*/
void ChallengeGame::dealResetGameCardAttackTimes()
{
    ResetGameCardAttackTimes at;
    this->slots.gcm.execEvery(at);
}

class RefreshCardState:public UserCardExec
{
    public:
	ChallengeGame *_game;
	RefreshCardState(ChallengeGame *game)
	{
	    _game = game;
	}
	bool exec(zCard *object)
	{
	    if(object->data.pos.loc() == Cmd::CARDCELLTYPE_COMMON /*|| object->data.pos.loc() == Cmd::CARDCELLTYPE_HERO*/)
	    {
		if(object->data.state && _game)
		{
		    _game->sendCardStateInfo(object);
		}
	    }
	    return true;
	}
};
/**
 * \brief 刷新所有卡牌状态到客户端
 * \param
 * \return
*/
void ChallengeGame::dealRefreshCardState()
{
    RefreshCardState at(this);
    this->slots.gcm.execEvery(at);
}

class CheckFreezeState:public UserCardExec
{
    public:
	ChallengeGame *_game;
	CheckFreezeState(ChallengeGame *game)
	{
	    _game = game;
	}
	bool exec(zCard *object)
	{
	    if(object->data.pos.loc() == Cmd::CARDCELLTYPE_COMMON || object->data.pos.loc() == Cmd::CARDCELLTYPE_HERO)
	    {
		if(object->isFreeze() && _game->getTotalRoundCount() > object->pk.freeze_round)
		{
		    object->clearFreeze();
		}
	    }
	    return true;
	}
};
/**
 * \brief 重置冻结状态
 * \param
 * \return
*/
void ChallengeGame::dealCheckFreezeState()
{
    CheckFreezeState at(this);
    this->slots.gcm.execEvery(at);
}

/**
 * \brief 获得自己英雄
 * \param
 * \return
*/
zCard* ChallengeGame::getSelfHero(DWORD playerID)
{
    zCard *pDef = NULL;
    if(playerID == players[0].playerID)
    {
	this->slots.hero1.getObjectByZone(&pDef, 0, 0);
    }
    else if(playerID == players[1].playerID)
    {
	this->slots.hero2.getObjectByZone(&pDef, 0, 0);
    }
    return pDef;
}

/**
 * \brief 获得敌方英雄
 * \param
 * \return
*/
zCard* ChallengeGame::getEnemyHero(DWORD playerID)
{
    zCard *pDef = NULL;
    if(playerID == players[0].playerID)
    {
	this->slots.hero2.getObjectByZone(&pDef, 0, 0);
    }
    else if(playerID == players[1].playerID)
    {
	this->slots.hero1.getObjectByZone(&pDef, 0, 0);
    }
    return pDef;
}

/**
 * \brief 获得自己武器
 * \param
 * \return
*/
zCard* ChallengeGame::getSelfEquip(DWORD playerID)
{
    zCard *pDef = NULL;
    if(playerID == players[0].playerID)
    {
	this->slots.equip1.getObjectByZone(&pDef, 0, 0);
    }
    else if(playerID == players[1].playerID)
    {
	this->slots.equip2.getObjectByZone(&pDef, 0, 0);
    }
    return pDef;
}

/**
 * \brief 获得敌方武器
 * \param
 * \return
*/
zCard* ChallengeGame::getEnemyEquip(DWORD playerID)
{
    zCard *pDef = NULL;
    if(playerID == players[0].playerID)
    {
	this->slots.equip2.getObjectByZone(&pDef, 0, 0);
    }
    else if(playerID == players[1].playerID)
    {
	this->slots.equip1.getObjectByZone(&pDef, 0, 0);
    }
    return pDef;
}

/**
 * \brief 获得自己hero技能
 * \param
 * \return
*/
zCard* ChallengeGame::getSelfSkill(DWORD playerID)
{
    zCard *pDef = NULL;
    if(playerID == players[0].playerID)
    {
	this->slots.skill1.getObjectByZone(&pDef, 0, 0);
    }
    else if(playerID == players[1].playerID)
    {
	this->slots.skill2.getObjectByZone(&pDef, 0, 0);
    }
    return pDef;
}

/**
 * \brief 获得敌方hero技能
 * \param
 * \return
*/
zCard* ChallengeGame::getEnemySkill(DWORD playerID)
{
    zCard *pDef = NULL;
    if(playerID == players[0].playerID)
    {
	this->slots.skill2.getObjectByZone(&pDef, 0, 0);
    }
    else if(playerID == players[1].playerID)
    {
	this->slots.skill1.getObjectByZone(&pDef, 0, 0);
    }
    return pDef;
}

SceneUser* ChallengeGame::getOther(DWORD playerID)
{
    SceneUser *other = NULL;
    if(playerID == players[0].playerID)
    {
	other = SceneUserManager::getMe().getUserByID(players[1].playerID);
    }
    else if(playerID == players[1].playerID)
    {
	other = SceneUserManager::getMe().getUserByID(players[0].playerID);
    }
    return other;
}

/**
 * \brief
 * \param desc:描述
 * \param flag:true,手动释放的法术
 *	       false,触发出来的法术
 * \return
*/
bool ChallengeGame::action(char* desc, bool flag)
{
    if(actionList.empty())
    {
	return false;
    }
    Zebra::logger->debug("ChallengeGame::action 执行表大小:%u", actionList.size());
    DWORD dwEntryID = actionList.front().dwAttThisID;
    DWORD dwMagicType = actionList.front().dwMagicType;
    zCard* pEntry = this->slots.gcm.getObjectByThisID(dwEntryID);

    Cmd::stCardAttackMagicUserCmd in;
    in.dwAttThisID = dwEntryID;
    in.dwDefThisID = actionList.front().dwDefThisID;
    in.dwMagicType = dwMagicType;
    bool flag2 = actionList.front().flag;
    
    actionList.pop_front();

    if(!pEntry)
    {
	return false;
    }

    zSkillB *skillbase = skillbm.get(dwMagicType);
    if(!skillbase)
    {
	return false;
    }
    Zebra::logger->debug("[技能action] %s 释放:%u",desc, dwMagicType);
    std::vector<SkillStatus>::const_iterator iter;
    for(iter = skillbase->skillStatus.begin(); iter!=skillbase->skillStatus.end(); iter++)
    {
	SkillStatus *pSkillStatus = (SkillStatus *)&*iter;
	doOperation(pSkillStatus, pEntry, &in, flag?flag:flag2);
    }
    dealHurtAction();
    dealCureAction();

    if(skillbase->conditionType && skillbase->conditionID)  //额外的后续效果
    {
	bool extra = checkExtraCondition(skillbase->conditionType, skillbase->conditionID, in.dwDefThisID, pEntry);
	if(extra)
	{
	    Cmd::stCardAttackMagicUserCmd inA;
	    inA.dwAttThisID = dwEntryID;
	    inA.dwDefThisID = in.dwDefThisID;
	    inA.dwMagicType = skillbase->skillAID;
	    inA.flag = 1;
	    addActionList(inA);
	}
	else
	{
	    Cmd::stCardAttackMagicUserCmd inB;
	    inB.dwAttThisID = dwEntryID;
	    inB.dwDefThisID = in.dwDefThisID;
	    inB.dwMagicType = skillbase->skillBID;
	    inB.flag = 1;
	    addActionList(inB);
	}
    }


    while(!actionList.empty())
    {
	action( "执行");		    //执行效果
    }

    //检查死亡情况
    dealDeadAction();
    return true;
}

/**
 * \brief
 * \param entry:技能释放者
 * \param desc:描述
 * \param flag:true,手动释放的法术
 *	       false,触发出来的法术
 * \return
*/
void ChallengeGame::doOperation(const SkillStatus *pSkillStatus, zCard* entry, const Cmd::stCardAttackMagicUserCmd *rev, bool flag)
{
    entry->carrier.status = pSkillStatus;
    entry->carrier.skillbase = skillbm.get(rev->dwMagicType);
    entry->carrier.revCmd = *rev;
    entry->carrier.attacker = entry;

    DWORD playerID = entry->playerID;
    DWORD mainThisID = entry->data.qwThisID;

    WORD wdAttack = pSkillStatus->attack;
    WORD range = pSkillStatus->range;	    
    WORD condition = pSkillStatus->condition;	    
    WORD mode = pSkillStatus->mode;
    WORD num = pSkillStatus->num;
    WORD useHand = pSkillStatus->useHand;   //是否使用手选目标
    
    std::vector<DWORD> defList;
    if(wdAttack == 1)		//单攻
    {
	zCard* pDef = this->slots.gcm.getObjectByThisID(rev->dwDefThisID);
	if(flag && useHand)	//手动释放的
	{
	    if(!pDef)
	    {
		Zebra::logger->error("单攻技能%u选择的目标不存在",rev->dwMagicType);
		return;
	    }
	}
	else	//触发的或者不用手选目标的
	{
	    if(range & Cmd::ATTACK_TARGET_SHERO)	//自己英雄
	    {
		pDef = getSelfHero(playerID);
	    }
	    else if(range & Cmd::ATTACK_TARGET_EHERO)	//敌方英雄
	    {
		pDef = getEnemyHero(playerID);
	    }
	    else if(range & Cmd::ATTACK_TARGET_MYSELF)	//施法者自身
	    {
		pDef = entry;
	    }
	    else if(range & Cmd::ATTACK_TARGET_EEQUIP)	//敌方武器
	    {
		pDef = getEnemyEquip(playerID);
	    }
	    else if(range & Cmd::ATTACK_TARGET_SEQUIP)	//己方武器
	    {
		pDef = getSelfEquip(playerID);
	    }
	}
	if(!pDef)
	    return;

	std::vector<DWORD> thisID_Vec;
	if(pDef->isAttend() && (pSkillStatus->range & Cmd::ATTACK_TARGET_LEFT_RIGHT))	    //两侧
	{
	    if(getLeftRightID(thisID_Vec, pDef->data.qwThisID))
	    {
	    }
	}
	else
	{
	    if((pSkillStatus->range & Cmd::ATTACK_TARGET_EHERO)
		    || (pSkillStatus->range & Cmd::ATTACK_TARGET_EATTEND)
		    || (pSkillStatus->range & Cmd::ATTACK_TARGET_SHERO)
		    || (pSkillStatus->range & Cmd::ATTACK_TARGET_SATTEND)
		    || (pSkillStatus->range & Cmd::ATTACK_TARGET_MYSELF))
	    {
		thisID_Vec.push_back(pDef->data.qwThisID);
	    }
	}
	for(DWORD i=0; i<thisID_Vec.size(); i++)
	{
	    DWORD finalThisID = thisID_Vec[i];
	    zCard *p = this->slots.gcm.getObjectByThisID(finalThisID);
	    if(p)
	    {
		defList.push_back(finalThisID);
		p->skillStatusM.putOperationToMe(entry->carrier, true);
		refreshClient(entry->data.qwThisID, defList, rev->dwMagicType);
	    }
	}
	checkHaloClear(entry->data.qwThisID, thisID_Vec);
    }
    else if(wdAttack == 2)	//群攻
    {
	std::vector<DWORD> thisID_Vec;
	collectTarget(playerID, range, thisID_Vec, condition);
	if(0 == mode)	//不包括本体
	{
	    std::vector<DWORD>::iterator it=std::find(thisID_Vec.begin(), thisID_Vec.end(), mainThisID);
	    if(it!=thisID_Vec.end())
	    {
		thisID_Vec.erase(it);
	    }
	}
	for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end(); )
	{
	    zCard *p = this->slots.gcm.getObjectByThisID(*it);
	    if((p && p->isDie())
		    ||(!p))
	    {
		it = thisID_Vec.erase(it);
	    }
	    else
	    {
		++it;
	    }
	}
	if(thisID_Vec.empty())	    
	    return;
	
	for(DWORD i=0; i<thisID_Vec.size(); i++)
	{
	    DWORD finalThisID = thisID_Vec[i];
	    zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	    if(pDef && !pDef->isDie())
	    {
		defList.push_back(finalThisID);
		pDef->skillStatusM.putOperationToMe(entry->carrier, true);
	    }
	}
	checkHaloClear(entry->data.qwThisID, thisID_Vec);
    }
    else if(wdAttack == 3)  //随机目标:随机num次目标
    {
	for(DWORD i=0; i<num; i++)
	{
	    std::vector<DWORD> thisID_Vec;
	    collectTarget(playerID, range, thisID_Vec, condition);
	    if(0 == mode)	//不包括本体
	    {
		std::vector<DWORD>::iterator it=std::find(thisID_Vec.begin(), thisID_Vec.end(), mainThisID);
		if(it!=thisID_Vec.end())
		{
		    thisID_Vec.erase(it);
		}
	    }
	    for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end(); )
	    {
		zCard *p = this->slots.gcm.getObjectByThisID(*it);
		if((p && p->isDie())
			||(!p))
		{
		    it = thisID_Vec.erase(it);
		}
		else
		{
		    ++it;
		}
	    }

	    if(thisID_Vec.empty())	    //待随机列表为空
		return;

	    DWORD index = zMisc::randBetween(0, thisID_Vec.size()-1);
	    DWORD finalThisID = thisID_Vec[index];
	    zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	    if(pDef)
	    {
		defList.push_back(finalThisID);
		pDef->skillStatusM.putOperationToMe(entry->carrier, true);
		refreshClient(entry->data.qwThisID, defList, rev->dwMagicType);
		defList.clear();
	    }
	}
    }
    else if(wdAttack == 4)  //随机目标:随机num个目标
    {
	std::vector<DWORD> thisID_Vec;
	collectTarget(playerID, range, thisID_Vec, condition);
	if(0 == mode)	//不包括本体
	{
	    std::vector<DWORD>::iterator it=std::find(thisID_Vec.begin(), thisID_Vec.end(), mainThisID);
	    if(it!=thisID_Vec.end())
	    {
		thisID_Vec.erase(it);
	    }
	}
	for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end(); )
	{
	    zCard *p = this->slots.gcm.getObjectByThisID(*it);
	    if((p && p->isDie())
		    ||(!p))
	    {
		it = thisID_Vec.erase(it);
	    }
	    else
	    {
		++it;
	    }
	}
	if(thisID_Vec.empty())	    //待随机列表为空
	    return;

	std::random_shuffle(thisID_Vec.begin(), thisID_Vec.end());
	for(DWORD i=0; i<thisID_Vec.size() && i<num; i++)
	{
	    DWORD finalThisID = thisID_Vec[i];
	    zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	    if(pDef)
	    {
		defList.push_back(finalThisID);
		pDef->skillStatusM.putOperationToMe(entry->carrier, true);
	    }
	}

    }
    if(entry 
	    && (wdAttack == 2 || wdAttack == 4))
    {
	refreshClient(entry->data.qwThisID, defList, rev->dwMagicType);
    }
}

struct birthInfo
{
    DWORD dwThisID;
    DWORD dwTime;
    birthInfo()
    {
	dwThisID = 0;
	dwTime = 0;
    }
};
bool lessmark(const birthInfo& b1, const birthInfo& b2)  
{  
    return   (b1.dwTime   <   b2.dwTime);  
}

/**
 * \brief   收集作用目标
 * \param flag 范围
 * \param condition 条件
 * \return
*/
bool ChallengeGame::collectTarget(DWORD playerID, const DWORD flag, std::vector<DWORD> &targets, const DWORD condition)
{
    std::vector<birthInfo> birthInfoVec;
    birthInfoVec.clear();

    if(flag & Cmd::ATTACK_TARGET_EHERO)
    {
	zCard *pDef = NULL;
	pDef = getEnemyHero(playerID);
	if(pDef)
	{
	    birthInfo info;
	    info.dwThisID = pDef->data.qwThisID;
	    info.dwTime = pDef->playingTime;
	    birthInfoVec.push_back(info);
	}
    }
    if(flag & Cmd::ATTACK_TARGET_EATTEND)
    {
	if(playerID == players[0].playerID)
	{
	    zCard *pDef = NULL;
	    for(WORD i=0; i<this->slots.main2.size(); i++)
	    {
		pDef = this->slots.main2.container[i];
		if(pDef)
		{
		    birthInfo info;
		    info.dwThisID = pDef->data.qwThisID;
		    info.dwTime = pDef->playingTime;
		    birthInfoVec.push_back(info);
		}
	    }
	}
	else if(playerID == players[1].playerID)
	{
	    zCard *pDef = NULL;
	    for(WORD i=0; i<this->slots.main1.size(); i++)
	    {
		pDef = this->slots.main1.container[i];
		if(pDef)
		{
		    birthInfo info;
		    info.dwThisID = pDef->data.qwThisID;
		    info.dwTime = pDef->playingTime;
		    birthInfoVec.push_back(info);
		}
	    }
	}
    }
    if(flag & Cmd::ATTACK_TARGET_SHERO)
    {
	zCard *pDef = NULL;
	pDef = getSelfHero(playerID);
	if(pDef)
	{
	    birthInfo info;
	    info.dwThisID = pDef->data.qwThisID;
	    info.dwTime = pDef->playingTime;
	    birthInfoVec.push_back(info);
	}
    }
    if(flag & Cmd::ATTACK_TARGET_SATTEND)
    {
	if(playerID == players[0].playerID)
	{
	    zCard *pDef = NULL;
	    for(WORD i=0; i<this->slots.main1.size(); i++)
	    {
		pDef = this->slots.main1.container[i];
		if(pDef)
		{
		    birthInfo info;
		    info.dwThisID = pDef->data.qwThisID;
		    info.dwTime = pDef->playingTime;
		    birthInfoVec.push_back(info);
		}
	    }
	}
	else if(playerID == players[1].playerID)
	{
	    zCard *pDef = NULL;
	    for(WORD i=0; i<this->slots.main2.size(); i++)
	    {
		pDef = this->slots.main2.container[i];
		if(pDef)
		{
		    birthInfo info;
		    info.dwThisID = pDef->data.qwThisID;
		    info.dwTime = pDef->playingTime;
		    birthInfoVec.push_back(info);
		}
	    }
	}
    }
    if(flag & Cmd::ATTACK_TARGET_SHAND)
    {
	if(playerID == players[0].playerID)
	{
	    zCard *pDef = NULL;
	    for(WORD i=0; i<this->slots.hand1.size(); i++)
	    {
		pDef = this->slots.hand1.container[i];
		if(pDef)
		{
		    birthInfo info;
		    info.dwThisID = pDef->data.qwThisID;
		    info.dwTime = pDef->playingTime;
		    birthInfoVec.push_back(info);
		}
	    }
	}
	else if(playerID == players[1].playerID)
	{
	    zCard *pDef = NULL;
	    for(WORD i=0; i<this->slots.hand2.size(); i++)
	    {
		pDef = this->slots.hand2.container[i];
		if(pDef)
		{
		    birthInfo info;
		    info.dwThisID = pDef->data.qwThisID;
		    info.dwTime = pDef->playingTime;
		    birthInfoVec.push_back(info);
		}
	    }
	}
    }
    if(flag & Cmd::ATTACK_TARGET_EHAND)
    {
	if(playerID == players[0].playerID)
	{
	    zCard *pDef = NULL;
	    for(WORD i=0; i<this->slots.hand2.size(); i++)
	    {
		pDef = this->slots.hand2.container[i];
		if(pDef)
		{
		    birthInfo info;
		    info.dwThisID = pDef->data.qwThisID;
		    info.dwTime = pDef->playingTime;
		    birthInfoVec.push_back(info);
		}
	    }
	}
	else if(playerID == players[1].playerID)
	{
	    zCard *pDef = NULL;
	    for(WORD i=0; i<this->slots.hand1.size(); i++)
	    {
		pDef = this->slots.hand1.container[i];
		if(pDef)
		{
		    birthInfo info;
		    info.dwThisID = pDef->data.qwThisID;
		    info.dwTime = pDef->playingTime;
		    birthInfoVec.push_back(info);
		}
	    }
	}
    }
    if(flag & Cmd::ATTACK_TARGET_SEQUIP)
    {
	zCard *pDef = NULL;
	pDef = getSelfEquip(playerID);
	if(pDef)
	{
	    birthInfo info;
	    info.dwThisID = pDef->data.qwThisID;
	    info.dwTime = pDef->playingTime;
	    birthInfoVec.push_back(info);
	}
    }
    if(flag & Cmd::ATTACK_TARGET_EEQUIP)
    {
	zCard *pDef = NULL;
	pDef = getEnemyEquip(playerID);
	if(pDef)
	{
	    birthInfo info;
	    info.dwThisID = pDef->data.qwThisID;
	    info.dwTime = pDef->playingTime;
	    birthInfoVec.push_back(info);
	}
    }

    if(condition)		//条件筛选
    {
	if(condition & Cmd::TARGET_CONDITION_INJURED)
	{
	    for(std::vector<birthInfo>::iterator it=birthInfoVec.begin(); it!=birthInfoVec.end();)
	    {
		zCard* p = this->slots.gcm.getObjectByThisID(it->dwThisID);
		if(p && !p->injured())
		{
		    it = birthInfoVec.erase(it);
		}
		else
		{
		    ++it;
		}
	    }
	}
	if(condition & Cmd::TARGET_CONDITION_DAMG2)
	{
	    for(std::vector<birthInfo>::iterator it=birthInfoVec.begin(); it!=birthInfoVec.end();)
	    {
		zCard* p = this->slots.gcm.getObjectByThisID(it->dwThisID);
		if(p && !p->damageGreat2())
		{
		    it = birthInfoVec.erase(it);
		}
		else
		{
		    ++it;
		}
	    }
	}
	if(condition & Cmd::TARGET_CONDITION_DAML3)
	{
	    for(std::vector<birthInfo>::iterator it=birthInfoVec.begin(); it!=birthInfoVec.end();)
	    {
		zCard* p = this->slots.gcm.getObjectByThisID(it->dwThisID);
		if(p && !p->damageLess3())
		{
		    it = birthInfoVec.erase(it);
		}
		else
		{
		    ++it;
		}
	    }
	}
	if(condition & Cmd::TARGET_CONDITION_DEADLAN)
	{
	    for(std::vector<birthInfo>::iterator it=birthInfoVec.begin(); it!=birthInfoVec.end();)
	    {
		zCard* p = this->slots.gcm.getObjectByThisID(it->dwThisID);
		if(p && !p->hasDeadLanguage())
		{
		    it = birthInfoVec.erase(it);
		}
		else
		{
		    ++it;
		}
	    }
	}

    }
    
    std::sort(birthInfoVec.begin(), birthInfoVec.end(), lessmark);	//升序
    for(std::vector<birthInfo>::iterator it=birthInfoVec.begin(); it!=birthInfoVec.end(); it++)
    {
	targets.push_back(it->dwThisID);
    }

    if(targets.empty())	    //目标列表为空
	return false;
    return true;
}

/**
 * \brief   获取全场除targets以外的目标
 * \param targets:需要排除的目标
 * \return
*/
bool ChallengeGame::collectNotTarget(DWORD playerID, std::vector<DWORD> targets, std::vector<DWORD> &nottargets)
{
    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_SEQUIP
	+ Cmd::ATTACK_TARGET_EHERO + Cmd::ATTACK_TARGET_EATTEND + Cmd::ATTACK_TARGET_EEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);

    std::sort(thisID_Vec.begin(), thisID_Vec.end());
    std::sort(targets.begin(), targets.end());

    std::vector<DWORD>::iterator it;
    //必须保证nottargets足够大,否则将挂掉
    it = std::set_difference(thisID_Vec.begin(), thisID_Vec.end(), targets.begin(), targets.end(), nottargets.begin());
    nottargets.resize(it-nottargets.begin());

    if(nottargets.empty())
	return false;
    return true;
}

/**
 * \brief
 * \param entry:光环释放者
 * \param desc:描述
 * \return
*/
bool ChallengeGame::actionHalo(char* desc)
{
    if(actionHaloList.empty())
    {
	return false;
    }
    Zebra::logger->debug("ChallengeGame::actionHalo 执行表大小:%u", actionHaloList.size());
    DWORD dwEntryID = actionHaloList.front().dwAttThisID;
    DWORD dwMagicType = actionHaloList.front().dwMagicType;
    zCard* pEntry = this->slots.gcm.getObjectByThisID(dwEntryID);

    Cmd::stCardAttackMagicUserCmd in;
    in.dwAttThisID = dwEntryID;
    in.dwDefThisID = actionHaloList.front().dwDefThisID;
    in.dwMagicType = dwMagicType;
    
    actionHaloList.pop_front();

    if(!pEntry)
    {
	return false;
    }

    zSkillB *skillbase = skillbm.get(dwMagicType);
    if(!skillbase)
    {
	return false;
    }
    Zebra::logger->debug("[光环action] %s 释放:%u",desc, dwMagicType);
    std::vector<SkillStatus>::const_iterator iter;
    for(iter = skillbase->skillStatus.begin(); iter!=skillbase->skillStatus.end(); iter++)
    {
	SkillStatus *pSkillStatus = (SkillStatus *)&*iter;
	doOperation(pSkillStatus, pEntry, &in, false);
    }

    while(!actionHaloList.empty())
    {
	actionHalo("执行光环");		    //执行效果
    }
    return true;
}

bool ChallengeGame::checkExtraCondition(const DWORD type, const DWORD conditionID, const DWORD dwDefThisID, zCard* entry)
{
    switch(type)
    {
	case Cmd::EXTRA_CONDITION_TYPE_EXIST:
	    return checkExist(conditionID, entry);
	    break;
	case Cmd::EXTRA_CONDITION_TYPE_STATE:
	    return checkHasState(conditionID, dwDefThisID);
	    break;
	case Cmd::EXTRA_CONDITION_TYPE_ISXXX:
	    return checkIsSomeOne(conditionID, dwDefThisID, entry);
	    break;
	case 10000:	//无条件为真
	    return true;
	    break;
	default:
	    break;
    }
    return false;
}

/**
 * \brief 判断entry的光环是否开启
 * \param
 * \return
*/
bool ChallengeGame::checkHaloOpen(const DWORD type, const DWORD conditionID, zCard* entry)
{
    switch(type)
    {
	case Cmd::EXTRA_CONDITION_TYPE_EXIST:
	case Cmd::EXTRA_CONDITION_TYPE_STATE:
	    return checkExtraCondition(type, conditionID, entry->data.qwThisID, entry);
	    break;
	case 10000:	//无条件为真
	    return true;
	    break;
	default:
	    break;
    }
    return false;
}

bool ChallengeGame::checkExist(const DWORD conditionID, zCard* entry)
{
    switch(conditionID)
    {
	case 1:
	    {
		if(entry->playerID == players[0].playerID)
		{
		    zCard *pDef = NULL;
		    for(WORD i=0; i<this->slots.main1.size(); i++)
		    {
			pDef = this->slots.main1.container[i];
			if(pDef && pDef->base->race == 1)
			{
			    return true;
			}
		    }
		}
		else if(entry->playerID == players[1].playerID)
		{
		    zCard *pDef = NULL;
		    for(WORD i=0; i<this->slots.main2.size(); i++)
		    {
			pDef = this->slots.main2.container[i];
			if(pDef && pDef->base->race == 1)
			{
			    return true;
			}
		    }
		}
	    }
	    break;
	default:
	    break;
    }
    return false;
}

bool ChallengeGame::checkHasState(const DWORD conditionID, const DWORD dwDefThisID)
{
    zCard* pDef = this->slots.gcm.getObjectByThisID(dwDefThisID);
    if(!pDef)
	return false;
    switch(conditionID)
    {
	case 1:
	    return pDef->injured();
	    break;
	case 2:
	    return pDef->hasShield();
	    break;
	case 3:
	    return pDef->isDie();
	    break;
	default:
	    break;
    }
    return false;
}

bool ChallengeGame::checkIsSomeOne(const DWORD conditionID, const DWORD dwDefThisID, zCard* entry)
{
    zCard* pDef = this->slots.gcm.getObjectByThisID(dwDefThisID);
    if(!pDef)
	return false;
    switch(conditionID)
    {
	case 3:
	    return (pDef->isHero() && entry->isEnemy(pDef));
	    break;
	default:
	    break;
    }
    return false;
}

bool ChallengeGame::checkHaloClear(const DWORD mainThisID, std::vector<DWORD> targets)
{
    std::vector<DWORD> nottargets(100);
    if(collectNotTarget(privilegeUser, targets, nottargets))
    {
	for(std::vector<DWORD>::iterator it = nottargets.begin(); it!=nottargets.end(); it++)
	{//清除这这些身上的mainThisID光环
	    zCard *pDef = this->slots.gcm.getObjectByThisID(*it);
	    if(pDef)
		pDef->removeOneHaloInfo(mainThisID); 
	}
    }
    return true;
}

/**
 * \brief 清除由mainThisID释放的所有效果
 * \param
 * \return
*/
bool ChallengeGame::clearAllHalo(const DWORD mainThisID)
{
    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_SEQUIP
	+ Cmd::ATTACK_TARGET_EHERO + Cmd::ATTACK_TARGET_EATTEND + Cmd::ATTACK_TARGET_EEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);
    
    for(DWORD i=0; i<thisID_Vec.size(); i++)
    {
	DWORD finalThisID = thisID_Vec[i];
	zCard *pDef = this->slots.gcm.getObjectByThisID(finalThisID);
	if(pDef)
	{
	    pDef->removeOneHaloInfo(mainThisID);	//删除mainThisID给pDef的光环
	}
    }
    return true;
}
