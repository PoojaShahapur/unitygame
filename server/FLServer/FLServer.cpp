/**
* \brief zebra项目登陆服务器,负责登陆,建立帐号、档案等功能
*
*/
#include "FLServer.h"
#include "ServerACL.h"
#include "zConfile.h"
#include "TimeTick.h"
#include "ServerManager.h"
#include "zTCPTask.h"
#include "ServerTask.h"
#include "LoginTask.h"
#include "LoginManager.h"
#include "GYListManager.h"
#include "zMNetService.h"
#include "zArg.h"
#include "UserTask.h"

zDBConnPool * FLService::dbConnPool = NULL;
DBMetaData * FLService::metaData = NULL;
zLogger *FLService::ulogger = NULL;

FLService::FLService()
    :zMNetService("LoginServer")
{
    login_port = 0;
    inside_port = 0;
    user_port = 0;
    //master_bind_port = 0;
    loginTaskPool = NULL;
    serverTaskPool = NULL;
    userTaskPool = NULL;
}

FLService::~FLService()
{

}
/**
* \brief 初始化网络服务器程序
*
* 实现了虚函数<code>zService::init</code>
*
* \return 是否成功
*/
bool FLService::init()
{
	Zebra::logger->debug("FLService::init");  

	dbConnPool = zDBConnPool::newInstance(NULL);

	if (NULL == dbConnPool
		|| !dbConnPool->putURL(0,Zebra::global["mysql"].c_str(),false))
	{
		//MessageBox(NULL,"连接数据库失败","FLServer",MB_ICONERROR);
		return false;
	}

	metaData = DBMetaData::newInstance("");

	if (NULL == metaData
		|| !metaData->init(Zebra::global["mysql"]))
	{
	    return false;
	}

	if (!zMNetService::init()) return false;
	if (!ServerACL::getMe().init()) return false;

	////---if (!InfoClientManager::getInstance().init()) return false;  

	//初始化连接线程池
	int state = state_none;
	Zebra::to_lower(Zebra::global["threadPoolState"]);
	if ("repair" == Zebra::global["threadPoolState"]
	|| "maintain" == Zebra::global["threadPoolState"])
		state = state_maintain;

	loginTaskPool = new zTCPTaskPool(2048, state);
	if (NULL == loginTaskPool
		|| !loginTaskPool->init())
		return false;

	serverTaskPool = new zTCPTaskPool(2048, state);
	if (NULL == serverTaskPool
		|| !serverTaskPool->init())
		return false;

	userTaskPool = new zTCPTaskPool(2048, state);
	if (NULL == userTaskPool
		|| !userTaskPool->init())
	    return false;

#if 0
	pingTaskPool = new zTCPTaskPool(atoi(Zebra::global["threadPoolServer"].c_str()),state);
	if (NULL == pingTaskPool
		|| !pingTaskPool->init())
		return false;
#endif
#if 0
	login_port  = atoi(Zebra::global["login_port"].c_str());
	inside_port = atoi(Zebra::global["inside_port"].c_str());
	ping_port   = atoi(Zebra::global["ping_port"].c_str());

	if (!zMNetService::bind("登陆端口",login_port)
		|| !zMNetService::bind("内部服务端口",inside_port)
		|| !zMNetService::bind("PING端口",ping_port))
	{
		return false;
	}
#endif
	inside_port = atoi(Zebra::global["inside_port"].c_str());
	if(!zMNetService::bind("bind inside_port", inside_port))
	{
	    return false;
	}
	login_port  = atoi(Zebra::global["login_port"].c_str());
	if(!zMNetService::bind("bind login_port", login_port))
	{
	    return false;
	}
	user_port  = atoi(Zebra::global["user_port"].c_str());
	if(!zMNetService::bind("bind user_port", user_port))
	{
	    return false;
	}
	FLTimeTick::getInstance().start();

	return true;
}

