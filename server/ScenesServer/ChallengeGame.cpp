/*************************************************************************
 Author: wang
 Created Time: 2014��12��11�� ������ 10ʱ47��11��
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
    Zebra::logger->debug("[����]������һ��game,����(%u)����:%s(%u) ���1(%u������:%u) ���2(%u, ����:%u)",gameID, gameType2Name(gameType), gameType, playerID1, cardsNumber1, playerID2, cardsNumber2);
    
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
 * \brief   ������ֽ�ɫID
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
    Zebra::logger->debug("[����] PVP ս��:%u ������һ��:(%s)�� ��:(%s(%u,%u)) �� ��ɫ(%s,%u)",o->gameID, o->base->name, slotType2Name(slot).c_str(), o->data.pos.x, o->data.pos.y, getPlayerName(o->playerID), o->playerID);
}

/**
 * \brief ��ʼ����ɫ�ƿ�
 * \param
 * \return
*/
bool ChallengeGame::initCardsLib(SceneUser *pUser, std::vector<DWORD> &libVec)
{
    if(!pUser)
	return false;
    DWORD index = pUser->ctData.cardsIndex;
    if(index < 1000)	//���������㷨(������ְҵ+�����������M��)
    {}
    else if(index >= 1000)	//�Զ��������㷨(������Լ��趨������ȥ���)
    {
	if(GroupCardManager::getMe().initOneChallengeCards(*pUser, index, libVec))
	{
	    std::random_shuffle(libVec.begin(), libVec.end());
	    Zebra::logger->debug("[����] (%u)���� (%s,%u) ��ʼ���ƿ�", gameID, pUser->name, pUser->id);
	    for(std::vector<DWORD>::iterator it=libVec.begin(); it!=libVec.end(); it++)
	    {
		zCardB *cb = cardbm.get(*it);
		if(cb)
		{
		    Zebra::logger->debug("[����] (%u)���� (%s,%u) (%s,%u)", gameID, pUser->name, pUser->id, cb->name, cb->id);
		}
	    }
	    return true;
	}
    }
    return false;
}

