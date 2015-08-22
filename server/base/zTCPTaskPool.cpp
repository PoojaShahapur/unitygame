/**
 * \brief ʵ���̳߳���,���ڴ�������ӷ�����
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

unsigned long zTCPTaskPool::usleep_time=50000L;                    /**< ѭ���ȴ�ʱ�� */
/**
 * \brief ������������
 *
 */
//typedef std::list<zTCPTask *,__gnu_cxx::__pool_alloc<zTCPTask *> > zTCPTaskContainer;
typedef std::list<zTCPTask *> zTCPTaskContainer;

/**
 * \brief �����������������
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
 * \brief ����TCP���ӵ���֤,�����֤��ͨ��,��Ҫ�����������
 *
 */
class zVerifyThread : public zThread,public zTCPTaskQueue
{

    private:

	zTCPTaskPool *pool;
	zTCPTaskContainer tasks;  /**< �����б� */
	zTCPTaskContainer::size_type task_count;      /**< tasks����(��֤�̰߳�ȫ*/
	int kdpfd;
	epollfdContainer epfds;

	zMutex m_Lock;
	/**
	 * \brief ���һ����������
	 * \param task ��������
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
	 * \brief ���캯��
	 * \param pool ���������ӳ�
	 * \param name �߳�����
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
	 * \brief ��������
	 *
	 */
	~zVerifyThread()
	{
	    TEMP_FAILURE_RETRY(::close(kdpfd));
	}

	void run();

};

/**
 * \brief �ȴ�������ָ֤��,��������֤
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
		    //����ָ��ʱ����֤��û��ͨ��,��Ҫ��������
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
			Zebra::logger->error("�׽ӿڴ���");
			remove(task);
			task->resetState();
			pool->addRecycle(task);
		    }
		    else if(epfds[i].events & EPOLLIN)
		    {
			switch(task->verifyConn())
			{
			    case 1://��֤�ɹ�
				remove(task);
				if(task->uniqueAdd())
				{
				    task->setUnique();
				    pool->addSync(task);
				}
				else
				{
				    Zebra::logger->error("�ͻ���Ψһ����֤ʧ��");
				    task->resetState();
				    pool->addRecycle(task);
				}
				break;
			    case 0://��ʱ ����ᴦ��
				break;
			    case -1:
				Zebra::logger->error("�ͻ���������֤ʧ��");
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

    //�����еȴ���֤�����е����Ӽ��뵽���ն�����,������Щ����
    for(it = tasks.begin(), next = it, ++next; it != tasks.end(); it= next, ++next)
    {
	zTCPTask *task = *it;
	remove(it);
	task->resetState();
	pool->addRecycle(task);
    }
}

/**
 * \brief �ȴ������߳�ͬ����֤�������,���ʧ�ܻ��߳�ʱ,����Ҫ��������
 *
 */
class zSyncThread : public zThread,public zTCPTaskQueue
{

    private:

	zTCPTaskPool *pool;
	zTCPTaskContainer tasks;  /**< �����б� */

	zMutex m_Lock;
	void _add(zTCPTask *task)
	{
	    tasks.push_front(task);
	}

    public:

	/**
	 * \brief ���캯��
	 * \param pool ���������ӳ�
	 * \param name �߳�����
	 */
	zSyncThread(
		zTCPTaskPool *pool,
		const std::string &name = std::string("zSyncThread"))
	    : zThread(name),pool(pool)
	{}

	/**
	 * \brief ��������
	 *
	 */
	~zSyncThread() {};

	void run();

};

/**
 * \brief �ȴ������߳�ͬ����֤�������
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
			//�ȴ������߳�ͬ����֤�ɹ�
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
			//�ȴ������߳�ͬ����֤ʧ��,��Ҫ��������
			it = tasks.erase(it);
			task->resetState();
			pool->addRecycle(task);
			break;
		}
	    }
	}
	zThread::msleep(200);
    }

    //�����еȴ�ͬ����֤�����е����Ӽ��뵽���ն�����,������Щ����
    for(it = tasks.begin(); it != tasks.end();)
    {
	zTCPTask *task = *it;
	it = tasks.erase(it);
	task->resetState();
	pool->addRecycle(task);
    }
}

/**
 * \brief TCP���ӵ��������߳�,һ��һ���̴߳�����TCP����,���������������Ч��
 *
 */
class zOkayThread : public zThread,public zTCPTaskQueue
{

    private:


	zTCPTaskPool *pool;
	zTCPTaskContainer tasks;  /**< �����б� */
	zTCPTaskContainer::size_type task_count;      /**< tasks����(��֤�̰߳�ȫ*/
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

	static const zTCPTaskContainer::size_type connPerThread = 512;  /**< ÿ���̴߳����������� */

	/**
	 * \brief ���캯��
	 * \param pool ���������ӳ�
	 * \param name �߳�����
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
	 * \brief ��������
	 *
	 */
	~zOkayThread()
	{
	    TEMP_FAILURE_RETRY(::close(kdpfd));
	}

	void run();

	/**
	 * \brief ������������ĸ���
	 * \return ����̴߳��������������
	 */
	const zTCPTaskContainer::size_type size() const
	{
	    return task_count + _size;
	}

};

/**
 * \brief �������߳�,�ص��������ӵ��������ָ��
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
		    //�������ź�ָ��
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
 * \brief ���ӻ����߳�,�����������õ�TCP����,�ͷ���Ӧ����Դ
 *
 */

class zRecycleThread : public zThread,public zTCPTaskQueue
{

    private:

	zTCPTaskPool *pool;
	zTCPTaskContainer tasks;  /**< �����б� */