/**
* \brief 新建立一个连接任务
* 实现纯虚函数<code>zMNetService::newTCPTask</code>
* \param sock TCP/IP连接
* \param srcPort 连接来源端口
* \return 新的连接任务
*/
void FLService::newTCPTask(const int sock,const WORD srcPort)
{  
	Zebra::logger->debug("FLService::newTCPTask");

	if (srcPort == login_port)
	{
		//客户端登陆验证连接
		zTCPTask *tcpTask = new LoginTask(loginTaskPool,sock);
		if (NULL == tcpTask)
			::close(sock);
		else if (!loginTaskPool->addVerify(tcpTask))
		{
			SAFE_DELETE(tcpTask);
		}
	}
	else if (srcPort == inside_port)
	{
		//每个区的管理服务器连接
		zTCPTask *tcpTask = new ServerTask(serverTaskPool,sock);
		if (NULL == tcpTask)
			::close(sock);
		else if (!serverTaskPool->addVerify(tcpTask))
		{
			SAFE_DELETE(tcpTask);
		}
	}
	else if(srcPort == user_port)
	{
	    zTCPTask *tcpTask = new UserTask(userTaskPool,sock);
	    if (NULL == tcpTask)
		::close(sock);
	    else if (!userTaskPool->addVerify(tcpTask))
	    {
		SAFE_DELETE(tcpTask);
	    }
	}
	else if (srcPort == ping_port)
	{
#if 0
		// 获取PING服务器列表
		zTCPTask *tcpTask = new PingTask(serverTaskPool,sock);
		if (NULL == tcpTask)
			::close(sock);
		else if (!pingTaskPool->addVerify(tcpTask))
		{
			SAFE_DELETE(tcpTask);
		}
#endif
	}
	else
		::close(sock);
}

/**
* \brief 结束网络服务器
*
* 实现了纯虚函数<code>zService::final</code>
*
*/
void FLService::final()
{
	zMNetService::final();

	ServerManager::delInstance();
	LoginManager::delInstance();
	GYListManager::delInstance();

	FLTimeTick::getInstance().final();
	FLTimeTick::getInstance().join();
	FLTimeTick::delInstance();

	////---InfoClientManager::delInstance();
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
class LoginConfile:public zConfile
{
	bool parseYour(const xmlNodePtr node)
	{
		if (node)
		{
			xmlNodePtr child=parser.getChildNode(node,NULL);
			while(child)
			{
				if (strcmp((char *)child->name,"InfoServer")==0)
				{
					char buf[64];
					if (parser.getNodeContentStr(child,buf,64))
					{
						Zebra::global["InfoServer"]=buf;
						if (parser.getNodePropStr(child,"port",buf,64))
							Zebra::global["InfoPort"]=buf;
					}
				}
				else
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
void FLService::reloadConfig()
{
	LoginConfile sc;
	sc.parse("FLServer");
	Zebra::logger->setLevel(Zebra::global["log"]);
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
	Zebra::logger=new zLogger("FLServer");

	//设置缺省参数
	Zebra::global["login_port"]    = "7000";
	Zebra::global["inside_port"]   = "7001";
	Zebra::global["ping_port"]     = "7002";
	Zebra::global["InfoServer"]    = "127.0.0.1";
	Zebra::global["InfoPort"]      = "9903";
	Zebra::global["configdir"]  = "Config/";

	//解析配置文件参数
	LoginConfile sc;
	if (!sc.parse("FLServer")) return EXIT_FAILURE;
	//解析命令行参数
	zArg::getArg()->add(super_options, super_parse_opt, 0, super_doc);
	zArg::getArg()->parse(argc, argv);
	if (atoi(Zebra::global["maxGatewayUser"].c_str()))
	{
		LoginManager::maxGatewayUser = atoi(Zebra::global["maxGatewayUser"].c_str());
	}

	//设置日志级别
	Zebra::logger->setLevel(Zebra::global["log"]);
	//设置写本地日志文件
	if ("" != Zebra::global["logfilename"]){
		Zebra::logger->addLocalFileLog(Zebra::global["logfilename"]);
	}
	if("true" == Zebra::global["daemon"])
	{
		Zebra::logger->info("FLServer Program will be run as a daemon");
		Zebra::logger->removeConsoleLog();
		daemon(1,1);
	}  
	FLService::getMe().main();
	FLService::delMe();

	return EXIT_SUCCESS;
}
