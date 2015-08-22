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
	DWORD mp;	    //��ǰħ��
	DWORD maxmp;	    //ħ������
	DWORD forbid;	    //��������ħ��
	/**
	 * \brief
	 *  ���ʵ�ʿ���ħ�� = mp - forbid
	 *  
	 *  mp ��ĳЩ����»ᳬ�� maxmp,�������ֻ����ʱ��
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
	DWORD playerID;			    //���ID
	DWORD cardsNumber;		    //����ID
	char playerName[MAX_NAMESIZE+1];    //�������
	bool prepare;			    //׼������
	std::vector<DWORD> prepareHand;	    //׼���׶ε�����
	std::vector<DWORD> cardsLibVec;	    //�ƿ�	    ����baseID
    private:
	bool inited;			    //��ʼ�����
	WORD tiredTimes;		    //�ۼ�ƣ�ʹ���
	MagicPoint mp;			    //ħ��

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

