#include "zTime.h"
#include "Zebra.h"

long zRTime::my_timezone = 0L;

/**
 * \brief 得到系统时区设置字符串
 *
 * \param s 时区将放入此字符串中
 * \return 返回参数s
 */
std::string & zRTime::getLocalTZ()
{
  static std::string s;
  static bool init = false;
  if(!init)
  {
	std::ostringstream so;
	tzset();
	if(0L == my_timezone)
	    my_timezone = timezone;
	so<< tzname[0] << timezone/3600;
	s = so.str();
	init = true;
  }
  return s;
}

static zMutex tz_lock;

void zRTime::save_timezone(std::string &tzstr)
{
    tz_lock.lock();
    std::string ss = zRTime::getLocalTZ();
    std::ostringstream so;
    so << "TZ=" << ss;
    tzstr = so.str();
}
