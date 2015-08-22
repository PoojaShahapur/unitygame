#include <iostream>
#include <string>
#include <ext/numeric>

#include "Zebra.h"
#include "zThread.h"
#include "zDBConnPool.h"
#include "zService.h"
#include "zTCPServer.h"
#include "zSocket.h"
#include "zNetService.h"
#include "zMisc.h"
#include "zArg.h"
#include "zConfile.h"

#include "FLClient.h"
#include "FLClientManager.h"
//#include "InfoClient.h"
//#include "InfoClientManager.h"
#include "ServerManager.h"
#include "RoleregCache.h"
#include "RoleregClientManager.h"
#include "RoleregClient.h"
#include "RolechangeClientManager.h"
#include "RolechangeClient.h"
#include "RolechangeCommand.h"
#include "TimeTick.h"
#include "ServerTask.h"
#include "SuperServer.h"
#include "BroadClientManager.h"

/*rief 实现管理服务器
*
* 对一个区中的所有服务器进行管理
* 
*/




zDBConnPool *SuperService::dbConnPool = NULL;

SuperService *SuperService::instance = NULL;

/**
* \brief 从数据库中获取服务器信息
*
* 如果数据库中没有管理服务器的信息,需要初始化一条记录
*
*/
bool SuperService::getServerInfo()
{
	static const dbCol col_define[] =
	{
		{"ID",zDBConnPool::DB_WORD,sizeof(WORD)},
		{"TYPE",zDBConnPool::DB_WORD,sizeof(WORD)},
		{"IP",zDBConnPool::DB_STR,sizeof(char[MAX_IP_LENGTH])},
		{"PORT",zDBConnPool::DB_WORD,sizeof(WORD)},
		{NULL,0,0}
	};
	struct
	{
		WORD wdServerID;
		WORD wdServerType;
		char pstrIP[MAX_IP_LENGTH];
		WORD wdPort;
	}
	*pData = NULL;
	char where[32];

	connHandleID handle = dbConnPool->getHandle();
	if ((connHandleID)-1 == handle)
	{
		Zebra::logger->error("不能从数据库连接池获取连接句柄");
		return false;
	}
	bzero(where,sizeof(where));
	snprintf(where,sizeof(where) - 1,"`TYPE`=%u",SUPERSERVER);
	DWORD retcode = dbConnPool->exeSelect(handle,"`SERVERLIST`",col_define,where,NULL,(BYTE **)&pData);
	if ((DWORD)1 == retcode && pData)
	{
		//只有一条满足条件的记录
		if (strcmp(pstrIP,pData->pstrIP) == 0)
		{
			wdServerID = pData->wdServerID;
			wdPort = pData->wdPort;
			SAFE_DELETE_VEC(pData);

			Zebra::logger->debug("ServerID=%u IP=%s Port=%u",wdServerID,pstrIP,wdPort);
		}
		else
		{
			Zebra::logger->error("数据库中的记录不符合：%s,%s",pstrIP,pData->pstrIP);
			SAFE_DELETE_VEC(pData);
			dbConnPool->putHandle(handle);
			return false;
		}
	}
	else
	{
		//查询出错,或者记录太多
		Zebra::logger->error("不能找到SUPERSERVER记录,或者数据库中SUPERSERVER记录太多.");
		SAFE_DELETE_VEC(pData);
		dbConnPool->putHandle(handle);
		return false;
	}
	dbConnPool->putHandle(handle);

	return true;
}

/**
* \brief 初始化网络服务器程序
*
* 实现纯虚函数<code>zService::init</code>
*
* \return 是否成功
*/
bool SuperService::init()
{
	Zebra::logger->info("SuperService::init");

	dbConnPool = zDBConnPool::newInstance(NULL);
	if (NULL == dbConnPool
		|| !dbConnPool->putURL(0,Zebra::global["mysql"].c_str(),false))
	{
	    Zebra::logger->error("SuperServer connect mysql error");
		return false;
	}

	strncpy(pstrIP,zSocket::getIPByIfName(Zebra::global["ifname"].c_str()),MAX_IP_LENGTH - 1);
	//Zebra::logger->debug("%s",pstrIP);

	if (!getServerInfo())
		return false;

	if (!FLClientManager::getInstance().init())
		return false;

	if (!RoleregClientManager::getInstance().init())
		return false;

	if (!RolechangeClientManager::getInstance().init())
		return false;


	if (!BroadClientManager::getInstance().init())
	    return false;

	////---if (!InfoClientManager::getInstance().init())
	////---  return false;

	//初始化连接线程池
	int state = state_none;
	Zebra::to_lower(Zebra::global["threadPoolState"]);
	if ("repair" == Zebra::global["threadPoolState"]
	|| "maintain" == Zebra::global["threadPoolState"])
		state = state_maintain;
	taskPool = new zTCPTaskPool(2048, state);
	if (NULL == taskPool
		|| !taskPool->init())
		return false;

	if (!zNetService::init(wdPort))
	{
		return false;
	}

	SuperTimeTick::getInstance().start();

	return true;
}

