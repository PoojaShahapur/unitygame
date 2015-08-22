/**
* \brief ʵ������������Ŀ�ܴ���
*
* 
*/

#include "zSubNetService.h"


zSubNetService *zSubNetService::subNetServiceInst = NULL;

/**
* \brief ��������������ӿͻ�����
*
*/
class SuperClient : public zTCPBufferClient
{

public:

	friend class zSubNetService;

	/**
	* \brief ���캯��
	*
	*/
	SuperClient() : zTCPBufferClient("����������ͻ���"),verified(false)
	{
		//Zebra::logger->debug("SuperClient::SuperClient");
	}

	/**
	* \brief ��������
	*
	*/
	~SuperClient() {};

	void run();
	bool msgParse_Startup(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
	bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

private:

	bool verified;      /**< �Ƿ��Ѿ�ͨ���˹������������֤ */

};

/**
* \brief ����zThread�еĴ��麯��,���̵߳����ص�����,���ڴ�����յ���ָ��
*
*/
void SuperClient::run()
{
	zTCPBufferClient::run();
	//����������֮������ӶϿ�,��Ҫ�رշ�����
	zSubNetService::subNetServiceInstance()->Terminate();
}

/**
* \brief �������Թ���������Ĺ���������ָ��
*
* \param pNullCmd �������ָ��
* \param nCmdLen ָ���
* \return �����Ƿ�ɹ�
*/
bool SuperClient::msgParse_Startup(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
	using namespace Cmd::Super;

	//Zebra::logger->error("cmd from super");

	switch(pNullCmd->para)
	{
	case PARA_GAMETIME:
		{
			t_GameTime *ptCmd = (t_GameTime *)pNullCmd;

			//Zebra::logger->error("PARA_GAMETIME %lu",ptCmd->qwGameTime);
			Zebra::qwGameTime = ptCmd->qwGameTime;
			return true;
		}
		break;
	case PARA_STARTUP_RESPONSE:
		{
			t_Startup_Response *ptCmd = (t_Startup_Response *)pNullCmd;

			Zebra::logger->debug("super server return msg,PARA_STARTUP_RESPONSE %d,%d",ptCmd->wdServerID,ptCmd->wdPort);
			zSubNetService::subNetServiceInstance()->setServerInfo(ptCmd);
			return true;
		}

		break;
	case PARA_STARTUP_SERVERENTRY_NOTIFYME:
		{
			t_Startup_ServerEntry_NotifyMe *ptCmd = (t_Startup_ServerEntry_NotifyMe *)pNullCmd;

			//�����������
			Zebra::logger->debug("PARA_STARTUP_SERVERENTRY_NOTIFYME size = %d ",ptCmd->size );
			for(WORD i = 0; i < ptCmd->size; i++)
			{
				//��Ҫһ��������������Щ�������б�
				zSubNetService::subNetServiceInstance()->addServerEntry(ptCmd->entry[i]);
			}
			if(!verified)
			    verified = true;
			return true;
		}

		break;
	case PARA_STARTUP_SERVERENTRY_NOTIFYOTHER:
		{
			t_Startup_ServerEntry_NotifyOther *ptCmd = (t_Startup_ServerEntry_NotifyOther *)pNullCmd;

			//Zebra::logger->error("PARA_STARTUP_SERVERENTRY_NOTIFYOTHER");
			//��Ҫһ��������������Щ�������б�
			zSubNetService::subNetServiceInstance()->addServerEntry(ptCmd->entry);
			// �������SuperServer
			return sendCmd(ptCmd,nCmdLen);
		}
		break;
	}

	Zebra::logger->error("SuperClient::msgParse_Startup(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
	return false;
}

/**
* \brief �������Թ����������ָ��
*
* \param pNullCmd �������ָ��
* \param nCmdLen ָ���
* \return �����Ƿ�ɹ�
*/
bool SuperClient::msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#ifdef _MSGPARSE_
	Zebra::logger->error("?? SuperClient::msgParse(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
#endif

	switch(pNullCmd->cmd)
	{
	case Cmd::Super::CMD_STARTUP:
		if (msgParse_Startup(pNullCmd,nCmdLen)) return true;
		break;
	default:
		if (zSubNetService::subNetServiceInstance()->msgParse_SuperService(pNullCmd,nCmdLen)) return true;
		break;
	}

	Zebra::logger->error("SuperClient::msgParse(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
	return false;
}

/**
* \brief ���캯��
* 
* \param name ����
* \param wdType ����������
*/
zSubNetService::zSubNetService(const std::string &name,const WORD wdType) : zNetService(name),superClient(NULL)
{
	subNetServiceInst = this;
	
	superClient  = new SuperClient();
	wdServerID   = 0;
	wdServerType = wdType;
	bzero(pstrIP,sizeof(pstrIP));
	wdPort        = 0;
}

/**
* \brief ����������
*
*/
zSubNetService::~zSubNetService()
{
	Zebra::logger->debug("zSubNetService::~zSubNetService");
	serverList.clear();

	SAFE_DELETE(superClient);

	subNetServiceInst = NULL;
}

/**
* \brief ��ʼ���������������
*
* ʵ�ִ��麯��<code>zService::init</code>
* ���������������������,���õ���������Ϣ
*
* \return �Ƿ�ɹ�
*/
bool zSubNetService::init()
{
	Zebra::logger->debug("zSubNetService::init");
	//���������������������if (!superClient->connect(superIP, superPort))
	if (!superClient->connect(Zebra::global["server"].c_str(), atoi(Zebra::global["port"].c_str())))
	{
	    Zebra::logger->error("connect superserver ERROR!!!!!!!!");
	    return false;
	}

	//���͵�½�����������ָ��
	Cmd::Super::t_Startup_Request tCmd;
	tCmd.wdServerType = wdServerType;
	strcpy(tCmd.pstrIP, pstrIP);
	if (!superClient->sendCmd(&tCmd,sizeof(tCmd)))
	{
		Zebra::logger->error("send msg to superserver ERROR!!!!!!!!");
		return false;
	}


	//�ȴ����������������Ϣ
	while(!superClient->verified)
	{
		BYTE pstrCmd[zSocket::MAX_DATASIZE];
		int nCmdLen = superClient->pSocket->recvToCmd(pstrCmd,sizeof(pstrCmd),true);    
		if (-1 == nCmdLen)
		{
		    Zebra::logger->error("wait for superserver response ERROR!!!!!!!!");
			return false;
		}
		else if (nCmdLen > 0)
		{
			if (!superClient->msgParse((Cmd::t_NullCmd *)pstrCmd,nCmdLen))
			{
				return false;
			}
		}
	}

	//Zebra::logger->info("zSubNetService::init %d,%d,%s:%d",wdServerType,wdServerID,pstrIP,wdPort);

	std::ostringstream so;
	so << Zebra::logger->getName() << "[" <<wdServerID << "]";
	Zebra::logger->setName(so.str());

	//�����߳���������������
	superClient->start();

	//������ʵ�ĳ�ʼ������
	return zNetService::init(wdPort);
}

/**
* \brief ȷ�Ϸ�������ʼ���ɹ�,�����������ص�����
*
* �����������t_Startup_OKָ����ȷ�Ϸ����������ɹ�
*
* \return ȷ���Ƿ�ɹ�
*/
bool zSubNetService::validate()
{
	Cmd::Super::t_Startup_OK tCmd;

	Zebra::logger->debug("zSubNetService::validate");  
	tCmd.wdServerID = wdServerID;
	bool OK = false;

	OK = superClient->sendCmd(&tCmd,sizeof(tCmd));
	Zebra::logger->debug("zSubNetService::validate isOk=%u",OK?1:0);
	return OK;
}

/**
* \brief �������������
*
* ʵ�ִ��麯��<code>zService::final</code>
*
*/
void zSubNetService::final()
{
	Zebra::logger->debug("zSubNetService::final");
	zNetService::final();

	//�رյ����������������
	superClient->final();
	superClient->join();
	superClient->close();
}

/**
* \brief ��������������ָ��
*
* \param pstrCmd �����͵�ָ��
* \param nCmdLen ������ָ��Ĵ�С
* \return �����Ƿ�ɹ�
*/
bool zSubNetService::sendCmdToSuperServer(const void *pstrCmd,const int nCmdLen)
{
	//Zebra::logger->debug("zSubNetService::sendCmdToSuperServer");
	return superClient->sendCmd(pstrCmd,nCmdLen);
}

/**
* \brief ���ݹ��������������Ϣ,���÷���������Ϣ
*
* \param ptCmd ���������������Ϣ
*/
void zSubNetService::setServerInfo(const Cmd::Super::t_Startup_Response *ptCmd)
{  
	wdServerID = ptCmd->wdServerID;
	wdPort     = ptCmd->wdPort;
	strncpy(pstrIP,ptCmd->pstrIP,sizeof(pstrIP)); 
	//Zebra::logger->info("zSubNetService::setServerInfo(%d,%s:%d) %s",wdServerID,ptCmd->pstrIP,wdPort,pstrIP);
}

/**
* \brief ��ӹ�����������Ϣ��һ��������
*
*/
void zSubNetService::addServerEntry(const Cmd::Super::ServerEntry &entry)
{
	mlock.lock();
	//���Ȳ�����û���ظ���
	std::deque<Cmd::Super::ServerEntry>::iterator it;
	bool found = false;
	for(it = serverList.begin(); it != serverList.end(); it++)
	{
		if (entry.wdServerID == it->wdServerID)
		{
			found = true;
			break;
		}
	}

	if (found)
	{
		//�Ѿ�����ֻ�Ǹ���
		(*it) = entry;
	}
	else
	{
		//��������,��Ҫ�½���һ���ڵ�
		serverList.push_back(entry);
	}
	mlock.unlock();
}

/**
* \brief ������ط�������Ϣ
*
* \param wdServerID ���������
* \return ��������Ϣ
*/
const Cmd::Super::ServerEntry *zSubNetService::getServerEntryById(const WORD wdServerID)
{
	Zebra::logger->debug("zSubNetService::getServerEntryById(%d)",wdServerID);
	Cmd::Super::ServerEntry *ret = NULL;
	std::deque<Cmd::Super::ServerEntry>::iterator it;
	mlock.lock();
	for(it = serverList.begin(); it != serverList.end(); it++)
	{
		if (wdServerID == it->wdServerID)
		{
			ret = &(*it);
			break;
		}
	}
	mlock.unlock();
	return ret;
}

/**
* \brief ������ط�������Ϣ
*
* \param wdServerType ����������
* \return ��������Ϣ
*/
const Cmd::Super::ServerEntry *zSubNetService::getServerEntryByType(const WORD wdServerType)
{
	Zebra::logger->debug("zSubNetService::getServerEntryByType(type=%d)",wdServerType);
	Cmd::Super::ServerEntry *ret = NULL;
	std::deque<Cmd::Super::ServerEntry>::iterator it;
	mlock.lock();
	for(it = serverList.begin(); it != serverList.end(); it++)
	{
		Zebra::logger->debug("��������Ϣ��%d,%d",wdServerType,it->wdServerType);
		if (wdServerType == it->wdServerType)
		{
			ret = &(*it);
			break;
		}
	}
	mlock.unlock();
	return ret;
}

/**
* \brief ������ط�������Ϣ
*
* \param wdServerType ����������
* \param prev ��һ����������Ϣ
* \return ��������Ϣ
*/
const Cmd::Super::ServerEntry *zSubNetService::getNextServerEntryByType(const WORD wdServerType,const Cmd::Super::ServerEntry **prev)
{
	Zebra::logger->debug("zSubNetService::getNextServerEntryByType");
	Cmd::Super::ServerEntry *ret = NULL;
	bool found = false;
	std::deque<Cmd::Super::ServerEntry>::iterator it;
	mlock.lock();
	for(it = serverList.begin(); it != serverList.end(); it++)
	{
		Zebra::logger->debug("��������Ϣ��%d,%d",wdServerType,it->wdServerType);
		if (wdServerType == it->wdServerType)
		{
			if (NULL == prev
				|| found)
			{
				ret = &(*it);
				break;
			}
			else if (!found
				&& (*prev)->wdServerID == it->wdServerID)
			{
				found = true;
			}
		}
	}
	mlock.unlock();
	return ret;
}

