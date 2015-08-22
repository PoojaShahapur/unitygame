
#ifndef _zMutex_h
#define _zMutex_h
#include <pthread.h>
#include <iostream>
#include "zNoncopyable.h"
/**
* \brief 临界区，封装了系统临界区，避免了使用系统临界区时候需要手工初始化和销毁临界区对象的操作
*
*/
class zMutex : private zNoncopyable
{

	friend class zCond;

public:

//	/**
//	* \brief 构造函数，构造一个临界区对象
//	*
//	*/
//	zMutex() 
//	{
//		InitializeCriticalSection(&m_critical);
//	}
//
//	/**
//	* \brief 析构函数，销毁一个临界区对象
//	*
//	*/
//	~zMutex()
//	{
//		DeleteCriticalSection(&m_critical);
//	}
//
//	/**
//	* \brief 加锁一个临界区
//	*
//	*/
//	inline void lock()
//	{
////		Zebra::logger->debug("Locking - %0.8X - %s(%u)", (DWORD)this, file,line );
//		EnterCriticalSection(&m_critical);
////		Zebra::logger->debug("Locked - %0.8X - %s(%u)", (DWORD)this, file,line );
//	}
//
//	/**
//	* \brief 解锁一个临界区
//	*
//	*/
//	inline void unlock()
//	{
////		Zebra::logger->debug("UnLock - %0.8X - %s(%u)", (DWORD)this, file,line );
//		LeaveCriticalSection(&m_critical);
//	}
//
//private:
//
//	CRITICAL_SECTION    m_critical; // 系统临界区
	/**
	* \brief 构造函数，构造一个互斥体对象
	*
	*/
	zMutex(int kind = PTHREAD_MUTEX_FAST_NP) 
	{
		pthread_mutexattr_t attr;
		::pthread_mutexattr_init(&attr);
		::pthread_mutexattr_settype(&attr, kind);
		::pthread_mutex_init(&mutex, &attr);
	}

	/**
	* \brief 析构函数，销毁一个互斥体对象
	*
	*/
	~zMutex()
	{
		::pthread_mutex_destroy(&mutex);
	}

	/**
	* \brief 加锁一个互斥体
	*
	*/
	inline void lock()
	{
#if 0
		if( WaitForSingleObject(m_hMutex,10000) == WAIT_TIMEOUT )
		{
			char szName[MAX_PATH];
			GetModuleFileName(NULL,szName,sizeof(szName));
			::MessageBox(NULL,"发生死锁！", szName, MB_ICONERROR);
		}
#endif
				::pthread_mutex_lock(&mutex);

	}

	/**
	* \brief 解锁一个互斥体
	*
	*/
	inline void unlock()
	{
		::pthread_mutex_unlock(&mutex);
	}

private:

	pthread_mutex_t mutex;    /**< 系统互斥体 */

};

/**
* \brief Wrapper
* 方便在复杂函数中锁的使用
*/
class zMutex_scope_lock : private zNoncopyable
{

public:

	/**
	* \brief 构造函数
	* 对锁进行lock操作
	* \param m 锁的引用
	*/
	zMutex_scope_lock(zMutex &m) : mlock(m)
	{
		mlock.lock();
	}

	/**
	* \brief 析购函数
	* 对锁进行unlock操作
	*/
	~zMutex_scope_lock()
	{
		mlock.unlock();
	}

private:

	/**
	* \brief 锁的引用
	*/
	zMutex &mlock;

};

#endif

