/**
* \brief ÊµÏÖÍøÂç·şÎñÆ÷µÄ¿ò¼Ü´úÂë
*
* 
*/

#include "zSubNetService.h"


zSubNetService *zSubNetService::subNetServiceInst = NULL;

/**
* \brief ¹ÜÀí·şÎñÆ÷µÄÁ¬½Ó¿Í»§¶ËÀà
*
*/
class SuperClient : public zTCPBufferClient
{

public:

	friend class zSubNetService;

	/**
	* \brief ¹¹Ôìº¯Êı
	*
	*/
	SuperClient() : zTCPBufferClient("¹ÜÀí·şÎñÆ÷¿Í»§¶Ë"),verified(false)
	{
		//Zebra::logger->debug("SuperClient::SuperClient");
	}

	/**
	* \brief Îö¹¹º¯Êı
	*
	*/
	~SuperClient() {};

	void run();
	bool msgParse_Startup(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
	bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

private:

	bool verified;      /**< ÊÇ·ñÒÑ¾­Í¨¹ıÁË¹ÜÀí·şÎñÆ÷µÄÑéÖ¤ */

};

/**
* \brief ÖØÔØzThreadÖĞµÄ´¿Ğéº¯Êı,ÊÇÏß³ÌµÄÖ÷»Øµ÷º¯Êı,ÓÃÓÚ´¦Àí½ÓÊÕµ½µÄÖ¸Áî
*
*/
void SuperClient::run()
{
	zTCPBufferClient::run();
	//Óë¹ÜÀí·şÎñÆ÷Ö®¼äµÄÁ¬½Ó¶Ï¿ª,ĞèÒª¹Ø±Õ·şÎñÆ÷
	zSubNetService::subNetServiceInstance()->Terminate();
}

/**
* \brief ½âÎöÀ´×Ô¹ÜÀí·şÎñÆ÷µÄ¹ØÓÚÆô¶¯µÄÖ¸Áî
*
* \param pNullCmd ´ı´¦ÀíµÄÖ¸Áî
* \param nCmdLen Ö¸Áî³¤¶È
* \return ½âÎöÊÇ·ñ³É¹¦
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

			//¿¿¿¿¿¿¿¿¿¿¿
			Zebra::logger->debug("PARA_STARTUP_SERVERENTRY_NOTIFYME size = %d ",ptCmd->size );
			for(WORD i = 0; i < ptCmd->size; i++)
			{
				//ĞèÒªÒ»¸öÈİÆ÷À´¹ÜÀíÕâĞ©·şÎñÆ÷ÁĞ±í
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
			//ĞèÒªÒ»¸öÈİÆ÷À´¹ÜÀíÕâĞ©·şÎñÆ÷ÁĞ±í
			zSubNetService::subNetServiceInstance()->addServerEntry(ptCmd->entry);
			// ·¢ËÍÃüÁîµ½SuperServer
			return sendCmd(ptCmd,nCmdLen);
		}
		break;
	}

	Zebra::logger->error("SuperClient::msgParse_Startup(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
	return false;
}

/**
* \brief ½âÎöÀ´×Ô¹ÜÀí·şÎñÆ÷µÄÖ¸Áî
*
* \param pNullCmd ´ı´¦ÀíµÄÖ¸Áî
* \param nCmdLen Ö¸Áî³¤¶È
* \return ½âÎöÊÇ·ñ³É¹¦
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
* \brief ¹¹Ôìº¯Êı
* 
* \param name Ãû³Æ
* \param wdType ·şÎñÆ÷ÀàĞÍ
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
* \brief ĞéÎö¹¹º¯Êı
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
* \brief ³õÊ¼»¯ÍøÂç·şÎñÆ÷³ÌĞò
*
* ÊµÏÖ´¿Ğéº¯Êı<code>zService::init</code>
* ½¨Á¢µ½¹ÜÀí·şÎñÆ÷µÄÁ¬½Ó,²¢µÃµ½·şÎñÆ÷ĞÅÏ¢
*
* \return ÊÇ·ñ³É¹¦
*/
bool zSubNetService::init()
{
	Zebra::logger->debug("zSubNetService::init");
	//½¨Á¢µ½¹ÜÀí·şÎñÆ÷µÄÁ¬½Óif (!superClient->connect(superIP, superPort))
	if (!superClient->connect(Zebra::global["server"].c_str(), atoi(Zebra::global["port"].c_str())))
	{
	    Zebra::logger->error("connect superserver ERROR!!!!!!!!");
	    return false;
	}

	//·¢ËÍµÇÂ½¹ÜÀí·şÎñÆ÷µÄÖ¸Áî
	Cmd::Super::t_Startup_Request tCmd;
	tCmd.wdServerType = wdServerType;
	strcpy(tCmd.pstrIP, pstrIP);
	if (!superClient->sendCmd(&tCmd,sizeof(tCmd)))
	{
		Zebra::logger->error("send msg to superserver ERROR!!!!!!!!");
		return false;
	}


	//µÈ´ı¹ÜÀí·şÎñÆ÷·µ»ØĞÅÏ¢
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

	//½¨Á¢Ïß³ÌÓë¹ÜÀí·şÎñÆ÷½»»¥
	superClient->start();

	//µ÷ÓÃÕæÊµµÄ³õÊ¼»¯º¯Êı
	return zNetService::init(wdPort);
}

/**
* \brief È·ÈÏ·şÎñÆ÷³õÊ¼»¯³É¹¦,¼´½«½øÈëÖ÷»Øµ÷º¯Êı
*
* Ïò·şÎñÆ÷·¢ËÍt_Startup_OKÖ¸ÁîÀ´È·ÈÏ·şÎñÆ÷Æô¶¯³É¹¦
*
* \return È·ÈÏÊÇ·ñ³É¹¦
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
* \brief ½áÊøÍøÂç·şÎñÆ÷
*
* ÊµÏÖ´¿Ğéº¯Êı<code>zService::final</code>
*
*/
void zSubNetService::final()
{
	Zebra::logger->debug("zSubNetService::final");
	zNetService::final();

	//¹Ø±Õµ½¹ÜÀí·şÎñÆ÷µÄÁ¬½Ó
	superClient->final();
	superClient->join();
	superClient->close();
}

/**
* \brief Ïò¹ÜÀí·şÎñÆ÷·¢ËÍÖ¸Áî
*
* \param pstrCmd ´ı·¢ËÍµÄÖ¸Áî
* \param nCmdLen ´ı·¢ËÍÖ¸ÁîµÄ´óĞ¡
* \return ·¢ËÍÊÇ·ñ³É¹¦
*/
bool zSubNetService::sendCmdToSuperServer(const void *pstrCmd,const int nCmdLen)
{
	//Zebra::logger->debug("zSubNetService::sendCmdToSuperServer");
	return superClient->sendCmd(pstrCmd,nCmdLen);
}

/**
* \brief ¸ù¾İ¹ÜÀí·şÎñÆ÷·µ»ØĞÅÏ¢,ÉèÖÃ·şÎñÆ÷µÄĞÅÏ¢
*
* \param ptCmd ¹ÜÀí·şÎñÆ÷·µ»ØĞÅÏ¢
*/
void zSubNetService::setServerInfo(const Cmd::Super::t_Startup_Response *ptCmd)
{  
	wdServerID = ptCmd->wdServerID;
	wdPort     = ptCmd->wdPort;
	strncpy(pstrIP,ptCmd->pstrIP,sizeof(pstrIP)); 
	//Zebra::logger->info("zSubNetService::setServerInfo(%d,%s:%d) %s",wdServerID,ptCmd->pstrIP,wdPort,pstrIP);
}

/**
* \brief Ìí¼Ó¹ØÁª·şÎñÆ÷ĞÅÏ¢µ½Ò»¸öÈİÆ÷ÖĞ
*
*/
void zSubNetService::addServerEntry(const Cmd::Super::ServerEntry &entry)
{
	mlock.lock();
	//Ê×ÏÈ²éÕÒÓĞÃ»ÓĞÖØ¸´µÄ
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
		//ÒÑ¾­´æÔÚÖ»ÊÇ¸üĞÂ
		(*it) = entry;
	}
	else
	{
		//»¹²»´æÔÚ,ĞèÒªĞÂ½¨Á¢Ò»¸ö½Úµã
		serverList.push_back(entry);
	}
	mlock.unlock();
}

