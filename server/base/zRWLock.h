
#ifndef _zRWLock_h
#define _zRWLock_h
#include <pthread.h>
#include "zNoncopyable.h"
#include "Zebra.h"
/**
* \brief 封装了系统读写锁，使用上要简单，省去了手工初始化和销毁系统读写锁的工作，这些工作都可以由构造函数和析构函数来自动完成
*
*/
class zRWLock : private zNoncopyable
{

public:
	unsigned int rd_count;
	unsigned int wr_count;
	/**
	* \brief 构造函数，用于创建一个读写锁
	*
	*/
	zRWLock():rd_count(0),wr_count(0)
	{
		int nRet = ::pthread_rwlock_init(&rwlock, NULL);
		if(0 != nRet)
		{
			//失败了
		}
	}

	/**
	* \brief 析构函数，用于销毁一个读写锁
	*
	*/
	~zRWLock()
	{
		int nRet = ::pthread_rwlock_destroy(&rwlock);
		if(0 != nRet)
		{
			//失败了
		}
	}

	/**
	* \brief 对读写锁进行读加锁操作
	*
	*/
	inline void rdlock()
	{
		int nRet = ::pthread_rwlock_rdlock(&rwlock);
		if(0 != nRet)
		{
			//失败了
		}
		++rd_count;
	};

	/**
	* \brief 对读写锁进行写加锁操作
	*
	*/
	inline void wrlock()
	{
		int nRet = ::pthread_rwlock_wrlock(&rwlock);
		if(0 != nRet)
		{
			//失败了
		}
		++wr_count;
		++rd_count;
	}

	/**
	* \brief 对读写锁进行解锁操作
	*
	*/
	inline void unlock()
	{
		int nRet = ::pthread_rwlock_unlock(&rwlock);
		if(0 != nRet)
		{
			//失败了
		}
		--rd_count;
	}

private:

	pthread_rwlock_t rwlock;    /**< 系统读写锁 */

};

/**
* \brief rdlock Wrapper
* 方便在复杂函数中读写锁的使用
*/
class zRWLock_scope_rdlock : private zNoncopyable
{

public:

	/**
	* \brief 构造函数
	* 对锁进行rdlock操作
	* \param m 锁的引用
	*/
	zRWLock_scope_rdlock(zRWLock &m) : rwlock(m)
	{
		rwlock.rdlock();
	}

	/**
	* \brief 析购函数
	* 对锁进行unlock操作
	*/
	~zRWLock_scope_rdlock()
	{
		rwlock.unlock();
	}

private:

	/**
	* \brief 锁的引用
	*/
	zRWLock &rwlock;

};

/**
* \brief wrlock Wrapper
* 方便在复杂函数中读写锁的使用
*/
class zRWLock_scope_wrlock : private zNoncopyable
{

public:

	/**
	* \brief 构造函数
	* 对锁进行wrlock操作
	* \param m 锁的引用
	*/
	zRWLock_scope_wrlock(zRWLock &m) : rwlock(m)
	{
		rwlock.wrlock();
	}

	/**
	* \brief 析购函数
	* 对锁进行unlock操作
	*/
	~zRWLock_scope_wrlock()
	{
		rwlock.unlock();
	}

private:

	/**
	* \brief 锁的引用
	*/
	zRWLock &rwlock;

};

#endif
