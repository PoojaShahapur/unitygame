/**
* \brief 定义登陆连接任务
*
*/

#include "LoginTask.h"
#include "LoginManager.h"
#include "GYListManager.h"

DWORD LoginTask::uniqueID = 0;

/**
* \brief 构造函数
* \param pool 所属的连接池
* \param sock TCP/IP套接口
*/
DWORD g_LoginTaskDebugValue = 0;
LoginTask::LoginTask( zTCPTaskPool *pool,const int sock) : zTCPTask(pool,sock,NULL,true,false),lifeTime(), tempid(0)
{
	static BYTE key[16] = {0x3f, 0x79, 0xd5, 0xe2, 0x4a, 0x8c, 0xb6, 0xc1, 0xaf, 0x31, 0x5e, 0xc7, 0xeb, 0x9d, 0x6e, 0xcb};

	Zebra::logger->debug("LoginTask::LoginTask");
	bzero(jpegPassport,sizeof(jpegPassport));
//#ifndef _MOBILE
	Zebra::logger->debug("LoginTask::LoginTask Have ENCDEC_MSG");
	mSocket.setEncMethod(CEncrypt::ENCDEC_RC5);
	mSocket.set_key_rc5((const BYTE *)key,16,12);
//#endif
}

int LoginTask::verifyConn()
{
    Zebra::logger->debug("LoginTask::verifyConn()");
    int retcode = mSocket.recvToBuf_NoPoll();
    Zebra::logger->debug("retcode is :%d sizeof(stUserVerifyVerCmd):%u sizeof(stNullUserCmd):%u", retcode,sizeof(Cmd::stUserVerifyVerCmd),sizeof(Cmd::stNullUserCmd));
    if (retcode > 0)
    {
	BYTE pstrCmd[zSocket::MAX_DATASIZE];
	int nCmdLen = mSocket.recvToCmd_NoPoll(pstrCmd,sizeof(pstrCmd));
	Zebra::logger->debug("retcode is :%d nCmdLen:%d", retcode,nCmdLen);
	if (nCmdLen <= 0)
	    //这里只是从缓冲取数据包，所以不会出错，没有数据直接返回
	    return 0;
	else
	{
	    using namespace Cmd;
	    Zebra::logger->debug("LoginTask::verifyConn");

	    stUserVerifyVerCmd *ptCmd = (stUserVerifyVerCmd *)pstrCmd;
	    Zebra::logger->info("stUserVerifyVerCmd ptCmd->version(%u)",ptCmd->version);
	    if (LOGON_USERCMD == ptCmd->byCmd
		    && USER_VERIFY_VER_PARA == ptCmd->byParam)
	    {        
		verify_client_version = ptCmd->version;
		Zebra::logger->info("客户端连接指令验证通过(%s:%u)",mSocket.getIP(),mSocket.getPort());
		return 1;
	    }
	    else
	    {
		Zebra::logger->error("Cmd or Para error%s:%u)\n",mSocket.getIP(),mSocket.getPort());
		return -1;
	    }      
	}
    }
    else
	return retcode;
}

int LoginTask::recycleConn()
{
	Zebra::logger->debug("LoginTask::recycleConn()");
	mSocket.force_sync();
	return 1;
}

void LoginTask::addToContainer()
{
	Zebra::logger->debug("LoginTask::addToContainer()");
	using namespace Cmd;
	BYTE buf[zSocket::MAX_DATASIZE];
	stJpegPassportUserCmd *cmd = (stJpegPassportUserCmd *)buf;
	constructInPlace(cmd);
#if 0
	int size = 0;
	void *ret = jpeg_Passport(jpegPassport,sizeof(jpegPassport),&size);
	if (ret)
	{
		if (size >  0 && size <= (int)(zSocket::MAX_DATASIZE - sizeof(stJpegPassportUserCmd) - 100))
		{
			Zebra::logger->info("生成图形验证码：%s",jpegPassport);
			cmd->size = size;
			bcopy(ret,cmd->data,size,sizeof(buf) - sizeof(stJpegPassportUserCmd));
		}
		free(ret);
	}
#endif
	//sendCmd(cmd,sizeof(stJpegPassportUserCmd) + cmd->size);
}

bool LoginTask::uniqueAdd()
{
	Zebra::logger->debug("LoginTask::uniqueAdd()");
	return LoginManager::getInstance().add(this);
}

