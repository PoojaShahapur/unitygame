/*************************************************************************
 Author: wang
 Created Time: 2014年10月22日 星期三 11时07分00秒
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

#define BATTLE_USER_NUM 2   //对战人数

class UserSession;
class SceneSession;

struct stChallengeInfo
{
    DWORD dwCharID;		//角色ID
    char name[MAX_NAMESIZE+1];
    DWORD matchScore;		//战力
    DWORD cardsNumber;		//套牌号
    DWORD reqMatchTime;		//请求匹配的时间戳
    DWORD matchTimes;		//累计匹配次数
    WORD occupation;		//职业
    BYTE matched;		//是否已匹配
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
    BYTE hasSend;		//发送拉人
    DWORD groupID;		//唯一分组编号
    stChallengeInfo info[BATTLE_USER_NUM];	    //对战双方的信息
    DWORD matchTime;		//匹配成功的时间戳
    BYTE canClear;		//是否可以清除这个元素(1,是;0,否)
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
    void doRelaxGroup();	    //休闲模式匹配
    void doRankingGroup();	    //排名模式匹配
    void doCompetitiveGroup();	    //竞技模式匹配
    typedef std::list<stChallengeInfo> WaitForMatch;   //待匹配的	对战信息列表
    typedef std::list<stMatchedInfo> MatchedList;		//已经匹配好的list 


    void checkMatchedList();

    zUniqueID<DWORD>	*relax_chanllengeID;		//PVP 休闲对战
    WaitForMatch relaxBufferList;			//PVP 休闲
    WaitForMatch relaxForMatchList;			//PVP 休闲
    MatchedList relaxList;				//PVP 休闲
    //一个玩家请求对战后的处理过程 
    //step1 加入 relaxBufferList
    //step2 每5秒检测一次 relaxBufferList，如果挑战者请求匹配离当前超过5秒以上 goto step3
    //step3 每5秒把 relaxBufferList 中数据全部放入 relaxForMatchList ，并按照战力排序 goto step4
    //step4 遍历排序后的 relaxForMatchList，将相邻的两个玩家两两配对，并加入 relaxList中 goto step5
    //step5 遍历 relaxList 将信息发送给玩家

    zUniqueID<DWORD>	*ranking_chanllengeID;		//PVP 排名对战
    zUniqueID<DWORD>	*competitive_chanllengeID;	//PVP 竞技对战
    zUniqueID<DWORD>	*friend_chanllengeID;		//PVP 好友对战
    zUniqueID<DWORD>	*practise_chanllengeID;		//PVE 普通练习
    zUniqueID<DWORD>	*boss_chanllengeID;		//PVE BOSS模式


    std::map<WORD, DWORD>   allGameCount;	//界域ID---对战局数
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
