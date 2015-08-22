/**
* \brief zebra��Ŀ����������,��Ϸ���󲿷����ݶ��ڱ�ʵ��
*/

#include "ScenesServer.h"
#include "Zebra.h"
#include "SceneUserManager.h"
#include "zConfile.h"
#include "SceneTaskManager.h"
#include "SceneManager.h"
#include "TimeTick.h"
#include "SessionClient.h"
#include "RecordClient.h"
#include "Chat.h"
#include "MiniClient.h"
#include "zArg.h"
#include "zXML.h"
//#include "zDebugNew.h"
#include "zDatabaseManager.h"
#include "Quest.h"
#include "QuestTable.h"
//#include "script.h"
#include "SceneNpcManager.h"
#include "NpcTrade.h"
#include "ObjectManager.h"
#include "Recycle.h"
#include "SkillStatusManager.h"
#include "SceneMsgCmdHandle.h"
#include "CardEffectCfg.h"
#include "DynamicSceneManager.h"

ScenesService *ScenesService::instance = NULL;
bool ScenesService::reload=false;
zLogger * ScenesService::gmlogger = NULL;
zLogger * ScenesService::objlogger = NULL;
zLogger * ScenesService::wglogger = NULL;
std::set<DWORD> dupMapList;

#if 0
WORD max_level = 0;
QWORD max_exp = 0;
WORD trun_point_rate = 0;
WORD trun_skill_rate = 0;

//sky װ�����伫Ʒ����
WORD g_blue_changce = 0;
WORD g_green_changce = 0;
WORD g_purple_changce = 0;
WORD g_orange_changce = 0;

//sky ���漼���б�
std::vector<DWORD> UseableMagicList;

//sky ��ȴʱ�����ñ�
std::vector<stXmlItemCoolTime> vXmlItemCoolTime;

//[Shx Add ��װ���ñ�]
std::vector<stxml_SuitAttribute> vXmlSuitAttribute;

std::vector<BYTE> CoolTimeSendData;

//sky �����ָ�ϵ��(Ĭ����4%)
WORD recoverRate = 4;

Cmd::stChannelChatUserCmd * ScenesService::pStampData = 0;

/// �й����辭��
DWORD cancel_country_need_money = 50000; //Ĭ���嶧
DWORD is_cancel_country = 0; // �Ƿ������ѹ�

NFilterModuleArray g_nFMA;


//[shx Add ����װ��Ʒһ����־,ʹ����Ա����ٶ�λ]
void InitObjBase_SuitData()
{
	for(int i = 0; i < vXmlSuitAttribute.size(); i ++)
	{
		LPDWORD pParts = vXmlSuitAttribute[i].MemberList;
		for(int j = 0; j < vXmlSuitAttribute[i].count; j ++)
		{
			zObjectB* pBase = objectbm.get(pParts[j]);
			if(pBase ) 
			{
				if( pBase->nSuitData >= 0)
					printf("\n����: ��Ʒ %u ����װ�����ļ��б��ظ�ʹ��\n", pParts[j]);

				pBase->nSuitData = i;
			}			
		}
	}
}
#endif

