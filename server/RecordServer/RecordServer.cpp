/**
 * \brief zebra项目档案服务器,用于创建、储存和读取档案
 *
 */

#include "RecordServer.h"
#include "RecordTask.h"
#include "RecordSessionManager.h"
#include "zConfile.h"
#include "zArg.h"
#include "RolechangeCommand.h"
#include "BinaryVersion.h"

zDBConnPool *RecordService::dbConnPool = NULL;

RecordService *RecordService::instance = NULL;

/**
 * \brief 初始化网络服务器程序
 *
 * 实现了虚函数<code>zService::init</code>
 *
 * \return 是否成功
 */
bool RecordService::init()
{
  Zebra::logger->debug("RecordService::init");
  verify_version = 0;
  dbConnPool = zDBConnPool::newInstance(NULL);
  if (NULL == dbConnPool
      || !dbConnPool->putURL(0,Zebra::global["mysql"].c_str(),false))
  {
	  Zebra::logger->debug("连接数据库失败");
  //MessageBox(NULL,"连接数据库失败","RecordServer",MB_ICONERROR);
    return false;
  }

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

  strncpy(pstrIP,zSocket::getIPByIfName(Zebra::global["ifname"].c_str()),MAX_IP_LENGTH - 1);
  //Zebra::logger->debug("%s",pstrIP);

  if (!zSubNetService::init())
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
void RecordService::newTCPTask(const int sock,const struct sockaddr_in *addr)
{
  Zebra::logger->debug("RecordService::newTCPTask");
  RecordTask *tcpTask = new RecordTask(taskPool,sock,addr);
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
 * \brief 解析来自管理服务器的指令
 *
 * 这些指令是网关和管理服务器交互的指令<br>
 * 实现了虚函数<code>zSubNetService::msgParse_SuperService</code>
 *
 * \param pNullCmd 待解析的指令
 * \param nCmdLen 待解析的指令长度
 * \return 解析是否成功
 */
bool RecordService::msgParse_SuperService(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen)
{
    switch(pNullCmd->cmd)
    {
	case Cmd::Rolechange::CMD_BATTLE:
	    {
		using namespace Cmd::Rolechange;
		switch(pNullCmd->para)
		{
		    case PARA_CHECK_ZONE_STATE:
			{
			    t_checkZoneState* rev = (t_checkZoneState*)pNullCmd;
			    
			    //这里要不要检测角色在线呢
			    
			    DWORD newuserid = 0;
			    DWORD offlineTime = 0;
			    DWORD zone_state = RecordTask::getSourceZoneInfo(rev->accid, rev->charid, 0, newuserid, offlineTime);
			    time_t now_time = time(NULL);
			    if(!(rev->isForce)
				    && offlineTime > 0 
				    && (now_time-offlineTime) < 30*60
				    && newuserid > 0)
			    {
				t_retCheckZoneState send;
				send.charid = rev->charid;
				send.accid = rev->accid;
				send.type = 3;
				send.dwTime = 1800-(now_time-offlineTime);
				RecordService::getInstance().sendCmdToZone(rev->fromGameID, &send, sizeof(send));
				return true;
			    }
			    else //删除角色数据
			    {	
				struct exist_struct
				{
				    DWORD accid;
				}__attribute__((packed));

				const dbCol exist_define[]=
				{
				    { "`ACCID`",zDBConnPool::DB_DWORD,sizeof(DWORD) },
				    { "`SOURCE_ID`",zDBConnPool::DB_DWORD,sizeof(DWORD) },
				    { "`SOURCE_ZONE`",zDBConnPool::DB_DWORD,sizeof(DWORD) },
				    { NULL,0,0}

				};
				connHandleID handle = RecordService::dbConnPool->getHandle();
				if((connHandleID)-1 == handle)
				{
				    Zebra::logger->error("不能获取数据库句柄");
				    return false;
				}
				exist_struct *es = NULL;
				char where[128];
				bzero(where, sizeof(where));
				snprintf(where, sizeof(where)-1, "ACCID='%u'", rev->accid);
				Zebra::logger->debug("[状态检查] 检查 %u 是否存在", rev->accid);

				unsigned int retcode = RecordService::dbConnPool->exeSelect(handle, "`CHARBASE`", exist_define, where, "ACCID DESC", (BYTE **)&es);
				if(es)
				{
				    SAFE_DELETE_VEC(es);
				}
				if(retcode == 0)
				{
				    RecordService::dbConnPool->putHandle(handle);

				    Zebra::logger->info("[状态检查] %u,%u 无角色,允许源区:%u 解锁",rev->accid, rev->charid, rev->fromGameID);

				    t_retCheckZoneState send;
				    send.charid = rev->charid;
				    send.accid = rev->accid;
				    send.type = 0;
				    RecordService::getInstance().sendCmdToZone(rev->fromGameID, &send, sizeof(send));
				    return true;

				}
				else
				{
				    Zebra::logger->info("[状态检查] %u,%u 有角色, zone_state:%u 删除战场数据",rev->accid, rev->charid, zone_state);
				    RecordService::dbConnPool->exeDelete(handle, "`CHARBASE`", where);
				    RecordService::dbConnPool->putHandle(handle);

				    t_retCheckZoneState send;
				    send.charid = rev->charid;
				    send.accid = rev->accid;
				    send.type = 0;
				    RecordService::getInstance().sendCmdToZone(rev->fromGameID, &send, sizeof(send));
				    return true;

				}

			    }

			    return true;
			}
			break;
		    case PARA_SEND_USER_TOZONE:
			{
			    t_sendUserToZone* rev = (t_sendUserToZone*)pNullCmd;
			    Cmd::Record::t_WriteUser_SceneRecord* temp_record = (Cmd::Record::t_WriteUser_SceneRecord*)rev->data;

			    Zebra::logger->trace("[转区] 收到 %u 区 %u,%u,%s 的 %u 类型数据传送消息",
				    rev->fromGameZone.id, rev->accid, temp_record->charbase.source_id, temp_record->charbase.name, rev->type);
			    if(temp_record->charbase.zone_state == CHANGEZONE_RETURNED)
    			    {
				DWORD newuserid = 0;
				DWORD offlineTime = 0;
				DWORD zone_state = RecordTask::getSourceZoneInfo(rev->accid, temp_record->charbase.source_id, temp_record->charbase.target_zone, newuserid, offlineTime);
				if(zone_state == CHANGEZONE_CHANGED)
				{
				    temp_record->charbase.zone_state = CHANGEZONE_NONE;
				    temp_record->charbase.id = newuserid;
				    temp_record->charbase.source_id = 0;
				    temp_record->charbase.source_zone = 0;
				    temp_record->charbase.target_zone = 0;
				    temp_record->id = temp_record->charbase.id;
				    //这里可能要设置回传的地图等等
				    if(RecordTask::saveCharBase(temp_record))
				    {
					Zebra::logger->error("[转区] 保存 %u 区 %u,%u,%s 的 %u 数据成功 通知其删除记录",
						rev->fromGameZone.id, rev->accid, temp_record->charbase.source_id, temp_record->charbase.name, rev->type);
					RecordTask::return_send_user(rev->fromGameZone.id, rev->accid, rev->userid, TYPE_BACKZONE, TOZONE_SUCCESS);
					
				    }
				    else
				    {
					Zebra::logger->error("[转区] 保存 %u 区 %u,%u,%s 的 %u 数据失败 通知其打开状态",
						rev->fromGameZone.id, rev->accid, temp_record->charbase.source_id, temp_record->charbase.name, rev->type);
					RecordTask::return_send_user(rev->fromGameZone.id, rev->accid, rev->userid, TYPE_BACKZONE, TOZONE_FAIL);

				    }

				}
				else
				{
				    Zebra::logger->error("[转区] 收到 %u 区 %u,%u,%s 的 %u 转回消息，失败",
					    rev->fromGameZone.id, rev->accid, temp_record->charbase.source_id, temp_record->charbase.name, rev->type);

				}
			    }
			    else
			    {
				int ret = RecordTask::check_valid(rev->accid, rev->userid, rev->fromGameZone.id);
				if((ret == 0)
					||(ret == 1 && (rev->type==2 || rev->type==4)))
				{
				    if(!RecordTask::newChangeZoneCharbase(rev))
				    {
					Zebra::logger->error("[转区] 保存 %u 区 %u,%u,%s 的 %u 数据失败 通知源区",
						rev->fromGameZone.id, rev->accid, temp_record->charbase.source_id, temp_record->charbase.name, rev->type);
					RecordTask::return_send_user(rev->fromGameZone.id, rev->accid, rev->userid, TYPE_TOZONE, TOZONE_FAIL);

				    }
				    else
				    {
					if(rev->type == 4)
					{
					    RecordTask::return_send_user(rev->fromGameZone.id, rev->accid, rev->userid, TYPE_TOZONE, TOZONE_SUCCESS, temp_record->charbase.name);

					}
				    }
				}
				else if(ret == 1)
				{
					Zebra::logger->error("[转区] 保存 %u 区 %u,%u,%s 的 %u 数据失败 通知源区",
						rev->fromGameZone.id, rev->accid, temp_record->charbase.source_id, temp_record->charbase.name, rev->type);
					RecordTask::return_send_user(rev->fromGameZone.id, rev->accid, rev->userid, TYPE_TOZONE, TOZONE_FAIL);

				}
			    }
			    return true;
			}
			break;
		    case PARA_RET_SEND_USER_TOZONE:
			{
			    t_retSendUserToZone* ptCmd = (t_retSendUserToZone*)pNullCmd;
			    if(ptCmd->type == TYPE_BACKZONE)
			    {
				if(ptCmd->state == TOZONE_SUCCESS)
				{//回传成功,删除战区记录
				    if(!RecordTask::delCharbase(ptCmd->accid, ptCmd->userid))
				    {
					Zebra::logger->trace("[转区] %u,%u,向%u回传成功 ,但是删除战区本地记录失败, 需要手动删除",
						ptCmd->accid, ptCmd->userid, ptCmd->fromGameZone.id);
				    }
				}
				else
				{//更新zonestate
				    DWORD zone_state = RecordTask::getZoneState(ptCmd->accid, ptCmd->userid);
				    if(zone_state == CHANGEZONE_RETURNED /*&& !RecordUserM::getMe().verify(ptCmd->accid, ptCmd->userid, 0)*/)
				    {
					Zebra::logger->trace("[转区] %u,%u,向%u回传失败 , 状态转换为 CHANGEZONE_TARGETZONE",
						ptCmd->accid, ptCmd->userid, ptCmd->fromGameZone.id);
					RecordTask::updateZoneState(ptCmd->accid, ptCmd->userid, CHANGEZONE_TARGETZONE);

				    }
				}
			    }
			    else if(ptCmd->type == TYPE_TOZONE)
			    {
				if(ptCmd->state == TOZONE_FAIL)
				{//更新zonestate
				    DWORD zone_state = RecordTask::getZoneState(ptCmd->accid, ptCmd->userid, ptCmd->fromGameZone.id);
				    if((zone_state == CHANGEZONE_CHANGED || zone_state == CHANGEZONE_FOREVER) 
					    /*&& !RecordUserM::getMe().verify(ptCmd->accid, ptCmd->userid, 0)*/)
				    {
					Zebra::logger->trace("[转区] %u,%u,向%u转区失败 , 状态转换为 CHANGEZONE_NONE",
						ptCmd->accid, ptCmd->userid, ptCmd->fromGameZone.id);
					RecordTask::updateZoneState(ptCmd->accid, ptCmd->userid, CHANGEZONE_NONE);

				    }

				}
			    }
			    return true;
			}
			break;
		    case PARA_CHECK_VALID:
			{
			    t_Check_Valid *ptCmd = (t_Check_Valid *)pNullCmd;
			    struct exist_struct
			    {
				DWORD accid;
				DWORD charid;
				DWORD source_id;
				DWORD source_zone;
				DWORD zone_state;
			    }__attribute__((packed));
			    const dbCol exist_define[] = 
			    {
				{ "`ACCID`",	  zDBConnPool::DB_DWORD,	sizeof(DWORD)},
				{ "`CHARID`",	  zDBConnPool::DB_DWORD,	sizeof(DWORD)},
				{ "`SOURCE_ID`",	  zDBConnPool::DB_DWORD,	sizeof(DWORD)},
				{ "`SOURCE_ZONE`",	  zDBConnPool::DB_DWORD,	sizeof(DWORD)},
				{ "`ZONE_STATE`",	  zDBConnPool::DB_DWORD,	sizeof(DWORD)},
				{ NULL,0,0}

			    };

			    Cmd::Rolechange::t_Ret_Check_Valid send;
			    send.toGameZone = ptCmd->fromGameZone;
			    send.accid = ptCmd->accid;
			    send.userid = ptCmd->userid;
			    send.type = ptCmd->type;

			    if(ptCmd->charbaseSize != sizeof(CharBase)
				    || ptCmd->verifyVersion != RecordService::getInstance().verify_version
				    || ptCmd->checkvalidSize != sizeof(Cmd::Rolechange::t_Check_Valid)
				    || ptCmd->objVersion != BINARY_VERSION
				    || ptCmd->checkWriteSize != sizeof(Cmd::Record::t_WriteUser_SceneRecord))
			    {
				send.state = Cmd::Rolechange::BATTLE_VERSION_INVALID;
				Zebra::logger->error("[转区] 取版本不一致 禁止转区");
				RecordService::getInstance().sendCmdToSuperServer(&send, sizeof(send));
				return true;
			    }

			    if(ptCmd->type == Cmd::Rolechange::TYPE_BACKZONE)
			    {
				send.state = Cmd::Rolechange::BATTLE_SUCCESS;
				Zebra::logger->info("[转区] 版本验证通过 可以回到本区");
				RecordService::getInstance().sendCmdToSuperServer(&send, sizeof(send));
				return true;

			    }

			    connHandleID handle = RecordService::dbConnPool->getHandle();
			    if((connHandleID)-1 == handle)
			    {
				return false;
			    }
			    exist_struct *es;
			    char where[128];
			    bzero(where, sizeof(where));
			    snprintf(where, sizeof(where)-1, "ACCID='%u'", ptCmd->accid);
			    unsigned int retcode = RecordService::dbConnPool->exeSelect(handle, "`CHARBASE`", exist_define, where, "ACCID DESC", (BYTE **)&es);
			    RecordService::dbConnPool->putHandle(handle);
			    if(es)
			    {

				SAFE_DELETE_VEC(es);
			    }
			    if(retcode > 0)
			    {
				send.state = Cmd::Rolechange::BATTLE_USER_REPEAT;
			    }
			    else
			    {
				send.state = Cmd::Rolechange::BATTLE_SUCCESS;
			    }
			    RecordService::getInstance().sendCmdToSuperServer(&send, sizeof(send));
			    return true;
			}
			break;
		    default:
			break;
		}
	    }
	    break;
	default:
	    break;
    }
    Zebra::logger->error("RecordService::msgParse_SuperService(%u,%u,%u)",pNullCmd->cmd,pNullCmd->para,nCmdLen);
    return false;
}

bool RecordService::sendCmdToZone(DWORD zone, const void* cmd, int len)
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
    RecordService::getInstance().sendCmdToSuperServer(send, sizeof(Cmd::Super::t_ForwardMsg_Scene)+len);
    return true;
}

/**
 * \brief 结束网络服务器
 *
 * 实现了纯虚函数<code>zService::final</code>
 *
 */
void RecordService::final()
{
  Zebra::logger->debug("RecordService::final");
  while(!RecordSessionManager::getInstance().empty())
  {
    zThread::msleep(10);
  }
  if (taskPool)
  {
    taskPool->final();
    SAFE_DELETE(taskPool);
  }
  zSubNetService::final();

  RecordSessionManager::delInstance();

  zDBConnPool::delInstance(&dbConnPool);

  Zebra::logger->debug("RecordService::final");
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
class RecordConfile:public zConfile
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
void RecordService::reloadConfig()
{
  Zebra::logger->debug("RecordService::reloadConfig");
#if 0 
  RecordConfile rc;
  rc.parse("RecordServer");
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


  Zebra::logger=new zLogger("RecordServer");

  Zebra::global["configdir"]  = "Config/";

  //设置缺省参数

  //解析配置文件参数
  RecordConfile rc;
  if (!rc.parse("RecordServer"))
  {
    return EXIT_FAILURE;
  }
//解析命令行参数
	zArg::getArg()->add(super_options, super_parse_opt, 0, super_doc);
	zArg::getArg()->parse(argc, argv);

  //指令检测开关
  if (Zebra::global[std::string("cmdswitch")] == "true")
  {
    //zTCPTask::analysis._switch = true;
    //zTCPClient::analysis._switch=true;
  }
  else
  {
    //zTCPTask::analysis._switch = false;
    //zTCPClient::analysis._switch=false;
  }


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
  
  RecordService::getInstance().main();
  RecordService::delInstance();

  return EXIT_SUCCESS;
}
#if 0
int __stdcall WinMain( HINSTANCE, HINSTANCE, LPSTR, int )
{
	return 1;
}
#endif

