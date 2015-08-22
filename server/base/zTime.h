
/**
* \brief 时间定义
*
* 
*/
#ifndef _ZTIME_H_
#define _ZTIME_H_

#include <time.h>
#include <sys/time.h>

#include "zType.h"
#include "zMutex.h"
#include "zMisc.h"
/**
* \brief 真实时间类,对timeval结构简单封装,提供一些常用时间函数
* 时间精度精确到毫秒，
* 关于timeval请man gettimeofday
*/
class zRTime
{

private:

	/**
	* \brief 真实时间换算为毫秒
	*
	*/
	QWORD _msecs;

	/**
	* \brief 得到当前真实时间
	*
	* \return 真实时间，单位毫秒
	*/
	QWORD _now()
	{
		QWORD retval = 0LL;
		struct timespec tv;
		clock_gettime(CLOCK_REALTIME, &tv);
		retval = tv.tv_sec;
		retval *= 1000LL;
		retval += (tv.tv_nsec / 1000000L);
		return retval;
	}

	/**
	* \brief 得到当前真实时间延迟后的时间
	* \param delay 延迟，可以为负数，单位毫秒
	*/
	void nowByDelay(int delay)
	{
		_msecs = _now();
		addDelay(delay);
	}

public:

	/**
	* \brief 构造函数
	*
	* \param delay 相对于现在时间的延时，单位毫秒
	*/
	zRTime(const int delay = 0)
	{
		nowByDelay(delay);
	}

	/**
	* \brief 拷贝构造函数
	*
	* \param rt 拷贝的引用
	*/
	zRTime(const zRTime &rt)
	{
		_msecs = rt._msecs;
	}

	/**
	* \brief 获取当前时间
	*
	*/
	void now()
	{
		_msecs = _now();
	}

	/**
	* \brief 返回秒数
	*
	* \return 秒数
	*/
	DWORD sec() const
	{
		return _msecs / 1000L;
	}

	/**
	* \brief 返回毫秒数
	*
	* \return 毫秒数
	*/
	DWORD msec() const
	{
		return _msecs % 1000L;
	}

	/**
	* \brief 返回总共的毫秒数
	*
	* \return 总共的毫秒数
	*/
	QWORD msecs() const
	{
		return _msecs;
	}

	/**
	* \brief 返回总共的毫秒数
	*
	* \return 总共的毫秒数
	*/
	void setmsecs(QWORD data)
	{
		_msecs = data;
	}

	/**
	* \brief 加延迟偏移量
	*
	* \param delay 延迟，可以为负数，单位毫秒
	*/
	void addDelay(int delay)
	{
		_msecs += delay;
	}

	/**
	* \brief 重载=运算符号
	*
	* \param rt 拷贝的引用
	* \return 自身引用
	*/
	zRTime & operator= (const zRTime &rt)
	{
		_msecs = rt._msecs;
		return *this;
	}

	/**
	* \brief 重构+操作符
	*
	*/
	const zRTime & operator+ (const zRTime &rt)
	{
		_msecs += rt._msecs;
		return *this;
	}

	/**
	* \brief 重构-操作符
	*
	*/
	const zRTime & operator- (const zRTime &rt)
	{
		_msecs -= rt._msecs;
		return *this;
	}

	/**
	* \brief 重构>操作符，比较zRTime结构大小
	*
	*/
	bool operator > (const zRTime &rt) const
	{
		return _msecs > rt._msecs;
	}

	/**
	* \brief 重构>=操作符，比较zRTime结构大小
	*
	*/
	bool operator >= (const zRTime &rt) const
	{
		return _msecs >= rt._msecs;
	}

	/**
	* \brief 重构<操作符，比较zRTime结构大小
	*
	*/
	bool operator < (const zRTime &rt) const
	{
		return _msecs < rt._msecs;
	}

	/**
	* \brief 重构<=操作符，比较zRTime结构大小
	*
	*/
	bool operator <= (const zRTime &rt) const
	{
		return _msecs <= rt._msecs;
	}

	/**
	* \brief 重构==操作符，比较zRTime结构是否相等
	*
	*/
	bool operator == (const zRTime &rt) const
	{
		return _msecs == rt._msecs;
	}

	/**
	* \brief 计时器消逝的时间，单位毫秒
	* \param rt 当前时间
	* \return 计时器消逝的时间，单位毫秒
	*/
	QWORD elapse(const zRTime &rt) const
	{
		if (rt._msecs > _msecs)
			return (rt._msecs - _msecs);
		else
			return 0LL;
	}

