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
* \brief �����̳߳��࣬��װ��һ���̴߳��������ӵ��̳߳ؿ��
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
	* \brief ���Ӽ���߳�
	*
	*/
	zCheckconnectThread *checkconnectThread;;
	/**
	* \brief ���ӵȴ�������Ϣ���߳�
	*
	*/
	zCheckwaitThread *checkwaitThread;;
	/**
	* \brief ���гɹ����Ӵ�������߳�
	*
	*/
	zThreadGroup taskThreads;

	/**
	* \brief ������������
	*
	*/
	//typedef std::list<zTCPClientTask *,__pool_alloc<zTCPClientTask *> > zTCPClientTaskContainer;
	typedef std::list<zTCPClientTask *> zTCPClientTaskContainer;


	/**
	* \brief �����������������
	*
	*/
	typedef zTCPClientTaskContainer::iterator zTCPClientTask_IT;

	zMutex mlock;          /**< ������� */
	zTCPClientTaskContainer tasks;  /**< �����б� */

public:
	unsigned long usleep_time;                                        /**< ѭ���ȴ�ʱ�� */
};

#endif