bool LoginTask::uniqueRemove()
{
	Zebra::logger->debug("LoginTask::uniqueRemove");
	LoginManager::getInstance().remove(this);
	return true;
}

bool LoginTask::requestLogin(const Cmd::stUserRequestLoginCmd *ptCmd)
{
	Zebra::logger->debug("LoginTask::requestLogin");

	using namespace Cmd;
	using namespace Cmd::DBAccess;

	//生成区唯一编号
	GameZone_t gameZone;
	gameZone.game = ptCmd->game;
	gameZone.zone = ptCmd->zone;
	Zebra::logger->info("�����¼��Ϸ�� :gameid=%u(game=%u,zone=%u),jpegPassport=%s",gameZone.id,gameZone.game,gameZone.zone,ptCmd->jpegPassport);
	
	//��֤�ͻ��˰汾��
	BYTE retcode = LOGIN_RETURN_VERSIONERROR;
	if(GYListManager::getInstance().verifyVer(gameZone, verify_client_version, retcode))
	{
	    Zebra::logger->debug("�ͻ������� ͨ���汾����֤");
	}
	else
	{
	    LoginReturn(retcode);
	    return false;
	}


	t_LoginServer_SessionCheck tCmd;
	bzero(&tCmd.session,sizeof(tCmd.session));
	tCmd.session.gameZone    = gameZone;
	tCmd.session.loginTempID = tempid;
	strncpy(tCmd.session.client_ip,getIP(),MAX_IP_LENGTH);    
	strncpy(tCmd.session.name, ptCmd->pstrName, sizeof(tCmd.session.name));		    //�˺���
	strncpy(tCmd.session.passwd, (char *)(ptCmd->pstrPassword), sizeof(tCmd.session.passwd));	    //����

	//验证客户端版本号
#if 0
	//验证jpeg图形验证码
	if (FLService::getMe().jpeg_passport
		&& strncmp(jpegPassport,ptCmd->jpegPassport,sizeof(jpegPassport)))
	{
		Zebra::logger->error("图形验证码错误：%s,%s",jpegPassport,ptCmd->jpegPassport);
		LoginReturn(LOGIN_RETURN_JPEG_PASSPORT);
		return false;
	}
#endif

	//验证用户名称和密码合法性
	if (strlen(tCmd.session.name) == 0
		|| strlen(tCmd.session.name) >= MAX_ACCNAMESIZE
		|| strlen(tCmd.session.passwd) == 0
		|| strlen(tCmd.session.passwd) >= MAX_PASSWORD)
	{
		LoginReturn(LOGIN_RETURN_PASSWORDERROR);
		return false;
	}

	static const dbCol verifylogin_define[]=
	{
		{ "ACCID",zDBConnPool::DB_DWORD,sizeof(DWORD) },
		{ "PASSWORD",zDBConnPool::DB_STR,sizeof(char[MAX_PASSWORD]) },
		{ "LOGINNAME",zDBConnPool::DB_STR,sizeof(char[MAX_NAMESIZE]) },
		{ "ISUSED",zDBConnPool::DB_BYTE,sizeof(BYTE) },
		{ "ISFORBID",zDBConnPool::DB_BYTE,sizeof(BYTE) },
		{ NULL,0,0}
	};
#pragma pack(1)
	struct
	{
		DWORD accid;
		char  pstrPassword[MAX_PASSWORD];
		char  pstrName[MAX_NAMESIZE];
		BYTE  isUsed,isForbid;
	}
	data;
#pragma pack()
	char where[128];

	//验证用户账号和密码
	bzero(&data,sizeof(data));
	connHandleID handle = FLService::dbConnPool->getHandle();
	if ((connHandleID)-1 == handle)
	{
		LoginReturn(LOGIN_RETURN_DB);
		return false;
	}
	Zebra::logger->info("��½�˺�:%s ׼����ѯ���ݿ�",tCmd.session.name);
	bzero(where,sizeof(where));
	snprintf(where,sizeof(where) - 1,"LOGINNAME = '%s'",tCmd.session.name);
	if (FLService::dbConnPool->exeSelectLimit(handle,"`LOGIN`",verifylogin_define,where,NULL,1,(BYTE*)(&data)) != 1)
	{
		FLService::dbConnPool->putHandle(handle);
		LoginReturn(LOGIN_RETURN_PASSWORDERROR);
		Zebra::logger->error("name:%s ,password error!!!", tCmd.session.name);
		tCmd.session.state = 4;
		return false;
	}
#if 0
#ifdef _MOBILE
	if (strcmp(data.pstrPassword, tCmd.session.passwd) //比对密码
		|| strcmp(data.pstrName, tCmd.session.name))
	{
		Zebra::logger->error("�˺ŵ�½:%s userInutPassword:%s DBPassword:%s",tCmd.session.name, tCmd.session.passwd, data.pstrPassword);
		FLService::dbConnPool->putHandle(handle);
		LoginReturn(LOGIN_RETURN_PASSWORDERROR);
		return false;
	}
	else
	{
	    Zebra::logger->info("�˺ŵ�½:%s userInutPassword:%s DBPassword:%s ����ȶԳɹ�!",tCmd.session.name, tCmd.session.passwd, data.pstrPassword);
	}
#endif
#endif
	//FLService::dbConnPool->updateDatatimeCol(handle,"`LOGIN`","`LASTACTIVEDATE`");
	FLService::dbConnPool->putHandle(handle);

	//账号已经在使用中
	if (data.isUsed)
	{
		LoginReturn(LOGIN_RETURN_IDINUSE);
		Zebra::logger->error("账号正在使用中");    
		return false;
	}
	// 账号已经被禁止
	if (data.isForbid)
	{
		LoginReturn(LOGIN_RETURN_IDINCLOSE);
		Zebra::logger->error("账号已经禁用");
		tCmd.session.state = 1;
		return false;
	}

	tCmd.retcode       = SESSIONCHECK_SUCCESS;
	tCmd.session.state = 0;
	tCmd.session.accid = data.accid;
	LoginManager::getInstance().verifyReturn(tCmd.session.loginTempID,tCmd.retcode,tCmd.session);

	return true;  
}