/**
* \brief ��ʼ���������������
*
* ʵ�����麯��<code>zService::init</code>
*
* \return �Ƿ�ɹ�
*/
bool ScenesService::init()
{
	using namespace Cmd;


	//NFilterModuleArray::const_iterator pIterator;

	Zebra::logger->info("ScenesService::init()");

	verify_version = atoi(VERSION_STRING);
	Zebra::logger->info("�������汾��:%d", verify_version);

	for(int i=0; i<13; i++) countryPower[i]=1;

	//��ʼ�������̳߳�
	int state = state_none;
	Zebra::to_lower(Zebra::global["threadPoolState"]);
	if ("repair" == Zebra::global["threadPoolState"]
	|| "maintain" == Zebra::global["threadPoolState"])
		state = state_maintain;
	taskPool = new zTCPTaskPool(2048, state, 5000);

	if (NULL == taskPool
		|| !taskPool->init())
		return false;

	strncpy(pstrIP,zSocket::getIPByIfName(Zebra::global["ifname"].c_str()),MAX_IP_LENGTH - 1);
	//Zebra::logger->debug("%s",pstrIP);

	if (!zSubNetService::init())
	{
		return false;
	}

	const Cmd::Super::ServerEntry *serverEntry = NULL;

	//���ӵ���������
	serverEntry = getServerEntryByType(RECORDSERVER);
	if (NULL == serverEntry)
	{
		Zebra::logger->error("�����ҵ����������������Ϣ,�������ӵ���������");
		return false;
	}

	recordClient = new RecordClient("�����������ͻ���",serverEntry->pstrIP,serverEntry->wdPort);
	if (NULL == recordClient)
	{
		Zebra::logger->error("û���㹻�ڴ�,���ܽ��������������ͻ���ʵ��");
		return false;
	}
	if (!recordClient->connectToRecordServer())
	{
		Zebra::logger->error("ScenesService::init ���ӵ���������ʧ��");
		return false;
	}
	if (recordClient->start())
	{
	}
	//Zebra::logger->info("��ʼ������������ģ��(%s:%d)�ɹ�",serverEntry->pstrIP,serverEntry->wdPort);

	//���ӻỰ������
	serverEntry = getServerEntryByType(SESSIONSERVER);
	if (NULL == serverEntry)
	{
		Zebra::logger->error("�����ҵ��Ự�����������Ϣ,�������ӻỰ������");
		return false;
	}
	sessionClient = new SessionClient("�Ự�������ͻ���",serverEntry->pstrIP,serverEntry->wdPort);
	if (NULL == sessionClient)
	{
		Zebra::logger->error("û���㹻�ڴ�,���ܽ����Ự�������ͻ���ʵ��");
		return false;
	}
	if (!sessionClient->connectToSessionServer())
	{
		Zebra::logger->error("ScenesService::init ���ӻỰ������ʧ��");
		return false;
	}
	if (sessionClient->start())
		Zebra::logger->info("��ʼ���Ự������ģ��(%s:%d)�ɹ�",serverEntry->pstrIP,serverEntry->wdPort);
#if 0
	//����С��Ϸ������
	serverEntry = getServerEntryByType(MINISERVER);
	if (NULL == serverEntry)
	{
		Zebra::logger->error("�����ҵ�С��Ϸ�����������Ϣ,��������С��Ϸ������");
		return false;
	}
	miniClient = new MiniClient("С��Ϸ�������ͻ���",serverEntry->pstrIP,serverEntry->wdPort,serverEntry->wdServerID);
	if (NULL == miniClient)
	{
		Zebra::logger->error("û���㹻�ڴ�,���ܽ���С��Ϸ�������ͻ���ʵ��");
		return false;
	}
	if (!miniClient->connectToMiniServer())
	{
		Zebra::logger->error("ScenesService::init ����С��Ϸ������ʧ��");
		return false;
	}
	if (!miniClient->start())
	{
		Zebra::logger->warn("��ʼ��Mini������ģ��ʧ��");
	}
	else{
		Zebra::logger->info("miniClient start OK!!!(%s:%d)",serverEntry->pstrIP,serverEntry->wdPort);
	}
#endif
	xml::Configs::load_scene(); //aotu parse xml

	if (!SceneNpcManager::getMe().init())
	{
		Zebra::logger->warn("��ʼ��NPC������ʧ��");
	}
	else
	{
		Zebra::logger->info("��ʼ��NPC�������ɹ�");
	}
	
	SkillStatusManager::initFunctionList();

	if(SceneRecycle::getMe().start())
	{
	    Zebra::logger->info("��ʼ��SceneRecycleģ��ɹ�");
	}


	//���ػ�������
	if (!loadAllBM())
	{
		Zebra::logger->error("��ʼ����������ģ��ʧ��");
		return false;
	}
	Zebra::logger->info("��ʼ����������ģ��ɹ�");
#if 0
	char srv[256];
	bzero(srv,sizeof(srv));
	sprintf(srv,"WS[%d]",getServerID());
	objlogger = new zLogger(srv);
	objlogger->setLevel(Zebra::global["log"]);
	//����д������־�ļ�
	if ("" != Zebra::global["objlogfilename"])
	{
		bzero(srv,sizeof(srv));
		char sub[256];
		bzero(sub,sizeof(sub));
		_snprintf(srv,sizeof(srv),"%s",Zebra::global["objlogfilename"].c_str());
		char *tok = strstr(srv,".");
		if (tok != NULL)
		{
			strncpy(sub,tok,sizeof(sub));
			bzero(tok,strlen(tok));
			sprintf(srv + strlen(srv),"%d",getServerID());
			strncat(srv,sub,sizeof(srv) - strlen(srv));
		}
		else
		{
			_snprintf(srv + strlen(srv),sizeof(srv) - strlen(srv),"%d",getServerID());
		}
		objlogger->addLocalFileLog(srv);
		objlogger->removeConsoleLog();
	}
#endif
	objlogger = new zLogger("objlog");
	objlogger->setLevel(Zebra::global["log"]);
        if ("" != Zebra::global["objlogfilename"])
	{

	                  objlogger->addLocalFileLog(Zebra::global["objlogfilename"]);
	                   objlogger->removeConsoleLog();
	}

	gmlogger = new zLogger("gmlog");
	gmlogger->setLevel(Zebra::global["log"]);
	if ("" != Zebra::global["gmlogfilename"]){
		gmlogger->addLocalFileLog(Zebra::global["gmlogfilename"]);
		gmlogger->removeConsoleLog();
	}

	wglogger = new zLogger("wglog");
	wglogger->setLevel(Zebra::global["log"]);
	if ("" != Zebra::global["wglogfilename"]){
		wglogger->addLocalFileLog(Zebra::global["wglogfilename"]);
		wglogger->removeConsoleLog();
	}

	if (!SceneManager::getInstance().init())
	{
		Zebra::logger->error("��ʼ������������ʧ��");
		return false;
	}
	if (!NpcTrade::getInstance().init())
	{
		Zebra::logger->error("��ʼ��NPC��������ģ��ʧ��");
		return false;
	}

	if(!DynamicSceneManager::getMe().loadConfig())
	{
	    Zebra::logger->error("��ʼ��DynamicSceneManager�����ļ�ʧ��");
	    return false;
	}
	Zebra::logger->info("��ʼ��NPC��������ģ��ɹ�");
	ALLVARS1(server_id,getServerID());
	ALLVARS(load);

	if (!QuestTable::instance().init())
	{
		Zebra::logger->error("��ʼ������ģ��ʧ��");
		return false;
	}
	Zebra::logger->info("��ʼ������ģ��ɹ�");
	SceneMsgCmdHandle::getMe().registerSceneMsgCmd();

	if(Zebra::global["battleZone"] != "")
	{
	    battleZoneID = atoi(Zebra::global["battleZone"].c_str());
	    Zebra::logger->info("����ս��ID :%d", battleZoneID);
	}
	if(Zebra::global["battle"] == "true")
	{
	    battle = true;
	}
	else
	{
	    battle = false;
	}

	CardEffectCfg::getMe().loadAllEffect();
#if 0
	LuaVM* vm = ScriptingSystemLua::instance().createVM();
	LuaScript* script = ScriptingSystemLua::instance().createScriptFromFile("newquest/quest.lua");
	Binder bind;
	bind.bind(vm);
	vm->execute(script);
	SAFE_DELETE(script);
	//sppeed up
	ScriptQuest::get_instance().sort();
#endif

#if 0
	if (!MagicRangeInit::getInstance().init())
	{
		Zebra::logger->error("��ʼ��������Χ����ģ��ʧ��");
		return false;
	}
	Zebra::logger->info("��ʼ��������Χ����ģ��ɹ�");

	CountryDareM::getMe().init();
	CountryTechM::getMe().init();
	CountryAllyM::getMe().init();

	loadFilter(g_nFMA,"ScenesServer_*.dll");
	//init
	for(pIterator=g_nFMA.begin(); pIterator != g_nFMA.end();pIterator++)
	{
		if (NULL != pIterator->filter_init)
		{
			pIterator->filter_init();
		}
	}

	Zebra::logger->info("ScenesService::init() OK");

	globalBox::newInstance();
	globalBox::getInstance().init();
	boxCircle::newInstance();
	scriptMessageFilter::initFilter();

	//Shx ��ȡ��װ�����б�
	LoadSuitInfo();
	InitObjBase_SuitData();


	//sky ��ȡ��Ʒ��ȴ�б�
	LoadItmeCoolTime();
	//sky �Ѷ�������ȴ���ݷ�װ�ɷ����ͻ��˵����ݰ�
	StlToSendData();
	//duplicateManager::newInstance();
#endif
	if (!SceneTimeTick::getInstance().start())
	{
		Zebra::logger->warn("��ʼ��TimeTickģ��ʧ��");
	}
	else{
		Zebra::logger->info("SceneTimeTick start OK!!!");
	}
	return true;
}


