#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "RolechangeClient.h"
#include "SuperServer.h"
#include "RolechangeCommand.h"
#include "SuperCommand.h"
#include "ServerManager.h"
#include "RolechangeClientManager.h"
/*rief �����½�������ͻ���
 *
 * 
 */
DWORD RolechangeClient::tempidAllocator = 0;
/**
 * \brief ���캯��
 * \param ip ��������ַ
 * \param port �������˿�
 */
RolechangeClient::RolechangeClient(const std::string &ip, const WORD port) : zTCPClientTask(ip, port, false), tempid(++tempidAllocator), netType(NetType_near)
{
  Zebra::logger->debug("RolechangeClient::RolechangeClient(%s:%u)",ip.c_str(),port);
}

/**
 * \brief ��������
 *
 */
RolechangeClient::~RolechangeClient()
{
  Zebra::logger->debug("RolechangeClient::~RolechangeClient");
}

int RolechangeClient::checkRebound()
{
  Zebra::logger->debug("RolechangeClient::checkRebound");
  int retcode = pSocket->recvToBuf_NoPoll();
  if (retcode > 0)
  {
    BYTE pstrCmd[zSocket::MAX_DATASIZE];
    int nCmdLen = pSocket->recvToCmd_NoPoll(pstrCmd,sizeof(pstrCmd));
    if (nCmdLen <= 0)
      //����ֻ�Ǵӻ���ȡ���ݰ������Բ������û������ֱ�ӷ���
      return 0;
    else
    {
      using namespace Cmd::Rolechange;

      t_LoginCmd_OK *ptCmd = (t_LoginCmd_OK *)pstrCmd;
      if (CMD_LOGIN == ptCmd->cmd
          && PARA_LOGIN_OK == ptCmd->para)
      {
        Zebra::logger->debug("RolechangeServer return the gameZone info :zoneid=%u(gameid=%u,zone=%u),name=%s,nettype=%u",
            ptCmd->gameZone.id,
            ptCmd->gameZone.game,
            ptCmd->gameZone.zone,
            ptCmd->name,
            ptCmd->netType);
        netType = (ptCmd->netType == 0 ? NetType_near : NetType_far);
        return 1;
      }
      else
      {
        Zebra::logger->error("RolechangeServer return the gameZone info, para ERROR!!!");
        return -1;
      }
    }
  }
  else
    return retcode;
}

void RolechangeClient::addToContainer()
{
  Zebra::logger->debug("RolechangeClient::addToContainer");
  RolechangeClientManager::getInstance().add(this);
}

void RolechangeClient::removeFromContainer()
{
  Zebra::logger->debug("RolechangeClient::removeFromContainer");
  RolechangeClientManager::getInstance().remove(this);
}

bool RolechangeClient::connect()
{
  Zebra::logger->debug("RolechangeClient::connect");
  if (!zTCPClientTask::connect())
    return false;

  using namespace Cmd::Rolechange;
  t_LoginCmd cmd;
  strncpy(cmd.strIP,SuperService::getInstance().getIP(),sizeof(cmd.strIP));
  cmd.port = SuperService::getInstance().getPort();
  return sendCmd(&cmd,sizeof(cmd));
}

