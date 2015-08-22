#ifndef __BROADCLIENTMANAGER_H_
#define __BROADCLIENTMANAGER_H_

#include <ext/hash_map>
#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "BroadClient.h"
#include "zTime.h"
#include "zRWLock.h"

/**
 * \brief 统一用户平台登陆服务器的客户端连接类管理器
 */
class BroadClientManager
{

  public:

    ~BroadClientManager();

    /**
     * \brief 获取类的唯一实例
     * \return 类的唯一实例引用
     */
    static BroadClientManager &getInstance()
    {
      if (NULL == instance)
        instance = new BroadClientManager();

      return *instance;
    }

    /**
     * \brief 销毁类的唯一实例
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
     * \brief 客户端连接管理池
     */
    zTCPClientTaskPool *flClientPool;
    /**
     * \brief 进行断线重连检测的时间记录
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
     * \brief 存放连接已经成功的连接容器类型
     */
    typedef std::multiset<BroadClient *, lt_client> BroadClientContainer;
    typedef BroadClientContainer::iterator iter;
    typedef BroadClientContainer::const_iterator const_iter;
    typedef BroadClientContainer::value_type value_type;
    /**
     * \brief 存放连接已经成功的连接容器
     */
    BroadClientContainer allClients;
    /**
     * \brief 容器访问读写锁
     */
    zRWLock rwlock;

};




#endif