bool LoginTask::refrainLogin(const Cmd::stUserRefrainLoginCmd *ptCmd)
{
	Zebra::logger->debug("LoginTask::refrainLogin");

	using namespace Cmd;
	using namespace Cmd::DBAccess;

	//生成区唯一编号
	GameZone_t gameZone;
	gameZone.game = ptCmd->game;
	gameZone.zone = ptCmd->zone;
	Zebra::logger->info("����ת���� :gameid=%u(game=%u,zone=%u)",gameZone.id,gameZone.game,gameZone.zone);
	
	connHandleID handle = FLService::dbConnPool->getHandle();
	if ((connHandleID)-1 == handle)
	{
		LoginReturn(LOGIN_RETURN_DB);
		return false;
	}
	DBFieldSet* changeLogin = FLService::metaData->getFields("changeZoneLogin");
	if(!changeLogin)
	{
	    Zebra::logger->error("���ݿ�û�б� changeZoneLogin");
	    FLService::dbConnPool->putHandle(handle);
	    LoginReturn(LOGIN_RETURN_DB);
	    return false;
	}

	DBRecord where;
	std::ostringstream so_accid, so_secretkey, so_game, so_zone, so_mac;
	so_accid<<"accid='"<<ptCmd->accid<<"'";
	so_accid<<"secretkey='"<<ptCmd->secretkey<<"'";
	so_accid<<"game='"<<ptCmd->game<<"'";
	so_accid<<"zone='"<<ptCmd->zone<<"'";

	where.put("accid", so_accid.str());
	where.put("secretkey", so_secretkey.str());
	where.put("game", so_game.str());
	where.put("zone", so_zone.str());

	DBRecordSet* result = FLService::dbConnPool->exeSelect(handle, changeLogin, NULL, &where);
	if(!result)
	{
	    SAFE_DELETE(result);
	    FLService::dbConnPool->putHandle(handle);
	    LoginReturn(LOGIN_RETURN_CHANGE_LOGIN);
	    return false;
	}
	SAFE_DELETE(result);

	DBRecord delwhere;
	delwhere.put("accid", so_accid.str());
	delwhere.put("secretkey", so_secretkey.str());
	FLService::dbConnPool->exeDelete(handle, changeLogin, &delwhere);
	FLService::dbConnPool->putHandle(handle);



	t_ChangeServer_SessionCheck tCmd;
	bzero(&tCmd.session,sizeof(tCmd.session));
	tCmd.session.gameZone    = gameZone;
	tCmd.session.loginTempID = tempid;
	strncpy(tCmd.session.client_ip,getIP(),MAX_IP_LENGTH);    
	strncpy(tCmd.session.name,ptCmd->pstrName,sizeof(tCmd.session.name));

	static const dbCol verifylogin_define[]=
	{
		{ "ACCID",zDBConnPool::DB_DWORD,sizeof(DWORD) },
		{ "PASSWORD",zDBConnPool::DB_STR,sizeof(char[MAX_PASSWORD]) },
		{ "LOGINNAME",zDBConnPool::DB_STR,sizeof(char[MAX_NAMESIZE]) },
		{ "ISUSED",zDBConnPool::DB_BYTE,sizeof(BYTE) },
		{ "ISFORBID",zDBConnPool::DB_BYTE,sizeof(BYTE) },
		{ NULL,0,0}
	};
#pragma pack(1)
	struct
	{
		DWORD accid;
		char  pstrPassword[MAX_PASSWORD];
		char  pstrName[MAX_NAMESIZE];
		BYTE  isUsed,isForbid;
	}
	data;
#pragma pack()

	//验证用户账号和密码
	bzero(&data,sizeof(data));
	handle = FLService::dbConnPool->getHandle();
	if ((connHandleID)-1 == handle)
	{
		LoginReturn(LOGIN_RETURN_DB);
		return false;
	}
	Zebra::logger->info("用户 %s 字符ID登陆",ptCmd->pstrName);
	char selectwhere[128];
	bzero(selectwhere,sizeof(selectwhere));
	snprintf(selectwhere,sizeof(selectwhere) - 1,"LOGINNAME = '%s'",ptCmd->pstrName);
	if (FLService::dbConnPool->exeSelectLimit(handle,"`LOGIN`",verifylogin_define,selectwhere,NULL,1,(BYTE*)(&data)) != 1)
	{
		FLService::dbConnPool->putHandle(handle);
		LoginReturn(LOGIN_RETURN_PASSWORDERROR);
		Zebra::logger->error("name:%s ,password error!!!", ptCmd->pstrName);
		tCmd.session.state = 4;
		return false;
	}
#if 0
	if (strcmp(data.pstrPassword,(char*)ptCmd->pstrPassword) //比对密码
		|| strcmp(data.pstrName,ptCmd->pstrName))
	{
		Zebra::logger->error("%s",data.pstrPassword);
		Zebra::logger->error("%s",ptCmd->pstrPassword);
		FLService::dbConnPool->putHandle(handle);
		LoginReturn(LOGIN_RETURN_PASSWORDERROR);
		Zebra::logger->error("strcmp�ȶԳ��� name:%s ,password error!!!", ptCmd->pstrName);
		return false;
	}
#endif
	//FLService::dbConnPool->updateDatatimeCol(handle,"`LOGIN`","`LASTACTIVEDATE`");
	FLService::dbConnPool->putHandle(handle);

	//账号已经在使用中
	if (data.isUsed)
	{
		LoginReturn(LOGIN_RETURN_IDINUSE);
		Zebra::logger->error("账号正在使用中");    
		return false;
	}
	// 账号已经被禁止
	if (data.isForbid)
	{
		LoginReturn(LOGIN_RETURN_IDINCLOSE);
		Zebra::logger->error("账号已经禁用");
		tCmd.session.state = 1;
		return false;
	}

	tCmd.retcode       = SESSIONCHECK_SUCCESS;
	tCmd.session.state = 0;
	tCmd.session.accid = data.accid;
	LoginManager::getInstance().verifyReturn(tCmd.session.loginTempID,tCmd.retcode,tCmd.session);

	return true;  
}

bool LoginTask::msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#ifdef _MSGPARSE_
	Zebra::logger->error("?? LoginTask::msgParse(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
#endif

	using namespace Cmd;

	stLogonUserCmd *logonCmd = (stLogonUserCmd *)pNullCmd;
	switch(logonCmd->byCmd)
	{
	case LOGON_USERCMD:
		switch(logonCmd->byParam)
		{
		case USER_REQUEST_LOGIN_PARA:
			Zebra::logger->info("USER_REQUEST_LOGIN_PARA");
			if (requestLogin((stUserRequestLoginCmd *)logonCmd)) return true;
			break;
		case REQUEST_CLIENT_IP_PARA:
			{
			    stReturnClientIP send;
			    bcopy(mSocket.getIP(), send.pstrIP, sizeof(send.pstrIP));
#ifdef _MOBILE
			    send.wdPort = mSocket.getPort();
#endif
			    sendCmd(&send, sizeof(send));
			    return true;
			}
			break;
		}
		break;
	}
	Zebra::logger->error("LoginTask::msgParse(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
	return false;
}
