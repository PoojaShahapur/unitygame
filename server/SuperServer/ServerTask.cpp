#include <iostream>
#include <vector>
#include <iterator>
#include <ext/hash_map>

#include "zService.h"
#include "zTCPServer.h"
#include "zTCPTask.h"
#include "Zebra.h"
#include "zString.h"
#include "zDBConnPool.h"
/*f ʵ�ַ�����������
*
* 
*/
#include "FLClient.h"
#include "FLClientManager.h"
//#include "InfoClient.h"
//#include "InfoClientManager.h"
#include "RoleregCache.h"
#include "ServerManager.h"
#include "ServerTask.h"
#include "SuperServer.h"
#include "zType.h"
#include "FLCommand.h"
#include "GmToolCommand.h"
#include "BroadCommand.h"
#include "RoleregClientManager.h"
#include "RoleregCommand.h"
#include "RolechangeClientManager.h"
#include "RolechangeCommand.h"
#include "BroadClientManager.h"

/**
* \brief ��֤һ�������������Ƿ�Ϸ�
*
* ÿһ̨����������Ϣ�����ݿ��ж��м�¼��������ݿ���û����Ӧ�ļ�¼����ô�������������������ǲ��Ϸ��ģ���Ҫ�����Ͽ�����<br>
* ������֤��һ�����еķ�����֮������ι�ϵ
*
* \param wdType ����������
* \param pstrIP ��������ַ
* \return ��֤�Ƿ�ɹ�
*/
bool ServerTask::verify(WORD wdType,const char *pstrIP)
{
	Zebra::logger->info("ServerTask::verify(%s:%u)",pstrIP,wdType);
	char where[64];

	this->wdServerType = wdType;
	strncpy(this->pstrIP,pstrIP,sizeof(this->pstrIP));
	Zebra::logger->info("ServerTask::verify %d,%s",wdType,pstrIP);

	connHandleID handle = SuperService::dbConnPool->getHandle();
	if ((connHandleID)-1 == handle)
	{
		Zebra::logger->error("���ܴ����ݿ����ӳػ�ȡ���Ӿ��");
		return false;
	}

	bzero(where,sizeof(where));
	std::string escapeIP;
	snprintf(where,sizeof(where) - 1,"`TYPE`=%u AND `IP`='%s'",wdType,SuperService::dbConnPool->escapeString(handle,pstrIP,escapeIP).c_str());
	static const dbCol col_define[] =
	{
		{"ID",zDBConnPool::DB_WORD,sizeof(WORD)},
		{"PORT",zDBConnPool::DB_WORD,sizeof(WORD)},
		{NULL,0,0}
	};
	struct
	{
		WORD wdServerID;
		WORD wdPort;
	}
	*pData = NULL;

	DWORD retcode = SuperService::dbConnPool->exeSelect(handle,"`SERVERLIST`",col_define,where,"`ID`",(BYTE **)&pData);
	if (retcode == (DWORD)-1
		|| retcode == 0
		|| NULL == pData)
	{
		Zebra::logger->error("���ݿ���û����Ӧ�ķ�������¼ %u",wdType);
		SuperService::dbConnPool->putHandle(handle);
		return false;
	}

	SuperService::dbConnPool->putHandle(handle);

	//ĳЩ���ͷ�������һ������ֻ����һ̨
	if (retcode > 1
		&& (wdType == BILLSERVER
		|| wdType == SESSIONSERVER)
		)
	{
		SAFE_DELETE_VEC(pData);
		Zebra::logger->error("this type server must only one ,but now has %u in table SERVERLIST",wdType);
		return false;
	}

	//�����ݿ���ȡ���ݳɹ�����Ҫ����Щ������ȡ��һ��������
	DWORD i;
	for(i = 0; i < retcode; i++)
	{
		if (ServerManager::getInstance().uniqueVerify(pData[i].wdServerID))
		{
			wdServerID = pData[i].wdServerID;
			wdPort = pData[i].wdPort;
			break;
		}
	}
	SAFE_DELETE_VEC(pData);
	if (i == retcode)
	{
		Zebra::logger->error("�������Ѿ���������ˣ�û�п��ü�¼");
		return false;
	}

	//���ط�������Ϣ��������
	Cmd::Super::t_Startup_Response tCmd;
	tCmd.wdServerID = wdServerID;
	tCmd.wdPort     = wdPort;
	strncpy(tCmd.pstrIP,pstrIP,sizeof(tCmd.pstrIP));
	if (!sendCmd(&tCmd,sizeof(tCmd)))
	{

		Zebra::logger->error("�����������ָ��ʧ��%u(%u)",wdServerID,wdPort);
		return false;
	}

	return true;
}

static __gnu_cxx::hash_map<int, std::vector<int> > serverSequence;

