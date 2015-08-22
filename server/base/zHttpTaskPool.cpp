/**
 * \brief ʵ���̳߳���,���ڴ�������ӷ�����
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
	zHttpTaskContainer tasks;  /**< �����б� */

	int kdpfd;
	epollfdContainer epfds;
	epollfdContainer::size_type fds_count;      
    public:
	/**
	 * \brief ���һ����������
	 * \param task ��������
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
	 * \brief ���캯��
	 * \param pool ���������ӳ�
	 * \param name �߳�����
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
	 * \brief ��������
	 *
	 */
	~zHttpThread()
	{
	    TEMP_FAILURE_RETRY(::close(kdpfd));
	}

	void run();

};

/**
 * \brief �ȴ�������ָ֤��,��������֤
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
			Zebra::logger->error("�׽ӿڴ���");
			remove(task);
		    }
		    else if(epfds[i].events & EPOLLIN)
		    {
			switch(task->httpCore())
			{
			    case 1://��֤�ɹ�
			    case -1: //��֤ʧ��
				remove(task);
				break;
			    case 0://��ʱ ����ᴦ��
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
		    //����ָ��ʱ����֤��û��ͨ��,��Ҫ��������
		    remove(it);
		}
	    }

	}
	m_Lock.unlock();

	zThread::msleep(50);
    }

    //�����еȴ���֤�����е����Ӽ��뵽���ն�����,������Щ����
    for(it = tasks.begin(), next = it, ++next; it != tasks.end(); it= next, ++next)
    {
	remove(it);
    }
}


/**
 * \brief ��һ��TCP������ӵ���֤������,��Ϊ���ڶ����֤����,��Ҫ����һ�����㷨��ӵ���ͬ����֤���������
 *
 * \param task һ����������
 */
bool zHttpTaskPool::addHttp(zHttpTask *task)
{

    Zebra::logger->debug("zHttpTaskPool::addHttp");
    //��Ϊ���ڶ����֤����,��Ҫ����һ�����㷨��ӵ���ͬ����֤���������
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
 * \brief ��ʼ���̳߳�,Ԥ�ȴ��������߳�
 *
 * \return ��ʼ���Ƿ�ɹ�
 */
bool zHttpTaskPool::init()
{
    Zebra::logger->debug("zHttpTaskPool::init");
    //������ʼ����֤�߳�
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
 * \brief �ͷ��̳߳�,�ͷŸ�����Դ,�ȴ������߳��˳�
 *
 */
void zHttpTaskPool::final()
{
    httpThreads.joinAll();
}