/**
 * \brief ��ʼ����ս������Ϣ
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
	
	//��ʼ��BOSS��Ӣ�ۺ�Ӣ�ۼ���
    }

	
    //��ʼ��������1
    for(BYTE i=0; i<players[0].prepareHand.size(); i++)
    {
	CardToHandSlot(players[0].playerID, players[0].prepareHand[i], true);
    }

    //��ʼ��������2
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
	    Zebra::logger->debug("[����]��ʼ����(%u)���ζ�ս���1(%s,%u)���� %s",gameID, pUser->name, pUser->id, players[0].isInited()?"OK":"FAIL");
	}
	else if(pUser->id == players[1].playerID && pUser->ctData.cardsIndex == players[1].cardsNumber)
	{
	    strncpy(players[1].playerName, pUser->name, MAX_NAMESIZE);
	    if(initCardsLib(pUser, players[1].cardsLibVec))
	    {
		players[1].setInit();
	    }
	    Zebra::logger->debug("[����]��ʼ����(%u)���ζ�ս���2(%s,%u)���� %s",gameID, pUser->name, pUser->id, players[1].isInited()?"OK":"FAIL");
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
	    Zebra::logger->debug("[����]��ʼ����(%u)���ζ�ս���1(%s,%u)���� %s",gameID, pUser->name, pUser->id, players[0].isInited()?"OK":"FAIL");

	    //��ʼ��BOSS���ƿ�
	}
    }
    return true;
}


/**
 * \brief ��ҵ��ߺ���ν���
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
	    Zebra::logger->debug("[����] PVP ս��:%u ���1(%s,%u)���ߺ��ٴν�����Ϸ",gameID, pUser->name, pUser->id);
	}
	else if(pUser->id == players[1].playerID && pUser->ctData.cardsIndex == players[1].cardsNumber)
	{
	    Zebra::logger->debug("[����] PVP ս��:%u ���2(%s,%u)���ߺ��ٴν�����Ϸ",gameID, pUser->name, pUser->id);
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
	Zebra::logger->error("[PK] ������(%s:%u)�Ļغ� ����ë��", user.name, user.id);
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
 * \brief ��ͨ����
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
	Zebra::logger->error("��ͨ����  ���Ŀ��ѡ�����");
	Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "��ͨ���� ������֤ʧ�ܻ���Ŀ��ѡ�����");
	return false;
    }
    Zebra::logger->debug("[PK] ������%s(%s,%u) ������%s", pAtt->base->name, user.name, user.id, pDef->base->name);
    
    DWORD now = SceneTimeTick::currentTime.sec();
    DWORD pAttHisID = 0;
    DWORD pDefHisID = 0;
    CardToRecordSlot(user.id, pAtt, now, pAttHisID);
    CardToRecordSlot(user.id, pDef, now, pDefHisID);

    std::vector<DWORD> vec;
    vec.push_back(pDefHisID);
    sendBattleHistoryInfo(pAttHisID, vec);	    //��ͨ������ʷ

    if(pAtt->pk.sAttackID)
    {
	Cmd::stCardAttackMagicUserCmd in;
	in.dwAttThisID = rev->dwAttThisID;
	in.dwDefThisID = 0;
	in.dwMagicType = pAtt->pk.sAttackID;
	addActionList(in);
	startOneFlow();
	action("��ʼ����");		    //��ʼ����ʱ����
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
	action("������");		    //������ʱ����
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

    dealAttackEndAction(pAtt->data.qwThisID, pDef->data.qwThisID, defHurt);	//pAtt��������Ч��
    dealAttackEndAction(pDef->data.qwThisID, pAtt->data.qwThisID, attHurt);	//pDef��������Ч��

    dealHaloEffectAction();

    const DWORD NONE_SKILL_ID = 1;	    //һ���յļ���
    Cmd::stCardAttackMagicUserCmd in;
    in.dwAttThisID = pAtt->data.qwThisID;
    in.dwDefThisID = 0;
    in.dwMagicType = NONE_SKILL_ID;
    addActionList(in);

    action( "��ͨ����");
    endOneFlow();
    

    return true;

}

/**
 * \brief �����ƹ���
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
	Zebra::logger->error("SKILL  %s ���Ƿ����� Ҳ����Ӣ�ۼ��� ",pAtt->base->name);
	Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "�ͷż���ʧ��  ����ӵ���߼Ȳ��Ƿ��� Ҳ����Ӣ������");
	return false;
    }
    if(pAtt->base->needTarget)
    {
	zCard *pDef = this->slots.gcm.getObjectByThisID(rev->dwDefThisID);
	if(pDef && pDef->canNotAsFashuTarget())
	{
	    Zebra::logger->error("SKILL  ���Ŀ��ѡ����� ��Ŀ�겻�ܱ�������Ӣ�ۼ���ѡ��");
	    Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "������ �ͷż���ʧ�� ��Ŀ�겻�ܱ�������Ӣ�ۼ���ѡ��");
	    return false;
	}
	if(!checkSelectedTarget(pAtt, pAtt->base->needTarget, rev->dwDefThisID))
	{
	    Zebra::logger->error("SKILL  ���Ŀ��ѡ�����");
	    Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "������ �ͷż���ʧ�� ���Ŀ��ѡ�����");
	    return false;
	}
    }
    if(pAtt->isHeroMagicCard() && !pAtt->checkAttackTimes())
    {
	Zebra::logger->error("SKILL  Ӣ�ۼ�����(%u)ֻ��ʹ��һ�� ", pAtt->playerID);
	Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "�ͷż���ʧ��  Ӣ������ÿ�غ�ֻ��ʹ��һ��");
	return false;
    }
    
    if(!checkMp(pAtt->playerID, pAtt->data.mpcost))
    {
	Zebra::logger->error("SKILL  ���������Ҫ��(%u) ��(%u)������ˮ�� ", pAtt->playerID, pAtt->data.mpcost);
	Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "�ͷż���ʧ��  ���������Ҫ����%u������ˮ���ſ���",pAtt->data.mpcost);
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

    dealUseMagicCardAction();	    //ʹ�÷����ƴ�������Ч��
    action( "������ʹ��");
   
    zCard *hero = getSelfHero(user.id);
    if(hero)
    {
	Cmd::stCardAttackMagicUserCmd in;
	in.dwAttThisID = hero->data.qwThisID;
	in.dwDefThisID = rev->dwDefThisID;
	in.dwMagicType = rev->dwMagicType;
	addActionList(in);
	action( "�����ƻ�Ӣ�ۼ���", true);
    }

    return true;
}

/**
 * \brief ���ƴ���
 * \param
 * \return
*/
bool ChallengeGame::cardMoveAndAttack(SceneUser &user, const Cmd::stCardMoveAndAttackMagicUserCmd *rev)
{
    if(!isInGame(user.id))
	return false;
    if(!isHavePrivilege(user.id))
    {
	Zebra::logger->error("[����] ������Ļغϡ������Ƿ�  %s(%u)", user.name, user.id);
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
		    Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "�������Ѿ�����");
		    return false;
		}
	    }
	    else
	    {
		if(this->slots.main2.space() == 0)
		{
		    Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "�������Ѿ�����");
		    return false;
		}
	    }
	}
	//�����߼���Ҫ˼·
	//���ս��Ч����Ҫѡ��Ŀ�꣬�ж�ѡ���Ŀ���Ƿ�Ϸ�
	//�����ж��ƶ�λ���Ƿ�Ϸ�
	//�ж������Ƿ�����
	//���������ƶ�����
	//����������ս��Ч��
	//�������


	bool noTarget = false;
	if(pAtt->hasShout())    //ս����
	{
	    if(pAtt->base->shoutTarget) //��ҪĿ���ս��
	    {
		if(canFindTarget(pAtt, pAtt->base->shoutTarget))    //ȷʵ�п�ѡ��Ŀ��
		{
		    if(!checkSelectedTarget(pAtt, pAtt->base->shoutTarget, rev->dwDefThisID))
		    {
			Zebra::logger->error("SKILL  ���Ŀ��ѡ�����");
			Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "ս�� �ͷż���ʧ�� ���Ŀ��ѡ�����");
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
	if(!moveUserCard(user, rev->dwAttThisID, rev->dst))  //���������,�����ƶ�,������ʷ��¼
	{
	    Zebra::logger->error("����%s�ƶ�ʧ��",pAtt->base->name);
	    return false;
	}
	dealOtherEffectAction(pAtt->data.qwThisID, Cmd::ACTION_TYPE_USEATTEND);	    //ʹ�������
	action( "ʹ�������");		    //ʹ������ƴ���

	if(pAtt->hasShout() && !noTarget)    //ս����
	{
	    Cmd::stCardAttackMagicUserCmd in;
	    in.dwAttThisID = rev->dwAttThisID;
	    in.dwDefThisID = rev->dwDefThisID;
	    in.dwMagicType = pAtt->pk.shoutID;
	    addActionList(in);
	    action("�ͷ�ս��", true);		    //�ͷ�ս��
	}

	dealOtherEffectAction(pAtt->data.qwThisID, Cmd::ACTION_TYPE_ATTEND_IN);	    //����ϳ�
	dealHaloEffectAction();	    //�⻷���
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
	case CHALLENGE_STATE_NONE:	//��Ϸ�Ѵ���(��˫���������)
	    {
		DWORD now = SceneTimeTick::currentTime.sec();
		if(now > createTime+200)
		{
		    Zebra::logger->debug("[����] ��:%u���� ��ս������:%u ���ʱ��û�г�ʼ��˫������", gameID, now-createTime);
		    on_GameOver();
		    break;
		}
		if(players[0].isInited() && players[1].isInited())	//�����ս˫�����ݶ���ʼ�����
		{
		    sendEnemyBaseInfo();
		    //do something at end of init
		    upperHand = generateUpperHand();
		    //��ʼ����һ������
		    DWORD now = SceneTimeTick::currentTime.sec();
		    if(now > createTime)
		    {
			Zebra::logger->debug("[����] ��:%u���� ��ʼ���ܺ�ʱ :%u ��", gameID, now-createTime);
		    }
		    setPrivilege(upperHand);
		    Zebra::logger->debug("[����]��ʼ����(%u)���ζ�ս���������� ���ֵ������:%u",gameID, upperHand);
		    sendFirstHandCard(upperHand);
		    setState(CHALLENGE_STATE_PREPARE);	   
		}
	    }
	    break;
	case CHALLENGE_STATE_PREPARE:	//׼��״̬����û����
	    {
		challengePrepareTime++;
		Zebra::logger->debug("[����]�����Ƕ�ս׼���׶�Ŷ~ ����:%u",challengePrepareTime);
		if(challengePrepareTime >= cfg_prepareTime || (players[0].prepare && players[1].prepare))	    //׼��ʱ�������ֱ�ӿ�ս
		{
		    setState(CHALLENGE_STATE_BATTLE);

		    initBaseBattleInfo();

		    on_RoundStart();
		}
	    }
	    break;
	case CHALLENGE_STATE_BATTLE:	//��ս״̬
	    {
		challengeLastTime++;
		if(isPVP())
		{
		    if(SceneTimeTick::currentTime.sec() >= currRoundStartTime + cfg_roundTime)	    //��Ҳ�����ʱ��
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
			    Zebra::logger->debug("[����] ��:%u���� ˫��������ʱ�䳬�� %u �룬ϵͳǿ�ƽ���", gameID, notOnlineTime);
			    dealGameResult(upperHand, true);
			    on_GameOver();
			    break;
			}
		    }
		    if(totalRoundCount >= battle.limit.peaceNum)
		    {
			Zebra::logger->debug("[����] ��:%u���� ��ս������ %u �غ� ��ϵͳǿ�ƽ���", gameID, totalRoundCount);
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
	case CHALLENGE_STATE_CANCLEAR:	//�ȴ�����״̬
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
 * \brief �غϿ�ʼ
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
	    Zebra::logger->debug("[����] PVP ս��:%u %s(%u) �ĻغϿ�ʼ ��:(%u)�غ� ս���Ѿ�������:(%u)��",gameID, pUser->name, pUser->id, totalRoundCount, challengeLastTime);
	}
	else
	{
	    Zebra::logger->debug("[����] PVP ս��:%u ��ɫ������(%u) �ĻغϿ�ʼ ��:(%u)�غ� ս���Ѿ�������:(%u)��",gameID, privilegeUser, totalRoundCount, challengeLastTime);
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
 * \brief �غϽ���
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
	Zebra::logger->error("������ʱ�䱻�ص��� �����ⳡGAME");
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
	    Zebra::logger->debug("%s �ĻغϽ��� ��ʱ:%u�� �ܻغ���:%u",pUser->name, tmpTime, totalRoundCount);
	}
    }
    else if(isHavePrivilege(players[1].playerID))
    {
	setPrivilege(players[0].playerID);
	SceneUser *pUser = SceneUserManager::getMe().getUserByID(players[1].playerID);
	if(pUser)
	{
	    Zebra::logger->debug("%s �ĻغϽ��� ��ʱ:%u�� �ܻغ���:%u",pUser->name, tmpTime, totalRoundCount);
	}
    }
    dealCheckFreezeState();	//������ӵĶ���״̬
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
 * \brief ���ƿ��һ����
 * \param playerID ���ID
 *	  lib �ƿ������
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
	Zebra::logger->debug("[����] ��ƣ���� ���(%u)", playerID);
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
	sendBattleHistoryInfo(pAttHisID, vec);	    //ƣ���˺�Ӣ����ʷ

	if(o)
	{//��������
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
 * \brief ��ӵ��ƿ�
 * \param   playerID ���ID
 *	    cardID ����ID
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
    Zebra::logger->debug("[����] ����:(%u)��ӵ��� ��ɫ(%u)���ƿ��� ", cardID, playerID);
    return true;
}

/**
 * \brief ��ӵ����Ʋ�
 * \param   playerID ���ID
 *	    cardID ����ID
 *	    first �Ƿ����������
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
	    Zebra::logger->debug("[����] PVP ս��:%u %s ���� ��ɫ(%s,%u) ����(%u)����", gameID, slotType2Name(slot).c_str(), getPlayerName(playerID), playerID, cardID);
	    return false;
	}
    }
    else if(playerID == players[1].playerID)
    {
	slot = CardSlots::HAND2_PACK;
	if(this->slots.hand2.space() == 0)
	{
	    sendHandFullInfo(playerID, cardID);
	    Zebra::logger->debug("[����] PVP ս��:%u %s ���� ��ɫ(%s,%u) ����(%u)����", gameID, slotType2Name(slot).c_str(), getPlayerName(playerID), playerID, cardID);
	    return false;
	}
    }
    pOther = getOther(playerID);
    if(slot == CardSlots::HAND2_PACK || slot == CardSlots::HAND1_PACK)
    {
	zCard *outObj = NULL;
	if(CardToOneSlot(playerID, cardID, slot, outObj, "ADD ���Ʋ�"))
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
 * \brief �ٻ���Ӣ�۲�
 * \param   playerID ���ID
 *	    cardID ����ID
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
    return CardToOneSlot(playerID, cardID, slot, outObj, "ADD Ӣ�۲�");
}

/**
 * \brief �ٻ������ܲ�
 * \param   playerID ���ID
 *	    cardID ����ID
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
    return CardToOneSlot(playerID, cardID, slot, outObj, "ADD ���ܲ�");
}

/**
 * \brief �ٻ���������
 * \param   playerID ���ID
 *	    cardID ����ID
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
	if(CardToOneSlot(playerID, cardID, slot, outObj, "ADD ������"))
	{
	    if(outObj)
	    {
		equipEnter(playerID, outObj);

		dealOtherEffectAction(outObj->data.qwThisID, Cmd::ACTION_TYPE_ATTEND_IN);	    //����ϳ�
		dealHaloEffectAction();	    //�⻷���
		return true;
	    }
	}
    }
    return false;
}

/**
 * \brief �ٻ���ս����
 * \param   playerID ���ID
 *	    cardID ����ID
 *	    dwThisID �ٻ����Ķ�����ThisID
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
	if(CardToOneSlot(playerID, cardID, slot, outObj, "ADD ս���� �ٻ�"))
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
		dealOtherEffectAction(outObj->data.qwThisID, Cmd::ACTION_TYPE_ATTEND_IN);	    //����ϳ�
		dealHaloEffectAction();	    //�⻷���
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
	logToObj(o, "ADD ս���� ����");
	if(this->slots.addObject(o, true, slot))
	{//֪ͨ˫�����Ӣ�ۼ������
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

	    dealOtherEffectAction(o->data.qwThisID, Cmd::ACTION_TYPE_ATTEND_IN);	    //����ϳ�
	    dealHaloEffectAction();	    //�⻷���
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
    logToObj(o, "ADD ��ս��¼��");
    if(this->slots.addObject(o, true, slot))
    {//֪ͨ˫����Ҽ�¼�����
	Zebra::logger->debug("[����] PVP ս��:%u ������һ��:(%s)�� ��(%s(%u,%u))->��:(%s(%u,%u)) ��,ʣ��ռ�%u ��ɫ(%u)",
		gameID, o->base->name,
		"����", card->data.pos.x, card->data.pos.y,
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
 * \brief �����������
 * \param playerID ���ID
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
	logToObj(card, "DELETE ����");
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
	{//֪ͨ˫�����Ӣ�����
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
	    retstr += (name1+"��ս��");
	    break;
	case CardSlots::MAIN2_PACK:
	    retstr += (name2+"��ս��");
	    break;
	case CardSlots::HAND1_PACK:
	    retstr += (name1+"���Ʋ�");
	    break;
	case CardSlots::HAND2_PACK:
	    retstr += (name2+"���Ʋ�");
	    break;
	case CardSlots::EQUIP1_PACK:
	    retstr += (name1+"������");
	    break;
	case CardSlots::EQUIP2_PACK:
	    retstr += (name2+"������");
	    break;
	case CardSlots::SKILL1_PACK:
	    retstr += (name1+"���ܲ�");
	    break;
	case CardSlots::SKILL2_PACK:
	    retstr += (name2+"���ܲ�");
	    break;
	case CardSlots::HERO1_PACK:
	    retstr += (name1+"Ӣ�۲�");
	    break;
	case CardSlots::HERO2_PACK:
	    retstr += (name2+"Ӣ�۲�");
	    break;
	case CardSlots::TOMB_PACK:
	    retstr += "��ս��¼��";
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
	    retval = "���ж�ս";
	    break;
	case CHALLENGE_GAME_RANKING_TYPE:
	    retval = "������ս";
	    break;
	case CHALLENGE_GAME_COMPETITIVE_TYPE:
	    retval = "������ս";
	    break;
	case CHALLENGE_GAME_FRIEND_TYPE:
	    retval = "���Ѷ�ս";
	    break;
	case CHALLENGE_GAME_PRACTISE_TYPE:
	    retval = "��ϰģʽ";
	    break;
	case CHALLENGE_GAME_BOSS_TYPE:
	    retval = "BOSSģʽ";
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
	Zebra::logger->error("[����] ������(%s:%u)�Ļغ� ����ë��", user.name, user.id);
    }
    return false;
}

bool ChallengeGame::giveUpBattle(SceneUser &user)
{
    if(!isInGame(user.id))
    {
	return false;
    }
    Zebra::logger->debug("[����]��ս:%u ���(%s:%u)������", gameID, user.name, user.id);
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
	Zebra::logger->error("�������  ��������Ҫ��(%u) ��(%u)������ˮ�� ", srcobj->playerID, srcobj->data.mpcost);
	Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "����ͨ�����ʧ��  ��������Ҫ����%u������ˮ���ſ���",srcobj->data.mpcost);
	return false;
    }
    else
    {
	reduceMp(srcobj->playerID, srcobj->data.mpcost);
    }

    stObjectLocation org = srcobj->data.pos;
    Zebra::logger->info("[����]%s �����ƶ����� %s(%s,%d,%d,%d)->(%s,%u,%d,%d)",user.name,srcobj->base->name,
	    "����",org.loc(),org.xpos(),org.ypos(),
	    "����",dst.loc(),dst.xpos(),dst.ypos());
    
    if(dst.dwLocation == Cmd::CARDCELLTYPE_EQUIP && srcobj->isEquip()) //Ŀ��λ��������,�ݻپɵ�����
    {
	zCard *oldEquip = getSelfEquip(user.id);
	if(oldEquip)
	{
	    equipLeave(user.id, oldEquip);
	}
    }
    else if(dst.dwLocation == Cmd::CARDCELLTYPE_SKILL && srcobj->isHeroMagicCard())	//Ŀ��λ���Ǽ���,�ݻپɵļ���
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
    else if(dst.dwLocation == Cmd::CARDCELLTYPE_HERO && srcobj->isHero())	//Ŀ��λ����Ӣ��,�ݻپɵ�Ӣ��
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
    else if(dst.dwLocation == Cmd::CARDCELLTYPE_COMMON && srcobj->isAttend())	    //Ŀ��λ������ս��
    {
	if(user.id == players[0].playerID)
	{
	    if(this->slots.main1.space() == 0)
	    {
		Zebra::logger->error("PVPս�� ս����������� �ƶ�ʧ��");
		return false;
	    }
	}
	else if(user.id == players[1].playerID)
	{
	    if(this->slots.main2.space() == 0)
	    {
		Zebra::logger->error("PVPս�� ս����������� �ƶ�ʧ��");
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
	    sendBattleHistoryInfo(pMainHisID, vec);	    //�ƶ����Ƴɹ���ʷ
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
	Zebra::logger->info("[����]%s �ƶ����Ƴɹ� %s(%s,%d,%d,%d)->(%s,%u,%d,%d)",user.name,srcobj->base->name,
		"����",org.loc(),org.xpos(),org.ypos(),
		"����",dst.loc(),dst.xpos(),dst.ypos());
    }
    else
    {
	ret.success = 0;
	Zebra::logger->info("[����]%s �ƶ�����ʧ�� %s(%s,%d,%d,%d)->(%s,%u,%d,%d)",user.name,srcobj->base->name,
		"����",org.loc(),org.xpos(),org.ypos(),
		"����",dst.loc(),dst.xpos(),dst.ypos());
    }
    user.sendCmdToMe(&ret, sizeof(ret));
    return true;
}

bool ChallengeGame::startGame(SceneUser &user, BYTE change)
{
    if(getState() != CHALLENGE_STATE_PREPARE)	//ֻ��׼��״̬�ſ��Ի���
    {
	Zebra::logger->error("[����] ������:%u ״̬ �Ѿ����ܻ�����",getState());
	return false;
    }
    const DWORD countUp = 3;	//����3��
    const DWORD countLow = 4;	//����4��

    DWORD count = countLow;	//Ĭ�Ϻ���
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
	Zebra::logger->debug("��û�з��� ��");
    }
    else
    {
	Zebra::logger->debug("��û�з��� û��");
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
{//��ʱ������ʷ��¼��ʾ
#if 0
    zCard *pMainHis = this->slots.gcm.getObjectByThisID(mainID);
    zCard *pOtherHis = NULL;
    if(pMainHis)
    {
	BUFFER_CMD(Cmd::stRetBattleHistoryInfoUserCmd, history, zSocket::MAX_USERDATASIZE);	    //������ʷ
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
 * \brief ��ͻ���ˢ��ս������
 * \param pAttThisID ������
 *	  defThisIDVec �������б�
 *	  dwMagicType ����ID
 *	  type ��������(1,�ٻ�;0,����)
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
		Zebra::logger->info("[����] PVP ��ս���� ʤ���ѷ� ʤ��:(%s,%u) ����:(%s,%u)",winner->name, winner->id, loser->name, loser->id);
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
		Zebra::logger->info("[����] PVP ��ս���� ƽ���ճ� ʤ��:(%s,%u) ����:(%s,%u)",winner->name, winner->id, loser->name, loser->id);
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
 * \brief �������
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
 * \brief ���з��ĳ������
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
	Zebra::logger->error("������� ��Ҫ��(%u)�ֶ�ѡ��һ��Ŀ�� Ŀ���Ѿ���������", pAtt->playerID);
	return false;
    }
    if(pAtt->data.pos.loc() == Cmd::CARDCELLTYPE_RECORD || pDef->data.pos.loc() == Cmd::CARDCELLTYPE_RECORD)
    {
	Zebra::logger->error("��Ȼ����ʷ��¼�е����� ��(pAtt->playerID:%u) �����˰�~",pAtt->playerID);
	return false;
    }
    if(pAtt == pDef)
    {
	Zebra::logger->error("Ŀ����ô�ܺ͹�����һ�� ������%s() ������%s", pAtt->base->name, pDef->base->name);
	return false;
    }
    if(needTarget)
    {
	if(pDef->isSneak())
	{
	    Zebra::logger->error("[PK] Ŀ��ѡ�����(ʩ������ָ�����ε�λ) ������%s() ������%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "��ͨ����ʧ��  ��ͨ�������ܴ�Ǳ�е�λ");
	    return false;
	}
	if(rightTarget(pDef, pAtt, needTarget))
	{

	}
	else
	{
	    Zebra::logger->error("SKILL  ���Ŀ��ѡ�����");
	    return false;
	}
	return true;
    }
    else	//��ͨ����Ŀ����
    {
	if(pAtt->isFreeze())
	{
	    Zebra::logger->error("[PK] ��ͨ����ʧ��(�㴦�ڶ���״̬���ܽ��й���) ������%s() ������%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "��ͨ����ʧ��  ���Լ����ڶ������޷�����");
	    return false;
	}
	if(!pDef->isHero() && !pDef->isAttend())
	{
	    Zebra::logger->error("[PK] ��ͨ����ʧ��(ֻ�ܹ�����ӻ���Ӣ��) ������%s() ������%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "��ͨ����ʧ��  ���������Ķ���Ȳ���Ӣ��Ҳ�������");
	    return false;
	}
	if(!pDef->preAttackMe(pAtt))
	{
	    Zebra::logger->error("[PK] ��ͨ����ʧ��(preAttackMe ��֤ʧ��) ������%s() ������%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "��ͨ����ʧ��  ������֤ʧ��");
	    return false;
	}
	if(!pAtt->hasDamage())
	{
	    Zebra::logger->error("[PK] ��ͨ����ʧ��(��û�й�������) ������%s() ������%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "��ͨ����ʧ��  �㶼û�й����� ���ܷ��𹥻�");
	    return false;
	}
	if(pDef->isSneak())
	{
	    Zebra::logger->error("[PK] ��ͨ����ʧ��(��ͨ�������ܴ����ε�λ) ������%s() ������%s", pAtt->base->name, pDef->base->name);
	    //Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "��ͨ����ʧ��  ��ͨ�������ܴ�Ǳ�е�λ");
	    return false;
	}
	if(!pDef->isSneak())	//�����������������,������Ч
	{
	    if(checkEnemyTaunt(pAtt->playerID) && !pDef->hasTaunt())
	    {
		Zebra::logger->error("[PK] ��ͨ����ʧ��(��ù���һ���г������Ӳ���) ������%s() ������%s", pAtt->base->name, pDef->base->name);
		//Channel::sendSys(&user, Cmd::INFO_TYPE_GAME, "��ͨ����ʧ��  ��ù���һ�����г������Ӳ���");
		return false;
	    }
	}
	return true;
    }

    return false;
}

bool ChallengeGame::rightTarget(zCard* main, zCard* other, DWORD range)
{
    if(((range & Cmd::ATTACK_TARGET_EHERO) && /*�з�Ӣ��*/main->isHero() && other->isEnemy(main))
	    || ((range & Cmd::ATTACK_TARGET_EATTEND) && /*�з����*/main->isAttend() && other->isEnemy(main))
	    || ((range & Cmd::ATTACK_TARGET_SHERO) && /*����Ӣ��*/main->isHero() && !other->isEnemy(main))
	    || ((range & Cmd::ATTACK_TARGET_SATTEND) && /*�������*/main->isAttend() && !other->isEnemy(main))
	    || ((range & Cmd::ATTACK_TARGET_MYSELF) && /*����*/(main->data.qwThisID == other->data.qwThisID)))
    {
	return true;
    }
    return false;
}

