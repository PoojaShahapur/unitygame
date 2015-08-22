#ifndef _SceneTimeTick_h_
#define _SceneTimeTick_h_
#include "zThread.h"
#include "zTime.h"

/**
 * \brief 时间回调函数
 */
class SceneTimeTick : public zThread
{

  public:

    /// 当前时间
    static zRTime currentTime;

    /**
     * \brief 析构函数
     */
    ~SceneTimeTick() {};

    /**
     * \brief 获取唯一实例
     */
    static SceneTimeTick &getInstance()
    {
      if (NULL == instance)
        instance = new SceneTimeTick();

      return *instance;
    }

    /**
     * \brief 释放类的唯一实例
     */
    static void delInstance()
    {
      SAFE_DELETE(instance);
    }

    void run();

  private:
	  /// sky 1秒钟记数器
	  Timer _one_sec;

    /// 五秒钟计数器
    Timer _five_sec;

    // 一分钟计数器
    Timer _one_min;

    // 竞赛进行标志
    bool quiz;

    /// 唯一实例
    static SceneTimeTick *instance;

    /**
     * \brief 构造函数
     */
    SceneTimeTick() : zThread("TimeTick"),_one_sec(1),_five_sec(5),_one_min(60),quiz(false) {};

};
#endif

