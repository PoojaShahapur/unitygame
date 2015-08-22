/**
* \brief zebra��Ŀ��½������,�����½,�����ʺš������ȹ���
*
*/
#ifndef _FLServer_h_
#define _FLServer_h_

#include "Zebra.h"
#include "zMisc.h"
#include "zMNetService.h"
#include "zTCPTaskPool.h"
#include "zDBConnPool.h"
#include "zMetaData.h"
#include "zSingleton.h"


/**
* \brief �����½������
*
* ��½����,�����½,�����ʺš������ȹ���<br>
* �����ʹ����Singleton���ģʽ,��֤��һ��������ֻ��һ�����ʵ��
*
*/
class roleRegService : public Singleton<roleRegService>, public zMNetService
{

public:

	/**
	* \brief ��ȡ���ӳ��е�������
	* \return ������
	*/
	const int getPoolSize() const
	{
		return taskPool->getSize();
	}
	
	const int getMaxPoolSize() const
	{
	    return taskPool->getMaxConns();
	}
	/**
	* \brief ��ȡ����������
	* \return ����������
	*/
	const WORD getType() const
	{
		return LOGINSERVER;
	}

	void reloadConfig();

	static zDBConnPool *dbConnPool;

	friend class Singleton<roleRegService>;
	roleRegService();
	~roleRegService();

private:
	static zLogger *ulogger;
	bool init();
	void newTCPTask(const int sock,const WORD srcPort);
	void final();

	WORD client_port;

	zTCPTaskPool *taskPool;
};

#endif

