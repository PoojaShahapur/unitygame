#ifndef __TIMETICK_H_
#define __TIMETICK_H_

#include <iostream>
#include <string>

#include "zTime.h"
#include "zThread.h"


class BillTimeTick : public zThread
{

  public:

    ~BillTimeTick() {};

    /// 当前时间
    static zRTime currentTime;
    static Timer _one_min;
    static Timer _one_sec;
    static BillTimeTick &getInstance()
    {
      if (NULL == instance)
        instance = new BillTimeTick();

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

    static BillTimeTick *instance;

    BillTimeTick() : zThread("TimeTick") {};

};


#endif
