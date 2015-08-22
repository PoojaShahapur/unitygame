#include "ChallengePlayer.h"
///////////////////////////////////////////////
//
//code[ScenesServer/ChallengePlayer.cpp] defination by codemokey
//
//
///////////////////////////////////////////////
#include <bitset>
#include "zMisc.h"


ChallengePlayer::ChallengePlayer()
{
   memset(this, 0, sizeof(*this)); 
}

ChallengePlayer::~ChallengePlayer()
{
}

void ChallengePlayer::clearPrepareHand()
{
    prepareHand.clear();
}

bool ChallengePlayer::isInited()
{
    return inited;
}

void ChallengePlayer::setInit()
{
    inited = true;
}

void ChallengePlayer::increaseTiredTimes()
{
    tiredTimes++;
}

WORD ChallengePlayer::getTiredTimes()
{
    return tiredTimes;
}

DWORD ChallengePlayer::extractOneCardFromLib()
{
    if(cardsLibVec.empty())
	return 0;
    DWORD id = 0;
    DWORD index = zMisc::randBetween(0, cardsLibVec.size()-1);
    id = cardsLibVec[index];
    //remove first element with value val
    std::vector<DWORD>::iterator pos = cardsLibVec.begin() + index;
    if (pos != cardsLibVec.end()) 
    {
	cardsLibVec.erase(pos);
    }
    return id;
}

bool ChallengePlayer::changeFirstHand(BYTE count, BYTE change)
{
    std::bitset<8> bs(change);
    if((change > 0) && !prepare)
    {
	for(BYTE i=0; i<count; i++)
	{
	    if(bs.test(i) && prepareHand[i])
	    {
		cardsLibVec.push_back(prepareHand[i]);
		prepareHand[i] = 0;	//设置被换的位置
	    }
	}

	for(BYTE i=0; i<count; i++)
	{
	    if(prepareHand[i] == 0)
	    {
		prepareHand[i] = extractOneCardFromLib();
	    }
	}
	prepare = true;
	return true;
    }
    prepare = true;
    return false;
}

bool ChallengePlayer::checkMp(DWORD needMp)
{
    return mp.check(needMp);
}

bool ChallengePlayer::reduceMp(DWORD needMp)
{
    return mp.reduce(needMp);
}

DWORD ChallengePlayer::getMp()
{
    return mp.mp;
}

DWORD ChallengePlayer::getMaxMp()
{
    return mp.maxmp;
}

DWORD ChallengePlayer::getForbid()
{
    return mp.forbid;
}

void ChallengePlayer::resetMp()
{
    mp.resetMp();
}


void ChallengePlayer::addMp(const DWORD value)
{
    mp.addMp(value);
}

void ChallengePlayer::addMaxMp(const DWORD value)
{
    mp.addMaxMp(value);
}

void ChallengePlayer::addOneCardToLib(DWORD cardID)
{
    if(cardsLibVec.empty())
    {
	cardsLibVec.push_back(cardID);
    }
    else
    {
	DWORD index = zMisc::randBetween(0, cardsLibVec.size()-1);
	std::vector<DWORD>::iterator pos = cardsLibVec.begin() + index;
	if (pos != cardsLibVec.end()) 
	{
	    cardsLibVec.insert(pos, cardID);
	}
    }
}
