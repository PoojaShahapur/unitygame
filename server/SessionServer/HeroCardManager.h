/*************************************************************************
 Author: wang
 Created Time: 2014��10��22�� ������ 11ʱ07��00��
 File Name: SessionServer/HeroCardManager.h
 Description: 
 ************************************************************************/
#include "zSingleton.h"
#include "zType.h"
#include "zUniqueID.h"
#include "SessionCommand.h"
#include <map>
#include <queue>
#include <list>

#define BATTLE_USER_NUM 2   //��ս����

class UserSession;
class SceneSession;

struct stChallengeInfo
{
    DWORD dwCharID;		//��ɫID
    char name[MAX_NAMESIZE+1];
    DWORD matchScore;		//ս��
    DWORD cardsNumber;		//���ƺ�
    DWORD reqMatchTime;		//����ƥ���ʱ���
    DWORD matchTimes;		//�ۼ�ƥ�����
    WORD occupation;		//ְҵ
    BYTE matched;		//�Ƿ���ƥ��
    stChallengeInfo()
    {
	bzero(this, sizeof(*this));
    }

    stChallengeInfo &operator= (const stChallengeInfo& other)
    {
	bcopy(&other, this, sizeof(*this));
	return *this;
    }

    bool operator == (const stChallengeInfo &other) const
    {
	return dwCharID == other.dwCharID;
    }

    bool operator < (const stChallengeInfo &other) const
    {
	return (this->matchScore < other.matchScore);
    }
#if 0
    bool operator > (const stChallengeInfo &other) const
    {
	return (this->matchScore > other.matchScore);
    }
#endif
}__attribute__((packed));

struct stMatchedInfo
{
    WORD rank;
    BYTE hasSend;		//��������
    DWORD groupID;		//Ψһ������
    stChallengeInfo info[BATTLE_USER_NUM];	    //��ս˫������Ϣ
    DWORD matchTime;		//ƥ��ɹ���ʱ���
    BYTE canClear;		//�Ƿ����������Ԫ��(1,��;0,��)
    stMatchedInfo()
    {
	bzero(this, sizeof(*this));
    }
};

class HeroCardManager : public Singleton<HeroCardManager>
{
    friend class SingletonFactory<HeroCardManager>;
    public:
    HeroCardManager();
    ~HeroCardManager();
    public:
    bool putGameIDBack(BYTE type, DWORD gameID);
    bool processMessage(Cmd::Session::t_ReqFightMatch_SceneSession *rev);
    void addUser(DWORD userID, BYTE type, DWORD score, DWORD cardsNumber);
    void cancelUser(DWORD userID, BYTE type);
    void timer();
    void doRelaxGroup();	    //����ģʽƥ��
    void doRankingGroup();	    //����ģʽƥ��
    void doCompetitiveGroup();	    //����ģʽƥ��
    typedef std::list<stChallengeInfo> WaitForMatch;   //��ƥ���	��ս��Ϣ�б�
    typedef std::list<stMatchedInfo> MatchedList;		//�Ѿ�ƥ��õ�list 


    void checkMatchedList();

    zUniqueID<DWORD>	*relax_chanllengeID;		//PVP ���ж�ս
    WaitForMatch relaxBufferList;			//PVP ����
    WaitForMatch relaxForMatchList;			//PVP ����
    MatchedList relaxList;				//PVP ����
    //һ����������ս��Ĵ������ 
    //step1 ���� relaxBufferList
    //step2 ÿ5����һ�� relaxBufferList�������ս������ƥ���뵱ǰ����5������ goto step3
    //step3 ÿ5��� relaxBufferList ������ȫ������ relaxForMatchList ��������ս������ goto step4
    //step4 ���������� relaxForMatchList�������ڵ��������������ԣ������� relaxList�� goto step5
    //step5 ���� relaxList ����Ϣ���͸����

    zUniqueID<DWORD>	*ranking_chanllengeID;		//PVP ������ս
    zUniqueID<DWORD>	*competitive_chanllengeID;	//PVP ������ս
    zUniqueID<DWORD>	*friend_chanllengeID;		//PVP ���Ѷ�ս
    zUniqueID<DWORD>	*practise_chanllengeID;		//PVE ��ͨ��ϰ
    zUniqueID<DWORD>	*boss_chanllengeID;		//PVE BOSSģʽ


    std::map<WORD, DWORD>   allGameCount;	//����ID---��ս����
    void updateSessionGameCount(WORD country, DWORD count);
    WORD getMinCountCountry();
    std::string getCreateGameMapName();
    bool putTwoUserToBattleScene(UserSession *pUser1, UserSession *pUser2, char* mapName);
    private:
    bool checkChanllengeInfo(WaitForMatch list, stChallengeInfo info);
    DWORD randomOnePVPSceneID();
    bool matchPVPSuccess(SceneSession *scene, UserSession *pUser1, UserSession *pUser2, 
	    DWORD groupID, DWORD cardsNumber1, DWORD cardsNumber2, DWORD type, DWORD sceneNumber, char* mapName);
    void doApplyPractice(DWORD userID, DWORD bossID, DWORD cardsNumber);
    bool applyPVESuccess(SceneSession *scene, UserSession *pUser1, DWORD bossID, 
	    DWORD groupID, DWORD cardsNumber1, DWORD cardsNumber2, DWORD type, DWORD sceneNumber, char* mapName);
};
