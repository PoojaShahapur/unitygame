#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "RoleregClient.h"
#include "SuperServer.h"
#include "RoleregCommand.h"
#include "SuperCommand.h"
#include "ServerManager.h"
#include "RoleregClientManager.h"
/*rief 定义登陆服务器客户端
 *
 * 
 */

/**
 * \brief 构造函数
 * \param ip 服务器地址
 * \param port 服务器端口
 */
RoleregClient::RoleregClient(const std::string &ip, const WORD port) : zTCPClientTask(ip, port, false),netType(NetType_near)
{
  Zebra::logger->debug("RoleregClient::RoleregClient(%s:%u)",ip.c_str(),port);
}

/**
 * \brief 析构函数
 *
 */
RoleregClient::~RoleregClient()
{
  Zebra::logger->debug("RoleregClient::~RoleregClient");
}

int RoleregClient::checkRebound()
{
  Zebra::logger->debug("RoleregClient::checkRebound");
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
      using namespace Cmd::RoleReg;

      t_LoginRoleReg_OK *ptCmd = (t_LoginRoleReg_OK *)pstrCmd;
      if (CMD_LOGIN == ptCmd->cmd
          && PARA_LOGIN_OK == ptCmd->para)
      {
        Zebra::logger->debug("RoleRegServer return the gameZone info :zoneid=%u(gameid=%u,zone=%u),name=%s,nettype=%u",
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
        Zebra::logger->error("RoleRegServer return the gameZone info, para ERROR!!!");
        return -1;
      }
    }
  }
  else
    return retcode;
}

void RoleregClient::addToContainer()
{
  Zebra::logger->debug("RoleregClient::addToContainer");
  RoleregClientManager::getInstance().add(this);
}

void RoleregClient::removeFromContainer()
{
  Zebra::logger->debug("RoleregClient::removeFromContainer");
  RoleregClientManager::getInstance().remove(this);
}

bool RoleregClient::connect()
{
  Zebra::logger->debug("RoleregClient::connect");
  if (!zTCPClientTask::connect())
    return false;

  using namespace Cmd::RoleReg;
  t_LoginRoleReg cmd;
  strncpy(cmd.strIP,SuperService::getInstance().getIP(),sizeof(cmd.strIP));
  cmd.port = SuperService::getInstance().getPort();
  return sendCmd(&cmd,sizeof(cmd));
}

bool RoleregClient::msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#ifdef _MSGPARSE_
  Zebra::logger->error("?? RoleregClient::msgParse(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
#endif

  using namespace Cmd::RoleReg;

  switch(pNullCmd->cmd)
  {
      case CMD_REG_WITHID:
	  {
	      switch(pNullCmd->para)
	      {
		  case PARA_CHARNAME_REG_WITHID:
		      {
			  using namespace Cmd::Super;
			  t_Charname_reg_withID *ptCmd = (t_Charname_reg_withID *)pNullCmd;
			  Zebra::logger->debug("[平台charid分配] 收到平台返回的 name(%s) accid(%u) 唯一id(%u)", ptCmd->name, ptCmd->accid, ptCmd->charid);
			  if(ptCmd->state & Cmd::Super::ROLEREG_STATE_WRITE)
			  {
			      if(ptCmd->regType == ROLE_REG_WITHID)
			      {
				  t_Charname_Gateway cmd;
				  cmd.regType = ptCmd->regType;
				  cmd.wdServerID = ptCmd->wdServerID;
				  cmd.accid = ptCmd->accid;
				  bcopy(ptCmd->name, cmd.name, sizeof(cmd.name));
				  cmd.state = ptCmd->state;
				  cmd.charid = ptCmd->charid;
				  ServerManager::getInstance().broadcastByID(cmd.wdServerID, &cmd, sizeof(cmd));
			      }
			      if(ptCmd->state & Cmd::Super::ROLEREG_STATE_OK)
			      {
			      }
			      else
			      {}
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

  Zebra::logger->error("RoleregClient::msgParse(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
  return false;
}

