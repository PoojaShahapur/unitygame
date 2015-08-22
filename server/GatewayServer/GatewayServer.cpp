#include <time.h>
#include "zConfile.h"
#include "Zebra.h"
#include "zMisc.h"
#include "zSubNetService.h"
#include "zXMLParser.h"
#include "GmToolCommand.h"
#include "BillClient.h"



#include "GateUserManager.h"
#include "GatewayServer.h"
#include "GatewayTask.h"
#include "GatewayTaskManager.h"
#include "LoginSessionManager.h"
#include "MiniClient.h"
#include "RecordClient.h"
#include "SceneClient.h"
//#include "SceneClientManager.h"
#include "zArg.h"
#include "TimeTick.h"
#include "RolechangeCommand.h"
#include "WordFilterManager.h"
#include "GmIPFilter.h"
/*ay服务器,负责用户指令检查转发、加密解密等
 */



GatewayService *GatewayService::instance = NULL;
zTCPTaskPool * GatewayService::taskPool = NULL;
bool GatewayService::service_gold=true;
bool GatewayService::service_stock=true;
DWORD merge_version = 0;

/**
 * \brief 初始化网络服务器程序
 *
 * 实现了虚函数<code>zService::init</code>
 *
 * \return 是否成功
 */
bool GatewayService::init()
{
  Zebra::logger->debug("GatewayService::init");
  verify_client_version = atoi(VERSION_STRING);
  Zebra::logger->info("服务器版本号:%d",verify_client_version);

  //加载国家名称(地图)信息
  if (!country_info.init())
  {
    Zebra::logger->error("加载地图名称失败!");
  }

  //初始化连接线程池
  int state = state_none;
  Zebra::to_lower(Zebra::global["threadPoolState"]);
  if ("repair" == Zebra::global["threadPoolState"]
      || "maintain" == Zebra::global["threadPoolState"])
    state = state_maintain;

  taskPool = new zTCPTaskPool(4000, state, 50000);//网关可以连4000个
  if (NULL == taskPool
      || !taskPool->init())
    return false;

  strncpy(pstrIP,zSocket::getIPByIfName(Zebra::global["ifname"].c_str()),MAX_IP_LENGTH - 1);
  Zebra::logger->info("GatewayService::init(%s)",pstrIP);

  if (!zSubNetService::init())
  {
    return false;
  }

  const Cmd::Super::ServerEntry *serverEntry = NULL;
  
  //连接会话服务器
  serverEntry = getServerEntryByType(SESSIONSERVER);
  if (NULL == serverEntry)
  {
    Zebra::logger->error("不能找到会话服务器相关信息,不能连接会话服务器");
    return false;
  }
  sessionClient = new SessionClient("会话服务器客户端",serverEntry->pstrIP,serverEntry->wdPort);
  if (NULL == sessionClient)
  {
    Zebra::logger->error("没有足够内存,不能建立会话服务器客户端实例");
    return false;
  }
  if (!sessionClient->connectToSessionServer())
  {
    Zebra::logger->error("GatewayService::init 连接会话服务器失败");
    //return false;
  }
  sessionClient->start();

  //连接计费服务器
  serverEntry = getServerEntryByType(BILLSERVER);
  if (NULL == serverEntry)
  {
    Zebra::logger->error("不能找到计费服务器相关信息,不能连接计费服务器");
    return false;
  }
  accountClient = new BillClient("计费服务器客户端",serverEntry->pstrIP,serverEntry->wdPort,serverEntry->wdServerID);
  if (NULL == accountClient)
  {
    Zebra::logger->error("没有足够内存,不能建立计费服务器客户端实例");
    return false;
  }
  if (!accountClient->connectToBillServer())
  {
    Zebra::logger->error("GatewayService::init 连接计费服务器失败");
    return false;
  }
  accountClient->start();

  //连接所有的场景服务器
  serverEntry = getServerEntryByType(SCENESSERVER);
  if (serverEntry)
  {
    if (!SceneClientManager::getInstance().init())
      return false;
  }  

  //连接所有的档案服务器
  serverEntry = getServerEntryByType(RECORDSERVER);
  if (NULL == serverEntry)
  {
    Zebra::logger->error("不能找到档案服务器相关信息,不能连接档案服务器");
    return false;
  }
  recordClient = new RecordClient("档案服务器客户端",serverEntry->pstrIP,serverEntry->wdPort);
  if (NULL == recordClient)
  {
    Zebra::logger->error("没有足够内存,不能建立档案服务器客户端实例");
    return false;
  }
  if (!recordClient->connectToRecordServer())
  {
    Zebra::logger->error("GatewayService::init 连接档案服务器失败");
    return false;
  }
  recordClient->start();
#if 0
  //连接小游戏服务器
  serverEntry = getServerEntryByType(MINISERVER);
  if (NULL == serverEntry)
  {
    Zebra::logger->error("不能找到小游戏服务器相关信息,不能连接小游戏服务器");
    return false;
  }
  miniClient = new MiniClient("小游戏服务器客户端",serverEntry->pstrIP,serverEntry->wdPort,serverEntry->wdServerID);
  if (NULL == miniClient)
  {
    Zebra::logger->error("没有足够内存,不能建立小游戏服务器客户端实例");
    return false;
  }
  if (!miniClient->connectToMiniServer())
  {
    Zebra::logger->error("GatewayService::init 连接小游戏服务器失败");
    return false;
  }
  miniClient->start();
#endif
  if (!GateUserManager::getInstance()->init())
    return false;

  WordFilterManager::getMe().loadFile();
  GmIPFilter::getMe().init();

  GatewayTimeTick::getInstance().start();

  Zebra::logger->debug("初始化成功完成！");
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
void GatewayService::newTCPTask(const int sock,const struct sockaddr_in *addr)
{
  Zebra::logger->debug("GatewayService::newTCPTask");
  GatewayTask *tcpTask = new GatewayTask(taskPool,sock,addr);
  if (NULL == tcpTask)
    //内存不足,直接关闭连接
    TEMP_FAILURE_RETRY(::close(sock));
  else if (!taskPool->addVerify(tcpTask))
  {
    //得到了一个正确连接,添加到验证队列中
    SAFE_DELETE(tcpTask);
  }
}

bool GatewayService::notifyLoginServer()
{
  Zebra::logger->debug("GatewayService::notifyLoginServer");
  using namespace Cmd::Super;
  t_GYList_Gateway tCmd;

  tCmd.wdServerID = wdServerID;
  tCmd.wdPort     = wdPort;
  strncpy(tCmd.pstrIP,pstrIP,sizeof(tCmd.pstrIP));
  if (!GatewayService::getInstance().isTerminate())
  {
    tCmd.wdNumOnline = getPoolSize();
    //printf("网关目前在线人数:%d\n",tCmd.wdNumOnline);
  }
  else
  {
    tCmd.wdNumOnline = 0;
  }
  tCmd.state = getPoolState();
  tCmd.zoneGameVersion = verify_client_version;

  return sendCmdToSuperServer(&tCmd,sizeof(tCmd));
}

/**
 * \brief 解析来自管理服务器的指令
 *
 * 这些指令是网关和管理服务器交互的指令<br>
 * 实现了虚函数<code>zSubNetService::msgParse_SuperService</code>
 *
 * \param pNullCmd 待解析的指令
 * \param nCmdLen 待解析的指令长度
 * \return 解析是否成功
 */
bool GatewayService::msgParse_SuperService(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
    using namespace Cmd::Super;

    if (CMD_GATEWAY == pNullCmd->cmd)
    {
	switch(pNullCmd->para)
	{
	    case PARA_GATEWAY_RQGYLIST:
		Zebra::logger->info("PARA_GATEWAY_RQGYLIST");
		return notifyLoginServer();
	    case PARA_NOTIFYGATE_FINISH:
		{
		    if(!startUpFinish)
			startUpFinish = true;
		}
		break;
	    case PARA_CHARNAME_GATEWAY:
		{
		    t_Charname_Gateway *rev = (t_Charname_Gateway *)pNullCmd;

		    Zebra::logger->info("PARA_CHARNAME_GATEWAY");
		    GateUser *pUser=GateUserManager::getInstance()->getUserByAccID(rev->accid);
		    if (pUser
			    && rev->state & ROLEREG_STATE_WRITE)
		    {
			if(rev->state & ROLEREG_STATE_OK)
			{
			    pUser->createCharCmd.charid = rev->charid;
			    if (!recordClient->sendCmd(&pUser->createCharCmd,sizeof(pUser->createCharCmd)))
				return false;
			}
			else if (rev->state)
			{
			    //创建角色失败,角色名称重复
			    pUser->nameRepeat();
			    Zebra::logger->warn("角色名重复 GatewayService::msgParse_SuperService");
			}
		    }

		    return true;
		}
		break;
	    case PARA_CHANGE_ZONE_DEL:
		{
		    t_ChangeZoneDel_Gateway* rev = (t_ChangeZoneDel_Gateway*)pNullCmd;
		    Zebra::logger->debug("[转区] 转区成功,删除角色");

		    Cmd::Session::t_DelChar_GateSession send;
		    send.accid = rev->accid;
		    send.id = rev->userid;
		    send.type = 1;
		    strncpy(send.name, rev->name, MAX_NAMESIZE);
		    sessionClient->sendCmd(&send, sizeof(send));
		    return true;
		}
		break;
	    default:
		break;
	}
    }
    else if(Cmd::Rolechange::CMD_BATTLE == pNullCmd->cmd)
    {
	Cmd::Rolechange::t_retCheckZoneState* rev = (Cmd::Rolechange::t_retCheckZoneState*)pNullCmd;
	GateUser* pUser = GateUserManager::getInstance()->getUserByAccID(rev->accid);
	if(pUser && pUser->isSelectState())
	{
	    const Cmd::SelectUserInfo* info = pUser->getSelectUserInfoByID(rev->charid);
	    if(info && info->zone_state == CHANGEZONE_CHANGED && rev->type==0)
	    {
		Cmd::Record::t_update_ZoneState send;
		send.accid = rev->accid;
		send.charid = rev->charid;
		send.state = 0;
		if(!recordClient->sendCmd(&send, sizeof(send)))
		    return true;

		pUser->TerminateWait();
		Zebra::logger->trace("[状态检查] 转区后登陆 解锁下线(%u,%u)",rev->accid, rev->charid);
	    }
	    else if(info && info->zone_state == CHANGEZONE_CHANGED && rev->type==2)
	    {
		Cmd::stRetUnlockChangeZone send;
		send.type = Cmd::UNLOCK_SUCCESS;
		send.charNo = pUser->getCharNo(rev->charid);
		pUser->sendCmd(&send, sizeof(send));
	    }
	    else if(rev->type == 1)
	    {
		Cmd::stRetUnlockChangeZone send;
		send.type = Cmd::UNLOCK_FAIL;
		send.charNo = pUser->getCharNo(rev->charid);
		pUser->sendCmd(&send, sizeof(send));
	    }
	    else if(rev->type == 3)
	    {
		Cmd::stRetUnlockChangeZone send;
		send.type = Cmd::UNLOCK_TARGETTIMEFAIL;
		send.charNo = pUser->getCharNo(rev->charid);
		send.dwTime = rev->dwTime;
		pUser->sendCmd(&send, sizeof(send));
	    }
	    else if(rev->type == 4)
	    {
		Cmd::stRetUnlockChangeZone send;
		send.type = Cmd::UNLOCK_NORMAL;
		send.charNo = pUser->getCharNo(rev->charid);
		pUser->sendCmd(&send, sizeof(send));
	    }
	    else if(rev->type == 5)
	    {
		Cmd::stRetUnlockChangeZone send;
		send.type = Cmd::UNLOCK_ONLINEINKF;
		send.charNo = pUser->getCharNo(rev->charid);
		pUser->sendCmd(&send, sizeof(send));
	    }
	    else if(rev->type == 6)
	    {
		Cmd::stServerReturnLoginFailedCmd send;
		send.byReturnCode = Cmd::LOGIN_RETURN_ALREADY_TOZONE;
		pUser->sendCmd(&send, sizeof(send));
	    }
	}
	return true;
    }

    Zebra::logger->error("GatewayService::msgParse_SuperService(%d,%d,%d)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
    return false;
}

bool GatewayService::sendCmdToZone(DWORD zone, const void* cmd, int len)
{
    if(!cmd) 
	return false;
    BYTE buf[zSocket::MAX_DATASIZE];
    bzero(buf, sizeof(buf));
    Cmd::Super::t_ForwardMsg_Scene *send = (Cmd::Super::t_ForwardMsg_Scene *)buf;
    constructInPlace(send);
    send->toGameZone.id = zone;
    send->size = len;
    bcopy(cmd, send->msg, len);
    GatewayService::getInstance().sendCmdToSuperServer(send, sizeof(Cmd::Super::t_ForwardMsg_Scene)+len);
    return true;
}

/**
 * \brief 结束网络服务器
 *
 * 实现了纯虚函数<code>zService::final</code>
 *
 */
void GatewayService::final()
{
  Zebra::logger->debug("GatewayService::final");
  GatewayTimeTick::getInstance().final();
  GatewayTimeTick::getInstance().join();
  GatewayTimeTick::delInstance();
  GateUserManager::getInstance()->removeAllUser(); 

  if (taskPool)
  {
    taskPool->final();
    SAFE_DELETE(taskPool);
  }
  //必须放再taskPool之后处理,否则会down机
  //SceneClientManager::getInstance().final();
  SceneClientManager::delInstance();
  // */
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
  zSubNetService::final();

  GatewayTaskManager::delInstance();

  LoginSessionManager::delInstance();

  GateUserManager::delInstance();

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
class GatewayConfile:public zConfile
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
void GatewayService::reloadConfig()
{
  Zebra::logger->debug("GatewayService::reloadConfig");
#if 0
  GatewayConfile gc;
  gc.parse("GatewayServer");
  if ("true" == Zebra::global["rolereg_verify"])
    GatewayService::getInstance().rolereg_verify = true;
  else
    GatewayService::getInstance().rolereg_verify = false;
  
  //指令检测开关
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
  
  if (!country_info.reload())
  {
    Zebra::logger->error("重新加载国家配置!");
  }

  merge_version = atoi(Zebra::global["merge_version"].c_str());
#ifdef _DEBUG
  Zebra::logger->debug("[合区]: 重新加载合区版本号",merge_version);
#endif  
#endif
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
  Zebra::logger=new zLogger("GatewayServer");

  //设置缺省参数
  Zebra::global["countryorder"] = "0";
  Zebra::global["configdir"]  = "Config/";


  //解析配置文件参数
  GatewayConfile gc;
  if (!gc.parse("GatewayServer"))
    return EXIT_FAILURE;
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

	        Zebra::logger->info("GatewayServer Program will be run as a daemon");
		      Zebra::logger->removeConsoleLog();
		            daemon(1, 1); 
			      }
  Zebra::logger->debug("rolereg_verify %s",Zebra::global["rolereg_verify"].c_str());
  if ("1" == Zebra::global["rolereg_verify"])
    GatewayService::getInstance().rolereg_verify = true;
  else
    GatewayService::getInstance().rolereg_verify = false;
  //指令检测开关
  if (Zebra::global["cmdswitch"] == "true")
  {
    //zTCPTask::analysis._switch = true;
    //zTCPClient::analysis._switch=true;
  }
  else
  {
    //zTCPTask::analysis._switch = false;
    //zTCPClient::analysis._switch=false;
  }

  merge_version = atoi(Zebra::global["merge_version"].c_str());
#if 0
  Zebra_Startup();
#endif
  GatewayService::getInstance().main();
  //GatewayService::delInstance();

  return EXIT_SUCCESS;
}

