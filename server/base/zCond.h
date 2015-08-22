#ifndef _zCond_h_
#define _zCond_h_
#include <pthread.h>
#include "zNoncopyable.h"
#include "zMutex.h"

/**
* \brief 封装了系统条件变量，使用上要简单，省去了手工初始化和销毁系统条件变量的工作，这些工作都可以由构造函数和析构函数来自动完成
*
*/
class zCond : private zNoncopyable
{

public:

	/**
	* \brief 构造函数，用于创建一个条件变量
	*
	*/
	zCond()
	{
		::pthread_cond_init(&cond, NULL);
	}

	/**
	* \brief 析构函数，用于销毁一个条件变量
	*
	*/
	~zCond()
	{
		::pthread_cond_destroy(&cond);
	}

	/**
	* \brief 对所有等待这个条件变量的线程广播发送信号，使这些线程能够继续往下执行
	*
	*/
	void broadcast()
	{
		::pthread_cond_broadcast(&cond);
	}

	/**
	* \brief 对所有等待这个条件变量的线程发送信号，使这些线程能够继续往下执行
	*
	*/
	void signal()
	{
		::pthread_cond_signal(&cond);
	}

	/**
	* \brief 等待特定的条件变量满足
	*
	*
	* \param m_hMutex 需要等待的互斥体
	*/
	void wait(zMutex &mutex)
	{
		::pthread_cond_wait(&cond, &mutex.mutex);
	}

private:

	pthread_cond_t cond;    /**< 系统条件变量 */

};

#endif
