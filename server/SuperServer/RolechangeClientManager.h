#ifndef __RolechangeClientManager_H_
#define __RolechangeClientManager_H_

#include <set>
#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "RolechangeClient.h"
#include "zTime.h"
#include "zRWLock.h"

/**
 * \brief ͳһ�û�ƽ̨��½�������Ŀͻ��������������
 */
class RolechangeClientManager
{

  public:

    ~RolechangeClientManager();

    /**
     * \brief ��ȡ���Ψһʵ��
     * \return ���Ψһʵ������
     */
    static RolechangeClientManager &getInstance()
    {
      if (NULL == instance)
        instance = new RolechangeClientManager();

      return *instance;
    }

    /**
     * \brief �������Ψһʵ��
     */
    static void delInstance()
    {
      SAFE_DELETE(instance);
    }

    bool init();
    void timeAction(const zTime &ct);
    void add(RolechangeClient *roleRegClient);
    void remove(RolechangeClient *roleRegClient);
    bool broadcastOne(const void *pstrCmd,int nCmdLen);
    bool sendTo(const DWORD tempid, const void *pstrCmd, int nCmdLen);
    bool reload();

  private:

    RolechangeClientManager();
    static RolechangeClientManager *instance;

    /**
     * \brief �ͻ������ӹ����
     */
    zTCPClientTaskPool *rolechangeClientPool;
    /**
     * \brief ���ж�����������ʱ���¼
     */
    zTime actionTimer;

    struct lt_client
    {
	bool operator() (RolechangeClient *s1, RolechangeClient *s2) const
	{
	    return s1->getNetType() < s2->getNetType();
	}
    };
    /**
     * \brief ��������Ѿ��ɹ���������������
     */
    typedef std::map<const DWORD, RolechangeClient *> RolechangeClient_map;
    typedef RolechangeClient_map::iterator iter;
    typedef RolechangeClient_map::const_iterator const_iter;
    typedef RolechangeClient_map::value_type value_type;
    /**
     * \brief ��������Ѿ��ɹ�����������
     */
    RolechangeClient_map allClients;

    typedef std::multiset<RolechangeClient *, lt_client> RolechangeClient_set;
    RolechangeClient_set setter;
    /**
     * \brief �������ʶ�д��
     */
    zRWLock rwlock;

};




#endif
