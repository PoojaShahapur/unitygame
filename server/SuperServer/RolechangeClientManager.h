#ifndef __RolechangeClientManager_H_
#define __RolechangeClientManager_H_

#include <set>
#include "zTCPClientTask.h"
#include "zTCPClientTaskPool.h"
#include "RolechangeClient.h"
#include "zTime.h"
#include "zRWLock.h"

/**
 * \brief 统一用户平台登陆服务器的客户端连接类管理器
 */
class RolechangeClientManager
{

  public:

    ~RolechangeClientManager();

    /**
     * \brief 获取类的唯一实例
     * \return 类的唯一实例引用
     */
    static RolechangeClientManager &getInstance()
    {
      if (NULL == instance)
        instance = new RolechangeClientManager();

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
    void add(RolechangeClient *roleRegClient);
    void remove(RolechangeClient *roleRegClient);
    bool broadcastOne(const void *pstrCmd,int nCmdLen);
    bool sendTo(const DWORD tempid, const void *pstrCmd, int nCmdLen);
    bool reload();

  private:

    RolechangeClientManager();
    static RolechangeClientManager *instance;

    /**
     * \brief 客户端连接管理池
     */
    zTCPClientTaskPool *rolechangeClientPool;
    /**
     * \brief 进行断线重连检测的时间记录
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
     * \brief 存放连接已经成功的连接容器类型
     */
    typedef std::map<const DWORD, RolechangeClient *> RolechangeClient_map;
    typedef RolechangeClient_map::iterator iter;
    typedef RolechangeClient_map::const_iterator const_iter;
    typedef RolechangeClient_map::value_type value_type;
    /**
     * \brief 存放连接已经成功的连接容器
     */
    RolechangeClient_map allClients;

    typedef std::multiset<RolechangeClient *, lt_client> RolechangeClient_set;
    RolechangeClient_set setter;
    /**
     * \brief 容器访问读写锁
     */
    zRWLock rwlock;

};




#endif
