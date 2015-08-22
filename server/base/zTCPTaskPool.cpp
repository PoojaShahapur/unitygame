/**
 * \brief 实现线程池类,用于处理多连接服务器
 */

#include "zTCPTaskPool.h"
#include "zTime.h"

#include <iostream>
#include <sstream>
#include <string>
#include <vector>
#include <assert.h>
#include <ext/pool_allocator.h>

zTCPTask* g_DeleteLog = NULL;

zMutex g_DeleteLock;

unsigned long zTCPTaskPool::usleep_time=50000L;                    /**< 循环等待时间 */
/**
 * \brief 连接任务链表
 *
 */
//typedef std::list<zTCPTask *,__gnu_cxx::__pool_alloc<zTCPTask *> > zTCPTaskContainer;
typedef std::list<zTCPTask *> zTCPTaskContainer;

/**
 * \brief 连接任务链表叠代器
 *
 */
typedef zTCPTaskContainer::iterator zTCPTask_IT;

typedef std::vector<struct epoll_event> epollfdContainer;

class zTCPTaskQueue
{
    public:
	zTCPTaskQueue() :_size(0) {}
	virtual ~zTCPTaskQueue() {}
	inline void add(zTCPTask *task)
	{
	    mlock.lock();
	    _queue.push(task);
	    _size++;
	    mlock.unlock();
	}
	inline void check_queue()
	{
	    mlock.lock();
	    while(!_queue.empty())
	    {
		_temp_queue.push(_queue.front());
		_queue.pop();
	    }
	    _size = 0;
	    mlock.unlock();
	    while(!_temp_queue.empty())
	    {
		zTCPTask *task = _temp_queue.front();
		_temp_queue.pop();
		_add(task);
	    }
	}
    protected:
	virtual void _add(zTCPTask *task) = 0;
	DWORD _size;
    private:
	zMutex mlock;
	//std::queue<zTCPTask *,std::deque<zTCPTask *,__gnu_cxx::__pool_alloc<zTCPTask *> > > _queue;
	std::queue<zTCPTask *, std::deque<zTCPTask *> > _queue;
	std::queue<zTCPTask *, std::deque<zTCPTask *> > _temp_queue;
};

/**
 * \brief 处理TCP连接的验证,如果验证不通过,需要回收这个连接
 *
 */
class zVerifyThread : public zThread,public zTCPTaskQueue
{

    private:

	zTCPTaskPool *pool;
	zTCPTaskContainer tasks;  /**< 任务列表 */
	zTCPTaskContainer::size_type task_count;      /**< tasks计数(保证线程安全*/
	int kdpfd;
	epollfdContainer epfds;

	zMutex m_Lock;
	/**
	 * \brief 添加一个连接任务
	 * \param task 连接任务
	 */
	void _add(zTCPTask *task)
	{
	    task->addEpoll(kdpfd, EPOLLIN | EPOLLERR | EPOLLPRI, (void*)task);
	    tasks.push_back(task);
	    task_count = tasks.size();
	    if(task_count > epfds.size())
	    {
		epfds.resize(task_count + 16);
	    }
	}

	void remove(zTCPTask *task)
	{
	    task->delEpoll(kdpfd, EPOLLIN | EPOLLERR | EPOLLPRI);
	    tasks.remove(task);
	    task_count = tasks.size();
	}

	void remove(zTCPTask_IT &it)
	{
	    (*it)->delEpoll(kdpfd, EPOLLIN | EPOLLERR | EPOLLPRI);
	    tasks.erase(it);
	    task_count = tasks.size();
	}

    public:

	/**
	 * \brief 构造函数
	 * \param pool 所属的连接池
	 * \param name 线程名称
	 */
	zVerifyThread(
		zTCPTaskPool *pool,
		const std::string &name = std::string("zVerifyThread"))
	    : zThread(name),pool(pool)
	{
	    task_count = 0;
	    kdpfd = epoll_create(256);
	    assert(-1 != kdpfd);
	    epfds.resize(256);
	}

	/**
	 * \brief 析构函数
	 *
	 */
	~zVerifyThread()
	{
	    TEMP_FAILURE_RETRY(::close(kdpfd));
	}

	void run();

};

/**
 * \brief 等待接受验证指令,并进行验证
 *
 */
