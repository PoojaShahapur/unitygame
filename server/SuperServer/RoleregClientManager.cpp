#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "RoleregClientManager.h"
#include "RoleregClient.h"
#include "zXMLParser.h"
/*现统一用户平台客户端连接的管理容器
 */




/**
 * \brief 类的唯一实例指针
 */
RoleregClientManager *RoleregClientManager::instance = NULL;

/**
 * \brief 构造函数
 */
RoleregClientManager::RoleregClientManager()
{
  roleregClientPool = NULL;
}

/**
 * \brief 析构函数
 */
RoleregClientManager::~RoleregClientManager()
{
  SAFE_DELETE(roleregClientPool);
}

/**
 * \brief 初始化管理器
 * \return 初始化是否成功
 */
bool RoleregClientManager::init()
{
  Zebra::logger->debug("RoleregClientManager::init");
  roleregClientPool = new zTCPClientTaskPool();
  if (NULL == roleregClientPool
      || !roleregClientPool->init())
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
    xmlNodePtr zebra_node = xml.getChildNode(root,"RoleregServerList");
    while(zebra_node)
    {
      if (strcmp((char *)zebra_node->name,"RoleregServerList") == 0)
      {
        xmlNodePtr node = xml.getChildNode(zebra_node,"server");
        while(node)
        {
          if (strcmp((char *)node->name,"server") == 0)
          {
	     Zebra::global["RoleregServer"]   = "";
            Zebra::global["RoleregPort"]   = "";

            if (xml.getNodePropStr(node,"ip", Zebra::global["RoleregServer"])
                && xml.getNodePropStr(node,"port", Zebra::global["RoleregPort"]))
            {
              Zebra::logger->debug("LoginServer: %s,%s",Zebra::global["RoleregServer"].c_str(),Zebra::global["RoleregPort"].c_str());
              roleregClientPool->put(new RoleregClient(Zebra::global["RoleregServer"], atoi(Zebra::global["RoleregPort"].c_str())));
            }
          }

          node = xml.getNextNode(node,NULL);
        }
      }

      zebra_node = xml.getNextNode(zebra_node,NULL);
    }
  }
  Zebra::logger->info("RoleregClientManager::init OK");
  return true;
}

/**
 * \brief 周期间隔进行连接的断线重连工作
 * \param ct 当前时间
 */
void RoleregClientManager::timeAction(const zTime &ct)
{
  Zebra::logger->debug("RoleregClientManager::timeAction");
  if (actionTimer.elapse(ct) > 4)
  {
    if (roleregClientPool)
      roleregClientPool->timeAction(ct);
    actionTimer = ct;
  }
}

/**
 * \brief 向容器中添加已经成功的连接
 * \param RoleregClient 待添加的连接
 */
void RoleregClientManager::add(RoleregClient *roleregClient)
{
  Zebra::logger->debug("RoleregClientManager::add");
  if (roleregClient)
  {
    zRWLock_scope_wrlock scope_wrlock(rwlock);
    const_iter it = find(allClients.begin(), allClients.end(), roleregClient);
    if (it == allClients.end())
    {
      allClients.insert(roleregClient);
    }
  }
}

/**
 * \brief 从容器中移除断开的连接
 * \param RoleregClient 待移除的连接
 */
void RoleregClientManager::remove(RoleregClient *roleregClient)
{
  Zebra::logger->debug("RoleregClientManager::remove");
  if (roleregClient)
  {
    zRWLock_scope_wrlock scope_wrlock(rwlock);
    iter it = find(allClients.begin(), allClients.end(), roleregClient);
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
bool RoleregClientManager::broadcastOne(const void *pstrCmd,int nCmdLen)
{
  Zebra::logger->debug("RoleregClientManager::broadcastOne");
  zRWLock_scope_rdlock scope_rdlock(rwlock);
  for(iter it = allClients.begin(); it != allClients.end(); ++it)
  {
      if((*it)->sendCmd(pstrCmd,nCmdLen))
	  return true;
  }
  return false;
}


