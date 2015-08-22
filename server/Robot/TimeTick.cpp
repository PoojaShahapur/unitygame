#include <iostream>
#include <string>
#include "TimeTick.h"
#include "ClientManager.h"
zRTime ClientTimeTick::currentTime;
ClientTimeTick *ClientTimeTick::instance = NULL;

/**
 * \brief �߳�������
 *
 */
void ClientTimeTick::run()
{
  while(!isFinal())
  {
    //��ȡ��ǰʱ��
    currentTime.now();
    ClientManager::getInstance().timeAction();
    zThread::msleep(50);
  }
}