/**
* \brief �½���һ����������
*
* ʵ�ִ��麯��<code>zNetService::newTCPTask</code>
*
* \param sock TCP/IP����
* \param addr ��ַ
*/
void ScenesService::newTCPTask(const int sock,const struct sockaddr_in *addr)
{
	SceneTask *tcpTask = new SceneTask(taskPool,sock,addr);

	if (NULL == tcpTask)
	{
		//�ڴ治��,ֱ�ӹر�����
		::close(sock);
	}
	else if (!taskPool->addVerify(tcpTask))
	{
		//�õ���һ����ȷ����,��ӵ���֤������
		SAFE_DELETE(tcpTask);
	}
}

/**
* \brief �������Թ����������ָ��
*
* ��Щָ�������غ͹��������������ָ��<br>
* ʵ�����麯��<code>zSubNetService::msgParse_SuperService</code>
*
* \param pNullCmd ��������ָ��
* \param nCmdLen ��������ָ���
* \return �����Ƿ�ɹ�
*/
bool ScenesService::msgParse_SuperService(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
#if 0
	switch(pNullCmd->cmd)
	{
	case Cmd::GmTool::CMD_GMTOOL:
		{
			using namespace Cmd::GmTool;

			switch(pNullCmd->para)
			{
			case PARA_PUNISH_GMTOOL:
				{
					t_Punish_GmTool * rev = (t_Punish_GmTool *)pNullCmd;
					SceneUser *pUser = SceneUserManager::getMe().getUserByName(rev->userName);
					if (!pUser) break;
					switch (rev->operation)
					{
					case 1://����
						{
							pUser->delayForbidTalk(rev->delay);
							if (rev->delay>0)
							{
								Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"�㱻GM���� %d ��",rev->delay);                                               
								ScenesService::gmlogger->info("��� %s ������ %d ��",pUser->name,rev->delay);
							}
							else
							{
								Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,"�㱻GM�������,���ڿ���˵����");
								ScenesService::gmlogger->info("��� %s ���������",pUser->name);
							}
						}
						break;
					case 2://�ؽ���
						break;
					case 3://������
						{
							OnQuit event(1);
							EventTable::instance().execute(*pUser,event);
							execute_script_event(pUser,"quit");

							pUser->save(Cmd::Record::LOGOUT_WRITEBACK);
							Cmd::Session::t_unregUser_SceneSession ret;
							ret.dwUserID=pUser->id;
							ret.dwSceneTempID=pUser->scene->tempid;
							ret.retcode=Cmd::Session::UNREGUSER_RET_ERROR;
							sessionClient->sendCmd(&ret,sizeof(ret));
							Cmd::Scene::t_Unreg_LoginScene retgate;
							retgate.dwUserID = pUser->id;
							retgate.dwSceneTempID = pUser->scene->tempid;
							retgate.retcode = Cmd::Scene::UNREGUSER_RET_ERROR;
							pUser->gatetask->sendCmd(&retgate,sizeof(retgate));

							pUser->unreg();
						}
						break;
					case 4://����
						{
							Channel::sendSys(pUser,Cmd::INFO_TYPE_FAIL,rev->reason);
						}
						break;
					default:
						return true;
					}

					rev->level = pUser->charbase.level;
					rev->accid = pUser->charbase.accid;
					zRTime ct;
					rev->startTime = ct.sec();
					strncpy(rev->country,SceneManager::getInstance().getCountryNameByCountryID(pUser->charbase.country),MAX_NAMESIZE);
					ScenesService::getInstance().sendCmdToSuperServer(rev,sizeof(t_Punish_GmTool));
				}
				break;
			}
		}
		break;
	}
