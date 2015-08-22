/**
* \brief ʵ�ַ�����������
*
* 
*/

#include "roleRegServer.h"
#include "RoleTask.h"
#include "RoleregCommand.h"
#include "ServerACL.h"
#include "TableManager.h"
#include <iomanip>
/**
* \brief �ȴ�������ָ֤�������֤
*
* ʵ���麯��<code>zTCPTask::verifyConn</code>
*
* \return ��֤�Ƿ�ɹ������߳�ʱ
*/
int RoleTask::verifyConn()
{
	Zebra::logger->debug("RoleTask::verifyConn");
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
			using namespace Cmd::RoleReg;

			t_LoginRoleReg *ptCmd = (t_LoginRoleReg *)pstrCmd;
			if (CMD_LOGIN == ptCmd->cmd
				&& PARA_LOGIN == ptCmd->para
				)
			{
				bool mcheck = ServerACL::getMe().check(getIP(),ptCmd->port,gameZone,name);
				if (mcheck)
				{
					Zebra::logger->debug("This is a right zone(%s:%d)",getIP(),ptCmd->port);
					return 1;
				}else{
					Zebra::logger->error("This is a wrong zone(%s:%d)ChcekFailer",getIP(),ptCmd->port);
					return -1;
				}
			}
			else
			{
				Zebra::logger->error("msg t_LoginRoleReg, para error(%s:%d)",getIP(),ptCmd->port);
				return -1;
			}
		}
	}
	else
		return retcode;
}

int RoleTask::waitSync()
{
	Zebra::logger->debug("RoleTask::waitSync");
	using namespace Cmd::RoleReg;
	t_LoginRoleReg_OK cmd;
	cmd.gameZone = gameZone;
	bzero(cmd.name,sizeof(cmd.name));
	strncpy(cmd.name,name.c_str(),sizeof(cmd.name) - 1);
	if (sendCmd(&cmd,sizeof(cmd)))
		return 1;
	else
		return -1;
}

