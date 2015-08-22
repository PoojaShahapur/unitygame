/**
* \brief 实现服务器连接类
*
* 
*/

#include "FLServer.h"
#include "UserTask.h"
#include "UserCommand.h"
#include "ServerACL.h"

using namespace std;
using namespace Cmd::UserServer;

/**
* \brief 等待接受验证指令并进行验证
*
* 实现虚函数<code>zTCPTask::verifyConn</code>
*
* \return 验证是否成功，或者超时
*/
int UserTask::verifyConn()
{
	Zebra::logger->debug("UserTask::verifyConn");
	int retcode = mSocket.recvToBuf_NoPoll();
	if (retcode > 0)
	{
		BYTE pstrCmd[zSocket::MAX_DATASIZE];
		int nCmdLen = mSocket.recvToCmd_NoPoll(pstrCmd,sizeof(pstrCmd));
		if (nCmdLen <= 0)
			//这里只是从缓冲取数据包，所以不会出错，没有数据直接返回
			return 0;
		else
		{
			t_logon *ptCmd = (t_logon *)pstrCmd;
			if (CMD_USER_LOGON == ptCmd->cmd)
			{
				bool mcheck = ServerACL::getMe().check(getIP(),ptCmd->port,gameZone,name);
				if (mcheck)
				{
					Zebra::logger->debug("BillClient connect right !!!(%s:%d)",getIP(),ptCmd->port);
					return 1;
				}else{
					Zebra::logger->error("This is a wrong zone(%s:%d)ChcekFailer",getIP(),ptCmd->port);
					return -1;
				}
			}
			else
			{
				Zebra::logger->error("msg t_LoginFL, para error(%s:%d)",getIP(),ptCmd->port);
				return -1;
			}
		}
	}
	else
		return retcode;
}

int UserTask::waitSync()
{
	Zebra::logger->debug("UserTask::waitSync");
	t_logon_OK cmd;
	cmd.gameZone = gameZone;
	bzero(cmd.name,sizeof(cmd.name));
	strncpy(cmd.name,name.c_str(),sizeof(cmd.name) - 1);
	cmd.netType = netType;
	if (sendCmd(&cmd,sizeof(cmd)))
		return 1;
	else
		return -1;
}

/**
* \brief 添加到唯一性验证容器中
* 实现了虚函数<code>zTCPTask::uniqueAdd</code>
*/
void UserTask::addToContainer()
{
    UserContainer::getMe().add(this);
}

/**
* \brief 从唯一性验证容器中删除
* 实现了虚函数<code>zTCPTask::uniqueRemove</code>
*/
void UserTask::removeFromContainer()
{
    UserContainer::getMe().remove(this);
}

/**
* \brief 解析来自各个服务器连接的指令
* \param pNullCmd 待处理的指令
* \param nCmdLen 指令长度
* \return 处理是否成功
*/
bool UserTask::msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#ifdef _MSGPARSE_
    Zebra::logger->error("?? UserTask::msgParse(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
#endif
    t_cmd_ret cmds;
    switch(pNullCmd->cmd)
    {
	case CMD_USER_CONSUME://消费
	    {
		t_cmd_consume *pCmd = (t_cmd_consume*)pNullCmd;
		consume_func(pCmd, cmds);
		sendCmd(&cmds, sizeof(cmds));
		Zebra::logger->info("%s,%d,%d,%d,%d,%d,%s",
			pCmd->tid,cmds.ret,pCmd->uid,AT_CONSUME,pCmd->source,pCmd->point,pCmd->remark);
		return true;
	    }
	    break;
	case CMD_USER_FILLIN://充值
	    {
		return true;
	    }
	    break;
	case CMD_USER_QBALANCE://查询
	    {
		t_cmd_qbalance *pCmd = (t_cmd_qbalance*)pNullCmd;
		qbalance_func(pCmd, cmds);
		sendCmd(&cmds, sizeof(cmds));
		return true;
	    }
	    break;
    }
    Zebra::logger->error("UserTask::msgParse(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
    return false;
}