void zVerifyThread::run()
{
    Zebra::logger->debug("zVerifyThread::run");

    zRTime currentTime;
    zTCPTask_IT it,next;

    while(!isFinal())
    {
	currentTime.now();
	check_queue();
	if(!tasks.empty())
	{
	    for(it = tasks.begin(), next = it, ++next;  it != tasks.end(); it = next, ++next)
	    {
		zTCPTask *task = *it;
		if (task->checkVerifyTimeout(currentTime))
		{
		    //超过指定时间验证还没有通过,需要回收连接
		    remove(it);
		    task->resetState();
		    pool->addRecycle(task);
		}
	    }

	    int retcode = epoll_wait(kdpfd, &epfds[0], task_count, 0);
	    if(retcode > 0)
	    {
		for(int i = 0; i<retcode; i++)
		{
		    zTCPTask *task = (zTCPTask *)epfds[i].data.ptr;
		    if(epfds[i].events & (EPOLLERR | EPOLLPRI))
		    {
			Zebra::logger->error("套接口错误");
			remove(task);
			task->resetState();
			pool->addRecycle(task);
		    }
		    else if(epfds[i].events & EPOLLIN)
		    {
			switch(task->verifyConn())
			{
			    case 1://验证成功
				remove(task);
				if(task->uniqueAdd())
				{
				    task->setUnique();
				    pool->addSync(task);
				}
				else
				{
				    Zebra::logger->error("客户端唯一性验证失败");
				    task->resetState();
				    pool->addRecycle(task);
				}
				break;
			    case 0://超时 下面会处理
				break;
			    case -1:
				Zebra::logger->error("客户端连接验证失败");
				remove(task);
				task->resetState();
				pool->addRecycle(task);
				break;
			}
		    }
		}
	    }
	}

	zThread::msleep(50);
    }

    //把所有等待验证队列中的连接加入到回收队列中,回收这些连接
    for(it = tasks.begin(), next = it, ++next; it != tasks.end(); it= next, ++next)
    {
	zTCPTask *task = *it;
	remove(it);
	task->resetState();
	pool->addRecycle(task);
    }
}

/**
 * \brief 等待其它线程同步验证这个连接,如果失败或者超时,都需要回收连接
 *
 */
class zSyncThread : public zThread,public zTCPTaskQueue
{

    private:

	zTCPTaskPool *pool;
	zTCPTaskContainer tasks;  /**< 任务列表 */

	zMutex m_Lock;
	void _add(zTCPTask *task)
	{
	    tasks.push_front(task);
	}

    public:

	/**
	 * \brief 构造函数
	 * \param pool 所属的连接池
	 * \param name 线程名称
	 */
	zSyncThread(
		zTCPTaskPool *pool,
		const std::string &name = std::string("zSyncThread"))
	    : zThread(name),pool(pool)
	{}

	/**
	 * \brief 析构函数
	 *
	 */
	~zSyncThread() {};

	void run();

};

/**
 * \brief 等待其它线程同步验证这个连接
 *
 */
void zSyncThread::run()
{
    Zebra::logger->debug("zSyncThread::run");
    zTCPTask_IT it;

    while(!isFinal())
    {
	check_queue();
	if (!tasks.empty())
	{
	    for(it = tasks.begin(); it != tasks.end();)
	    {
		zTCPTask *task = (*it);
		switch(task->waitSync())
		{
		    case 1:
			//等待其它线程同步验证成功
			it = tasks.erase(it);
			if (!pool->addOkay(task))
			{
			    task->resetState();
			    pool->addRecycle(task);
			}
			break;
		    case 0:
			++it;
			break;
		    case -1:
			//等待其它线程同步验证失败,需要回收连接
			it = tasks.erase(it);
			task->resetState();
			pool->addRecycle(task);
			break;
		}
	    }
	}
	zThread::msleep(200);
    }

    //把所有等待同步验证队列中的连接加入到回收队列中,回收这些连接
    for(it = tasks.begin(); it != tasks.end();)
    {
	zTCPTask *task = *it;
	it = tasks.erase(it);
	task->resetState();
	pool->addRecycle(task);
    }
}

/**
 * \brief TCP连接的主处理线程,一般一个线程带几个TCP连接,这样可以显著提高效率
 *
 */
class zOkayThread : public zThread,public zTCPTaskQueue
{

    private:


	zTCPTaskPool *pool;
	zTCPTaskContainer tasks;  /**< 任务列表 */
	zTCPTaskContainer::size_type task_count;      /**< tasks计数(保证线程安全*/
	int kdpfd;
	epollfdContainer epfds;

	zMutex m_Lock;

	void _add(zTCPTask *task)
	{
	    task->addEpoll(kdpfd, EPOLLIN |EPOLLOUT | EPOLLERR | EPOLLPRI, (void*)task);
	    tasks.push_back(task);
	    task_count = tasks.size();
	    if(task_count > epfds.size())
	    {
		epfds.resize(task_count + 16);
	    }
	    task->ListeningRecv(false);
	}

