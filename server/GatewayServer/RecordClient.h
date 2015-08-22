#ifndef __RECORDCLIENT_H_
#define __RECORDCLIENT_H_


#include "zTCPClient.h"
#include "zMutex.h"
/**
* \brief 网关与档案服务器的连接
*
*/
class RecordClient : public zTCPBufferClient
{

public:

	/**
	* \brief 构造函数
	*
	* \param name 名称
	* \param ip 服务器地址
	* \param port 服务器端口
	*/
	RecordClient(
		const std::string &name,
		const std::string &ip,
		const WORD port)
		: zTCPBufferClient(name,ip,port) {};

	bool connectToRecordServer();
	void run();
	bool msgParse(const Cmd::t_NullCmd *pNullCmd,const DWORD nCmdLen);

};

extern RecordClient *recordClient;



#endif
