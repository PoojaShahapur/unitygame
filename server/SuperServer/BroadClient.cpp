#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "BroadClient.h"
#include "SuperServer.h"
#include "BroadCommand.h"
#include "SuperCommand.h"
#include "ServerManager.h"
#include "BroadClientManager.h"
/*rief 定义登陆服务器客户端
 *
 * 
 */

/**
 * \brief 构造函数
 * \param ip 服务器地址
 * \param port 服务器端口
 */
BroadClient::BroadClient(const std::string &ip, const WORD port) : zTCPClientTask(ip,port,false),netType(NetType_near)
{
  Zebra::logger->debug("BroadClient::BroadClient(%s:%u)",ip.c_str(),port);
}

/**
 * \brief 析构函数
 *
 */
BroadClient::~BroadClient()
{
  Zebra::logger->debug("BroadClient::~BroadClient");
}

int BroadClient::checkRebound()
{
  Zebra::logger->debug("BroadClient::checkRebound");
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
      using namespace Cmd::BroadCast;

      t_LoginCmd_OK *ptCmd = (t_LoginCmd_OK *)pstrCmd;
      if (CMD_LOGIN == ptCmd->cmd
          && PARA_LOGIN_OK == ptCmd->para)
      {
        return 1;
      }
      else
      {
        Zebra::logger->error("BroadServer return the gameZone info, para ERROR!!!");
        return -1;
      }
    }
  }
  else
    return retcode;
}

void BroadClient::addToContainer()
{
  Zebra::logger->debug("BroadClient::addToContainer");
  BroadClientManager::getInstance().add(this);
}

void BroadClient::removeFromContainer()
{
  Zebra::logger->debug("BroadClient::removeFromContainer");
  BroadClientManager::getInstance().remove(this);
}

bool BroadClient::connect()
{
  Zebra::logger->debug("BroadClient::connect");
  if (!zTCPClientTask::connect())
    return false;

  using namespace Cmd::BroadCast;
  t_LoginCmd cmd;
  strncpy(cmd.strIP, SuperService::getInstance().getIP(), MAX_IP_LENGTH);
  cmd.port = SuperService::getInstance().getPort();
  return sendCmd(&cmd,sizeof(cmd));
}

bool BroadClient::msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#ifdef _MSGPARSE_
  Zebra::logger->error("?? BroadClient::msgParse(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
#endif

  using namespace Cmd::BroadCast;

  switch(pNullCmd->cmd)
  {
      case CMD_BROAD:
	  {
	    switch(pNullCmd->para)
	    {
		case PARA_BROADMESSAGE:
		    {
			t_BroadcastMessage* rev = (t_BroadcastMessage*)pNullCmd;
			msgParse_Broad((Cmd::t_NullCmd *)rev->data, rev->dataLen);
			return true;
		    }
		    break;
	    }
	    return true;
	  }
      break;
      default:
      break;
  }

  Zebra::logger->error("BroadClient::msgParse(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
  return false;
}

bool BroadClient::msgParse_Broad(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
  Zebra::logger->debug("BroadClient::msgParse_gyList");
  using namespace Cmd::BroadCast;

  switch(pNullCmd->cmd)
  {//需要提供全区广播服务的在这里
      case CMD_BROADDATA:
	  {
	      switch(pNullCmd->para)
	      {
		  case PARA_BROAD_CONSUME:
		      {
			  return true;
		      }
		  default:
		      break;
	      }
	      return true;
	  }
	  break;
      case Cmd::Super::CMD_SESSION:
	  {
	      switch(pNullCmd->para)
	      {
		  case Cmd::Super::PARA_USER_ONLINE_BROADCAST:
		      {
			  ServerManager::getInstance().broadcastByType(SESSIONSERVER, pNullCmd, nCmdLen);
		      }
		      break;
		  default:
		      break;
	      }
	  }
	  break;
  }

  Zebra::logger->error("BroadClient::msgParse_Broad(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
  return false;
}

