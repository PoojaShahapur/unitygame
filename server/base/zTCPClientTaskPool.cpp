/**
* \brief ʵ���̳߳���,���ڴ�������ӷ�����
*
* 
*/


#include "zTCPClientTaskPool.h"
#include <ext/pool_allocator.h>
#include <assert.h>
#include "zTime.h"


/**
* \brief ���TCP����״��,���δ����,��������
*
*/
class zCheckconnectThread : public zThread
{
private:
	zTCPClientTaskPool *pool;
public:
	zCheckconnectThread(
		zTCPClientTaskPool *pool,
		const std::string &name = std::string("zCheckconnectThread"))
		: zThread(name),pool(pool)
	{
	}
	virtual void run()
	{
		while(!isFinal())
		{
			zThread::sleep(4);
			zTime ct;
			pool->timeAction(ct);
		}
	}
};

/**
* \brief ������������
*
*/
//typedef std::list<zTCPClientTask *,__gnu_cxx::__pool_alloc<zTCPClientTask *> > zTCPClientTaskContainer;
typedef std::list<zTCPClientTask *> zTCPClientTaskContainer;

/**
* \brief �����������������
*
*/
typedef zTCPClientTaskContainer::iterator zTCPClientTask_IT;

typedef std::vector<struct epoll_event> epollfdContainer;

class zTCPClientTaskQueue
{
public:
	zTCPClientTaskQueue() :_size(0) {}
	virtual ~zTCPClientTaskQueue() {}
	inline void add(zTCPClientTask *task)
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
			zTCPClientTask *task = _queue.front();
			_queue.pop();
			_add(task);
		}
		_size = 0;
		mlock.unlock();
	}
protected:
	virtual void _add(zTCPClientTask *task) = 0;
	DWORD _size;
private:
	zMutex mlock;
	//std::queue<zTCPClientTask *,std::deque<zTCPClientTask *,__gnu_cxx::__pool_alloc<zTCPClientTask *> > > _queue;
	std::queue<zTCPClientTask *> _queue;
};

/**
* \brief ����TCP���ӵ���֤,�����֤��ͨ��,��Ҫ�����������
*
*/
class zCheckwaitThread : public zThread,public zTCPClientTaskQueue
{

private:

	zTCPClientTaskPool *pool;
	zTCPClientTaskContainer tasks;  /**< �����б� */
	zTCPClientTaskContainer::size_type task_count;          /**< tasks����(��֤�̰߳�ȫ*/
	int kdpfd;
	epollfdContainer epfds;

	/**
	* \brief ���һ����������
	* \param task ��������
	*/
	void _add(zTCPClientTask *task)
	{
		Zebra::logger->debug("zCheckwaitThread::_add");

		task->addEpoll(kdpfd, EPOLLIN | EPOLLOUT| EPOLLERR| EPOLLPRI, (void *)task);
		tasks.push_back(task);
		task_count = tasks.size();
		if(task_count > epfds.size())
		{
			epfds.resize(task_count + 16);
		}
	}

	void remove(zTCPClientTask *task)
	{
		Zebra::logger->debug("zCheckwaitThread::remove");
		task->delEpoll(kdpfd, EPOLLIN | EPOLLOUT| EPOLLERR| EPOLLPRI);
		tasks.remove(task);
		task_count = tasks.size();
	}

	void remove(zTCPClientTask_IT &it)
	{
		Zebra::logger->debug("zCheckwaitThread::remove");
		(*it)->delEpoll(kdpfd, EPOLLIN | EPOLLOUT| EPOLLERR| EPOLLPRI);
		tasks.erase(it);
		task_count = tasks.size();
	}

public:

	/**
	* \brief ���캯��
	* \param pool ���������ӳ�
	* \param name �߳�����
	*/
	zCheckwaitThread(
		zTCPClientTaskPool *pool,
		const std::string &name = std::string("zCheckwaitThread"))
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
	~zCheckwaitThread()
	{
		TEMP_FAILURE_RETRY(::close(kdpfd));
	}

	virtual void run();

};

/**
* \brief �ȴ�������ָ֤��,��������֤
*
*/
void zCheckwaitThread::run()
{
	Zebra::logger->debug("zCheckwaitThread::run");

	zTCPClientTask_IT it,next;

	while(!isFinal())
	{

		check_queue();

		{

			if(!tasks.empty())
			{

				int retcode = epoll_wait(kdpfd, &epfds[0], task_count, 0);
				if(retcode > 0)
				{
					for(int i = 0; i<retcode; i++)
					{
						zTCPClientTask *task = (zTCPClientTask *)epfds[i].data.ptr;
						if(epfds[i].events & (EPOLLERR | EPOLLPRI))
						{
							Zebra::logger->error("�׽ӿڴ���");
							remove(task);
							task->resetState();
						}
						else if(epfds[i].events & EPOLLIN)
						{
							switch(task->checkRebound())
							{
							case 1://��֤�ɹ�
								remove(task);
								if(!pool->addMain(task))
								{
									task->resetState();

								}
								break;
							case 0://��ʱ ����ᴦ��
								break;
							case -1:

								remove(task);
								task->resetState();

								break;
							}
						}
					}
				}
			}

		}

		zThread::msleep(50);
	}

	for(it = tasks.begin(), next = it,++next; it!= tasks.end(); it = next, ++next)
	{
		zTCPClientTask *task = *it;
		remove(it);
		task->resetState();

	}

}