#endif
	Zebra::logger->error("ScenesService::msgParse_SuperService(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
	return false;
}

/**
* \brief �������������
*
* ʵ���˴��麯��<code>zService::final</code>
*
*/
void ScenesService::final()
{
#if 0
	NFilterModuleArray::const_iterator pIterator;

	//term
	for(pIterator=g_nFMA.begin(); pIterator != g_nFMA.end();pIterator++)
	{
		if (NULL != pIterator->filter_term)
		{
			pIterator->filter_term();
		}
	}
#endif
	SceneTimeTick::getInstance().final();
	SceneTimeTick::getInstance().join();
	SceneTimeTick::delInstance();

	if(GlobalVar::server_id())
	{
	    ALLVARS(save);
	}
	SceneUserManager::getMe().removeAllUser();

	if (taskPool)
	{
		taskPool->final();
		SAFE_DELETE(taskPool);
	}
	if (sessionClient)
	{
		sessionClient->final();
		sessionClient->join();
		SAFE_DELETE(sessionClient);
	}

	if (recordClient)
	{
		recordClient->final();
		recordClient->join();
		SAFE_DELETE(recordClient);
	}

	SceneTaskManager::delInstance();

	SceneManager::delInstance();
	GlobalObjectIndex::delInstance();

	SceneRecycle::getMe().final();
	SceneRecycle::getMe().join();
	SceneRecycle::getMe().delInstance();


	unloadAllBM();
	zSubNetService::final();

	//check_leaks();//check memory leak
	Zebra::logger->debug("ScenesService::final");
	SAFE_DELETE(gmlogger);
}

