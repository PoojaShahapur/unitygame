/**
 * \brief zebra项目档案服务器,用于创建、储存和读取档案
 *
 */

#include "ClientService.h"
#include "zConfile.h"
#include "zArg.h"
#include "LoginClient.h"
#include "ClientManager.h"
#include "Client.h"
#include "TimeTick.h"
ClientService *ClientService::instance = NULL;

/**
 * \brief 初始化网络服务器程序
 *
 * 实现了虚函数<code>zService::init</code>
 *
 * \return 是否成功
 */
bool ClientService::init()
{
  Zebra::logger->debug("ClientService::init");

  if(!ClientManager::getInstance().init())
  {
      Zebra::logger->error("ClientManager::init 失败！");
      return false;
  }

  ClientTimeTick::getInstance().start();

  if (!zService::init())
  {
      return false;
  }
  loginClient = new LoginClient("登录服务器", Zebra::global["server"].c_str(), atoi(Zebra::global["port"].c_str()));
  if(NULL == loginClient)
      return false;
  DWORD first_user = atoi(Zebra::global["user"].c_str());
  DWORD max_user = atoi(Zebra::global["count"].c_str());
  max_user += first_user;

  bool loginSucc = false;
  WORD loginNum = 0;
  WORD wdPort = 0;
  DWORD accid = 0;
  DWORD loginTempID = 0;
  std::ostringstream oss;
  std::ostringstream errorAccount;

  for(DWORD i=first_user; i<max_user; ++i)
  {
      oss.str("");
      oss<<Zebra::global["prefix"].c_str()<<i;
      if(loginClient->requestIP())
      {
	  DWORD retVal = 0;
	  DWORD interval = 5;
	  BYTE connTime = 1;
	  const BYTE MAX_CONN = 3;
	  for(;(retVal=loginClient->loginLoginServer(oss.str().c_str(),Zebra::global["passwd"].c_str()))==LOGIN_ID_IN_USE; ++connTime)
	  {
	      if(connTime == MAX_CONN)
	      {
		  Zebra::logger->debug("账号 %u, %d次登录失败，终止登陆 ",i,MAX_CONN);
		  break;
	      }
	      Zebra::logger->debug("账号 %u, %d次登录失败，将于:%d秒后重试",i,connTime,interval);
	      zThread::msleep(interval*1000);
	      interval *= 2;
	  }
	  if(retVal == LOGIN_SUCCESS)
	  {
	      loginSucc = true;
	  }
	  else
	  {
	      loginSucc = false;
	  }
      }
      if(loginSucc)
      {
	  char pstrIP[MAX_IP_LENGTH];
	  bzero(pstrIP, sizeof(pstrIP));
	  wdPort = 0;
	  accid = 0;
	  loginTempID = 0;
	  DES_cblock key_des;
	  loginClient->getGateInfo(pstrIP, wdPort, accid, loginTempID);
	  loginClient->get_key_des((char*)key_des);
	  Zebra::logger->debug("IP:%s port:%u accid:%u loginTempID:%u",pstrIP,wdPort,accid,loginTempID);
	  if(ClientManager::getInstance().addClientTask(new Client(oss.str(), pstrIP, wdPort),accid, loginTempID, key_des))
	  {
	      Zebra::logger->debug("登录数量:%d/%d",++loginNum, max_user-first_user);
	      continue;
	  }
      }
      Zebra::logger->debug("账号:%u登陆错误",i);
      errorAccount<<i<<" ";
  }
  Zebra::logger->debug("登陆结束，成功数量:%d/%d,错误账号:%s",loginNum, max_user-first_user, 0!=errorAccount.str().size()?errorAccount.str().c_str():"无");
  if(0 == loginNum)
      return false;
  return true;
}

/**
 * \brief 结束网络服务器
 *
 * 实现了纯虚函数<code>zService::final</code>
 *
 */
void ClientService::final()
{
  Zebra::logger->debug("ClientService::final");
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
class ClientConfile:public zConfile
{
    public:
	ClientConfile(const char* confile = "Clientconfig.xml") 
	    : zConfile(confile)
	{

	}
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


bool ClientService::serviceCallback()
{
    return true;
}

int main(int argc,char **argv)
{
  Zebra::logger=new zLogger("Client");

  Zebra::global["configdir"]  = "Config/";
  Zebra::global["log"] = "debug";
  Zebra::global["logfilename"] = "../log/client.log";
  Zebra::global["server"] = "192.168.125.254";
  Zebra::global["port"] = "10002";
  Zebra::global["user"] = "10";
  Zebra::global["prefix"] = "zhanghao";
  Zebra::global["count"] = "100";
  //设置缺省参数

  ClientConfile cc;
  if(!cc.parse("Client"))
  {
      return EXIT_FAILURE;
  }
//解析命令行参数
	zArg::getArg()->add(super_options, super_parse_opt, 0, super_doc);
	zArg::getArg()->parse(argc, argv);


  //设置日志级别
  Zebra::logger->setLevel(Zebra::global["log"]);
  //设置写本地日志文件
  if ("" != Zebra::global["logfilename"]){
    Zebra::logger->addLocalFileLog(Zebra::global["logfilename"]);
    }
    
  if ("true" == Zebra::global["daemon"])
  {
      Zebra::logger->info("RecordServer Program will be run as a daemon");
      Zebra::logger->removeConsoleLog();
      daemon(1, 1);
  }
  
  ClientService::getInstance().main();
  ClientService::delInstance();

  return EXIT_SUCCESS;
}
