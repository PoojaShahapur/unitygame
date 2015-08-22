#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "BroadClientManager.h"
#include "BroadClient.h"
#include "zXMLParser.h"
/*��ͳһ�û�ƽ̨�ͻ������ӵĹ�������
 */




/**
 * \brief ���Ψһʵ��ָ��
 */
BroadClientManager *BroadClientManager::instance = NULL;

/**
 * \brief ���캯��
 */
BroadClientManager::BroadClientManager()
{
  flClientPool = NULL;
}

/**
 * \brief ��������
 */
BroadClientManager::~BroadClientManager()
{
  SAFE_DELETE(flClientPool);
}

/**
 * \brief ��ʼ��������
 * \return ��ʼ���Ƿ�ɹ�
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
    Zebra::logger->error("����ͳһ�û�ƽ̨��½�������б��ļ�ʧ��");
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
 * \brief ���ڼ���������ӵĶ�����������
 * \param ct ��ǰʱ��
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
 * \brief ������������Ѿ��ɹ�������
 * \param flClient ����ӵ�����
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
 * \brief ���������Ƴ��Ͽ�������
 * \param flClient ���Ƴ�������
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
 * \brief ��ɹ����������ӹ㲥ָ��
 * \param pstrCmd ���㲥��ָ��
 * \param nCmdLen ���㲥ָ��ĳ���
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