static struct argp_option super_options[] =
{	
	{"daemon",		'd',	0,			0,	"Run as daemon",								0},
	{"log",			'l',	"level",	0,	"Log level",									0},
	{"logfilename",	'f',	"filename",	0,	"Log file name",								0},
	{"mysql",		'y',	"mysql",	0,	"MySQL[mysql://user:passwd@host:port/dbName]",	0},
	{"ifname",		'i',	"ifname",	0,	"Local network device",							0},
	{0,				0,		0,			0,	0,												0}
};

static error_t super_parse_opt(int key, char *arg, struct argp_state *state)
{
	switch(key)
	{
		case 'c':
		Zebra::global["configfile"] = arg;
		break;
		case 'd':
		Zebra::global["daemon"] = "true";
		break;
		case 'l':
		Zebra::global["log"] = arg;
		break;
		case 'f':
		Zebra::global["logfilename"] = arg;
		break;
		case 'y':
		Zebra::global["mysql"] = arg;
		break;
		case 'i':
		Zebra::global["ifname"] = arg;
		break;
		default:
		return ARGP_ERR_UNKNOWN;
	}
	return 0;
}

static char super_doc[] = "\nSuperServer\n" "\tgame server manager";

const char *argp_program_version = "Program version ";
/**
* \brief ��ȡ�����ļ�
*
*/
class SceneConfile:public zConfile
{
	bool parseYour(const xmlNodePtr node)
	{
		if (node)
		{
			xmlNodePtr child=parser.getChildNode(node,NULL);
			while(child)
			{
				parseNormal(child);
				child=parser.getNextNode(child,NULL);
			}
			return true;
		}
		else
			return false;
	}
};

/**
* \brief ���¶�ȡ�����ļ�,ΪHUP�źŵĴ�����
*
*/
void ScenesService::reloadConfig()
{
	reload=true;
	Zebra::logger->debug("ScenesService::reloadConfig");
}

