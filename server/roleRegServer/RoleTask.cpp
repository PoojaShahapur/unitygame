/**
* \brief 实现服务器连接类
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
* \brief 等待接受验证指令并进行验证
*
* 实现虚函数<code>zTCPTask::verifyConn</code>
*
* \return 验证是否成功，或者超时
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
			//这里只是从缓冲取数据包，所以不会出错，没有数据直接返回
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
* \brief 解析来自各个服务器连接的指令
* \param pNullCmd 待处理的指令
* \param nCmdLen 指令长度
* \return 处理是否成功
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
    //选择一个表名字
    using namespace std;

    unsigned int hash = TableManager::hashString(ptCmd->name);
    Zebra::logger->debug("===表名===%s", TableManager::getMe().TableName("rolereg", hash));
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
		Zebra::logger->debug("生成charid失败");
	    }
	    else
	    {
		Cmd.state |= ROLEREG_STATE_OK;
		Cmd.charid = last_insert_id;
		Zebra::logger->debug("生成charid成功 账号:%s,%u charid:%u",ptCmd->name, ptCmd->accid, Cmd.charid);
	    }
	    roleRegService::dbConnPool->putHandle(handle);
	}
	else
	{
	    Cmd.state &= ~ROLEREG_STATE_OK;
	    Zebra::logger->error("获取句柄失败");
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
	    Zebra::logger->debug("查询的表(%s)名字(%s)已经在数据库中,不能使用这个名字", TableManager::getMe().TableName(tableName, hash), name);
	    roleRegService::dbConnPool->putHandle(handle);
	    return true;
	}
	else
	{
	    Zebra::logger->debug("查询的表(%s)名字(%s)不在数据库中,可以使用这个名字创建角色", TableManager::getMe().TableName(tableName, hash), name);
	    roleRegService::dbConnPool->putHandle(handle);
	    return false;
	}

    }
    else
    {
	Zebra::logger->error("获取句柄失败");
	return false;
    }
    return false;
}
