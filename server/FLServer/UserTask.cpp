/**
* \brief ʵ�ַ�����������
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
* \brief �ȴ�������ָ֤�������֤
*
* ʵ���麯��<code>zTCPTask::verifyConn</code>
*
* \return ��֤�Ƿ�ɹ������߳�ʱ
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
			//����ֻ�Ǵӻ���ȡ���ݰ������Բ������û������ֱ�ӷ���
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
* \brief ��ӵ�Ψһ����֤������
* ʵ�����麯��<code>zTCPTask::uniqueAdd</code>
*/
void UserTask::addToContainer()
{
    UserContainer::getMe().add(this);
}

/**
* \brief ��Ψһ����֤������ɾ��
* ʵ�����麯��<code>zTCPTask::uniqueRemove</code>
*/
void UserTask::removeFromContainer()
{
    UserContainer::getMe().remove(this);
}

/**
* \brief �������Ը������������ӵ�ָ��
* \param pNullCmd �������ָ��
* \param nCmdLen ָ���
* \return �����Ƿ�ɹ�
*/
bool UserTask::msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#ifdef _MSGPARSE_
    Zebra::logger->error("?? UserTask::msgParse(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
#endif
    t_cmd_ret cmds;
    switch(pNullCmd->cmd)
    {
	case CMD_USER_CONSUME://����
	    {
		t_cmd_consume *pCmd = (t_cmd_consume*)pNullCmd;
		consume_func(pCmd, cmds);
		sendCmd(&cmds, sizeof(cmds));
		Zebra::logger->info("%s,%d,%d,%d,%d,%d,%s",
			pCmd->tid,cmds.ret,pCmd->uid,AT_CONSUME,pCmd->source,pCmd->point,pCmd->remark);
		return true;
	    }
	    break;
	case CMD_USER_FILLIN://��ֵ
	    {
		return true;
	    }
	    break;
	case CMD_USER_QBALANCE://��ѯ
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

