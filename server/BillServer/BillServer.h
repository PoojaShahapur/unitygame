#ifndef __BILLSERVER_H_
#define __BILLSERVER_H_

/**
 * \brief zebra项目计费服务器
 *
 */
#include <set>
#include "zSubNetService.h"
#include "zMisc.h"
#include "zDBConnPool.h"
#include "zMetaData.h"









/**
 * \brief 定义计费服务类
 *
 * 这个类使用了Singleton设计模式,保证了一个进程中只有一个类的实例
 *
 */
class BillService : public zSubNetService
{

  public:

    bool msgParse_SuperService(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

    /**
     * \brief 虚析构函数
     *
     */
    ~BillService()
    {
      instance = NULL;

      //关闭线程池
      if (taskPool)
      {
        taskPool->final();
        SAFE_DELETE(taskPool);
      }
    }

    const int getPoolSize() const
    {
      if (taskPool)
      {
        return taskPool->getSize();
      }
      else
      {
        return 0;
      }
    }

    /**
     * \brief 返回唯一的类实例
     *
     * \return 唯一的类实例
     */
    static BillService &getInstance()
    {
      if (NULL == instance)
        instance = new BillService();

      return *instance;
    }

    /**
     * \brief 释放类的唯一实例
     *
     */
    static void delInstance()
    {
      SAFE_DELETE(instance);
    }

    void reloadConfig();

    /**
     * \brief 指向数据库连接池实例的指针
     *
     */
    static zDBConnPool *dbConnPool;

    /**
     * \brief 指向数据库表管理器的指针
     *
     */
                static DBMetaData* metaData;    

    /**
     * \brief 指向交易日志的指针
     *
     */
                static zLogger* tradelog;    



  private:

    /**
     * \brief 类的唯一实例指针
     *
     */
    static BillService *instance;

    zTCPTaskPool *taskPool;        /**< TCP连接池的指针 */

    /**
     * \brief 构造函数
     *
     */
    BillService() : zSubNetService("计费服务器",BILLSERVER)
    {
      taskPool = NULL;
    }

    bool init();
    void newTCPTask(const int sock,const struct sockaddr_in *addr);
    void final();
};


#endif

