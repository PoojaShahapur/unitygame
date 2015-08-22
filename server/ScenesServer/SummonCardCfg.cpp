#include "SummonCardCfg.h"
///////////////////////////////////////////////
//
//code[ScenesServer/SummonCardCfg.cpp] defination by codemokey
//
//
///////////////////////////////////////////////
#include "zXML.h"
using namespace xml;


SummonCardCfg::SummonCardCfg()
{}

SummonCardCfg::~SummonCardCfg()
{}

DWORD SummonCardCfg::randomOneIDByIndex(DWORD index)
{
    DWORD id = 0;
    SummonConfig::CardMapIter it = summon.card.find(index);
    if(it != summon.card.end())
    {
	DWORD totalOdds = 0;
	for(SummonConfig::Card::ItemMapIter it2 = it->second.item.begin(); it2 != it->second.item.end(); it2++)
	{
	    totalOdds += it2->second.odds;
	}

	DWORD num = zMisc::randBetween(1, totalOdds);
	DWORD tempnum = 0;
	for(SummonConfig::Card::ItemMapIter it2 = it->second.item.begin(); it2 != it->second.item.end(); it2++)
	{
	    if(num <= (it2->second.odds+tempnum))
	    {
		id = it2->first;
		break;
	    }
	    else
	    {
		tempnum += it2->second.odds;
	    }
	}
    }
    return id;
}
