/**
 * \brief 实现管理服务器
 *
 * 对一个区中的所有服务器进行管理
 * 
 */
#ifndef _DumpServer_h_
#define _DumpServer_h_

#include <iostream>
#include <string>
#include <ext/numeric>
#include "zService.h"
#include "zThread.h"
#include "zSocket.h"
#include "zTCPServer.h"
#include "zNetService.h"
#include "zDBConnPool.h"
#include "zMisc.h"
#include "zType.h"
#include "zHttpTaskPool.h"


/**
 * \brief 管理服务器类
 *
 * 派生了基类<code>zNetService</code>
 *
 */
class DumpServer : public zNetService
{

  public:


    /**
     * \brief 获取类的唯一实例
     *
     * 使用了Singleton设计模式，保证了一个进程中只有一个类的实例
     *
     * \return 类的唯一实例
     */
    static DumpServer &getInstance()
    {
      if (NULL == instance)
        instance = new DumpServer();

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

    /**
     * \brief 指向数据库连接池实例的指针
     *
     */
    static zDBConnPool *dbConnPool;

  private:

    /**
     * \brief 类的唯一实例指针
     *
     */
    static DumpServer *instance;


    /**
     * \brief 构造函数
     *
     */
    DumpServer();

    /**
     * \brief 析构函数
     *
     * 虚函数
     *
     */
    ~DumpServer()
    {
      instance = NULL;

      //关闭线程池
      if (httpTaskPool)
      {
        httpTaskPool->final();
        SAFE_DELETE(httpTaskPool);
      }
    }

    bool init();
    void newTCPTask(const int sock,const struct sockaddr_in *addr);
    void final();

    zHttpTaskPool *httpTaskPool;        /**< 连接池的指针 */

};

#endif
