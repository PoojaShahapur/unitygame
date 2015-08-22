#ifndef __BROADCLIENTMANAGER_H_
#define __BROADCLIENTMANAGER_H_

#include <ext/hash_map>
#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "BroadClient.h"
#include "zTime.h"
#include "zRWLock.h"

/**
 * \brief ͳһ�û�ƽ̨��½�������Ŀͻ��������������
 */
class BroadClientManager
{

  public:

    ~BroadClientManager();

    /**
     * \brief ��ȡ���Ψһʵ��
     * \return ���Ψһʵ������
     */
    static BroadClientManager &getInstance()
    {
      if (NULL == instance)
        instance = new BroadClientManager();

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
    void add(BroadClient *flClient);
    void remove(BroadClient *flClient);
    bool broadcastOne(const void *pstrCmd,int nCmdLen);

  private:

    BroadClientManager();
    static BroadClientManager *instance;

    /**
     * \brief �ͻ������ӹ����
     */
    zTCPClientTaskPool *flClientPool;
    /**
     * \brief ���ж�����������ʱ���¼
     */
    zTime actionTimer;
    
    struct lt_client
    {
	bool operator()(BroadClient *s1, BroadClient *s2) const
	{
	    return s1->getNetType() < s2->getNetType();
	}
    };
    /**
     * \brief ��������Ѿ��ɹ���������������
     */
    typedef std::multiset<BroadClient *, lt_client> BroadClientContainer;
    typedef BroadClientContainer::iterator iter;
    typedef BroadClientContainer::const_iterator const_iter;
    typedef BroadClientContainer::value_type value_type;
    /**
     * \brief ��������Ѿ��ɹ�����������
     */
    BroadClientContainer allClients;
    /**
     * \brief �������ʶ�д��
     */
    zRWLock rwlock;

};




#endif
