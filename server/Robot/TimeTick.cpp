#include <iostream>
#include <string>
#include "TimeTick.h"
#include "ClientManager.h"
zRTime ClientTimeTick::currentTime;
ClientTimeTick *ClientTimeTick::instance = NULL;

/**
 * \brief 线程主函数
 *
 */
void ClientTimeTick::run()
{
  while(!isFinal())
  {
    //获取当前时间
    currentTime.now();
    ClientManager::getInstance().timeAction();
    zThread::msleep(50);
  }
}

