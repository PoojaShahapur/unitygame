/**
 * \brief zebra项目档案服务器，用于创建、储存和读取档案
 *
 */
#ifndef _RecordService_h_
#define _RecordService_h_
#include "zSubNetService.h"
#include "zType.h"
#include "zDBConnPool.h"

/**
 * \brief 定义档案服务类
 *
 * 项目档案服务器，用于创建、储存和读取档案<br>
 * 这个类使用了Singleton设计模式，保证了一个进程中只有一个类的实例
 *
 */
class RecordService : public zSubNetService
{

  public:

    bool msgParse_SuperService(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

    /**
     * \brief 虚析构函数
     *
     */
    ~RecordService()
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
    static RecordService &getInstance()
    {
      if (NULL == instance)
        instance = new RecordService();

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
    
    DWORD verify_version;	//转区验证版本号

    bool sendCmdToZone(DWORD zone, const void *cmd, int len);
  private:

    /**
     * \brief 类的唯一实例指针
     *
     */
    static RecordService *instance;

    zTCPTaskPool *taskPool;        /**< TCP连接池的指针 */

    /**
     * \brief 构造函数
     *
     */
    RecordService() : zSubNetService("档案服务器",RECORDSERVER)
    {
      taskPool = NULL;
    }

    bool init();
    void newTCPTask(const int sock,const struct sockaddr_in *addr);
    void final();
};

#endif