static void initServerSequence() __attribute__((constructor));
void initServerSequence()
{
	serverSequence[UNKNOWNSERVER]  =  std::vector<int>();
	serverSequence[SUPERSERVER]  =  std::vector<int>();
	serverSequence[LOGINSERVER]  =  std::vector<int>();
	serverSequence[RECORDSERVER]  =  std::vector<int>();
#if 0
	serverSequence[MINISERVER]  =  std::vector<int>();

	int data0[] = { RECORDSERVER };
	serverSequence[SESSIONSERVER]  =  std::vector<int>(data0,data0 + sizeof(data0) / sizeof(int));
	int data1[] = { RECORDSERVER,SESSIONSERVER,MINISERVER};
	serverSequence[SCENESSERVER]  =  std::vector<int>(data1,data1 + sizeof(data1) / sizeof(int));
	int data2[] = { RECORDSERVER,BILLSERVER,SESSIONSERVER,SCENESSERVER,MINISERVER};
	serverSequence[GATEWAYSERVER]  =  std::vector<int>(data2,data2 + sizeof(data2) / sizeof(int));
#endif
	int data0[] = { RECORDSERVER };
	serverSequence[SESSIONSERVER]  =  std::vector<int>(data0,data0 + sizeof(data0) / sizeof(int));
	int data1[] = { RECORDSERVER,SESSIONSERVER};
	serverSequence[SCENESSERVER]  =  std::vector<int>(data1,data1 + sizeof(data1) / sizeof(int));
	int data2[] = { RECORDSERVER,BILLSERVER,SESSIONSERVER,SCENESSERVER};
	serverSequence[GATEWAYSERVER]  =  std::vector<int>(data2,data2 + sizeof(data2) / sizeof(int));


}

/**
* \brief �ȴ�������ָ֤�������֤
*
* ʵ���麯��<code>zTCPTask::verifyConn</code>
*
* \return ��֤�Ƿ�ɹ������߳�ʱ
*/
int ServerTask::verifyConn()
{
	Zebra::logger->debug("ServerTask::verifyConn");
	int retcode = mSocket.recvToBuf_NoPoll();
	if (retcode > 0)
	{
		BYTE pstrCmd[zSocket::MAX_DATASIZE];
		int nCmdLen = mSocket.recvToCmd_NoPoll(pstrCmd,sizeof(pstrCmd));
		if (nCmdLen <= 0)
			return 0;
		else
		{
			Cmd::Super::t_Startup_Request *ptCmd = (Cmd::Super::t_Startup_Request *)pstrCmd;
			if (Cmd::Super::CMD_STARTUP == ptCmd->cmd
				&& Cmd::Super::PARA_STARTUP_REQUEST == ptCmd->para)
			{
				if (verify(ptCmd->wdServerType,ptCmd->pstrIP))
				{
					Zebra::logger->debug("�ͻ�������ͨ����֤(%s:%u)",ptCmd->pstrIP,ptCmd->wdServerType);
					return 1;
				}
				else
				{
					Zebra::logger->error("�ͻ���������֤ʧ��(%s:%u)",ptCmd->pstrIP,ptCmd->wdServerType);
					return -1;
				}
			}
			else
			{
				Zebra::logger->error("�ͻ�������ָ����֤ʧ��(%s:%u)",ptCmd->pstrIP,ptCmd->wdServerType);
				return -1;
			}
		}
	}
	else
		return retcode;
}


/**
* \brief ��֤ĳ�����͵����з������Ƿ���ȫ�������
*
* \param wdType ����������
* \param sv ���������������ɹ��ķ������б�
* \return ��֤�Ƿ�ɹ�
*/
bool ServerTask::verifyTypeOK(const WORD wdType,std::vector<ServerTask *> &sv)
{
	Zebra::logger->info("ServerTask::verifyTypeOK(wdType=%u)",wdType);
	static const dbCol col_define[] =
	{
		{"ID",zDBConnPool::DB_WORD,sizeof(WORD)},
		{NULL,0,0}
	};
	char where[64];
	WORD *ID = NULL;

	if (0 == wdType)
		return true;

	bzero(where,sizeof(where));
	snprintf(where,sizeof(where) - 1,"`TYPE`=%u",wdType);

	connHandleID handle = SuperService::dbConnPool->getHandle();
	if ((connHandleID)-1 == handle)
	{
		Zebra::logger->error("���ܴ����ݿ����ӳػ�ȡ���Ӿ��");
		return false;
	}

	DWORD retcode = SuperService::dbConnPool->exeSelect(handle,"`SERVERLIST`",col_define,where,"`ID`",(BYTE **)&ID);
	if (retcode == (DWORD)-1
		|| retcode == 0
		|| NULL == ID)
	{
		Zebra::logger->error("���ݿ���û����Ӧ�ķ�������¼ %u",wdType);
		SuperService::dbConnPool->putHandle(handle);
		return false;
	}
	SuperService::dbConnPool->putHandle(handle);

	bool retval = true;
	for(DWORD i = 0; i < retcode; i++)
	{
		//�����������Ŀ϶���OK״̬
		ServerTask * pServer = ServerManager::getInstance().getServer(ID[i]);
		if (NULL == pServer)
		{
			retval = false;
			break;
		}
		else
		{
			sv.push_back(pServer);
		}
	}
	SAFE_DELETE_VEC(ID);

	return retval;
}

