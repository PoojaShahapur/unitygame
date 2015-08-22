/**
 * \brief zebra项目档案服务器，用于创建、储存和读取档案
 *
 */
#ifndef _ClientService_h_
#define _ClientService_h_
#include "zService.h"
#include "zType.h"
class LoginClient;

/**
 * \brief 定义档案服务类
 *
 * 项目档案服务器，用于创建、储存和读取档案<br>
 * 这个类使用了Singleton设计模式，保证了一个进程中只有一个类的实例
 *
 */
class ClientService : public zService
{

  public:

    /**
     * \brief 虚析构函数
     *
     */
    ~ClientService()
    {
    }

    /**
     * \brief 返回唯一的类实例
     *
     * \return 唯一的类实例
     */
    static ClientService &getInstance()
    {
      if (NULL == instance)
        instance = new ClientService();

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

    bool init();
    bool serviceCallback();
    void final();

    LoginClient *loginClient;
  private:

    /**
     * \brief 类的唯一实例指针
     *
     */
    static ClientService *instance;

    /**
     * \brief 构造函数
     *
     */
    ClientService() : zService("测试客户端")
    {
    }

};

#endif

