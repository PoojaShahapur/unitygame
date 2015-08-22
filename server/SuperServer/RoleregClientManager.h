#ifndef __RoleregClientManager_H_
#define __RoleregClientManager_H_

#include <set>
#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "RoleregClient.h"
#include "zTime.h"
#include "zRWLock.h"

/**
 * \brief ͳһ�û�ƽ̨��½�������Ŀͻ��������������
 */
class RoleregClientManager
{

  public:

    ~RoleregClientManager();

    /**
     * \brief ��ȡ���Ψһʵ��
     * \return ���Ψһʵ������
     */
    static RoleregClientManager &getInstance()
    {
      if (NULL == instance)
        instance = new RoleregClientManager();

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
    void add(RoleregClient *roleRegClient);
    void remove(RoleregClient *roleRegClient);
    bool broadcastOne(const void *pstrCmd,int nCmdLen);
    bool reload();

  private:

    RoleregClientManager();
    static RoleregClientManager *instance;

    /**
     * \brief �ͻ������ӹ����
     */
    zTCPClientTaskPool *roleregClientPool;
    /**
     * \brief ���ж�����������ʱ���¼
     */
    zTime actionTimer;

    struct lt_client
    {
	bool operator() (RoleregClient *s1, RoleregClient *s2) const
	{
	    return s1->getNetType() < s2->getNetType();
	}
    };
    /**
     * \brief ��������Ѿ��ɹ���������������
     */
    typedef std::multiset<RoleregClient *, lt_client> RoleRegClientContainer;
    typedef RoleRegClientContainer::iterator iter;
    typedef RoleRegClientContainer::const_iterator const_iter;
    /**
     * \brief ��������Ѿ��ɹ�����������
     */
    RoleRegClientContainer allClients;
    /**
     * \brief �������ʶ�д��
     */
    zRWLock rwlock;

};




#endif