	static long my_timezone;
	static std::string & getLocalTZ();
	static void save_timezone(std::string & tzstr);
	static void getLocalTime(struct tm & tv1,time_t timValue)
	{
		timValue -= my_timezone;
		gmtime_r(&timValue, &tv1);
	}

};

/**
* \brief 时间类,对struct tm结构简单封装
*/

class zTime
{

public:

	/**
	* \brief 构造函数
	*/
	zTime()
	{
		time(&secs);
		zRTime::getLocalTime(tv,secs);
	}

	/**
	* \brief 拷贝构造函数
	*/
	zTime(const zTime &ct)
	{
		secs = ct.secs;
		zRTime::getLocalTime(tv,secs);
	}

	/**
	* \brief 获取当前时间
	*/
	void now()
	{
		time(&secs);
		zRTime::getLocalTime(tv,secs);
	}

	/**
	* \brief 返回存储的时间
	* \return 时间，秒
	*/
	time_t sec() const
	{
		return secs;
	}

	/**
	* \brief 重载=运算符号
	* \param rt 拷贝的引用
	* \return 自身引用
	*/
	zTime & operator= (const zTime &rt)
	{
		secs = rt.secs;
		return *this;
	}

	/**
	* \brief 重构+操作符
	*/
	const zTime & operator+ (const zTime &rt)
	{
		secs += rt.secs;
		return *this;
	}

	/**
	* \brief 重构-操作符
	*/
	const zTime & operator- (const zTime &rt)
	{
		secs -= rt.secs;
		return *this;
	}

	/**
	* \brief 重构-操作符
	*/
	const zTime & operator-= (const time_t s)
	{
		secs -= s;
		return *this;
	}

	/**
	* \brief 重构>操作符，比较zTime结构大小
	*/
	bool operator > (const zTime &rt) const
	{
		return secs > rt.secs;
	}

	/**
	* \brief 重构>=操作符，比较zTime结构大小
	*/
	bool operator >= (const zTime &rt) const
	{
		return secs >= rt.secs;
	}

	/**
	* \brief 重构<操作符，比较zTime结构大小
	*/
	bool operator < (const zTime &rt) const
	{
		return secs < rt.secs;
	}

	/**
	* \brief 重构<=操作符，比较zTime结构大小
	*/
	bool operator <= (const zTime &rt) const
	{
		return secs <= rt.secs;
	}

	/**
	* \brief 重构==操作符，比较zTime结构是否相等
	*/
	bool operator == (const zTime &rt) const
	{
		return secs == rt.secs;
	}

	/**
	* \brief 计时器消逝的时间，单位秒
	* \param rt 当前时间
	* \return 计时器消逝的时间，单位秒
	*/
	time_t elapse(const zTime &rt) const
	{
		if (rt.secs > secs)
			return (rt.secs - secs);
		else
			return 0;
	}

	/**
	* \brief 计时器消逝的时间，单位秒
	* \return 计时器消逝的时间，单位秒
	*/
	time_t elapse() const
	{
		zTime rt;
		return (rt.secs - secs);
	}

	/**
	* \brief 得到当前分钟，范围0-59点
	*
	* \return 
	*/
	int getSec()
	{
		return tv.tm_sec;
	}

	/**
	* \brief 得到当前分钟，范围0-59点
	*
	* \return 
	*/
	int getMin()
	{
		return tv.tm_min;
	}

	/**
	* \brief 得到当前小时，范围0-23点
	*
	* \return 
	*/
	int getHour()
	{
		return tv.tm_hour;
	}

	/**
	* \brief 得到天数，范围1-31
	*
	* \return 
	*/
	int getMDay()
	{
		return tv.tm_mday;
	}

	/**
	* \brief 得到当前星期几，范围1-7
	*
	* \return 
	*/
	int getWDay()
	{
		return tv.tm_wday;
	}

	/**
	* \brief 得到当前月份，范围1-12
	*
	* \return 
	*/
	int getMonth()
	{
		return tv.tm_mon+1;
	}

	/**
	* \brief 得到当前年份
	*
	* \return 
	*/
	int getYear()
	{
		return tv.tm_year+1900;
	}  

private:

	/**
	* \brief 存储时间，单位秒
	*/
	time_t secs;

	/**
	* \brief tm结构，方便访问
	*/
	struct tm tv;


};