/**
 * \brief �����Ƿ��п���Ŀ��
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
	    Zebra::logger->debug("[PK] װ������ Ӣ��(%s)�����仯 ��ǰ����:%u",hero->base->name, hero->data.damage);
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
	    Zebra::logger->debug("[PK] �����볡 Ӣ��(%s)�������仯 ��ǰ����:%u",hero->base->name, hero->data.damage);
	}
    }
    return false;
}

/**
 * \brief �����˺����Ӽӳ�
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
    Zebra::logger->debug("[PK] %u �˺������ܼӳ�:%u",playerID, dam);
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
		    if(object->data.pos.loc() == Cmd::CARDCELLTYPE_HAND)    //�Է�����
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
 * \brief ����ʹ�÷����ƴ���Ч��
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
 * \brief ����غϿ�ʼ����Ч��
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
	action( "�غϿ�ʼ");		    //�غϿ�ʼ�͵з��غϿ�ʼʱ����
	endOneFlow();
    }
}

/**
 * \brief ����غϽ�������Ч��
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
	action( "�غϽ���");		    //�غϽ����͵з��غϽ���ʱ����
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
	actionList.push_front(in);	    //�ӵ�ǰ��
	action( "�鵽ĳ����");		    //�鵽��ʱ����
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
	    actionList.push_front(in);	    //�ӵ�ǰ��
	    flag = true;
	}
    }
    if(flag)
    {
	action( "����Ч��");		    //����ʱ����
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
    action( "ִ��");		   
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
 * \brief   ��mainThisID��ĳЧ��Ӱ���,
 *	    ��鳡��������ɫ�Ƿ���Ҫ����Ч��
 * \param
 * \return
*/
void ChallengeGame::dealOtherEffectAction(DWORD mainThisID, DWORD type)
{   
    zCard *main = this->slots.gcm.getObjectByThisID(mainThisID);	//����Ӱ��Ķ���
    if(!main)
    {
	return;
    }

    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_EHERO + Cmd::ATTACK_TARGET_EATTEND+ Cmd::ATTACK_TARGET_SEQUIP + Cmd::ATTACK_TARGET_EEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);
    if(thisID_Vec.empty())	    
	return;
    for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end(); )	//�ų�û��Ч���Ķ���
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
    for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end(); )	//�ų����������÷�Χ�Ķ���
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
    for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end(); it++)		//ȡ������condition��
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
		action( "��ӽ���");
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
 * \brief ��������Ч��
 * \param
 * \return
