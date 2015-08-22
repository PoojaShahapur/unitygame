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

//extern MasterClient *masterClient;


/**
* \brief �����½������
*
* ��½����,�����½,�����ʺš������ȹ���<br>
* �����ʹ����Singleton���ģʽ,��֤��һ��������ֻ��һ�����ʵ��
*
*/
class FLService : public Singleton<FLService>, public zMNetService
{

public:

	/**
	* \brief ��ȡ���ӳ��е�������
	* \return ������
	*/
	const int getPoolSize() const
	{
		return loginTaskPool->getSize();
	}
	
	const int getMaxPoolSize() const
	{
	    return loginTaskPool->getMaxConns();
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
	static DBMetaData* metaData;

	friend class Singleton<FLService>;
	FLService();
	~FLService();
private:
	static zLogger *ulogger;
	bool init();
	void newTCPTask(const int sock,const WORD srcPort);
	void final();

	WORD login_port;
	WORD inside_port;
	WORD user_port;
	WORD ping_port;

	zTCPTaskPool *loginTaskPool;
	zTCPTaskPool *serverTaskPool;
	zTCPTaskPool *userTaskPool;
	//zTCPTaskPool *pingTaskPool;
};

#endif

