/*************************************************************************
 Author: wang
 Created Time: 2014��12��11�� ������ 10ʱ38��26��
 File Name: ScenesServer/ChallengeGame.h
 Description: 
 ************************************************************************/
#ifndef _ChallengeGame_h_
#define _ChallengeGame_h_
#include "ChallengeGameManager.h"
#include "SessionCommand.h"
#include "zCard.h"
#include "ChallengePlayer.h"
#include <list>

const DWORD maxPlayer = 2;

class SceneUser;



class ChallengeGame
{
    public:
	ChallengeGame(const DWORD groupID, const Cmd::Session::ChallengeGameType type, const DWORD playerID1, const DWORD cardsNumber1, const DWORD playerID2, const DWORD cardsNumber2, const DWORD sceneNumber);
	~ChallengeGame();
	void clean();

	bool init(SceneUser *pUser);
	void setState(ChallengeState);
	ChallengeState getState() const;

	void setGameType(Cmd::Session::ChallengeGameType);
	Cmd::Session::ChallengeGameType getGameType() const;
	
	bool isPVP();
	bool isPVE();

	void timer();
	void timer_BATTLE();

	DWORD getGameID()
	{
	    return gameID;
	}
	DWORD gameID;			    //��սΨһID
	Cmd::Session::ChallengeGameType gameType;	//��ս����
	ChallengeState state;				//��ս״̬
	CardSlots slots;		    //ս�����۹���
	ChallengePlayer players[maxPlayer];	    //��Ҷ�̬����
    private:
	DWORD _sceneNumber;
	DWORD createTime;		    //��ս����ʱ���	
	DWORD challengePrepareTime;	    //׼��ʱ��(����Ϸ���ݳ�ʼ����˫���滻��ϵ�һ����)
	DWORD challengeLastTime;	    //��ս����ʱ��(�ӵ�һ���غϿ�ʼ��ʱ��������)
	DWORD notOnlineTime;		    //˫��������ʱ��
	DWORD upperHand;	//��¼���ֵ����ID
	DWORD privilegeUser;	//��Ȩ�޵�һ��
	DWORD totalRoundCount;		    //�ܻغ������� 
	DWORD currRoundStartTime;	    //���غϿ�ʼʱ��
	DWORD currRoundEndTime;		    //���غϽ���ʱ��
	//############################���������ļ�������##################################
	DWORD cfg_prepareTime;	    //һ��ս����׼��ʱ������
	DWORD cfg_roundTime;	    //һ���غϵ�ʱ������

    public:
	void logToObj(zCard* o, char *desc);
	void logScene(zCard* o, int slot);
	char* getPlayerName(DWORD playerID);
	bool initCardsLib(SceneUser* pUser, std::vector<DWORD> &libVec);	    //��ʼ���ƿ�
	bool initBaseBattleInfo();	
	void sendFirstHandCard(DWORD upper);
	void sendEnemyBaseInfo();
	void sendLibNumInfo();
	void sendMpInfo();
	void sendCardInfo(zCard* o);
	void refreshCardInfo(zCard* o);
	void sendOutCardInfo(const DWORD cardID);
	/**
	 * \brief ��ͻ���ˢ��ս������
	 * \param pAttThisID ������
	 *	  defThisIDVec �������б�
	 *	  dwMagicType ����ID
	 *	  type ��������(1,�ٻ�;0,����)
	 *
	 * \return
	 */
	void refreshClient(DWORD pAttThisID, std::vector<DWORD> defThisIDVec, DWORD dwMagicType, BYTE type=0);
	void sendHandFullInfo(DWORD playerID, DWORD id);
	void sendEnemyHandNum(SceneUser *pUser);
	void sendBattleHistoryInfo(DWORD mainID, std::vector<DWORD> otherIDVec);
	void sendCardStateInfo(zCard* o);
	bool drawOneCard(DWORD playerID, std::vector<DWORD> &lib, const DWORD dwSkillID=0);
	void sendAllCardList(SceneUser *pUser);

	std::string slotType2Name(DWORD slot);
	char* gameType2Name(DWORD gameType);

	bool addCardToLib(const DWORD playerID, const DWORD cardID);
	bool CardToHandSlot(const DWORD playerID, const DWORD cardID, const bool first=false, const DWORD dwSkillID=0);
	bool CardToHeroSlot(const DWORD playerID, const DWORD cardID);
	bool CardToSkillSlot(const DWORD playerID, const DWORD cardID);
	bool CardToEquipSlot(const DWORD playerID, const DWORD cardID);
	bool CardToCommonSlot(const DWORD playerID, const DWORD cardID, const DWORD dwSkillID);
	bool copyCardToCommonSlot(const DWORD playerID, zCard* card, const DWORD dwSkillID);
	bool CardToRecordSlot(const DWORD playerID, zCard* card, DWORD timeSequence, DWORD &dwThisID);
	/**
	 * \brief �����������
	 */
	bool dropOneRandomCardFromHandSlot(const DWORD playerID);
	bool CardToOneSlot(const DWORD playerID, const DWORD cardID, int slot, zCard*& object, char* desc);
	
	DWORD getChallengeLastTime()
	{
	    return challengeLastTime;
	}
	DWORD getTotalRoundCount()
	{
	    return totalRoundCount;
	}

	DWORD generateUpperHand();	//��������
	
	void setPrivilege(DWORD playerID);
	bool isHavePrivilege(DWORD playerID);

