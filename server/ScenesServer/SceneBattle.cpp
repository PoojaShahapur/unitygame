/*************************************************************************
 Author: wang
 Created Time: 2015年02月13日 星期五 14时26分09秒
 File Name: ScenesServer/SceneBattle.cpp
 Description: 
 ************************************************************************/
#include "BattleUserCmd.h"
#include "SceneUser.h"
#include "RolechangeCommand.h"
#include "ScenesServer.h"
#include "BinaryVersion.h"

bool SceneUser::doBattleCmd(const Cmd::stBattleUserCmd *rev, unsigned int cmdLen)
{
    using namespace Cmd;
    switch(rev->byParam)
    {
	case REQ_RETURN_SOURCE_PARA:	//回原区
	    {
		if(ScenesService::getInstance().battle && this->charbase.zone_state == CHANGEZONE_TARGETZONE)
		{
		    Cmd::Rolechange::t_Check_Valid send;
		    send.accid = this->accid;
		    send.userid = this->id;
		    send.userlevel = 0;
		    send.toGameZone.id = this->charbase.source_zone;
		    send.charbaseSize = sizeof(CharBase);
		    send.verifyVersion = ScenesService::getInstance().verify_version;
		    send.checkvalidSize = sizeof(Cmd::Rolechange::t_Check_Valid);
		    send.objVersion = BINARY_VERSION;
		    send.checkWriteSize = sizeof(Cmd::Record::t_WriteUser_SceneRecord);
		    send.type = Cmd::Rolechange::TYPE_BACKZONE;

		    if(ScenesService::getInstance().sendCmdToSuperServer(&send, sizeof(send)))
		    {//log ok
			Zebra::logger->trace("[转区] 请求回原区 向SuperServer发送t_Check_Valid 成功 accid:%u,charid:%u userlevel:%u",
				send.accid, send.userid, send.userlevel);
		    }
		    else
		    {//log fail
			Zebra::logger->trace("[转区] 请求回原区 向SuperServer发送t_Check_Valid 失败 accid:%u,charid:%u userlevel:%u",
				send.accid, send.userid, send.userlevel);
		    }
		}
		return true;
	    }
	    break;
	case REQ_ENTER_BATTLE_PARA:	//去战区
	    {
		Cmd::Rolechange::t_Check_Valid send;
		send.accid = this->accid;
		send.userid = this->id;
		send.userlevel = this->charbase.level;
		if(ScenesService::getInstance().battleZoneID == 0)
		    return true;
		send.toGameZone.game = 10;
		send.toGameZone.zone = ScenesService::getInstance().battleZoneID;
		send.charbaseSize = sizeof(CharBase);
		send.checkvalidSize = sizeof(Cmd::Rolechange::t_Check_Valid);
		send.objVersion = BINARY_VERSION;
		send.checkWriteSize = sizeof(Cmd::Record::t_WriteUser_SceneRecord);
		send.verifyVersion = ScenesService::getInstance().verify_version;
		send.type = Cmd::Rolechange::TYPE_TOZONE;
		if(ScenesService::getInstance().sendCmdToSuperServer(&send, sizeof(send)))
		{//log ok
		    Zebra::logger->trace("[转区] 请求去战区 向SuperServer发送t_Check_Valid 成功 accid:%u,charid:%u userlevel:%u",
			    send.accid, send.userid, send.userlevel);
		}
		else
		{//log fail
		    Zebra::logger->trace("[转区] 请求去战区 向SuperServer发送t_Check_Valid 失败 accid:%u,charid:%u userlevel:%u",
			    send.accid, send.userid, send.userlevel);
		}
		return true;
	    }
	    break;
	default:
	    break;

    }
    return false;
}