void ScenesService::checkAndReloadConfig()
{
	if (reload)
	{
		reload=false;
		Zebra::logger->debug("ScenesService::checkAndReloadConfig");
		SceneConfile sc;
		sc.parse("ScenesServer");
#if 0
		loadAllBM();
		NpcTrade::getInstance().init();
		
#endif
		//MessageSystem::getInstance().init();
		//��ʱ�浵����
		if (atoi(Zebra::global["writebacktimer"].c_str()))
		{
			ScenesService::getInstance().writeBackTimer = atoi(Zebra::global["writebacktimer"].c_str());
		}
		else
		{
			ScenesService::getInstance().writeBackTimer = 600;
		}

		ScenesService::getInstance().ExpRate       = atoi(Zebra::global["ExpRate"].c_str());
		ScenesService::getInstance().DropRate      = atoi(Zebra::global["DropRate"].c_str());
		ScenesService::getInstance().DropRateLevel = atoi(Zebra::global["DropRateLevel"].c_str());
#if 0
		//ָ���⿪��
		if (Zebra::global["cmdswitch"] == "true")
		{
			zTCPTask::analysis._switch = true;
			zTCPClient::analysis._switch=true;
		}
		else
		{
			zTCPTask::analysis._switch = false;
			zTCPClient::analysis._switch=false;
		}
#endif
	}
}
#if 0
/**
* \brief ���¶�ȡ�������ļ�
*
*/
DWORD ScenesService::updateStampData()
{
	std::string process_file;

	if (pStampData)
	{
		free(pStampData);
		pStampData = 0;
	}

	int f=0;

	process_file = Zebra::global["configdir"] + "process.dat";
	// f = open(process_file.c_str(),O_RDONLY);
	/* if (f != -1)
	{
	pStampData = (Cmd::stChannelChatUserCmd *)malloc(zSocket::MAX_DATASIZE);
	bzero(pStampData,zSocket::MAX_DATASIZE);
	constructInPlace(pStampData);

	pStampData->dwType = Cmd::CHAT_TYPE_SYSTEM;
	pStampData->dwSysInfoType = Cmd::INFO_TYPE_GAME;
	strncpy(pStampData->pstrChat,"��ӭ����Ӣ����˫",MAX_CHATINFO-1);
	pStampData->dwFromID = read(f,(void *)(pStampData->tobject_array),zSocket::MAX_DATASIZE-sizeof(Cmd::stChannelChatUserCmd));
	close(f);

	pStampData->dwChannelID = atoi(Zebra::global["service_flag"].c_str()) & Cmd::Session::SERVICE_PROCESS;
	return pStampData->dwFromID;
	}*/

	return 1;
	//return 0;
}
#endif
/**
* \brief ���������
*
* \param argc ��������
* \param argv �����б�
* \return ���н��
*/
int main(int argc,char *argv[])
{
	Zebra::logger=new zLogger("ScenesServer");


	//����ȱʡ����
	Zebra::global["datadir"]         = "data/";
	Zebra::global["configdir"]         = "Config/";
	Zebra::global["questdir"]        = "quest/";
	Zebra::global["cmdswitch"]       = "true";
	Zebra::global["writebacktimer"]  = "600";
	Zebra::global["ExpRate"]         = "1";
	Zebra::global["DropRate"]        = "1";
	Zebra::global["DropRateLevel"]   = "0";
	Zebra::global["mail_service"]    = "on";
	Zebra::global["vardir"] = "game_var/";	    //�洢�������var����

	//���������ļ�����
	SceneConfile sc;
	if (!sc.parse("ScenesServer"))
		return EXIT_FAILURE;
//���������в���
	zArg::getArg()->add(super_options, super_parse_opt, 0, super_doc);
	zArg::getArg()->parse(argc, argv);
	//������־����
	Zebra::logger->setLevel(Zebra::global["log"]);
	//����д������־�ļ�
	if ("" != Zebra::global["logfilename"]){
		Zebra::logger->addLocalFileLog(Zebra::global["logfilename"]);
	}

	if (atoi(Zebra::global["writebacktimer"].c_str()))
	{
		ScenesService::getInstance().writeBackTimer = atoi(Zebra::global["writebacktimer"].c_str());
	}
	else
	{
		ScenesService::getInstance().writeBackTimer = 600;
	}

	ScenesService::getInstance().ExpRate       = atoi(Zebra::global["ExpRate"].c_str());
	ScenesService::getInstance().DropRate      = atoi(Zebra::global["DropRate"].c_str());
	ScenesService::getInstance().DropRateLevel = atoi(Zebra::global["DropRateLevel"].c_str());
#if 0
	//ָ���⿪��
	if (Zebra::global["cmdswitch"] == "true")
	{
		zTCPTask::analysis._switch = true;
		zTCPClient::analysis._switch=true;
	}
	else
	{
		zTCPTask::analysis._switch = false;
		zTCPClient::analysis._switch=false;
	}

	scriptTaskManagement::newInstance();
	meterialsManager::newInstance();
	duplicateManager::newInstance();

	Zebra_Startup();
#endif
	if ("true" == Zebra::global["daemon"])
	      {

		        Zebra::logger->info("ScenesServer Program will be run as a daemon");
			      Zebra::logger->removeConsoleLog();
			            daemon(1, 1); 
				      }
	ScenesService::getInstance().main();
	ScenesService::delInstance();
	SceneUserManager::destroyMe();



	return EXIT_SUCCESS;
}