	void remove(zTCPTask_IT &it)
	{
	    (*it)->delEpoll(kdpfd, EPOLLIN |EPOLLOUT| EPOLLERR | EPOLLPRI);
	    tasks.erase(it);
	    task_count = tasks.size();
	}

    public:

	static const zTCPTaskContainer::size_type connPerThread = 512;  /**< 每个线程带的连接数量 */

	/**
	 * \brief 构造函数
	 * \param pool 所属的连接池
	 * \param name 线程名称
	 */
	zOkayThread(
		zTCPTaskPool *pool,
		const std::string &name = std::string("zOkayThread"))
	    : zThread(name),pool(pool)
	{

	    task_count = 0;
	    kdpfd = epoll_create(connPerThread);
	    assert(-1 != kdpfd);
	    epfds.resize(connPerThread);
	}

	/**
	 * \brief 析构函数
	 *
	 */
	~zOkayThread()
	{
	    TEMP_FAILURE_RETRY(::close(kdpfd));
	}

	void run();

	/**
	 * \brief 返回连接任务的个数
	 * \return 这个线程处理的连接任务数
	 */
	const zTCPTaskContainer::size_type size() const
	{
	    return task_count + _size;
	}

};

/**
 * \brief 主处理线程,回调处理连接的输入输出指令
 *
 */
void zOkayThread::run()
{
    Zebra::logger->debug("zOkayThread::run");

    zRTime currentTime;
    zRTime _1_msec(currentTime), _50_msec(currentTime);
    zTCPTask_IT it,next;

    int kdpfd_r;
    epollfdContainer epfds_r;
    kdpfd_r = epoll_create(256);
    assert(-1 != kdpfd_r);
    epfds.resize(256);
    DWORD fds_count_r = 0;
    bool check = false;
    while(!isFinal())
    {
	currentTime.now();
	if(check)
	{
	    check_queue();
	    if (!tasks.empty())
	    {
		for(it = tasks.begin(), next = it, ++next; it != tasks.end(); it = next, ++next)
		{
		    zTCPTask *task = *it;
		    //检查测试信号指令
		    task->checkSignal(currentTime);

		    if (task->isTerminateWait())
		    {
			task->Terminate();
		    }
		    if (task->isTerminate())
		    {
			if(task->isFdsrAdd())
			{

			    task->delEpoll(kdpfd_r, EPOLLIN | EPOLLERR | EPOLLPRI);
			    fds_count_r--;
			}
			remove(it);
			task->getNextState();
			pool->addRecycle(task);
		    }
		    else
		    {
			if(!task->isFdsrAdd())
			{
			    task->addEpoll(kdpfd_r, EPOLLIN | EPOLLERR | EPOLLPRI, (void*)task);
			    task->fdsrAdd();
			    fds_count_r++;
			    if(fds_count_r > epfds_r.size())
			    {
				epfds_r.resize(fds_count_r + 16);
			    }
			}
		    }
		}
	    }
	    check = false;
	}

	zThread::msleep(2);
	if(fds_count_r && _1_msec.elapse(currentTime) >= 2)
	{
	    int retcode = epoll_wait(kdpfd_r, &epfds_r[0], fds_count_r, 0);
	    if(retcode > 0)
	    {
		for(int i = 0; i < retcode; i++)
		{
		    zTCPTask *task = (zTCPTask *)epfds_r[i].data.ptr;
		    if(epfds_r[i].events & (EPOLLERR | EPOLLPRI))
		    {
			task->Terminate(zTCPTask::terminate_active);
			check = true;
		    }
		    else
		    {
			if(epfds[i].events & EPOLLIN)
			{
			    if(!task->ListeningRecv(true))
			    {
				task->Terminate(zTCPTask::terminate_active);
				check = true;
			    }
			}
		    }
		    epfds_r[i].events = 0;
		}
	    }
	}
	if(check)
	{
	    continue;
	}
	if(_50_msec.elapse(currentTime) >= (pool->usleep_time/1000))
	{
	    _50_msec = currentTime;
	    if(!tasks.empty())
	    {
		int retcode = epoll_wait(kdpfd, &epfds[0], task_count, 0);
		if(retcode > 0)
		{
		    for(int i = 0; i < retcode; i++)
		    {
			zTCPTask *task = (zTCPTask *)epfds[i].data.ptr;
			if(epfds[i].events & (EPOLLERR | EPOLLPRI))
			{
			    task->Terminate(zTCPTask::terminate_active);
			}
			else
			{
			    if(epfds[i].events & EPOLLIN)
			    {
				if(!task->ListeningRecv(true))
				{
				    task->Terminate(zTCPTask::terminate_active);
				}
			    }
			    if(epfds[i].events & EPOLLOUT)
			    {
				if(!task->ListeningSend())
				{
				    task->Terminate(zTCPTask::terminate_active);
				}
			    }
			}
			epfds[i].events = 0;
		    }
		}

	    }
	    check = true;
	}
    }

    for(it = tasks.begin(), next = it,++next; it!= tasks.end(); it = next, ++next)
    {
	zTCPTask *task = *it;
	remove(it);
	task->getNextState();
	pool->addRecycle(task);
    }
    TEMP_FAILURE_RETRY(::close(kdpfd_r));
}