/**
* \brief ֪ͨ���������ķ�����
* \return ֪ͨ�Ƿ�ɹ�
*/
bool ServerTask::notifyOther(WORD dstID)
{
	Zebra::logger->info("ServerTask::notifyOther(dstID =%u)",dstID);
	using namespace Cmd::Super;

	t_Startup_ServerEntry_NotifyOther Cmd;

	bzero(&Cmd.entry,sizeof(Cmd.entry));
	Cmd.entry.wdServerID = wdServerID;
	Cmd.entry.wdServerType = wdServerType;
	strncpy(Cmd.entry.pstrIP,pstrIP,MAX_IP_LENGTH - 1);
	Cmd.entry.wdPort = wdPort;
	Cmd.entry.state = state;

	for(Container::iterator it = ses.begin(); it != ses.end(); ++it)
	{
		if (dstID == it->first.wdServerID)
		{
			ServerTask * pDst = ServerManager::getInstance().getServer(dstID);
			if (pDst)
			{
				pDst->sendCmd(&Cmd,sizeof(Cmd));
			}
			break;
		}
	}

	return true;
}
/**
* \brief ֪ͨ���������ķ�����
* \return ֪ͨ�Ƿ�ɹ�
*/
bool ServerTask::notifyOther()
{
	Zebra::logger->debug("ServerTask::notifyOther()");
	using namespace Cmd::Super;
	t_Startup_ServerEntry_NotifyOther Cmd;
	bool retval = true;

	bzero(&Cmd.entry,sizeof(Cmd.entry));
	Cmd.entry.wdServerID = wdServerID;
	Cmd.entry.wdServerType = wdServerType;
	strncpy(Cmd.entry.pstrIP,pstrIP,MAX_IP_LENGTH - 1);
	Cmd.entry.wdPort = wdPort;
	Cmd.entry.state = state;

	for(Container::iterator it = ses.begin(); it != ses.end(); ++it)
	{
		Cmd.srcID = it->first.wdServerID;
		// ֪ͨ�����ķ�����
		bool curval = ServerManager::getInstance().broadcastByID(Cmd.srcID,&Cmd,sizeof(Cmd));
		Zebra::logger->info("ServerTask::notifyOther()srcid=%u = %u ?",Cmd.srcID,curval);
		retval &= curval;
	}
	return retval;
}

/**
* \brief �յ�notifyOther�ظ�
* \param wdServerID Ŀ�ķ��������
*/
void ServerTask::responseOther(const WORD wdServerID)
{
	Zebra::logger->info("ServerTask::responseOther(%u)",wdServerID);
	for(Container::iterator it = ses.begin(); it != ses.end(); ++it)
	{
		if (it->first.wdServerID == wdServerID)
		{
			Zebra::logger->debug("�ظ��ɹ� %d",it->first.wdServerID);
			it->second = true;
		}
	}
}

/**
* \brief ֪ͨ�������������ķ�������Ϣ�б�
* \return ֪ͨ�Ƿ�ɹ�
*/
bool ServerTask::notifyMe()
{  
	if (hasNotifyMe) return true;

	Zebra::logger->debug("ServerTask::notifyMe(serverid=%u servertype=%u)",wdServerID,wdServerType);

	using namespace Cmd::Super;

	BYTE pBuffer[zSocket::MAX_DATASIZE];
	t_Startup_ServerEntry_NotifyMe *ptCmd = (t_Startup_ServerEntry_NotifyMe *)pBuffer;
	constructInPlace(ptCmd);
	ptCmd->size = 0;

	//check for notify other response
	Zebra::logger->debug("in super notify %d",ses.size());
	for(Container::iterator it = ses.begin(); it != ses.end(); ++it)
	{
		if (it->second)
		{
			bzero(&ptCmd->entry[ptCmd->size],sizeof(ptCmd->entry[ptCmd->size]));
			ptCmd->entry[ptCmd->size].wdServerID = it->first.wdServerID;
			ptCmd->entry[ptCmd->size].wdServerType = it->first.wdServerType;
			strncpy(ptCmd->entry[ptCmd->size].pstrIP,it->first.pstrIP,MAX_IP_LENGTH - 1);
			ptCmd->entry[ptCmd->size].wdPort = it->first.wdPort;
			ptCmd->entry[ptCmd->size].state = it->first.state;

			ptCmd->size++;
		}
		else
			return false;
	}

	if (sendCmd(ptCmd,sizeof(t_Startup_ServerEntry_NotifyMe) + ptCmd->size * sizeof(ServerEntry)))
		hasNotifyMe = true;

	return hasNotifyMe;
}

/**
* \brief �����������������ϵ��Ҳ��������˳��
* \return �Ƿ������������ķ������Ѿ��������
*/
bool ServerTask::processSequence()
{
	if (hasprocessSequence) return true;

	Zebra::logger->debug("ServerTask::processSequence(wdServerType=%u)",wdServerType);
	using namespace Cmd::Super;

	ses.clear();

	std::vector<int> sequence = serverSequence[wdServerType];

	Zebra::logger->error("sequence.size = %d",sequence.size());
	for(std::vector<int>::const_iterator it = sequence.begin(); it != sequence.end(); it++)
	{    
		std::vector<ServerTask *> sv;
		if (!verifyTypeOK(*it,sv)) return false;
		for(std::vector<ServerTask *>::const_iterator sv_it = sv.begin(); sv_it != sv.end(); sv_it++)
		{
			ServerEntry se;
			bzero(&se,sizeof(se));
			se.wdServerID = (*sv_it)->wdServerID;
			se.wdServerType = (*sv_it)->wdServerType;
			strncpy(se.pstrIP,(*sv_it)->pstrIP,MAX_IP_LENGTH - 1);
			se.wdPort = (*sv_it)->wdPort;
			se.state = (*sv_it)->state;

			//���������ķ����������Լ����������б���,��Ӧ����Ϣ��ɷ�
			ses.insert(Container::value_type(se,false));
		}
	}
	hasprocessSequence = true;
	Zebra::logger->error("ses.size = %d",ses.size());
	return true;
}

