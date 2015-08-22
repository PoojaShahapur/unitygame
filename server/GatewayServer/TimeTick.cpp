#include <iostream>
#include <string>
#include "Zebra.h"
#include "zThread.h"
#include "zTime.h"
#include "TimeTick.h"
#include "LoginSessionManager.h"
#include "GatewayTaskManager.h"

/*��?
 *
 * 
 */

#include "GatewayServer.h"

zRTime GatewayTimeTick::currentTime;
GatewayTimeTick *GatewayTimeTick::instance = NULL;

struct GatewayTaskCheckTime : public GatewayTaskManager::GatewayTaskCallback
{
  bool exec(GatewayTask *gt)
  {
    return gt->checkTime(GatewayTimeTick::currentTime);
  }
};

/**
 * \brief �߳�������
 *
 */
void GatewayTimeTick::run()
{
  int nSeconds;

  nSeconds = 0;
  while(!isFinal())
  {
    zThread::sleep(1);

    //��ȡ��ǰʱ��
    currentTime.now();

    if (one_second(currentTime) ) {
      LoginSessionManager::getInstance().update(currentTime);

      GatewayTaskCheckTime gtct;
      GatewayTaskManager::getInstance().execAll(gtct);

      if (nSeconds++ > 60)
      {
        //60 seconds
        nSeconds = 0;
        Zebra::logger->debug("���ص�ǰ��������:%d\n",GatewayService::getInstance().getPoolSize());
      }
    }
  }
}