/**
* \brief TCP���ӵ��������߳�,һ��һ���̴߳�����TCP����,���������������Ч��
*
*/
class zTCPClientTaskThread : public zThread,public zTCPClientTaskQueue
{

private:

	zTCPClientTaskPool *pool;
	zTCPClientTaskContainer tasks;  /**< �����б� */
	zTCPClientTaskContainer::size_type task_count;          /**< tasks����(��֤�̰߳�ȫ*/
	int kdpfd;
	epollfdContainer epfds;

	zMutex m_Lock;
	/**
	* \brief ���һ����������
	* \param task ��������
	*/
	void _add(zTCPClientTask *task)
	{
		Zebra::logger->debug("zCheckwaitThread::_add");

		task->addEpoll(kdpfd, EPOLLIN | EPOLLOUT| EPOLLERR| EPOLLPRI, (void *)task);
		tasks.push_back(task);
		task_count = tasks.size();
		if(task_count > epfds.size())
		{
			epfds.resize(task_count + 16);
		}
	}

	void remove(zTCPClientTask *task)
	{
		Zebra::logger->debug("zCheckwaitThread::remove");
		task->delEpoll(kdpfd, EPOLLIN | EPOLLOUT| EPOLLERR| EPOLLPRI);
		tasks.remove(task);
		task_count = tasks.size();
	}

	void remove(zTCPClientTask_IT &it)
	{
		Zebra::logger->debug("zCheckwaitThread::remove");
		(*it)->delEpoll(kdpfd, EPOLLIN | EPOLLOUT| EPOLLERR| EPOLLPRI);
		tasks.erase(it);
		task_count = tasks.size();
	}

public:

	static const zTCPClientTaskContainer::size_type connPerThread = 256;  /**< ÿ���̴߳����������� */

	/**
	* \brief ���캯��
	* \param pool ���������ӳ�
	* \param name �߳�����
	*/
	zTCPClientTaskThread(
		zTCPClientTaskPool *pool,
		const std::string &name = std::string("zTCPClientTaskThread"))
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
	~zTCPClientTaskThread()
	{
		TEMP_FAILURE_RETRY(::close(kdpfd));
	}

	virtual void run();

	/**
	* \brief ������������ĸ���
	* \return ����̴߳��������������
	*/
	const zTCPClientTaskContainer::size_type size() const
	{
		return task_count + _size;
	}

};