/**
* \brief �ȴ��������������
*
* ���������ܵ�������������ص�������Ϣ�����������ĳ�ʼ������ʼ����Ϸ����������ָ����������<br>
* ��������������ܵ�ȷ��������ɵ�ָ��ͻ��������������뵽��������Ķ����У������������������������������<br>
* ʵ�����麯��<code>zTCPTask::waitSync</code>
*
* \return �����������Ƿ�ͨ���������ʱ��Ҫ�����ȴ�
*/
int ServerTask::waitSync()
{
	//Zebra::logger->debug("ServerTask::waitSync");
	int retcode = mSocket.checkIOForRead();
	if (-1 == retcode)
	{
		Zebra::logger->error("ServerTask::waitSync can not read");
		return -1;
	}
	else if (retcode > 0)
	{
		//�׽ӿ�׼�����˽������ݣ��������ݵ����壬�����Դ���ָ��
		retcode = mSocket.recvToBuf_NoPoll();
		if (-1 == retcode)
		{
			Zebra::logger->error("ServerTask::waitSync");
			return -1;
		}
	}

	BYTE pstrCmd[zSocket::MAX_DATASIZE];
	int nCmdLen = mSocket.recvToCmd_NoPoll(pstrCmd,sizeof(pstrCmd));
	if (nCmdLen > 0)
	{
		Cmd::Super::t_Startup_OK *ptCmd = (Cmd::Super::t_Startup_OK *)pstrCmd;    
		Zebra::logger->debug("ServerTask::waitSync nCmdLen=:%d ptCmd->cmd=%u ptCmd->para=%u ptCmd->wdServerID=%u wdServerID=%u", nCmdLen,ptCmd->cmd,ptCmd->para,ptCmd->wdServerID,wdServerID);
		if ((Cmd::Super::CMD_STARTUP == ptCmd->cmd
			&& Cmd::Super::PARA_STARTUP_OK == ptCmd->para
			&& wdServerID == ptCmd->wdServerID)
		    ||(ptCmd->cmd==5 && ptCmd->para==3))
		{
			Zebra::logger->debug("ServerTask::waitSync OK! (%u,%u)",ptCmd->wdServerID,wdServerID);
			return 1;
		}
		else
		{
			Zebra::logger->error("ServerTask::waitSync Fail!(%u,%u)",ptCmd->wdServerID,wdServerID);
			return -1;
		}
	}

	if(checkSequenceTime()
		&& processSequence()
		&& notifyOther())
	    sequenceOK = true;
	if (sequenceOK)
	{
		notifyMe();    
	}
	//�ȴ���ʱ
	return 0;
}

/**
* \brief ȷ��һ�����������ӵ�״̬�ǿ��Ի��յ�
*
* ��һ������״̬�ǿ��Ի��յ�״̬����ô��ζ��������ӵ������������ڽ��������Դ��ڴ��а�ȫ��ɾ���ˣ���<br>
* ʵ�����麯��<code>zTCPTask::recycleConn</code>
*
* \return �Ƿ���Ի���
*/
int ServerTask::recycleConn()
{
	return 1;
}

/**
* \brief ��ӵ�ȫ��������
*
* ʵ�����麯��<code>zTCPTask::addToContainer</code>
*
*/
void ServerTask::addToContainer()
{

	Zebra::logger->debug("ServerTask::addToContainer %u",getPort());
	ServerManager::getInstance().addServer(this);
}

/**
* \brief ��ȫ��������ɾ��
*
* ʵ�����麯��<code>zTCPTask::removeToContainer</code>
*
*/
void ServerTask::removeFromContainer()
{
	Zebra::logger->debug("ServerTask::removeFromContainer");
	//��������ط������رգ�����֪ͨ���еĵ�½���������عر�
	if (GATEWAYSERVER == wdServerType)
	{
		Cmd::FL::t_GYList_FL tCmd;

		tCmd.wdServerID = wdServerID;
		bzero(tCmd.pstrIP,sizeof(tCmd.pstrIP));
		tCmd.wdPort = 0;
		tCmd.wdNumOnline = 0;
		tCmd.state = state_maintain;
		tCmd.zoneGameVersion = 0;

		FLClientManager::getInstance().broadcast(&tCmd,sizeof(tCmd));
	}

	ServerManager::getInstance().removeServer(this);
}

/**
* \brief ��ӵ�Ψһ����֤������
*
* ʵ�����麯��<code>zTCPTask::uniqueAdd</code>
*
*/
bool ServerTask::uniqueAdd()
{
	Zebra::logger->debug("ServerTask::uniqueAdd");
	return ServerManager::getInstance().uniqueAdd(this);
}