/**
* \brief ²éÕÒÏà¹Ø·şÎñÆ÷ĞÅÏ¢
*
* \param wdServerID ·şÎñÆ÷±àºÅ
* \return ·şÎñÆ÷ĞÅÏ¢
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
* \brief ²éÕÒÏà¹Ø·şÎñÆ÷ĞÅÏ¢
*
* \param wdServerType ·şÎñÆ÷ÀàĞÍ
* \return ·şÎñÆ÷ĞÅÏ¢
*/
const Cmd::Super::ServerEntry *zSubNetService::getServerEntryByType(const WORD wdServerType)
{
	Zebra::logger->debug("zSubNetService::getServerEntryByType(type=%d)",wdServerType);
	Cmd::Super::ServerEntry *ret = NULL;
	std::deque<Cmd::Super::ServerEntry>::iterator it;
	mlock.lock();
	for(it = serverList.begin(); it != serverList.end(); it++)
	{
		Zebra::logger->debug("·şÎñÆ÷ĞÅÏ¢£º%d,%d",wdServerType,it->wdServerType);
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
* \brief ²éÕÒÏà¹Ø·şÎñÆ÷ĞÅÏ¢
*
* \param wdServerType ·şÎñÆ÷ÀàĞÍ
* \param prev ÉÏÒ»¸ö·şÎñÆ÷ĞÅÏ¢
* \return ·şÎñÆ÷ĞÅÏ¢
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
		Zebra::logger->debug("·şÎñÆ÷ĞÅÏ¢£º%d,%d",wdServerType,it->wdServerType);
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

