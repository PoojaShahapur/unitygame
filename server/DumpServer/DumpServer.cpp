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
#include "DumpServer.h"
#include "HttpTask.h"

/*rief 实现管理服务器
*
* 对一个区中的所有服务器进行管理
* 
*/

zDBConnPool *DumpServer::dbConnPool = NULL;

DumpServer::DumpServer() : zNetService("DumpServer"), httpTaskPool(NULL)
{

}

DumpServer *DumpServer::instance = NULL;

/**
* \brief 初始化网络服务器程序
*
* 实现纯虚函数<code>zService::init</code>
*
* \return 是否成功
*/
bool DumpServer::init()
{
	Zebra::logger->info("DumpServer::init");

	dbConnPool = zDBConnPool::newInstance(NULL);
	if (NULL == dbConnPool
		|| !dbConnPool->putURL(0,Zebra::global["mysql"].c_str(),false))
	{
	    Zebra::logger->error("DumpServer connect mysql error");
		return false;
	}

	httpTaskPool = new zHttpTaskPool();
	if (NULL == httpTaskPool
		|| !httpTaskPool->init())
		return false;

	if (!zNetService::init(atoi(Zebra::global["http_port"].c_str())))
	{
		return false;
	}

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
void DumpServer::newTCPTask(const int sock,const struct sockaddr_in *addr)
{
	HttpTask *tcpTask = new HttpTask(httpTaskPool,sock);
	if (NULL == tcpTask)
		//内存不足,直接关闭连接
		TEMP_FAILURE_RETRY(::close(sock));
	else if (!httpTaskPool->addHttp(tcpTask))
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
void DumpServer::final()
{
    zNetService::final();
    SAFE_DELETE(httpTaskPool);
    zDBConnPool::delInstance(&dbConnPool);
    Zebra::logger->debug("DumpServer::final");
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

static char super_doc[] = "\nDumpServer\n" "\tgame server manager";

const char *argp_program_version = "Program version ";
									
/**
* \brief 读取配置文件
*
*/
class DumpConfile:public zConfile
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
* \brief 主程序入口
*
* \param argc 参数个数
* \param argv 参数列表
* \return 运行结果
*/
int main(int argc,char **argv)
{
	Zebra::logger=new zLogger("DumpServer");
	Zebra::global["configdir"]  = "Config/";
	Zebra::global["http_port"]  = "8080";
	Zebra::global["logfilename"]  = "/log/dumpserver.log";

	//解析命令行参数
	zArg::getArg()->add(super_options, super_parse_opt, 0, super_doc);
	zArg::getArg()->parse(argc, argv);
	
	//解析配置文件参数
	DumpConfile sc;
	if (!sc.parse("DumpServer"))
		return EXIT_FAILURE;

	//设置日志级别
	Zebra::logger->setLevel(Zebra::global["log"]);
	//设置写本地日志文件
	if ("" != Zebra::global["logfilename"]){
		Zebra::logger->addLocalFileLog(Zebra::global["logfilename"]);
	}
	if("true" == Zebra::global["daemon"])
	{
	    Zebra::logger->info("DumpServer Program will be run as a daemon");
	    Zebra::logger->removeConsoleLog();
	    daemon(1,1);
	}

	DumpServer::getInstance().main();
	DumpServer::delInstance();

	return EXIT_SUCCESS;
}

