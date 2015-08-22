/*************************************************************************
 Author: wang
 Created Time: 2015��02��13�� ������ 14ʱ26��09��
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
	case REQ_RETURN_SOURCE_PARA:	//��ԭ��
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
			Zebra::logger->trace("[ת��] �����ԭ�� ��SuperServer����t_Check_Valid �ɹ� accid:%u,charid:%u userlevel:%u",
				send.accid, send.userid, send.userlevel);
		    }
		    else
		    {//log fail
			Zebra::logger->trace("[ת��] �����ԭ�� ��SuperServer����t_Check_Valid ʧ�� accid:%u,charid:%u userlevel:%u",
				send.accid, send.userid, send.userlevel);
		    }
		}
		return true;
	    }
	    break;
	case REQ_ENTER_BATTLE_PARA:	//ȥս��
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
		    Zebra::logger->trace("[ת��] ����ȥս�� ��SuperServer����t_Check_Valid �ɹ� accid:%u,charid:%u userlevel:%u",
			    send.accid, send.userid, send.userlevel);
		}
		else
		{//log fail
		    Zebra::logger->trace("[ת��] ����ȥս�� ��SuperServer����t_Check_Valid ʧ�� accid:%u,charid:%u userlevel:%u",
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
