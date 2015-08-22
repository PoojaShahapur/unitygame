/**
* \brief zebra项目登陆服务器,负责登陆,建立帐号、档案等功能
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
* \brief 定义登陆服务类
*
* 登陆服务,负责登陆,建立帐号、档案等功能<br>
* 这个类使用了Singleton设计模式,保证了一个进程中只有一个类的实例
*
*/
class FLService : public Singleton<FLService>, public zMNetService
{

public:

	/**
	* \brief 获取连接池中的连接数
	* \return 连接数
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
	* \brief 获取服务器类型
	* \return 服务器类型
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

