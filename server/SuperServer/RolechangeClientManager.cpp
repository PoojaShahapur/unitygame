#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "RolechangeClientManager.h"
#include "RolechangeClient.h"
#include "zXMLParser.h"
/*��ͳһ�û�ƽ̨�ͻ������ӵĹ�������
 */




/**
 * \brief ���Ψһʵ��ָ��
 */
RolechangeClientManager *RolechangeClientManager::instance = NULL;

/**
 * \brief ���캯��
 */
RolechangeClientManager::RolechangeClientManager()
{
  rolechangeClientPool = NULL;
}

/**
 * \brief ��������
 */
RolechangeClientManager::~RolechangeClientManager()
{
  SAFE_DELETE(rolechangeClientPool);
}

/**
 * \brief ��ʼ��������
 * \return ��ʼ���Ƿ�ɹ�
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
    Zebra::logger->error("����ͳһ�û�ƽ̨��½�������б��ļ�ʧ��");
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
 * \brief ���ڼ���������ӵĶ�����������
 * \param ct ��ǰʱ��
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
 * \brief ������������Ѿ��ɹ�������
 * \param RolechangeClient ����ӵ�����
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
 * \brief ���������Ƴ��Ͽ�������
 * \param RolechangeClient ���Ƴ�������
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
 * \brief ��ɹ����������ӹ㲥ָ��
 * \param pstrCmd ���㲥��ָ��
 * \param nCmdLen ���㲥ָ��ĳ���
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