/**
* \brief �������߳�,�ص��������ӵ��������ָ��
*
*/
void zTCPClientTaskThread::run()
{
	Zebra::logger->debug("zTCPClientTaskThread::run");
	zRTime currentTime;
	zRTime _1_msec(currentTime), _50_msec(currentTime);
	zTCPClientTask_IT it,next;
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
					zTCPClientTask *task = *it;

					if (task->isTerminate())
					{
						if(task->isFdsrAdd())
						{

							task->delEpoll(kdpfd_r, EPOLLIN | EPOLLERR | EPOLLPRI);
							fds_count_r--;
							task->fdsrAdd(false);
						}
						remove(it);
						task->getNextState();

					}
					else
					{
						if(task->checkFirstMainLoop())
						{
							task->ListeningRecv(false);
						}
						if(!task->isFdsrAdd())
						{
							task->addEpoll(kdpfd_r, EPOLLIN | EPOLLERR | EPOLLPRI, (void*)task);
							task->fdsrAdd(true);
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
					zTCPClientTask *task = (zTCPClientTask *)epfds_r[i].data.ptr;
					if(epfds_r[i].events & (EPOLLERR | EPOLLPRI))
					{
						task->Terminate(zTCPClientTask::TM_sock_error);
						check = true;
					}
					else
					{
						if(epfds[i].events & EPOLLIN)
						{
							if(!task->ListeningRecv(true))
							{
								task->Terminate(zTCPClientTask::TM_sock_error);
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
						zTCPClientTask *task = (zTCPClientTask *)epfds[i].data.ptr;
						if(epfds[i].events & (EPOLLERR | EPOLLPRI))
						{
							task->Terminate(zTCPClientTask::TM_sock_error);
						}
						else
						{
							if(epfds[i].events & EPOLLIN)
							{
								if(!task->ListeningRecv(true))
								{
									task->Terminate(zTCPClientTask::TM_sock_error);
								}
							}
							if(epfds[i].events & EPOLLOUT)
							{
								if(!task->ListeningSend())
								{
									task->Terminate(zTCPClientTask::TM_sock_error);
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
		zTCPClientTask *task = *it;
		remove(it);
		task->getNextState();

	}


}



/**
* \brief ��������
*
*/
zTCPClientTaskPool::~zTCPClientTaskPool()
{
	if (checkconnectThread)
	{
		checkconnectThread->final();
		checkconnectThread->join();
		SAFE_DELETE(checkconnectThread);
	}
	if (checkwaitThread)
	{
		checkwaitThread->final();
		checkwaitThread->join();
		SAFE_DELETE(checkwaitThread);
	}

	taskThreads.joinAll();

	zTCPClientTask_IT it,next;


	if(tasks.size() > 0)
		for(it = tasks.begin(),next = it,next++; it != tasks.end(); it = next,next == tasks.end()? next : next++)
		{
			zTCPClientTask *task = *it;
			tasks.erase(it);
			SAFE_DELETE(task);
		}
}

zTCPClientTaskThread *zTCPClientTaskPool::newThread()
{
	std::ostringstream name;
	name << "zTCPClientTaskThread[" << taskThreads.size() << "]";
	zTCPClientTaskThread *taskThread = new zTCPClientTaskThread(this,name.str());
	if (NULL == taskThread)
		return NULL;
	if (!taskThread->start())
		return NULL;
	taskThreads.add(taskThread);
	return taskThread;
}

/**
* \brief ��ʼ���̳߳�,Ԥ�ȴ��������߳�
*
* \return ��ʼ���Ƿ�ɹ�
*/
bool zTCPClientTaskPool::init()
{
	checkconnectThread = new zCheckconnectThread(this); 
	if (NULL == checkconnectThread)
		return false;
	if (!checkconnectThread->start())
		return false;
	checkwaitThread = new zCheckwaitThread(this);
	if (NULL == checkwaitThread)
		return false;
	if (!checkwaitThread->start())
		return false;

	if (NULL == newThread())
		return false;

	return true;
}

/**
* \brief ��һ��ָ��������ӵ�����
* \param task ����ӵ�����
*/
bool zTCPClientTaskPool::put(zTCPClientTask *task)
{
	if (task)
	{
		mlock.lock();
		tasks.push_front(task);
		mlock.unlock();
		return true;
	}
	else
		return false;
}

/**
* \brief ��ʱִ�е�����
* ��Ҫ������ͻ��˶��߳�������
*/
void zTCPClientTaskPool::timeAction(const zTime &ct)
{
	mlock.lock();
	for(zTCPClientTask_IT it = tasks.begin(); it != tasks.end(); ++it)
	{
		zTCPClientTask *task = *it;
		switch(task->getState())
		{
		case zTCPClientTask::close:
			if (task->checkStateTimeout(zTCPClientTask::close,ct,4)
				&& task->connect())
			{
				addCheckwait(task);
			}
			break;
		case zTCPClientTask::sync:
			break;
		case zTCPClientTask::okay:
			//�Ѿ�������״̬,������������ź�
			task->checkConn();
			break;
		case zTCPClientTask::recycle:
			if (task->checkStateTimeout(zTCPClientTask::recycle,ct,4))
				task->getNextState();
			break;
		}
	}
	mlock.unlock();
}

/**
* \brief ��������ӵ��ȴ�������֤���صĶ�����
* \param task ����ӵ�����
*/
void zTCPClientTaskPool::addCheckwait(zTCPClientTask *task)
{
	checkwaitThread->add(task);
	task->getNextState();
}

/**
* \brief ��������ӵ�������ѭ����
* \param task ����ӵ�����
* \return ����Ƿ�ɹ�
*/
bool zTCPClientTaskPool::addMain(zTCPClientTask *task)
{
	zTCPClientTaskThread *taskThread = NULL;
	for(DWORD i = 0; i < taskThreads.size(); i++)
	{
		zTCPClientTaskThread *tmp = (zTCPClientTaskThread *)taskThreads.getByIndex(i);
		//Zebra::logger->debug("%u",tmp->size());
		if (tmp && tmp->size() < connPerThread)
		{
			taskThread = tmp;
			break;
		}
	}
	if (NULL == taskThread)
		taskThread = newThread();
	if (taskThread)
	{
		taskThread->add(task);
		task->getNextState();
		return true;
	}
	else
	{
		Zebra::logger->fatal("zTCPClientTaskPool::addMain: ���ܵõ�һ�������߳�");
		return false;
	}
}