//[Shx Add ��ȡ��װ������Ϣ]
bool ScenesService::LoadSuitInfo()
{
#if 0
	char sFileName[MAX_PATH];
	strcpy( sFileName, "data/SuitInfo.xml" );

	zXMLParser xml;
	if (!xml.initFile(sFileName))
	{
		Zebra::logger->error("������װ�����ļ� %s ʧ��",sFileName);
		return false;
	}
	xmlNodePtr _root;
	xmlNodePtr _suits;

	_root = xml.getRootNode("HREO");
	_suits = xml.getChildNode(_root, "SUITS");	
	if(_root && _suits)
	{


		xmlNodePtr _node = xml.getChildNode(_suits,"suit");  
		while (_node) 
		{	
			stxml_SuitAttribute stData;		

			xml.getNodePropNum(_node, "id", &stData.id, 2);

			std::string str="";
			xml.getNodePropStr(_node, "name", str);			
			strncpy(stData.Name, str.c_str(), 30);

			xmlNodePtr _part = xml.getChildNode(_node, "part");
			int n_p = 0;
			while (_part && n_p < MAX_SUIT_NUM)
			{
				xml.getNodePropNum(_part,"itemid", &stData.MemberList[n_p], 4);				
				n_p ++;					
				_part = xml.getNextNode(_part,"part");
			}
			stData.count = n_p;	
			
			int n_e = 0;
			xmlNodePtr _Effect = xml.getChildNode(_node, "effect");					
			while (_Effect && n_e < MAX_SUIT_NUM)
			{
				st_SuitEffect stEffect;			
				xml.getNodePropNum(_Effect, "itemcount",&stEffect.eRequire,	1);			
				xml.getNodePropNum(_Effect, "ekey",		&stEffect.eKey,		1);
				xml.getNodePropNum(_Effect, "evalue",	&stEffect.eValue,	2);	

				stData.EffectList.push_back(stEffect);				
				n_e ++;
				_Effect = xml.getNextNode(_Effect,"effect");				
			}
			stData.eCount = n_e;

			vXmlSuitAttribute.push_back(stData);

			_node = xml.getNextNode(_node, "suit");	

			printf("������װ %s,����: %d, ����: %d\n", stData.Name, n_p, n_e);
		}
		printf("��ȡ��װ���ü�¼: %d\n", vXmlSuitAttribute.size());
		return true;
	}
	Zebra::logger->error("��ȡ��װ�����ļ� %s ʧ��",sFileName);
	return false;
#endif
	return true;
}
//End Shx

