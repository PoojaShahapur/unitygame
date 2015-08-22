/*************************************************************************
 Author: wang
 Created Time: 2014��11��06�� ������ 17ʱ15��45��
 File Name: ScenesServer/Recycle.cpp
 Description: 
 ************************************************************************/
#include "Recycle.h"
#include "TimeTick.h"
#include "SceneNpc.h"

SceneRecycle *SceneRecycle::instance = NULL;

void SceneRecycle::insert(SceneNpc *n, DWORD delay)
{
    if(!n) return;
    npcList.push(n);
}

void SceneRecycle::run()
{
    while(!isFinal())
    {
	zThread::msleep(3000);
	DWORD delNum = npcList.size()/5;
	if(delNum)
	{
	    std::set<SceneNpc *> npcDelList;
	    for(DWORD i=0; i<delNum; i++)
	    {
		npcDelList.insert(npcList.front());
		npcList.pop();
	    }
#ifdef _WC1_DEBUG
	    if(!npcDelList.empty())
		Zebra::logger->debug("[Recycle]delete %u npc", npcDelList.size());
#endif
	    for(std::set<SceneNpc *>::iterator di = npcDelList.begin(); di!=npcDelList.end(); di++)
	    {
		delete *di;
	    }
	    npcDelList.clear();
	}
    }
}
