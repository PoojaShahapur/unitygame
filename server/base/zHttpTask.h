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
* \brief ����һ�������࣬���̳߳صĹ�����Ԫ
*
*/
class zHttpTask : public zProcessor,private zNoncopyable
{

public:


	/**
	* \brief ���캯�������ڴ���һ������
	*
	*
	* \param pool �������ӳ�ָ��
	* \param sock �׽ӿ�
	* \param addr ��ַ
	* \param compress �ײ����ݴ����Ƿ�֧��ѹ��
	* \param checkSignal �Ƿ���������·�����ź�
	*/
	zHttpTask(
		zHttpTaskPool *pool,
		const int sock,
		const struct sockaddr_in *addr = NULL) :pool(pool), lifeTime(), delflag(false),paddr(NULL)
	{
	    pSocket = new zSocket(sock, addr, false);
	}

	/**
	* \brief ������������������һ������
	*
	*/
	virtual ~zHttpTask() 
	{
	    SAFE_DELETE(pSocket);	
	}

	/**
	* \brief ���pollfd�ṹ
	* \param pfd �����Ľṹ
	* \param events �ȴ����¼�����
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
	* \brief ����Ƿ���֤��ʱ
	*
	*
	* \param ct ��ǰϵͳʱ��
	* \param interval ��ʱʱ�䣬����
	* \return ����Ƿ�ɹ�
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

	zSocket *pSocket;                /**< �ײ��׽ӿ� */
	
private:

	zHttpTaskPool *pool;                /**< ���������ĳ� */
	zRTime lifeTime;                /**< ���Ӵ���ʱ���¼ */
	
	bool delflag;
	zHttpTask **paddr;
};
#endif