/**
* \brief 新建立一个连接任务
*
* 实现纯虚函数<code>zNetService::newTCPTask</code>
*
* \param sock TCP/IP连接
* \param addr 地址
*/
void SuperService::newTCPTask(const int sock,const struct sockaddr_in *addr)
{
	ServerTask *tcpTask = new ServerTask(taskPool,sock,addr);
	if (NULL == tcpTask)
		//内存不足,直接关闭连接
		TEMP_FAILURE_RETRY(::close(sock));
	else if (!taskPool->addVerify(tcpTask))
	{
		//得到了一个正确连接,添加到验证队列中
		SAFE_DELETE(tcpTask);
	}
}

/**
* \brief 结束网络服务器
*
* 实现纯虚函数<code>zService::final</code>
*
*/
void SuperService::final()
{
    Cmd::Rolechange::t_confirmReceive cmd;
    cmd.state = Cmd::Rolechange::DIS_RECEIVE;
    cmd.gameZone = SuperService::getInstance().getZoneID();
    if(!RolechangeClientManager::getInstance().broadcastOne(&cmd, sizeof(cmd)))
    {
	Zebra::logger->trace("SuperServer停机 发送拒绝信息到RolechangeServer失败");
    }
    else
    {
	Zebra::logger->trace("SuperServer停机 发送拒绝信息到RolechangeServer成功");
    }

	SuperTimeTick::getInstance().final();
	SuperTimeTick::getInstance().join();
	SuperTimeTick::delInstance();

	if (taskPool)
	{
		taskPool->final();
		SAFE_DELETE(taskPool);
	}
	////---InfoClientManager::delInstance();

	zNetService::final();

	ServerManager::delInstance();

	FLClientManager::delInstance();

	RoleregClientManager::delInstance();

	RolechangeClientManager::delInstance();
	
	BroadClientManager::delInstance();

	RoleregCache::delInstance();

	zDBConnPool::delInstance(&dbConnPool);

	Zebra::logger->debug("SuperService::final");
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
* \brief 读取配置文件
*
*/
class SuperConfile:public zConfile
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
* \brief 重新读取配置文件,为HUP信号的处理函数
*
*/
void SuperService::reloadConfig()
{
	Zebra::logger->debug("SuperService::reloadConfig");
	SuperConfile sc;
	sc.parse("SuperServer");
	//指令检测开关
	if (Zebra::global["cmdswitch"] == "true")
	{
		//zTCPTask::analysis._switch = true;
	}
	else
	{
		//zTCPTask::analysis._switch = false;
	}
}

/**
* \brief 主程序入口
*
* \param argc 参数个数
* \param argv 参数列表
* \return 运行结果
*/
int main(int argc,char **argv)
{
	Zebra::logger=new zLogger("SuperServer");
	Zebra::global["configdir"]  = "Config/";

	//解析命令行参数
	zArg::getArg()->add(super_options, super_parse_opt, 0, super_doc);
	zArg::getArg()->parse(argc, argv);
	
	//解析配置文件参数
	SuperConfile sc;
	if (!sc.parse("SuperServer"))
		return EXIT_FAILURE;

	//指令检测开关
	if (Zebra::global["cmdswitch"] == "true")
	{
		//zTCPTask::analysis._switch = true;
	}
	else
	{
		//zTCPTask::analysis._switch = false;
	}

	//设置日志级别
	Zebra::logger->setLevel(Zebra::global["log"]);
	//设置写本地日志文件
	if ("" != Zebra::global["logfilename"]){
		Zebra::logger->addLocalFileLog(Zebra::global["logfilename"]);
	}
	if("true" == Zebra::global["daemon"])
	{
	    Zebra::logger->info("SuperServer Program will be run as a daemon");
	    Zebra::logger->removeConsoleLog();
	    daemon(1,1);
	}

	SuperService::getInstance().main();
	SuperService::delInstance();

	return EXIT_SUCCESS;
}