	bool on_RoundStart();	//�غϿ�ʼ
	bool on_RoundEnd();	//�غϽ���
	bool on_GameOver();	//��Ϸ����
	
	bool dealGameResult(const DWORD loserID, bool isPeace=false);
	
	bool wakeUpAllAttend();
	bool checkEnemyTaunt(DWORD playerID);
	bool checkSelectedTarget(zCard* pAtt, const DWORD needTarget, const DWORD dwDefThisID);
	bool checkExtraCondition(const DWORD type, const DWORD conditionID, const DWORD dwDefThisID, zCard* entry);
	bool checkHaloOpen(const DWORD type, const DWORD conditionID, zCard* entry);
	bool checkExist(const DWORD conditionID, zCard* entry);
	bool checkHasState(const DWORD conditionID, const DWORD dwDefThisID);
	bool checkIsSomeOne(const DWORD conditionID, const DWORD dwDefThisID, zCard* entry);
	bool checkHaloClear(const DWORD mainThisID, std::vector<DWORD> targets);
	bool clearAllHalo(const DWORD mainThisID); 
	bool canFindTarget(zCard* pAtt, const DWORD needTarget);
	bool rightTarget(zCard* main, zCard* other, DWORD range);
	bool equipEnter(DWORD playerID, zCard* equip);
	bool equipLeave(DWORD playerID, zCard* equip);
	DWORD recalMagicAddDam(DWORD playerID);
	void dealResetGameCardAttackTimes();
	void dealRefreshCardState();
	void dealCheckFreezeState();
	void dealUseMagicCardAction();		//ʹ�÷�����ʱ����Ч��
	void dealRoundStartAction();		//�غϿ�ʼЧ��
	void dealRoundEndAction();		//�غϽ���Ч��
	void dealDrawCardSuccessAction(DWORD thisID);	//���ųɹ�Ч��
	void dealDeadAction();			//����Ч��
	void dealHurtAction();						//��ɫ����
	void dealCureAction();						//��ɫ������
	void dealOtherEffectAction(DWORD mainThisID, DWORD type);			//������ɫ��Ӱ��
	void dealAttackEndAction(DWORD dwAttThisID, DWORD dwDefThisID, bool hurt);			//��������
	void dealHaloEffectAction();					//�⻷Ч������
	void clearIllegalHalo();					//����Ƿ��Ĺ⻷
	bool getLeftRightID(std::vector<DWORD> &vec, DWORD mainThisID);
	void dealEquipState();				    //��������״̬�л�(��\��)
	void startOneFlow();
	void endOneFlow();
	void clearTempEffectUnit(std::vector<t_EffectUnit> &vec);
	//############################�����û�����������#################################
	bool endUserRound(SceneUser &user);
	bool giveUpBattle(SceneUser &user);
	bool moveUserCard(SceneUser &user, DWORD thisID, stObjectLocation dst);
	bool startGame(SceneUser &user, BYTE change);
	bool secondEnter(SceneUser *pUser);
	bool cardAttackMagic(SceneUser &user, const Cmd::stCardAttackMagicUserCmd *rev);
	bool cardNormalAttack(SceneUser &user, const Cmd::stCardAttackMagicUserCmd *rev);
	bool cardSkillAttack(SceneUser &user, const Cmd::stCardAttackMagicUserCmd *rev);
	bool cardMoveAndAttack(SceneUser &user, const Cmd::stCardMoveAndAttackMagicUserCmd *rev);

    public:
	bool checkMp(DWORD playerID, DWORD needMp);
	bool reduceMp(DWORD playerID, DWORD needMp);
	bool isInGame(DWORD playerID);
	zCard* getSelfHero(DWORD playerID);
	zCard* getEnemyHero(DWORD playerID);
	zCard* getSelfEquip(DWORD playerID);
	zCard* getEnemyEquip(DWORD playerID);
	zCard* getSelfSkill(DWORD playerID);
	zCard* getEnemySkill(DWORD playerID);
	SceneUser* getOther(DWORD playerID);

	bool action(char* desc, bool flag=false);
	void doOperation(const SkillStatus *pSkillStatus, zCard* entry, const Cmd::stCardAttackMagicUserCmd *rev, bool flag);
	bool collectTarget(DWORD playerID, const DWORD flag, std::vector<DWORD> &targets, const DWORD condition=0);
	bool collectNotTarget(DWORD playerID, std::vector<DWORD> targets, std::vector<DWORD> &nottargets);
	bool actionHalo(char* desc);
    private:
	//#############################�ҵĻغ�һЩ��ʱ����#################################
	std::list<Cmd::stCardAttackMagicUserCmd>  actionList;	    //һ����������һ������������Ч���б�
	std::vector<DWORD> hurtList;				    //һ������ID�������ȫ�����˵Ľ�ɫ
	std::vector<DWORD> cureList;				    //һ������ID�������ȫ�������ƵĽ�ɫ
	std::list<Cmd::stCardAttackMagicUserCmd>  actionHaloList;	    //һ����������һ��������������й⻷����������Ч���б�
    public:
	void addHurtList(DWORD dwThisID)
	{
	    hurtList.push_back(dwThisID);
	}
	void addCureList(DWORD dwThisID)
	{
	    cureList.push_back(dwThisID);
	}
	void addActionList(Cmd::stCardAttackMagicUserCmd cmd)
	{
	    actionList.push_back(cmd);
	}

};

#endif