*/
void ChallengeGame::dealAttackEndAction(DWORD dwAttThisID, DWORD dwDefThisID, bool defHurt)
{
    zCard* main = this->slots.gcm.getObjectByThisID(dwAttThisID);
    if(main && main->pk.attackEndCondition && main->pk.attackEndID)
    {
	bool condition = false;
	if(main->pk.attackEndCondition == 1)	//������Ҫ�����ж�
	{
	    if(defHurt)
		condition = true;
	}
	else if(main->pk.attackEndCondition == 10000)	//����
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
 * \brief ����⻷Ч��
 * \param
 * \return
*/
void ChallengeGame::dealHaloEffectAction()
{
    std::vector<DWORD> thisID_Vec;
    DWORD range = Cmd::ATTACK_TARGET_SHERO + Cmd::ATTACK_TARGET_SATTEND + Cmd::ATTACK_TARGET_SEQUIP
	+ Cmd::ATTACK_TARGET_EHERO + Cmd::ATTACK_TARGET_EATTEND + Cmd::ATTACK_TARGET_EEQUIP;
    collectTarget(privilegeUser, range, thisID_Vec);

    for(std::vector<DWORD>::iterator it=thisID_Vec.begin(); it!=thisID_Vec.end(); )	//ȥ���ǹ⻷��ID
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
	if(main && checkHaloOpen(main->pk.halo_Ctype, main->pk.halo_Cid, main))	//����
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
	else	//����
	{
	    clearAllHalo(*it);
	}
    }
    actionHalo("�⻷");

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
	if(p)	    //����
	{//���p���ϵ����зǷ��⻷
	    if(p->haloInfoMap.empty())
		continue;
	    for(std::map<DWORD, t_haloInfo>::iterator it = p->haloInfoMap.begin(); it != p->haloInfoMap.end(); )
	    {
		zCard *main = this->slots.gcm.getObjectByThisID(it->first);
		if(main && checkHaloOpen(main->pk.halo_Ctype, main->pk.halo_Cid, main))   //����
		{
		    ++it;
		}
		else
		{
		    p->clearOneHaloInfo(it->first);	//���
		    p->haloInfoMap.erase(it++);	//ɾ��
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
 * \brief   ��������״̬�л�(��\��)
 * \param
 * \return
*/
void ChallengeGame::dealEquipState()
{
    zCard *equip = getSelfEquip(privilegeUser);
    if(equip)	
    {
	//����
	zCard *hero = getSelfHero(privilegeUser);
	if(hero)
	{
	    Zebra::logger->debug("[PK]�غϿ�ʼ Ӣ������ %s ����", equip->base->name);
	    hero->data.equipOpen = 1;
	    sendCardInfo(hero);
	}
    }

    equip = NULL;
    equip = getEnemyEquip(privilegeUser);
    if(equip)
    {
	//�ر�
	zCard *hero = getEnemyHero(privilegeUser);
	if(hero && hero->checkAttackTimes())
	{
	    Zebra::logger->debug("[PK]�غϿ�ʼ Ӣ������ %s �ر�", equip->base->name);
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
 * \brief ���ÿɹ�������
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
 * \brief ˢ�����п���״̬���ͻ���
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
 * \brief ���ö���״̬
 * \param
 * \return
*/
void ChallengeGame::dealCheckFreezeState()
{
    CheckFreezeState at(this);
    this->slots.gcm.execEvery(at);
}

/**
 * \brief ����Լ�Ӣ��
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
 * \brief ��õз�Ӣ��
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
 * \brief ����Լ�����
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
 * \brief ��õз�����
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
 * \brief ����Լ�hero����
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
 * \brief ��õз�hero����
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
 * \param desc:����
 * \param flag:true,�ֶ��ͷŵķ���
 *	       false,���������ķ���
 * \return
*/
bool ChallengeGame::action(char* desc, bool flag)
{
    if(actionList.empty())
    {
	return false;
    }
    Zebra::logger->debug("ChallengeGame::action ִ�б��С:%u", actionList.size());
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
    Zebra::logger->debug("[����action] %s �ͷ�:%u",desc, dwMagicType);
    std::vector<SkillStatus>::const_iterator iter;
    for(iter = skillbase->skillStatus.begin(); iter!=skillbase->skillStatus.end(); iter++)
    {
	SkillStatus *pSkillStatus = (SkillStatus *)&*iter;
	doOperation(pSkillStatus, pEntry, &in, flag?flag:flag2);
    }
    dealHurtAction();
    dealCureAction();

    if(skillbase->conditionType && skillbase->conditionID)  //����ĺ���Ч��
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
	action( "ִ��");		    //ִ��Ч��
    }

    //����������
    dealDeadAction();
    return true;
}

/**
 * \brief
 * \param entry:�����ͷ���
 * \param desc:����
 * \param flag:true,�ֶ��ͷŵķ���
 *	       false,���������ķ���
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
    WORD useHand = pSkillStatus->useHand;   //�Ƿ�ʹ����ѡĿ��
    
    std::vector<DWORD> defList;
    if(wdAttack == 1)		//����
    {
	zCard* pDef = this->slots.gcm.getObjectByThisID(rev->dwDefThisID);
	if(flag && useHand)	//�ֶ��ͷŵ�
	{
	    if(!pDef)
	    {
		Zebra::logger->error("��������%uѡ���Ŀ�겻����",rev->dwMagicType);
		return;
	    }
	}
	else	//�����Ļ��߲�����ѡĿ���
	{
	    if(range & Cmd::ATTACK_TARGET_SHERO)	//�Լ�Ӣ��
	    {
		pDef = getSelfHero(playerID);
	    }
	    else if(range & Cmd::ATTACK_TARGET_EHERO)	//�з�Ӣ��
	    {
		pDef = getEnemyHero(playerID);
	    }
	    else if(range & Cmd::ATTACK_TARGET_MYSELF)	//ʩ��������
	    {
		pDef = entry;
	    }
	    else if(range & Cmd::ATTACK_TARGET_EEQUIP)	//�з�����
	    {
		pDef = getEnemyEquip(playerID);
	    }
	    else if(range & Cmd::ATTACK_TARGET_SEQUIP)	//��������
	    {
		pDef = getSelfEquip(playerID);
	    }
	}
	if(!pDef)
	    return;

	std::vector<DWORD> thisID_Vec;
	if(pDef->isAttend() && (pSkillStatus->range & Cmd::ATTACK_TARGET_LEFT_RIGHT))	    //����
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
    else if(wdAttack == 2)	//Ⱥ��
    {
	std::vector<DWORD> thisID_Vec;
	collectTarget(playerID, range, thisID_Vec, condition);
	if(0 == mode)	//����������
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
    else if(wdAttack == 3)  //���Ŀ��:���num��Ŀ��
    {
	for(DWORD i=0; i<num; i++)
	{
	    std::vector<DWORD> thisID_Vec;
	    collectTarget(playerID, range, thisID_Vec, condition);
	    if(0 == mode)	//����������
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

	    if(thisID_Vec.empty())	    //������б�Ϊ��
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
    else if(wdAttack == 4)  //���Ŀ��:���num��Ŀ��
    {
	std::vector<DWORD> thisID_Vec;
	collectTarget(playerID, range, thisID_Vec, condition);
	if(0 == mode)	//����������
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
	if(thisID_Vec.empty())	    //������б�Ϊ��
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
 * \brief   �ռ�����Ŀ��
 * \param flag ��Χ
 * \param condition ����
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

    if(condition)		//����ɸѡ
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
    
    std::sort(birthInfoVec.begin(), birthInfoVec.end(), lessmark);	//����
    for(std::vector<birthInfo>::iterator it=birthInfoVec.begin(); it!=birthInfoVec.end(); it++)
    {
	targets.push_back(it->dwThisID);
    }

    if(targets.empty())	    //Ŀ���б�Ϊ��
	return false;
    return true;
}

/**
 * \brief   ��ȡȫ����targets�����Ŀ��
 * \param targets:��Ҫ�ų���Ŀ��
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
    //���뱣֤nottargets�㹻��,���򽫹ҵ�
    it = std::set_difference(thisID_Vec.begin(), thisID_Vec.end(), targets.begin(), targets.end(), nottargets.begin());
    nottargets.resize(it-nottargets.begin());

    if(nottargets.empty())
	return false;
    return true;
}

/**
 * \brief
 * \param entry:�⻷�ͷ���
 * \param desc:����
 * \return
*/
bool ChallengeGame::actionHalo(char* desc)
{
    if(actionHaloList.empty())
    {
	return false;
    }
    Zebra::logger->debug("ChallengeGame::actionHalo ִ�б��С:%u", actionHaloList.size());
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
    Zebra::logger->debug("[�⻷action] %s �ͷ�:%u",desc, dwMagicType);
    std::vector<SkillStatus>::const_iterator iter;
    for(iter = skillbase->skillStatus.begin(); iter!=skillbase->skillStatus.end(); iter++)
    {
	SkillStatus *pSkillStatus = (SkillStatus *)&*iter;
	doOperation(pSkillStatus, pEntry, &in, false);
    }

    while(!actionHaloList.empty())
    {
	actionHalo("ִ�й⻷");		    //ִ��Ч��
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
	case 10000:	//������Ϊ��
	    return true;
	    break;
	default:
	    break;
    }
    return false;
}

/**
 * \brief �ж�entry�Ĺ⻷�Ƿ���
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
	case 10000:	//������Ϊ��
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
	{//�������Щ���ϵ�mainThisID�⻷
	    zCard *pDef = this->slots.gcm.getObjectByThisID(*it);
	    if(pDef)
		pDef->removeOneHaloInfo(mainThisID); 
	}
    }
    return true;
}

/**
 * \brief �����mainThisID�ͷŵ�����Ч��
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
	    pDef->removeOneHaloInfo(mainThisID);	//ɾ��mainThisID��pDef�Ĺ⻷
	}
    }
    return true;
}
