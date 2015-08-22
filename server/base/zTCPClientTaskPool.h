#ifndef _zTCPClientTaskPool_h_
#define _zTCPClientTaskPool_h_

#include <string>
#include <vector>
#include <queue>
#include <list>
#include <unistd.h>
#include <sys/timeb.h>

#include "zSocket.h"
#include "zThread.h"
#include "zTCPClientTask.h"
#include "Zebra.h"
#include "zString.h"

class zCheckconnectThread;
class zCheckwaitThread;
class zTCPClientTaskThread;

/**
* \brief 连接线程池类，封装了一个线程处理多个连接的线程池框架
*
*/
class zTCPClientTaskPool : private zNoncopyable
{

public:

	explicit zTCPClientTaskPool(const DWORD connPerThread=512,const unsigned long us=50000L) : connPerThread(connPerThread)
	{       
		usleep_time=us;
		checkwaitThread = NULL; 
	} 
	~zTCPClientTaskPool();

	bool init();
	bool put(zTCPClientTask *task);
	void timeAction(const zTime &ct);

	void addCheckwait(zTCPClientTask *task);
	bool addMain(zTCPClientTask *task);
	void setUsleepTime(int time)
	{
		usleep_time = time;
	}

private:

	const DWORD connPerThread;
	zTCPClientTaskThread *newThread();

	/**
	* \brief 连接检测线程
	*
	*/
	zCheckconnectThread *checkconnectThread;;
	/**
	* \brief 连接等待返回信息的线程
	*
	*/
	zCheckwaitThread *checkwaitThread;;
	/**
	* \brief 所有成功连接处理的主线程
	*
	*/
	zThreadGroup taskThreads;

	/**
	* \brief 连接任务链表
	*
	*/
	//typedef std::list<zTCPClientTask *,__pool_alloc<zTCPClientTask *> > zTCPClientTaskContainer;
	typedef std::list<zTCPClientTask *> zTCPClientTaskContainer;


	/**
	* \brief 连接任务链表叠代器
	*
	*/
	typedef zTCPClientTaskContainer::iterator zTCPClientTask_IT;

	zMutex mlock;          /**< 互斥变量 */
	zTCPClientTaskContainer tasks;  /**< 任务列表 */

public:
	unsigned long usleep_time;                                        /**< 循环等待时间 */
};

#endif