	zMutex m_Lock;

	void _add(zTCPTask *task)
	{
	    tasks.push_front(task);
	}

    public:

	/**
	 * \brief ���캯��
	 * \param pool ���������ӳ�
	 * \param name �߳�����
	 */
	zRecycleThread(
		zTCPTaskPool *pool,
		const std::string &name = std::string("zRecycleThread"))
	    : zThread(name),pool(pool)
	{}

	/**
	 * \brief ��������
	 *
	 */
	~zRecycleThread() {};

	void run();

};

/**
 * \brief ���ӻ��մ����߳�,��ɾ���ڴ�ռ�֮ǰ��Ҫ��֤recycleConn����1
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
			    //����Ѿ�ͨ����Ψһ����֤,��ȫ��Ψһ������ɾ��
			    task->uniqueRemove();
			task->getNextState();
			SAFE_DELETE(task);
			break;
		    case 0://���ճ�ʱ���´��ٴ���
			++it;
			break;
		}
	    }
	}

	zThread::msleep(200);
    }

    //�������е�����
    for(it = tasks.begin(); it != tasks.end();)
    {
	//���մ�����ɿ����ͷ���Ӧ����Դ
	zTCPTask *task = *it;
	it = tasks.erase(it);
	if (task->isUnique())
	    //����Ѿ�ͨ����Ψһ����֤,��ȫ��Ψһ������ɾ��
	    task->uniqueRemove();
	task->getNextState();
	SAFE_DELETE(task);
    }
}


/**
 * \brief �������ӳ��������Ӹ���
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
 * \brief ��һ��TCP������ӵ���֤������,��Ϊ���ڶ����֤����,��Ҫ����һ�����㷨��ӵ���ͬ����֤���������
 *
 * \param task һ����������
 */
bool zTCPTaskPool::addVerify(zTCPTask *task)
{

    Zebra::logger->debug("zTCPTaskPool::addVerify");
    //��Ϊ���ڶ����֤����,��Ҫ����һ�����㷨��ӵ���ͬ����֤���������
    static DWORD hashcode = 0;
    zVerifyThread *pVerifyThread = (zVerifyThread *)verifyThreads.getByIndex(hashcode++ % maxVerifyThreads);
    if (pVerifyThread)
    {
	// state_sync -> state_okay
	/*
	 * whj
	 * ������״̬���������,
	 * ����ᵼ��һ��taskͬʱ�������߳��е�Σ�����
	 */
	task->getNextState();
	pVerifyThread->add(task);
    }
    return true;
}

/**
 * \brief ��һ��ͨ����֤��TCP������ӵ��ȴ�ͬ����֤������
 *
 * \param task һ����������
 */
void zTCPTaskPool::addSync(zTCPTask *task)
{
    Zebra::logger->debug("zTCPTaskPool::addSync");
    // state_sync -> state_okay
    /*
     * whj
     * ������״̬���������,
     * ����ᵼ��һ��taskͬʱ�������߳��е�Σ�����
     */
    task->getNextState();
    syncThread->add(task);
}

/**
 * \brief ��һ��ͨ����֤��TCP���������
 *
 * \param task һ����������
 * \return ����Ƿ�ɹ�
 */
bool zTCPTaskPool::addOkay(zTCPTask *task)
{
    Zebra::logger->debug("zTCPTaskPool::addOkay");
    //���ȱ������е��߳�,�ҳ����еĲ������������ٵ��߳�,���ҳ�û���������߳�
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
	 * ������״̬���������,
	 * ����ᵼ��һ��taskͬʱ�������߳��е�Σ�����
	 */
	task->getNextState();
	//����߳�ͬʱ�������������û�е�������
	pmin->add(task);
	return true;
    }
    if (nostart)
    {
	//�̻߳�û������,��Ҫ�����߳�,�ٰ���ӵ�����̵߳Ĵ��������
	if (nostart->start())
	{
	    Zebra::logger->debug("zTCPTaskPool create work thread");
	    // state_sync -> state_okay
	    /*
	     * whj
	     * ������״̬���������,
	     * ����ᵼ��һ��taskͬʱ�������߳��е�Σ�����
	     */
	    task->getNextState();
	    //����߳�ͬʱ�������������û�е�������
	    nostart->add(task);
	    return true;
	}
	else
	    Zebra::logger->fatal("zTCPTaskPool can not create work thread");
    }

    Zebra::logger->fatal("zTCPTaskPool can not find property thread to deal conn");
    //û���ҵ��߳��������������,��Ҫ���չر�����
    return false;
}

/**
 * \brief ��һ��TCP������ӵ����մ��������
 *
 * \param task һ����������
 */

void zTCPTaskPool::addRecycle(zTCPTask *task)
{
    Zebra::logger->debug("zTCPTaskPool::addRecycle");
    recycleThread->add(task);
}


/**
 * \brief ��ʼ���̳߳�,Ԥ�ȴ��������߳�
 *
 * \return ��ʼ���Ƿ�ɹ�
 */
bool zTCPTaskPool::init()
{
    Zebra::logger->debug("zTCPTaskPool::init");
    //������ʼ����֤�߳�
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

    //������ʼ���ȴ�ͬ����֤�߳�
    syncThread = new zSyncThread(this);
    if (syncThread && !syncThread->start())
	return false;

    //������ʼ���������̳߳�
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

    //������ʼ�������̳߳�
    recycleThread = new zRecycleThread(this);
    if (recycleThread && !recycleThread->start())
	return false;

    return true;
}

/**
 * \brief �ͷ��̳߳�,�ͷŸ�����Դ,�ȴ������߳��˳�
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