bool RolechangeClient::msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#ifdef _MSGPARSE_
  Zebra::logger->error("?? RolechangeClient::msgParse(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
#endif

  using namespace Cmd::Rolechange;

  switch(pNullCmd->cmd)
  {
      case CMD_BATTLE:
	  {
	      switch(pNullCmd->para)
	      {
		  case PARA_RTN_ZONE_LIST:
		      {}
		      break;
		  case PARA_RET_SEND_USER_TOZONE:
		      {
			  t_retSendUserToZone* cmd = (t_retSendUserToZone*)pNullCmd;
			  if(cmd->type == 0 && cmd->state == 0)
			  {
			      Cmd::Super::t_ChangeZoneDel_Gateway send;	
			      send.userid = cmd->userid;
			      send.accid = cmd->accid;
			      strncpy(send.name, cmd->name, MAX_NAMESIZE);
			      ServerManager::getInstance().broadcastByOne(GATEWAYSERVER, &send, sizeof(send));
			      Zebra::logger->debug("[ת��] ת���ɹ� ����ɾ����Ϣ������:%s, %u, %u",cmd->name, cmd->accid, cmd->userid);
			  }
			  else
			  {
			      ServerManager::getInstance().broadcastByType(RECORDSERVER, pNullCmd, nCmdLen);
			  }
			  return true;
		      }
		      break;
		  case PARA_SEND_USER_TOZONE:
		      {
			  t_sendUserToZone* rev = (t_sendUserToZone*)pNullCmd;
			  if(!ServerManager::getInstance().broadcastByType(RECORDSERVER, pNullCmd, nCmdLen))
			  {
			      t_retSendUserToZone send;
			      send.accid = rev->accid;
			      send.userid = rev->userid;
			      send.state = TOZONE_FAIL;
			      Zebra::logger->debug("[ת��] ����������ݵ� RECORDSERVERʧ��, %u, %u, %u ת��ʧ��", rev->fromGameZone.id, rev->accid, rev->userid);

			      RolechangeClientManager::getInstance().broadcastOne(&send, sizeof(send));
			  }
			  return true;
		      }
		      break;
		  case PARA_CHECK_VALID:
		      {
			  t_Check_Valid *ptCmd = (t_Check_Valid *)pNullCmd;

			  Zebra::logger->debug("[ת��] �յ� roleChangeServer ת������ PARA_CHECK_VALID");

			  t_Ret_Check_Valid send;
			  send.toGameZone = ptCmd->fromGameZone;
			  send.fromGameZone = SuperService::getInstance().getZoneID();
			  send.accid = ptCmd->accid;
			  send.userid = ptCmd->userid;
			  send.type = ptCmd->type;
			  if(Zebra::global["battle"] == "true" && ptCmd->type==Cmd::Rolechange::TYPE_TOZONE)
			  {
			      if(ptCmd->userlevel >= (DWORD)atoi(Zebra::global["battle_start_level"].c_str())
				      && ptCmd->userlevel <= (DWORD)atoi(Zebra::global["battle_end_level"].c_str()))
			      {
				  Zebra::logger->debug("[ת��] �յ� roleChangeServer ת������ PARA_CHECK_VALID(��ȥս��), ������֤ͨ�� ����RECORDSERVER������֤");
				  ServerManager::getInstance().broadcastByType(RECORDSERVER, pNullCmd, nCmdLen);
				  return true;

			      }
			      else
			      {
				  send.state = Cmd::Rolechange::BATTLE_INVALID;

			      }
			  }
			  else if(ptCmd->type==Cmd::Rolechange::TYPE_BACKZONE)
			  {
			      Zebra::logger->debug("[ת��] �յ� roleChangeServer ת������ PARA_CHECK_VALID(���ԭ��), ������֤ͨ�� ����RECORDSERVER������֤");
			      ServerManager::getInstance().broadcastByType(RECORDSERVER, pNullCmd, nCmdLen);
			      return true;
			  }
			  else
			  {
			      send.state = Cmd::Rolechange::BATTLE_INVALID;
			  }
			  RolechangeClientManager::getInstance().broadcastOne(&send, sizeof(send));
			  return true;
		      }
		      break;
		  case PARA_RET_CHECK_VALID:
		      {
			  if(ServerManager::getInstance().broadcastByType(SESSIONSERVER, pNullCmd, nCmdLen))
			  {
			      Zebra::logger->debug("[ת��] �յ� roleChangeServer ������PARA_RET_CHECK_VALID,ת��ȥ SESSIONSERVER �ɹ�");
			  }
			  else
			  {
			      Zebra::logger->error("[ת��] �յ� roleChangeServer ������PARA_RET_CHECK_VALID,ת��ȥ SESSIONSERVER ʧ��");
			  }
			  return true;
		      }
		      break;
		  default:
		      break;
	      }
	  }
	  break;
      case Cmd::Rolechange::CMD_COMMON:
	  {
	      switch(pNullCmd->para)
	      {
		  case PARA_FORWARD_MSG:
		      {
			  t_ForwardMsg_CommonCmd* rev = (t_ForwardMsg_CommonCmd*)pNullCmd;
			  Zebra::logger->debug("[����] ���ܵ� from=%u to=%u size=%u",rev->fromGameZone.id, rev->toGameZone.id, rev->size);
			  return msgParse_Forward((const Cmd::t_NullCmd *)rev->msg, rev->size, rev->fromGameZone.id);
		      }
		      break;
		  default:
		      break;
	      }
	  }
	  break;
      default:
	  break;
  }

  Zebra::logger->error("RolechangeClient::msgParse(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
  return false;
}

bool RolechangeClient::msgParse_Forward(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen, DWORD fromGameID)
{
    if(!pNullCmd)
	return false;
    using namespace Cmd::Rolechange;
    switch(pNullCmd->cmd)
    {
	case CMD_BATTLE:
	    {
		switch(pNullCmd->para)
		{
		    case PARA_RET_CHECK_ZONE_STATE:
			{
			    return ServerManager::getInstance().broadcastByType(GATEWAYSERVER, pNullCmd, nCmdLen);
			}
			break;
		    case PARA_CHECK_ZONE_STATE:
			{
			    t_checkZoneState* rev = (t_checkZoneState*)pNullCmd;
			    rev->fromGameID = fromGameID;
			    if(rev->isForce == 2)	    //����Ƿ�������
			    {
				ServerManager::getInstance().broadcastByType(SESSIONSERVER, pNullCmd, nCmdLen);
			    }
			    else
			    {
				ServerManager::getInstance().broadcastByType(RECORDSERVER, pNullCmd, nCmdLen);
			    }
			    return true;
			}
			break;
		    default:
			break;
		}
	    }
	    break;
	default:
	    break;
    }
    return false;
}
