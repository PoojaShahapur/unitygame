#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "RolechangeClient.h"
#include "SuperServer.h"
#include "RolechangeCommand.h"
#include "SuperCommand.h"
#include "ServerManager.h"
#include "RolechangeClientManager.h"
/*rief 定义登陆服务器客户端
 *
 * 
 */
DWORD RolechangeClient::tempidAllocator = 0;
/**
 * \brief 构造函数
 * \param ip 服务器地址
 * \param port 服务器端口
 */
RolechangeClient::RolechangeClient(const std::string &ip, const WORD port) : zTCPClientTask(ip, port, false), tempid(++tempidAllocator), netType(NetType_near)
{
  Zebra::logger->debug("RolechangeClient::RolechangeClient(%s:%u)",ip.c_str(),port);
}

/**
 * \brief 析构函数
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
      //这里只是从缓冲取数据包，所以不会出错，没有数据直接返回
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
			      Zebra::logger->debug("[转区] 转区成功 发送删除消息到网关:%s, %u, %u",cmd->name, cmd->accid, cmd->userid);
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
			      Zebra::logger->debug("[转区] 发送玩家数据到 RECORDSERVER失败, %u, %u, %u 转区失败", rev->fromGameZone.id, rev->accid, rev->userid);

			      RolechangeClientManager::getInstance().broadcastOne(&send, sizeof(send));
			  }
			  return true;
		      }
		      break;
		  case PARA_CHECK_VALID:
		      {
			  t_Check_Valid *ptCmd = (t_Check_Valid *)pNullCmd;

			  Zebra::logger->debug("[转区] 收到 roleChangeServer 转发来的 PARA_CHECK_VALID");

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
				  Zebra::logger->debug("[转区] 收到 roleChangeServer 转发来的 PARA_CHECK_VALID(想去战区), 配置验证通过 发往RECORDSERVER继续验证");
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
			      Zebra::logger->debug("[转区] 收到 roleChangeServer 转发来的 PARA_CHECK_VALID(想回原区), 配置验证通过 发往RECORDSERVER继续验证");
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
			      Zebra::logger->debug("[转区] 收到 roleChangeServer 发来的PARA_RET_CHECK_VALID,转发去 SESSIONSERVER 成功");
			  }
			  else
			  {
			      Zebra::logger->error("[转区] 收到 roleChangeServer 发来的PARA_RET_CHECK_VALID,转发去 SESSIONSERVER 失败");
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
			  Zebra::logger->debug("[跨区] 接受到 from=%u to=%u size=%u",rev->fromGameZone.id, rev->toGameZone.id, rev->size);
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
			    if(rev->isForce == 2)	    //玩家是否跨服在线
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