/**
* \brief ��Ψһ����֤������ɾ��
*
* ʵ�����麯��<code>zTCPTask::uniqueRemove</code>
*
*/
bool ServerTask::uniqueRemove()
{
	Zebra::logger->debug("ServerTask::uniqueRemove");
	return ServerManager::getInstance().uniqueRemove(this);
}

/**
* \brief �������Թ���������Ĺ���������ָ��
*
* \param pNullCmd �������ָ��
* \param nCmdLen ָ���
* \return �����Ƿ�ɹ�
*/
bool ServerTask::msgParse_Startup(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
	Zebra::logger->debug("ServerTask::msgParse_Startup");
	using namespace Cmd::Super;

	switch(pNullCmd->para)
	{
	case PARA_STARTUP_SERVERENTRY_NOTIFYOTHER://1,4
		{    
			// ��Ӧ�����ķ�������NOTIFYOTHER��Ӧ��Ϣ    
			t_Startup_ServerEntry_NotifyOther *ptCmd = (t_Startup_ServerEntry_NotifyOther *)pNullCmd;  
			//       
			ServerManager::getInstance().responseOther(ptCmd->entry.wdServerID,ptCmd->srcID);

			return true;
		}
		break;
	case PARA_RESTART_SERVERENTRY_NOTIFYOTHER:
		{
			t_restart_ServerEntry_NotifyOther *notify = (t_restart_ServerEntry_NotifyOther*)pNullCmd;
			ServerTask * pSrc = ServerManager::getInstance().getServer(notify->srcID);
			if (pSrc)
			{
				pSrc->notifyOther(notify->dstID);
				return true;
			}
		}
		break;
	default:
		break;
	}

	Zebra::logger->error("ServerTask::msgParse_Startup(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
	return false;
}

/**
* \brief �������ԼƷѷ�������ָ��
*
* \param pNullCmd �������ָ��
* \param nCmdLen ָ���
* \return �����Ƿ�ɹ�
*/
bool ServerTask::msgParse_Bill(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
	using namespace Cmd::Super;
	switch(pNullCmd->para)
	{
	case PARA_BILL_IDINUSE:
		{
			t_idinuse_Bill *ptCmd = (t_idinuse_Bill *)pNullCmd;
			Cmd::FL::t_idinuse_Session tCmd;

			tCmd.accid = ptCmd->accid;
			tCmd.loginTempID = ptCmd->loginTempID;
			tCmd.wdLoginID = ptCmd->wdLoginID;
			bcopy(ptCmd->name,tCmd.name,sizeof(tCmd.name));
			FLClientManager::getInstance().sendTo(tCmd.wdLoginID,&tCmd,sizeof(tCmd));

			return true;
		}
		break;
	}
	Zebra::logger->error("ServerTask::msgParse_Bill(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
	return false;
}

/**
* \brief �����������ط�������ָ��
*
* \param pNullCmd �������ָ��
* \param nCmdLen ָ���
* \return �����Ƿ�ɹ�
*/
bool ServerTask::msgParse_Gateway(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
	Zebra::logger->debug("ServerTask::msgParse_Gateway(%u,%u)",pNullCmd->cmd,pNullCmd->para);
	using namespace Cmd::Super;

	switch(pNullCmd->para)
	{
	case PARA_GATEWAY_GYLIST:
		{
			t_GYList_Gateway *ptCmd = (t_GYList_Gateway *)pNullCmd;
			Cmd::FL::t_GYList_FL tCmd;


			//֪ͨ����,�Ѿ��յ�����ע����Ϣ
			t_notifyFinish_Gateway gCmd;
			sendCmd(&gCmd,sizeof(gCmd));

			OnlineNum = ptCmd->wdNumOnline;

			Zebra::logger->info("GYList:%d,%s:%d,onlines=%u,state=%d",ptCmd->wdServerID,ptCmd->pstrIP,ptCmd->wdPort,ptCmd->wdNumOnline,ptCmd->state);
			strncpy(tCmd.pstrIP,ptCmd->pstrIP,sizeof(tCmd.pstrIP));
			tCmd.wdServerID      = ptCmd->wdServerID;
			tCmd.wdPort          = ptCmd->wdPort;
			tCmd.wdNumOnline     = ptCmd->wdNumOnline;
			tCmd.state           = ptCmd->state;
			tCmd.zoneGameVersion = ptCmd->zoneGameVersion;
			FLClientManager::getInstance().broadcast(&tCmd,sizeof(tCmd));

			return true;
		}
		break;
	case PARA_GATEWAY_NEWSESSION:
		{
			t_NewSession_Gateway *ptCmd = (t_NewSession_Gateway *)pNullCmd;
			Cmd::FL::t_NewSession_Session tCmd;

			tCmd.session = ptCmd->session;
			//bcopy(&ptCmd->session,&tCmd.session,sizeof(tCmd.session));
			FLClientManager::getInstance().sendTo(tCmd.session.wdLoginID,&tCmd,sizeof(tCmd));

			return true;
		}
		break;
	case PARA_CHARNAME_GATEWAY:
		{
			t_Charname_Gateway *ptCmd = (t_Charname_Gateway *)pNullCmd;        
#if 0
			//�����Ƿ���ü����������ɣ�����Ҫ������������
			if (!RoleregCache::getInstance().msgParse_loginServer(ptCmd->wdServerID,ptCmd->accid,ptCmd->name,ptCmd->state))
			{
				if (ptCmd->state & ROLEREG_STATE_TEST)
				{
					Zebra::logger->error("����ʧ�ܣ�����������ɫ��%u,%s",ptCmd->accid,ptCmd->name);
					t_Charname_Gateway tCmd;
					bcopy(ptCmd,&tCmd,sizeof(tCmd));
					tCmd.state |= ROLEREG_STATE_HAS;  //�����Ѿ����ڱ�־
					sendCmd(&tCmd,sizeof(tCmd));
				}
				if (ptCmd->state & (ROLEREG_STATE_WRITE | ROLEREG_STATE_CLEAN))
				{
					RoleregCache::getInstance().add(*ptCmd);
				}
			}
#endif
			Cmd::RoleReg::t_Charname_reg_withID cmd;
			cmd.regType = ptCmd->regType;
			cmd.wdServerID = ptCmd->wdServerID;
			cmd.accid = ptCmd->accid;
			cmd.charid = ptCmd->charid;
			cmd.gameZone = SuperService::getInstance().getZoneID();
			bcopy(ptCmd->name, cmd.name, sizeof(cmd.name));
			cmd.state = ptCmd->state;
			Zebra::logger->debug("��ɫ����ָ�%u,%u,%s,%u",ptCmd->accid,ptCmd->charid,ptCmd->name,ptCmd->state);
			if(!RoleregClientManager::getInstance().broadcastOne(&cmd, sizeof(cmd)))
			{
			    if(ptCmd->state & ROLEREG_STATE_WRITE)
			    {
					Zebra::logger->error("����ʧ�ܣ�����������ɫ��%u,%s",ptCmd->accid,ptCmd->name);
					t_Charname_Gateway tCmd;
					bcopy(ptCmd,&tCmd,sizeof(tCmd));
					tCmd.state |= ROLEREG_STATE_WRITE;  //���ò��ɹ�
					sendCmd(&tCmd,sizeof(tCmd));
			    }
			}
			return true;
		}
		break;
	}
	
	Zebra::logger->error("ServerTask::msgParse_Gateway(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
	return false;
}

/**
* \brief ����GM���ߵ�ָ��
*
* \param pNullCmd �������ָ��
* \param nCmdLen ָ���
* \return �����Ƿ�ɹ�
*/
bool ServerTask::msgParse_GmTool(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
	using namespace Cmd::GmTool;
	switch(pNullCmd->para)
	{
	case PARA_LOG_GMTOOL:
		{
			t_Log_GmTool * rev = (t_Log_GmTool *)pNullCmd;
			rev->zone = SuperService::getInstance().getZoneID();
			////---InfoClientManager::getInstance().broadcastOne(rev,nCmdLen);
			return true;
		}
		break;
	case PARA_CHAT_GMTOOL:
		{
			t_Chat_GmTool * rev = (t_Chat_GmTool *)pNullCmd;
			rev->zone = SuperService::getInstance().getZoneID();
			strncpy(rev->server,SuperService::getInstance().getZoneName().c_str(),MAX_NAMESIZE);

			////---InfoClientManager::getInstance().broadcastOne(rev,nCmdLen);
			/*
			if (ret)
			Zebra::logger->debug("[GM����]ת��������Ϣ��InfoServer,%s ��(%u)",rev->userName,rev->zone.id);
			else
			Zebra::logger->debug("[GM����]ת��������Ϣ��InfoServerʧ��,%s ��(%u)",rev->userName,rev->zone.id);
			*/
			return true;
		}
		break;
	case PARA_MSG_GMTOOL:
		{
			t_Msg_GmTool * rev = (t_Msg_GmTool *)pNullCmd;
			rev->zone = SuperService::getInstance().getZoneID();
			//strncpy(rev->server,SuperService::getInstance().getZoneName().c_str(),MAX_NAMESIZE);
			////---InfoClientManager::getInstance().broadcastOne(rev,sizeof(t_Msg_GmTool));
			/*
			if (ret)
			Zebra::logger->debug("[GM����]ת��GM������InfoServer,%s ��(%u)",rev->userName,rev->zone.id);
			else
			Zebra::logger->debug("[GM����]ת��GM������InfoServerʧ��,%s ��(%u)",rev->userName,rev->zone.id);
			*/
			return true;
		}
		break;
	case PARA_NEW_MSG_GMTOOL:
		{
			t_NewMsg_GmTool * rev = (t_NewMsg_GmTool *)pNullCmd;
			rev->zone = SuperService::getInstance().getZoneID();
			////---InfoClientManager::getInstance().broadcastOne(rev,sizeof(t_NewMsg_GmTool));
			/*
			if (ret)
			Zebra::logger->debug("[GM����]ת��GM������InfoServer,%s ��(%u)",rev->userName,rev->zone.id);
			else
			Zebra::logger->debug("[GM����]ת��GM������InfoServerʧ��,%s ��(%u)",rev->userName,rev->zone.id);
			*/
			return true;
		}
		break;
	case PARA_PUNISH_GMTOOL:
		{
			t_Punish_GmTool * rev = (t_Punish_GmTool *)pNullCmd;
			rev->zone = SuperService::getInstance().getZoneID();
			strncpy(rev->server,SuperService::getInstance().getZoneName().c_str(),MAX_NAMESIZE);
			////---InfoClientManager::getInstance().broadcastOne(rev,sizeof(t_Punish_GmTool));
			/*
			if (ret)
			Zebra::logger->debug("[GM����]ת��GM������InfoServer,%s ��(%u)",rev->userName,rev->zone.id);
			else
			Zebra::logger->debug("[GM����]ת��GM������InfoServerʧ��,%s ��(%u)",rev->userName,rev->zone.id);
			*/
			return true;
		}
		break;
	}
	return false;
}

bool ServerTask::msgParse_CountryOnline(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#if 0
	using namespace Cmd::Super;
	switch(pNullCmd->para)
	{
	case PARA_COUNTRYONLINE:
		{
			t_CountryOnline * rev = (t_CountryOnline *)pNullCmd;
			BYTE pBuffer[zSocket::MAX_DATASIZE];
			Cmd::Info::t_Country_OnlineNum *cmd = (Cmd::Info::t_Country_OnlineNum *)pBuffer;
			constructInPlace(cmd);
			cmd->rTimestamp = rev->rTimestamp;
			cmd->GameZone = SuperService::getInstance().getZoneID();
			cmd->OnlineNum = rev->OnlineNum;

			for(DWORD i = 0; i < cmd->OnlineNum; i++)
			{
				cmd->CountryOnline[i].country = rev->CountryOnline[i].country;
				cmd->CountryOnline[i].num = rev->CountryOnline[i].num;
			}
			return true;////---InfoClientManager::getInstance().sendTo(rev->infoTempID,cmd,sizeof(Cmd::Info::t_Country_OnlineNum) + cmd->OnlineNum * sizeof(Cmd::Info::t_Country_OnlineNum::Online));
		}
		break;
	}
#endif
	return false;
}

/**
* \brief �������Ը������������ӵ�ָ��
*
* \param pNullCmd �������ָ��
* \param nCmdLen ָ���
* \return �����Ƿ�ɹ�
*/
bool ServerTask::msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#ifdef _MSGPARSE_
	Zebra::logger->error("?? ServerTask::msgParse(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
#endif

	switch(pNullCmd->cmd)
	{
	case Cmd::Super::CMD_STARTUP:
		if (msgParse_Startup(pNullCmd,nCmdLen))
		{
			return true;
		}
		break;
	case Cmd::Super::CMD_BILL:
		if (msgParse_Bill(pNullCmd,nCmdLen))
		{
			return true;
		}
		break;
	case Cmd::Super::CMD_GATEWAY:
		if (msgParse_Gateway(pNullCmd,nCmdLen))
		{
			return true;
		}
		break;
	case Cmd::GmTool::CMD_GMTOOL:
		if (msgParse_GmTool(pNullCmd,nCmdLen))
		{
			return true;
		}
		break;
	case Cmd::Super::CMD_COUNTRYONLINE:
		if (msgParse_CountryOnline(pNullCmd,nCmdLen))
		{
			return true;
		}
		break;
	case Cmd::Super::CMD_SESSION:
		{
		    switch(pNullCmd->para)
		    {
			case Cmd::Super::PARA_SHUTDOWN:
			    {
				SuperService::getInstance().Terminate();
				Zebra::logger->info("Session����ͣ��ά��");
				return true;
			    }
			    break;
			case Cmd::Super::PARA_USER_ONLINE_BROADCAST:
			    {
				BUFFER_CMD(Cmd::BroadCast::t_BroadcastMessage, send, zSocket::MAX_USERDATASIZE);
				send->srcZone = SuperService::getInstance().getZoneID();
				send->destZone.game = 10;
				send->dataLen = nCmdLen;
				strncpy(send->data, (const char*)pNullCmd, nCmdLen);
				BroadClientManager::getInstance().broadcastOne(send, sizeof(Cmd::BroadCast::t_BroadcastMessage)+send->dataLen);
				Zebra::logger->debug("[�㲥��Ϣ]׼�������㲥������ ��:%u", send->srcZone.zone);
				return true;
			    }
			    break;
		    }
		}
		break;
	case Cmd::Super::CMD_SCENE:
		{
		    if(msgParse_Scene(pNullCmd, nCmdLen))
			return true;
		}
		break;
	case Cmd::Rolechange::CMD_BATTLE:
		{
		    if(msgParse_Battle(pNullCmd, nCmdLen))
			return true;
		}
		break;
	}
	Zebra::logger->error("ServerTask::msgParse(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
	return false;
}


const char* ServerTask::GetServerTypeName(const WORD wdServerType)
{
	//UNKNOWNSERVER  =  0, /** δ֪���������� */
	//	SUPERSERVER      =  1, /** ��������� */
	//	LOGINSERVER     =  10, /** ��½������ */
	//	RECORDSERVER  =  11, /** ���������� */
	//	BILLSERVER      =  12, /** �Ʒѷ����� */
	//	SESSIONSERVER  =  20, /** �Ự������ */
	//	SCENESSERVER  =  21, /** ���������� */
	//	GATEWAYSERVER  =  22, /** ���ط����� */
	//	MINISERVER      =  23    /** С��Ϸ������ */
	switch(wdServerType)
	{
	case UNKNOWNSERVER:
		return "δ֪����������";
	case SUPERSERVER:
		return "���������";
	case LOGINSERVER:
		return "��½������";
	case RECORDSERVER:
		return "����������";
	case BILLSERVER:
		return "�Ʒѷ�����";
	case SESSIONSERVER:
		return "�Ự������";
	case SCENESSERVER:
		return "����������";
	case GATEWAYSERVER:
		return "���ط�����";
	case MINISERVER:
		return "С��Ϸ������";
	default:
		return "�������������";
	}
	return NULL;
}

bool ServerTask::msgParse_Battle(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
	Zebra::logger->debug("ServerTask::msgParse_Gateway(%u,%u)",pNullCmd->cmd,pNullCmd->para);
	using namespace Cmd::Super;
	using namespace Cmd::Rolechange;
	switch(pNullCmd->para)
	{
	    case PARA_RET_SEND_USER_TOZONE:
		{
		    t_retSendUserToZone* rev = (t_retSendUserToZone*)pNullCmd;
		    rev->fromGameZone = SuperService::getInstance().getZoneID();
		    if(RolechangeClientManager::getInstance().broadcastOne(rev, nCmdLen))
		    {//log ok
		    }
		    else
		    {//log fail
		    }
		    return true;
		}
		break;
	    case PARA_SEND_USER_TOZONE:
		{
		    t_sendUserToZone* rev = (t_sendUserToZone*)pNullCmd;
		    rev->fromGameZone = SuperService::getInstance().getZoneID();
		    if(!RolechangeClientManager::getInstance().broadcastOne(rev, nCmdLen))
		    {
			t_retSendUserToZone send;
			send.fromGameZone.zone = rev->toGameZone.zone;
			send.accid = rev->accid;
			send.userid = rev->userid;
			send.state = TOZONE_FAIL;
			send.type = rev->type;
			//log fail

			ServerManager::getInstance().broadcastByType(RECORDSERVER, &send, sizeof(send));
		    }
		    else
		    {//log ok

		    }
		    return true;
		}
		break;;
	    case PARA_CHECK_VALID:
		{
		    t_Check_Valid* rev = (t_Check_Valid*)pNullCmd;
		    rev->fromGameZone = SuperService::getInstance().getZoneID();
		    if(rev->toGameZone == rev->fromGameZone)
		    {
			return true;
		    }

		    if(RolechangeClientManager::getInstance().broadcastOne(rev, nCmdLen))
		    {//log ok
			Zebra::logger->debug("[����] �� roleChangeServer ���� from=%u to=%u accid=%u �ɹ�",rev->fromGameZone.zone, rev->toGameZone.zone, rev->accid);
		    }
		    else
		    {//log fail
			Zebra::logger->error("[����] �� roleChangeServer ���� from=%u to=%u accid=%u ʧ��",rev->fromGameZone.zone, rev->toGameZone.zone, rev->accid);
		    }
		    return true;
		    
		}
		break;
	    case PARA_RET_CHECK_VALID:	//�յ���������������֤������Ϣ,ת����ת��������
		{
		    t_Ret_Check_Valid* rev = (t_Ret_Check_Valid*)pNullCmd;
		    rev->fromGameZone = SuperService::getInstance().getZoneID();
		    if(RolechangeClientManager::getInstance().broadcastOne(rev, nCmdLen))
		    {//log ok
			Zebra::logger->debug("[����] �յ����� RECORDSERVER ��������֤������Ϣ ,ת����roleChangeServerʱ�ɹ�");
		    }
		    else
		    {//log fail
			Zebra::logger->debug("[����] �յ����� RECORDSERVER ��������֤������Ϣ ,ת����roleChangeServerʱʧ��");
		    }
		    return true;
		}
		break;
	    default:
		break;
	}
	return false;
}

bool ServerTask::msgParse_Scene(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
    using namespace Cmd::Super;
    switch(pNullCmd->para)
    {
	case PARA_FORWARD_MSG_SCENE:
	    {
		t_ForwardMsg_Scene* rev = (t_ForwardMsg_Scene*)pNullCmd;
		BYTE buf[zSocket::MAX_DATASIZE];
		bzero(buf, sizeof(buf));
		Cmd::Rolechange::t_ForwardMsg_CommonCmd *send =  (Cmd::Rolechange::t_ForwardMsg_CommonCmd *)buf;
		constructInPlace(send);
		send->fromGameZone = SuperService::getInstance().getZoneID();
		send->toGameZone = rev->toGameZone;
		send->size = rev->size;
		bcopy(rev->msg, send->msg, rev->size);
		RoleregClientManager::getInstance().broadcastOne(send, sizeof(Cmd::Rolechange::t_ForwardMsg_CommonCmd)+rev->size);
		Zebra::logger->debug("[����] ���� from=%u to=%u size=%u",send->fromGameZone.id, send->toGameZone.id, send->size);
		return true;
	    }
	    break;
    }
    return false;
}
