/*************************************************************************
 Author: wang
 Created Time: 2014年12月11日 星期四 10时38分26秒
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
	DWORD gameID;			    //对战唯一ID
	Cmd::Session::ChallengeGameType gameType;	//对战类型
	ChallengeState state;				//对战状态
	CardSlots slots;		    //战场卡槽管理
	ChallengePlayer players[maxPlayer];	    //玩家动态数据
    private:
	DWORD _sceneNumber;
	DWORD createTime;		    //对战创建时间戳	
	DWORD challengePrepareTime;	    //准备时间(从游戏数据初始化到双方替换完毕第一把牌)
	DWORD challengeLastTime;	    //对战持续时间(从第一个回合开始计时的总秒数)
	DWORD notOnlineTime;		    //双方不在线时长
	DWORD upperHand;	//记录先手的玩家ID
	DWORD privilegeUser;	//有权限的一方
	DWORD totalRoundCount;		    //总回合数计数 
	DWORD currRoundStartTime;	    //本回合开始时间
	DWORD currRoundEndTime;		    //本回合结束时间
	//############################来自配置文件的数据##################################
	DWORD cfg_prepareTime;	    //一局战斗的准备时间限制
	DWORD cfg_roundTime;	    //一个回合的时间限制

    public:
	void logToObj(zCard* o, char *desc);
	void logScene(zCard* o, int slot);
	char* getPlayerName(DWORD playerID);
	bool initCardsLib(SceneUser* pUser, std::vector<DWORD> &libVec);	    //初始化牌库
	bool initBaseBattleInfo();	
	void sendFirstHandCard(DWORD upper);
	void sendEnemyBaseInfo();
	void sendLibNumInfo();
	void sendMpInfo();
	void sendCardInfo(zCard* o);
	void refreshCardInfo(zCard* o);
	void sendOutCardInfo(const DWORD cardID);
	/**
	 * \brief 向客户端刷新战斗数据
	 * \param pAttThisID 攻击者
	 *	  defThisIDVec 防御者列表
	 *	  dwMagicType 技能ID
	 *	  type 数据类型(1,召唤;0,其他)
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
	 * \brief 随机丢弃手牌
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

	DWORD generateUpperHand();	//产生先手
	
	void setPrivilege(DWORD playerID);
	bool isHavePrivilege(DWORD playerID);

	bool on_RoundStart();	//回合开始
	bool on_RoundEnd();	//回合结束
	bool on_GameOver();	//游戏结束
	
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
	void dealUseMagicCardAction();		//使用法术牌时触发效果
	void dealRoundStartAction();		//回合开始效果
	void dealRoundEndAction();		//回合结束效果
	void dealDrawCardSuccessAction(DWORD thisID);	//抽排成功效果
	void dealDeadAction();			//亡语效果
	void dealHurtAction();						//角色受伤
	void dealCureAction();						//角色受治疗
	void dealOtherEffectAction(DWORD mainThisID, DWORD type);			//其他角色受影响
	void dealAttackEndAction(DWORD dwAttThisID, DWORD dwDefThisID, bool hurt);			//攻击结束
	void dealHaloEffectAction();					//光环效果处理
	void clearIllegalHalo();					//清除非法的光环
	bool getLeftRightID(std::vector<DWORD> &vec, DWORD mainThisID);
	void dealEquipState();				    //处理武器状态切换(开\关)
	void startOneFlow();
	void endOneFlow();
	void clearTempEffectUnit(std::vector<t_EffectUnit> &vec);
	//############################来自用户的主动处理#################################
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
	//#############################我的回合一些临时数据#################################
	std::list<Cmd::stCardAttackMagicUserCmd>  actionList;	    //一个操作或者一个触发的所有效果列表
	std::vector<DWORD> hurtList;				    //一个技能ID处理后处理全场受伤的角色
	std::vector<DWORD> cureList;				    //一个技能ID处理后处理全场受治疗的角色
	std::list<Cmd::stCardAttackMagicUserCmd>  actionHaloList;	    //一个操作或者一个触发引起的所有光环触发的所有效果列表
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
