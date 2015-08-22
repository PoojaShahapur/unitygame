#ifndef _zHttpTask_h_
#define _zHttpTask_h_

#include <string>
#include <vector>
#include <queue>
#include <list>
#include <unistd.h>
#include <sys/timeb.h>

#include "zSocket.h"
#include "zNoncopyable.h"
#include "zTime.h"
#include "zHttpTaskPool.h"

class zHttpTaskPool;
/**
* \brief 定义一个任务类，是线程池的工作单元
*
*/
class zHttpTask : public zProcessor,private zNoncopyable
{

public:


	/**
	* \brief 构造函数，用于创建一个对象
	*
	*
	* \param pool 所属连接池指针
	* \param sock 套接口
	* \param addr 地址
	* \param compress 底层数据传输是否支持压缩
	* \param checkSignal 是否发送网络链路测试信号
	*/
	zHttpTask(
		zHttpTaskPool *pool,
		const int sock,
		const struct sockaddr_in *addr = NULL) :pool(pool), lifeTime(), delflag(false),paddr(NULL)
	{
	    pSocket = new zSocket(sock, addr, false);
	}

	/**
	* \brief 析构函数，用于销毁一个对象
	*
	*/
	virtual ~zHttpTask() 
	{
	    SAFE_DELETE(pSocket);	
	}

	/**
	* \brief 填充pollfd结构
	* \param pfd 待填充的结构
	* \param events 等待的事件参数
	*/
	bool addEpoll(int kdpfd, __uint32_t events, void *ptr)
	{
	    if(pSocket)
		pSocket->addEpoll(kdpfd, events, ptr);
	    return true;
	}

	bool delEpoll(int kdpfd, __uint32_t events)
	{
	    if(pSocket)
		pSocket->delEpoll(kdpfd, events);
	    return true;
	}
	/**
	* \brief 检测是否验证超时
	*
	*
	* \param ct 当前系统时间
	* \param interval 超时时间，毫秒
	* \return 检测是否成功
	*/
	bool checkHttpTimeout(const zRTime &ct,const QWORD interval = 2000) const
	{
		return (lifeTime.elapse(ct) > interval);
	}


	virtual int httpCore()
	{
	    return 1;
	}

	virtual bool sendCmd(const void *pstrCmd,int nCmdLen);

	bool ifdel()
	{
	    return delflag;
	}

	void setflag(bool newflag)
	{
	    delflag = newflag;
	}

	zHttpTask** getaddr()
	{
	    return paddr;
	}
	
	void setaddr(zHttpTask **address)
	{
	    paddr = address;
	}

protected:

	zSocket *pSocket;                /**< 底层套接口 */
	
private:

	zHttpTaskPool *pool;                /**< 任务所属的池 */
	zRTime lifeTime;                /**< 连接创建时间记录 */
	
	bool delflag;
	zHttpTask **paddr;
};
#endif

