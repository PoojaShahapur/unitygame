/*************************************************************************
 Author: wang
 Created Time: 2014年11月04日 星期二 11时08分52秒
 File Name: ScenesServer/Trade.cpp
 Description: 
 ************************************************************************/
#include "SceneUser.h"
#include "SceneNpc.h"
#include "NpcTrade.h"
#include "QuestTable.h"
#include "QuestEvent.h"

bool SceneUser::doTradeCmd(const Cmd::stTradeUserCmd *rev, unsigned int cmdLen)
{
    using namespace Cmd;
    switch(rev->byParam)
    {
	case VISITNPC_TRADE_USERCMD_PARAMETER:
	    {
		stVisitNpcTradeUserCmd *ptCmd = (stVisitNpcTradeUserCmd *)rev;
		SceneNpc *sceneNpc = SceneNpcManager::getMe().getNpcByTempID(ptCmd->dwNpcTempID);
		if(!sceneNpc)
		    return false;
		BYTE buf[zSocket::MAX_DATASIZE];
		stVisitNpcTradeUserCmd *cmd = (stVisitNpcTradeUserCmd *)buf;
		bzero(buf, sizeof(buf));
		constructInPlace(cmd);

		if(sceneNpc /*&& this->canVisitNpc(sceneNpc)*/)
		{
		    visitNpc(sceneNpc->id, sceneNpc->tempid);
		    int len = 0;
		    int status = 0;

		    OnVisit event(sceneNpc->id);
		    EventTable::instance().execute(*this, event);
		    
		    len = quest_list.get_menu(cmd->menuTxt, status);

		    if(NpcTrade::getInstance().getNpcMenu(sceneNpc->id, cmd->menuTxt+len))
		    {
			cmd->byReturn = 1;
			cmd->action = NpcTrade::getInstance().getNpcAction(sceneNpc->id);
			cmd->dwNpcTempID = sceneNpc->tempid;
		    }
		    sendCmdToMe(cmd, sizeof(stVisitNpcTradeUserCmd)+strlen(cmd->menuTxt));
		}
		return true;
	    }
	    break;
	default:
	    break;
    }
    return false;
}
