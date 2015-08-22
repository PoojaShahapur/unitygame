#ifndef _zHttpTaskPool_h_
#define _zHttpTaskPool_h_
#include <string>
#include <vector>
#include <queue>
#include <unistd.h>
#include <sys/timeb.h>

#include "zSocket.h"
#include "zThread.h"
#include "zHttpTask.h"
#include "Zebra.h"
#include "zString.h"

class zHttpTask;

/**
* \brief 连接线程池类，封装了一个线程处理多个连接的线程池框架
*
*/
class zHttpTaskPool : private zNoncopyable
{

public:

	/**
	* \brief 构造函数
	*/
	zHttpTaskPool()
	{
	};

	/**
	* \brief 析构函数，销毁一个线程池对象
	*
	*/
	~zHttpTaskPool()
	{
		final();
	}

	bool addHttp(zHttpTask *task);
	bool init();
	void final();

private:

	static const int maxHttpThreads = 8;          /**< 最大验证线程数量 */
	zThreadGroup httpThreads;                /**< 验证线程，可以有多个 */

};
#endif