bool ScenesService::LoadItmeCoolTime()
{
#if 0
	char CoolFileName[MAX_PATH];
	strcpy( CoolFileName, "data/ItemCoolTime.xml" );

	zXMLParser xml;
	if (!xml.initFile(CoolFileName))
	{
		Zebra::logger->error("������Ʒ��ȴ�ļ� %s ʧ��",CoolFileName);
		return false;
	}

	xmlNodePtr root;

	root = xml.getRootNode("CoolTime");

	if (root)
	{
		xmlNodePtr node = xml.getChildNode(root,"cooldown");  
		while (node) 
		{
			stXmlItemCoolTime CoolTiem;
			stItemTypeCoolTiem TypeCool;
			stItemIdCoolTime IdCool;

			if(!xml.getNodePropNum(node, "CoolID", &(CoolTiem.CoolTimeType),sizeof(WORD))) 
			{
				return false;
			}

			if(!xml.getNodePropNum(node, "times", &(CoolTiem.nCoolTime), sizeof(DWORD)))
			{
				return false;
			}

			xmlNodePtr phaseNode = xml.getChildNode(node,"objclass");

			while(phaseNode)
			{
				if(!xml.getNodePropNum(phaseNode, "type", &(TypeCool.ItemType), sizeof(WORD)))
				{
					return false;
				}

				if(!xml.getNodePropNum(phaseNode, "time", &(TypeCool.CoolTime), sizeof(DWORD)))
				{
					TypeCool.CoolTime = 0;
				}

				CoolTiem.TypeCoolTime.push_back(TypeCool);

				phaseNode = xml.getNextNode(phaseNode, "objclass");
			}

			xmlNodePtr IdNode = xml.getChildNode(node, "objid");

			while(IdNode)
			{
				if(!xml.getNodePropNum(IdNode, "id", &(IdCool.ItemID), sizeof(DWORD)))
				{
					return false;
				}

				if(!xml.getNodePropNum(IdNode, "time", &(IdCool.CoolTime), sizeof(DWORD)))
				{
					IdCool.CoolTime = 0;
				}

				CoolTiem.IdCoolTime.push_back(IdCool);

				IdNode = xml.getNextNode(IdNode, "objid");
			}

			vXmlItemCoolTime.push_back(CoolTiem);

			node = xml.getNextNode(node,"cooldown");
		}

		int count = vXmlItemCoolTime.size();
		printf("��ȡ��Ʒ��ȴ��������:%d", count);

		return true;
	}

	return false;
#endif
	return true;
}

bool ScenesService::StlToSendData()
{
#if 0
	int nLen = 0;
	int index = 0;

	std::vector<stXmlItemCoolTime>::iterator iter;
	std::vector<stItemTypeCoolTiem>::iterator iter1;
	std::vector<stItemIdCoolTime>::iterator iter2;

	for(iter=vXmlItemCoolTime.begin(); iter!=vXmlItemCoolTime.end(); iter++)
	{
		nLen += iter->TypeCoolTime.size() * sizeof(stItemTypeCoolTiem); // TypeCoolTime
		nLen += iter->IdCoolTime.size() * sizeof(stItemIdCoolTime); // IdCoolTime
	}

	nLen += vXmlItemCoolTime.size() * sizeof(WORD); // TypeCoolTime
	nLen += vXmlItemCoolTime.size() * sizeof(DWORD);// nCoolTime

	CoolTimeSendData.resize(nLen + sizeof(Cmd::stItemCoolTimesUserCmd) + sizeof(WORD) * vXmlItemCoolTime.size() * 2);
	Cmd::stItemCoolTimesUserCmd cmd(vXmlItemCoolTime.size());
	memcpy(&CoolTimeSendData[index], &cmd, sizeof(Cmd::stItemCoolTimesUserCmd), sizeof(Cmd::stItemCoolTimesUserCmd));
	index += sizeof(Cmd::stItemCoolTimesUserCmd);
	for(iter=vXmlItemCoolTime.begin(); iter!=vXmlItemCoolTime.end(); iter++)
	{
		memcpy(&CoolTimeSendData[index],&(*iter).CoolTimeType, sizeof(WORD), sizeof(WORD));
		index += sizeof(WORD);
		memcpy(&CoolTimeSendData[index],&(*iter).nCoolTime, sizeof(DWORD), sizeof(DWORD));
		index += sizeof(DWORD);

		WORD Count = iter->TypeCoolTime.size();

		memcpy(&CoolTimeSendData[index],&Count, sizeof(WORD), sizeof(WORD));
		index += sizeof(WORD);

		for(iter1=iter->TypeCoolTime.begin(); iter1!=iter->TypeCoolTime.end(); iter1++)
		{
			memcpy(&CoolTimeSendData[index], &(*iter1), sizeof(stItemTypeCoolTiem), sizeof(stItemTypeCoolTiem));
			index += sizeof(stItemTypeCoolTiem);
		}

		Count = iter->IdCoolTime.size();
		memcpy(&CoolTimeSendData[index],&Count, sizeof(WORD), sizeof(WORD));
		index += sizeof(WORD);

		for(iter2=iter->IdCoolTime.begin(); iter2!=iter->IdCoolTime.end(); iter2++)
		{
			memcpy(&CoolTimeSendData[index], &(*iter2), sizeof(stItemIdCoolTime), sizeof(stItemIdCoolTime));
			index += sizeof(stItemIdCoolTime);
		}
	}
#endif
	return true;
}
