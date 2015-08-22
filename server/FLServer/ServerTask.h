/**
* \brief 服务器连接任务
*/
#ifndef _ServerTask_h_
#define _ServerTask_h_
#include "zTCPTaskPool.h"

class ServerTask : public zTCPTask
{

public:
	DWORD old;
	/**
	* \brief 构造函数
	* 用于创建一个服务器连接任务
	* \param pool 所属的连接池
	* \param sock TCP/IP套接口
	*/
	ServerTask(
		zTCPTaskPool *pool,
		const int sock) : zTCPTask(pool,sock,NULL)
	{
	}

	/**
	* \brief 虚析构函数
	*/
	~ServerTask() {
	};

	int verifyConn();
	int waitSync();
	bool uniqueAdd();
	bool uniqueRemove();
	bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

	/*void setZoneID(const GameZone_t &gameZone)
	{
	this->gameZone = gameZone;
	}*/

	const GameZone_t &getZoneID() const
	{
		return gameZone;
	}

private:

	GameZone_t gameZone;
	std::string name;

	bool msgParse_gyList(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);
	bool msgParse_session(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

};
#endif

