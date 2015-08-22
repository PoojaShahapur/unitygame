/**
* \brief zebra��Ŀ��½������,�����½,�����ʺš������ȹ���
*
*/
#include "ServerACL.h"
#include "zConfile.h"
#include "zTCPTask.h"
#include "zMNetService.h"
#include "zArg.h"
#include "roleRegServer.h"
#include "RoleTask.h"
#include "TableManager.h"

zDBConnPool * roleRegService::dbConnPool = NULL;
zLogger *roleRegService::ulogger = NULL;

roleRegService::roleRegService()
    :zMNetService("roleRegService")
{
    client_port = 0;
    taskPool = NULL;
}

roleRegService::~roleRegService()
{
    SAFE_DELETE(taskPool);
}
/**
* \brief ��ʼ���������������
*
* ʵ�����麯��<code>zService::init</code>
*
* \return �Ƿ�ɹ�
*/
bool roleRegService::init()
{
	Zebra::logger->debug("roleRegService::init");  

	TableManager::getMe().addTableNames("rolereg");

	dbConnPool = zDBConnPool::newInstance(TableManager::dbHashCode);

	if (NULL == dbConnPool
		|| !dbConnPool->putURL(0,Zebra::global["mysql"].c_str(),false))
	{
		//MessageBox(NULL,"�������ݿ�ʧ��","FLServer",MB_ICONERROR);
		return false;
	}
	
	if (!zMNetService::init()) return false;
	if (!ServerACL::getMe().init()) return false;


	//��ʼ�������̳߳�
	int state = state_none;
	Zebra::to_lower(Zebra::global["threadPoolState"]);
	if ("repair" == Zebra::global["threadPoolState"]
	|| "maintain" == Zebra::global["threadPoolState"])
		state = state_maintain;

	taskPool = new zTCPTaskPool(2048, state);
	if (NULL == taskPool
		|| !taskPool->init())
		return false;

	client_port  = atoi(Zebra::global["client_port"].c_str());
	if(!zMNetService::bind("bind client_port", client_port))
	{
	    return false;
	}

	return true;
}

/**
* \brief �½���һ����������
* ʵ�ִ��麯��<code>zMNetService::newTCPTask</code>
* \param sock TCP/IP����
* \param srcPort ������Դ�˿�
* \return �µ���������
*/
void roleRegService::newTCPTask(const int sock,const WORD srcPort)
{  
	Zebra::logger->debug("roleRegService::newTCPTask");

	if(srcPort == client_port)
	{
		RoleTask *tcpTask = new RoleTask(taskPool,sock);
		if (NULL == tcpTask)
			::close(sock);
		else if (!taskPool->addVerify(tcpTask))
		{
			SAFE_DELETE(tcpTask);
		}
	}
	else
		::close(sock);
}

/**
* \brief �������������
*
* ʵ���˴��麯��<code>zService::final</code>
*
*/
void roleRegService::final()
{
	zMNetService::final();
	TableManager::delMe();
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
class roleRegConfile:public zConfile
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
void roleRegService::reloadConfig()
{
	roleRegConfile sc;
	sc.parse("roleRegServer");
	Zebra::logger->setLevel(Zebra::global["log"]);
}

/**
* \brief ���������
*
* \param argc ��������
* \param argv �����б�
* \return ���н��
*/
int main(int argc,char **argv)
{
	Zebra::logger=new zLogger("roleRegServer");

	//����ȱʡ����
	Zebra::global["client_port"]    = "9901";
	Zebra::global["configdir"]  = "Config/";
	Zebra::global["dbCount"]  = "1";
	Zebra::global["tableCount"]  = "32";


	//���������ļ�����
	roleRegConfile sc;
	if (!sc.parse("roleRegServer")) return EXIT_FAILURE;
	//���������в���
	zArg::getArg()->add(super_options, super_parse_opt, 0, super_doc);
	zArg::getArg()->parse(argc, argv);

	//������־����
	Zebra::logger->setLevel(Zebra::global["log"]);
	//����д������־�ļ�
	if ("" != Zebra::global["logfilename"]){
		Zebra::logger->addLocalFileLog(Zebra::global["logfilename"]);
	}
	if("true" == Zebra::global["daemon"])
	{
		Zebra::logger->info("roleRegServer Program will be run as a daemon");
		Zebra::logger->removeConsoleLog();
		daemon(1,1);
	}  
	roleRegService::getMe().main();
	roleRegService::delMe();

	return EXIT_SUCCESS;
}
