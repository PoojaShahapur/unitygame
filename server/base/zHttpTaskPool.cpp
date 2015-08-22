/**
 * \brief 实现线程池类,用于处理多连接服务器
 */

#include "zHttpTaskPool.h"
#include "zTime.h"

#include <iostream>
#include <sstream>
#include <string>
#include <vector>
#include <assert.h>
#include <ext/pool_allocator.h>


class zHttpThread : public zThread
{

    private:

	typedef std::list<zHttpTask *> zHttpTaskContainer;

	typedef std::vector<struct epoll_event> epollfdContainer;

	zHttpTaskPool *pool;
	zRTime currentTime;
	zMutex m_Lock;
	zHttpTaskContainer tasks;  /**< 任务列表 */

	int kdpfd;
	epollfdContainer epfds;
	epollfdContainer::size_type fds_count;      
    public:
	/**
	 * \brief 添加一个连接任务
	 * \param task 连接任务
	 */
	bool add(zHttpTask *task)
	{
	    bool retval = false;
	    m_Lock.lock();
	    if(task->addEpoll(kdpfd, EPOLLIN | EPOLLERR | EPOLLPRI, (void*)task))
	    {
		tasks.push_back(task);
		fds_count++;
		if(fds_count > epfds.size())
		{
		    epfds.resize(fds_count + 16);
		}
		retval = true;
	    }
	    zHttpTask **last = &tasks.back();
	    task->setaddr(last);
	    m_Lock.unlock();
	    return retval;
	}

	void remove(zHttpTask *task)
	{
	    task->delEpoll(kdpfd, EPOLLIN | EPOLLERR | EPOLLPRI);
	    tasks.remove(task);
	    *task->getaddr() = NULL;
	    SAFE_DELETE(task);
	    fds_count--;
	}

	void remove(zHttpTaskContainer::iterator &it)
	{
	    zHttpTask *task = *it;
	    task->delEpoll(kdpfd, EPOLLIN | EPOLLERR | EPOLLPRI);
	    *it = NULL;
	    tasks.erase(it);
	    SAFE_DELETE(task);
	    fds_count--;
	}

    public:

	/**
	 * \brief 构造函数
	 * \param pool 所属的连接池
	 * \param name 线程名称
	 */
	zHttpThread(
		zHttpTaskPool *pool,
		const std::string &name = std::string("zHttpThread"))
	    : zThread(name),pool(pool),currentTime()
	{
	    kdpfd = epoll_create(256);
	    assert(-1 != kdpfd);
	    epfds.resize(256);
	    fds_count = 0;
	}

	/**
	 * \brief 析构函数
	 *
	 */
	~zHttpThread()
	{
	    TEMP_FAILURE_RETRY(::close(kdpfd));
	}

	void run();

};

/**
 * \brief 等待接受验证指令,并进行验证
 *
 */
void zHttpThread::run()
{
    Zebra::logger->debug("zHttpThread::run");

    zHttpTaskContainer::iterator it,next;

    while(!isFinal())
    {
	m_Lock.lock();
	if(!tasks.empty())
	{
	    int retcode = epoll_wait(kdpfd, &epfds[0], fds_count, 0);
	    if(retcode > 0)
	    {
		for(int i = 0; i<retcode; i++)
		{
		    zHttpTask *task = (zHttpTask *)epfds[i].data.ptr;
		    if(epfds[i].events & (EPOLLERR | EPOLLPRI))
		    {
			Zebra::logger->error("套接口错误");
			remove(task);
		    }
		    else if(epfds[i].events & EPOLLIN)
		    {
			switch(task->httpCore())
			{
			    case 1://验证成功
			    case -1: //验证失败
				remove(task);
				break;
			    case 0://超时 下面会处理
				break;
			}
		    }
		}
	    }

	    currentTime.now();

	    for(it = tasks.begin(), next = it, ++next;  it != tasks.end(); it = next, ++next)
	    {
		zHttpTask *task = *it;
		if (task->checkHttpTimeout(currentTime, 100000) || task->ifdel())
		{
		    //超过指定时间验证还没有通过,需要回收连接
		    remove(it);
		}
	    }

	}
	m_Lock.unlock();

	zThread::msleep(50);
    }

    //把所有等待验证队列中的连接加入到回收队列中,回收这些连接
    for(it = tasks.begin(), next = it, ++next; it != tasks.end(); it= next, ++next)
    {
	remove(it);
    }
}


/**
 * \brief 把一个TCP连接添加到验证队列中,因为存在多个验证队列,需要按照一定的算法添加到不同的验证处理队列中
 *
 * \param task 一个连接任务
 */
bool zHttpTaskPool::addHttp(zHttpTask *task)
{

    Zebra::logger->debug("zHttpTaskPool::addHttp");
    //因为存在多个验证队列,需要按照一定的算法添加到不同的验证处理队列中
    static DWORD hashcode = 0;
    bool retval = false;
    zHttpThread *pHttpThread = (zHttpThread *)httpThreads.getByIndex(hashcode++ % maxHttpThreads);
    if (pHttpThread)
    {
	retval = pHttpThread->add(task);
    }
    return retval;
}

/**
 * \brief 初始化线程池,预先创建各种线程
 *
 * \return 初始化是否成功
 */
bool zHttpTaskPool::init()
{
    Zebra::logger->debug("zHttpTaskPool::init");
    //创建初始化验证线程
    for(int i = 0; i < maxHttpThreads; i++)
    {
	std::ostringstream name;
	name << "zHttpThread[" << i << "]";
	zHttpThread *pHttpThread = new zHttpThread(this,name.str());
	if (NULL == pHttpThread)
	    return false;
	if (!pHttpThread->start())
	    return false;
	httpThreads.add(pHttpThread);
    }

    return true;
}

/**
 * \brief 释放线程池,释放各种资源,等待各种线程退出
 *
 */
void zHttpTaskPool::final()
{
    httpThreads.joinAll();
}