/**
* \brief �������Ը������������ӵ�ָ��
* \param pNullCmd �������ָ��
* \param nCmdLen ָ���
* \return �����Ƿ�ɹ�
*/
bool RoleTask::msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#ifdef _MSGPARSE_
	Zebra::logger->error("?? RoleTask::msgParse(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
#endif
	Zebra::logger->error("a RoleTask Cmd");
	using namespace Cmd::RoleReg;

	switch(pNullCmd->cmd)
	{
	case CMD_REG_WITHID:
		if (msgParse_regwithId(pNullCmd, nCmdLen)) return true;
		break;
	}
	Zebra::logger->error("RoleTask::msgParse(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
	return false;
}

bool RoleTask::msgParse_regwithId(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
	using namespace Cmd::RoleReg;
	switch(pNullCmd->para)
	{
	    case PARA_CHARNAME_REG_WITHID:
		{
		    t_Charname_reg_withID *ptCmd = (t_Charname_reg_withID *)pNullCmd;
		    WORD regType = ptCmd->regType;
		    if(regType == ROLE_REG_WITHID)
		    {
			if(msgParse_loginServer_withcharId(ptCmd, nCmdLen))
			{
			    return true;
			}
		    }
		    else
		    {
			Zebra::logger->error("PARA_CHARNAME_REG_WITHID regType ERROR:%u",regType);
		    }
		}
		break;
	}
	Zebra::logger->error("RoleTask::msgParse_regwithId(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
	return false;
}

bool RoleTask::msgParse_loginServer_withcharId(const Cmd::RoleReg::t_Charname_reg_withID *ptCmd, const DWORD nCmdLen)
{
    using namespace Cmd::RoleReg;
    t_Charname_reg_withID Cmd;
    Cmd.wdServerID = ptCmd->wdServerID;
    Cmd.accid = ptCmd->accid;
    Cmd.gameZone = ptCmd->gameZone;
    Cmd.regType = ptCmd->regType;
    strcpy(Cmd.name, ptCmd->name);
    Cmd.state = ptCmd->state;
    //ѡ��һ��������
    using namespace std;

    unsigned int hash = TableManager::hashString(ptCmd->name);
    Zebra::logger->debug("===����===%s", TableManager::getMe().TableName("rolereg", hash));
    Zebra::logger->debug("hash=%u", hash);

    if(ptCmd->state & ROLEREG_STATE_WRITE)
    {
	if(ifExistName(ptCmd->name, "rolereg"))
	{
	    Cmd.state &= ~ROLEREG_STATE_OK;
	    return sendCmd(&Cmd, sizeof(Cmd));
	}

	RoleData Tmpinfo;
	Tmpinfo.accid = ptCmd->accid;
	Tmpinfo.zone = ptCmd->gameZone.zone;
	Tmpinfo.game = ptCmd->gameZone.game;
	strcpy(Tmpinfo.name, ptCmd->name);
	static const dbCol create_con_define[] =
	{
	    { "`NAME`",zDBConnPool::DB_STR,sizeof(char[MAX_NAMESIZE]) },
	    { "`GAME`",zDBConnPool::DB_WORD,sizeof(WORD) },
	    { "`ZONE`",zDBConnPool::DB_WORD,sizeof(WORD) },
	    { "`ACCID`",zDBConnPool::DB_DWORD,sizeof(DWORD) },
	    { NULL,0,0}
	};
	connHandleID handle = roleRegService::dbConnPool->getHandle();
	if((connHandleID)-1 != handle)
	{
	    DWORD last_insert_id = roleRegService::dbConnPool->exeInsert(handle, 
		    TableManager::getMe().TableName("rolereg", hash),
		    create_con_define, (unsigned char*)&Tmpinfo);
	    if((DWORD)-1 == last_insert_id)
	    {
		Cmd.state &= ~ROLEREG_STATE_OK;
		Zebra::logger->debug("����charidʧ��");
	    }
	    else
	    {
		Cmd.state |= ROLEREG_STATE_OK;
		Cmd.charid = last_insert_id;
		Zebra::logger->debug("����charid�ɹ� �˺�:%s,%u charid:%u",ptCmd->name, ptCmd->accid, Cmd.charid);
	    }
	    roleRegService::dbConnPool->putHandle(handle);
	}
	else
	{
	    Cmd.state &= ~ROLEREG_STATE_OK;
	    Zebra::logger->error("��ȡ���ʧ��");
	}
    }
    return sendCmd(&Cmd, sizeof(Cmd));
}

bool RoleTask::ifExistName(const char* name, const char* tableName)
{
    unsigned int hash = TableManager::hashString(name);
    connHandleID handle = roleRegService::dbConnPool->getHandle(&hash);
    if((connHandleID)-1 != handle)
    {
	DWORD accID = 0;
	static const dbCol sel_con_define[] = 
	{
	    {"`ACCID`", zDBConnPool::DB_DWORD, sizeof(DWORD)},
	    {NULL, 0, 0}
	};
	char esc_buffer[strlen(name)*2 + 1];
	bzero(esc_buffer, sizeof(esc_buffer));
	roleRegService::dbConnPool->escapeString(handle, name, esc_buffer, 0);

	char where[128];
	memset(where, 0, sizeof(where));
	sprintf(where, "NAME = '%s'", esc_buffer);
	unsigned int tmpret = roleRegService::dbConnPool->exeSelectLimit(handle,
		TableManager::getMe().TableName(tableName, hash), 
		sel_con_define, where, NULL, 1, (unsigned char*)&accID);
	if(1 == tmpret)
	{
	    Zebra::logger->debug("��ѯ�ı�(%s)����(%s)�Ѿ������ݿ���,����ʹ���������", TableManager::getMe().TableName(tableName, hash), name);
	    roleRegService::dbConnPool->putHandle(handle);
	    return true;
	}
	else
	{
	    Zebra::logger->debug("��ѯ�ı�(%s)����(%s)�������ݿ���,����ʹ��������ִ�����ɫ", TableManager::getMe().TableName(tableName, hash), name);
	    roleRegService::dbConnPool->putHandle(handle);
	    return false;
	}

    }
    else
    {
	Zebra::logger->error("��ȡ���ʧ��");
	return false;
    }
    return false;
}