/**
 * \brief 连接回收线程,回收所有无用的TCP连接,释放相应的资源
 *
 */

class zRecycleThread : public zThread,public zTCPTaskQueue
{

    private:

	zTCPTaskPool *pool;
	zTCPTaskContainer tasks;  /**< 任务列表 */

	zMutex m_Lock;

	void _add(zTCPTask *task)
	{
	    tasks.push_front(task);
	}

    public:

	/**
	 * \brief 构造函数
	 * \param pool 所属的连接池
	 * \param name 线程名称
	 */
	zRecycleThread(
		zTCPTaskPool *pool,
		const std::string &name = std::string("zRecycleThread"))
	    : zThread(name),pool(pool)
	{}

	/**
	 * \brief 析构函数
	 *
	 */
	~zRecycleThread() {};

	void run();

};

/**
 * \brief 连接回收处理线程,在删除内存空间之前需要保证recycleConn返回1
 *
 */
//std::map<zTCPTask*,int> g_RecycleLog;
void zRecycleThread::run()
{
    Zebra::logger->debug("zRecycleThread::run");
    zTCPTask_IT it;

    while(!isFinal())
    {		
	check_queue();
	if (!tasks.empty())
	{
	    for(it = tasks.begin(); it != tasks.end();)
	    {
		zTCPTask *task = *it;
		switch(task->recycleConn())
		{
		    case 1:
			it = tasks.erase(it);
			if (task->isUnique())
			    //如果已经通过了唯一性验证,从全局唯一容器中删除
			    task->uniqueRemove();
			task->getNextState();
			SAFE_DELETE(task);
			break;
		    case 0://回收超时，下次再处理
			++it;
			break;
		}
	    }
	}

	zThread::msleep(200);
    }

    //回收所有的连接
    for(it = tasks.begin(); it != tasks.end();)
    {
	//回收处理完成可以释放相应的资源
	zTCPTask *task = *it;
	it = tasks.erase(it);
	if (task->isUnique())
	    //如果已经通过了唯一性验证,从全局唯一容器中删除
	    task->uniqueRemove();
	task->getNextState();
	SAFE_DELETE(task);
    }
}


/**
 * \brief 返回连接池中子连接个数
 *
 */
const int zTCPTaskPool::getSize()
{
    Zebra::logger->debug("zTCPTaskPool::getSize");
    struct MyCallback : zThreadGroup::Callback
    {
	int size;
	MyCallback() : size(0) {}
	void exec(zThread *e)
	{
	    zOkayThread *pOkayThread = (zOkayThread *)e;
	    size += pOkayThread->size();
	}
    };
    MyCallback mcb;
    okayThreads.execAll(mcb);
    return mcb.size;
}

/**
 * \brief 把一个TCP连接添加到验证队列中,因为存在多个验证队列,需要按照一定的算法添加到不同的验证处理队列中
 *
 * \param task 一个连接任务
 */
bool zTCPTaskPool::addVerify(zTCPTask *task)
{

    Zebra::logger->debug("zTCPTaskPool::addVerify");
    //因为存在多个验证队列,需要按照一定的算法添加到不同的验证处理队列中
    static DWORD hashcode = 0;
    zVerifyThread *pVerifyThread = (zVerifyThread *)verifyThreads.getByIndex(hashcode++ % maxVerifyThreads);
    if (pVerifyThread)
    {
	// state_sync -> state_okay
	/*
	 * whj
	 * 先设置状态再添加容器,
	 * 否则会导致一个task同时在两个线程中的危险情况
	 */
	task->getNextState();
	pVerifyThread->add(task);
    }
    return true;
}

/**
 * \brief 把一个通过验证的TCP连接添加到等待同步验证队列中
 *
 * \param task 一个连接任务
 */
void zTCPTaskPool::addSync(zTCPTask *task)
{
    Zebra::logger->debug("zTCPTaskPool::addSync");
    // state_sync -> state_okay
    /*
     * whj
     * 先设置状态再添加容器,
     * 否则会导致一个task同时在两个线程中的危险情况
     */
    task->getNextState();
    syncThread->add(task);
}

