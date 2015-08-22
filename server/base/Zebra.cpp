#include <stdlib.h>
#include <iostream>
#include "zMisc.h"
#include "Zebra.h"
#include "zTime.h"


namespace Zebra
{
    __thread unsigned int seedp = 0;
  volatile QWORD qwGameTime = 0;

  zLogger *logger = NULL;

  zProperties global;

  /**
   * \brief 初始化一些全局变量
   *
   */
  static void initGlobal()  __attribute__ ((constructor));	//before main
  void initGlobal()
  {
    global["threadPoolClient"] = "512";
    global["threadPoolServer"] = "2048";
    global["server"]           = "127.0.0.1";
    global["port"]             = "10000";
    global["ifname"]           = "eth0";
    global["mysql"]            = "mysql://zebra:zebra@127.0.0.1:3306/zebra";
    global["log"]              = "debug";
  }
  /**
   * \brief 释放一些全局变量
   *
   */
  static void finalGlobal() __attribute__ ((destructor));	//after main
  void finalGlobal()
  {
    //SAFE_DELETE(logger);
  }
};
