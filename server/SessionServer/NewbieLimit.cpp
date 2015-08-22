/*************************************************************************
 Author: wang
 Created Time: 2014��12��23�� ���ڶ� 12ʱ09��12��
 File Name: SessionServer/NewbieLimit.cpp
 Description: 
 ************************************************************************/
#include "NewbieLimit.h"
#include "Session.h"
#include "SessionManager.h"

#include <limits>
bool NewbieLimit::_bEnabled = true;

NewbieLimit::NewbieLimit(void)
{}

NewbieLimit::~NewbieLimit(void)
{}

std::string NewbieLimit::getNewbieMapName(const std::string &sCountryName)
{
    const int MAX_TOWNS = 5;
    const std::string TOWNS[MAX_TOWNS] = 
    {
	"��ճ�",
	"��ճǷ���һ",
	"��ճǷ�����",
	"��ճǷ�����",
	"��ճǷ�����"
    };

    if(!_bEnabled)
	return TOWNS[0];

    const std::string &sNewbieLimit = Zebra::global["newbielimit"];
    if(sNewbieLimit.empty())
	return TOWNS[0];
    const int NEWBIE_LIMIT = atoi(sNewbieLimit.c_str());

    int iBestTown = 0;
    int nMin = std::numeric_limits<int>::max();
   
    for(int i=0; i<MAX_TOWNS; i++)
    {	
	std::string sFullName = sCountryName + "��" + TOWNS[i];
	const SceneSession* pScene = SceneSessionManager::getInstance()->getSceneByName(sFullName.c_str());
	if(NULL == pScene)
	{
	    continue;
	}
	int nUserCount = pScene->getUserCount();
	if(nUserCount < NEWBIE_LIMIT)
	    return TOWNS[i];
	
	if(nUserCount > nMin) 
	    continue;
	nMin = nUserCount;
	iBestTown = i;
    }
    return TOWNS[iBestTown];
}

void NewbieLimit::enable(bool bEnable)
{
    _bEnabled = bEnable;
    Zebra::logger->info("[���ִ����] ���� %s",bEnable?"����":"�ر�");
}