/**
 * \brief 把一个通过验证的TCP处理队列中
 *
 * \param task 一个连接任务
 * \return 添加是否成功
 */
bool zTCPTaskPool::addOkay(zTCPTask *task)
{
    Zebra::logger->debug("zTCPTaskPool::addOkay");
    //首先遍历所有的线程,找出运行的并且连接数最少的线程,再找出没有启动的线程
    zOkayThread *pmin = NULL,*nostart = NULL;
    for(int i = 0; i < maxThreadCount; i++)
    {
	zOkayThread *pOkayThread = (zOkayThread *)okayThreads.getByIndex(i);
	if (pOkayThread)
	{
	    if (pOkayThread->isAlive())
	    {
		if (NULL == pmin || pmin->size() > pOkayThread->size())
		    pmin = pOkayThread;
	    }
	    else
	    {
		nostart = pOkayThread;
		break;
	    }
	}
    }
    if (pmin && pmin->size() < zOkayThread::connPerThread)
    {
	// state_sync -> state_okay
	/*
	 * whj
	 * 先设置状态再添加容器,
	 * 否则会导致一个task同时在两个线程中的危险情况
	 */
	task->getNextState();
	//这个线程同时处理的连接数还没有到达上限
	pmin->add(task);
	return true;
    }
    if (nostart)
    {
	//线程还没有运行,需要创建线程,再把添加到这个线程的处理队列中
	if (nostart->start())
	{
	    Zebra::logger->debug("zTCPTaskPool create work thread");
	    // state_sync -> state_okay
	    /*
	     * whj
	     * 先设置状态再添加容器,
	     * 否则会导致一个task同时在两个线程中的危险情况
	     */
	    task->getNextState();
	    //这个线程同时处理的连接数还没有到达上限
	    nostart->add(task);
	    return true;
	}
	else
	    Zebra::logger->fatal("zTCPTaskPool can not create work thread");
    }

    Zebra::logger->fatal("zTCPTaskPool can not find property thread to deal conn");
    //没有找到线程来处理这个连接,需要回收关闭连接
    return false;
}

/**
 * \brief 把一个TCP连接添加到回收处理队列中
 *
 * \param task 一个连接任务
 */

void zTCPTaskPool::addRecycle(zTCPTask *task)
{
    Zebra::logger->debug("zTCPTaskPool::addRecycle");
    recycleThread->add(task);
}


/**
 * \brief 初始化线程池,预先创建各种线程
 *
 * \return 初始化是否成功
 */
bool zTCPTaskPool::init()
{
    Zebra::logger->debug("zTCPTaskPool::init");
    //创建初始化验证线程
    for(int i = 0; i < maxVerifyThreads; i++)
    {
	std::ostringstream name;
	name << "zVerifyThread[" << i << "]";
	zVerifyThread *pVerifyThread = new zVerifyThread(this,name.str());
	if (NULL == pVerifyThread)
	    return false;
	if (!pVerifyThread->start())
	    return false;
	verifyThreads.add(pVerifyThread);
    }

    //创建初始化等待同步验证线程
    syncThread = new zSyncThread(this);
    if (syncThread && !syncThread->start())
	return false;

    //创建初始化主运行线程池
    maxThreadCount = (maxConns + zOkayThread::connPerThread - 1) / zOkayThread::connPerThread;
    Zebra::logger->debug("maxConns Capacity is:%d, connPerThread:%d, thread num:%d",maxConns,zOkayThread::connPerThread,maxThreadCount);
    for(int i = 0; i < maxThreadCount; i++)
    {
	std::ostringstream name;
	name << "zOkayThread[" << i << "]";
	zOkayThread *pOkayThread = new zOkayThread(this,name.str());
	if (NULL == pOkayThread)
	    return false;
	if (i < minThreadCount && !pOkayThread->start())
	    return false;
	okayThreads.add(pOkayThread);
    }

    //创建初始化回收线程池
    recycleThread = new zRecycleThread(this);
    if (recycleThread && !recycleThread->start())
	return false;

    return true;
}

/**
 * \brief 释放线程池,释放各种资源,等待各种线程退出
 *
 */
void zTCPTaskPool::final()
{
    verifyThreads.joinAll();
    if (syncThread)
    {
	syncThread->final();
	syncThread->join();
	SAFE_DELETE(syncThread);
    }

    okayThreads.joinAll();
    if (recycleThread)
    {
	recycleThread->final();
	recycleThread->join();
	SAFE_DELETE(recycleThread);
    }
}

