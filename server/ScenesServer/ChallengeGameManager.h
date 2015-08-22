/*************************************************************************
 Author: wang
 Created Time: 2014��12��11�� ������ 10ʱ13��45��
 File Name: ScenesServer/ChallengeGameManager.h
 Description: 
 ************************************************************************/
#ifndef _ChallengeGameManager_h_
#define _ChallengeGameManager_h_
#include "zSingleton.h"
#include "zType.h"
#include "zTime.h"
#include "SessionCommand.h"
#include "HeroCardCmd.h"
#include <map>

enum ChallengeState
{
    CHALLENGE_STATE_NONE	= 0,	//��Ϸ�ոմ���(��δ���˫������)
    CHALLENGE_STATE_PREPARE	= 1,	//׼���׶�(��ʱ�����滻��һ�ѵ�����)
    CHALLENGE_STATE_BATTLE	= 2,	//ս����
    CHALLENGE_STATE_END		= 3,	//��Ϸ����
    CHALLENGE_STATE_CANCLEAR	= 4,	//����ж��״̬
};

class ChallengeGame;
class SceneUser;
class zCard;

class ChallengeGameManager : public Singleton<ChallengeGameManager>
{
    friend class SingletonFactory<ChallengeGameManager>;
    public:
    ~ChallengeGameManager();
    ChallengeGameManager();
    void clean();
    void userOffline();
    bool doUserCmd();
    bool doSceneCmd();
    void timer();
    bool hasUnfinishedGame(SceneUser *pUser);
    bool isFighting(SceneUser *pUser);
    zCard* getUserCardByThisID(DWORD gameID, DWORD thisID);
    bool getUserHero(DWORD gameID, DWORD userID, zCard *ret);
    ChallengeGame* getGameByID(DWORD gameID);

    typedef std::map<DWORD, ChallengeGame *> GAMEMAP;
    typedef std::map<DWORD, ChallengeGame *>::iterator GAMEMAP_ITER;
    GAMEMAP gameMap;

    DWORD getGameCount()
    {
	return gameCount;
    }
    void increaseGameCount()
    {
	gameCount++;
    }
    void decreaseGameCount()
    {
	if(gameCount > 0)
	    gameCount--;
	else
	    gameCount = 0;
    }

    DWORD createGame(Cmd::Session::t_CreateNewPkGame_SceneSession *cmd);
    bool initGameData(SceneUser *pUser);
    bool handlesecondEnter(SceneUser *pUser);
    bool handleStartGame(SceneUser &user, BYTE change);
    bool handleEndOneRound(SceneUser &user);
    bool handleGiveUpBattle(SceneUser &user);
    bool handleCardAttackMagic(SceneUser &user, const Cmd::stCardAttackMagicUserCmd *rev);
    bool handleCardMoveAndAttackMagic(SceneUser &user, const Cmd::stCardMoveAndAttackMagicUserCmd *rev);
    private:
    DWORD gameCount;
    Timer _ten_sec;	    //10��Ѹ���������ս�����������Ự
};

class ChallengeTmpData
{
    public:
	ChallengeTmpData()
	{
	    groupID = 0;
	    cardsIndex = 0;
	    gameType = 0;
	}
	DWORD groupID;		//��ս��Ψһ���
	DWORD cardsIndex;	//��սʹ�õ���������
	BYTE gameType;		//��ս����

	bool clear()
	{
	    groupID = 0;
	    cardsIndex = 0;
	    gameType = 0;
	    return true;
	}

	unsigned int saveChallengeData(unsigned char* dest);
	unsigned int loadChallengeData(unsigned char* src);
};
#endif
