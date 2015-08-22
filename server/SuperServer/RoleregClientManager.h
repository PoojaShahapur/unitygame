#ifndef __RoleregClientManager_H_
#define __RoleregClientManager_H_

#include <set>
#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "RoleregClient.h"
#include "zTime.h"
#include "zRWLock.h"

/**
 * \brief 统一用户平台登陆服务器的客户端连接类管理器
 */
class RoleregClientManager
{

  public:

    ~RoleregClientManager();

    /**
     * \brief 获取类的唯一实例
     * \return 类的唯一实例引用
     */
    static RoleregClientManager &getInstance()
    {
      if (NULL == instance)
        instance = new RoleregClientManager();

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
    void add(RoleregClient *roleRegClient);
    void remove(RoleregClient *roleRegClient);
    bool broadcastOne(const void *pstrCmd,int nCmdLen);
    bool reload();

  private:

    RoleregClientManager();
    static RoleregClientManager *instance;

    /**
     * \brief 客户端连接管理池
     */
    zTCPClientTaskPool *roleregClientPool;
    /**
     * \brief 进行断线重连检测的时间记录
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
     * \brief 存放连接已经成功的连接容器类型
     */
    typedef std::multiset<RoleregClient *, lt_client> RoleRegClientContainer;
    typedef RoleRegClientContainer::iterator iter;
    typedef RoleRegClientContainer::const_iterator const_iter;
    /**
     * \brief 存放连接已经成功的连接容器
     */
    RoleRegClientContainer allClients;
    /**
     * \brief 容器访问读写锁
     */
    zRWLock rwlock;

};




#endif
