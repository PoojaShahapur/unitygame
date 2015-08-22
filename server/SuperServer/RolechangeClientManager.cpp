#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "RolechangeClientManager.h"
#include "RolechangeClient.h"
#include "zXMLParser.h"
/*现统一用户平台客户端连接的管理容器
 */




/**
 * \brief 类的唯一实例指针
 */
RolechangeClientManager *RolechangeClientManager::instance = NULL;

/**
 * \brief 构造函数
 */
RolechangeClientManager::RolechangeClientManager()
{
  rolechangeClientPool = NULL;
}

/**
 * \brief 析构函数
 */
RolechangeClientManager::~RolechangeClientManager()
{
  SAFE_DELETE(rolechangeClientPool);
}

/**
 * \brief 初始化管理器
 * \return 初始化是否成功
 */
bool RolechangeClientManager::init()
{
  Zebra::logger->debug("RolechangeClientManager::init");
  rolechangeClientPool = new zTCPClientTaskPool();
  if (NULL == rolechangeClientPool
      || !rolechangeClientPool->init())
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
    xmlNodePtr zebra_node = xml.getChildNode(root,"RolechangeServerList");
    while(zebra_node)
    {
      if (strcmp((char *)zebra_node->name,"RolechangeServerList") == 0)
      {
        xmlNodePtr node = xml.getChildNode(zebra_node,"server");
        while(node)
        {
          if (strcmp((char *)node->name,"server") == 0)
          {
	     Zebra::global["RolechangeServer"]   = "";
            Zebra::global["RolechangePort"]   = "";

            if (xml.getNodePropStr(node,"ip", Zebra::global["RolechangeServer"])
                && xml.getNodePropStr(node,"port", Zebra::global["RolechangePort"]))
            {
              Zebra::logger->debug("LoginServer: %s,%s",Zebra::global["RolechangeServer"].c_str(),Zebra::global["RolechangePort"].c_str());
              rolechangeClientPool->put(new RolechangeClient(Zebra::global["RolechangeServer"], atoi(Zebra::global["RolechangePort"].c_str())));
            }
          }

          node = xml.getNextNode(node,NULL);
        }
      }

      zebra_node = xml.getNextNode(zebra_node,NULL);
    }
  }
  Zebra::logger->info("RolechangeClientManager::init OK");
  return true;
}

/**
 * \brief 周期间隔进行连接的断线重连工作
 * \param ct 当前时间
 */
void RolechangeClientManager::timeAction(const zTime &ct)
{
  Zebra::logger->debug("RolechangeClientManager::timeAction");
  if (actionTimer.elapse(ct) > 4)
  {
    if (rolechangeClientPool)
      rolechangeClientPool->timeAction(ct);
    actionTimer = ct;
  }
}

/**
 * \brief 向容器中添加已经成功的连接
 * \param RolechangeClient 待添加的连接
 */
void RolechangeClientManager::add(RolechangeClient *rolechangeClient)
{
  Zebra::logger->debug("RolechangeClientManager::add");
  if (rolechangeClient)
  {
    zRWLock_scope_wrlock scope_wrlock(rwlock);
    const_iter it = allClients.find(rolechangeClient->getTempID());
    if (it == allClients.end())
    {
      allClients.insert(value_type(rolechangeClient->getTempID(), rolechangeClient));
      setter.insert(rolechangeClient);
    }
  }
}

/**
 * \brief 从容器中移除断开的连接
 * \param RolechangeClient 待移除的连接
 */
void RolechangeClientManager::remove(RolechangeClient *rolechangeClient)
{
  Zebra::logger->debug("RolechangeClientManager::remove");
  if (rolechangeClient)
  {
    zRWLock_scope_wrlock scope_wrlock(rwlock);
    iter it = allClients.find(rolechangeClient->getTempID());
    if (it != allClients.end())
    {
      allClients.erase(it);
      setter.erase(rolechangeClient);
    }
  }
}

/**
 * \brief 向成功的所有连接广播指令
 * \param pstrCmd 待广播的指令
 * \param nCmdLen 待广播指令的长度
 */
bool RolechangeClientManager::broadcastOne(const void *pstrCmd,int nCmdLen)
{
  Zebra::logger->debug("RolechangeClientManager::broadcastOne");
  zRWLock_scope_rdlock scope_rdlock(rwlock);
  for(RolechangeClient_set::iterator it = setter.begin(); it != setter.end(); ++it)
  {
      if((*it)->sendCmd(pstrCmd,nCmdLen))
	  return true;
  }
  return false;
}

bool RolechangeClientManager::sendTo(const DWORD tempid, const void *pstrCmd,int nCmdLen)
{
  zRWLock_scope_rdlock scope_rdlock(rwlock);
  iter it = allClients.find(tempid);
  if(it != allClients.end())
  {
      return it->second->sendCmd(pstrCmd,nCmdLen);
  }
  return false;

}
