#ifndef _zHttpTaskPool_h_
#define _zHttpTaskPool_h_
#include <string>
#include <vector>
#include <queue>
#include <unistd.h>
#include <sys/timeb.h>

#include "zSocket.h"
#include "zThread.h"
#include "zHttpTask.h"
#include "Zebra.h"
#include "zString.h"

class zHttpTask;

/**
* \brief �����̳߳��࣬��װ��һ���̴߳��������ӵ��̳߳ؿ��
*
*/
class zHttpTaskPool : private zNoncopyable
{

public:

	/**
	* \brief ���캯��
	*/
	zHttpTaskPool()
	{
	};

	/**
	* \brief ��������������һ���̳߳ض���
	*
	*/
	~zHttpTaskPool()
	{
		final();
	}

	bool addHttp(zHttpTask *task);
	bool init();
	void final();

private:

	static const int maxHttpThreads = 8;          /**< �����֤�߳����� */
	zThreadGroup httpThreads;                /**< ��֤�̣߳������ж�� */

};
#endif

