#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "BroadClientManager.h"
#include "BroadClient.h"
#include "zXMLParser.h"
/*现统一用户平台客户端连接的管理容器
 */




/**
 * \brief 类的唯一实例指针
 */
BroadClientManager *BroadClientManager::instance = NULL;

/**
 * \brief 构造函数
 */
BroadClientManager::BroadClientManager()
{
  flClientPool = NULL;
}

/**
 * \brief 析构函数
 */
BroadClientManager::~BroadClientManager()
{
  SAFE_DELETE(flClientPool);
}

/**
 * \brief 初始化管理器
 * \return 初始化是否成功
 */
bool BroadClientManager::init()
{
  Zebra::logger->debug("BroadClientManager::init");
  flClientPool = new zTCPClientTaskPool(atoi(Zebra::global["threadPoolClient"].c_str()));
  if (NULL == flClientPool
      || !flClientPool->init())
    return false;

  zXMLParser xml;
  if (!xml.initFile(Zebra::global["configdir"] + "loginServerList.xml"))
  {
    Zebra::logger->error("加载统一用户平台登陆服务器列表文件失败");
    return false;
  }
  xmlNodePtr root = xml.getRootNode("Zebra");
  if (root)
  {
    xmlNodePtr zebra_node = xml.getChildNode(root,"BroadServerList");
    while(zebra_node)
    {
      if (strcmp((char *)zebra_node->name,"BroadServerList") == 0)
      {
        xmlNodePtr node = xml.getChildNode(zebra_node,"server");
        while(node)
        {
          if (strcmp((char *)node->name,"server") == 0)
          {
	     Zebra::global["BroadServer"]   = "";
            Zebra::global["BroadPort"]   = "";

            if (xml.getNodePropStr(node,"ip", Zebra::global["BroadServer"])
                && xml.getNodePropStr(node,"port", Zebra::global["BroadPort"]))
            {
              Zebra::logger->debug("BroadServer: %s,%s",Zebra::global["BroadServer"].c_str(),Zebra::global["BroadPort"].c_str());
              flClientPool->put(new BroadClient(Zebra::global["BroadServer"], atoi(Zebra::global["BroadPort"].c_str())));
            }
          }

          node = xml.getNextNode(node,NULL);
        }
      }

      zebra_node = xml.getNextNode(zebra_node,NULL);
    }
  }
  Zebra::logger->info("BroadClientManager::init OK");
  return true;
}

/**
 * \brief 周期间隔进行连接的断线重连工作
 * \param ct 当前时间
 */
void BroadClientManager::timeAction(const zTime &ct)
{
  Zebra::logger->debug("BroadClientManager::timeAction");
  if (actionTimer.elapse(ct) > 4)
  {
    if (flClientPool)
      flClientPool->timeAction(ct);
    actionTimer = ct;
  }
}

/**
 * \brief 向容器中添加已经成功的连接
 * \param flClient 待添加的连接
 */
void BroadClientManager::add(BroadClient *flClient)
{
  Zebra::logger->debug("BroadClientManager::add");
  if (flClient)
  {
    zRWLock_scope_wrlock scope_wrlock(rwlock);
    const_iter it = find(allClients.begin(), allClients.end(), flClient);
    if (it == allClients.end())
    {
      allClients.insert(flClient);
    }
  }
}

/**
 * \brief 从容器中移除断开的连接
 * \param flClient 待移除的连接
 */
void BroadClientManager::remove(BroadClient *flClient)
{
  Zebra::logger->debug("BroadClientManager::remove");
  if (flClient)
  {
    zRWLock_scope_wrlock scope_wrlock(rwlock);
    iter it = find(allClients.begin(), allClients.end(), flClient);
    if (it != allClients.end())
    {
      allClients.erase(it);
    }
  }
}

/**
 * \brief 向成功的所有连接广播指令
 * \param pstrCmd 待广播的指令
 * \param nCmdLen 待广播指令的长度
 */
bool BroadClientManager::broadcastOne(const void *pstrCmd,int nCmdLen)
{
  Zebra::logger->debug("BroadClientManager::broadcast");
  zRWLock_scope_rdlock scope_rdlock(rwlock);
  for(iter it = allClients.begin(); it != allClients.end(); ++it)
  {
    return ((*it)->sendCmd(pstrCmd,nCmdLen));
  }
  return false;
}

