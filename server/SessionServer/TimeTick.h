#ifndef _SessionTimeTick_h_
#define _SessionTimeTick_h_

#include <iostream>
#include <string>
#include "Zebra.h"
#include "zThread.h"
#include "zTime.h"

class SessionTimeTick : public zThread
{

  public:

    static zRTime currentTime;

    ~SessionTimeTick() {};

    static SessionTimeTick &getInstance()
    {
      if (NULL == instance)
        instance = new SessionTimeTick();

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

    void run();

  private:
    Timer _five_sec;	//zhu yi bianliang de shunxu 
    Timer _one_sec;
    Timer _one_min;
    Timer _ten_min;
    Timer _one_hour;
    Timer _five_min; // [ranqd Add] 五分钟定时器
    static SessionTimeTick *instance;
    SessionTimeTick() : zThread("TimeTick"),_five_sec(5),_one_sec(1),_one_min(60),_ten_min(60*10),_one_hour(3480),_five_min(300) 
    {
    };
};
#endif