class Timer
{
public:
	Timer(const float how_long,const int delay=0) : _long((int)(how_long*1000)),_timer(delay*1000)
	{

	}
	Timer(const float how_long,const zRTime cur) : _long((int)(how_long*1000)),_timer(cur)
	{
		_timer.addDelay(_long);
	}
	void next(const zRTime &cur)
	{
		_timer=cur;
		_timer.addDelay(_long);
	} 
	bool operator() (const zRTime& current)
	{
		if (_timer <= current) {
			_timer = current;
			_timer.addDelay(_long);
			return true;
		}

		return false;
	}
private:
	int _long;
	zRTime _timer;
};


//时间间隔具有随机性
class RandTimer
{
public:
#define next_time(_long) (_long / 2 + zMisc::randBetween(0,_long))
	RandTimer(const float how_long,const int delay=0) : _long((int)(how_long*1000)),_timer(delay*1000)
	{

	}
	RandTimer(const float how_long,const zRTime cur) : _long((int)(how_long*1000)),_timer(cur)
	{
		_timer.addDelay(next_time(_long));
	}
	void next(const zRTime &cur)
	{
		_timer=cur;
		_timer.addDelay(next_time(_long));
	} 
	bool operator() (const zRTime& current)
	{
		if (_timer <= current) {
			_timer = current;
			_timer.addDelay(next_time(_long));
			return true;
		}

		return false;
	}
private:
	int _long;
	zRTime _timer;
};

/**
 * \brief   һ��ָ��Ĵ���ʱ��
*/
struct CommandTime
{
    private:
	struct timespec _tv_1;
	struct timespec _tv_2;
	const QWORD _need_log;
	const char* _where;
	const BYTE _cmd;
	const BYTE _para;
    public:
	CommandTime(const QWORD interval, const char* where, BYTE cmd, BYTE para)
	    :_need_log(interval), _where(where), _cmd(cmd), _para(para)
	{
	    clock_gettime(CLOCK_REALTIME, &_tv_1);
	}
	~CommandTime()
	{
	    clock_gettime(CLOCK_REALTIME, &_tv_2);
	    QWORD end = _tv_2.tv_sec*1000000L + _tv_2.tv_nsec/1000L;
	    QWORD begin = _tv_1.tv_sec*1000000L + _tv_1.tv_nsec/1000L;
	    if(end - begin > _need_log)
	    {
		Zebra::logger->debug("[CommandTime]��%s����ָ��[%u,%u]ִ��ʱ��:%llu ΢��", _where, _cmd, _para, end-begin);
	    }

	}
};

/**
 * \brief   һ�������Ĵ���ʱ��
*/
struct FunctionTime
{
    private:
	struct timespec _tv_1;
	struct timespec _tv_2;
	const QWORD _need_log;
	const char* _fun_name;
	const char* _dis;
	const int _dis_len;
    public:
	FunctionTime(const QWORD interval, const char* func=NULL, const char* dis=NULL, const int dis_len=16)
	    :_need_log(interval), _fun_name(func), _dis(dis), _dis_len(dis_len)
	{
	    clock_gettime(CLOCK_REALTIME, &_tv_1);
	}
	~FunctionTime()
	{
	    clock_gettime(CLOCK_REALTIME, &_tv_2);
	    QWORD end = _tv_2.tv_sec*1000000L + _tv_2.tv_nsec/1000L;
	    QWORD begin = _tv_1.tv_sec*1000000L + _tv_1.tv_nsec/1000L;
	    if(end - begin > _need_log)
	    {
		char buf[_dis_len+1];
		bzero(buf, sizeof(buf));
		strncpy(buf, _dis, _dis_len);
		Zebra::logger->debug("[FunctionTime]%s ִ��ʱ��:%llu ΢��,����:%s", _fun_name, end-begin, buf);
	    }

	}
};

/**
 * \brief   һ�����εĴ���ʱ��
*/
struct BlockTime
{
    private:
	struct timespec _tv_1;
	struct timespec _tv_2;
	const QWORD _need_log;
	const char* _where;
    public:
	BlockTime(const QWORD interval, const char* where)
	    :_need_log(interval), _where(where)
	{
	    clock_gettime(CLOCK_REALTIME, &_tv_1);
	}
	~BlockTime()
	{
	}
	void elapse()
	{
	    clock_gettime(CLOCK_REALTIME, &_tv_2);
	    QWORD end = _tv_2.tv_sec*1000000L + _tv_2.tv_nsec/1000L;
	    QWORD begin = _tv_1.tv_sec*1000000L + _tv_1.tv_nsec/1000L;
	    if(end - begin > _need_log)
	    {
		Zebra::logger->debug("[BlockTime] %s ����δ���,ִ��ʱ��:%llu ΢��", _where, end-begin);
	    }
	}
};
#endif
