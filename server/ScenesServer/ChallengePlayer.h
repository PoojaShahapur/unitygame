#ifndef _SCENESSERVER_CHALLENGEPLAYERDATA_H_
#define _SCENESSERVER_CHALLENGEPLAYERDATA_H_
#include "zType.h"
#include <vector>
///////////////////////////////////////////////
//
//code[ScenesServer/ChallengePlayer.h] defination by codemokey
//
//
///////////////////////////////////////////////

const DWORD mpLimit = 10;
class MagicPoint
{
    public:
	DWORD mp;	    //当前魔法
	DWORD maxmp;	    //魔法上限
	DWORD forbid;	    //被禁锢的魔法
	/**
	 * \brief
	 *  玩家实际可用魔法 = mp - forbid
	 *  
	 *  mp 在某些情况下会超过 maxmp,不过这个只是暂时的
	 *  
	 */

	MagicPoint()
	{
	    mp = 0;
	    maxmp = 0;
	    forbid = 0;
	}
	void resetMp()
	{
	    maxmp++;
	    if(maxmp >= mpLimit)
	    {
		maxmp = mpLimit;
	    }
	    mp = maxmp;
	}
	void addMp(const DWORD value)
	{
	    mp += value;
	    if(mp >= mpLimit)
	    {
		mp = mpLimit;
	    }
	}
	void addMaxMp(const DWORD value)
	{
	    maxmp += value;
	    if(maxmp >= mpLimit)
	    {
		maxmp = mpLimit;
	    }
	}
	bool check(const DWORD need)
	{
	    if(mp >= (need+forbid))
		return true;
	    return false;
	}
	bool reduce(const DWORD need)
	{
	    if(check(need))
	    {
		mp -= need;
		return true;
	    }
	    else
	    {
		return false;
	    }
	}
};

class ChallengePlayer
{
    public:
        ChallengePlayer();
        ~ChallengePlayer();
    public:
	DWORD playerID;			    //玩家ID
	DWORD cardsNumber;		    //套牌ID
	char playerName[MAX_NAMESIZE+1];    //玩家名字
	bool prepare;			    //准备好了
	std::vector<DWORD> prepareHand;	    //准备阶段的手牌
	std::vector<DWORD> cardsLibVec;	    //牌库	    卡牌baseID
    private:
	bool inited;			    //初始化完毕
	WORD tiredTimes;		    //累计疲劳次数
	MagicPoint mp;			    //魔法

    public:
	void clearPrepareHand();
	bool isInited();
	void setInit();
	void increaseTiredTimes();
	WORD getTiredTimes();
	DWORD extractOneCardFromLib();
	bool changeFirstHand(BYTE count, BYTE change);
	bool checkMp(DWORD needMp);
	bool reduceMp(DWORD needMp);
	DWORD getMp();
	DWORD getMaxMp();
	DWORD getForbid();
	void resetMp();
	void addMp(const DWORD value);
	void addMaxMp(const DWORD value);
	void addOneCardToLib(DWORD cardID);

};

#endif //_SCENESSERVER_CHALLENGEPLAYERDATA_H_

